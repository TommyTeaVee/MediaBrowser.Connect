using ServiceStack;

namespace MediaBrowser.Connect.ServiceModel.Users
{
    [Route("/users/{Id}", "GET", Summary = "Gets user information.")]
    public class GetUser : IReturn<UserDto>
    {
        [ApiMember(Description = "The user's ID", DataType = "string", IsRequired = true, ParameterType = "path")]
        public int Id { get; set; }
    }
}