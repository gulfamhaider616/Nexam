using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Nexam.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult VerifyAdmin(string adminname, string adminpassword)
        {
            string name = new MCQPrepBA().VerifyAdmin(adminname, adminpassword);
            if (!String.IsNullOrEmpty(name))
            {
                FormsAuthentication.SetAuthCookie(name, false);
                return RedirectToAction("GetAllSubjects");// View("GetAllStudents");
            }
            else
            {
                return View("Login");
            }
        }

        [Authorize]
        public ActionResult GetAllSubjects()
        {
            List<SubjectDO> list = new MCQPrepBA().GetAllSubjects();
            return View(list);
        }

        [Authorize]
        public ActionResult AddNewSubject()
        {
            return View();
        }

        [Authorize]
        public ActionResult AddNewQuestion()
        {
            List<SubjectDO> list = new MCQPrepBA().GetAllSubjects();
            return View(list);
        }

        [Authorize]
        public ActionResult GetContacts()
        {
            return View(new MCQPrepBA().GetContacts());
        }

        //[Authorize]
        public ActionResult Logout()
        {
            //FormsAuthentication.SignOut();
            Session["name"] = null;
            Session["Id"] = null;
            return RedirectToAction("Index", "Home", new { area = "" });
        }

        [Authorize]
        public ActionResult SaveSubject(string subjectname,string subjectdetail)
        {
            SubjectDO subject = new SubjectDO();
            subject.SubjectName = subjectname;
            subject.SubjectDetail = subjectdetail;
            bool result = new MCQPrepBA().SaveSubject(subject);
            if(result)
            {
                return RedirectToAction("GetAllSubjects");
            }
            else
            {
                return RedirectToAction("AddNewSubject");
            }
        }

        [Authorize]
        public ActionResult SaveQuestion(string subjectid, string questionstatement,string numberofchoices,string choices,string correctanswer)
        {
            QuestionDO question = new QuestionDO();
            question.SubjectID = Convert.ToInt32(subjectid);
            question.QuestionStatement = questionstatement;
            question.NumberOfChoices = Convert.ToInt32(numberofchoices);
            question.Choices = choices;
            question.CorrectAnswer = correctanswer;
            bool result = new MCQPrepBA().SaveQuestion(question);
            if (result)
            {
                return RedirectToAction("AddNewQuestion");
            }
            else
            {
                return RedirectToAction("GetAllSubjects");
            }
        }

        [Authorize]
        public ActionResult GetAllQuestionsByAdmin(int SubjectID)
        {
            return View(new MCQPrepBA().GetAllQuestionsBySubjectID(SubjectID));
        }

        [Authorize]
        public ActionResult GetQuestionById(int questionID)
        {
            return View(new MCQPrepBA().GetQuestionById(questionID));
        }

        [Authorize]
        public ActionResult SaveEditQuestion(string questionid, string subjectid, string questionstatement, string numberofchoices, string choices, string correctanswer)
        {
            QuestionDO question = new QuestionDO();
            question.QuestionID= Convert.ToInt32(questionid);
            question.SubjectID = Convert.ToInt32(subjectid);
            question.QuestionStatement = questionstatement;
            question.NumberOfChoices = Convert.ToInt32(numberofchoices);
            question.Choices = choices;
            question.CorrectAnswer = correctanswer;
            bool result = new MCQPrepBA().SaveEditQuestion(question);
            if (result)
            {
                return RedirectToAction("GetAllSubjects");
            }
            else
            {
                return RedirectToAction("AddNewQuestion");
            }
        }

      

        [Authorize]
        public ActionResult AdminSearchResult(string search)
        {
            return View(new MCQPrepBA().SearchResult(search));
        }

        public ActionResult DeleteQuestion(int questionID)
        {
            var result = new MCQPrepBA().DeleteQuestion(questionID);
            return RedirectToAction("GetAllSubjects");
        }
    }
}