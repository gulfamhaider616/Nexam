using Nexam.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Nexam.Business
{
    public class NTSBA
    {
        public bool SaveNTSNewPaper(PaperDO paper)
        {
            DataSet set = new NTSDA().SaveNTSNewPaper(paper);
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

        public bool SaveNTSQuestion(QuestionDO question)
        {
            DataSet set = new NTSDA().SaveNTSQuestion(question);
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

        public List<PaperDO> GetAllNTSPaper()
        {
            List<PaperDO> list = new List<PaperDO>();
            DataSet dataset = new NTSDA().GetAllNTSPaper();
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

        public QuestionsListSubjectDO GetAllNTSQuestionsByPaperID(int paperid)
        {
            QuestionsListSubjectDO obj = new QuestionsListSubjectDO();
            List<QuestionDO> list = new List<QuestionDO>();
            DataSet dataset = new NTSDA().GetAllNTSQuestionsByPaperID(paperid);
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
                        string[] choices = allChoices.Split(',','،');
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
            obj.PaperList = GetAllNTSPaper();
            return obj;
        }

        public QuestionDO GetNTSQuestionById(int questionid)
        {
            DataSet dataset = new NTSDA().GetNTSQuestionById(questionid);
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

        public bool DeleteNTSQuestionById(int questionid)
        {
            int result = new NTSDA().DeleteNTSQuestionById(questionid);
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