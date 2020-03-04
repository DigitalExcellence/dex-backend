using System;
using System.Collections.Generic;
using System.Text;
using Models;
using Repositories;
using Services.Base;

namespace Services.Services
{


	public interface IProjectService : IService<Project>
	{
	}

	public class ProjectService : Service<Project>, IProjectService
	{
		protected new IProjectRepository Repository => (IProjectRepository)base.Repository;

		public ProjectService(IProjectRepository repository) : base(repository)
		{
		}
	}
}
