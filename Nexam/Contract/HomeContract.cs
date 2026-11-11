using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nexam
{
    public class HomeContract
    {
        public List<SubjectDO> SubjectList { get; set; }
        public List<PaperDO> PPSCList { get; set; }
        public List<PaperDO> FPSCList { get; set; }
        public List<PaperDO> NTSList { get; set; }
        public List<PaperDO> CSSList { get; set; }
    }
}