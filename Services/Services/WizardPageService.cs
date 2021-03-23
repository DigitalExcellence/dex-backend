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
using Repositories;
using Services.Base;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Services
{

    /// <summary>
    ///     The wizard page service
    /// </summary>
    public interface IWizardPageService : IService<WizardPage>
    {

        /// <summary>
        ///     This method validates whether all the specified wizard page ids exists.
        /// </summary>
        /// <param name="wizardPageIds">A collection of wizard page ids that get validated on their existence.</param>
        /// <returns>This method returns true if all the wizard pages exist, otherwise it will return false.</returns>
        Task<bool> ValidateWizardPagesExist(IEnumerable<int> wizardPageIds);

    }

    /// <summary>
    ///     The wizard page service
    /// </summary>
    public class WizardPageService : Service<WizardPage>, IWizardPageService
    {

        /// <summary>
        ///     The wizard page service constructor
        /// </summary>
        /// <param name="repository"></param>
        public WizardPageService(IWizardPageRepository repository) : base(repository) { }

        /// <summary>
        ///     The repository
        /// </summary>
        protected new IWizardPageRepository Repository => (IWizardPageRepository) base.Repository;

        /// <summary>
        ///     This method validates whether all the specified wizard page ids exists.
        /// </summary>
        /// <param name="wizardPageIds">A collection of wizard page ids that get validated on their existence.</param>
        /// <returns>This method returns true if all the wizard pages exist, otherwise it will return false.</returns>
        public async Task<bool> ValidateWizardPagesExist(IEnumerable<int> wizardPageIds)
        {
            IEnumerable<WizardPage> wizardPages = await Repository.GetRange(wizardPageIds);
            return wizardPageIds.Count() == wizardPages.Count();
        }

    }

}
