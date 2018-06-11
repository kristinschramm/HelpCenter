using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelpCenter.Models
{
    public class WorkOrderStatus
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsOpen { get; set; }
    }
}