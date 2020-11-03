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

using Ganss.XSS;
using Models;
using Repositories;
using Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Services.Services
{

    public interface ICallToActionService : IService<CallToAction>
    {
        public Task<List<CallToAction>> GetAllGraduateCallToActions();
    }

    public class CallToActionService : Service<CallToAction>, ICallToActionService
    {
        private readonly IUserService userService;
        protected new ICallToActionRepository Repository => (ICallToActionRepository) base.Repository;

        public CallToActionService(ICallToActionRepository repository, IUserService userService) : base(repository)
        {
           this.userService = userService;
        }

        

        async Task<List<CallToAction>> ICallToActionService.GetAllGraduateCallToActions()
        {
            List<User> users = userService.GetAllExpectedGraduatingUsers();
            IEnumerable<CallToAction> allCallToActions = await Repository.GetAll();
            DateTime now = DateTime.Now;
            DateTime max = DateTime.Now.AddMonths(6);

            List<CallToAction> callToActions = new List<CallToAction>();

            foreach(CallToAction callToAction in users.Where(user => user.ExpectedGraduationDate <= max &&
                                                                     user.ExpectedGraduationDate >= now)
                                                      .SelectMany(user => from cta in allCallToActions
                                                                          where user.Id == cta.UserId && cta.Type == 0
                                                                          select new CallToAction(user.Id, CallToActionType.graduationReminder)))
            {
                callToActions.Add(callToAction);

                Add(callToAction);
            }
            Save();

            return callToActions;
        }
    }
}
