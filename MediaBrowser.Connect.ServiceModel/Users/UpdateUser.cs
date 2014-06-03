using ServiceStack;

namespace MediaBrowser.Connect.ServiceModel.Users
{
    [Route("/users/{Id}", "POST", Summary="Updates a user")]
    public class UpdateUser : UserDto, IReturn<UserDto>
    {
        public string Password { get; set; }
    }
}