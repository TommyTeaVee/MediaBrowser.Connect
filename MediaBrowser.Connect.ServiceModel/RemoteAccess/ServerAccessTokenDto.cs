using System;

namespace MediaBrowser.Connect.ServiceModel.RemoteAccess
{
    public class ServerAccessTokenDto
    {
        public string ServerId { get; set; }
        public int UserId { get; set; }
        public string AccessToken { get; set; }
        public string ServerUrl { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}