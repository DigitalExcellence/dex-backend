using Models;
using NUnit.Framework.Interfaces;
using Repositories.Tests.DataGenerators;
using Repositories.Tests.DataGenerators.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Repositories.Tests.DataSources
{

    /// <summary>
    ///     Attribute to generate tags
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public class TagDataSourceAttribute : Attribute, IParameterDataSource
    {

        private readonly int amountToGenerate;
        private readonly IFakeDataGenerator<Tag> fakeDataGenerator;

        /// <summary>
        ///     Initializes tagDataSourceAttribute
        /// </summary>
        public TagDataSourceAttribute()
        {
            fakeDataGenerator = new TagDataGenerator();
        }

        /// <summary>
        ///     Initializes tagyDataSourceAttribute
        ///     and setting the amount of tags to be generated
        /// </summary>
        public TagDataSourceAttribute(int amount) : this()
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
            if(amountToGenerate <= 1)
            {
                return new[] {fakeDataGenerator.Generate()};
            }
            List<Tag> tags = fakeDataGenerator.GenerateRange(amountToGenerate)
                                                .ToList();
            return new[] {tags};
        }

    }

}
