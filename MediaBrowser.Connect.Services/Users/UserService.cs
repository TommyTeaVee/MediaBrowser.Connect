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
        // todo direct access to user creation is not what we eventually want; we need something like a CAPTCHA and email verification before submitting to this API call
        // ideally, we would have a proper account sign up page on mediabrowser.tv, but redirecting users to the forum sign up page would also work
        public UserDto Post(CreateUser request)
        {
            IUserProvider userProvider = GetUserProvider();
            return userProvider.CreateUser(request, request.Password);
        }
        
        [Authenticate]
        public object Get(GetUser request)
        {
            IAuthSession session = GetSession();
            if (session == null || !session.IsAuthenticated || (session.UserAuthId != request.Id.ToString(CultureInfo.InvariantCulture) && !session.HasRole(Roles.Admin))) {
                throw new UnauthorizedAccessException();
            }

            var cacheKey = UserCacheKey(request.Id);
            return Request.ToOptimizedResultUsingCache(Cache, cacheKey, () => {
                IUserProvider userProvider = GetUserProvider();
                return userProvider.GetUser(request.Id);
            });
        }

        private static string UserCacheKey(int userId)
        {
            return "users/{0}".Fmt(userId);
        }

        [Authenticate]
        public UserDto Put(UpdateUser request)
        {
            return Post(request);
        }

        [Authenticate]
        public UserDto Post(UpdateUser request)
        {
            IAuthSession session = GetSession();
            if (session == null || !session.IsAuthenticated || (session.UserAuthId != request.Id.ToString(CultureInfo.InvariantCulture) && !session.HasRole(Roles.Admin))) {
                throw new UnauthorizedAccessException();
            }

            IUserProvider userProvider = GetUserProvider();
            var result = userProvider.UpdateUser(request, request.Password);

            Request.RemoveFromCache(Cache, UserCacheKey(request.Id));

            return result;
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