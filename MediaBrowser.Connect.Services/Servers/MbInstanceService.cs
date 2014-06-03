using MediaBrowser.Connect.ServiceModel.Servers;
using ServiceStack;

namespace MediaBrowser.Connect.Services.Servers
{
    public class MbInstanceService : Service
    {
        public ServerInstanceDto Post(RegisterServerInstance request)
        {
            return new ServerInstanceDto();
        }

        public void Post(UpdateServerInstance request) {}
    }
}