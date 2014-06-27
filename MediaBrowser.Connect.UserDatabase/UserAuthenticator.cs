using MediaBrowser.Connect.Interfaces;
using MediaBrowser.Connect.Interfaces.Auth;
using ServiceStack;
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

            using (var db = _connectionFactory.Open()) {
                db.CreateTableIfNotExists<UserAuthData>();
            }
        }

        public bool TryAuthenticate(IServiceBase authService, string username, string password)
        {
            using (var db = _connectionFactory.Open()) {
                UserAuthData user;

                if (IsUsernameEmailAddress(username)) {
                    user = db.SingleWhere<UserAuthData>("Email", username);
                } else {
                    user = db.SingleWhere<UserAuthData>("Username", username);
                }

                if (user == null) {
                    return false;
                }

                var hashedPassword = CalculateHashedPassword(password, user.Salt);
                return user.Password == hashedPassword;
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
