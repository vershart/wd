using System;
using System.IO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using MySql.Data.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using Sludinajumi.Api.Auth;
using Sludinajumi.Api.Data;
using Sludinajumi.Api.Models;
using static Sludinajumi.Api.Helpers.Strings;

namespace Sludinajumi
{
    public class Startup
    {

        private const string SecretKey = "202cb962ac59075b964b07152d234b70";
        private readonly SymmetricSecurityKey _signingKey = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(SecretKey));

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            var builder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddDbContext<SludinajumiContext>(options =>
                options.UseSqlite(Configuration.GetConnectionString("Sqlite")));

            services.AddSingleton<IJwtFactory, JwtFactory>();

            var jwtAppSettingOptions = Configuration.GetSection(nameof(JwtIssuerOptions));

            services.Configure<JwtIssuerOptions>(options =>
            {
                options.Issuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                options.Audience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)];
                options.SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256);
            });

            services.AddAuthorization(options => {
                options.AddPolicy(
                    "ApiUser",
                    policy => policy.RequireClaim(ClaimIdentifiers.Id, Claims.ApiAccess)
                );
            });

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<SludinajumiContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 9;
                options.Password.RequiredUniqueChars = 7;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromHours(1);
                options.Lockout.MaxFailedAccessAttempts = 10;
                options.Lockout.AllowedForNewUsers = true;

                options.User.RequireUniqueEmail = true;

                //options.SignIn.RequireConfirmedEmail = true;
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)],
                        ValidateAudience = true,
                        ValidAudience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)],
                        ValidateIssuerSigningKey = true,
                        RequireExpirationTime = false,
                        ValidateLifetime = false,
                        ClockSkew = TimeSpan.Zero,
                        IssuerSigningKey = _signingKey,
                    };
                }
            );

            services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireAdministratorRole", policy => policy.RequireRole("Administrator"));
                options.AddPolicy("RequireConfirmedOrganization", policy => policy.RequireRole("ConfirmedOrganization"));
            });

            services.AddMvc();

            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.AreaViewLocationFormats.Clear();
                options.AreaViewLocationFormats.Add("/Manage/Views/{1}/{0}.cshtml");
                options.AreaViewLocationFormats.Add("/Manage/Views/Shared/{0}.cshtml");
                options.AreaViewLocationFormats.Add("/Views/Shared/{0}.cshtml");
            });

            services.AddSwaggerGen(c =>
            {

                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "Sludinājumi API",
                    Description = "Sludinājumi API paplašinātā dokumentācija",
                    TermsOfService = "AS IS",
                });
                
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var xmlPath = Path.Combine(basePath, "Sludinajumi.xml"); 
                c.IncludeXmlComments(xmlPath);

            });    
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {

            loggerFactory
                .AddConsole(LogLevel.Debug)
                .AddDebug(LogLevel.Debug);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }

            app.UseAuthentication();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sludinājumi API v1");
            });


            app.UseStaticFiles();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "manageArea", 
                    template: "{area:exists}/{controller=Dashboard}/{action=Index}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Account}/{action=RegisterAdministrator}/");
            });

        }
    }
}
