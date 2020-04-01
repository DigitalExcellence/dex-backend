using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Models;
using Repositories;
using Search;
using Services.Base;

namespace Services.Services
{
    public interface IProjectService : IService<Project>
    {
        IEnumerable<Project> Search(IEnumerable<string> values);
    }

    public class ProjectService : Service<Project>, IProjectService
    {
        protected new IProjectRepository Repository => (IProjectRepository) base.Repository;

        public ProjectService(IProjectRepository repository) : base(repository)
        {

        }

        public IEnumerable<Project> Search(IEnumerable<string> values)
        {
            IList<Project> found = new List<Project>();

            foreach (string v in values)
            {
                Repository.SearchProject(v);
            }

            return found;
        }
    }
}