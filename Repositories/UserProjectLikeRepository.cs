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

using Microsoft.EntityFrameworkCore;
using Models;
using Repositories.Base;
using System.Linq;

namespace Repositories
{

    public interface IUserProjectLikeRepository : IRepository<LikedProjectByUser>
    {

        bool CheckIfUserAlreadyLiked(int userId, int projectId);

    }

    public class UserProjectLikeRepository : Repository<LikedProjectByUser>,
                                             IUserProjectLikeRepository
    {
        public UserProjectLikeRepository(DbContext dbContext) :
            base(dbContext) { }

        public override void Add(LikedProjectByUser likedProjectByUser)
        {
            DbContext.Add(likedProjectByUser);
        }

        public override void Remove(LikedProjectByUser likedProjectByUser)
        {
            LikedProjectByUser likedProjectToRemove = GetDbSet
                    <LikedProjectByUser>()
                .SingleOrDefault(project => project.UserId ==
                                            likedProjectByUser.CreatorOfProject.Id &&
                                            project.LikedProject.Id ==
                                            likedProjectByUser.LikedProject.Id);

            GetDbSet<LikedProjectByUser>()
                .Remove(likedProjectToRemove);
        }

        bool IUserProjectLikeRepository
            .CheckIfUserAlreadyLiked(int userId, int projectId)
        {
            LikedProjectByUser likedProjectByUser = GetDbSet<LikedProjectByUser>()
                .SingleOrDefault(project =>
                                     (project.UserId == userId) && project.LikedProject.Id == projectId);

            if(likedProjectByUser != null)
            {
                return true;
            }
            return false;
        }
    }

}
