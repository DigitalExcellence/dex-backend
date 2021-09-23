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
using System.Collections.Generic;

namespace Services.ExternalDataProviders
{

    /// <summary>
    ///     The interface for a data source adaptee.
    /// </summary>
    public interface IDataSourceAdaptee
    {

        /// <summary>
        ///     Gets the value for the guid from the data source adaptee.
        /// </summary>
        string Guid { get; }

        /// <summary>
        ///     Defines whether the API requires authentication by default (even for fetching 'public' projects).
        /// </summary>
        bool AlwaysRequiresAuthentication { get; }

        /// <summary>
        ///     Gets or sets a value for the Title property from the data source adaptee.
        /// </summary>
        string Title { get; set; }

        /// <summary>
        ///     Gets the value for the Base Url from the data source adaptee.
        /// </summary>
        string BaseApiUrl { get; set; }

        /// <summary>
        ///     Gets or sets a value for the IsVisible property from the data source adaptee.
        /// </summary>
        public bool IsVisible { get; set; }

        /// <summary>
        ///     Gets or sets a value for the Icon property from the data source adaptee.
        /// </summary>
        public File Icon { get; set; }

        /// <summary>
        ///     Gets or sets a value for the Description property from the data source adaptee.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///     Gets or sets a value for the DataSourceWizardPages property from the data source adaptee.
        /// </summary>
        public IList<DataSourceWizardPage> DataSourceWizardPages { get; set; }

    }

}
