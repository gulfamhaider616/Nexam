using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nexam
{
    public class SubCategoryDO
    {
        public int SubCategoryID { get; set; }
        public int SubjectID { get; set; }
        public string Name { get; set; }
        public string Detail { get; set; }
    }
}