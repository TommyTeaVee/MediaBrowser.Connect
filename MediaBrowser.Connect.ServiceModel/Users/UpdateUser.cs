using ServiceStack;

namespace MediaBrowser.Connect.ServiceModel.Users
{
    [Route("/users/{Id}", "POST, PUT", Summary = "Updates a user.")]
    public class UpdateUser : UserDto, IReturn<UserDto>
    {
        [ApiMember(Description = "The user's password", DataType = "string", ParameterType = "query")]
        public string Password { get; set; }

        [ApiMember(Description = "The user's ID", DataType = "number", IsRequired = true, ParameterType = "path")]
        public override int Id
        {
            get { return base.Id; }
            set { base.Id = value; }
        }
    }
}