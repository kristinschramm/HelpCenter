using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelpCenter.Models
{
    public class EMail
    {
        public int Id { get; set; }
        public DateTime CreateDateTime { get; set; }
        public string ToEmailAddress { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool Sent { get; set; }
    }
}