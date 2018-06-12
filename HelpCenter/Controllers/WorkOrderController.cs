using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HelpCenter.Models;
using System.Data.Entity;
using Microsoft.AspNet.Identity;

namespace HelpCenter.Controllers
{
    [Authorize]
    public class WorkOrderController : Controller
    {
        private ApplicationDbContext _context;

        public WorkOrderController()
        {
            _context = new ApplicationDbContext();
        }

        // GET: WorkOrder
        public ActionResult Index()
        {
            IEnumerable<WorkOrder> workOrders;

            if (User.IsInRole(RoleName.LeaseHolder))
            {
                var userId = User.Identity.GetUserId();

                workOrders = _context.WorkOrders
                    .Include(w => w.AssignedUser)
                    .Include(w => w.Category)
                    .Include(w => w.Location)
                    .Include(w => w.Unit)
                    .Include(w => w.Requestor)
                    .Include(w => w.Status)
                    .Where(w => w.RequestorId == userId)
                    .ToList();
            }
            else
            {
                workOrders = _context.WorkOrders
                    .Include(w => w.AssignedUser)
                    .Include(w => w.Category)
                    .Include(w => w.Location)
                    .Include(w => w.Unit)
                    .Include(w => w.Requestor)
                    .Include(w => w.Status)
                    .ToList();
            }

            return View(workOrders);
        }

        [Authorize(Roles = RoleName.EmployeeRoles)]
        public ActionResult Edit (int id)
        {
            var workOrder = _context.WorkOrders.Single(w => w.Id == id);

            return View(workOrder);
        }

        [HttpPost]
        [Authorize(Roles = RoleName.EmployeeRoles)]
        public ActionResult Edit (int id, WorkOrder workOrder)
        {
            var workOrderInDb = _context.WorkOrders.Single(w => w.Id == id);

            workOrderInDb.AssignedUserId = workOrder.AssignedUserId;
            workOrderInDb.CategoryId = workOrder.CategoryId;
            workOrderInDb.ExpectedCompletionDateTime = workOrder.ExpectedCompletionDateTime;
            workOrderInDb.LocationId = workOrder.LocationId;
            workOrderInDb.ModifiedDateTime = DateTime.Now;
            workOrderInDb.RequestorId = workOrder.RequestorId;
            if(workOrderInDb.StatusId != workOrder.StatusId)
            {
                workOrderInDb.StatusId = workOrder.StatusId;
                workOrderInDb.StatusDateTime = DateTime.Now;
            }
            workOrderInDb.Subject = workOrder.Subject;
            workOrderInDb.UnitId = workOrder.UnitId;

            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}