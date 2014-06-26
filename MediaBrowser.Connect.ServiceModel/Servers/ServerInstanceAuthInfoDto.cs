using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaBrowser.Connect.ServiceModel.Servers
{
    public class ServerInstanceAuthInfoDto: ServerInstanceDto
    {
        public string AccessKey { get; set; }
    }
}
