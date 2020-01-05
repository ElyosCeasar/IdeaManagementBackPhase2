using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObject.Idea
{
    public class IdeaForShowDto
    {
        public int ID { get; set; }
        public string USERNAME { get; set; }
        public string FullName { get; set; }
        public byte STATUS_ID { get; set; }
        public string STATUS { get; set; }
        public string TITLE { get; set; }
        public System.DateTime SAVE_DATE { get; set; }
    }
}
