using ServiceStack;

namespace MediaBrowser.Connect.ServiceModel.Users
{
    [Route("/user", "GET", Summary = "Gets the currently authenticated user.")]
    public class GetCurrentUser : IReturn<UserDto>
    {
    }
}
