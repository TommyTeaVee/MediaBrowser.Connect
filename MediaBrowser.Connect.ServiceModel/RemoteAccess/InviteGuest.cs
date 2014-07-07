using ServiceStack;

namespace MediaBrowser.Connect.ServiceModel.RemoteAccess
{
    [Route("/servers/{ServerId}/guests", "POST", Summary = "Invites a guest to access a Media Browser Server instance.")]
    public class InviteGuest : IReturn<ServerAccessTokenDto>
    {
        [ApiMember(Description = "The MBS instance ID", DataType = "string", IsRequired = true, ParameterType = "path")]
        public string ServerId { get; set; }

        [ApiMember(Description = "The user's login username or email address", IsRequired = false, ParameterType = "query")]
        public string Username { get; set; }

        [ApiMember(Description = "The user's ID", IsRequired = false, ParameterType = "query")]
        public string UserId { get; set; }

        [ApiMember(Description = "The access key to grant", DataType = "string", IsRequired = true, ParameterType = "query")]
        public string AccessKey { get; set; }
    }
}