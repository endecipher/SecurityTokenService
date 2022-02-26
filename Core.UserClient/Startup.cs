using Core.UserClient.Common;
using Core.UserClient.Data.DB;
using Core.UserClient.Encryption;
using Core.UserClient.Handlers;
using Core.UserClient.Policies.AdultRequirement;
using Core.UserClient.Policies.OAuthRequirement;
using Core.UserClient.Policies.SecurityLevelRequirement;
using Core.UserClient.Utility;
using Core.UserClient.Utility.Requests;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Core.UserClient
{
    public partial class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public static IConfiguration Configuration { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            services.AddHttpClient();

            services.AddAuthorization((options) =>
            {
                options.InvokeHandlersAfterFailure = true;

                options.AddPolicy(Utility.Policies.AdultPolicy, (builder) => builder.AddRequirements(new AdultRequirement()).Build());
                options.AddPolicy(Utility.Policies.HighestSecurityPolicy, (builder) => builder.AddRequirements(SecurityLevelRequirement.Highest()).Build());
                options.AddPolicy(Utility.Policies.OAuthSignedInPolicy, (builder) => builder.AddRequirements(new OAuthRequirement()).Build());
            });

            services.AddAuthentication(config =>
            {
                config.DefaultAuthenticateScheme = AuthenticationSchemes.Cookie;
                config.DefaultSignInScheme = AuthenticationSchemes.Cookie;
                config.DefaultChallengeScheme = AuthenticationSchemes.OAuth;
            })
            .AddCookie(AuthenticationSchemes.Cookie, options =>
            {
                options.LoginPath = new PathString("/User/SignIn");
                options.AccessDeniedPath = new PathString("/User/AccessDenied");
            })
            .AddOAuth<OAuthOptions, CustomOAuthHandler>(AuthenticationSchemes.OAuth, config =>
            {
                config.AuthorizationEndpoint = Configuration["AuthorizationServers:Core.Access:AuthorizationEndpoint"];
                config.TokenEndpoint = Configuration["AuthorizationServers:Core.Access:TokenEndpoint"];
                config.CallbackPath = Configuration["AuthorizationServers:Core.Access:InternalCallbackPath"];
                config.ClientId = Configuration["AuthorizationServers:Core.Access:Client_Id"];
                config.ClientSecret = Configuration["AuthorizationServers:Core.Access:Client_Secret"];
                config.SaveTokens = true;

                config.Events = new OAuthEvents
                {
                    OnCreatingTicket = context =>
                    {
                        var claims = JsonConvert.DeserializeObject<Dictionary<string, string>>(Helper
                            .GetJSONSerializedPayloadFromAccessToken(context.AccessToken));

                        List<Claim> claimsList = new List<Claim>();

                        foreach (var claim in claims)
                        {
                            Claim claimVal = new Claim(claim.Key, claim.Value);
                            claimsList.Add(claimVal);
                        }

                        if (context.Principal != null)
                        {
                            context.Principal = new ClaimsPrincipal();
                        }

                        context.Principal.AddIdentity(new ClaimsIdentity(claims: claimsList, authenticationType: AuthenticationSchemes.OAuth));

                        context.HttpContext.SignInAsync(scheme: AuthenticationSchemes.Cookie, principal: context.Principal, properties: new AuthenticationProperties
                        {
                            AllowRefresh = true,
                            IssuedUtc = System.DateTimeOffset.Now,
                            ExpiresUtc = System.DateTimeOffset.Now.AddMinutes(2),
                            IsPersistent = true,
                        }).Wait();

                        return Task.CompletedTask;
                    },
                };
            });

            /*
             * AddControllersWithViews covers MVC, and AddMvc covers both MVC and Razor Pages.
             */
            services.AddControllersWithViews((mvcOptions) =>
            {
            }).AddRazorRuntimeCompilation();


            services.AddScoped<IGeneralHttpClient, GeneralHttpClient>();

            services.AddDbContext<ClientDbContext>((optionsBuilder) =>
            {
                var connectionString = Configuration.GetConnectionString("DefaultConnection");

                optionsBuilder.UseSqlServer(connectionString, sqlOptionsBuilder =>
                {
                    sqlOptionsBuilder.EnableRetryOnFailure();
                });
            });

            services.AddScoped<IRepository, Repository>();
            services.AddSingleton<IJoker, Joker>();
            services.AddScoped<IEncryptionHelper, EncryptionHelper>();

            services.AddScoped<IAuthorizationHandler, SecurityLevelHandler>();
            services.AddScoped<IAuthorizationHandler, AdultRequirementHandler>();
            services.AddScoped<IAuthorizationHandler, OAuthRequirementHandler>();
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
