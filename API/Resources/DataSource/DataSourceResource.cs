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

using System.Collections.Generic;

namespace API.Resources
{

    /// <summary>
    ///     The view model of a data source.
    /// </summary>
    public class DataSourceResource
    {

        /// <summary>
        ///     Gets or Set the Title of the data source.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        ///     Gets or sets the Description of the data source.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///     Gets or sets the visibility of the data source.
        /// </summary>
        public bool IsVisible { get; set; }

        /// <summary>
        ///     Gets or sets the icon id of the data source.
        /// </summary>
        public int IconId { get; set; }

        /// <summary>
        ///     Gets or sets the wizard page resources for the data source.
        /// </summary>
        public IEnumerable<DataSourceWizardPageResource> WizardPageResources { get; set; }

    }

}
