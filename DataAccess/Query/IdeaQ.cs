using DataAccess.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataTransferObject.Idea;
using System.Net.Http;
using DataTransferObject.Common;

namespace DataAccess.Query
{
    public class IdeaQ
    {
        private IdeaManagmentDatabaseEntities _db;
        //-------------------------------------------------------------------------------------------------
        public IEnumerable<IdeaForShowDto> GetAllIdea()
        {
            IEnumerable<IdeaForShowDto> res = null;
            using (_db = new IdeaManagmentDatabaseEntities())
            {
                res = _db.IDEAS.Select(x => new IdeaForShowDto()
                {
                    ID =x.ID,
                    USERNAME =x.USERNAME,
                    FullName =x.USER.FIRST_NAME+" "+ x.USER.LAST_NAME,
                    SAVE_DATE =x.SAVE_DATE,
                    STATUS =x.IDEA_STATUS.TITLE,
                    STATUS_ID =x.STATUS_ID,
                    TITLE =x.TITLE
                  });

  
            }
            return res;
        }
        //----------------------------------------------------------------------------------------------------------
        public IdeaDetailForShowDto GetSpecificIdea(int ideaId)
        {
            IdeaDetailForShowDto res = null; 
            using (_db = new IdeaManagmentDatabaseEntities())
            {
                var idea = _db.IDEAS.FirstOrDefault(x => x.ID == ideaId);
                if (idea != null)
                    res = new IdeaDetailForShowDto()
                    {
                        ID = idea.ID,
                        POINT = idea.IDEA_POINTS.Sum(x => x.POINT),
                        SAVE_DATE = idea.SAVE_DATE,
                        STATUS = idea.IDEA_STATUS.TITLE,
                        CURRENT_SITUATION = idea.CURRENT_SITUATION,
                        PREREQUISITE = idea.PREREQUISITE,
                        ADVANTAGES = idea.ADVANTAGES,
                        STEPS = idea.STEPS,
                        USERNAME = idea.USERNAME,
                        FullName = idea.USER.FIRST_NAME + " " + idea.USER.LAST_NAME,
                        STATUS_ID = idea.STATUS_ID

                    };
                 
                
            }
            return res;
        }
        //-----------------------------------------------------------------------------------------------------------
        public Result SendNewIdea(NewIdeaDto newIdea)
        {
            Result result = new Result();

            using (_db = new IdeaManagmentDatabaseEntities())
            {

                IDEA idea = new IDEA()
                {
                    USERNAME = newIdea.USERNAME,
                    TITLE = newIdea.TITLE,
                    SAVE_DATE = DateTime.Now,
                    ADVANTAGES=newIdea.ADVANTAGES,
                    PREREQUISITE= newIdea.PREREQUISITE,
                    STEPS= newIdea.STEPS,
                    CURRENT_SITUATION= newIdea.CURRENT_SITUATION
                };
                _db.IDEAS.Add(idea);
                _db.SaveChanges();
                result.value = true;
                result.content = "ایده جدید ایجاد شد";
                return result;
            }
        }
        //----------------------------------------------------------------------------------------------------------

        public Result EditIdea(int ideaId, ChangedIdeaDto ideaForChange)
        {
            Result result = new Result();

            using (_db = new IdeaManagmentDatabaseEntities())
            {
                var idea = _db.IDEAS.FirstOrDefault(x => x.ID == ideaId); 
                if (idea == null)
                {
                    result.value = false;
                    result.content = "ایده پیدا نشد";
                    return result;
                }

                idea.USERNAME = ideaForChange.USERNAME;
                idea.TITLE = ideaForChange.TITLE;
                idea.PREREQUISITE = ideaForChange.PREREQUISITE;
                idea.STEPS = ideaForChange.STEPS;
                idea.CURRENT_SITUATION = ideaForChange.CURRENT_SITUATION;
                idea.ADVANTAGES = ideaForChange.ADVANTAGES;
                idea.MODIFY_DATE = DateTime.Now;
                _db.SaveChanges();
                result.value = true;
                result.content = "ایده اصلاح شد";
                return result;
            }
        }

        //----------------------------------------------------------------------------------------------------------
        public Result VoteToIdea(IdeaPointDto voteDetail)
        {
            Result result = new Result();

            using (_db = new IdeaManagmentDatabaseEntities())
            {
                var idea_point = _db.IDEA_POINTS.FirstOrDefault(x => x.IDEA_ID == voteDetail.IDEA_ID && x.USERNAME == voteDetail.USERNAME);
                if (idea_point == null)
                {
                    var idea = _db.IDEAS.FirstOrDefault(x => x.ID == voteDetail.IDEA_ID);
                    if (idea == null)
                    {
                        result.value = false;
                        result.content = "ایده پیدا نشد";
                        return result;
                    }
                    var user = _db.USERS.FirstOrDefault(x => x.USERNAME == voteDetail.USERNAME);
                    if (user == null)
                    {
                        result.value = false;
                        result.content = "کاربر پیدا نشد";
                        return result;
                    };
                    if (idea.USERNAME == voteDetail.USERNAME)
                    {
                        result.value = false;
                        result.content = "به ایده ی خود نمی شود رای داد";
                        return result;
                    }

                    IDEA_POINTS idea_point_ = new IDEA_POINTS()
                    {
                        IDEA_ID = voteDetail.IDEA_ID,
                        USERNAME = voteDetail.USERNAME,
                        SAVE_DATE = DateTime.Now,
                        POINT = voteDetail.POINT
                    };

                    _db.IDEA_POINTS.Add(idea_point_);
                    _db.SaveChanges();
                    result.value = true;
                    result.content = "به ایده امتیاز داده شد";
                    return result;
                }

                idea_point.POINT = voteDetail.POINT;
                idea_point.MODIFY_DATE = DateTime.Now;
                _db.SaveChanges();
                result.value = true;
                result.content = "امتیاز ایده اصلاح شد";
                return result;
            }
        }

        //-------------------------------------------------------------------------------------------------
        public IEnumerable<IdeaDto> GetIdeasTop10All()
        {
            List<IdeaDto> res = new List<IdeaDto>();
            using (_db = new IdeaManagmentDatabaseEntities())
            {
                var ideaPoint = _db.IDEA_POINTS.GroupBy(x => x.IDEA_ID).Select(y => new
                {
                    IDEA_ID = y.Key,
                    TOTAL_POINT = y.Sum(x => x.POINT)
                }).OrderByDescending(x => x.TOTAL_POINT).Take(10).ToList();

                foreach(var i in ideaPoint)
                {
                    var idea = _db.IDEAS.Single(z => z.ID == i.IDEA_ID);
            
                    res.Add(new IdeaDto()
                    {
                        TITLE = idea.TITLE,
                        TOTAL_POINT = i.TOTAL_POINT,
                        FullName = idea.USER.FIRST_NAME+" "+idea.USER.LAST_NAME,
                        USERNAME = idea.USERNAME,
                        STATUS = idea.IDEA_STATUS.TITLE,
                        SAVE_DATE = idea.SAVE_DATE
                    });
                }
                

            }
            return res;
        }
        //-------------------------------------------------------------------------------------------------
        public IEnumerable<IdeaDto> GetIdeasTop10CurrentMonth()
        {
            List<IdeaDto> res = new List<IdeaDto>();
             //--------------------
             var now = DateTime.Now;
            var convertedToShamsi = Persia.Calendar.ConvertToPersian(now);
            int currentDayOfMonth = convertedToShamsi.ArrayType[2];
            var startDateDateMonthInMiladi = DateTime.Now.AddDays(-currentDayOfMonth + 1);

            int numberOfDaysToEndMonth = 0;
            //چک کردن ماه 31 روزه 30 روزه و29 روزه
            if (convertedToShamsi.ArrayType[1] < 7)
                numberOfDaysToEndMonth = 31 - currentDayOfMonth;/// تعداد روزی که باید بریم جلو تو ماه میلادی
            else if (convertedToShamsi.ArrayType[1] < 12)
                numberOfDaysToEndMonth = 30 - currentDayOfMonth; 
            else
                numberOfDaysToEndMonth = 29 - currentDayOfMonth; 
            var finishDateMonthInMiladi = DateTime.Now.AddDays(numberOfDaysToEndMonth);

            startDateDateMonthInMiladi = new DateTime(startDateDateMonthInMiladi.Year, startDateDateMonthInMiladi.Month, startDateDateMonthInMiladi.Day, 23, 59, 59);
            finishDateMonthInMiladi = new DateTime(finishDateMonthInMiladi.Year, finishDateMonthInMiladi.Month, finishDateMonthInMiladi.Day, 23, 59, 59);
            //--------------------
            using (_db = new IdeaManagmentDatabaseEntities())
            {
                var bestIdeas = _db.IDEA_POINTS.
                    Where(p => p.IDEA.SAVE_DATE >= startDateDateMonthInMiladi && p.IDEA.SAVE_DATE <= finishDateMonthInMiladi)
                    .GroupBy(x => x.IDEA_ID)
                    .Select(y => new
                    {
                        IDEA_ID = y.Key,
                        TOTAL_POINT = y.Sum(x => x.POINT)
                    }).OrderByDescending(x => x.TOTAL_POINT)
                .Take(10);

                foreach(var x in bestIdeas)
                {
                    var idea = _db.IDEAS.Single(i => i.ID == x.IDEA_ID);
                    res.Add(new IdeaDto()
                    {
                        TITLE = idea.TITLE,
                        TOTAL_POINT = x.TOTAL_POINT,
                        FullName = idea.USER.FIRST_NAME+" "+idea.USER.LAST_NAME,
                        USERNAME = idea.USERNAME,
                        STATUS = idea.IDEA_STATUS.TITLE,
                        SAVE_DATE = idea.SAVE_DATE
                    });
                }

            }
            return res;
        }
        //-------------------------------------------------------------------------------------------------
        public IEnumerable<IdeaDto> GetIdeasTop10CurrentWeek()
        {
            List<IdeaDto> res = new List<IdeaDto>();
            //--------------------

            var currentDate = DateTime.Now;
            var converted = Persia.Calendar.ConvertToPersian(currentDate);
            int dayOfWeek = converted.DayOfWeek+1;
            var startDayOfWeekInMillady = DateTime.Now.AddDays(-dayOfWeek + 1);//اول
            startDayOfWeekInMillady = new DateTime(startDayOfWeekInMillady.Year, startDayOfWeekInMillady.Month, startDayOfWeekInMillady.Day, 0, 0, 0);
            var endDayOfWeekInMillady = DateTime.Now.AddDays(7 - dayOfWeek);//آخرش
            endDayOfWeekInMillady = new DateTime(endDayOfWeekInMillady.Year, endDayOfWeekInMillady.Month, endDayOfWeekInMillady.Day, 23, 59, 59);
            //--------------------
            using (_db = new IdeaManagmentDatabaseEntities())
            {
                var bestIdeas = _db.IDEA_POINTS.Where(p => p.IDEA.SAVE_DATE >= startDayOfWeekInMillady && p.IDEA.SAVE_DATE <= endDayOfWeekInMillady)
                    .GroupBy(x => x.IDEA_ID)
                    .Select(y => new
                {
                    IDEA_ID = y.Key,
                    TOTAL_POINT = y.Sum(x => x.POINT)
                }).OrderByDescending(x => x.TOTAL_POINT).Take(10);

                foreach (var x in bestIdeas)
                {
                    var idea = _db.IDEAS.Single(i => i.ID == x.IDEA_ID);
                    res.Add(new IdeaDto()
                    {
                        TITLE = idea.TITLE,
                        TOTAL_POINT = x.TOTAL_POINT,
                        FullName = idea.USER.FIRST_NAME + " " + idea.USER.LAST_NAME,
                        USERNAME = idea.USERNAME,
                        STATUS = idea.IDEA_STATUS.TITLE,
                        SAVE_DATE = idea.SAVE_DATE
                    });
                }

            }
            return res;
        }
        //-------------------------------------------------------------------------------------------------


        public bool IsIdeaLocked(int id)
        {
            using (_db = new IdeaManagmentDatabaseEntities())
            {
                var idea = _db.IDEAS.FirstOrDefault(x => x.ID == id);
                if (idea == null)
                {
                    return true;//this true means not found
                }
                else
                {
                    return idea.STATUS_ID == 0 ? false : true;
                }
            }
        }
    }
}

