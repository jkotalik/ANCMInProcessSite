using System;
using System.Linq;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Server.IIS;
using Microsoft.AspNetCore.Server.IISIntegration;

namespace WebApplication29
{
    public class Startup
    {
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IAuthenticationSchemeProvider authSchemeProvider)
        {
            app.Run(async (context) =>
            {
                context.Response.ContentType = "text/plain";

                await context.Response.WriteAsync("Hello World - " + DateTimeOffset.Now + Environment.NewLine);
                await context.Response.WriteAsync(Environment.NewLine);

                await context.Response.WriteAsync("Address:" + Environment.NewLine);
                await context.Response.WriteAsync("Scheme: " + context.Request.Scheme + Environment.NewLine);
                await context.Response.WriteAsync("Host: " + context.Request.Headers["Host"] + Environment.NewLine);
                await context.Response.WriteAsync("PathBase: " + context.Request.PathBase.Value + Environment.NewLine);
                await context.Response.WriteAsync("Path: " + context.Request.Path.Value + Environment.NewLine);
                await context.Response.WriteAsync("Query: " + context.Request.QueryString.Value + Environment.NewLine);
                await context.Response.WriteAsync(Environment.NewLine);

                await context.Response.WriteAsync("Connection:" + Environment.NewLine);
                await context.Response.WriteAsync("RemoteIp: " + context.Connection.RemoteIpAddress + Environment.NewLine);
                await context.Response.WriteAsync("RemotePort: " + context.Connection.RemotePort + Environment.NewLine);
                await context.Response.WriteAsync("LocalIp: " + context.Connection.LocalIpAddress + Environment.NewLine);
                await context.Response.WriteAsync("LocalPort: " + context.Connection.LocalPort + Environment.NewLine);
                await context.Response.WriteAsync("ClientCert: " + context.Connection.ClientCertificate + Environment.NewLine);
                await context.Response.WriteAsync(Environment.NewLine);

                await context.Response.WriteAsync("User: " + context.User.Identity.Name + Environment.NewLine);
                var scheme = await authSchemeProvider.GetSchemeAsync(IISDefaults.AuthenticationScheme);
                await context.Response.WriteAsync("DisplayName: " + scheme?.DisplayName + Environment.NewLine);

                await context.Response.WriteAsync(Environment.NewLine);

                await context.Response.WriteAsync("Headers:" + Environment.NewLine);
                foreach (var header in context.Request.Headers)
                {
                    await context.Response.WriteAsync(header.Key + ": " + header.Value + Environment.NewLine);
                }
                await context.Response.WriteAsync(Environment.NewLine);

                await context.Response.WriteAsync("Environment Variables:" + Environment.NewLine);
                var vars = Environment.GetEnvironmentVariables();
                foreach (var key in vars.Keys.Cast<string>().OrderBy(key => key, StringComparer.OrdinalIgnoreCase))
                {
                    var value = vars[key];
                    await context.Response.WriteAsync(key + ": " + value + Environment.NewLine);
                }
                await context.Response.WriteAsync(Environment.NewLine);

                // accessing IIS server variables
                await context.Response.WriteAsync("Server Variables:" + Environment.NewLine);

                foreach (var varName in IISServerVarNames)
                {
                    await context.Response.WriteAsync(varName + ": " + context.GetIISServerVariable(varName) + Environment.NewLine);
                }

                await context.Response.WriteAsync(Environment.NewLine);
                if (context.Features.Get<IHttpUpgradeFeature>() != null)
                {
                    await context.Response.WriteAsync("Websocket feature is enabled.");
                }
                else
                {
                    await context.Response.WriteAsync("Websocket feature is disabled.");
                }
            });
        }

        private static readonly string[] IISServerVarNames =
        {
            "AUTH_TYPE",
            "AUTH_USER",
            "CONTENT_TYPE",
            "HTTP_HOST",
            "HTTPS",
            "REMOTE_PORT",
            "REMOTE_USER",
            "REQUEST_METHOD",
            "WEBSOCKET_VERSION"
        };

        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}
