using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Repositories;
using Services.Services;

namespace API.Extensions
{
    /// <summary>
    /// DependencyInjectionExtensions
    /// </summary>
    public static class DependencyInjectionExtensions
    {
        /// <summary>
        /// Adds all the services and repositories.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <returns></returns>
        public static IServiceCollection AddServicesAndRepositories(this IServiceCollection services)
        {
            services.AddScoped<DbContext, ApplicationDbContext>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<IProjectRepository, ProjectRepository>();

            return services;
        }
    }
}