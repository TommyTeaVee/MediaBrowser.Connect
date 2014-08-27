using System.Collections.Generic;
using System.Linq;
using MediaBrowser.Connect.ServiceModel.RemoteAccess;
using MediaBrowser.Connect.ServiceModel.Servers;
using NUnit.Framework;
using ServiceStack;
using ServiceStack.OrmLite;

namespace MediaBrowser.Connect.ServerDatabase.Tests
{
    [TestFixture]
    public class ServerProviderTests
    {
        [Test]
        public void GetAccessTokensForServer()
        {
            var db = new ServerDatabase {Connection = new OrmLiteConnectionFactory(":memory:", SqliteDialect.Provider)};
            var provider = new ServerProvider(db);

            ServerInstanceAuthInfoDto serverInfo = provider.RegisterServerInstance("test", "test.com:8096/mediabrowser/");
            ServerAccessTokenDto token1 = provider.RegisterServerAccessToken(serverInfo.Id, 1, "ihaveaccess", UserType.LinkedAccount);
            ServerAccessTokenDto token2 = provider.RegisterServerAccessToken(serverInfo.Id, 2, "ihaveotheraccess", UserType.LinkedAccount);

            List<ServerAccessTokenDto> tokens = provider.GetServerAccessTokens(serverInfo.Id).ToList();


            Assert.That(tokens.Count, Is.EqualTo(2));

            Assert.That(tokens[0].ServerId, Is.EqualTo(serverInfo.Id));
            Assert.That(tokens[0].UserId, Is.EqualTo(1));
            Assert.That(tokens[0].ServerUrl, Is.EqualTo(serverInfo.Url));
            Assert.That(tokens[0].AccessToken, Is.EqualTo("ihaveaccess"));

            Assert.That(tokens[1].ServerId, Is.EqualTo(serverInfo.Id));
            Assert.That(tokens[1].UserId, Is.EqualTo(2));
            Assert.That(tokens[1].ServerUrl, Is.EqualTo(serverInfo.Url));
            Assert.That(tokens[1].AccessToken, Is.EqualTo("ihaveotheraccess"));
        }

        [Test]
        public void GetAccessTokensForUser()
        {
            var db = new ServerDatabase {Connection = new OrmLiteConnectionFactory(":memory:", SqliteDialect.Provider)};
            var provider = new ServerProvider(db);

            ServerInstanceAuthInfoDto serverInfo1 = provider.RegisterServerInstance("test", "test.com:8096/mediabrowser/");
            ServerInstanceAuthInfoDto serverInfo2 = provider.RegisterServerInstance("test2", "test2.com:8096/mediabrowser/");

            ServerAccessTokenDto token1 = provider.RegisterServerAccessToken(serverInfo1.Id, 1, "ihaveaccess", UserType.LinkedAccount);
            ServerAccessTokenDto token2 = provider.RegisterServerAccessToken(serverInfo2.Id, 1, "ihaveotheraccess", UserType.LinkedAccount);
            ServerAccessTokenDto token3 = provider.RegisterServerAccessToken(serverInfo2.Id, 2, "ihaveotheraccess", UserType.LinkedAccount);

            List<ServerAccessTokenDto> tokens = provider.GetUsersServerAccessTokens(1).ToList();


            Assert.That(tokens.Count, Is.EqualTo(2));

            Assert.That(tokens[0].ServerId, Is.EqualTo(serverInfo1.Id));
            Assert.That(tokens[0].UserId, Is.EqualTo(1));
            Assert.That(tokens[0].ServerUrl, Is.EqualTo(serverInfo1.Url));
            Assert.That(tokens[0].AccessToken, Is.EqualTo("ihaveaccess"));

            Assert.That(tokens[1].ServerId, Is.EqualTo(serverInfo2.Id));
            Assert.That(tokens[1].UserId, Is.EqualTo(1));
            Assert.That(tokens[1].ServerUrl, Is.EqualTo(serverInfo2.Url));
            Assert.That(tokens[1].AccessToken, Is.EqualTo("ihaveotheraccess"));
        }

        [Test]
        public void ModifyAccessToken()
        {
            var db = new ServerDatabase {Connection = new OrmLiteConnectionFactory(":memory:", SqliteDialect.Provider)};
            var provider = new ServerProvider(db);

            ServerInstanceAuthInfoDto serverInfo = provider.RegisterServerInstance("test", "test.com:8096/mediabrowser/");
            ServerAccessTokenDto tokenInfo = provider.RegisterServerAccessToken(serverInfo.Id, 1, "ihaveaccess", UserType.LinkedAccount);
            ServerAccessTokenDto updatedTokenInfo = provider.RegisterServerAccessToken(serverInfo.Id, 1, "ihaveotheraccess", UserType.LinkedAccount);

            Assert.That(updatedTokenInfo.ServerId, Is.EqualTo(serverInfo.Id));
            Assert.That(updatedTokenInfo.UserId, Is.EqualTo(1));
            Assert.That(updatedTokenInfo.ServerUrl, Is.EqualTo(serverInfo.Url));
            Assert.That(updatedTokenInfo.AccessToken, Is.EqualTo("ihaveotheraccess"));
        }

        [Test]
        public void RegisterAccessToken()
        {
            var db = new ServerDatabase {Connection = new OrmLiteConnectionFactory(":memory:", SqliteDialect.Provider)};
            var provider = new ServerProvider(db);

            ServerInstanceAuthInfoDto serverInfo = provider.RegisterServerInstance("test", "test.com:8096/mediabrowser/");
            ServerAccessTokenDto tokenInfo = provider.RegisterServerAccessToken(serverInfo.Id, 1, "ihaveaccess", UserType.LinkedAccount);

            Assert.That(tokenInfo.ServerId, Is.EqualTo(serverInfo.Id));
            Assert.That(tokenInfo.UserId, Is.EqualTo(1));
            Assert.That(tokenInfo.ServerUrl, Is.EqualTo(serverInfo.Url));
            Assert.That(tokenInfo.AccessToken, Is.EqualTo("ihaveaccess"));
        }

        [Test]
        [ExpectedException(typeof (HttpError), ExpectedMessage = "Unrecognized server ID")]
        public void RegisterAccessTokenThrowsOnInvalidServerId()
        {
            var db = new ServerDatabase {Connection = new OrmLiteConnectionFactory(":memory:", SqliteDialect.Provider)};
            var provider = new ServerProvider(db);

            provider.RegisterServerAccessToken("invalid server id", 1, "ihaveaccess", UserType.LinkedAccount);
        }

        [Test]
        public void RegisterServer()
        {
            var db = new ServerDatabase {Connection = new OrmLiteConnectionFactory(":memory:", SqliteDialect.Provider)};
            var provider = new ServerProvider(db);

            ServerInstanceAuthInfoDto info = provider.RegisterServerInstance("test", "test.com:8096/mediabrowser/");

            Assert.That(info, Is.Not.Null);
            Assert.That(info.Name, Is.EqualTo("test"));
            Assert.That(info.Url, Is.EqualTo("test.com:8096/mediabrowser/"));
            Assert.That(info.Id, Is.Not.Empty);
            Assert.That(info.AccessKey, Is.Not.Empty);
        }

        [Test]
        public void RevokeAccessToken()
        {
            var db = new ServerDatabase {Connection = new OrmLiteConnectionFactory(":memory:", SqliteDialect.Provider)};
            var provider = new ServerProvider(db);

            ServerInstanceAuthInfoDto serverInfo = provider.RegisterServerInstance("test", "test.com:8096/mediabrowser/");
            ServerAccessTokenDto tokenInfo = provider.RegisterServerAccessToken(serverInfo.Id, 1, "ihaveaccess", UserType.LinkedAccount);

            List<ServerAccessTokenDto> tokens = provider.GetServerAccessTokens(serverInfo.Id).ToList();

            Assert.That(tokens.Count, Is.EqualTo(1));

            provider.RevokeServerAccessToken(serverInfo.Id, 1);

            tokens = provider.GetServerAccessTokens(serverInfo.Id).ToList();

            Assert.That(tokens, Is.Empty);
        }

        [Test]
        public void UpdateServer()
        {
            var db = new ServerDatabase {Connection = new OrmLiteConnectionFactory(":memory:", SqliteDialect.Provider)};
            var provider = new ServerProvider(db);

            ServerInstanceAuthInfoDto info = provider.RegisterServerInstance("test", "test.com:8096/mediabrowser/");
            ServerInstanceDto updated = provider.UpdateServerInstance(info.Id, "newname", "newurl.com:8096/mediabrowser/");

            Assert.That(updated, Is.Not.Null);
            Assert.That(updated.Name, Is.EqualTo("newname"));
            Assert.That(updated.Url, Is.EqualTo("newurl.com:8096/mediabrowser/"));
            Assert.That(updated.Id, Is.EqualTo(info.Id));
        }
    }
}