using System.Text;
using Nexam.Models.Entities;

namespace Nexam.Business.Helpers;

/// <summary>Builds the printable HTML for a PPSC paper, which is then rendered to PDF.</summary>
internal static class PaperHtmlBuilder
{
    private const string LogoUrl = "http://nexam.com/images/trans-nexamLogo.png";

    public static string Build(IReadOnlyList<Question> questions)
    {
        var paperName = questions.Count > 0 ? questions[0].PaperName : "";
        var html = new StringBuilder();

        html.Append("<!DOCTYPE html><html><head><meta http-equiv='Content-Type' content='text/html; charset=utf-8'/>"
            + "<title>" + paperName + "</title>"
            + "</head><body style='font-family: sans-serif;'>"
            + "<div class='page' style='page-break-after:always;' ><div class='cover-page'>"
            + "<div class='logo-campus' style='text-align: center;'>"
            + "<img src='" + LogoUrl + "' style='width: 207px;margin-top: 68px;' /></div>"
            + "<div class='booklet-name' style='margin-top: 185px;font-size: 20px !important;text-align: center;'><b style='color: #093;'>Paper Name:</b>"
            + "<div class='assess-name' style='text-align: center;;word-wrap: break-word;font-size: 20px !important;'>" + paperName + "</div>"
            + "<span class='ver-no'></span></div>"
            + "</div><div class='logo-sc' style='margin-top: 250px;text-align: center;'><img src='" + LogoUrl + "' style='width: 110px;'/><br/><span>© All rights reserved | Nexam.</span></div></div>");

        html.Append("<div class='page'>");
        var counter = 1;
        foreach (var question in questions)
        {
            html.Append("<div class='question' style='page-break-inside: avoid;padding: 15px;'><div class='questionstatement' style='display: flex;'><span class='Item-Counter' style='margin-right: 15px;'><strong>" + counter + ".</strong></span><span class='Item-Container'>");
            html.Append("<strong>" + question.QuestionStatement + "</strong></span></div>");
            html.Append("<ol style='list-style-type: upper-alpha;line-height: 26px;'>");
            foreach (var option in QuestionHelper.ParseOptions(question.Choices) ?? new List<QuestionOption>())
                html.Append("<li>" + option.Choice + "</li>");
            html.Append("</ol>");
            html.Append("<div><strong> Correct Answer : " + question.CorrectAnswer + "</strong></div></div>");
            counter++;
        }
        html.Append("<div><hr style='width=100%'/><span>© All rights reserved | Nexam.</span></div></div></body></html>");

        return html.ToString();
    }
}
