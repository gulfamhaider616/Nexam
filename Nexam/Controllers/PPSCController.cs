using Nexam.Business;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//using IronPdf;

namespace Nexam.Controllers
{
    public class PPSCController : Controller
    {
        // GET: PPSC
        [Authorize]
        public ActionResult PPSC_Home()
        {
            return View();
        }
        [Authorize]
        public ActionResult NewPaper()
        {
            NewPPSCPaperContract obj = new NewPPSCPaperContract();
            obj = new PPSCBA().GetPaperYearPost();
            return View(obj);
        }
        [Authorize]
        public ActionResult NewPPSCPost()
        {
            return View();
        }
        [Authorize]
        public ActionResult SaveNewPaper(string paperyear, string paperpost, string name, string detail)
        {
            PaperDO paper = new PaperDO();
            paper.PaperYearID = Convert.ToInt32(paperyear);
            paper.PaperPostID = Convert.ToInt32(paperpost);
            paper.PaperName = name;
            paper.PaperDetail = detail;
            bool result = new PPSCBA().SaveNewPaper(paper);
            if (result)
            {
                return RedirectToAction("PPSC_Home");
            }
            else
            {
                return RedirectToAction("NewPaper");
            }
        }
        [Authorize]
        public ActionResult SaveUpdatedPPSCPaper(string paperid, string paperyear, string paperpost, string name, string detail)
        {
            PaperDO paper = new PaperDO();
            paper.PaperID = Convert.ToInt32(paperid);
            paper.PaperYearID = Convert.ToInt32(paperyear);
            paper.PaperPostID = Convert.ToInt32(paperpost);
            paper.PaperName = name;
            paper.PaperDetail = detail;
            bool result = new PPSCBA().UpdatePPSCPaper(paper);
            if (result)
            {
                return RedirectToAction("GetAllPPSCPapersByAdmin");
            }
            else
            {
                return RedirectToAction("NewPaper");
            }
        }
        [Authorize]
        public ActionResult SavePPSCPost(string postname, string postdetail)
        {
            PostDO post = new PostDO();
            post.PostName = postname;
            post.PostDetail = postdetail;
            bool result = new PPSCBA().SavePPSCPost(post);
            if (result)
            {
                return RedirectToAction("NewPPSCPost");
            }
            else
            {
                return RedirectToAction("NewPaper");
            }
        }
        [Authorize]
        public ActionResult NewPPSCQuestion()
        {
            HomeContract contract = new HomeContract();
            contract.PPSCList = new PPSCBA().GetAllPPSCPaper();
            contract.SubjectList = new MCQPrepBA().GetAllSubjects();
            //List<PaperDO> list = new PPSCBA().GetAllPPSCPaper();
            return View(contract);
        }
        [Authorize]
        public ActionResult SaveNewPPSCQuestion(string paperid, string subjectid, string questionstatement, string numberofchoices, string choices, string correctanswer)
        {
            QuestionDO question = new QuestionDO();
            question.PaperID = Convert.ToInt32(paperid);
            question.SubjectID = Convert.ToInt32(subjectid);
            question.QuestionStatement = questionstatement;
            question.NumberOfChoices = Convert.ToInt32(numberofchoices);
            question.Choices = choices;
            question.CorrectAnswer = correctanswer;
            bool result = new PPSCBA().SavePPSCQuestion(question);
            if (result)
            {
                return RedirectToAction("NewPPSCQuestion");
            }
            else
            {
                return RedirectToAction("PPSC_Home");
            }
        }

        public ActionResult GetAllPPSCPapers()
        {
            List<PaperDO> list = new PPSCBA().GetAllPPSCPaper();
            return View(list);
        }
        public ActionResult GetAllPPSCPapersByAdmin()
        {
            List<PaperDO> list = new PPSCBA().GetAllPPSCPaper();
            return View(list);
        }

        public ActionResult GetAllQuestionsByPaperID(int paperid)
        {
            return View(new PPSCBA().GetAllQuestionsByPaperID(paperid));
        }

        [Authorize]
        public ActionResult GetAllQuestionsByPaperIDAdmin(int paperid)
        {
            return View(new PPSCBA().GetAllQuestionsByPaperID(paperid));
        }

        public ActionResult GetPPSCQuestionById(int questionID)
        {
            //HomeContract contract = new HomeContract();
            
            return View(new PPSCBA().GetPPSCQuestionById(questionID));
        }

        [Authorize]
        public ActionResult DeletePPSCQuestionById(int questionID)
        {
            var result = new PPSCBA().DeletePPSCQuestionById(questionID);
            return RedirectToAction("GetAllPPSCPapersByAdmin");
        }
        public ActionResult PPSCQuestionDiscussion(int questionID)
        {
            return View(new PPSCBA().GetPPSCQuestionById(questionID));
        }
        [Authorize]
        public ActionResult UpdatePPSCPaper(int paperid)
        {
            NewPPSCPaperContract obj = new NewPPSCPaperContract();
            obj = new PPSCBA().GetPaperYearPost();
            obj.Paper = new PPSCBA().GetPPSCPaperDetail(paperid);
            return View(obj);
        }
        [Authorize]
        public ActionResult SaveEditPPSCQuestion(string questionid, string subjectid, string questionstatement, string numberofchoices, string choices, string correctanswer)
        {
            QuestionDO question = new QuestionDO();
            question.QuestionID = Convert.ToInt32(questionid);
            question.SubjectID = Convert.ToInt32(subjectid);
            question.QuestionStatement = questionstatement;
            question.NumberOfChoices = Convert.ToInt32(numberofchoices);
            question.Choices = choices;
            question.CorrectAnswer = correctanswer;
            bool result = new MCQPrepBA().SaveEditPPSCQuestion(question);
            if (result)
            {
                return RedirectToAction("GetAllPPSCPapersByAdmin");
            }
            else
            {
                return RedirectToAction("PPSC_Home");
            }
        }
        public ActionResult Download_PPSC_Paper(int PaperID)
        {
            string html = new PPSCBA().PPSCPaperHTML(PaperID);
            string strStoredFileName = Guid.NewGuid() + ".pdf";
            string strFilePathUrl = "/PDFFiles/" + strStoredFileName;
            string strSaveFileUrl = "~/images/" + strFilePathUrl;
            var Renderer = new IronPdf.HtmlToPdf();
            var PDF = Renderer.RenderHtmlAsPdf(html);
            PDF.SaveAs(strSaveFileUrl);
            string startupPath = AppDomain.CurrentDomain.BaseDirectory + "images\\PDFFiles\\" + strStoredFileName;
            string startupPath1 = Environment.CurrentDirectory;
            System.Diagnostics.Process.Start(startupPath);
            Response.ContentType = "Application/pdf";
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + strStoredFileName);
            Response.TransmitFile(Server.MapPath("~/images/PDFFiles/" + strStoredFileName));
            Response.End();
            return RedirectToAction("GetAllPPSCPapers", "PPSC");
        }

        public ActionResult SearchPPSCPaperYearwise(int yearid)
        {
            List<PaperDO> list = new PPSCBA().SearchPPSCPaperYearwise(yearid);
            return View(list);
        }

    }
}