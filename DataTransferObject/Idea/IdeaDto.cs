using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObject.Idea
{
    public class IdeaDto
    {
        public string TITLE { get; set; }
        public int TOTAL_POINT { get; set; }
        public string FullName { get; set; }
        public string USERNAME { get; set; }
        public string STATUS { get; set; }
        public System.DateTime SAVE_DATE { get; set; }
    }
}
