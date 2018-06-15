using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HelpCenter.Models
{
    public class AppUser
    {
        public string Id { get; set; }

        [Required]
        [Display(Name ="First Name")]
        public string NameFirst { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string NameLast { get; set; }

        public string NameFirstLast
        {
            get { return NameFirst + " " + NameLast; }
        }

        public string NameLastFirst
        {
            get { return NameLast + ", " + NameFirst; }
        }

        public string NameFirstLastEmail
        {
            get { return NameFirst + " " + NameLast + " <" + EmailAddress + ">"; }
        }

        public string NameLastFirstEmail
        {
            get { return NameLast + ", " + NameFirst + " <" + EmailAddress + ">"; }
        }

        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Email Address")]

        public string EmailAddress { get; set; }
    }
}