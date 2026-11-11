using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexam.Contract
{
    public class AskQuestionDO
    {
        public int AskQuestionID { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Country { get; set; }
        public string Subject { get; set; }
        public string Explanation { get; set; }
        public int Publish { get; set; }
    }
}
