using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Models;
using Repositories.Base;

namespace Repositories
{
	public interface IProjectRepository : IRepository<Project>
	{
	}

	public class ProjectRepository : Repository<Project>, IProjectRepository
	{
		public ProjectRepository(DbContext dbContext) : base(dbContext)
		{
		}
	}
}
