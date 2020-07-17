using BOTAIML.ChatBot.DirectLineServer.Core.Middlewares;
using BOTAIML.ChatBot.DirectLineServer.Core.Services;
using BOTAIML.ChatBot.DirectLineServer.Core.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using System;
using System.IO;

namespace BOTAIML.ChatBot.DirectLineServer.Core
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseDirectLineCors(this IApplicationBuilder app)
        {
            app.UseCors(DirectLineDefaults.CorsPolicyNames);
            return app;
        }

        public static IApplicationBuilder UseDirectLineUploadsStatic(this IApplicationBuilder app)
        {
            var sp = app.ApplicationServices;
            var env = sp.GetRequiredService<IWebHostEnvironment>();
            var directLineOpt = sp.GetRequiredService<IOptions<DirectLineSettings>>()?.Value;

            if (directLineOpt == null)
            {
                throw new Exception("DirectLineOptions cannot be null!");
            }

            var baseDirOfAttachments = Path.Combine(env.ContentRootPath, directLineOpt.Attachments.BaseDirectoryForUploading);
            var requestPath = directLineOpt.Attachments.BaseUrlForDownloading;
            requestPath = requestPath.StartsWith("/") ? requestPath : "/" + requestPath;
            if (!Directory.Exists(baseDirOfAttachments))
                Directory.CreateDirectory(baseDirOfAttachments);

            var fileProvider = new PhysicalFileProvider(baseDirOfAttachments) { };  //todo : file filter
            var so = new StaticFileOptions()
            {
                RequestPath = requestPath,
                FileProvider = fileProvider,
            };
            app.UseStaticFiles(so);
            return app;
        }

        public static IApplicationBuilder UseDirectLineCore(this IApplicationBuilder app)
        {

            var sp = app.ApplicationServices;
            var env = sp.GetRequiredService<IWebHostEnvironment>();
            var directLineSettings = sp.GetRequiredService<IOptions<DirectLineSettings>>()?.Value;
            if (directLineSettings == null)
            {
                throw new Exception("DirectLineOptions cannot be null!");
            }

            var webSocketOptions = new WebSocketOptions()
            {
                KeepAliveInterval = TimeSpan.FromSeconds(120),
                ReceiveBufferSize = 4 * 1024
            };

            var botEndPoint = directLineSettings.BotEndpoint;
            var botOrigin = UtilsEx.GetOrigin(botEndPoint);
            webSocketOptions.AllowedOrigins.Add(botOrigin);
            webSocketOptions.AllowedOrigins.Add("*");
            app.UseWebSockets(webSocketOptions);
            app.UseMiddleware<WebSocketConnectionMiddleware>();
            return app;
        }
    }
}