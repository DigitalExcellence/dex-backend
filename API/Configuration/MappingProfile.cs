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
            CreateMap<LikedProjectByUser, UserProjectLikeResourceResult>()
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

            CreateMap<UserResource, User>();
            CreateMap<User, UserResourceResult>();
            CreateMap<User, LimitedUserResourceResult>();

            CreateMap<ProjectResource, Project>();
            CreateMap<Project, ProjectResourceResult>();
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

            CreateMap<EmbeddedProjectResource, EmbeddedProject>();
            CreateMap<EmbeddedProject, EmbeddedProjectResourceResult>();

            CreateMap<RoleScopeResource, RoleScope>();
            CreateMap<RoleScope, RoleScopeResource>();
        }
    }
}
