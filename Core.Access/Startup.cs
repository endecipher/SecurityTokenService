using Core.Access.Encryption;
using Core.Access.Identity.DB;
using Core.Access.Models;
using Core.Access.Models.Strategy;
using Core.Access.Utility;
using Core.Access.Utility.Authentication;
using Core.Access.Utility.Claims;
using Core.Access.Utility.ConfigParser;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Core.Access
{
    public class Startup
    {
        public static IConfiguration Configuration { get; private set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(Strings.AuthenticationSchemes.Default)
                .AddJwtBearer(Strings.AuthenticationSchemes.Default, options =>
                {
                    options.Events = new JwtBearerEvents()
                    {
                        OnMessageReceived = context =>
                        {
                            if (context.Request.Query.ContainsKey(Strings.Common.access_token))
                            {
                                context.Token = context.Request.Query[Strings.Common.access_token];
                            }

                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddControllersWithViews().AddRazorRuntimeCompilation();

            services.AddDbContext<AccessDbContext>(optionsBuilder =>
            {
                var connectionString = Configuration.GetConnectionString("DefaultConnection");

                optionsBuilder.UseSqlServer(connectionString, sqlOptionsBuilder =>
                {
                    sqlOptionsBuilder.EnableRetryOnFailure();
                });
            });

            services
                .AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<AccessDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 1;
                options.Password.RequiredUniqueChars = 0;

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";

                options.User.RequireUniqueEmail = false;
            });

            services.AddScoped<IRepository, Repository>();
            services.AddScoped<ICustomExecutionStrategy, CustomExecutionStrategy>();
            services.AddScoped<ITransaction, Transaction>();

            services.AddScoped<IHttpHelper, HttpHelper>();
            services.AddScoped<IClaimManager, ClaimManager>();
            services.AddScoped<IJsonWebTokenManager, JsonWebTokenManager>();
            services.AddScoped<IUtilities, Utilities>();
            services.AddScoped<IConfigParser, ConfigParser>();
            services.AddScoped<IEncryptionHelper, EncryptionHelper>();

            services.AddScoped<IStrategy<OAuthModel, OnAuthAttemptContext>, AuthAttemptStrategy>();
            services.AddScoped<IStrategy<OAuthModel, OnAuthCodeGenerationContext>, CodeGenerationStrategy>();

            services.AddScoped<IStrategy<OAuthModel, OnTokenExchangeContext>, TokenExchangeStrategy>();
            services.AddScoped<IStrategy<OAuthModel, OnTokenValidationContext>, TokenValidationStrategy>();

            services.AddScoped<IStrategy<UserModel, OnUserGenerationContext>, UserGenerationStrategy>();
            services.AddScoped<IStrategy<ClientModel, OnClientGenerationContext>, ClientGenerationStrategy>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IOptions<JwtBearerOptions> jwtOptions, IJsonWebTokenManager jsonWebTokenManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage(new DeveloperExceptionPageOptions
                {
                    SourceCodeLineCount = 20,
                });
            }

            app.UseStaticFiles();

            app.Use(async (httpContext, next) =>
            {
                if (httpContext != null && jwtOptions.Value.TokenValidationParameters?.IssuerSigningKey == null)
                {
                    jwtOptions.Value.TokenValidationParameters = jsonWebTokenManager.TokenValidationParameters;
                }

                await next.Invoke();
            });

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
