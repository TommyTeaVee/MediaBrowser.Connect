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

    public class UpdateServerInstanceValidator : AbstractValidator<UpdateServerInstance> 
    {
        public UpdateServerInstanceValidator()
        {
            RuleFor(r => r.Name).NotEmpty();
            RuleFor(r => r.Url).NotEmpty();
        }
    }

    public class CreateAccessTokenValidator : AbstractValidator<CreateAccessToken> 
    {
        public CreateAccessTokenValidator()
        {
            RuleFor(r => r.AccessKey).NotEmpty();
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
            var serverId = AuthenticateServer();
            if (serverId == null || serverId != request.ServerId) {
                throw new UnauthorizedAccessException();
            }

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
            return serverProvider.RegisterServerAccessToken(serverId, int.Parse(session.UserAuthId), request.AccessKey, UserType.LinkedAccount);
        }

        private string AuthenticateServer()
        {
            var accessKeyHeader = Request.GetHeader("Access-Key");
            if (string.IsNullOrEmpty(accessKeyHeader)) {
                return null;
            }
            
            var parts = accessKeyHeader.Split(':');
            if (parts.Length != 2) {
                return null;
            }

            var serverId = parts[0];
            var accessKey = parts[1];

            var authenticator = GetServerAuthenticator();
            if (authenticator.TryAuthenticate(serverId, accessKey)) {
                return serverId;
            }

            return null;
        }

        private bool AuthenticateUser(int requiredUserId)
        {
            IAuthSession session = GetSession();
            return session != null && session.IsAuthenticated && (session.UserAuthId == requiredUserId.ToString(CultureInfo.InvariantCulture) || session.HasRole(Roles.Admin));
        }

        public void Delete(DeleteAccessToken request)
        {
            var serverId = AuthenticateServer();
            var isServerAuthenticated = serverId != null && serverId == request.ServerId;
            if (!isServerAuthenticated && !AuthenticateUser(request.UserId)) {
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
            if (!AuthenticateUser(request.UserId)) {
                throw new UnauthorizedAccessException();
            }

            var serverProvider = GetServerProvider();
            return serverProvider.GetUsersServerAccessTokens(request.UserId).ToList();
        }
        
        private IServerProvider GetServerProvider()
        {
            var serverProvider = TryResolve<IServerProvider>();
            if (serverProvider == null) {
                throw new InvalidOperationException("No server provider has been registered.");
            }

            return serverProvider;
        }

        private IServerAuthenticator GetServerAuthenticator()
        {
            var serverAuthenticator = TryResolve<IServerAuthenticator>();
            if (serverAuthenticator == null) {
                throw new InvalidOperationException("No server authenticator has been registered.");
            }

            return serverAuthenticator;
        }
    }
}