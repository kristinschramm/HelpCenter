using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelpCenter.Models
{
    public class WorkOrderComment
    {
        public int Id { get; set; }

        public int WorkOrderId { get; set; }
        public WorkOrder WorkOrder { get; set; }

        public string Comment { get; set; }

        public DateTime CreateDateTime { get; set; }

        public int CommentorId { get; set; }
        public AppUser Commentor { get; set; }
    }
}