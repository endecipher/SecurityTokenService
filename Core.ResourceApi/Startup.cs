using Core.ResourceApi.Common;
using Core.ResourceApi.Protection.Authentication;
using Core.ResourceApi.Protection.Handlers;
using Core.ResourceApi.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Core.ResourceApi
{
    public partial class Startup
    {
        public Startup() { }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(Constants.DefaultScheme)
                .AddScheme<AuthenticationSchemeOptions, DefaultAuthenticationHandler>(Constants.DefaultScheme, (options) => { });

            services.AddAuthorization(config =>
            {
                var policy = new AuthorizationPolicyBuilder()
                            .AddRequirements(new JwtRequirement())
                            .Build();

                config.DefaultPolicy = policy;
            });

            services.AddScoped<IAuthorizationHandler, JwtRequirementHandler>();
            services.AddSingleton<IJoker, Joker>();
            services.AddHttpClient().AddHttpContextAccessor();
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
