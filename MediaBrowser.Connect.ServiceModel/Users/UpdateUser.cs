using ServiceStack;

namespace MediaBrowser.Connect.ServiceModel.Users
{
    [Route("/users/{Id}", "POST, PUT", Summary="Updates a user")]
    public class UpdateUser : UserDto, IReturn<UserDto>
    {
    }
}