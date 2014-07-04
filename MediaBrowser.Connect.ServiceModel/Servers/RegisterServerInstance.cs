using ServiceStack;

namespace MediaBrowser.Connect.ServiceModel.Servers
{
    [Route("/servers", "POST", Summary = "Registers a Media Browser Server instance with the service.")]
    public class RegisterServerInstance : IReturn<ServerInstanceAuthInfoDto>
    {
        [ApiMember(Description = "The server URL", DataType="string", IsRequired=true, ParameterType="query")]
        public string Url { get; set; }

        [ApiMember(Description = "The server Name", DataType = "string", IsRequired = true, ParameterType = "query")]
        public string Name { get; set; }
    }
}