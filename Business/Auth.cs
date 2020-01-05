using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataTransferObject.Auth;
using DataTransferObject.Common;

namespace Business
{
    public class Auth
    {
        private readonly DataAccess.Query.AuthQ _repository;
        //-------------------------------------------------------------------------------------------------
        public Auth()
        {
            _repository = new DataAccess.Query.AuthQ();
        }
        //-------------------------------------------------------------------------------------------------
        public Result Login(UserForLoginDto user)
        {
            Result res;
            if (user == null)
            {
                res= new Result() { content = "خطایی رخ داده است", value = false };
            }
           else if (user.Username == null)
            {
                res = new Result() { content ="لطفا نام کاربری خود را وارد کنید", value =false};
            }
            else if (user.Password == null)
            {
                res = new Result() { content = "لطفا کلمه ی عبور خود را وارد کنید", value = false };
            }
            else 
            {
                res = _repository.Login(user);
            }
            return res;  
        }
        //-------------------------------------------------------------------------------------------------

        public Result Registration(UserForRegistrationDto user)
        {
            Result res;
            if (user == null)
            {
                res = new Result() { content = "خطایی رخ داده است", value = false };
            }
            else if (
                user.FIRST_NAME==null ||
                user.EMAIL==null ||
                user.LAST_NAME==null ||
                user.USERNAME==null ||
                user.PASSWORD==null
                )
            {
                res = new Result() { content = "یکی از فیلد های اجباری پرنشده است", value = false };
            }
            else
            {
                res = _repository.Registration(user);
            }
            return res;
        }
        //-------------------------------------------------------------------------------------------------

    }
}
