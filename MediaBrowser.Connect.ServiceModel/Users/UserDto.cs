namespace MediaBrowser.Connect.ServiceModel.Users
{
    public class UserDto
    {
        public virtual int Id { get; set; }
        public virtual string ForumUsername { get; set; }
        public virtual string Email { get; set; }
        public virtual string ForumDisplayName { get; set; }
        public virtual string DisplayName { get; set; }
    }
}