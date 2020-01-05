using DataAccess.Mapper;
using DataTransferObject.Comment;
using DataTransferObject.Common;
using DataTransferObject.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Query
{
    public class CommentQ
    {
        private IdeaManagmentDatabaseEntities _db;
        //-------------------------------------------------------------------------------------------------
        public IEnumerable<IdeaCommentsDto> GetAllComments(int ideaId)
        {
            IEnumerable<IdeaCommentsDto> res=null;
            using (_db = new IdeaManagmentDatabaseEntities())
            {
                res = _db.IDEA_COMMENTS.Where(c => c.IDEA_ID == ideaId).Select(x => new IdeaCommentsDto()
                {
                    ID = x.ID,
                    IDEA_ID =x.IDEA_ID,
                    COMMENT =x.COMMENT,
                    SAVE_DATE =x.SAVE_DATE,
                    USERNAME =x.USERNAME,
                    FullName =x.USER.FIRST_NAME+" "+x.USER.LAST_NAME

                }).ToList();
            }
            return res;
        }

        //-----------------------------------------------------------------------------------------------------------
        public Result AddCommentToIdea(CommentDto newcomment)
        {
            Result result = new Result();

            using (_db = new IdeaManagmentDatabaseEntities())
            {
                var idea = _db.IDEAS.FirstOrDefault(x => x.ID == newcomment.IDEA_ID);
                if (idea == null)
                {
                    result.value = false;
                    result.content = "ایده پیدا نشد";
                    return result;
                }

                var ideaCommentForCheck = _db.IDEA_COMMENTS.FirstOrDefault(x => x.IDEA_ID == newcomment.IDEA_ID && x.USERNAME == newcomment.USERNAME);
                if (ideaCommentForCheck != null)
                {
                    result.value = false;
                    result.content = "قبلا شما پیشنهادی برای این ایده نوشته اید";
                    return result;
                }

                if (idea.USERNAME == newcomment.USERNAME)
                {
                    result.value = false;
                    result.content = "برای ایده خود نمی شود پیشنهاد گذاشت";
                    return result;
                }
   

                IDEA_COMMENTS ideaComment = new IDEA_COMMENTS()
                {
                    COMMENT = newcomment.COMMENT,
                    IDEA_ID = newcomment.IDEA_ID,
                    SAVE_DATE = DateTime.Now,
                    USERNAME = newcomment.USERNAME
                };
                _db.IDEA_COMMENTS.Add(ideaComment);
                _db.SaveChanges();
                result.value = true;
                result.content = "پیشنهاد جدید اضافه شد";
                return result;
            }
        }

        //-----------------------------------------------------------------------------------------------------------
        public Result UpdateComment(CommentDto newcomment)
        {
            Result result = new Result();

            using (_db = new IdeaManagmentDatabaseEntities())
            {
                var idea_ = _db.IDEAS.FirstOrDefault(x => x.ID == newcomment.IDEA_ID);
                if (idea_ == null)
                {
                    result.value = false;
                    result.content = "ایده پیدا نشد";
                    return result;
                }

                var idea_comment = _db.IDEA_COMMENTS.FirstOrDefault(x => x.IDEA_ID == newcomment.IDEA_ID && x.USERNAME == newcomment.USERNAME);
                if (idea_comment == null)
                {
                    result.value = false;
                    result.content = "قبلا شما پیشنهادی برای این ایده ننوشته اید";
                    return result;
                }

                idea_comment.COMMENT = newcomment.COMMENT;
                idea_comment.MODIFY_DATE = DateTime.Now;
                _db.SaveChanges();
                result.value = true;
                result.content = "پیشنهاد اصلاح شد";
                return result;
            }
        }
        //----------------------------------------------------------------------------------------------------------
        public Result VoteToComment(VoteToCommentDto voteDetail)
        {
            Result result = new Result();

            using (_db = new IdeaManagmentDatabaseEntities())
            {
                var comment_point = _db.COMMENT_POINTS.FirstOrDefault(x => x.COMMENT_ID == voteDetail.COMMENT_ID && x.USERNAME == voteDetail.USERNAME);
                if (comment_point == null)
                {
                    var comment = _db.IDEA_COMMENTS.FirstOrDefault(x => x.ID == voteDetail.COMMENT_ID);
                    if (comment == null)
                    {
                        result.value = false;
                        result.content = "پیشنهادی  با این مشخصات وجود ندارد";
                        return result;
                    }
                    var user = _db.USERS.FirstOrDefault(x => x.USERNAME == voteDetail.USERNAME);
                    if (user == null)
                    {
                        result.value = false;
                        result.content = "کاربر پیدا نشد";
                        return result;
                    };
                    if (comment.USERNAME == voteDetail.USERNAME)
                    {
                        result.value = false;
                        result.content = "به پیشنهاد خود نمی توان رای داد";
                        return result;
                    }

                    COMMENT_POINTS comment_point2 = new COMMENT_POINTS()
                    {
                        COMMENT_ID = voteDetail.COMMENT_ID,
                        SAVE_DATE = DateTime.Now,
                        POINT = voteDetail.POINT
                    };

                    _db.COMMENT_POINTS.Add(comment_point2);
                    _db.SaveChanges();
                    result.value = true;
                    result.content = "به پیشنهاد امتیاز داده شد";
                    return result;
                }

                comment_point.POINT = voteDetail.POINT;
                comment_point.MODIFY_DATE = DateTime.Now;
                _db.SaveChanges();
                result.value = true;
                result.content = "امتیاز پیشنهاداصلاح شد";
                return result;
            }
        }


    }
}
