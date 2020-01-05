using DataTransferObject.Comment;
using DataTransferObject.Common;
using DataTransferObject.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class Comment
    {
        private readonly DataAccess.Query.CommentQ _Repository;
        private readonly DataAccess.Query.IdeaQ _RepositoryIdea;
        //-------------------------------------------------------------------------------------------------
        public Comment()
        {
            _Repository = new DataAccess.Query.CommentQ();
            _RepositoryIdea = new DataAccess.Query.IdeaQ();
        }
        //-------------------------------------------------------------------------------------------------
        public IEnumerable<IdeaCommentsDto> GetAllComments(int ideaId)
        {
            return _Repository.GetAllComments(ideaId);
        }

        //-------------------------------------------------------------------------------------------------
        public Result AddCommentToIdea(CommentDto newcomment)
        {
            if (_RepositoryIdea.IsIdeaLocked(newcomment.IDEA_ID))
            {
                return new Result()
                {
                    value = false,
                    content = "ایده توسط کمیته بررسی شده و یا وجود ندارد و موارد وابسته به آن قابل تغییر نیست"
                };
            }
            else
                return _Repository.AddCommentToIdea(newcomment);
        }

        //-------------------------------------------------------------------------------------------------
        public Result UpdateComment(CommentDto newcomment)
        {
            if (_RepositoryIdea.IsIdeaLocked(newcomment.IDEA_ID))
            {
                return new Result()
                {
                    value = false,
                    content = "ایده توسط کمیته بررسی شده و یا وجود ندارد و موارد وابسته به آن قابل تغییر نیست"
                };
            }
            else
                return _Repository.UpdateComment(newcomment);
        }
        //-------------------------------------------------------------------------------------------------
        public Result VoteToComment(VoteToCommentDto vote)
        {
            if (vote.POINT < 0)
            {
                vote.POINT = -1;
            }
            else if (vote.POINT > 0)
            {
                vote.POINT = 1;
            }
            else
            {
                vote.POINT = 0;
            }

            if (_RepositoryIdea.IsIdeaLocked(vote.IDEA_ID))
            {
                return new Result()
                {
                    value = false,
                    content = "ایده توسط کمیته بررسی شده و یا وجود ندارد و موارد وابسته به آن قابل تغییر نیست"
                };
            }
            else
                return _Repository.VoteToComment(vote);
        }
    }
}
