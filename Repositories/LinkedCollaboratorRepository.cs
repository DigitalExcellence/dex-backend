using Repositories.Base;
using Models;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Repositories
{
    public interface ILinkedCollaboratorRepository : IRepository<CollaboratorLinkedUser>
    {

    }

    /// <summary>
    ///     The implementation for the linked collaborator repository
    /// </summary>
    /// <seealso cref="ILinkedCollaboratorRepository" />
    /// <seealso cref="Repository{CollaboratorLinkedUser}" />
    public class LinkedCollaboratorRepository : Repository<CollaboratorLinkedUser>, ILinkedCollaboratorRepository
    {

        /// <summary>
        ///     This is the linked collaborator repository constructor
        /// </summary>
        /// <param name="dbContext"></param>
        public LinkedCollaboratorRepository(DbContext dbContext) : base(dbContext) { }
    }
}
