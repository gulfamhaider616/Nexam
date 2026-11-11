namespace Nexam.Infrastructure;

public static class RouteExtensions
{
    /// <summary>The site's SEO-friendly URLs, followed by the conventional {controller}/{action}/{id?} route.</summary>
    public static void MapNexamRoutes(this IEndpointRouteBuilder app)
    {
        void Map(string pattern, string controller, string action) =>
            app.MapControllerRoute(name: pattern, pattern: pattern, defaults: new { controller, action });

        // Single sign-in for admins and users
        Map("Login", "Account", "Login");

        // CSS
        Map("CSS-Past-Papers", "CSS", "GetAllCSSPapers");
        Map("CSS-Past-Paper-Question/{paperid}", "CSS", "GetCompleteCSSPaperByID");

        // FPSC
        Map("FPSC-Past-Papers", "FPSC", "GetAllFPSCPapers");
        Map("FPSC-Past-Paper-Question/{paperid}", "FPSC", "GetAllFPSCQuestionsByPaperID");
        Map("FPSC-Past-Paper-Question-Discussion/{questionID}", "FPSC", "FPSCQuestionDiscussion");

        // NTS
        Map("NTS-Past-Papers", "NTS", "GetAllNTSPapers");
        Map("NTS-Past-Paper-Question/{paperid}", "NTS", "GetAllNTSQuestionsByPaperID");
        Map("NTS-Past-Paper-Question-Discussion/{questionID}", "NTS", "NTSQuestionDiscussion");

        // PPSC
        Map("PPSC-Past-Papers", "PPSC", "GetAllPPSCPapers");
        Map("PPSC-Past-Paper-Question/{paperid}", "PPSC", "GetAllQuestionsByPaperID");
        Map("PPSC-Past-Paper-Question-Discussion/{questionID}", "PPSC", "PPSCQuestionDiscussion");

        // Forum
        Map("Nexam-Forum", "Forum", "ForumHomePage");
        Map("Nexam-Forum-Question", "Forum", "AskQuestion");
        Map("Nexam-Forum-Question-Discussion/{QuestionID}", "Forum", "SingleQuestion");

        // Blog, login and general pages
        Map("all-blogs", "Blog", "ShowAllBlogs");
        Map("user-login", "Admin", "Login");
        Map("Question-Detail/{questionID}", "Home", "QuestionDiscussion");
        Map("nexam/terms-conditions", "Home", "Terms");
        Map("nexam/all-subjects", "Home", "Subjects");
        Map("nexam-contact", "Home", "Contact");
        Map("nexam-about", "Home", "About");
        Map("Questions/{SubjectID}", "Home", "GetAllQuestionsBySubjectID");

        app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
    }
}
