using Nexam.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nexam.Controllers
{
    public class CSSController : Controller
    {
        // GET: CSS
        [Authorize]
        public ActionResult CSS_Home()
        {
            return View();
        }
        [Authorize]
        public ActionResult NewCSSPaper()
        {
            return View();
        }
        [Authorize]
        [ValidateInput(false)]
        public ActionResult SaveCSSNewPaper(string name, string detail,string questionstatement)
        {
            PaperDO paper = new PaperDO();
            paper.PaperName = name;
            paper.PaperDetail = detail;
            paper.QuestionsText = questionstatement;
            bool result = new CSSBA().SaveCSSNewPaper(paper);
            if (result)
            {
                return RedirectToAction("CSS_Home");
            }
            else
            {
                return RedirectToAction("NewCSSPaper");
            }
        }
        //public ActionResult NewCSSQuestion()
        //{
        //    List<PaperDO> list = new CSSBA().GetAllCSSPaper();
        //    return View(list);
        //}
        //[Authorize]
        //[ValidateInput(false)]
        //public ActionResult SaveNewCSSQuestion(string paperid, string questionstatement)
        //{
        //    QuestionDO question = new QuestionDO();
        //    question.PaperID = Convert.ToInt32(paperid);
        //    question.QuestionStatement = questionstatement;
        //    bool result = new CSSBA().SaveCSSQuestion(question);
        //    if (result)
        //    {
        //        return RedirectToAction("NewCSSQuestion");
        //    }
        //    else
        //    {
        //        return RedirectToAction("CSS_Home");
        //    }
        //}

        public ActionResult GetAllCSSPapers()
        {
            List<PaperDO> list = new CSSBA().GetAllCSSPaper();
            return View(list);
        }
        [Authorize]
        public ActionResult GetAllCSSPapersByAdmin()
        {
            List<PaperDO> list = new CSSBA().GetAllCSSPaper();
            return View(list);
        }

        public ActionResult GetCompleteCSSPaperByID(int paperid)
        {
            return View(new CSSBA().GetCompleteCSSPaperByID(paperid));
        }

        //[Authorize]
        //public ActionResult GetCompleteCSSPaperByID(int paperid)
        //{
        //    return View(new CSSBA().GetCompleteCSSPaperByID(paperid));
        //}

        //public ActionResult GetCSSQuestionById(int questionID)
        //{
        //    return View(new CSSBA().GetCSSQuestionById(questionID));
        //}

        //[Authorize]
        //public ActionResult DeleteCSSQuestionById(int questionID)
        //{
        //    var result = new CSSBA().DeleteCSSQuestionById(questionID);
        //    return RedirectToAction("GetAllCSSPapersByAdmin");
        //}

        //public ActionResult CSSQuestionDiscussion(int questionID)
        //{
        //    return View(new CSSBA().GetCSSQuestionById(questionID));
        //}
    }
}