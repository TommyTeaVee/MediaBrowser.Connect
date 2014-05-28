using System;

namespace MediaBrowser.Connect.ServiceModel.RemoteAccess
{
    public class ServerAccessTokenDto
    {
        public Guid ServerId { get; set; }
        public Guid UserId { get; set; }
        public string AccessToken { get; set; }
        public string ServerIpAddress { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}