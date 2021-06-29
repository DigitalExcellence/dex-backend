/*
* Digital Excellence Copyright (C) 2020 Brend Smits
*
* This program is free software: you can redistribute it and/or modify
* it under the terms of the GNU Lesser General Public License as published
* by the Free Software Foundation version 3 of the License.
*
* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty
* of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
* See the GNU Lesser General Public License for more details.
*
* You can find a copy of the GNU Lesser General Public License
* along with this program, in the LICENSE.md file in the root project directory.
* If not, see https://www.gnu.org/licenses/lgpl-3.0.txt
*/

using API.Common;
using API.HelperClasses;
using Data;
using MessageBrokerPublisher;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Repositories;
using Services.ExternalDataProviders;
using Services.ExternalDataProviders.Helpers;
using Repositories.ElasticSearch;
using Services.Resources;
using Services.Services;
using Services.Sources;

namespace API.Extensions
{

    /// <summary>
    ///     DependencyInjectionExtensions
    /// </summary>
    public static class DependencyInjectionExtensions
    {

        /// <summary>
        ///     Adds all the services and repositories.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <returns>The services.</returns>
        public static IServiceCollection AddServicesAndRepositories(this IServiceCollection services)
        {
            services.AddScoped<DbContext, ApplicationDbContext>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<IProjectRepository, ProjectRepository>();

            services.AddScoped<IHighlightService, HighlightService>();
            services.AddScoped<IHighlightRepository, HighlightRepository>();

            services.AddScoped<ISearchService, SearchService>();

            services.AddScoped<IEmbedService, EmbedService>();
            services.AddScoped<IEmbedRepository, EmbedRepository>();

            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IRoleRepository, RoleRepository>();

            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();

            services.AddScoped<IProjectCategoryService, ProjectCategoryService>();
            services.AddScoped<IProjectCategoryRepository, ProjectCategoryRepository>();

            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IFileRepository, FileRepository>();

            services.AddScoped<IFileUploader, FileUploader>();

            services.AddScoped<IUserTaskService, UserTaskService>();
            services.AddScoped<IUserTaskRepository, UserTaskRepository>();

            services.AddScoped<IAuthorizationHandler, ScopeRequirementHandler>();

            services.AddScoped<IRestClientFactory, RestClientFactory>();

            services.AddScoped<ISourceManagerService, SourceManagerService>();
            services.AddScoped<IGitHubSource, GitHubSource>();
            services.AddScoped<IGitLabSource, GitLabSource>();

            services.AddScoped<IInstitutionService, InstitutionService>();
            services.AddScoped<IInstitutionRepository, InstitutionRepository>();

            services.AddScoped<IAuthorizationHelper, AuthorizationHelper>();
            services.AddScoped(typeof(IIndexOrderHelper<>), typeof(IndexOrderHelper<>));

            services.AddScoped<IUserProjectService, UserProjectService>();
            services.AddScoped<IUserProjectRepository, UserProjectRepository>();

            services.AddScoped<IUserUserService, UserUserService>();
            services.AddScoped<IUserUserRepository, UserUserRepository>();

            services.AddScoped<IDataProviderService, DataProviderService>();

            services.AddScoped<IDataSourceModelService, DataSourceModelService>();
            services.AddScoped<IDataSourceModelRepository, DataSourceModelRepository>();

            services.AddScoped<IDataProviderLoader, DataProviderLoader>();

            services.AddScoped<IWizardPageService, WizardPageService>();
            services.AddScoped<IWizardPageRepository, WizardPageRepository>();

            services.AddScoped<IUserProjectLikeService, UserProjectLikeService>();
            services.AddScoped<IUserProjectLikeRepository, UserProjectLikeRepository>();

            services.AddScoped<ICallToActionOptionService, CallToActionOptionService>();
            services.AddScoped<ICallToActionOptionRepository, CallToActionOptionRepository>();

            services.AddSingleton<Queries>();
            services.AddSingleton<ITaskPublisher, TaskPublisher>();

            services.AddSingleton<IAssemblyHelper, AssemblyHelper>();
          
            services.AddScoped<IProjectInstitutionService, ProjectInstitutionService>();
            services.AddScoped<IProjectInstitutionRepository, ProjectInstitutionRepository>();
          
            services.AddExternalDataSources();

            return services;
          
        }

        private static void AddExternalDataSources(this IServiceCollection services)
        {
            services.AddScoped<GithubDataSourceAdaptee>();
            services.AddScoped<GitlabDataSourceAdaptee>();
            services.AddScoped<JsFiddleDataSourceAdaptee>();
            services.AddScoped<FontysGitlabDataSourceAdaptee>();

            services.AddScoped<IUserProjectLikeService, UserProjectLikeService>();
            services.AddScoped<IUserProjectLikeRepository, UserProjectLikeRepository>();

            services.AddScoped<ICallToActionOptionService, CallToActionOptionService>();
            services.AddScoped<ICallToActionOptionRepository, CallToActionOptionRepository>();

            services.AddSingleton<ITaskPublisher, TaskPublisher>();
        }

    }

}
