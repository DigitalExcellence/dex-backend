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

using System;
using System.Collections.Generic;

namespace Models
{

    /// <summary>
    ///     This class contains all the properties for the wizard page. The wizard page
    ///     is an object that will get used in the frontend to figure out the order
    ///     of the pages that should get shown.
    /// </summary>
    public class WizardPage
    {

        /// <summary>
        ///     Gets or sets a value for the Id property.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     Gets or sets a value for the Name property.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets a value for the Description property.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///     Gets or sets a value for the CreatedAt property.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        ///     Gets or sets a value for the UpdatedAt property.
        /// </summary>
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        ///     Gets or sets a value for the DataSourceWizardPages property. This property represents the
        ///     link table between the data source table and the wizard pages table, resulting in the
        ///     many-to-many relation.
        /// </summary>
        public IList<DataSourceWizardPage> DataSourceWizardPages { get; set; }

    }

}
