using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HelpCenter.Models
{
    public class WorkOrderViewModel
    {
        public WorkOrder WorkOrder { get; set; }
        public List<WorkOrderComment> Comments { get; set; }
    }

    public class CreateWorkOrderViewModel
    {
        [Required]
        [StringLength(50)]
        public string Subject { get; set; }

        [Display(Name ="Category")]
        public int CategoryId { get; set; }
        public List<WorkOrderCategory> Categories { get; set; }

        [Display(Name = "Status")]
        public int StatusId { get; set; }
        public List<WorkOrderStatus> Statues { get; set; }

        [Display(Name = "Requestor")]
        public string RequestorId { get; set; }
        public List<AppUser> Users { get; set; }

        [Display(Name = "Location")]
        public int LocationId { get; set; }
        public List<Location> Locations { get; set; }

        [Display(Name = "Unit")]
        public int? UnitId { get; set; }

        [Display(Name = "Assigned Technician")]
        public string AssignedUserId { get; set; }
        public List<AppUser> Employees { get; set; }


        [Display(Name = "Expected Completion Date")]
        public DateTime? ExpectedCompletionDateTime { get; set; }

        [Required]
        [Display(Name = "Full Description")]
        public string WorkOrderDescription { get; set; }
    }
}