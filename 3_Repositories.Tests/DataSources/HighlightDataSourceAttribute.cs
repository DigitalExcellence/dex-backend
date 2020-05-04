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
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public class HighlightDataSourceAttribute : Attribute, IParameterDataSource
    {
        private readonly IFakeDataGenerator<Highlight> fakeDataGenerator;
        private readonly int amountToGenerate = 0;

        public HighlightDataSourceAttribute()
        {
            fakeDataGenerator = new HighlightDataGenerator();
        }

        public HighlightDataSourceAttribute(int amount) : this()
        {
            amountToGenerate = amount;
        }

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
