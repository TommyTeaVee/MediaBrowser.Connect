using System;
using ServiceStack;

namespace MediaBrowser.Connect.ServiceModel.RemoteAccess
{
    [Route("/servers/{ServerId}/tokens", "DELETE", Summary = "Revokes an access token for a specified user.")]
    public class DeleteAccessToken
    {
        public string ServerId { get; set; }
        public int UserId { get; set; }
    }
}