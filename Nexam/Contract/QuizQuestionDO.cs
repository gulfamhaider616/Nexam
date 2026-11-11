using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nexam
{
    public class QuizQuestionDO
    {
        public int QuizID { get; set; }

        public int QuestionID { get; set; }

        public string UserAnswer { get; set; }
        public string CorrectAnswer { get; set; }
        public bool IsCorrect { get; set; }

        public int PaperID { get; set; }

        public string PaperName { get; set; }
        public string QuestionStatement { get; set; }
        public int NumberOfChoices { get; set; }
        public string Choices { get; set; }
        public string Subject { get; set; }
        public string Title { get; set; }
        public int Score { get; set; }
    }
}