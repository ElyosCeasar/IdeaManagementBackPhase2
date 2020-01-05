using DataAccess.Mapper;
using DataTransferObject.Auth;
using DataTransferObject.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace DataAccess.Query
{
    public class AuthQ
    {
        private IdeaManagmentDatabaseEntities _db;
        //-------------------------------------------------------------------------------------------------

        public Result Registration(UserForRegistrationDto user)
        {
            Result res = new Result();
            var userFromDb = new UserQ().GetUser(user.USERNAME.Trim());
            if (userFromDb != null)
            {
                res.value = false;
                res.content = "نام کاربری موجود است";
            }else using (_db = new IdeaManagmentDatabaseEntities())
            {
                    var newUser = new USER()
                    {
                        EMAIL =user.EMAIL.Trim(),
                        FIRST_NAME =user.FIRST_NAME.Trim(),
                        LAST_NAME =user.LAST_NAME.Trim(),
                        USERNAME =user.USERNAME.Trim(),
                        PASSWORD =user.PASSWORD.Trim(),
                        SAVE_DATE = DateTime.Now
                        };
                    _db.USERS.Add(newUser);
                    _db.SaveChanges();
                    res.value = true;
                    res.content = "کاربر ایجاد شد";
                }

            return res;
        }
        //-------------------------------------------------------------------------------------------------

        public Result Login(UserForLoginDto user)
        {
            Result res = new Result();
            var userFromDb = new UserQ().GetUser(user.Username.Trim());
            if (userFromDb == null ||!userFromDb.PASSWORD.Equals(user.Password.Trim()))
            {
                res.value = false;
                res.content = "مشکل در احراز هوییت";
            }
            else
            {
                res.value = true;
                res.content = "ورود موفقیت آمیز بود";
            }
            return res;
        }
        //-------------------------------------------------------------------------------------------------


    }
}