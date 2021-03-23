using AutoMapper;
using Models;
using Services.ExternalDataProviders;
using Services.ExternalDataProviders.Resources;

namespace Services.Tests.Helpers
{

    public static class AutoMapperServiceTestHelper
    {

        public static IMapper GetIMapper()
        {
            MapperConfiguration config = new MapperConfiguration(conf =>
            {
                conf.CreateMap<JsFiddleDataSourceResourceResult, Project>()
                    .ForMember(d => d.Name, opt => opt.MapFrom(m => m.Title));

                conf.CreateMap<GithubDataSourceResourceResult, Project>()
                    .ForMember(dest => dest.ShortDescription, opt => opt.MapFrom(src => src.Description));

                conf.CreateMap<GitlabDataSourceResourceResult, Project>()
                    .ForMember(dest => dest.ShortDescription, opt => opt.MapFrom(src => src.Description));

                conf.CreateMap<IDataSourceAdaptee, DataSource>();
            });

            return config.CreateMapper();
        }

    }

}
