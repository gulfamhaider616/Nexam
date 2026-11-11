using Nexam.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Nexam.Business
{
    public class FPSCBA
    {
        public bool SaveFPSCNewPaper(PaperDO paper)
        {
            DataSet set = new FPSCDA().SaveFPSCNewPaper(paper);
            if (set.Tables.Count > 0)
            {
                if (set.Tables[0].Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public bool SaveFPSCQuestion(QuestionDO question)
        {
            DataSet set = new FPSCDA().SaveFPSCQuestion(question);
            if (set.Tables.Count > 0)
            {
                if (set.Tables[0].Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public List<PaperDO> GetAllFPSCPaper()
        {
            List<PaperDO> list = new List<PaperDO>();
            DataSet dataset = new FPSCDA().GetAllFPSCPaper();
            if (dataset.Tables.Count > 0)
            {
                foreach (DataRow dr in dataset.Tables[0].Rows)
                {
                    PaperDO paper = new PaperDO();
                    paper.PaperID = dr.Field<int>("PaperID");
                    paper.PaperName = dr.Field<string>("PaperName");
                    paper.PaperDetail = dr.Field<string>("PaperDetail");
                    list.Add(paper);
                }
            }
            return list;
        }

        public QuestionsListSubjectDO GetAllFPSCQuestionsByPaperID(int paperid)
        {
            QuestionsListSubjectDO obj = new QuestionsListSubjectDO();
            List<QuestionDO> list = new List<QuestionDO>();
            DataSet dataset = new FPSCDA().GetAllFPSCQuestionsByPaperID(paperid);
            if (dataset.Tables.Count > 0)
            {
                foreach (DataRow dr in dataset.Tables[0].Rows)
                {
                    QuestionDO question = new QuestionDO();
                    question.QuestionID = dr.Field<int>("QuestionID");
                    question.PaperID = dr.Field<int>("PaperID");
                    question.QuestionStatement = dr.Field<string>("QuestionStatement");
                    if (!String.IsNullOrEmpty(dr.Field<string>("Choices")))
                    {
                        string allChoices = dr.Field<string>("Choices");
                        string[] choices = allChoices.Split(',', '،');
                        List<OptionsDO> optionList = new List<OptionsDO>();
                        foreach (var choice in choices)
                        {
                            OptionsDO option = new OptionsDO();
                            option.Choice = choice;
                            optionList.Add(option);
                        }
                        question.Options = optionList;
                    }
                    question.CorrectAnswer = dr.Field<string>("CorrectAnswer");
                    list.Add(question);
                }
                if (dataset.Tables[0].Rows.Count > 0)
                    obj.Paper = dataset.Tables[0].Rows[0].Field<string>("PaperName");
            }
            obj.questions = list;
            obj.PaperList = GetAllFPSCPaper();
            return obj;
        }

        public QuestionDO GetFPSCQuestionById(int questionid)
        {
            DataSet dataset = new FPSCDA().GetFPSCQuestionById(questionid);
            QuestionDO question = new QuestionDO();
            if (dataset.Tables.Count > 0)
            {
                DataRow dr = dataset.Tables[0].Rows[0];
                question.QuestionID = dr.Field<int>("QuestionID");
                question.PaperID = dr.Field<int>("PaperID");
                question.PaperName = dr.Field<string>("PaperName");
                question.QuestionStatement = dr.Field<string>("QuestionStatement");
                question.Choices = dr.Field<string>("Choices");
                question.CorrectAnswer = dr.Field<string>("CorrectAnswer");
            }
            return question;
        }

        public bool DeleteFPSCQuestionById(int questionid)
        {
            int result = new FPSCDA().DeleteFPSCQuestionById(questionid);
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