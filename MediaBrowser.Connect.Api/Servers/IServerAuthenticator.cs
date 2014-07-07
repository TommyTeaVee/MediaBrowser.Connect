namespace MediaBrowser.Connect.Interfaces.Servers
{
    public interface IServerAuthenticator
    {
        bool TryAuthenticate(string serverId, string accessKey);
    }
}