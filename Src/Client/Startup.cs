using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Client
{

    public class Startup
    {
#if DEBUG
        public const string ServerUrl = "https://localhost:44382";
        public const string ApiUrl = "https://localhost:44332";
#else
        public const string ServerUrl = "https://192.168.5.215:44382";
        public const string ApiUrl = "https://192.168.5.215:44332";
#endif
        public void ConfigureServices(IServiceCollection services)
            {
                services.AddAuthentication(config =>
                {
                    // We check the cookie to confirm that we are authenticated
                    config.DefaultAuthenticateScheme = "ClientCookie";
                    // When we sign in we will deal out a cookie
                    config.DefaultSignInScheme = "ClientCookie";
                    // use this to check if we are allowed to do something.
                    config.DefaultChallengeScheme = "OurServer";
                })
                    .AddCookie("ClientCookie")
                    .AddOAuth("OurServer", config =>
                    {
                        config.ClientId = "client_id";
                        config.ClientSecret = "client_secret";
                        config.CallbackPath = "/oauth/callback";
                        config.AuthorizationEndpoint = $"{ServerUrl}/oauth/authorize";
                        config.TokenEndpoint = $"{ServerUrl}/oauth/token";
                        config.SaveTokens = true;

                        config.Events = new OAuthEvents()
                        {
                            OnCreatingTicket = context =>
                            {
                                context.Identity.AddClaims(Service.Decoder.Read(context.AccessToken));
                                return Task.CompletedTask;
                            }
                        };
                    });

            services.AddHttpClient();
            services.AddControllersWithViews()
                .AddRazorRuntimeCompilation();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
