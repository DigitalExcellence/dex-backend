using Models;
using Repositories;
using Services.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Services
{
    /// <summary>
    /// This is the project comment like service interface
    /// </summary>
    public interface IUserProjectCommentLikeService : IService<ProjectCommentLike>
    {

        /// <summary>
        ///     This is the interface method which checks if the user already like a comment
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="projectCommentId"></param>
        /// <returns>Boolean</returns>
        bool CheckIfUserAlreadyLiked(int userId, int projectCommentId);

    }
    public class UserProjectCommentLikeService : Service<ProjectCommentLike>, IUserProjectCommentLikeService
    {
        private readonly IProjectCommentRepository projectCommentRepository;
        public UserProjectCommentLikeService(IUserProjectCommentLikeRepository repository, IProjectCommentRepository projectCommentRepository) : base(repository)
        {
            this.projectCommentRepository = projectCommentRepository;

        }
        private new IUserProjectCommentLikeRepository Repository => (IUserProjectCommentLikeRepository) base.Repository;

        bool IUserProjectCommentLikeService.CheckIfUserAlreadyLiked(int userId, int projectCommentId)
        {
            if(Repository.CheckIfUserAlreadyLiked(userId, projectCommentId))
            {
                return true;
            }
            return false;
        }
    }
}
