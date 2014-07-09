using System;
using MediaBrowser.Connect.Interfaces.Servers;
using MediaBrowser.Connect.ServiceModel.RemoteAccess;
using ServiceStack;

namespace MediaBrowser.Connect.Services.Servers
{
    public class GuestAccountService : Service
    {
        public void Post(InviteGuest request)
        {
            string invitationId = Guid.NewGuid().ToString();
            Cache.Add(invitationId, request);

            //todo send invitation email
        }

        public object Any(AcceptInvitation request)
        {
            //todo direct to proper invitation completion pages

            var invitation = Cache.Get<InviteGuest>(request.InvitationId);
            if (invitation != null) {
                IServerProvider serverProvider = GetServerProvider();
                serverProvider.RegisterServerAccessToken(invitation.ServerId, invitation.UserId, invitation.AccessKey, UserType.Guest);

                Cache.Remove(request.InvitationId);
                return "Guest account activated.";
            }

            return "Guest invitation expired.";
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