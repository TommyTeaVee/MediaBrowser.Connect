using System.Collections.Generic;
using ServiceStack;

namespace MediaBrowser.Connect.ServiceModel.RemoteAccess
{
    [Route("/users/{UserId}/tokens", "GET", Summary = "Gets the access tokens granted by any Media Browser Server instance for a specific user.")]
    public class GetUsersAccessTokens : IReturn<IList<ServerAccessTokenDto>>
    {
         [ApiMember(Description = "The user's ID", DataType = "number", IsRequired = true, ParameterType = "path")]
        public int UserId { get; set; }
    }
}