using Models;
using NUnit.Framework.Interfaces;
using Repositories.Tests.DataGenerators;
using Repositories.Tests.DataGenerators.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repositories.Tests.DataSources
{
    /// <summary>
    /// Attribute to generate roles
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public class RoleDataSourceAttribute : Attribute, IParameterDataSource
    {
        private readonly IFakeDataGenerator<Role> fakeDataGenerator;
        private readonly int amountToGenerate = 0;

        /// <summary>
        /// Initializes roleDataSourceAttribute
        /// </summary>
        public RoleDataSourceAttribute()
        {
            fakeDataGenerator = new RoleDataGenerator();
        }

        /// <summary>
        /// Initializes roleDataSourceAttribute
        /// and setting the amount of users to be generated
        /// </summary>
        public RoleDataSourceAttribute(int amount) : this()
        {
            amountToGenerate = amount;
        }

        /// <summary>
        /// Generate the data and return it
        /// </summary>
        /// <param name="parameter">Extra parameters given in the attribute, not in use but required due to inheritance</param>
        /// <returns>The generated data</returns>
        public IEnumerable GetData(IParameterInfo parameter)
        {
            if(amountToGenerate <= 1)
            {
                return new[] { fakeDataGenerator.Generate() };
            }
            List<Role> roles = fakeDataGenerator.GenerateRange(amountToGenerate).ToList();
            return new[] { roles };
        }
    }
}
