using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nexam
{
    public class QuizDO
    {
        public int QuizID { get; set; }
        public string SubjectName { get; set; }

        public int SubjectID { get; set; }

        public string Title { get; set; }
        public int UserID { get; set; }
        public int Score { get; set; }
        public int TotalQuestions { get; set; }
        public string InsertDate { get; set; }
        public string UpdateDate { get; set; }
        public int TotalAttempts { get; set; }
        public int AllowedAttempts { get; set; }
        public int TimeLimit { get; set; }
        
        public List<QuizQuestionDO> quizQuestionDOs { get; set; }   
        public List<QuizDO> QuizDOs { get; set;}
    }
}