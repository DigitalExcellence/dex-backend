using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Digital_Excellence.Resources;
using Models;

namespace Digital_Excellence.Configuration
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMap<UserResource, User>();
			CreateMap<User, UserResourceResult>();

			CreateMap<ProjectResource, Project>();
			CreateMap<Project, ProjectResourceResult>();

		}

	}
}
