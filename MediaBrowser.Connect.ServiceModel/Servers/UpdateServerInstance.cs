using ServiceStack;

namespace MediaBrowser.Connect.ServiceModel.Servers
{
    [Route("/servers/{ServerId}", "POST", Summary = "Updates the connection settings for a Media Browser Server instance.")]
    public class UpdateServerInstance
    {
        [ApiMember(Description = "The server ID", DataType = "string", IsRequired = true, ParameterType = "path")]
        public string ServerId { get; set; }

        [ApiMember(Description = "The server URL", DataType = "string", IsRequired = true, ParameterType = "query")]
        public string Url { get; set; }

        [ApiMember(Description = "The server name", DataType = "string", IsRequired = true, ParameterType = "query")]
        public string Name { get; set; }
    }
}