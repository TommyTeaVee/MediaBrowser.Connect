﻿using Funq;
using MediaBrowser.Connect.Services.Auth;
using MediaBrowser.Connect.Services.Users;
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
            LogManager.LogFactory = new NLogFactory();

            container.Register<ICacheClient>(new MemoryCacheClient());

            Plugins.Add(new SwaggerFeature());
            Plugins.Add(new ValidationFeature());

            var authProviders = new IAuthProvider[] {
                new CredentialsAuthenticator(),
                new BasicAuthAuthenticator()
            };

            var authFeature = new AuthFeature(() => new AuthUserSession(), authProviders) {
                IncludeAssignRoleServices = false,
                IncludeRegistrationService = false
            };

            Plugins.Add(authFeature);
        }
    }
}