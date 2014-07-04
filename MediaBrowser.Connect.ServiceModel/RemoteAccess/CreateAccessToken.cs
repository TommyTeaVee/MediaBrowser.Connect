using ServiceStack;

namespace MediaBrowser.Connect.ServiceModel.RemoteAccess
{
    [Route("/servers/{ServerId}/users", "POST", Summary = "Grants an access token for an MB instance to a user")]
    public class CreateAccessToken : IReturn<ServerAccessTokenDto>
    {
        [ApiMember(Description = "The MBS instance ID", DataType = "string", IsRequired = true, ParameterType = "path")]
        public string ServerId { get; set; }

        [ApiMember(Description = "The access key to grant", DataType = "string", IsRequired = true, ParameterType = "query")]
        public string AccessKey { get; set; }
    }
}