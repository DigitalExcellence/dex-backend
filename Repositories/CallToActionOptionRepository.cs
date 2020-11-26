using Microsoft.EntityFrameworkCore;
using Models;
using Repositories.Base;

namespace Repositories
{

    public interface ICallToActionOptionRepository
    {



    }

    public class CallToActionOptionRepository : Repository<CallToActionOption>, ICallToActionOptionRepository
    {

        public CallToActionOptionRepository(DbContext dbContext) : base(dbContext) { }

    }

}
