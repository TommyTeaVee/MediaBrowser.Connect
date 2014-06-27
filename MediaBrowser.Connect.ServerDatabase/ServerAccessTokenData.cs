using System;
using ServiceStack.DataAnnotations;
using ServiceStack.Model;

namespace MediaBrowser.Connect.ServerDatabase
{
    public class ServerAccessTokenData : IHasId<string>
    {
        public string ServerId { get; set; }
        public int UserId { get; set; }
        public string AccessToken { get; set; }
        public DateTime CreatedAt { get; set; }

        public string Id
        {
            get { return ServerId + "/" + UserId; }
        }
    }

    public class ServerAccessTokenDataEx
    {
        [BelongTo(typeof(ServerData))]
        [Alias("Id")]
        public string ServerId { get; set; }
        [BelongTo(typeof(ServerData))]
        public string ServerIp { get; set; }
        [BelongTo(typeof(ServerAccessTokenData))]
        public int UserId { get; set; }
        [BelongTo(typeof(ServerAccessTokenData))]
        public string AccessToken { get; set; }
        [BelongTo(typeof(ServerAccessTokenData))]
        public DateTime CreatedAt { get; set; }
    }
}