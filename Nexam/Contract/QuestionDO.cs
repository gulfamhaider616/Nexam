using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nexam
{
    public class QuestionDO
    {
        public int QuestionID { get; set; }
        public int SubjectID { get; set; }
        public int PaperID { get; set; }
        public string SubjectName { get; set; }
        public string PaperName { get; set; }
        public string QuestionStatement { get; set; }
        public int NumberOfChoices { get; set; }
        public string Choices { get; set; }
        public string CorrectAnswer { get; set; }
        public List<OptionsDO> Options { get; set; }
    }
}