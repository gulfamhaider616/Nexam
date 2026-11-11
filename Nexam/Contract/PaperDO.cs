using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nexam
{
    public class PaperDO
    {
        public int PaperID { get; set; }
        public string PaperName { get; set; }
        public string PaperYear { get; set; }
        public int PaperYearID { get; set; }
        public int PaperPostID { get; set; }
        public string PaperPost { get; set; }
        public string PaperDetail { get; set; }
        public string QuestionsText { get; set; }

        public string PaperSlug { get; set; }
    }
}