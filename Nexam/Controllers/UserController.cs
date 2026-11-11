using Nexam.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nexam.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult User_Registration()
        {
            return View();
        }
        public ActionResult SaveUser(string fname, string lname,string email,string password,string pnumber,string country, string city)
        {
            UserDO user = new UserDO();
            user.FName = fname;
            user.LName = lname;
            user.Email = email;
            user.Password = password;
            user.PNumber = pnumber;
            user.Country = country;
            user.City = city;
            bool result = new UserBA().SaveUser(user);
            if(result)
            {
                return RedirectToAction("UserLogin");
            }
            else
            {
                return RedirectToAction("User_Registration");
            } 
        }
        public ActionResult UserLogin()
        {
            return View();
        }
        public ActionResult VerifyUser(string email, string password)
        {
            UserDO user = new UserBA().VerifyUser(email, password);
            if (user != null)
            {
                Session["name"] = user.FName;
                Session["Id"] = user.Id;
                return RedirectToAction("Index","Home");
            }
            else
            {
                return View("UserLogin");
            }
        }
        public ActionResult GetUserProfile(int userID)
        {
            return View(new UserBA().GetUserProfile(userID));
        }

        public ActionResult UserDashboard()
        {
            if (Session["Id"] != null)
            {
                int userID = Convert.ToInt32(Session["Id"]);
                List<QuizDO> list = new UserBA().GetUserQuiz(userID);
                return View(list);
            }
            else
            {
                return View("UserLogin");
            }
        }
        public ActionResult GetUserQuiz(int userID)
        {
            return View();
        }
    }
}