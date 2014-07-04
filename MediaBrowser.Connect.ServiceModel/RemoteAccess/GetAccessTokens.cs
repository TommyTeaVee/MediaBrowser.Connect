using System.Collections.Generic;
using ServiceStack;

namespace MediaBrowser.Connect.ServiceModel.RemoteAccess
{
    [Route("/servers/{ServerId}/users", "GET", Summary = "Gets a list of all access tokens for a Media Browser Server instance.")]
    public class GetAccessTokens : IReturn<IList<ServerAccessTokenDto>>
    {
        [ApiMember(Description = "The MBS instance ID", DataType = "string", IsRequired = true, ParameterType = "path")]
        public string ServerId { get; set; }
    }
}