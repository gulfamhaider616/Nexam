using Nexam.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using TextmagicRest.Model;

namespace Nexam
{
    public class QuizBA
    {
        public QuizQuestionContract SaveQuiz(QuizDO quiz)
        {
            DataSet dataset = new QuizDA().SaveQuiz(quiz);
            QuizQuestionContract contract = new QuizQuestionContract();
            List<QuestionDO> list = new List<QuestionDO>();
            if (dataset.Tables.Count > 0)
            {
                if (dataset.Tables[0].Rows.Count > 0)
                {
                    DataRow r = dataset.Tables[0].Rows[0];
                    contract.QuizID = Convert.ToInt32(r["QuizID"]);
                }
                foreach (DataRow dr in dataset.Tables[1].Rows)
                {
                    QuestionDO question = new QuestionDO();
                    question.QuestionID = dr.Field<int>("QuestionID");
                    question.SubjectID = dr.Field<int>("SubjectID");
                    question.SubjectName = dr.Field<string>("SubjectName");
                    question.QuestionStatement = dr.Field<string>("QuestionStatement");
                    question.NumberOfChoices = dr.Field<int>("NumberOfChoices");
                    question.Choices = dr.Field<string>("Choices");
                    question.CorrectAnswer = dr.Field<string>("CorrectAnswer");
                    list.Add(question);
                }
                contract.QuestionsDOs = list;
            }

            return contract;
        }
        public ResultDO QuizResult(QuizQuestionContract quizquestioncontract)
        {
            int totalques = quizquestioncontract.QuizQuestions.Count;

            int correctans = quizquestioncontract.QuizQuestions.Where(x => x.UserAnswer.Trim() == x.CorrectAnswer.Trim()).ToList().Count;
            int score = (int)Math.Ceiling((decimal)((100 * correctans) / totalques));
            ResultDO result =new ResultDO();
            result.Score = score.ToString() + "%";
            if(score <= 50)
            {
                result.Remarks = "FAIL";
            }
            else if(score > 50 && score <70)
            {
                result.Remarks = "PASS";
            }
            else
            {
                result.Remarks = "Excellent";
            }
            result.TotalQuestions = totalques;
            result.CorrectAnswers = correctans;
            int quizID = quizquestioncontract.QuizID;
            foreach(var question in quizquestioncontract.QuizQuestions)
            {
                question.QuizID = quizID;
                int res = new QuizDA().SaveQuizQuestions(question);
            }
            int result1 = new QuizDA().UpdateQuiz(quizID, score);
            return result;
        }
        public QuizDO GeTQuizById(int quizID)
        {
            QuizDO quizDeatil = new QuizDO();
            DataSet dataset = new QuizDA().GeTQuizById(quizID);
            if (dataset.Tables.Count > 0)
            {
                List<QuizQuestionDO> list = new List<QuizQuestionDO>();
                foreach (DataRow dr in dataset.Tables[0].Rows)
                {
                    QuizQuestionDO quiz = new QuizQuestionDO();
                    string corr = dr.Field<string>("CorrectAnswer");
                    string userans = dr.Field<string>("UserAnswer");
                    quiz.QuizID = dr.Field<int>("QuizID");
                    quiz.Subject = dr.Field<string>("SubjectName");
                    quiz.Title = dr.Field<string>("Title");
                    quiz.Score = Convert.ToInt32(dr.Field<string>("Score"));

                    quiz.QuestionStatement = dr.Field<string>("QuestionStatement");
                    quiz.Choices = dr.Field<string>("Choices");
                    quiz.CorrectAnswer = corr;
                    quiz.UserAnswer = userans;
                    quiz.IsCorrect = corr.Trim() == userans.Trim() ? true:false;
                    list.Add(quiz);
                }
                quizDeatil.quizQuestionDOs = list;
            }
            return quizDeatil;
        }
        //public UserDO VerifyUser(string email, string password)
        //{
        //    DataSet dataset = new UserDA().VerifyUser(email, password);
        //    UserDO user = new UserDO();
        //    //string user = "";
        //    if (dataset.Tables.Count > 0)
        //    {
        //        if (dataset.Tables[0].Rows.Count > 0)
        //        {
        //            DataRow r = dataset.Tables[0].Rows[0];
        //            user.FName = r["FirstName"].ToString();
        //            user.Id = Convert.ToInt32(r["Id"]);
        //        }
        //    }
        //    return user;
        //}
        //public List<UserDO> GetAllUsers()
        //{
        //    List<UserDO> list = new List<UserDO>();
        //    DataSet dataset = new UserDA().GetAllUsers();
        //    if (dataset.Tables.Count > 0)
        //    {
        //        foreach (DataRow dr in dataset.Tables[0].Rows)
        //        {
        //            UserDO user = new UserDO();
        //            user.Id = dr.Field<int>("Id");
        //            user.FName = dr.Field<string>("FirstName");
        //            user.LName = dr.Field<string>("LastName");
        //            user.Email = dr.Field<string>("Email");
        //            user.Password = dr.Field<string>("Password");
        //            user.PNumber = dr.Field<string>("PhoneNumber");
        //            user.Country = dr.Field<string>("Country");
        //            user.City = dr.Field<string>("City");
        //            list.Add(user);
        //        }
        //    }
        //    return list;
        //}
    }
}