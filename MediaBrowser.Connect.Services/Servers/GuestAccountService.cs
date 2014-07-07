using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediaBrowser.Connect.Interfaces.Servers;
using MediaBrowser.Connect.ServiceModel.RemoteAccess;
using ServiceStack;

namespace MediaBrowser.Connect.Services.Servers
{
    public class GuestAccountService : Service
    {
        public void Post(InviteGuest request)
        {
            var invitationId = Guid.NewGuid().ToString();
            Cache.Add(invitationId, request);

            //todo send invitation email
        }

        public object Any(AcceptInvitation request)
        {
            var invitation = Cache.Get<InviteGuest>(request.InvitationId);
            if (invitation != null) {
                var serverProvider = GetServerProvider();
                serverProvider.RegisterServerAccessToken(invitation.ServerId, invitation.UserId, invitation.AccessKey, UserType.Guest);
            }
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
