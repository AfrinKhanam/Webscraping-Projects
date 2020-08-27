﻿using System;
using System.Linq;
using IndianBank_ChatBOT.Models;
using IndianBank_ChatBOT.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Bot.Configuration;
using Microsoft.Bot.Connector.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

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

        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        #endregion

        #region methods

        public Startup(IWebHostEnvironment env, IConfiguration config, ILoggerFactory loggerFactory)
        {
            _isProduction = env.IsProduction();
            _loggerFactory = loggerFactory;

            Configuration = config;
        }

        /// <summary>
        /// Configure Services
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<KestrelServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

            var appSettings = Configuration.GetSection("AppSettings");

            services.Configure<AppSettings>(appSettings);

            services.AddHttpClient();

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

            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                builder => builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
            });


            services.AddAuthentication("CookieAuthentication")
               .AddCookie("CookieAuthentication", config =>
               {
                   config.Cookie.Name = "UserLoginCookie";
                   config.LoginPath = "/User/UserLogin";
               });

            if (_isProduction)
            {
                services.AddControllersWithViews()
                        .AddNewtonsoftJson(o =>
                        {
                            o.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                            o.SerializerSettings.Formatting = Formatting.Indented;
                            o.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                        });
            }
            else
            {
                services.AddControllersWithViews()
                        .AddRazorRuntimeCompilation()
                        .AddNewtonsoftJson(o =>
                        {
                            o.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                            o.SerializerSettings.Formatting = Formatting.Indented;
                            o.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                        });
            }

            //services.AddDbContext<LogDataContext>(options => options.UseNpgsql(Configuration.GetConnectionString("connString")));
            //services.AddTransient<LogDataContext, LogDataContext>();

            services.AddDbContext<AppDbContext>(options => options.UseNpgsql(Configuration.GetConnectionString("connString")));
            services.AddTransient<AppDbContext, AppDbContext>();

            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();

#pragma warning disable ASP0000 // Do not call 'IServiceCollection.BuildServiceProvider' in 'ConfigureServices'

            connectedServices.ServiceProvider = services.BuildServiceProvider();

#pragma warning restore ASP0000 // Do not call 'IServiceCollection.BuildServiceProvider' in 'ConfigureServices'

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
                    // BotChatActivityLogger.UpdateRaSaData(exception.Message, 0, "");

                    // BotChatActivityLogger.UpdateResponseJsonText(string.Empty);
                    // BotChatActivityLogger.UpdateSource(ResponseSource.Rasa);
                    // await BotChatActivityLogger.LogActivityCustom(activity, connectionString);
                    //  await context.SendActivityAsync("Error occured..!!.");
                    await context.SendActivityAsync("Sorry, I could not understand. Could you please rephrase the query.	");
                    //  await context.SendActivityAsync(exception.Message);

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
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(MyAllowSpecificOrigins);

            var aspnetEnv = Configuration.GetValue<string>("Environment");

            var isProduction = string.Equals(aspnetEnv, "production", StringComparison.OrdinalIgnoreCase);

            if (!isProduction)
            {
                app.UseDeveloperExceptionPage();

                app.UseCookiePolicy(new CookiePolicyOptions
                {
                    HttpOnly = HttpOnlyPolicy.Always,
                    Secure = CookieSecurePolicy.SameAsRequest,
                    MinimumSameSitePolicy = SameSiteMode.Strict
                });
            }
            else
            {
                app.UseExceptionHandler("/Error");

                app.UseCookiePolicy(new CookiePolicyOptions
                {
                    HttpOnly = HttpOnlyPolicy.Always,
                    Secure = CookieSecurePolicy.Always,
                    MinimumSameSitePolicy = SameSiteMode.Strict
                });
            }

            app.UseDefaultFiles()
               .UseStaticFiles()
               .UseBotFramework()
               .UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapDefaultControllerRoute();
            });
        }

        #endregion
    }
}
