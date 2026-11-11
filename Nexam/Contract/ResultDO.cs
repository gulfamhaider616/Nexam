using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nexam
{
    public class ResultDO
    {
        public string Score { get; set; }

        public string Name { get; set; }
        public string Remarks { get; set; }
        public int TotalQuestions { get; set; }
        public int CorrectAnswers { get; set; }
    }
}