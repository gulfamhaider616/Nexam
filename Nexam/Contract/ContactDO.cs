using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nexam
{
    public class ContactDO
    {
        public int ContactID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string ContactDate { get; set; }
    }
}