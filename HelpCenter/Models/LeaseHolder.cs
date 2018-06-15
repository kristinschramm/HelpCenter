using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HelpCenter.Models
{
    public class LeaseHolder : AppUser
    {
        [Display(Name="Location")]
        public int LocationId { get; set; }
        public Location Location { get; set; }

        [Display(Name ="Unit")]
        public int UnitId { get; set; }
        public Unit Unit { get; set; }
    }
}