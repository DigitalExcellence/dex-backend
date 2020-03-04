using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Digital_Excellence.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Digital_Excellence.Extensions
{
	/// <summary>
	/// AutoMapperExtensions
	/// </summary>
	public static class AutoMapperExtensions
	{
		/// <summary>
		/// Adds the autoMapper.
		/// </summary>
		/// <param name="services">The services.</param>
		/// <returns></returns>
		public static IServiceCollection AddAutoMapper(this IServiceCollection services)
		{
			// Auto Mapper Configurations
			var mappingConfig = new MapperConfiguration(mc =>
			{
				mc.AddProfile(new MappingProfile());
			});

			IMapper mapper = mappingConfig.CreateMapper();
			services.AddSingleton(mapper);

			return services;
		}
	}
}
