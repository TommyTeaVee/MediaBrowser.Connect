﻿using MediaBrowser.Connect.ServiceModel.Users;
using Moq;
using NUnit.Framework;
using ServiceStack;
using ServiceStack.OrmLite;

namespace MediaBrowser.Connect.UserDatabase.Tests
{
    [TestFixture]
    public class UserProviderTests
    {
        [Test]
        public void CanLoginAfterCreatingUser()
        {
            var connection = new OrmLiteConnectionFactory(":memory:", SqliteDialect.Provider);
            var provider = new UserProvider(connection);

            var user = new UserDto {
                Email = "jsmith@company.com",
                ForumUsername = "jsmith",
                ForumDisplayName = "John Smith",
                DisplayName = "John"
            };

            UserDto result = provider.CreateUser(user, "password");

            var service = new Mock<IServiceBase>();
            var authenticator = new UserAuthenticator(connection);

            bool isAuthenticated = authenticator.TryAuthenticate(service.Object, result.ForumUsername, "password");

            Assert.That(isAuthenticated, Is.True);
        }

        [Test]
        public void CreateUser()
        {
            var connection = new OrmLiteConnectionFactory(":memory:", SqliteDialect.Provider);
            var provider = new UserProvider(connection);

            var user = new UserDto {
                Email = "jsmith@company.com",
                ForumUsername = "jsmith",
                ForumDisplayName = "John Smith",
                DisplayName = "John"
            };

            UserDto result = provider.CreateUser(user, "password");

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(1));
            Assert.That(result.Email, Is.EqualTo(user.Email));
            Assert.That(result.ForumUsername, Is.EqualTo(user.ForumUsername));
            Assert.That(result.ForumDisplayName, Is.EqualTo(user.ForumDisplayName));
            Assert.That(result.DisplayName, Is.EqualTo(user.DisplayName));
        }

        [Test]
        [ExpectedException(typeof (HttpError), ExpectedMessage = "A user with that email address already exists")]
        public void CreateUserFailsOnDuplicateEmail()
        {
            var connection = new OrmLiteConnectionFactory(":memory:", SqliteDialect.Provider);
            var provider = new UserProvider(connection);

            var user = new UserDto {
                Email = "jsmith@company.com",
                ForumUsername = "jsmith",
                ForumDisplayName = "John Smith",
                DisplayName = "John"
            };

            provider.CreateUser(user, "password");

            var user2 = new UserDto {
                Email = "jsmith@company.com",
                ForumUsername = "jsmith2",
                ForumDisplayName = "John Smith 2",
                DisplayName = "John 2"
            };

            provider.CreateUser(user2, "password");
        }

        [Test]
        [ExpectedException(typeof (HttpError), ExpectedMessage = "A user with that username already exists")]
        public void CreateUserFailsOnDuplicateUsername()
        {
            var connection = new OrmLiteConnectionFactory(":memory:", SqliteDialect.Provider);
            var provider = new UserProvider(connection);

            var user = new UserDto {
                Email = "jsmith@company.com",
                ForumUsername = "jsmith",
                ForumDisplayName = "John Smith",
                DisplayName = "John"
            };

            provider.CreateUser(user, "password");

            var user2 = new UserDto {
                Email = "jsmith2@company.com",
                ForumUsername = "jsmith",
                ForumDisplayName = "John Smith 2",
                DisplayName = "John 2"
            };

            provider.CreateUser(user2, "password");
        }

        [Test]
        [ExpectedException(typeof (HttpError), ExpectedMessage = "User with ID 30 not found")]
        public void GetUserFailsWithUnrecognizedId()
        {
            var connection = new OrmLiteConnectionFactory(":memory:", SqliteDialect.Provider);
            var provider = new UserProvider(connection);

            provider.GetUser(30);
        }

        [Test]
        public void GetUserSuccess()
        {
            var connection = new OrmLiteConnectionFactory(":memory:", SqliteDialect.Provider);
            var provider = new UserProvider(connection);

            var user = new UserDto {
                Email = "jsmith@company.com",
                ForumUsername = "jsmith",
                ForumDisplayName = "John Smith",
                DisplayName = "John"
            };

            UserDto created = provider.CreateUser(user, "password");

            UserDto result = provider.GetUser(created.Id);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(1));
            Assert.That(result.Email, Is.EqualTo(user.Email));
            Assert.That(result.ForumUsername, Is.EqualTo(user.ForumUsername));
            Assert.That(result.ForumDisplayName, Is.EqualTo(user.ForumDisplayName));
            Assert.That(result.DisplayName, Is.EqualTo(user.DisplayName));
        }

        [Test]
        public void UpdateUserProfileData() 
        {
            var connection = new OrmLiteConnectionFactory(":memory:", SqliteDialect.Provider);
            var provider = new UserProvider(connection);

            var user = new UserDto {
                Email = "jsmith@company.com",
                ForumUsername = "jsmith",
                ForumDisplayName = "John Smith",
                DisplayName = "John"
            };

            var created = provider.CreateUser(user, "password");

            var updates = new UserDto {
                Id = created.Id,
                ForumDisplayName = "Updated Forum Display Name",
                DisplayName = "Updated Display Name"
            };

            var result = provider.UpdateUser(updates);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ForumDisplayName, Is.EqualTo(updates.ForumDisplayName));
            Assert.That(result.DisplayName, Is.EqualTo(updates.DisplayName));
        }

        [Test]
        public void UpdateUserAuthData()
        {
            var connection = new OrmLiteConnectionFactory(":memory:", SqliteDialect.Provider);
            var provider = new UserProvider(connection);

            var user = new UserDto {
                Email = "jsmith@company.com",
                ForumUsername = "jsmith",
                ForumDisplayName = "John Smith",
                DisplayName = "John"
            };

            var created = provider.CreateUser(user, "password");

            var updates = new UserDto {
                Id = created.Id,
                ForumUsername = "jsmith_updated",
                Email = "jsmith_updated@company.com"
            };

            var result = provider.UpdateUser(updates);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ForumUsername, Is.EqualTo(updates.ForumUsername));
            Assert.That(result.Email, Is.EqualTo(updates.Email));
        }

        [Test]
        public void LoginWithUpdatedEmail()
        {
            var connection = new OrmLiteConnectionFactory(":memory:", SqliteDialect.Provider);
            var provider = new UserProvider(connection);

            var user = new UserDto {
                Email = "jsmith@company.com",
                ForumUsername = "jsmith",
                ForumDisplayName = "John Smith",
                DisplayName = "John"
            };

            var created = provider.CreateUser(user, "password");

            var updates = new UserDto {
                Id = created.Id,
                ForumUsername = "jsmith_updated",
                Email = "jsmith_updated@company.com"
            };

            provider.UpdateUser(updates);

            var service = new Mock<IServiceBase>();
            var authenticator = new UserAuthenticator(connection);

            bool isAuthenticatedWithOldEmail = authenticator.TryAuthenticate(service.Object, user.Email, "password");
            bool isAuthenticatedWithNewEmail = authenticator.TryAuthenticate(service.Object, updates.Email, "password");

            Assert.That(isAuthenticatedWithOldEmail, Is.False);
            Assert.That(isAuthenticatedWithNewEmail, Is.True);
        }

        [Test]
        public void LoginWithUpdatedUsername()
        {
            var connection = new OrmLiteConnectionFactory(":memory:", SqliteDialect.Provider);
            var provider = new UserProvider(connection);

            var user = new UserDto {
                Email = "jsmith@company.com",
                ForumUsername = "jsmith",
                ForumDisplayName = "John Smith",
                DisplayName = "John"
            };

            var created = provider.CreateUser(user, "password");

            var updates = new UserDto {
                Id = created.Id,
                ForumUsername = "jsmith_updated",
                Email = "jsmith_updated@company.com"
            };

            provider.UpdateUser(updates);

            var service = new Mock<IServiceBase>();
            var authenticator = new UserAuthenticator(connection);

            bool isAuthenticatedWithOldUsername = authenticator.TryAuthenticate(service.Object, user.ForumUsername, "password");
            bool isAuthenticatedWithNewUsername = authenticator.TryAuthenticate(service.Object, updates.ForumUsername, "password");

            Assert.That(isAuthenticatedWithOldUsername, Is.False);
            Assert.That(isAuthenticatedWithNewUsername, Is.True);
        }
    }
}