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

namespace API.Resources
{

    /// <summary>
    ///     The view model for the wizard page in the data source. This model differs from the wizard page resource model
    ///     by having the order index and auth flow property and the wizard page id refers to the model of the wizard page
    ///     resource model.
    /// </summary>
    public class DataSourceWizardPageResource
    {

        /// <summary>
        ///     Gets or sets the wizard page id. This references a wizard page model.
        /// </summary>
        public int WizardPageId { get; set; }

        /// <summary>
        ///     Gets or sets the order index of the wizard page in this data source.
        /// </summary>
        public int OrderIndex { get; set; }

        /// <summary>
        ///     Gets or sets the auth flow of the wizard page in this data source.
        /// </summary>
        public bool AuthFlow { get; set; }

    }

}
