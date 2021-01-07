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

    public interface IUserTaskService : IService<UserTask>
    {
        public Task<List<UserTask>> GetAllOpenGraduateUserTasks(int withinAmountOfMonths);
        
        public Task<List<UserTask>> GetUserTasksForUser(int userId);

    }

    public class UserTaskService : Service<UserTask>, IUserTaskService
    {
        private readonly IUserService userService;
        protected new IUserTaskRepository Repository => (IUserTaskRepository) base.Repository;

        public UserTaskService(IUserTaskRepository repository, IUserService userService) : base(repository)
        {
           this.userService = userService;
        }

        

        public async Task<List<UserTask>> GetAllOpenGraduateUserTasks(int withinAmountOfMonths)
        {
            List<User> users = userService.GetAllExpectedGraduatingUsers(6);
            IEnumerable<UserTask> allUserTasks = await Repository.GetAll();
            List<UserTask> userTasks = new List<UserTask>();

            foreach(User u in users)
            {
               bool doesExist = false;
                foreach(UserTask userTask in allUserTasks)
                {
                    if(u.Id == userTask.User.Id)
                    {
                        if(userTask.Status == UserTaskStatus.Open && userTask.Type == UserTaskType.GraduationReminder)
                        {
                            userTasks.Add(userTask);
                            doesExist = true;
                        }
                        if(userTask.Status == UserTaskStatus.Completed)
                        {
                            doesExist = true;
                        }
                        if(userTask.Status == UserTaskStatus.Mailed)
                        {
                            doesExist = true;
                        }
                    }
                }
                if(!doesExist)
                {
                    UserTask userTask = new UserTask(u, UserTaskType.GraduationReminder);
                    Add(userTask);
                    userTasks.Add(userTask);
                }
            }

            Save();

            return userTasks;
        }

        public async Task<List<UserTask>> GetUserTasksForUser(int userId)
        {
            return await Repository.GetUserTasksForUser(userId);
        }
    }
}
