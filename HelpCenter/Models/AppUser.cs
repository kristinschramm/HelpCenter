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
        public string NameFirst { get; set; }
        [Required]
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

        public string PhoneNumber { get; set; }

        public string EmailAddress { get; set; }
    }
}