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

using API.Resources;
using AutoMapper;
using Models;
using Services.ExternalDataProviders;
using Services.ExternalDataProviders.Resources;
using System.Collections.Generic;

namespace API.Configuration
{

    /// <summary>
    ///     This profiles adds every resource mapping.
    /// </summary>
    public class MappingProfile : Profile
    {

        /// <summary>
        ///     Create a map for every resource mapping.
        /// </summary>
        public MappingProfile()
        {
            CreateMap<Project, AutocompleteProjectOutput>();
            CreateMap<AutocompleteProjectOutput, Project>();

            CreateMap<ProjectLike, UserProjectLikeOutput>()
                .ForMember(source => source.Id,
                           option => option
                               .MapFrom(destination =>
                                            destination.LikedProject.Id))
                .ForMember(source => source.Name,
                           option => option
                               .MapFrom(destination =>
                                            destination.LikedProject.Name))
                .ForMember(source => source.ShortDescription,
                           option => option
                               .MapFrom(destination =>
                                            destination.LikedProject.ShortDescription))
                .ForMember(source => source.Uri,
                           option => option
                               .MapFrom(destination =>
                                            destination.LikedProject.Uri))
                .ForMember(source => source.Description,
                           option => option
                               .MapFrom(destination =>
                                            destination.LikedProject.Description))
                .ForAllOtherMembers(member => member.Ignore());

            CreateMap<UserUserOutput, UserUser>();

            CreateMap<UserUser, UserUserOutput>()
                .ForMember(q => q.Id, opt => opt.MapFrom(q => q.FollowedUser.Id))
                .ForMember(q => q.Name, opt => opt.MapFrom(q => q.FollowedUser.Name))
                .ForAllOtherMembers(o => o.Ignore());

            CreateMap<UserProject, UserProjectOutput>()
                .ForMember(q => q.Id, opt => opt.MapFrom(p => p.Project.Id))
                .ForMember(q => q.Name, opt => opt.MapFrom(p => p.Project.Name))
                .ForMember(q => q.ShortDescription, opt => opt.MapFrom(p => p.Project.ShortDescription))
                .ForMember(q => q.Uri, opt => opt.MapFrom(p => p.Project.Uri))
                .ForMember(q => q.Description, opt => opt.MapFrom(p => p.Project.Description))
                .ForAllOtherMembers(o => o.Ignore());


            CreateMap<User, UserOutput>()
                .ForMember(q => q.UserTask, opt => opt.MapFrom(q => q.UserTasks))
                .ForMember(q => q.ExpectedGraduationDateTime, opt => opt.MapFrom(q => q.ExpectedGraduationDate));

            CreateMap<UserInput, User>()
                .ForMember(q => q.ExpectedGraduationDate, opt => opt.MapFrom(q => q.ExpectedGraduationDateTime));

            CreateMap<User, LimitedUserOutput>();

            CreateMap<ProjectInput, Project>()
                .ForMember(q => q.Categories, opt => opt.Ignore());
            CreateMap<Project, ProjectOutput>();
            CreateMap<ProjectLike, ProjectLikesOutput>();
            CreateMap<Project, ProjectHighlightOutput>();

            CreateMap<CollaboratorInput, Collaborator>();
            CreateMap<Collaborator, CollaboratorOutput>();

            CreateMap<Project, ProjectResultInput>();

            CreateMap<ProjectFilterParamsInput, ProjectFilterParams>();

            CreateMap<HighlightInput, Highlight>();
            CreateMap<Highlight, HighlightOutput>()
                .ForMember(e => e.Project,
                           opt => opt.MapFrom(d => d.Project));

            CreateMap<RoleInput, Role>();
            CreateMap<Role, RoleOutput>();

            CreateMap<CategoryInput, Category>();
            CreateMap<Category, CategoryOutput>();
            CreateMap<ProjectCategoryInput, ProjectCategory>();
            CreateMap<ProjectCategory, ProjectCategoryOutput>()
                .ForMember(q => q.Id, opt => opt.MapFrom(q=> q.Category.Id))
                .ForMember(q => q.Name, opt => opt.MapFrom(q => q.Category.Name));

            CreateMap<EmbeddedProjectInput, EmbeddedProject>();
            CreateMap<EmbeddedProject, EmbeddedProjectOutput>();

            CreateMap<FileOutput, File>();
            CreateMap<File, FileOutput>()
                .ForMember(e => e.UploaderUserId,
                           opt => opt.MapFrom(e => e.Uploader.Id));

            CreateMap<RoleScopeInput, RoleScope>();
            CreateMap<RoleScope, RoleScopeInput>();

            CreateMap<InstitutionInput, Institution>();
            CreateMap<Institution, InstitutionOutput>();

            CreateMap<UserTask, UserTaskOutput>()
                .ForMember(e => e.UserResourceResult,
                           opt => opt.MapFrom(d => d.User))
                .ForMember(e => e.Id, opt => opt.MapFrom(e => e.Id))
                .ForMember(e => e.Status, opt => opt.MapFrom(e => e.Status))
                .ForMember(e => e.Type, opt => opt.MapFrom(e => e.Type));
            CreateMap<Project, WizardProjectOutput>();

            CreateMap<IDataSourceAdaptee, DataSourceOutput>()
                .ForMember(dest => dest.WizardPages, opt => opt.MapFrom(src => src.DataSourceWizardPages));

            CreateMap<DataSourceInput, DataSource>()
                .ForMember(dest => dest.DataSourceWizardPages, opt => opt.MapFrom(src => src.WizardPageResources));
            CreateMap<DataSourceWizardPageInput, DataSourceWizardPage>();

            CreateMap<DataSourceWizardPage, WizardPageOutput>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.WizardPage.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.WizardPage.Name));

            CreateMap<DataSource, DataSourceOutput>()
                .ForMember(dest => dest.WizardPages, opt => opt.MapFrom(src => src.DataSourceWizardPages));
            CreateMap<IDataSourceAdaptee, DataSource>();

            CreateMap<OauthTokens, OauthTokensOutput>();

            CreateMap<CallToActionInput, CallToAction>()
                .ForMember(dest => dest.OptionValue, opt => opt.MapFrom(src => src.OptionValue.ToLower()));
            CreateMap<CallToAction, CallToActionOutput>();

            CreateMap<CallToActionOptionInput, CallToActionOption>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToLower()))
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Value.ToLower()));
            CreateMap<CallToActionOption, CallToActionOptionOutput>();

            CreateMap<WizardPageInput, WizardPage>();
            CreateMap<WizardPage, WizardPageOutput>();

            CreateMap<DataSourceWizardPage, DataSourceWizardPageOutput>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.WizardPage.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.WizardPage.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.WizardPage.Description));

            CreateMap<ProjectInstitution, ProjectInstitutionOutput>()
                .ForMember(dest => dest.InstititutionName, opt => opt.MapFrom(src => src.Institution.Name))
                .ForMember(dest => dest.ProjectName, opt => opt.MapFrom(src => src.Project.Name));


            CreateExternalSourceMappingProfiles();
        }

        private void CreateExternalSourceMappingProfiles()
        {
            CreateMap<JsFiddleDataSourceResourceResult, Project>()
                .ForMember(d => d.Name, opt => opt.MapFrom(m => m.Title));

            CreateMap<GithubDataSourceResourceResult, Project>()
                .ForMember(dest => dest.ShortDescription, opt => opt.MapFrom(src => src.Description));

            CreateMap<GitlabDataSourceResourceResult, Project>()
                .ForMember(dest => dest.ShortDescription, opt => opt.MapFrom(src => src.Description));

        }

    }

}
