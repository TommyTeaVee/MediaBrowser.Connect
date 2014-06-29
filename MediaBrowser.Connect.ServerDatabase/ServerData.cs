using ServiceStack.Model;

namespace MediaBrowser.Connect.ServerDatabase
{
    public class ServerData : IHasId<string>
    {
        public string Url { get; set; }
        public string Name { get; set; }
        public string Salt { get; set; }
        public string AccessKey { get; set; }
        public string Id { get; set; }
    }
}