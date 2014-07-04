using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using MediaBrowser.Connect.Interfaces.Auth;
using MediaBrowser.Connect.Interfaces.Servers;
using MediaBrowser.Connect.ServiceModel.RemoteAccess;
using MediaBrowser.Connect.ServiceModel.Servers;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.FluentValidation;

namespace MediaBrowser.Connect.Services.Servers
{
    public class RegisterServerInstanceValidator : AbstractValidator<RegisterServerInstance>
    {
        public RegisterServerInstanceValidator()
        {
            RuleFor(r => r.Url).NotEmpty();
            RuleFor(r => r.Name).NotEmpty();
        }
    }

    public class MbInstanceService : Service
    {
        public ServerInstanceAuthInfoDto Post(RegisterServerInstance request)
        {
            var serverProvider = GetServerProvider();
            return serverProvider.RegisterServerInstance(request.Name, request.Url);
        }

        public ServerInstanceDto Post(UpdateServerInstance request)
        {
            var serverProvider = GetServerProvider();
            return serverProvider.UpdateServerInstance(request.ServerId, request.Name, request.Url);
        }

        public ServerAccessTokenDto Post(CreateAccessToken request)
        {
            IAuthSession session = GetSession();
            if (session == null || !session.IsAuthenticated) {
                throw new UnauthorizedAccessException();
            }

            var serverId = AuthenticateServer();
            if (serverId == null) {
                throw new UnauthorizedAccessException();
            }

            var serverProvider = GetServerProvider();
            return serverProvider.RegisterServerAccessToken(serverId, int.Parse(session.UserAuthId), request.AccessKey);
        }

        private string AuthenticateServer()
        {
            throw new NotImplementedException();
        }

        public void Delete(DeleteAccessToken request)
        {
            var serverId = AuthenticateServer();
            if (serverId == null || serverId != request.ServerId) {
                throw new UnauthorizedAccessException();
            }

            var serverProvider = GetServerProvider();
            serverProvider.RevokeServerAccessToken(serverId, request.UserId);
        }

        public IList<ServerAccessTokenDto> Get(GetAccessTokens request)
        {
            var serverId = AuthenticateServer();
            if (serverId == null || serverId != request.ServerId) {
                throw new UnauthorizedAccessException();
            }

            var serverProvider = GetServerProvider();
            return serverProvider.GetServerAccessTokens(serverId).ToList();
        }

        [Authenticate]
        public IList<ServerAccessTokenDto> Get(GetUsersAccessTokens request)
        {
            IAuthSession session = GetSession();
            if (session == null || !session.IsAuthenticated || (session.UserAuthId != request.UserId.ToString(CultureInfo.InvariantCulture) && !session.HasRole(Roles.Admin))) {
                throw new UnauthorizedAccessException();
            }

            var serverProvider = GetServerProvider();
            return serverProvider.GetUsersServerAccessTokens(request.UserId).ToList();
        }

        public ServerAccessTokenDto Post(InviteGuest request)
        {
            throw new NotImplementedException();
        }

        private IServerProvider GetServerProvider()
        {
            var serverProvider = TryResolve<IServerProvider>();
            if (serverProvider == null) {
                throw new InvalidOperationException("No server provider has been registered.");
            }

            return serverProvider;
        }
    }
}