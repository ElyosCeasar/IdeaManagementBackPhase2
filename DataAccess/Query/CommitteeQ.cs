using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Mapper;
using DataTransferObject.Committee;
using DataTransferObject.Common;

namespace DataAccess.Query
{
    public class CommitteeQ
    {
        private IdeaManagmentDatabaseEntities _db;
        //-------------------------------------------------------------------------------------------------

        public Result VoteToIdea(int ideaId, VoteDetailDto voteDetailDto)
        {
            Result res = new Result();
            using (_db = new IdeaManagmentDatabaseEntities())
            {
                if (!_db.IDEAS.Any(x => x.ID == ideaId))
                {
                    res.content = "ایده ی مورد نظر پیدا نشد";
                    res.value = false;
                    return res;
                }

                if (!_db.COMMITTEE_VOTE_DETAIL.Any(x => x.IDEAS_ID == ideaId))
                {
                    var newVote = new COMMITTEE_VOTE_DETAIL()
                    {
                        COMMITTEE_MEMBER=voteDetailDto.COMMITTEE_MEMBER_USER_NAME,
                        IDEAS_ID=ideaId,
                         PROFIT_AMOUNT=voteDetailDto.PROFIT_AMOUNT,
                         SAVING_RESOURCE_AMOUNT=voteDetailDto.SAVING_RESOURCE_AMOUNT,
                        SAVE_DATE = DateTime.Now
                    };
                    _db.COMMITTEE_VOTE_DETAIL.Add(newVote);
                    _db.IDEAS.First(x => x.ID == ideaId).STATUS_ID =Convert.ToByte(voteDetailDto.Vote);
                    _db.SaveChanges();
                    res.value = true;
                    res.content = "رای کیته اعمال شد";
                }
                else
                {//update it is impossible do to bussines
                    var vote = _db.COMMITTEE_VOTE_DETAIL.First(x => x.IDEAS_ID == ideaId);
                    vote.PROFIT_AMOUNT = voteDetailDto.PROFIT_AMOUNT;
                    vote.SAVING_RESOURCE_AMOUNT = voteDetailDto.SAVING_RESOURCE_AMOUNT;
                    vote.SAVE_DATE = DateTime.Now;
                    vote.COMMITTEE_MEMBER = voteDetailDto.COMMITTEE_MEMBER_USER_NAME;
                    _db.IDEAS.First(x => x.ID == ideaId).STATUS_ID = Convert.ToByte(voteDetailDto.Vote);
                    _db.SaveChanges();
                    res.value = true;
                    res.content = "رای کیته اعمال شد";
                }
            }
            return res;
       }
        //-------------------------------------------------------------------------------------------------

        public Result UnVoteIdea(int ideaId, string username)
        {
            Result res = new Result();
            using (_db = new IdeaManagmentDatabaseEntities())
            {
                if (!_db.IDEAS.Any(x => x.ID == ideaId))
                {
                    res.content = "ایده ی مورد نظر پیدا نشد";
                    res.value = false;
                    return res;
                }

                if (!_db.COMMITTEE_VOTE_DETAIL.Any(x => x.IDEAS_ID == ideaId))
                {
                    res.content = "رای مورد نظر پیدا نشد";
                    res.value = false;
                    return res;
                }
                else
                {
                    _db.IDEAS.First(x => x.ID == ideaId).STATUS_ID = 0;
                    _db.COMMITTEE_VOTE_DETAIL.Remove(_db.COMMITTEE_VOTE_DETAIL.First(x => x.IDEAS_ID == ideaId));
                    _db.SaveChanges();
                }
            }
            return res;
        }
        //-------------------------------------------------------------------------------------------------
    }
}
