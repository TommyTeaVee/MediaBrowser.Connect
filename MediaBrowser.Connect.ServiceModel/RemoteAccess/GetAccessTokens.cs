using System;
using System.Collections.Generic;
using ServiceStack;

namespace MediaBrowser.Connect.ServiceModel.RemoteAccess
{
    [Route("/servers/{ServerId}/tokens", "GET", Summary = "Gets a list of all access tokens for a Media Browser Server instance.")]
    public class GetAccessTokens : IReturn<IList<ServerAccessTokenDto>>
    {
        public string ServerId { get; set; }
    }
}