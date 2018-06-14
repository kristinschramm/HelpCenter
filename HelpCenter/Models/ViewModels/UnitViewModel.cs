using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelpCenter.Models.ViewModels
{
    public class UnitViewModel
    {
        public Unit Unit { get; set; }
        
        public IEnumerable<WorkOrder> WorkOrders{get; set;}

        public LeaseHolder LeaseHolder { get; set; }

        public int OpenWorkOrderCount { get; set; }

        public Location Location { get; set; }
    }
}