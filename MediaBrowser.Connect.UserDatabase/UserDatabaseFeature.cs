using MediaBrowser.Connect.Interfaces.Auth;
using ServiceStack;
using ServiceStack.Data;
using ServiceStack.OrmLite;

namespace MediaBrowser.Connect.UserDatabase
{
    public class UserDatabaseFeature : IPlugin
    {
        public string ConnectionString { get; set; }

        public void Register(IAppHost appHost)
        {
            var connectionFactory = OpenDatabase();

            appHost.Register<IDbConnectionFactory>(connectionFactory);
            appHost.RegisterAs<UserAuthenticator, IUserAuthenticator>();
        }

        private OrmLiteConnectionFactory OpenDatabase()
        {
            var connectionString = ConnectionString ?? ":memory:";
            return new OrmLiteConnectionFactory(connectionString, SqliteDialect.Provider);
        }
    }
}
