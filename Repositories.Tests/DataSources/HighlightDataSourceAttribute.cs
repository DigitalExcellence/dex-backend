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
    /// Attribute to generate highlights
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public class HighlightDataSourceAttribute : Attribute, IParameterDataSource
    {
        private readonly IFakeDataGenerator<Highlight> fakeDataGenerator;
        private readonly int amountToGenerate = 0;

        /// <summary>
        /// Initializes highlightDataSourceAttribute
        /// </summary>
        public HighlightDataSourceAttribute()
        {
            fakeDataGenerator = new HighlightDataGenerator();
        }

        /// <summary>
        /// Initializes highlightDataSourceAttribute
        /// and setting the amount of highlights to be generated
        /// </summary>
        /// <param name="amount">Amount of highlights to be produced.</param>
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
