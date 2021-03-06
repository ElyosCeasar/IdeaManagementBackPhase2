﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using DataAccess.Mapper;
using DataTransferObject.Common;
using DataTransferObject.User;

namespace DataAccess.Query
{


    public class UserQ
    {
        private IdeaManagmentDatabaseEntities _db;
        //-------------------------------------------------------------------------------------------------
        public IEnumerable<UserForShowDto> GetAllUsers()
        {
            using (_db = new IdeaManagmentDatabaseEntities())
            {
                return _db.USERS.Select(x => new UserForShowDto()
                {
                    COMMITTEE_FLAG = x.COMMITTEE_FLAG,
                    ADMIN_FLAG = x.ADMIN_FLAG,
                    EMAIL = x.EMAIL,
                    FIRST_NAME = x.FIRST_NAME,
                    LAST_NAME = x.LAST_NAME,
                    SAVE_DATE = x.SAVE_DATE,
                    USERNAME = x.USERNAME
                }).ToList();
            }
        }
        //-------------------------------------------------------------------------------------------------

        public UserForProfileDto GetUserProfile(string username)
        {
            using (_db = new IdeaManagmentDatabaseEntities())
            {
                var user = _db.USERS.SingleOrDefault(x => x.USERNAME == username);
                if (user == null)
                    return null;
                return new UserForProfileDto()
                {
                    COMMITTEE_FLAG = user.COMMITTEE_FLAG,
                    ADMIN_FLAG = user.ADMIN_FLAG,
                    EMAIL = user.EMAIL,
                    FIRST_NAME = user.FIRST_NAME,
                    SAVE_DATE = user.SAVE_DATE,
                    USERNAME = user.USERNAME
                };
            }
        }
        //-------------------------------------------------------------------------------------------------

        public IEnumerable<UserShowingTop10Dto> GetTop10IdeaMaker()
        {
            IEnumerable<UserShowingTop10Dto> res=null;
            using (_db = new IdeaManagmentDatabaseEntities())
            {
                Dictionary<string, int> userScrs = new Dictionary<string, int>();
                var idea_points = _db.IDEA_POINTS.GroupBy(x => x.IDEA_ID).Select(y => new
                {
                    IDEA_ID = y.Key,
                    TOTAL_POINT = y.Sum(x => x.POINT)
                });
                var AllUsers= _db.USERS;
                foreach(var u in AllUsers)
                {
                   var userAllIdeasId= u.IDEAS.Select(x=>x.ID);
                    int scr = 0;
                    foreach(var i in userAllIdeasId)
                    {
                        scr += idea_points.Single(x => x.IDEA_ID == i).TOTAL_POINT;
                    }
                    userScrs.Add(u.USERNAME, scr);
                }

                res = userScrs.ToList().OrderByDescending(x => x.Value).Take(10).Select(u => new UserShowingTop10Dto() {
                    Count= _db.IDEAS.Count(),
                    UserName=u.Key,
                    FullName=_db.USERS.Single(x=>x.USERNAME==u.Key).FIRST_NAME+" "+ _db.USERS.Single(x => x.USERNAME == u.Key).LAST_NAME,
                    PointsCount= u.Value,
                });
            }
           return res;
         }
        //-------------------------------------------------------------------------------------------------

        public IEnumerable<UserForShowDto> FilterSerchingUsers(FilterUserRequestDto searchItem)
        {
            IEnumerable<UserForShowDto> res = null;
            using (_db = new IdeaManagmentDatabaseEntities())
            {
                System.Linq.IQueryable<USER> users=_db.USERS;
                if (searchItem.Username != null && searchItem.Username.Trim().Length > 0)
                {
                    users = users.Where(u => u.USERNAME.Contains(searchItem.Username.Trim()));
                }
                if (searchItem.FullName != null && searchItem.FullName.Trim().Length > 0)
                {
                    if (searchItem.FullName.Trim().Contains(" "))
                    {
                        var firstName = searchItem.FullName.Trim().Substring(0, searchItem.FullName.Trim().IndexOf(" "));
                        var lastName = searchItem.FullName.Trim().Substring(0, searchItem.FullName.Trim().IndexOf(" "));
                    }
                    else
                    {
                        users = users.Where(u => u.FIRST_NAME.Contains(searchItem.Username.Trim())|| u.LAST_NAME.Contains(searchItem.Username.Trim()));
                    }

                }
                switch (searchItem.PositionValue)
                {
                    case 0:
                        users = users.Where(u => u.ADMIN_FLAG==false&&u.COMMITTEE_FLAG==false);
                        break;
                    case 1:
                        users = users.Where(u => u.ADMIN_FLAG == false && u.COMMITTEE_FLAG == true);

                        break;
                    case 2:
                        users = users.Where(u => u.ADMIN_FLAG == true && u.COMMITTEE_FLAG == false);

                        break;
                    case 3:
                    default:
                        break;

                }
                res = users.Select(x => new UserForShowDto()
                {
                    COMMITTEE_FLAG = x.COMMITTEE_FLAG,
                    ADMIN_FLAG = x.ADMIN_FLAG,
                    EMAIL = x.EMAIL,
                    FIRST_NAME = x.FIRST_NAME,
                    LAST_NAME = x.LAST_NAME,
                    SAVE_DATE = x.SAVE_DATE,
                    USERNAME = x.USERNAME
                });
            }
            return res;
        }
        //-------------------------------------------------------------------------------------------------

        public IEnumerable<UserShowingTop10Dto> GetTop10CommentMaker()
        {
            IEnumerable<UserShowingTop10Dto> res = null;
            using (_db = new IdeaManagmentDatabaseEntities())
            {
                Dictionary<string, int> userScrs = new Dictionary<string, int>();
                var commentPoints = _db.COMMENT_POINTS.GroupBy(x => x.COMMENT_ID).Select(y => new
                {
                    comment_ID = y.Key,
                    TOTAL_POINT = y.Sum(x => x.POINT)
                });
                var AllUsers = _db.USERS;
                foreach (var u in AllUsers)
                {
                    var userAllCommentsId = u.IDEA_COMMENTS.Select(x => x.ID);
                    int scr = 0;
                    foreach (var i in userAllCommentsId)
                    {
                        scr += commentPoints.Single(x => x.comment_ID == i).TOTAL_POINT;
                    }
                    userScrs.Add(u.USERNAME, scr);
                }

                res = userScrs.ToList().OrderByDescending(x => x.Value).Take(10).Select(u => new UserShowingTop10Dto()
                {
                    Count = _db.IDEA_COMMENTS.Count(),
                    UserName = u.Key,
                    FullName = _db.USERS.Single(x => x.USERNAME == u.Key).FIRST_NAME + " " + _db.USERS.Single(x => x.USERNAME == u.Key).LAST_NAME,
                    PointsCount = u.Value,
                });
            }
            return res;
        }

        //-------------------------------------------------------------------------------------------------

        public Result PutUserProfile(ProfileForUpdateDto newProfile)
        {
            Result res = new Result();
            using (_db = new IdeaManagmentDatabaseEntities())
            {
                var user = _db.USERS.FirstOrDefault(u => u.USERNAME == newProfile.USERNAME);//username is old username and youcan't change it
                if (user == null)
                {
                    res.value = false;
                    res.content = "کاربر یافت نشد";
                }
                else
                {
                    user.LAST_NAME = newProfile.LAST_NAME;
                    user.FIRST_NAME= newProfile.FIRST_NAME;
                    user.EMAIL= newProfile.EMAIL;
                    user.PASSWORD= newProfile.PASSWORD;
                    user.MODIFY_DATE = DateTime.Now;
                    _db.SaveChanges();
                    res.value = true;
                    res.content = "تغییرات اعمال شد";
                }
            }
            return res;
        }
        //-------------------------------------------------------------------------------------------------

        public UserDetailForShowDto GetUser(string username)
        {
            UserDetailForShowDto res=null;
            using (_db = new IdeaManagmentDatabaseEntities())
            {
               var user = _db.USERS.SingleOrDefault(u => u.USERNAME == username);
                if (user != null)
                    res= new UserDetailForShowDto()
                    {
                        ADMIN_FLAG = user.ADMIN_FLAG,
                        COMMITTEE_FLAG = user.COMMITTEE_FLAG,
                        EMAIL = user.EMAIL,
                        FIRST_NAME = user.EMAIL,
                        LAST_NAME = user.LAST_NAME,
                        PASSWORD = user.PASSWORD,
                        SAVE_DATE =user.SAVE_DATE,
                        USERNAME = user.USERNAME
                    };
            }
            return res;
        }

        //----------------------------------------------------------------------------------------------------
        public Result ChangeCommitteFlag(string username, int value)
        {
            Result res = new Result();
            USER user;
       
            using (_db = new IdeaManagmentDatabaseEntities())
            {
                user = _db.USERS.FirstOrDefault(u => u.USERNAME == username);
                if (user != null)
                {
                    res.value = true;
                    if (value == 0)
                    {
                        
                        res.content = "کاربر از عضویت کمیته ارزیابی خارج شد";
                        user.COMMITTEE_FLAG = false;

                    }
                    else//1
                    {
                        res.content = "کاربر عضو کمیته ارزیابی شد";
                        user.COMMITTEE_FLAG = true;
                    }
                    _db.SaveChanges();

                }
                else
                {
                    res.value = false;
                    res.content = "کاربر یافت شد";
                }
                return res;
            }
        }
        //-------------------------------------------------------------------------------------------------

        public bool IsAdmin(string username)
        {
            using (_db = new IdeaManagmentDatabaseEntities())
            {
                var user_ = _db.USERS.SingleOrDefault(x => x.USERNAME == username);
                if (user_ == null)
                    return false;
                return user_.ADMIN_FLAG;
            };
        }
        //-------------------------------------------------------------------------------------------------

        public bool IsCommitteMember(string username)
        {
            using (_db = new IdeaManagmentDatabaseEntities())
            {
                var user_ = _db.USERS.SingleOrDefault(x => x.USERNAME == username);
                if (user_ == null)
                    return false;
                return user_.COMMITTEE_FLAG;
            };
        }
    }
}