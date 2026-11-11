using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Nexam.Business;

namespace Nexam.Controllers
{
    public class QuizController : Controller
    {
        // GET: Quiz
        public ActionResult QuizHome()
        {
            return View();
        }
        public ActionResult NewQuiz()
        {
            if (Session["name"] != null) {
                return View(new MCQPrepBA().GetAllSubjects().ToList());
            }
            else { return RedirectToAction("UserLogin","User"); }
        }
        
        public ActionResult SaveQuiz(string title, string subjectid,string totalquestions,string userid) 
        {
            QuizDO quiz=new QuizDO();
            quiz.Title = title;
            quiz.SubjectID = Convert.ToInt32(subjectid);
            quiz.TotalQuestions = Convert.ToInt32(totalquestions);
            quiz.UserID = Convert.ToInt32(userid);
            var result = new QuizBA().SaveQuiz(quiz);
            return View(result);
        }

        [HttpPost]
        public JsonResult InsertQuizQuestion(QuizQuestionContract quizquestioncontract)
        {
            var result = new QuizBA().QuizResult(quizquestioncontract);
            result.Name = Session["name"].ToString();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GeTQuizById(int quizID)
        {
            var result = new QuizBA().GeTQuizById(quizID);
            return View(result);
        }
    }
}