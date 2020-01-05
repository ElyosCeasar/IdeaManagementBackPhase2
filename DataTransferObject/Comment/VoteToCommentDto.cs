using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObject.Comment
{
    public class VoteToCommentDto
    {
     
        public string USERNAME_voter { get; set; }
        public int COMMENT_ID { get; set; }
        public int POINT { get; set; }
    }
}
