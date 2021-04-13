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

        /// <summary>
        ///     This method checks if the data sources has a collection of wizard pages. If not,
        ///     they will get added to the data source. This is just for testing purposes and does
        ///     not have to get extended when new data sources are added.
        /// </summary>
        /// <param name="context"></param>
        public static void SeedDataSourceWizardPages(ApplicationDbContext context)
        {
            foreach(DataSource dataSource in context.DataSource)
            {
                if(dataSource.DataSourceWizardPages == null || !dataSource.DataSourceWizardPages.Any())
                {
                    dataSource.DataSourceWizardPages = new List<DataSourceWizardPage>();
                    // Github
                    if(dataSource.Guid == "de38e528-1d6d-40e7-83b9-4334c51c19be")
                    {
                        if(context.WizardPage.FirstOrDefault(p => p.Id == 2) != null)
                            dataSource.DataSourceWizardPages.Add(new DataSourceWizardPage
                                                             {
                                                                 AuthFlow = false,
                                                                 DataSourceId = dataSource.Id,
                                                                 OrderIndex = 1,
                                                                 WizardPageId = 2
                                                             });
                    }
                    // Gitlab
                    else if(dataSource.Guid == "66de59d4-5db0-4bf8-a9a5-06abe8d3443a")
                    {
                        if(context.WizardPage.FirstOrDefault(p => p.Id == 2) != null)
                            dataSource.DataSourceWizardPages.Add(new DataSourceWizardPage
                                                             {
                                                                 AuthFlow = false,
                                                                 DataSourceId = dataSource.Id,
                                                                 OrderIndex = 1,
                                                                 WizardPageId = 2
                                                             });
                    }
                    // JsFiddle
                    else if(dataSource.Guid == "96666870-3afe-44e2-8d62-337d49cf972d")
                    {
                        if(context.WizardPage.FirstOrDefault(p => p.Id == 1) != null)
                            dataSource.DataSourceWizardPages.Add(new DataSourceWizardPage
                                                             {
                                                                 AuthFlow = false,
                                                                 DataSourceId = dataSource.Id,
                                                                 OrderIndex = 1,
                                                                 WizardPageId = 1
                                                             });

                        if(context.WizardPage.FirstOrDefault(p => p.Id == 3) != null)
                            dataSource.DataSourceWizardPages.Add(new DataSourceWizardPage
                                                             {
                                                                 AuthFlow = false,
                                                                 DataSourceId = dataSource.Id,
                                                                 OrderIndex = 2,
                                                                 WizardPageId = 3
                                                             });
                    }
                }
            }
        }

    }

}
