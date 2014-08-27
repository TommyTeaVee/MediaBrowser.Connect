using MediaBrowser.Connect.Interfaces.Auth;
using MediaBrowser.Connect.Interfaces.Users;
using ServiceStack;
using ServiceStack.Data;
using ServiceStack.OrmLite;

namespace MediaBrowser.Connect.UserDatabase
{
    public class UserDatabase
    {
        public IDbConnectionFactory Connection { get; set; }
    }

    public class UserDatabaseFeature : IPlugin
    {
        public string ConnectionString { get; set; }

        public void Register(IAppHost appHost)
        {
            var connectionFactory = OpenDatabase();

            appHost.Register(new UserDatabase {Connection = connectionFactory});
            appHost.RegisterAs<UserAuthenticator, IUserAuthenticator>();
            appHost.RegisterAs<UserProvider, IUserProvider>();
        }

        private OrmLiteConnectionFactory OpenDatabase()
        {
            if (string.IsNullOrEmpty(ConnectionString)) {
                return new OrmLiteConnectionFactory(":memory:", SqliteDialect.Provider);
            }

            return new OrmLiteConnectionFactory(ConnectionString, MySqlDialect.Provider);
        }
    }
}
