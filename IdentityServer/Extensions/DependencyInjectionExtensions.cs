using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Repositories;
using Services.DataProviders;
using Services.Services;

namespace IdentityServer
{

    public static class DependencyInjectionExtensions
    {

        public static IServiceCollection AddServicesAndRepositories(this IServiceCollection services)
        {
            services.AddScoped<DbContext, IdentityDbContext>();
            services.AddScoped<IIdentityUserService, IdentityUserService>();
            services.AddScoped<IIdentityUserRepository, IdentityUserRepository>();

            //services.AddScoped<IDataProviderService, DataProviderService>();
            //services.AddScoped<IDataProviderLoader, DataProviderLoader>();

            return services;
        }

    }

}
