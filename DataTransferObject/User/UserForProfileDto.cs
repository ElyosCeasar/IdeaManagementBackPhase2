using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataTransferObject.User
{
    public class UserForProfileDto
    {
        public string USERNAME { get; set; }
        public bool COMMITTEE_FLAG { get; set; }
        public bool ADMIN_FLAG { get; set; }
        public string EMAIL { get; set; }
        public string FIRST_NAME { get; set; }
        public string LAST_NAME { get; set; }
        public System.DateTime SAVE_DATE { get; set; }
    }
}