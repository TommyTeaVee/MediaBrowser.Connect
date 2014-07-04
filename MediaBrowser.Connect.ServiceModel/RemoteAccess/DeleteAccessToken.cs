using System;
using ServiceStack;

namespace MediaBrowser.Connect.ServiceModel.RemoteAccess
{
    [Route("/servers/{ServerId}/users/{UserId}", "DELETE", Summary = "Revokes an access token for a specified user.")]
    public class DeleteAccessToken
    {
        [ApiMember(Description = "The MBS instance ID", DataType = "string", IsRequired = true, ParameterType = "path")]
        public string ServerId { get; set; }

        [ApiMember(Description = "The user ID", DataType = "number", IsRequired = true, ParameterType = "path")]
        public int UserId { get; set; }
    }
}