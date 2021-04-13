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

using AutoMapper;
using Models;
using Services.ExternalDataProviders;
using Services.ExternalDataProviders.Resources;

namespace Services.Tests.Helpers
{

    /// <summary>
    ///     This class is a helper class for creating the IMapper interface in the service test project.
    /// </summary>
    public static class AutoMapperServiceTestHelper
    {

        /// <summary>
        ///     This method is responsible for creating and returning the IMapper interface.
        /// </summary>
        /// <returns>This method returns the IMapper interface.</returns>
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
