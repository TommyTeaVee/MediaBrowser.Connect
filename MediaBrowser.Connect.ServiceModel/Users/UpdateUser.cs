using ServiceStack;

namespace MediaBrowser.Connect.ServiceModel.Users
{
    [Route("/users/{Id}", "POST, PUT, UPDATE")]
    public class UpdateUser : UserDto, IReturn<UserDto>
    {
    }
}