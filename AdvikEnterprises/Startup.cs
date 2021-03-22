using AdvikEnterprises.Attributes;
using AdvikEnterprises.Helpers;
using AdvikEnterprises.Services;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace AdvikEnterprises
{
    //https://jasonwatmore.com/post/2019/10/16/aspnet-core-3-role-based-authorization-tutorial-with-example-api
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddCors();
            services.AddControllersWithViews(options =>
            {
                //options.Filters.Add(typeof(AddRandomNumberHeader2Filter));
            });

            // configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            // configure jwt authentication
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            // configure DI for application services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<JwtFilter>();

            //options.Filters.Add(typeof(AddRandomNumberHeader2Filter));
            //services.AddScoped<AddRandomNumberHeader2Filter>();
            services.AddAntiforgery(x => x.HeaderName = "XSRF-TOKEN");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IAntiforgery antiforgery)//, 
        {
            // global cors policy
            //app.UseCors(x => x
            //    .AllowAnyOrigin()
            //    .AllowAnyMethod()
            //    .AllowAnyHeader());
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.Use(next => context =>
            {
                if (context.Request.Path.Value.ToLower().Contains("getall") ||
                    context.Request.Path.Value.ToLower().Contains("authenticate") ||
                    context.Request.Path.Value.ToLower().Contains("getUserdata") ||
                    string.Equals(context.Request.Path.Value, "/", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(context.Request.Path.Value, "/index.html", StringComparison.OrdinalIgnoreCase))
                {
                        // We can send the request token as a JavaScript-readable cookie, and Angular will use it by default.
                        var tokens = antiforgery.GetAndStoreTokens(context);
                    context.Response.Cookies.Append("XSRF-TOKEN", tokens.RequestToken,
                        new CookieOptions() { HttpOnly = false });
                }

                return next(context);
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";
                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });

            //app.Use(next => context =>
            //{
            //    if (context.Request.Path.Value.ToLower().Contains("user") ||
            //        context.Request.Path.Value.ToLower().Contains("weather") ||
            //        string.Equals(context.Request.Path.Value, "/", StringComparison.OrdinalIgnoreCase) ||
            //        string.Equals(context.Request.Path.Value, "/index.html", StringComparison.OrdinalIgnoreCase))
            //    {
            //        // We can send the request token as a JavaScript-readable cookie, and Angular will use it by default.
            //        var tokens = antiforgery.GetAndStoreTokens(context);
            //        context.Response.Cookies.Append("XSRF-TOKEN", tokens.RequestToken,
            //            new CookieOptions() { HttpOnly = false });
            //    }

            //    return next(context);
            //});
        }
    }
}
