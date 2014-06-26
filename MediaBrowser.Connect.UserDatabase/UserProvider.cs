using System;
using System.Data;
using MediaBrowser.Connect.Interfaces.Users;
using MediaBrowser.Connect.ServiceModel.Users;
using Mono.Data.Sqlite;
using ServiceStack;
using ServiceStack.Data;
using ServiceStack.OrmLite;

namespace MediaBrowser.Connect.UserDatabase
{
    public class UserProvider : IUserProvider
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public UserProvider(UserDatabase database)
        {
            _connectionFactory = database.Connection;

            using (IDbConnection db = _connectionFactory.Open()) {
                db.CreateTableIfNotExists<UserAuthData>();
                db.CreateTableIfNotExists<UserProfileData>();
            }
        }

        public UserDto CreateUser(UserDto user, string password)
        {
            try {
                user.Id = 0;

                using (IDbConnection db = _connectionFactory.Open()) {
                    string salt = GenerateSalt();
                    var authData = new UserAuthData {
                        Username = user.ForumUsername,
                        Email = user.Email,
                        Salt = salt,
                        Password = UserAuthenticator.CalculateHashedPassword(password, salt)
                    };

                    authData.Id = (int) db.Insert(authData, true);

                    var profileData = new UserProfileData {
                        Id = authData.Id,
                        CreatedAt = DateTime.UtcNow,
                        DisplayName = user.DisplayName ?? user.ForumDisplayName ?? user.ForumUsername,
                        ForumDisplayName = user.ForumDisplayName ?? user.DisplayName ?? user.ForumUsername
                    };

                    db.Insert(profileData);

                    return CreateDto(authData, profileData);
                }
            } catch (SqliteException e) {
                if (e.Message.Contains("constraint violation")) {
                    if (e.Message.Contains("Username")) {
                        throw HttpError.Conflict("A user with that username already exists");
                    }
                    if (e.Message.Contains("Email")) {
                        throw HttpError.Conflict("A user with that email address already exists");
                    }
                }

                throw;
            }
        }

        public UserDto GetUser(int userId)
        {
            using (IDbConnection db = _connectionFactory.Open()) {
                // could probably be done in a single query..
                var auth = db.SingleById<UserAuthData>(userId);
                var profile = db.SingleWhere<UserProfileData>("Id", userId);

                if (auth == null || profile == null) {
                    throw HttpError.NotFound(string.Format("User with ID {0} not found", userId));
                }

                return CreateDto(auth, profile);
            }
        }

        public UserDto UpdateUser(UserDto user, string password)
        {
            if (!string.IsNullOrEmpty(user.Email) || !string.IsNullOrEmpty(user.ForumUsername) || !string.IsNullOrEmpty(password)) {
                UpdateAuthData(user, password);
            }

            if (!string.IsNullOrEmpty(user.DisplayName) || !string.IsNullOrEmpty(user.ForumDisplayName)) {
                UpdateProfileData(user);
            }

            return GetUser(user.Id);
        }

        public static string GenerateSalt()
        {
            return new Guid().ToString();
        }

        private void UpdateProfileData(UserDto user)
        {
            bool hasDisplayName = !string.IsNullOrEmpty(user.DisplayName);
            bool hasForumDisplayName = !string.IsNullOrEmpty(user.ForumDisplayName);

            var profileData = new UserProfileData {Id = user.Id, DisplayName = user.DisplayName, ForumDisplayName = user.ForumDisplayName};

            using (IDbConnection db = _connectionFactory.Open()) {
                if (hasDisplayName && hasForumDisplayName) {
                    db.UpdateOnly(profileData, u => new {u.DisplayName, u.ForumDisplayName});
                }
                if (hasDisplayName && !hasForumDisplayName) {
                    db.UpdateOnly(profileData, u => u.DisplayName);
                }
                if (!hasDisplayName && hasForumDisplayName) {
                    db.UpdateOnly(profileData, u => u.ForumDisplayName);
                }
            }
        }

        private void UpdateAuthData(UserDto user, string password)
        {
            bool hasEmail = !string.IsNullOrEmpty(user.Email);
            bool hasUsername = !string.IsNullOrEmpty(user.ForumUsername);
            bool hasPassword = !string.IsNullOrEmpty(password);

            var authData = new UserAuthData {
                Id = user.Id,
                Email = user.Email,
                Username = user.ForumUsername
            };

            using (IDbConnection db = _connectionFactory.Open()) {
                if (hasEmail && hasUsername) {
                    db.UpdateOnly(authData, u => new {u.Email, u.Username});
                }
                if (hasEmail && !hasUsername) {
                    db.UpdateOnly(authData, u => u.Email);
                }
                if (!hasEmail && hasUsername) {
                    db.UpdateOnly(authData, u => u.Username);
                }

                if (hasPassword) {
                    var currentAuthData = db.SingleById<UserAuthData>(user.Id);
                    var hashedPassword = UserAuthenticator.CalculateHashedPassword(password, currentAuthData.Salt);

                    currentAuthData.Password = hashedPassword;
                    db.UpdateOnly(currentAuthData, u => u.Password);
                }
            }
        }

        private UserDto CreateDto(UserAuthData auth, UserProfileData profile)
        {
            return new UserDto {
                Id = auth.Id,
                Email = auth.Email,
                ForumUsername = auth.Username,
                DisplayName = profile.DisplayName,
                ForumDisplayName = profile.ForumDisplayName
            };
        }
    }
}