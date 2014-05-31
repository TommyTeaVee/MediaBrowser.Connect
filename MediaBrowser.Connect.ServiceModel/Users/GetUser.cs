using System;
using ServiceStack;

namespace MediaBrowser.Connect.ServiceModel.Users
{
    [Route("/users/{UserId}", "GET", Summary = "Gets user information.")]
    public class GetUser : IReturn<UserDto>
    {
        public int UserId { get; set; }
    }
}