﻿using AngularJSAuthentication.API.Providers;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

[assembly: OwinStartup(typeof(AngularJSAuthentication.API.Startup))]
namespace AngularJSAuthentication.API
{
    /// <summary>
    /// this class will be fired once our server starts
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// The “Configuration” method accepts parameter of type “IAppBuilder” 
        /// this parameter will be supplied by the host at run-time. This “app” 
        /// parameter is an interface which will be used to compose the application 
        /// for our Owin server.
        /// </summary>
        /// <param name="app"></param>
        public void Configuration(IAppBuilder app) {
            //The “HttpConfiguration” object is used to configure API routes, 
            //so we’ll pass this object to method “Register” in “WebApiConfig” class
            HttpConfiguration config = new HttpConfiguration();

            ConfigureOAuth(app);

            WebApiConfig.Register(config);
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            app.UseWebApi(config);
        }

        /// <summary>
        /// set this options
        /// - The path for generating tokens will be as :”http://localhost:port/token”. 
        ///   We’ll see how we will issue HTTP POST request to generate token in the next steps.
        /// - We’ve specified the expiry for token to be 24 hours, so if the user tried to use 
        ///   the same token for authentication after 24 hours from the issue time, his request 
        ///   will be rejected and HTTP status code 401 is returned.
        /// - We’ve specified the implementation on how to validate the credentials for users 
        ///   asking for tokens in custom class named “SimpleAuthorizationServerProvider”.
        /// </summary>
        /// <param name="app"></param>
        public void ConfigureOAuth(IAppBuilder app)
        {
            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                Provider = new SimpleAuthorizationServerProvider()
            };

            // Token Generation
            app.UseOAuthAuthorizationServer(OAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }
    }
}