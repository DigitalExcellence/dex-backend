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
    /// Attribute to generate projects
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public class HighlightDataSourceAttribute : Attribute, IParameterDataSource
    {
        private readonly IFakeDataGenerator<Highlight> fakeDataGenerator;
        private readonly int amountToGenerate = 0;

        /// <summary>
        /// Initializes collaboratorDataSourceAttribute
        /// </summary>
        public HighlightDataSourceAttribute()
        {
            fakeDataGenerator = new HighlightDataGenerator();
        }

        /// <summary>
        /// Initializes projectDataSourceAttribute
        /// and setting the amount of projects to be generated
        /// </summary>
        public HighlightDataSourceAttribute(int amount) : this()
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
            List<Highlight> highlights = fakeDataGenerator.GenerateRange(amountToGenerate).ToList();
            return new[] { highlights };
        }
    }
}
