using Nexam.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Nexam.Business
{
    public class CSSBA
    {
        public bool SaveCSSNewPaper(PaperDO paper)
        {
            DataSet set = new CSSDA().SaveCSSNewPaper(paper);
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

        //public bool SaveCSSQuestion(QuestionDO question)
        //{
        //    DataSet set = new CSSDA().SaveCSSQuestion(question);
        //    if (set.Tables.Count > 0)
        //    {
        //        if (set.Tables[0].Rows.Count > 0)
        //        {
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        public List<PaperDO> GetAllCSSPaper()
        {
            List<PaperDO> list = new List<PaperDO>();
            DataSet dataset = new CSSDA().GetAllCSSPaper();
            if (dataset.Tables.Count > 0)
            {
                foreach (DataRow dr in dataset.Tables[0].Rows)
                {
                    PaperDO paper = new PaperDO();
                    paper.PaperID = dr.Field<int>("PaperID");
                    paper.PaperName = dr.Field<string>("PaperName");
                    paper.PaperDetail = dr.Field<string>("PaperDetail");
                    paper.PaperSlug = dr.Field<string>("PaperSlug");
                    list.Add(paper);
                }
            }
            return list;
        }

        public QuestionsListSubjectDO GetCompleteCSSPaperByID(int paperid)
        {
            QuestionsListSubjectDO obj = new QuestionsListSubjectDO();
            PaperDO paper = new PaperDO();
            DataSet dataset = new CSSDA().GetCompleteCSSPaperByID(paperid);
            if (dataset.Tables.Count > 0)
            {
                DataRow dr = dataset.Tables[0].Rows[0];
                paper.PaperID = dr.Field<int>("PaperID");
                paper.PaperName = dr.Field<string>("PaperName");
                paper.PaperDetail = dr.Field<string>("PaperDetail");
                paper.QuestionsText = dr.Field<string>("QuestionText");
                paper.PaperSlug = dr.Field<string>("PaperSlug");
            }
            obj.PaperDetail = paper;
            obj.PaperList = GetAllCSSPaper();
            return obj;
        }

        //public QuestionDO GetCSSQuestionById(int questionid)
        //{
        //    DataSet dataset = new CSSDA().GetCSSQuestionById(questionid);
        //    QuestionDO question = new QuestionDO();
        //    if (dataset.Tables.Count > 0)
        //    {
        //        DataRow dr = dataset.Tables[0].Rows[0];
        //        question.QuestionID = dr.Field<int>("QuestionID");
        //        question.PaperID = dr.Field<int>("PaperID");
        //        question.PaperName = dr.Field<string>("PaperName");
        //        question.QuestionStatement = dr.Field<string>("QuestionStatement");
        //        question.Choices = dr.Field<string>("Choices");
        //        question.CorrectAnswer = dr.Field<string>("CorrectAnswer");
        //    }
        //    return question;
        //}

        //public bool DeleteCSSQuestionById(int questionid)
        //{
        //    int result = new CSSDA().DeleteCSSQuestionById(questionid);
        //    if (result > 0)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}
    }
}