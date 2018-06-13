using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelpCenter.Models.ViewModels
{
    public class LeaseHolderViewModel
    {
        public LeaseHolder LeaseHolder { get; set; }
        public int OpenWorkOrderCount { get; set; }
     }
}