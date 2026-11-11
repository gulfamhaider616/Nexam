using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nexam
{
    public class QuizQuestionContract
    {
        public int QuizID { get; set; }

        public int UserID { get; set; }

        public List<QuizQuestionDO> QuizQuestions { get; set; }
        public List<QuestionDO> QuestionsDOs { get; set; }
    }
}