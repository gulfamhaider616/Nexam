using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nexam
{
    public class BlogDO
    {
       public string BlogID { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Country { get; set; } 
        public string Title { get; set; }
        public string Detail { get; set; }
    }
}