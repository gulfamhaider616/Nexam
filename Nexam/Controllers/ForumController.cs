using Nexam.Business;
using Nexam.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nexam.Controllers
{
    public class ForumController : Controller
    {
        // GET: Forum
        // GET: Forum
        public ActionResult ForumHomePage()
        {
            return View(new ForumBA().GetAllPublishedQuestion().ToList());
        }
        public ActionResult AskQuestion()
        {
            return View();
        }
        public ActionResult SaveQuestion(string uname, string qemail, string qcountry, string qsubject, string qexplanation)
        {
            AskQuestionDO question = new AskQuestionDO();
            question.UserName = uname;
            question.Email = qemail;
            question.Country = qcountry;
            question.Subject = qsubject;
            question.Explanation = qexplanation;
            bool res = new ForumBA().SaveQuestion(question);
            //if(res)
            //{
            return RedirectToAction("ForumHomePage", "Forum");
            //}
            //else
            //{
            //return View("AskQuestion");
            //}
        }
        public ActionResult SingleQuestion(int QuestionID)
        {
            return View(new ForumBA().GetSingleQuestion(QuestionID));
        }

        #region Forum

        [Authorize]
        public ActionResult ForumMainPage()
        {
            return View(new ForumBA().GetAllQuestions().ToList());
        }

        [Authorize]
        public ActionResult Publish()
        {
            return View(new ForumBA().GetAllPublishedQuestion().ToList());
        }

        [Authorize]
        public ActionResult UnPublish()
        {
            return View(new ForumBA().GetAllUnPublishedQuestion().ToList());
        }

        [Authorize]
        public JsonResult PublishQuestionByAdmin(int QuestionID)
        {
            return Json(new ForumBA().PublishQuestionByAdmin(QuestionID), JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public JsonResult DeleteQuestions(int QuestionID)
        {
            return Json(new ForumBA().DeleteQuestions(QuestionID), JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult AdminQuestionPreview(int QuestionID)
        {
            return View(new ForumBA().GetSingleQuestion(QuestionID));
        }
        #endregion
    }
}