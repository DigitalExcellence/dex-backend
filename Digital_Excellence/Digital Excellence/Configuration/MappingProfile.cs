﻿using API.Resources;
using API.Resources.Project;
using AutoMapper;
using Models;

namespace API.Configuration
{
	/// <summary>
	/// This profiles adds every resource mapping.
	/// </summary>
	public class MappingProfile : Profile
	{
		/// <summary>
		/// Create a map for every resource mapping.
		/// </summary>
		public MappingProfile()
		{
			CreateMap<UserResource, User>();
			CreateMap<User, UserResourceResult>();

			CreateMap<ProjectResource, Project>();
			CreateMap<Project, ProjectResourceResult>();

			CreateMap<CreateProjectResource, Project>();
			CreateMap<Project, CreateProjectResource>();

            CreateMap<CreateProjectResourceResult, Project>();
            CreateMap<Project, CreateProjectResourceResult>();


        }

	}
}
