using ServiceStack;

namespace MediaBrowser.Connect.ServiceModel.Servers
{
    [Route("/servers", "POST", Summary = "Registers a Media Browser Server instance with the service.")]
    public class RegisterServerInstance : IReturn<ServerInstanceAuthInfoDto>
    {
        public string Url { get; set; }
    }
}