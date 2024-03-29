﻿using ServiceStack.DataAnnotations;
using ServiceStack.Model;

namespace MediaBrowser.Connect.UserDatabase
{
    public class UserAuthData : IHasId<int>
    {
        [Index(Unique = true)]
        public string Username { get; set; }

        [Index(Unique = true)]
        public string Email { get; set; }

        public string Salt { get; set; }

        public string Password { get; set; }

        [AutoIncrement]
        public int Id { get; set; }
    }
}