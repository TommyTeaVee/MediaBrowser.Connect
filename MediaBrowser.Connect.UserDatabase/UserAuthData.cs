﻿using ServiceStack.DataAnnotations;
using ServiceStack.Model;

namespace MediaBrowser.Connect.UserDatabase
{
    public class UserAuthData : IHasId<int>
    {
        [AutoIncrement]
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Salt { get; set; }
        public string Password { get; set; }
    }
}
