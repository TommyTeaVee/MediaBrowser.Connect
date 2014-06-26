using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediaBrowser.Connect.ServiceModel.RemoteAccess;
using MediaBrowser.Connect.ServiceModel.Servers;

namespace MediaBrowser.Connect.Interfaces.Servers
{
    public interface IServerProvider
    {
        ServerInstanceAuthInfoDto RegisterServerInstance(string name, string ipAddress);
        ServerInstanceDto UpdateServerInstance(string serverId, string name, string ipAddress);
        ServerAccessTokenDto RegisterServerAccessToken(string serverId, int userId, string accessToken);
        void RevokeServerAccessToken(string serverId, int userId);
        IEnumerable<ServerAccessTokenDto> GetServerAccessTokens(string serverId);
        IEnumerable<ServerAccessTokenDto> GetUsersServerAccessTokens(int userId);
    }
}
