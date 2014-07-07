using System.Data;
using MediaBrowser.Connect.Interfaces.Servers;
using ServiceStack.Data;
using ServiceStack.OrmLite;

namespace MediaBrowser.Connect.ServerDatabase
{
    public class ServerAuthenticator : IServerAuthenticator
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public ServerAuthenticator(ServerDatabase database)
        {
            _connectionFactory = database.Connection;

            using (IDbConnection db = _connectionFactory.Open()) {
                db.CreateTableIfNotExists<ServerData>();
            }
        }

        public bool TryAuthenticate(string serverId, string accessKey)
        {
            using (IDbConnection db = _connectionFactory.Open()) {
                var server = db.SingleById<ServerData>(serverId);
                if (server == null) {
                    return false;
                }

                string hashedKey = ServerProvider.CalculateHashedKey(accessKey, server.Salt);
                return hashedKey == server.AccessKey;
            }
        }
    }
}