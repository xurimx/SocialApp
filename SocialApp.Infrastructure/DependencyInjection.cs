using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SocialApp.Application.Common.Interfaces;
using SocialApp.Application.Common.Interfaces.Repositories;
using SocialApp.Infrastructure.Data;
using SocialApp.Infrastructure.Identity;
using SocialApp.Infrastructure.Repositories;
using SocialApp.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialApp.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<SocialUserContext>(options =>
            {
                //options.UseInMemoryDatabase("dev");
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
                //options.UseLazyLoadingProxies();
            });

            services.AddScoped<ISocialUserContext>(provider => provider.GetService<SocialUserContext>());
            services.AddScoped<ISocialUserRepository, SocialUserRepository>();
            services.AddScoped<IIdentityService, IdentityService>();

            services.AddScoped<IDomainEventService, DomainEventService>();

            services.AddIdentityCore<ApplicationUser>()
                .AddEntityFrameworkStores<SocialUserContext>();

            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = "socialidentity";
                    options.ApiName = "SocialApi";
                    options.RequireHttpsMetadata = false;
                });

            return services;
        }
    }
}
