using Nexam.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nexam.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //EmailFormModel model = new EmailFormModel();
            //Business.EmailSenderBA email = new Business.EmailSenderBA();
            //string res = email.SendEmail(model);
            //email.sendMessage();
            HomeContract homeContract = new HomeContract();
            homeContract.SubjectList = new MCQPrepBA().GetAllSubjects();
            homeContract.PPSCList = new PPSCBA().GetAllPPSCPaper();
            homeContract.NTSList = new NTSBA().GetAllNTSPaper();
            homeContract.FPSCList = new FPSCBA().GetAllFPSCPaper();
            homeContract.CSSList = new CSSBA().GetAllCSSPaper();
            //List<SubjectDO> list = new MCQPrepBA().GetAllSubjects();
            return View(homeContract);
        }

        //[Route("/nexam-about")]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        //[Route("/nexam/all-subjects")]
        public ActionResult Subjects()
        {
            List<SubjectDO> list = new MCQPrepBA().GetAllSubjects();
            return View(list);
        }

        /// <summary>
        /// Sub Categories for a subject
        /// </summary>
        /// <param name="SubjectID"></param>
        /// <returns></returns>
        public ActionResult GetSubCategoryBySubjectID(int SubjectID)
        {
            return View(new MCQPrepBA().GetSubCategoryBySubjectID(SubjectID));
        }
        public ActionResult GetAllQuestionsBySubjectID(int SubjectID)
        {
            return View(new MCQPrepBA().GetAllQuestionsBySubjectID(SubjectID));
        }

        public ActionResult SaveContact(string name,string email, string subject)
        {
            ContactDO contact = new ContactDO();
            contact.Name = name;
            contact.Email = email;
            contact.Subject = subject;

            bool result = new MCQPrepBA().SaveContact(contact);
            if(result)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View("Contact");
            }
        }

        public ActionResult SearchResult(string search)
        {
            return View(new MCQPrepBA().SearchResult(search));
        }

        public ActionResult Terms()
        {
            return View();
        }

        public ActionResult QuestionDiscussion(int questionID)
        {
            return View(new MCQPrepBA().GetQuestionById(questionID));
        }
    }
}