using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelpCenter.Models
{
    public class Unit
    {
        public int Id { get; set; }

        public string Number { get; set; }

        public int LocationId { get; set; }
        public Location Location { get; set; }
    }
}