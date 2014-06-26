using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediaBrowser.Connect.Interfaces.Auth;
using MediaBrowser.Connect.Interfaces.Servers;
using MediaBrowser.Connect.Interfaces.Users;
using ServiceStack;
using ServiceStack.Data;
using ServiceStack.OrmLite;

namespace MediaBrowser.Connect.ServerDatabase
{
    public class ServerDatabase
    {
        public IDbConnectionFactory Connection { get; set; }
    }

    public class ServerDatabaseFeature : IPlugin
    {
        public string ConnectionString { get; set; }

        public void Register(IAppHost appHost)
        {
            var connectionFactory = OpenDatabase();

            appHost.Register(new ServerDatabase {Connection = connectionFactory});
            appHost.RegisterAs<ServerProvider, IServerProvider>();
        }

        private OrmLiteConnectionFactory OpenDatabase()
        {
            var connectionString = ConnectionString ?? ":memory:";
            return new OrmLiteConnectionFactory(connectionString, SqliteDialect.Provider);
        }
    }
}
