using MediaBrowser.Connect.Interfaces.Auth;
using ServiceStack;
using ServiceStack.Auth;

namespace MediaBrowser.Connect.Services.Auth
{
    /// <summary>
    ///     The <see cref="MediaBrowser.Connect.Services.Auth.BasicAuthAuthenticator" /> is a <see cref="BasicAuthProvider" />
    ///     which delegeates its authentication logic to an instance of <see cref="IUserAuthenticator" /> which has been
    ///     registered with the application's IoC container.
    /// </summary>
    public class BasicAuthAuthenticator : BasicAuthProvider
    {
        public override bool TryAuthenticate(IServiceBase authService, string userName, string password)
        {
            var authenticator = authService.TryResolve<IUserAuthenticator>();

            if (authenticator == null) {
                Log.Error("No user authenticator has been registered.");
                return false;
            }

            return authenticator.TryAuthenticate(authService, userName, password);
        }
    }
}