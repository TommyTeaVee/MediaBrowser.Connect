using ServiceStack;

namespace MediaBrowser.Connect.Interfaces.Auth
{
    public interface IUserAuthenticator
    {
        bool TryAuthenticate(IServiceBase authService, string userName, string password);
    }
}