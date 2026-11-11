using Nexam.Contract;
using Nexam.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Nexam.Business
{
    public class ForumBA
    {
        public bool SaveQuestion(AskQuestionDO question)
        {
            DataSet dataset = new ForumDA().SaveQuestion(question);
            var questionID = "";
            if (dataset.Tables.Count > 0)
            {
                if (dataset.Tables[0].Rows.Count > 0)
                {
                    DataRow r = dataset.Tables[0].Rows[0];
                    questionID = r["QuestionID"].ToString();
                }
            }
            if (questionID == "")
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public List<AskQuestionDO> GetAllQuestions()
        {
            List<AskQuestionDO> list = new List<AskQuestionDO>();
            DataSet dataset = new ForumDA().GetAllQuestions();
            if (dataset.Tables.Count > 0)
            {
                foreach (DataRow dr in dataset.Tables[0].Rows)
                {
                    AskQuestionDO question = new AskQuestionDO();
                    question.AskQuestionID = dr.Field<int>("QuestionID");
                    question.UserName = dr.Field<string>("UserName");
                    question.Email = dr.Field<string>("Email");
                    question.Country = dr.Field<string>("Country");
                    question.Subject = dr.Field<string>("Subject");
                    question.Explanation = dr.Field<string>("Explanation");
                    question.Publish = dr.Field<int>("IsPublish");
                    list.Add(question);
                }
            }
            return list;
        }

        public List<AskQuestionDO> GetAllPublishedQuestion()
        {
            List<AskQuestionDO> list = new List<AskQuestionDO>();
            DataSet dataset = new ForumDA().GetAllPublishedQuestions();
            if (dataset.Tables.Count > 0)
            {
                foreach (DataRow dr in dataset.Tables[0].Rows)
                {
                    AskQuestionDO question = new AskQuestionDO();
                    question.AskQuestionID = dr.Field<int>("QuestionID");
                    question.UserName = dr.Field<string>("UserName");
                    question.Email = dr.Field<string>("Email");
                    question.Country = dr.Field<string>("Country");
                    question.Subject = dr.Field<string>("Subject");
                    question.Explanation = dr.Field<string>("Explanation");
                    question.Publish = dr.Field<int>("IsPublish");
                    list.Add(question);
                }
            }
            return list;
        }

        public List<AskQuestionDO> GetAllUnPublishedQuestion()
        {
            List<AskQuestionDO> list = new List<AskQuestionDO>();
            DataSet dataset = new ForumDA().GetAllUnPublishedQuestions();
            if (dataset.Tables.Count > 0)
            {
                foreach (DataRow dr in dataset.Tables[0].Rows)
                {
                    AskQuestionDO question = new AskQuestionDO();
                    question.AskQuestionID = dr.Field<int>("QuestionID");
                    question.UserName = dr.Field<string>("UserName");
                    question.Email = dr.Field<string>("Email");
                    question.Country = dr.Field<string>("Country");
                    question.Subject = dr.Field<string>("Subject");
                    question.Explanation = dr.Field<string>("Explanation");
                    question.Publish = dr.Field<int>("IsPublish");
                    list.Add(question);
                }
            }
            return list;
        }

        public AskQuestionDO GetSingleQuestion(int QuestionID)
        {
            AskQuestionDO question = new AskQuestionDO();
            DataSet dataset = new ForumDA().GetSingleQuestion(QuestionID);
            if (dataset.Tables.Count > 0)
            {
                if (dataset.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = dataset.Tables[0].Rows[0];
                    question.AskQuestionID = dr.Field<int>("QuestionID");
                    question.UserName = dr.Field<string>("UserName");
                    question.Email = dr.Field<string>("Email");
                    question.Country = dr.Field<string>("Country");
                    question.Subject = dr.Field<string>("Subject");
                    question.Explanation = dr.Field<string>("Explanation");
                    question.Publish = dr.Field<int>("IsPublish");
                }
            }
            return question;
        }

        public bool PublishQuestionByAdmin(int QuestionID)
        {
            int result = new ForumDA().PublishQuestionByAdmin(QuestionID);
            if (result > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool DeleteQuestions(int QuestionID)
        {
            int result = new ForumDA().DeleteQuestions(QuestionID);
            if (result > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}