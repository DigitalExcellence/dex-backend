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
            CreateMap<Project, AutocompleteProjectResourceResult>();
            CreateMap<AutocompleteProjectResourceResult, Project>();

            CreateMap<ProjectLike, UserProjectLikeResourceResult>()
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

            CreateMap<UserUserResourceResult, UserUser>();

            CreateMap<UserUser, UserUserResourceResult>()
                .ForMember(q => q.Id, opt => opt.MapFrom(q => q.FollowedUser.Id))
                .ForMember(q => q.Name, opt => opt.MapFrom(q => q.FollowedUser.Name))
                .ForAllOtherMembers(o => o.Ignore());

            CreateMap<UserProject, UserProjectResourceResult>()
                .ForMember(q => q.Id, opt => opt.MapFrom(p => p.Project.Id))
                .ForMember(q => q.Name, opt => opt.MapFrom(p => p.Project.Name))
                .ForMember(q => q.ShortDescription, opt => opt.MapFrom(p => p.Project.ShortDescription))
                .ForMember(q => q.Uri, opt => opt.MapFrom(p => p.Project.Uri))
                .ForMember(q => q.Description, opt => opt.MapFrom(p => p.Project.Description))
                .ForAllOtherMembers(o => o.Ignore());


            CreateMap<User, UserResourceResult>()
                .ForMember(q => q.UserTask, opt => opt.MapFrom(q => q.UserTasks))
                .ForMember(q => q.ExpectedGraduationDateTime, opt => opt.MapFrom(q => q.ExpectedGraduationDate));

            CreateMap<UserResource, User>()
                .ForMember(q => q.ExpectedGraduationDate, opt => opt.MapFrom(q => q.ExpectedGraduationDateTime));

            CreateMap<User, LimitedUserResourceResult>();

            CreateMap<ProjectResource, Project>()
                .ForMember(q => q.Categories, opt => opt.Ignore());
            CreateMap<Project, ProjectResourceResult>();
            CreateMap<ProjectLike, ProjectLikesResourceResult>();
            CreateMap<Project, ProjectHighlightResourceResult>();

            CreateMap<CollaboratorResource, Collaborator>();
            CreateMap<Collaborator, CollaboratorResourceResult>();

            CreateMap<Project, ProjectResultResource>();

            CreateMap<ProjectFilterParamsResource, ProjectFilterParams>();

            CreateMap<HighlightResource, Highlight>();
            CreateMap<Highlight, HighlightResourceResult>()
                .ForMember(e => e.Project,
                           opt => opt.MapFrom(d => d.Project));

            CreateMap<RoleResource, Role>();
            CreateMap<Role, RoleResourceResult>();

            CreateMap<CategoryResource, Category>();
            CreateMap<Category, CategoryResourceResult>();
            CreateMap<ProjectCategoryResource, ProjectCategory>();
            CreateMap<ProjectCategory, ProjectCategoryResourceResult>()
                .ForMember(q => q.Id, opt => opt.MapFrom(q=> q.Category.Id))
                .ForMember(q => q.Name, opt => opt.MapFrom(q => q.Category.Name));

            CreateMap<EmbeddedProjectResource, EmbeddedProject>();
            CreateMap<EmbeddedProject, EmbeddedProjectResourceResult>();

            CreateMap<FileResourceResult, File>();
            CreateMap<File, FileResourceResult>()
                .ForMember(e => e.UploaderUserId,
                           opt => opt.MapFrom(e => e.Uploader.Id));

            CreateMap<RoleScopeResource, RoleScope>();
            CreateMap<RoleScope, RoleScopeResource>();

            CreateMap<InstitutionResource, Institution>();
            CreateMap<Institution, InstitutionResourceResult>();

            CreateMap<UserTask, UserTaskResourceResult>()
                .ForMember(e => e.UserResourceResult,
                           opt => opt.MapFrom(d => d.User))
                .ForMember(e => e.Id, opt => opt.MapFrom(e => e.Id))
                .ForMember(e => e.Status, opt => opt.MapFrom(e => e.Status))
                .ForMember(e => e.Type, opt => opt.MapFrom(e => e.Type));
            CreateMap<Project, WizardProjectResourceResult>();

            CreateMap<IDataSourceAdaptee, DataSourceResourceResult>()
                .ForMember(dest => dest.WizardPages, opt => opt.MapFrom(src => src.DataSourceWizardPages));

            CreateMap<DataSourceResource, DataSource>()
                .ForMember(dest => dest.DataSourceWizardPages, opt => opt.MapFrom(src => src.WizardPageResources));
            CreateMap<DataSourceWizardPageResource, DataSourceWizardPage>();

            CreateMap<DataSourceWizardPage, WizardPageResourceResult>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.WizardPage.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.WizardPage.Name));

            CreateMap<DataSource, DataSourceResourceResult>()
                .ForMember(dest => dest.WizardPages, opt => opt.MapFrom(src => src.DataSourceWizardPages));
            CreateMap<IDataSourceAdaptee, DataSource>();

            CreateMap<OauthTokens, OauthTokensResourceResult>();

            CreateMap<CallToActionResource, CallToAction>()
                .ForMember(dest => dest.OptionValue, opt => opt.MapFrom(src => src.OptionValue.ToLower()));
            CreateMap<CallToAction, CallToActionResourceResult>();

            CreateMap<CallToActionOptionResource, CallToActionOption>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToLower()))
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Value.ToLower()));
            CreateMap<CallToActionOption, CallToActionOptionResourceResult>();

            CreateMap<WizardPageResource, WizardPage>();
            CreateMap<WizardPage, WizardPageResourceResult>();

            CreateMap<DataSourceWizardPage, DataSourceWizardPageResourceResult>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.WizardPage.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.WizardPage.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.WizardPage.Description));

            CreateMap<ProjectInstitution, ProjectInstitutionResourceResult>()
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
