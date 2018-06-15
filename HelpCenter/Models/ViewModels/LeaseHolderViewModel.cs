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
        public Location Location { get; set; }
        public List<Location> Locations { get; set; }

        public Unit Unit { get; set; }

        public List<WorkOrder> WorkOrders { get; set; }
     }
}