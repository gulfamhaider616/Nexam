using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nexam
{
    public class UserDO
    {
        public int Id { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PNumber { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
    }
}