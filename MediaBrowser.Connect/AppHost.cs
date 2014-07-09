using System.Configuration;
using Funq;
using MediaBrowser.Connect.ServerDatabase;
using MediaBrowser.Connect.Services.Auth;
using MediaBrowser.Connect.Services.Users;
using MediaBrowser.Connect.UserDatabase;
using ServiceStack;
using ServiceStack.Api.Swagger;
using ServiceStack.Auth;
using ServiceStack.Caching;
using ServiceStack.Logging;
using ServiceStack.Logging.NLogger;
using ServiceStack.Text;
using ServiceStack.Validation;

namespace MediaBrowser.Connect
{
    public class AppHost : AppSelfHostBase
    {
        public AppHost()
            : base("Media Browser Connect Service", typeof (UserService).Assembly) {}

        public override void Configure(Container container)
        {
            JsConfig.DateHandler = DateHandler.ISO8601;
            //LogManager.LogFactory = new NLogFactory();
            
            var authProviders = new IAuthProvider[] {
                new CredentialsAuthenticator(),
                new BasicAuthAuthenticator()
            };

            var authFeature = new AuthFeature(() => new AuthUserSession(), authProviders) {
                IncludeAssignRoleServices = false,
                IncludeRegistrationService = false,
            };

            var connectionString = GetDbConnectionString();

            Plugins.Add(authFeature);
            Plugins.Add(new UserDatabaseFeature {ConnectionString = connectionString});
            Plugins.Add(new ServerDatabaseFeature {ConnectionString = connectionString});
            Plugins.Add(new SwaggerFeature());
            Plugins.Add(new ValidationFeature());

            container.Register<ICacheClient>(new MemoryCacheClient());
            container.RegisterValidators(typeof (CreateUserValidator).Assembly);
        }

        private string GetDbConnectionString()
        {
            var connection = ConfigurationManager.ConnectionStrings["SqliteDatabase"];
            if (connection != null) {
                return connection.ConnectionString;
            }

            return null;
        }
    }
}