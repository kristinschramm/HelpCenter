using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;

namespace HelpCenter.Models
{
    public class LeaseHolderIndexViewModel
    {
        LeaseHolder LeaseHolder { get; set;}
        WorkOrder WorkOrder { get; set; }

    }

    public class LeaseHolderProfileViewModel
    {
        public string Id { get; set; }
       
        public string Email { get; set; }

        [Display(Name ="Location")]
        public int LocationId { get; set; }
        public Location Location { get; set; }

        [Display(Name = "Unit Number")]
        public int UnitId { get; set; }
        public Unit Unit { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public string NameFirst { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string NameLast { get; set; }

        public string PhoneNumber { get; set; }

       
    }
}