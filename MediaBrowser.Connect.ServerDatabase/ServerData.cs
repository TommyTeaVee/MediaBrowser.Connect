using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.Model;

namespace MediaBrowser.Connect.ServerDatabase
{
    public class ServerData : IHasId<string>
    {
        public string Id { get; set; }
        public string IpAddress { get; set; }
        public string Name { get; set; }
        public string Salt { get; set; }
        public string AccessKey { get; set; }
    }
}
