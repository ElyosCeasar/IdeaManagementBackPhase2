using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObject.Idea
{
    public class IdeaDetailForShowDto
    {
        public int ID { get; set; }
        public string USERNAME { get; set; }
        public string FullName { get; set; }
        public byte STATUS_ID { get; set; }
        public string STATUS { get; set; }
        public string CURRENT_SITUATION { get; set; }
        public string PREREQUISITE { get; set; }
        public string STEPS { get; set; }
        public string ADVANTAGES { get; set; }
        public System.DateTime SAVE_DATE { get; set; }
        public int POINT { get; set; }

    }
}
