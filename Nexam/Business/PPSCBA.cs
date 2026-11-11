using Nexam.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Nexam.Business
{
    public class PPSCBA
    {
        public bool SaveNewPaper(PaperDO paper)
        {
            DataSet set = new PPSCDA().SaveNewPaper(paper);
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
        public bool UpdatePPSCPaper(PaperDO paper)
        {
            DataSet set = new PPSCDA().UpdatePPSCPaper(paper);
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
        public bool SavePPSCPost(PostDO post)
        {
            DataSet set = new PPSCDA().SavePPSCPost(post);
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

        public bool SavePPSCQuestion(QuestionDO question)
        {
            DataSet set = new PPSCDA().SavePPSCQuestion(question);
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

        public List<PaperDO> GetAllPPSCPaper()
        {
            List<PaperDO> list = new List<PaperDO>();
            DataSet dataset = new PPSCDA().GetAllPPSCPaper();
            if (dataset.Tables.Count > 0)
            {
                foreach (DataRow dr in dataset.Tables[0].Rows)
                {
                    PaperDO paper = new PaperDO();
                    paper.PaperID = dr.Field<int>("PaperID");
                    paper.PaperYear = dr.Field<string>("YearValue");
                    paper.PaperPost = dr.Field<string>("PostName");
                    paper.PaperName = dr.Field<string>("PaperName");
                    paper.PaperDetail = dr.Field<string>("PaperDetail");
                    list.Add(paper);
                }
            }
            return list;
        }

        public List<PaperDO> SearchPPSCPaperYearwise(int yearid)
        {
            List<PaperDO> list = new List<PaperDO>();
            DataSet dataset = new PPSCDA().SearchPPSCPaperYearwise(yearid);
            if (dataset.Tables.Count > 0)
            {
                foreach (DataRow dr in dataset.Tables[0].Rows)
                {
                    PaperDO paper = new PaperDO();
                    paper.PaperID = dr.Field<int>("PaperID");
                    paper.PaperYear = dr.Field<string>("YearValue");
                    paper.PaperPost = dr.Field<string>("PostName");
                    paper.PaperName = dr.Field<string>("PaperName");
                    paper.PaperDetail = dr.Field<string>("PaperDetail");
                    list.Add(paper);
                }
            }
            return list;
        }
        public NewPPSCPaperContract GetPaperYearPost()
        {
            NewPPSCPaperContract contract = new NewPPSCPaperContract();
            List<PaperYear> year = new List<PaperYear>();
            List<PaperPost> post = new List<PaperPost>();
            DataSet dataset = new PPSCDA().GetPaperYearPost();
            if (dataset.Tables.Count > 0)
            {
                foreach (DataRow dr in dataset.Tables[0].Rows)
                {
                    PaperYear paper = new PaperYear();
                    paper.YearID = dr.Field<int>("YearID");
                    paper.Year = dr.Field<string>("YearValue");
                    year.Add(paper);
                }

                foreach (DataRow dr in dataset.Tables[1].Rows)
                {
                    PaperPost paper = new PaperPost();
                    paper.PostID = dr.Field<int>("PostID");
                    paper.Post = dr.Field<string>("PostName");
                    post.Add(paper);
                }
                contract.PaperYear = year;
                contract.PaperPost = post;
            }
            return contract;
        }

        public QuestionsListSubjectDO GetAllQuestionsByPaperID(int paperid)
        {
            QuestionsListSubjectDO obj = new QuestionsListSubjectDO();
            List<QuestionDO> list = new List<QuestionDO>();
            DataSet dataset = new PPSCDA().GetAllQuestionsByPaperID(paperid);
            if (dataset.Tables.Count > 0)
            {
                foreach (DataRow dr in dataset.Tables[0].Rows)
                {
                    QuestionDO question = new QuestionDO();
                    question.QuestionID = dr.Field<int>("QuestionID");
                    question.PaperID = dr.Field<int>("PaperID");
                    question.QuestionStatement = dr.Field<string>("QuestionStatement");
                    question.SubjectName = dr.Field<string>("SubjectName");
                    question.NumberOfChoices = dr.Field<int>("NumberOfChoices");
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
                {
                    obj.Paper = dataset.Tables[0].Rows[0].Field<string>("PaperName");
                    obj.PaperID = dataset.Tables[0].Rows[0].Field<int>("PaperID");
                }
            }
            obj.questions = list;
            obj.PaperList = GetAllPPSCPaper();
            return obj;
        }

        public PaperDO GetPPSCPaperDetail(int paperid)
        {
            PaperDO paper = new PaperDO();
            DataSet dataset = new PPSCDA().GetPPSCPaperDetail(paperid);
            if (dataset.Tables.Count > 0)
            {
                DataRow dr = dataset.Tables[0].Rows[0];
                paper.PaperID = dr.Field<int>("PaperID");
                paper.PaperPost = dr.Field<string>("PostName");
                paper.PaperYear = dr.Field<string>("YearValue");
                paper.PaperName = dr.Field<string>("PaperName");
                paper.PaperDetail = dr.Field<string>("PaperDetail");
            }
            return paper;
        }

        public QuestionDO GetPPSCQuestionById(int questionid)
        {
            DataSet dataset = new PPSCDA().GetPPSCQuestionById(questionid);
            QuestionDO question = new QuestionDO();
            if (dataset.Tables.Count > 0)
            {
                DataRow dr = dataset.Tables[0].Rows[0];
                question.QuestionID = dr.Field<int>("QuestionID");
                question.PaperID = dr.Field<int>("PaperID");
                question.PaperName = dr.Field<string>("PaperName");
                try
                {
                    question.SubjectID = dr.Field<int>("SubjectID");
                }
                catch(Exception ex)
                {
                    question.SubjectID = 1;
                }
                //if (!string.IsNullOrWhiteSpace(dr.Field<string>("SubjectID").ToString()))
                //{
                //    question.SubjectID = dr.Field<int>("SubjectID");
                //}
                question.SubjectName = dr.Field<string>("SubjectName");
                question.QuestionStatement = dr.Field<string>("QuestionStatement");
                question.NumberOfChoices = dr.Field<int>("NumberOfChoices");
                question.Choices = dr.Field<string>("Choices");
                question.CorrectAnswer = dr.Field<string>("CorrectAnswer");
            }
            return question;
        }

        public bool DeletePPSCQuestionById(int questionid)
        {
            int result = new PPSCDA().DeletePPSCQuestionById(questionid);
            if (result > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string PPSCPaperHTML(int PaperID)
        {
            QuestionsListSubjectDO obj = new QuestionsListSubjectDO();
            List<QuestionDO> list = new List<QuestionDO>();
            DataSet dataset = new PPSCDA().GetAllQuestionsByPaperID(PaperID);
            int iItemCounter = 1;
            StringBuilder _html = new StringBuilder();
            string papername = "";
            if (dataset.Tables[0].Rows.Count > 0)
            {
                papername = dataset.Tables[0].Rows[0].Field<string>("PaperName");
            }
            _html.Append("<!DOCTYPE html><html><head><meta http-equiv='Content-Type' content='text/html; charset=utf-8'/>"
                + "<title>"+ papername + "</title>"
                + "</head><body style='font-family: sans-serif;'>"
                + "<div class='page' style='page-break-after:always;' ><div class='cover-page'>"
                + "<div class='logo-campus' style='text-align: center;'>"
                + "<img src='http://nexam.com/images/trans-nexamLogo.png' style='width: 207px;margin-top: 68px;' /></div>"
                + "<div class='booklet-name' style='margin-top: 185px;font-size: 20px !important;text-align: center;'><b style='color: #093;'>Paper Name:</b>"
                + "<div class='assess-name' style='text-align: center;;word-wrap: break-word;font-size: 20px !important;'>" + papername + "</div>"
                + "<span class='ver-no'></span></div>"
                + "</div><div class='logo-sc' style='margin-top: 250px;text-align: center;'><img src='http://nexam.com/images/trans-nexamLogo.png' style='width: 110px;'/><br/><span>© All rights reserved | Nexam.</span></div></div>");
            if (dataset.Tables.Count > 0)
            {
                _html.Append("<div class='page'>");
                foreach (DataRow dr in dataset.Tables[0].Rows)
                {
                    _html.Append("<div class='question' style='page-break-inside: avoid;padding: 15px;'><div class='questionstatement' style='display: flex;'><span class='Item-Counter' style='margin-right: 15px;'><strong>" + iItemCounter + ".</strong></span><span class='Item-Container'>");
                    _html.Append("<strong>"+dr.Field<string>("QuestionStatement")+"</strong></span></div>");
                    _html.Append("<ol style='list-style-type: upper-alpha;line-height: 26px;'>");
                    if (!String.IsNullOrEmpty(dr.Field<string>("Choices")))
                    {
                        string allChoices = dr.Field<string>("Choices");
                        string[] choices = allChoices.Split(',', '،');
                        foreach (var choice in choices)
                        {
                            _html.Append("<li>" + choice + "</li>");
                        }
                    }
                    _html.Append("</ol>");
                    _html.Append("<div><strong> Correct Answer : " + dr.Field<string>("CorrectAnswer") + "</strong></div></div>");
                    iItemCounter++;
                }
                _html.Append("<div><hr style='width=100%'/><span>© All rights reserved | Nexam.</span></div></div></body></html>");     
            }
            return _html.ToString();
        }
    }
}