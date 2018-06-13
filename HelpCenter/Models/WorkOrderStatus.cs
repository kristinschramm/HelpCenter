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

        public static readonly int New = 1;
        public static readonly int Assigned = 2;
        public static readonly int InProgress = 3;
        public static readonly int Completed = 4;
    }
}