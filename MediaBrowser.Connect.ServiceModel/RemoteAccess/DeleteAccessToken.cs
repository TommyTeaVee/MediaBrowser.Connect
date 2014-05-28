using System;
using ServiceStack;

namespace MediaBrowser.Connect.ServiceModel.RemoteAccess
{
    [Route("/servers/{ServerId}/tokens", "DELETE", Summary = "Revokes an access token for a specified user.")]
    public class DeleteAccessToken
    {
        public Guid ServerId { get; set; }
        public Guid UserId { get; set; }
    }
}