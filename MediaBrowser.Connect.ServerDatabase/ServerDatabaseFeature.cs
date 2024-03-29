﻿using MediaBrowser.Connect.Interfaces.Servers;
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
            OrmLiteConnectionFactory connectionFactory = OpenDatabase();

            appHost.Register(new ServerDatabase {Connection = connectionFactory});
            appHost.RegisterAs<ServerProvider, IServerProvider>();
            appHost.RegisterAs<ServerAuthenticator, IServerAuthenticator>();
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