using API.Resources;
using AutoMapper;
using Models;
using Search;

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

			CreateMap<CollaboratorResource, Collaborator>();
			CreateMap<Collaborator, CollaboratorResourceResult>();

			CreateMap<Project, SearchResultResource>();

			CreateMap<SearchResult, SearchResultResource>();
		}

	}
}
