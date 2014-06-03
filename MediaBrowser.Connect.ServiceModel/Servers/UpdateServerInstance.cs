using System;
using ServiceStack;

namespace MediaBrowser.Connect.ServiceModel.Servers
{
    [Route("/servers/{ServerId}", "POST", Summary = "Updates the connection settings for a Media Browser Server instance.")]
    public class UpdateServerInstance
    {
        public string ServerId { get; set; }
        public string IpAddress { get; set; }
    }
}