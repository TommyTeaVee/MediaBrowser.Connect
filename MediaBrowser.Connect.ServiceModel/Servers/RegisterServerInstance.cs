using ServiceStack;

namespace MediaBrowser.Connect.ServiceModel.Servers
{
    [Route("/servers", "POST", Summary = "Registers a Media Browser Server instance with the service.")]
    public class RegisterServerInstance : IReturn<ServerInstanceDto>
    {
        public string IpAddress { get; set; }
        public string Password { get; set; }
    }
}