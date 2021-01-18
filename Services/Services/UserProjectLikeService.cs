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
using Repositories.Base;
using Services.Base;
using System;
using System.Threading.Tasks;

namespace Services.Services
{

    public interface IUserProjectLikeService : IService<ProjectLike>
    {
        bool CheckIfUserAlreadyLiked(int userId, int projectId);
        Task SyncProjectToES(Project project);
    }

    public class UserProjectLikeService : Service<ProjectLike>,
                                          IUserProjectLikeService
    {
        IProjectRepository projectRepository;
        public UserProjectLikeService(IUserProjectLikeRepository repository, IProjectRepository projectRepository) :
            base(repository)
        {
            this.projectRepository = projectRepository;
        }

        private new IUserProjectLikeRepository Repository =>
            (IUserProjectLikeRepository) base.Repository;

        public override void Add(ProjectLike projectEntity)
        {
            Repository.Add(projectEntity);
        }

        public override void Remove(ProjectLike projectEntity)
        {
            Repository.Remove(projectEntity);
            
        }

        public async Task SyncProjectToES(Project project)
        {
             await projectRepository.SyncProjectToES(project);
        }
        bool IUserProjectLikeService.CheckIfUserAlreadyLiked(int userId, int projectId)
        {
            if(Repository.CheckIfUserAlreadyLiked(userId, projectId))
            {
                return true;
            }

            return false;
        }

    }

}
