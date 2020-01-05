using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObject.Idea
{
    public class ChangedIdeaDto
    {
        public string USERNAME { get; set; }

        public string TITLE { get; set; }

        public string CURRENT_SITUATION { get; set; }
        public string PREREQUISITE { get; set; }
        public string STEPS { get; set; }
        public string ADVANTAGES { get; set; }

    }
}
