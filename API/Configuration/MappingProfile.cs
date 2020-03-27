using API.Resources;
using AutoMapper;
using Models;

namespace API.Configuration
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserResource, User>();
            CreateMap<User, UserResourceResult>();

            CreateMap<ProjectResource, Project>();
            CreateMap<Project, ProjectResourceResult>();

            CreateMap<Project, SearchResultResource>();
        }
    }
}