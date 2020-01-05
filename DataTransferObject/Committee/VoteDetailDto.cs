using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObject.Committee
{
    public class VoteDetailDto
    {
        public int IDEAS_ID { get; set; }
        public string COMMITTEE_MEMBER_USER_NAME { get; set; }
        public Nullable<long> PROFIT_AMOUNT { get; set; }
        public Nullable<long> SAVING_RESOURCE_AMOUNT { get; set; }

        public int Vote { get; set; }
    }
}
