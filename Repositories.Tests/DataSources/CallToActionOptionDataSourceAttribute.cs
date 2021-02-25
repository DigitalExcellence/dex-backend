/*
* Digital Excellence Copyright (C) 2020 Brend Smits
*
* This program is free software: you can redistribute it and/or modify
* it under the terms of the GNU Lesser General Public License as published
* by the Free Software Foundation version 3 of the License.
*
* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty
* of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
* See the GNU Lesser General Public License for more details.
*
* You can find a copy of the GNU Lesser General Public License
* along with this program, in the LICENSE.md file in the root project directory.
* If not, see https://www.gnu.org/licenses/lgpl-3.0.txt
*/

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
    ///     This class is an attribute used for generating call to action options.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public class CallToActionOptionDataSourceAttribute : Attribute, IParameterDataSource
    {

        private readonly int amountToGenerate;

        private readonly IFakeDataGenerator<CallToActionOption> fakeDataGenerator;

        /// <summary>
        ///     This method initialized the CallToActionOptionDataSourceAttribute class.
        /// </summary>
        public CallToActionOptionDataSourceAttribute()
        {
            fakeDataGenerator = new CallToActionOptionDataGenerator();
        }

        public CallToActionOptionDataSourceAttribute(int amount)
            : this()
        {
            amountToGenerate = amount;
        }

        /// <summary>
        ///     This method generates the data and returns it.
        /// </summary>
        /// <param name="parameter">Extra parameter given in the attribute, not in use but required due to inheritance.</param>
        /// <returns>This method returns the generated data.</returns>
        public IEnumerable GetData(IParameterInfo parameter)
        {
            if(amountToGenerate <= 1)
            {
                return new[] {fakeDataGenerator.Generate()};
            }

            List<CallToActionOption> options = fakeDataGenerator.GenerateRange(amountToGenerate)
                                                                .ToList();
            return new[] {options};
        }

    }

}
