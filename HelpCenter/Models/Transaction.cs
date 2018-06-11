using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelpCenter.Models
{
    public class Transaction
    {
        public int Id { get; set; }

        public DateTime TransactionDateTime { get; set; }

        public string Description { get; set; }

        public int? WorkOrderId { get; set; }
        public WorkOrder WorkOrder { get; set; }

        public string LeaseHolderId { get; set; }
        public LeaseHolder LeaseHolder { get; set; }

        public int LocationId { get; set; }
        public Location Location { get; set; }

        public int UnitId { get; set; }
        public Unit Unit { get; set; }

        public double Amount { get; set; }


    }
}