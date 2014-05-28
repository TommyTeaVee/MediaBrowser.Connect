using ServiceStack;

namespace MediaBrowser.Connect.Api.Auth
{
    public interface IUserAuthenticator
    {
        bool TryAuthenticate(IServiceBase authService, string userName, string password);
    }
}