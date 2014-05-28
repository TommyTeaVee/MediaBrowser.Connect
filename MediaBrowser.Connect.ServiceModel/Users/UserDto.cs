﻿using System;

namespace MediaBrowser.Connect.ServiceModel.Users
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string ForumUsername { get; set; }
        public string Email { get; set; }
        public string ForumDisplayName { get; set; }
        public string DisplayName { get; set; }
    }
}