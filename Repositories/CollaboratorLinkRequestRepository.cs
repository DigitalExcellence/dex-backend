using Microsoft.EntityFrameworkCore;
using Models;
using Repositories.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repositories
{
    public interface ICollaboratorLinkRequestRepository : IRepository<CollaboratorLinkRequest>
    {

    }

    public class CollaboratorLinkRequestRepository : Repository<CollaboratorLinkRequest>, ICollaboratorLinkRequestRepository
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="CollaboratorLinkRequestRepository" /> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public CollaboratorLinkRequestRepository(DbContext dbContext) : base(dbContext) { }
    }
}
