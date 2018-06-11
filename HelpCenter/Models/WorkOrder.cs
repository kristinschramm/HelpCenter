using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelpCenter.Models
{
    public class WorkOrder
    {
        public int Id { get; set; }

        public string Subject { get; set; }

        public int? CategoryId { get; set; }
        public WorkOrderCategory Category { get; set; }

        public int StatusId { get; set; }
        public WorkOrderStatus Status { get; set; }

        public DateTime StatusDateTime { get; set; }

        public string RequestorId { get; set; }
        public AppUser Requestor { get; set; }

        public int? LocationId { get; set; }
        public Location Location { get; set; }

        public int? UnitId { get; set; }
        public Unit Unit { get; set; }

        public string AssignedUserId { get; set; }
        public AppUser AssignedUser { get; set; }

        public DateTime CreateDateTime { get; set; }

        public DateTime ModifiedDateTime { get; set; }

        public DateTime? ExpectedCompletionDateTime { get; set; }


    }
}