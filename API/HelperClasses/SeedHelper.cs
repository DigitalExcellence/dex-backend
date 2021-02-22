using Data;
using Microsoft.EntityFrameworkCore;
using Models;
using System.Collections.Generic;
using System.Linq;

namespace API.HelperClasses
{

    /// <summary>
    ///     This class checks if the seed matches the data in the database. If it doesn't match, it updates the database.
    /// </summary>
    public static class SeedHelper
    {

        /// <summary>
        ///     This method check if roles in the seed match the roles in the database. If they don't match, the roles are updated
        ///     or added.
        /// </summary>
        /// <param name="seededRoles"></param>
        /// <param name="context"></param>
        public static void InsertRoles(List<Role> seededRoles, ApplicationDbContext context)
        {
            List<Role> rolesInDb = context.Role.AsQueryable()
                                          .Include(s => s.Scopes)
                                          .ToList();

            foreach(Role entityInSeed in seededRoles)
            {
                if(rolesInDb.Find(e => e.Name == entityInSeed.Name) == null)
                {
                    context.Role.Add(entityInSeed);
                    continue;
                }

                Role foundEntity = rolesInDb.Find(e => e.Name == entityInSeed.Name);

                List<RoleScope> roleScopesToAdd = FindRoleScopesNotInDb(entityInSeed.Scopes, foundEntity.Scopes);
                foundEntity.Scopes.AddRange(roleScopesToAdd);
                context.Role.Update(foundEntity);
            }
            context.SaveChanges();
        }

        /// <summary>
        ///     This method checks if the role scopes for a specific role match the ones in the seed. If they don't they are added
        ///     to a list and being returned.
        /// </summary>
        /// <param name="seededRoleScope"></param>
        /// <param name="roleScopeInDb"></param>
        /// <returns></returns>
        public static List<RoleScope> FindRoleScopesNotInDb(List<RoleScope> seededRoleScope,
                                                            List<RoleScope> roleScopeInDb)
        {
            return seededRoleScope
                   .Where(entityInSeed => roleScopeInDb?.Find(e => e.Scope == entityInSeed.Scope) == null)
                   .ToList();
        }

        /// <summary>
        ///     This method checks if the seeded user is already in the database. The user should match identityId and role. If it
        ///     does not match, the user is updated or added.
        /// </summary>
        /// <param name="seedUser"></param>
        /// <param name="context"></param>
        public static void InsertUser(User seedUser, ApplicationDbContext context)
        {
            List<User> usersInDb = context.User.AsQueryable()
                                          .Include(e => e.Role)
                                          .ToList();

            if(usersInDb.Find(e => e.IdentityId == seedUser.IdentityId) != null)
            {
                User foundEntity = usersInDb.Find(e => e.IdentityId == seedUser.IdentityId);
                foundEntity.Role = seedUser.Role;
                context.Update(foundEntity);
                context.SaveChanges();
                return;
            }

            context.User.Add(seedUser);
            context.SaveChanges();
        }

    }

}
