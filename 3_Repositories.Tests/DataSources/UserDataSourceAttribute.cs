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
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public class UserDataSourceAttribute : Attribute, IParameterDataSource
    {
        private readonly IFakeDataGenerator<User> fakeDataGenerator;
        private readonly int amountToGenerate = 0;

        public UserDataSourceAttribute()
        {
            fakeDataGenerator = new UserDataGenerator();
        }

        public UserDataSourceAttribute(int amount) : this()
        {
            amountToGenerate = amount;
        }

        public IEnumerable GetData(IParameterInfo parameter)
        {
            if (amountToGenerate <= 1)
            {
                return new[] { fakeDataGenerator.Generate() };
            }
            List<User> users = fakeDataGenerator.GenerateRange(amountToGenerate).ToList();
            return new[] { users };
        }
    }
}
