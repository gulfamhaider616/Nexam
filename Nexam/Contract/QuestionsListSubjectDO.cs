using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nexam
{
    public class QuestionsListSubjectDO
    {
        public List<QuestionDO> questions { get; set; }
        public string Subject { get; set; }
        public string Paper { get; set; }
        public int PaperID { get; set; }
        public List<SubjectDO> SubjectList { get; set; }
        public List<PaperDO> PaperList { get; set; }
        public PaperDO PaperDetail { get; set; }
    }
}