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
    [AttributeUsage(AttributeTargets.Parameter)]
    public class ProjectInstitutionDataSourceAttribute : Attribute, IParameterDataSource
    {
        private readonly int amountToGenerate;
        private readonly IFakeDataGenerator<ProjectInstitution> fakeDataGenerator;

        public ProjectInstitutionDataSourceAttribute()
        {
            fakeDataGenerator = new ProjectInstitutionDataGenerator();
        }

        public ProjectInstitutionDataSourceAttribute(int amount) : this()
        {
            amountToGenerate = amount;
        }

        public IEnumerable GetData(IParameterInfo parameter)
        {
            if(amountToGenerate < 1)
                return new[] { fakeDataGenerator.Generate() };

            return new[] { fakeDataGenerator.GenerateRange(amountToGenerate) };
        }
    }
}
