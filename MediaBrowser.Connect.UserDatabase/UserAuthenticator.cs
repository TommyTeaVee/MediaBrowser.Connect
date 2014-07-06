using System.Data;
using System.Globalization;
using MediaBrowser.Connect.Interfaces;
using MediaBrowser.Connect.Interfaces.Auth;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Data;
using ServiceStack.OrmLite;

namespace MediaBrowser.Connect.UserDatabase
{
    public class UserAuthenticator : IUserAuthenticator
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public UserAuthenticator(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;

            using (IDbConnection db = _connectionFactory.Open()) {
                db.CreateTableIfNotExists<UserAuthData>();
            }
        }

        public bool TryAuthenticate(IServiceBase authService, string username, string password)
        {
            using (IDbConnection db = _connectionFactory.Open()) {
                UserAuthData user;

                if (IsUsernameEmailAddress(username)) {
                    user = db.SingleWhere<UserAuthData>("Email", username);
                } else {
                    user = db.SingleWhere<UserAuthData>("Username", username);
                }

                if (user == null) {
                    return false;
                }

                string hashedPassword = CalculateHashedPassword(password, user.Salt);
                if (user.Password != hashedPassword) {
                    return false;
                }

                IAuthSession session = authService.GetSession();
                session.UserAuthId = user.Id.ToString(CultureInfo.InvariantCulture);
                session.UserAuthName = session.UserName = user.Username;
                session.Email = user.Email;
                session.IsAuthenticated = true;

                var profile = db.SingleById<UserProfileData>(user.Id);
                if (profile != null) {
                    session.DisplayName = profile.DisplayName;
                }

                return true;
            }
        }

        public static string CalculateHashedPassword(string password, string salt)
        {
            // todo if we migrate the auth database from IP.Board, we need to ensure this matches their hashing code
            return (salt.Md5() + password.Md5()).Md5();
        }

        public static bool IsUsernameEmailAddress(string username)
        {
            return username.Contains("@");
        }
    }
}