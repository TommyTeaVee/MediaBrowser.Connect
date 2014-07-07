using ServiceStack;

namespace MediaBrowser.Connect.ServiceModel.RemoteAccess
{
    [Route("/invitations", Summary = "Accepts a guest invitation.")]
    public class AcceptInvitation
    {
        [ApiMember(Description = "Unique invitation ID", DataType = "string", IsRequired = true, ParameterType = "query")]
        public string InvitationId { get; set; }
    }
}