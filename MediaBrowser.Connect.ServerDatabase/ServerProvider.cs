using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using MediaBrowser.Connect.Interfaces;
using MediaBrowser.Connect.Interfaces.Servers;
using MediaBrowser.Connect.ServiceModel.RemoteAccess;
using MediaBrowser.Connect.ServiceModel.Servers;
using ServiceStack;
using ServiceStack.Data;
using ServiceStack.OrmLite;

namespace MediaBrowser.Connect.ServerDatabase
{
    public class ServerProvider
        : IServerProvider
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public ServerProvider(ServerDatabase database)
        {
            _connectionFactory = database.Connection;

            using (IDbConnection db = _connectionFactory.Open()) {
                db.CreateTableIfNotExists<ServerData>();
                db.CreateTableIfNotExists<ServerAccessTokenData>();
            }
        }

        public ServerInstanceAuthInfoDto RegisterServerInstance(string name, string url)
        {
            using (IDbConnection db = _connectionFactory.Open()) {
                string id = GenerateGuidString();
                string salt = GenerateGuidString();
                string key = GenerateGuidString();

                var serverData = new ServerData {
                    Id = id,
                    Name = name,
                    Url = url,
                    Salt = salt,
                    AccessKey = CalculateHashedKey(key, salt)
                };

                db.Insert(serverData);

                return new ServerInstanceAuthInfoDto {
                    Id = id,
                    Name = name,
                    Url = url,
                    AccessKey = key
                };
            }
        }

        public ServerInstanceDto UpdateServerInstance(string serverId, string name, string url)
        {
            using (IDbConnection db = _connectionFactory.Open()) {
                db.UpdateOnly(new ServerData {Name = name, Url = url}, s => new {s.Name, IpAddress = s.Url}, s => s.Id == serverId);
            }

            return new ServerInstanceDto {
                Id = serverId,
                Name = name,
                Url = url
            };
        }

        public ServerAccessTokenDto RegisterServerAccessToken(string serverId, int userId, string accessToken, UserType type)
        {
            using (IDbConnection db = _connectionFactory.Open()) {
                var serverData = db.SingleById<ServerData>(serverId);

                if (serverData == null) {
                    throw HttpError.NotFound("Unrecognized server ID");
                }

                var tokenData = new ServerAccessTokenData {
                    ServerId = serverId,
                    UserId = userId,
                    AccessToken = accessToken,
                    CreatedAt = DateTime.UtcNow,
                    UserType = type,
                    IsActive = true
                };

                db.Save(tokenData);

                var dto = tokenData.ConvertTo<ServerAccessTokenDto>();
                dto.ServerUrl = serverData.Url;

                return dto;
            }
        }

        public void RevokeServerAccessToken(string serverId, int userId)
        {
            using (IDbConnection db = _connectionFactory.Open()) {
                var tokenData = new ServerAccessTokenData {
                    ServerId = serverId,
                    UserId = userId
                };

                db.DeleteById<ServerAccessTokenData>(tokenData.Id);
            }
        }

        public IEnumerable<ServerAccessTokenDto> GetServerAccessTokens(string serverId)
        {
            using (IDbConnection db = _connectionFactory.Open()) {
                var serverData = db.SingleById<ServerData>(serverId);
                List<ServerAccessTokenData> tokens = db.Where<ServerAccessTokenData>(new {ServerId = serverId});

                return tokens.Select(t => {
                    var dto = t.ConvertTo<ServerAccessTokenDto>();
                    dto.ServerUrl = serverData.Url;

                    return dto;
                });
            }
        }

        public IEnumerable<ServerAccessTokenDto> GetUsersServerAccessTokens(int userId)
        {
            using (IDbConnection db = _connectionFactory.Open()) {
                var jn = new JoinSqlBuilder<ServerAccessTokenDataEx, ServerAccessTokenData>();

                jn.Join<ServerAccessTokenData, ServerData>(t => t.ServerId, s => s.Id, t => new {t.UserId, t.AccessToken, t.CreatedAt}, s => new {s.Id, s.Url})
                  .Where<ServerAccessTokenData>(t => t.UserId == userId);

                string sql = jn.ToSql();
                List<ServerAccessTokenDataEx> records = db.Select<ServerAccessTokenDataEx>(sql);

                return records.Select(t => t.ConvertTo<ServerAccessTokenDto>());
            }
        }

        public static string CalculateHashedKey(string key, string salt)
        {
            return (salt.Md5() + key.Md5()).Md5();
        }

        private string GenerateGuidString()
        {
            return Guid.NewGuid().ToString();
        }
    }
}