using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Nexam
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            #region CSS
                routes.MapRoute(
                    "CSS-Past-Papers", // Route name
                    "CSS-Past-Papers", // URL with parameters
                    new { controller = "CSS", action = "GetAllCSSPapers" }// Parameter defaults
                   );
                routes.MapRoute(
                   "CSS-Past-Paper-Question", // Route name
                   "CSS-Past-Paper-Question/{paperid}", // URL with parameters
                   new { controller = "CSS", action = "GetCompleteCSSPaperByID" }// Parameter defaults
                  );
                //routes.MapRoute(
                // "CSS-Past-Paper-Question-Discussion", // Route name
                // "CSS-Past-Paper-Question-Discussion/{questionID}", // URL with parameters
                // new { controller = "CSS", action = "CSSQuestionDiscussion" }// Parameter defaults
                //);
            #endregion

            #region FPSC
            routes.MapRoute(
                "FPSC-Past-Papers", // Route name
                "FPSC-Past-Papers", // URL with parameters
                new { controller = "FPSC", action = "GetAllFPSCPapers" }// Parameter defaults
               );
            routes.MapRoute(
               "FPSC-Past-Paper-Question", // Route name
               "FPSC-Past-Paper-Question/{paperid}", // URL with parameters
               new { controller = "FPSC", action = "GetAllFPSCQuestionsByPaperID" }// Parameter defaults
              );
            routes.MapRoute(
             "FPSC-Past-Paper-Question-Discussion", // Route name
             "FPSC-Past-Paper-Question-Discussion/{questionID}", // URL with parameters
             new { controller = "FPSC", action = "FPSCQuestionDiscussion" }// Parameter defaults
            );
            #endregion

            #region NTS
            routes.MapRoute(
                "NTS-Past-Papers", // Route name
                "NTS-Past-Papers", // URL with parameters
                new { controller = "NTS", action = "GetAllNTSPapers" }// Parameter defaults
               );


            routes.MapRoute(
               "NTS-Past-Paper-Question", // Route name
               "NTS-Past-Paper-Question/{paperid}", // URL with parameters
               new { controller = "NTS", action = "GetAllNTSQuestionsByPaperID" }// Parameter defaults
              );

            routes.MapRoute(
             "NTS-Past-Paper-Question-Discussion", // Route name
             "NTS-Past-Paper-Question-Discussion/{questionID}", // URL with parameters
             new { controller = "NTS", action = "NTSQuestionDiscussion" }// Parameter defaults
            );
            #endregion

            #region ppsc
            routes.MapRoute(
                "PPSC-Past-Papers", // Route name
                "PPSC-Past-Papers", // URL with parameters
                new { controller = "PPSC", action = "GetAllPPSCPapers" }// Parameter defaults
               );


            routes.MapRoute(
               "PPSC-Past-Paper-Question", // Route name
               "PPSC-Past-Paper-Question/{paperid}", // URL with parameters
               new { controller = "PPSC", action = "GetAllQuestionsByPaperID" }// Parameter defaults
              );

            routes.MapRoute(
             "PPSC-Past-Paper-Question-Discussion", // Route name
             "PPSC-Past-Paper-Question-Discussion/{questionID}", // URL with parameters
             new { controller = "PPSC", action = "PPSCQuestionDiscussion" }// Parameter defaults
            );
            #endregion

            #region Forum

                routes.MapRoute(
                    "Nexam-Forum", // Route name
                    "Nexam-Forum", // URL with parameters
                    new { controller = "Forum", action = "ForumHomePage" }// Parameter defaults
                   );
                routes.MapRoute(
                    "Nexam-Forum-Question", // Route name
                    "Nexam-Forum-Question", // URL with parameters
                    new { controller = "Forum", action = "AskQuestion" }// Parameter defaults
                   );
                routes.MapRoute(
                   "Nexam-Forum-Question-Discussion", // Route name
                   "Nexam-Forum-Question-Discussion/{QuestionID}", // URL with parameters
                   new { controller = "Forum", action = "SingleQuestion" }// Parameter defaults
                  );

            #endregion

            #region Blog
            routes.MapRoute(
            "all-blogs", // Route name
            "all-blogs", // URL with parameters
            new { controller = "Blog", action = "ShowAllBlogs" }// Parameter defaults
           );
            #endregion

            routes.MapRoute(
            "user-login", // Route name
            "user-login", // URL with parameters
            new { controller = "Admin", action = "Login" }// Parameter defaults
           );

            routes.MapRoute(
            "Question-Details", // Route name
            "Question-Detail/{questionID}", // URL with parameters
            new { controller = "Home", action = "QuestionDiscussion" }// Parameter defaults
           );

            routes.MapRoute(
            "Terms", // Route name
            "nexam/terms-conditions", // URL with parameters
            new { controller = "Home", action = "Terms" }// Parameter defaults
           );

            routes.MapRoute(
            "Subjects", // Route name
            "nexam/all-subjects", // URL with parameters
            new { controller = "Home", action = "Subjects" }// Parameter defaults
           );

            routes.MapRoute(
            "Contact", // Route name
            "nexam-contact", // URL with parameters
            new { controller = "Home", action = "Contact" }// Parameter defaults
           );

            routes.MapRoute(
            "About", // Route name
            "nexam-about", // URL with parameters
            new { controller = "Home", action = "About" }// Parameter defaults
           );

            routes.MapRoute(
            "Questions", // Route name
            "Questions/{SubjectID}", // URL with parameters
            new { controller = "Home", action = "GetAllQuestionsBySubjectID" }// Parameter defaults
           );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
