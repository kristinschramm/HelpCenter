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

        public static readonly string New = "NEW";
        public static readonly string Assigned = "ASSIGNED";
        public static readonly string InProgress = "IN PROGRESS";
        public static readonly string Completed = "COMPLETED";
    }
}