using System;
using ServiceStack.DataAnnotations;
using ServiceStack.Model;

namespace MediaBrowser.Connect.UserDatabase
{
    public class UserProfileData : IHasId<int>
    {
        [ForeignKey(typeof (UserAuthData))]
        public int Id { get; set; }

        public string DisplayName { get; set; }
        public string ForumDisplayName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLoggedIn { get; set; }
    }
}