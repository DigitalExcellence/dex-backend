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

namespace Models
{
    /// <summary>
    /// This class works as the link between the data source and the wizard page.
    /// </summary>
    public class DataSourceWizardPage
    {

        /// <summary>
        /// Gets or sets a value for the DataSource property.
        /// </summary>
        public DataSource DataSource { get; set; }

        /// <summary>
        /// Gets or sets a value for the DataSourceId property.
        /// </summary>
        public int DataSourceId { get; set; }

        /// <summary>
        /// Gets or sets a value for the WizardPage property.
        /// </summary>
        public WizardPage WizardPage { get; set; }

        /// <summary>
        /// Gets or sets a value for the WizardPageId property.
        /// </summary>
        public int WizardPageId { get; set; }

        /// <summary>
        /// Gets or sets a value for the AuthFlow property.
        /// </summary>
        public bool AuthFlow { get; set; }

        /// <summary>
        /// Gets or sets a value for the OrderIndex property.
        /// </summary>
        public int OrderIndex { get; set; }

    }

}
