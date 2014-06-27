using ServiceStack;

namespace MediaBrowser.Connect.ServiceModel.RemoteAccess
{
    [Route("/servers/{ServerId}/tokens", "POST", Summary = "Grants an access token for an MB instance to a user")]
    public class CreateAccessToken : IReturn<ServerAccessTokenDto>
    {
        public string ServerId { get; set; }
        public int UserId { get; set; }
        public string AccessToken { get; set; }
    }
}