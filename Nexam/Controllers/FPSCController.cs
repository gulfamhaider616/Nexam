using Nexam.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nexam.Controllers
{
    public class FPSCController : Controller
    {
        // GET: FPSC
        [Authorize]
        public ActionResult FPSC_Home()
        {
            return View();
        }
        [Authorize]
        public ActionResult NewFPSCPaper()
        {
            return View();
        }
        [Authorize]
        public ActionResult SaveFPSCNewPaper(string name, string detail)
        {
            PaperDO paper = new PaperDO();
            paper.PaperName = name;
            paper.PaperDetail = detail;
            bool result = new FPSCBA().SaveFPSCNewPaper(paper);
            if (result)
            {
                return RedirectToAction("FPSC_Home");
            }
            else
            {
                return RedirectToAction("NewFPSCPaper");
            }
        }
        [Authorize]
        public ActionResult NewFPSCQuestion()
        {
            List<PaperDO> list = new FPSCBA().GetAllFPSCPaper();
            return View(list);
        }
        [Authorize]
        public ActionResult SaveNewFPSCQuestion(string paperid, string questionstatement, string choices, string correctanswer)
        {
            QuestionDO question = new QuestionDO();
            question.PaperID = Convert.ToInt32(paperid);
            question.QuestionStatement = questionstatement;
            question.Choices = choices;
            question.CorrectAnswer = correctanswer;
            bool result = new FPSCBA().SaveFPSCQuestion(question);
            if (result)
            {
                return RedirectToAction("NewFPSCQuestion");
            }
            else
            {
                return RedirectToAction("FPSC_Home");
            }
        }

        public ActionResult GetAllFPSCPapers()
        {
            List<PaperDO> list = new FPSCBA().GetAllFPSCPaper();
            return View(list);
        }
        public ActionResult GetAllFPSCPapersByAdmin()
        {
            List<PaperDO> list = new FPSCBA().GetAllFPSCPaper();
            return View(list);
        }

        public ActionResult GetAllFPSCQuestionsByPaperID(int paperid)
        {
            return View(new FPSCBA().GetAllFPSCQuestionsByPaperID(paperid));
        }

        [Authorize]
        public ActionResult GetAllFPSCQuestionsByPaperIDAdmin(int paperid)
        {
            return View(new FPSCBA().GetAllFPSCQuestionsByPaperID(paperid));
        }

        public ActionResult GetFPSCQuestionById(int questionID)
        {
            return View(new FPSCBA().GetFPSCQuestionById(questionID));
        }

        [Authorize]
        public ActionResult DeleteFPSCQuestionById(int questionID)
        {
            var result = new FPSCBA().DeleteFPSCQuestionById(questionID);
            return RedirectToAction("GetAllFPSCPapersByAdmin");
        }

        public ActionResult FPSCQuestionDiscussion(int questionID)
        {
            return View(new FPSCBA().GetFPSCQuestionById(questionID));
        }
    }
}