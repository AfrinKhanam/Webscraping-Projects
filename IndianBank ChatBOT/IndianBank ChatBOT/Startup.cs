// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Linq;
using IndianBank_ChatBOT.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Bot.Configuration;
using Microsoft.Bot.Connector.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using IndianBank_ChatBOT.Utils;
using Microsoft.AspNetCore.Http;

namespace IndianBank_ChatBOT
{
    /// <summary>
    /// Startup class
    /// </summary>
    public class Startup
    {
        #region properties
        private ILoggerFactory _loggerFactory;
        private bool _isProduction = false;
        public IConfiguration Configuration { get; }

        #endregion

        #region methods

        public Startup(IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            _isProduction = env.IsProduction();
            _loggerFactory = loggerFactory;
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            if (env.IsDevelopment())
                builder.AddUserSecrets<Startup>();

            Configuration = builder.Build();
        }

        /// <summary>
        /// Configure Services
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            var appSettings = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettings);
            // Load the connected services from .bot file.
            var botFilePath = Configuration.GetSection("botFilePath")?.Value;
            var botFileSecret = Configuration.GetSection("botFileSecret")?.Value;
            var botConfig = BotConfiguration.Load(botFilePath, botFileSecret);
            services.AddSingleton(sp => botConfig ?? throw new InvalidOperationException($"The .bot config file could not be loaded."));
            // Initializes your bot service clients and adds a singleton that your Bot can access through dependency injection.
            var connectedServices = new BotServices(botConfig)
            {
                Configuration = Configuration
            };
            services.AddSingleton(sp => connectedServices);
            // Initialize Bot State
            var dataStore = new MemoryStorage();
            var userState = new UserState(dataStore);
            var conversationState = new ConversationState(dataStore);


            services.AddSingleton(dataStore);
            services.AddSingleton(userState);
            services.AddSingleton(conversationState);
            services.AddSingleton(new BotStateSet(userState, conversationState));
            services.AddMvc();
            //services.AddDbContext<LogDataContext>(options => options.UseNpgsql(Configuration.GetConnectionString("connString")));
            //services.AddTransient<LogDataContext, LogDataContext>();

            services.AddDbContext<AppDbContext>(options => options.UseNpgsql(Configuration.GetConnectionString("connString")));
            services.AddTransient<AppDbContext, AppDbContext>();

            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            connectedServices.ServiceProvider = services.BuildServiceProvider();

            services.AddBot<IndianBank_ChatBOT>(options =>
            {
                // Load the connected services from .bot file.
                var environment = _isProduction ? "production" : "development";
                var service = botConfig.Services.FirstOrDefault(s => s.Type == ServiceTypes.Endpoint && s.Name == environment);

                if (!(service is EndpointService endpointService))
                {
                    throw new InvalidOperationException($"The .bot file does not contain an endpoint with name '{environment}'.");
                }

                var connectionString = Configuration.GetConnectionString("connString");
                var myLogger = new BotChatActivityLogger(connectionString);

                options.CredentialProvider = new SimpleCredentialProvider(endpointService.AppId, endpointService.AppPassword);
                options.OnTurnError = async (context, exception) =>
                {
                    var activity = context.Activity;
                    BotChatActivityLogger.UpdateRaSaData("bye_intent", 0, "");
                    BotChatActivityLogger.UpdateResponseJsonText(string.Empty);
                    BotChatActivityLogger.UpdateSource(ResponseSource.Rasa);
                    await BotChatActivityLogger.LogActivityCustom(activity);
                    await context.SendActivityAsync("Sorry,I could not understand. Could you please rephrase the query.");
                    // await context.SendActivityAsync(exception.GetBaseException().ToString());
                };
                

                var transcriptMiddleware = new TranscriptLoggerMiddleware(myLogger);
                options.Middleware.Add(transcriptMiddleware);
                //Transcript store to log incoming and outgoing msg
                var transcriptStore = new MemoryTranscriptStore();
                // Typing Middleware (automatically shows typing when the bot is responding/working)
                var typingMiddleware = new ShowTypingMiddleware();
                options.Middleware.Add(typingMiddleware);
                options.Middleware.Add(new AutoSaveStateMiddleware(userState, conversationState));
            });
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">Application Builder.</param>
        /// <param name="env">Hosting Environment.</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // Configure Application Insights
            _loggerFactory.AddApplicationInsights(app.ApplicationServices, LogLevel.Warning);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseDefaultFiles()
               .UseStaticFiles()
               .UseBotFramework();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Faq}/{action=Display}");
            });
        }


        #endregion
    }
}
