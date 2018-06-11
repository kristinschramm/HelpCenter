using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelpCenter.Models
{
    public class LeaseHolder : User
    {
        public int LocationId { get; set; }
        public Location Location { get; set; }

        public int UnitId { get; set; }
        public Unit Unit { get; set; }
    }
}