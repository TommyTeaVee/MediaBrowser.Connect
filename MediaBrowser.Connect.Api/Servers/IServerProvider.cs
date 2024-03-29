﻿using System.Collections.Generic;
using MediaBrowser.Connect.ServiceModel.RemoteAccess;
using MediaBrowser.Connect.ServiceModel.Servers;

namespace MediaBrowser.Connect.Interfaces.Servers
{
    public interface IServerProvider
    {
        ServerInstanceAuthInfoDto RegisterServerInstance(string name, string url);
        ServerInstanceDto UpdateServerInstance(string serverId, string name, string url);
        ServerAccessTokenDto RegisterServerAccessToken(string serverId, int userId, string accessToken, UserType type);
        void RevokeServerAccessToken(string serverId, int userId);
        IEnumerable<ServerAccessTokenDto> GetServerAccessTokens(string serverId);
        IEnumerable<ServerAccessTokenDto> GetUsersServerAccessTokens(int userId);
    }
}