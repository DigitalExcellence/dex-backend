using Microsoft.EntityFrameworkCore;
using Models;
using Repositories.Base;

namespace Repositories
{

    public interface IDataSourceWizardPageRepository : IRepository<DataSourceWizardPage>
    {


    }

    public class DataSourceWizardPageRepository : Repository<DataSourceWizardPage>, IDataSourceWizardPageRepository
    {

        public DataSourceWizardPageRepository(DbContext dbContext) : base(dbContext) { }

    }

}
