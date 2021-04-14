using Models;
using NUnit.Framework.Interfaces;
using Repositories.Tests.DataGenerators;
using Repositories.Tests.DataGenerators.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Repositories.Tests.DataSources
{
    /// <summary>
    ///     Attribute to generate ProjectInstitutions
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public class ProjectInstitutionDataSourceAttribute : Attribute, IParameterDataSource
    {
        private readonly int amountToGenerate;
        private readonly IFakeDataGenerator<ProjectInstitution> fakeDataGenerator;

        /// <summary>
        ///     Initializes ProjectInstitutionDataSource
        /// </summary>
        public ProjectInstitutionDataSourceAttribute()
        {
            fakeDataGenerator = new ProjectInstitutionDataGenerator();
        }

        /// <summary>
        ///     Initializes ProjectInstitutionDatasource
        ///     and setting the amount of projects to be generated
        /// </summary>
        public ProjectInstitutionDataSourceAttribute(int amount) : this()
        {
            amountToGenerate = amount;
        }

        /// <summary>
        ///     Generate the data and return it
        /// </summary>
        /// <param name="parameter">Extra parameters given in the attribute, not in use but required due to inheritance</param>
        /// <returns>The generated data</returns>
        public IEnumerable GetData(IParameterInfo parameter)
        {
            if(amountToGenerate < 1)
                return new[] { fakeDataGenerator.Generate() };

            return new[] { fakeDataGenerator.GenerateRange(amountToGenerate) };
        }
    }
}
