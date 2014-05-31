using System;
using System.Globalization;
using MediaBrowser.Connect.Interfaces.Auth;
using MediaBrowser.Connect.Interfaces.Users;
using MediaBrowser.Connect.ServiceModel.Users;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.FluentValidation;

namespace MediaBrowser.Connect.Services.Users
{
    public class CreateUserValidator : AbstractValidator<CreateUser>
    {
        public CreateUserValidator()
        {
            RuleFor(r => r.Email).EmailAddress();
            RuleFor(r => r.ForumUsername).NotEmpty();
            RuleFor(r => r.Password).NotEmpty();
        }
    }

    public class UserService : Service
    {
        public UserDto Put(CreateUser request)
        {
            IUserProvider userProvider = GetUserProvider();
            return userProvider.CreateUser(request, request.Password);
        }
        
        [Authenticate]
        public UserDto Get(GetUser request)
        {
            IAuthSession session = GetSession();
            if (session == null || !session.IsAuthenticated || (session.Id != request.UserId.ToString(CultureInfo.InvariantCulture) && !session.HasRole(Roles.Admin))) {
                throw new UnauthorizedAccessException();
            }

            IUserProvider userProvider = GetUserProvider();
            return userProvider.GetUser(request.UserId);
        }

        [Authenticate]
        public UserDto Any(UpdateUser request)
        {
            IAuthSession session = GetSession();
            if (session == null || !session.IsAuthenticated || (session.Id != request.Id.ToString(CultureInfo.InvariantCulture) && !session.HasRole(Roles.Admin))) {
                throw new UnauthorizedAccessException();
            }

            IUserProvider userProvider = GetUserProvider();
            return userProvider.UpdateUser(request);
        }

        private IUserProvider GetUserProvider()
        {
            var userProvider = TryResolve<IUserProvider>();
            if (userProvider == null) {
                throw new InvalidOperationException("No user provider has been registered");
            }

            return userProvider;
        }
    }
}