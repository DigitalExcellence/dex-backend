using AngleSharp.Dom;
using Data;
using Data.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Models;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.HelperClasses
{
    public static class SeedHelper
    {

        public static void InsertRoles(List<Role> seededRoles, ApplicationDbContext context)
        {
            List<Role> rolesInDb = context.Role.AsQueryable()
                                          .Include(s => s.Scopes).ToList();

            foreach(Role entityInSeed in seededRoles)
            {
                if(rolesInDb.Find(e => e.Name == entityInSeed.Name) == null)
                {
                    context.Role.Add(entityInSeed);
                    continue;
                }

                Role foundEntity = rolesInDb.Find(e => e.Name == entityInSeed.Name);
                
                List<RoleScope> roleScopesToAdd = InsertRoleScopesNotInDb(entityInSeed.Scopes, foundEntity.Scopes, context);
                foundEntity.Scopes.AddRange(roleScopesToAdd);
                context.Role.Update(foundEntity);
                
            }
            context.SaveChanges();
        }

        public static List<RoleScope> InsertRoleScopesNotInDb(List<RoleScope> seededRoleScope,
                                                   List<RoleScope> roleScopeInDb,
                                                   ApplicationDbContext context)
        {
            List<RoleScope> roleScopesNotInDb = new List<RoleScope>();

            

            foreach(RoleScope entityInSeed in seededRoleScope)
            {
                if(roleScopeInDb == null)
                {
                    roleScopesNotInDb.Add(entityInSeed);
                }
                else if(roleScopeInDb.Find(e => e.Scope == entityInSeed.Scope) == null)
                {
                    roleScopesNotInDb.Add(entityInSeed);
                }
            }

            return roleScopesNotInDb;
        }


        /*public void SynchronizeSeedWithDb(List<TEntity> entitiesInSeed)
        {
            List<TEntity> entitiesNotInDb = new List<TEntity>();
            List<TEntity> entitiesInDb = dbSet.ToList();

            foreach(TEntity entityInSeed in entitiesInSeed)
            {
                if(!entitiesInDb.Contains(entityInSeed))
                {
                    entitiesNotInDb.Add(entityInSeed);
                }
            }

            dbSet.AddRange(entitiesNotInDb);
        }

        public void SynchronizeSeedWithDb(TEntity entityInSeed)
        {
            List<TEntity> entitiesInDb = dbSet.ToList();

            if(!entitiesInDb.Contains(entityInSeed))
            {
                dbSet.Add(entityInSeed);
            }
        }*/

    }
}
