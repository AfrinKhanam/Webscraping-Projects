using BOTAIML.ChatBot.DirectLineServer.Core;
using BOTAIML.ChatBot.DirectLineServer.Core.Authentication;
using BOTAIML.ChatBot.DirectLineServer.Core.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DirectLine
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDirectLine(Configuration.GetSection("DirectLine").Get<DirectLineSettings>());
            services.AddAuthentication()
                .AddDirectLine(Configuration.GetSection("Jwt").Get<DirectLineAuthenticationOptions>());
            services.AddAuthorization();

            services.AddControllers().AddNewtonsoftJson(); //.AddApplicationPart(typeof(DirectLineController).Assembly);

            services.Configure<ForwardedHeadersOptions>(opts =>
            {
                opts.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto | ForwardedHeaders.XForwardedHost;
                opts.KnownNetworks.Clear();
                opts.KnownProxies.Clear();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseForwardedHeaders();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            //app.UseHttpsRedirection();
            app.UseCookiePolicy();
            app.UseStaticFiles();
            app.UseDirectLineCors();

            app.UseRouting();

            //Security headers
            app.UseHsts(hsts => hsts.MaxAge(hours: 8).IncludeSubdomains());
            app.UseXContentTypeOptions();
            app.UseReferrerPolicy(opts => opts.NoReferrer());
            app.UseXXssProtection(options => options.EnabledWithBlockMode());
            app.UseXfo(options => options.SameOrigin());
            app.UseCsp(opts => opts
            .BlockAllMixedContent()
            .FrameAncestors(s => s.Self())
            );

            app.Use((context, next) => {
                context.Response.Headers.Add("Feature-Policy", new[]
                {
                    "geolocation 'none'",
                    "midi 'none'",
                    "notifications 'none'",
                    "push 'none'",
                    "sync-xhr '*'",
                    "microphone 'none'",
                    "camera 'none'",
                    "magnetometer 'none'",
                    "gyroscope 'none'",
                    "speaker 'self'",
                    "vibrate 'none'",
                    "fullscreen 'self'",
                    "payment 'none'"
                });
                return next.Invoke();
            });

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseDirectLineCore();
            app.UseDirectLineUploadsStatic();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
