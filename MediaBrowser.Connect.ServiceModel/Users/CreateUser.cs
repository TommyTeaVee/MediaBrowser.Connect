using ServiceStack;

namespace MediaBrowser.Connect.ServiceModel.Users
{
    [Route("/users", "POST", Summary = "Creates a new user")]
    public class CreateUser : UserDto, IReturn<UserDto>
    {
        public string Password { get; set; }
    }
}