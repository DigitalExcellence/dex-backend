using Bogus;
using Bogus.DataSets;
using Models;
using Repositories.Tests.DataGenerators;
using Repositories.Tests.DataGenerators.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repositories.Tests.DataGenerators
{
    /// <summary>
    /// FakeDataGenerator for the roles
    /// </summary>
    public class RoleDataGenerator : FakeDataGenerator<Role>
    {
        /// <summary>
        /// Initializes the RoleDataGenerator
        /// and define dataGenerator options
        /// </summary>
        public RoleDataGenerator()
        {
            List<RoleScope> roleScopes = new List<RoleScope>();
            for(int i = 0; i < 10; i++)
            {
                RoleScope roleScope = new RoleScope(new Faker().Random.String2(10));
                roleScopes.Add(roleScope);
            }

            Faker = new Faker<Role>()
                    .RuleFor(role => role.Scopes, roleScopes)
                    .RuleFor(role => role.Name, faker => faker.Random.String2(10));
        }
    }
}
