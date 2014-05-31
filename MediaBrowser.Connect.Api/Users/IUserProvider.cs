using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediaBrowser.Connect.ServiceModel.Users;

namespace MediaBrowser.Connect.Interfaces.Users
{
    public interface IUserProvider
    {
        UserDto CreateUser(UserDto user, string password);
        UserDto GetUser(int userId);
        UserDto UpdateUser(UserDto user);
    }
}
