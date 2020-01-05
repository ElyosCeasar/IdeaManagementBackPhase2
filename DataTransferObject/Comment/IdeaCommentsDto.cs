using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObject.User
{
    public class IdeaCommentsDto
    {
        public int ID { get; set; }
        public string USERNAME { get; set; }
        public string FullName { get; set; }
        public int IDEA_ID { get; set; }
        public string COMMENT { get; set; }
        public System.DateTime SAVE_DATE { get; set; }
    }
}
