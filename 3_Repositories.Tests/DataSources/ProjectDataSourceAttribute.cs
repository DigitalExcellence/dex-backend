using _3_Repositories.Tests.DataGenerators;
using _3_Repositories.Tests.DataGenerators.Base;
using Models;
using NUnit.Framework.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace _3_Repositories.Tests.DataSources
{
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public class ProjectDataSourceAttribute : Attribute, IParameterDataSource
    {
        private readonly IFakeDataGenerator<Project> _fakeDataGenerator;
        private readonly int _amountToGenerate = 0;

        public ProjectDataSourceAttribute()
        {
            _fakeDataGenerator = new ProjectDataGenerator();
        }

        public ProjectDataSourceAttribute(int amount) : this()
        {
            _amountToGenerate = amount;
        }

        public IEnumerable GetData(IParameterInfo parameter)
        {
            if(_amountToGenerate <= 1)
            {
                return new[] { _fakeDataGenerator.Generate() };
            }
            List<Project> projects = _fakeDataGenerator.GenerateRange(_amountToGenerate).ToList();
            return new[] { projects };
        }
    }
}
