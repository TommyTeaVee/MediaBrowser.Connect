﻿using ServiceStack;

namespace MediaBrowser.Connect.ServiceModel.Users
{
    [Route("/users/{Id}", "POST")]
    public class UpdateUser : UserDto, IReturn<UserDto>
    {
    }
}