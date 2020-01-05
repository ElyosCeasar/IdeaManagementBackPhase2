using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObject.User
{
    /// < th nzWidth="23%">نام و نام خانوادگی طراح</th>
    /// <th nzWidth = "23%" > نام کاربری</th>
    /// <th nzWidth = "24%" > تعداد پیشنهادها</th>
    /// <th nzWidth = "20%" > مجموع امتیاز ها</th>
    public class UserShowingTop10Dto
    {
        public string FullName { set; get; }
        public string UserName { set; get; }

        //idea / comments
        public int Count { set; get; }
        public int PointsCount { set; get; }
    }
}
