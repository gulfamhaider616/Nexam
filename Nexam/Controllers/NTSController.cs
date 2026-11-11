using Nexam.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nexam.Controllers
{
    public class NTSController : Controller
    {
        // GET: NTS
        [Authorize]
        public ActionResult NTS_Home()
        {
            return View();
        }
        [Authorize]
        public ActionResult NewNTSPaper()
        {
            return View();
        }
        [Authorize]
        public ActionResult SaveNTSNewPaper(string name, string detail)
        {
            PaperDO paper = new PaperDO();
            paper.PaperName = name;
            paper.PaperDetail = detail;
            bool result = new NTSBA().SaveNTSNewPaper(paper);
            if (result)
            {
                return RedirectToAction("NTS_Home");
            }
            else
            {
                return RedirectToAction("NewNTSPaper");
            }
        }
        [Authorize]
        public ActionResult NewNTSQuestion()
        {
            List<PaperDO> list = new NTSBA().GetAllNTSPaper();
            return View(list);
        }
        [Authorize]
        public ActionResult SaveNewNTSQuestion(string paperid, string questionstatement, string choices, string correctanswer)
        {
            QuestionDO question = new QuestionDO();
            question.PaperID = Convert.ToInt32(paperid);
            question.QuestionStatement = questionstatement;
            question.Choices = choices;
            question.CorrectAnswer = correctanswer;
            bool result = new NTSBA().SaveNTSQuestion(question);
            if (result)
            {
                return RedirectToAction("NewNTSQuestion");
            }
            else
            {
                return RedirectToAction("NTS_Home");
            }
        }

        public ActionResult GetAllNTSPapers()
        {
            List<PaperDO> list = new NTSBA().GetAllNTSPaper();
            return View(list);
        }
        public ActionResult GetAllNTSPapersByAdmin()
        {
            List<PaperDO> list = new NTSBA().GetAllNTSPaper();
            return View(list);
        }

        public ActionResult GetAllNTSQuestionsByPaperID(int paperid)
        {
            return View(new NTSBA().GetAllNTSQuestionsByPaperID(paperid));
        }

        [Authorize]
        public ActionResult GetAllNTSQuestionsByPaperIDAdmin(int paperid)
        {
            return View(new NTSBA().GetAllNTSQuestionsByPaperID(paperid));
        }

        public ActionResult GetNTSQuestionById(int questionID)
        {
            return View(new NTSBA().GetNTSQuestionById(questionID));
        }

        [Authorize]
        public ActionResult DeleteNTSQuestionById(int questionID)
        {
            var result = new NTSBA().DeleteNTSQuestionById(questionID);
            return RedirectToAction("GetAllNTSPapersByAdmin");
        }

        public ActionResult NTSQuestionDiscussion(int questionID)
        {
            return View(new NTSBA().GetNTSQuestionById(questionID));
        }
    }
}