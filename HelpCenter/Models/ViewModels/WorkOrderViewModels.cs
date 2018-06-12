using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelpCenter.Models
{
    public class WorkOrderViewModel
    {
        public WorkOrder WorkOrder { get; set; }
        public List<WorkOrderComment> Comments { get; set; }
    }
}