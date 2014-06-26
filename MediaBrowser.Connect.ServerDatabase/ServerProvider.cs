using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediaBrowser.Connect.Interfaces.Servers;
using MediaBrowser.Connect.ServiceModel.RemoteAccess;
using MediaBrowser.Connect.ServiceModel.Servers;
using ServiceStack.Data;
using ServiceStack.OrmLite;

namespace MediaBrowser.Connect.ServerDatabase
{
    public class ServerProvider
        : IServerProvider
    {
        private IDbConnectionFactory _connectionFactory;

        public ServerProvider(ServerDatabase database)
        {
            _connectionFactory = database.Connection;

            using (var db = _connectionFactory.Open()) {
                db.CreateTableIfNotExists<ServerData>();
            }
        }

        public ServerInstanceAuthInfoDto RegisterServerInstance(string name, string ipAddress)
        {
            using (var db = _connectionFactory.Open()) {
                string id = GenerateGuidString();
                string salt = GenerateGuidString();

                var serverData = new ServerData {
                    
                }
            }
        }

        private string GenerateGuidString()
        {
            return new Guid().ToString();
        }

        private string HashPassword(string password, string salt)
        {
            
        }

        public ServerInstanceDto UpdateServerInstance(string serverId, string name, string ipAddress)
        {
            throw new NotImplementedException();
        }

        public ServerAccessTokenDto RegisterServerAccessToken(string serverId, int userId, string accessToken)
        {
            throw new NotImplementedException();
        }

        public void RevokeServerAccessToken(string serverId, int userId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ServerAccessTokenDto> GetServerAccessTokens(string serverId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ServerAccessTokenDto> GetUsersServerAccessTokens(int userId)
        {
            throw new NotImplementedException();
        }
    }
}
