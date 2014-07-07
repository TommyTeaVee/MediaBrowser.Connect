using ServiceStack;

namespace MediaBrowser.Connect.ServiceModel.Users
{
    [Route("/users", "POST", Summary = "Creates a new user.")]
    public class CreateUser : UserDto, IReturn<UserDto>
    {
        [ApiMember(Description = "The user's password", DataType = "string", IsRequired = true, ParameterType = "query")]
        public string Password { get; set; }

        [ApiMember(Description = "The user's email address", DataType = "string", IsRequired = true, ParameterType = "query")]
        public override string Email
        {
            get { return base.Email; }
            set { base.Email = value; }
        }

        [ApiMember(Description = "The user's forum login username", DataType = "string", IsRequired = true, ParameterType = "query")]
        public override string ForumUsername
        {
            get { return base.ForumUsername; }
            set { base.ForumUsername = value; }
        }

        [ApiMember(Description = "The user's Media Browser display name", DataType = "string", ParameterType = "query")]
        public override string DisplayName
        {
            get { return base.DisplayName; }
            set { base.DisplayName = value; }
        }

        [ApiMember(Description = "The user's forum display name", DataType = "string", ParameterType = "query")]
        public override string ForumDisplayName
        {
            get { return base.ForumDisplayName; }
            set { base.ForumDisplayName = value; }
        }

        [ApiMember(Description = "The user's ID. Ignored when creating a new user", DataType = "number", ParameterType = "query")]
        public override int Id
        {
            get { return base.Id; }
            set { base.Id = value; }
        }
    }
}