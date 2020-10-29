using Microsoft.EntityFrameworkCore;
using Models;
using Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repositories
{

    /// <summary>
    /// The institution repository interface
    /// </summary>
    /// <seealso cref="IRepository{Institution}" />
    public interface IInstitutionRepository : IRepository<Institution>
    {

        /// <summary>
        /// This method returns all the institutions.
        /// </summary>
        /// <returns>This method returns a collection of institutions.</returns>
        Task<IEnumerable<Institution>> GetInstitutionsAsync();

    }

    /// <summary>
    /// The implementation for the institution repository
    /// </summary>
    /// <seealso cref="IInstitutionRepository" />
    /// <seealso cref="Repository{Institution}" />
    public class InstitutionRepository : Repository<Institution>, IInstitutionRepository
    {

        public InstitutionRepository(DbContext dbContext) : base(dbContext) { }

        public async Task<IEnumerable<Institution>> GetInstitutionsAsync()
        {
            return await GetDbSet<Institution>()
                       .ToListAsync();
        }

    }

}
