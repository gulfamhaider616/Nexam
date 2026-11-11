using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nexam
{
    public class NewPPSCPaperContract
    {
        public List<PaperYear> PaperYear { get; set; }
        public List<PaperPost> PaperPost { get; set; }
        public PaperDO Paper { get; set; }
    }
}