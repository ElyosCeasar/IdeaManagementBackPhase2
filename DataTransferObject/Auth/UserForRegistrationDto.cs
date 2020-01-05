using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataTransferObject.Auth
{
    public class UserForRegistrationDto
    {
        public string USERNAME { get; set; }
        public string PASSWORD { get; set; }
        public string EMAIL { get; set; }
        public string FIRST_NAME { get; set; }
        public string LAST_NAME { get; set; }
     

    }
}