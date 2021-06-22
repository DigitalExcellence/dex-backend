using MessageBrokerPublisher;
using Microsoft.EntityFrameworkCore;
using Models;
using Repositories.Base;
using System.Linq;

namespace Repositories
{
    public interface IUserProjectCommentLikeRepository: IRepository<ProjectCommentLike>
{
        bool CheckIfUserAlreadyLiked(int userId, int projectCommentId);
}
    public class UserProjectCommentLikeRepository : Repository<ProjectCommentLike>, IUserProjectCommentLikeRepository
    {
        private ITaskPublisher taskPublisher;
        public UserProjectCommentLikeRepository(DbContext dbContext, ITaskPublisher taskPublisher) : base(dbContext)
        {
                this.taskPublisher = taskPublisher;
        }
        public override void Add(ProjectCommentLike projectCommentLike)
        {
            DbContext.Add(projectCommentLike);
        }

         bool IUserProjectCommentLikeRepository.CheckIfUserAlreadyLiked(int userId, int projectCommentId)
        {
            ProjectCommentLike projectCommentLike = GetDbSet<ProjectCommentLike>()
                .SingleOrDefault(comment => comment.CommentLiker.Id == userId && comment.LikedComment.Id == projectCommentId);
            if(projectCommentLike != null)
            {
                return true;
            }
            return false;
        }
    }
}
