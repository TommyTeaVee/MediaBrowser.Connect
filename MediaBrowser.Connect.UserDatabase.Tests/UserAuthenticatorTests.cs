using System.Data;
using Moq;
using NUnit.Framework;
using ServiceStack;
using ServiceStack.OrmLite;

namespace MediaBrowser.Connect.UserDatabase.Tests
{
    [TestFixture]
    public class UserAuthenticatorTests
    {
        [Test]
        public void AuthenticateFailsWithIncorrectPassword()
        {
            var connection = new OrmLiteConnectionFactory(":memory:", SqliteDialect.Provider);
            var authenticator = new UserAuthenticator(connection);

            using (IDbConnection db = connection.Open()) {
                string salt = UserProvider.GenerateSalt();
                db.Insert(new UserAuthData {Username = "jsmith", Salt = salt, Password = UserAuthenticator.CalculateHashedPassword("password", salt)});
            }

            var service = new Mock<IServiceBase>();

            bool isAuthenticated = authenticator.TryAuthenticate(service.Object, "jsmith", "wrong_password");

            Assert.That(isAuthenticated, Is.False);
        }

        [Test]
        public void AuthenticateFalseWhenUserDoesNotExist()
        {
            var connection = new OrmLiteConnectionFactory(":memory:", SqliteDialect.Provider);
            var authenticator = new UserAuthenticator(connection);

            var service = new Mock<IServiceBase>();

            bool isAuthenticated = authenticator.TryAuthenticate(service.Object, "jsmith", "password");

            Assert.That(isAuthenticated, Is.False);
        }

        [Test]
        public void AuthenticateSucceedsWithCorrectLoginWithEmail()
        {
            var connection = new OrmLiteConnectionFactory(":memory:", SqliteDialect.Provider);
            var authenticator = new UserAuthenticator(connection);

            using (IDbConnection db = connection.Open()) {
                string salt = UserProvider.GenerateSalt();
                db.Insert(new UserAuthData {Username = "jsmith", Email = "jsmith@company.com", Salt = salt, Password = UserAuthenticator.CalculateHashedPassword("password", salt)});
            }

            var service = new Mock<IServiceBase>();

            bool isAuthenticated = authenticator.TryAuthenticate(service.Object, "jsmith@company.com", "password");

            Assert.That(isAuthenticated, Is.True);
        }

        [Test]
        public void AuthenticateSucceedsWithCorrectLoginWithUsername()
        {
            var connection = new OrmLiteConnectionFactory(":memory:", SqliteDialect.Provider);
            var authenticator = new UserAuthenticator(connection);

            using (IDbConnection db = connection.Open()) {
                string salt = UserProvider.GenerateSalt();
                db.Insert(new UserAuthData {Username = "jsmith", Email = "jsmith@company.com", Salt = salt, Password = UserAuthenticator.CalculateHashedPassword("password", salt)});
            }

            var service = new Mock<IServiceBase>();

            bool isAuthenticated = authenticator.TryAuthenticate(service.Object, "jsmith", "password");

            Assert.That(isAuthenticated, Is.True);
        }
    }
}