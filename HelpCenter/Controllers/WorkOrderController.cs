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
            else if (User.IsInRole(RoleName.Manager) || User.IsInRole(RoleName.Technician))
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
            else
            {
                return RedirectToAction("Index", "Home");
            }

            return View(workOrders);
        }

        public ActionResult Details (int id)
        {
            var workOrder = _context.WorkOrders
                .Include(w => w.AssignedUser)
                .Include(w => w.Category)
                .Include(w => w.Location)
                .Include(w => w.Requestor)
                .Include(w => w.Status)
                .Include(w => w.Unit)
                .Single(w => w.Id == id);

            if (User.IsInRole(RoleName.LeaseHolder))
            {
                if(User.Identity.GetUserId() == workOrder.RequestorId)
                {
                    var comments = _context.WorkOrderComments.Where(w => w.WorkOrderId == id).ToList();
                    var viewModel = new WorkOrderViewModel()
                    {
                        WorkOrder = workOrder,
                        Comments = comments
                    };

                    return View(viewModel);
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            else if(User.IsInRole(RoleName.Manager) || User.IsInRole(RoleName.Technician))
            {
                var comments = _context.WorkOrderComments.Where(w => w.WorkOrderId == id).ToList();
                var viewModel = new WorkOrderViewModel()
                {
                    WorkOrder = workOrder,
                    Comments = comments
                };
                return View(viewModel);
            }

            return RedirectToAction("Index", "Home");
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
            var sendStatusEmail = false;

            if(workOrderInDb.AssignedUserId != workOrder.AssignedUserId)
            {
                workOrderInDb.AssignedUserId = workOrder.AssignedUserId;
                sendStatusEmail = true;
            }
            workOrderInDb.CategoryId = workOrder.CategoryId;
            if(workOrderInDb.ExpectedCompletionDateTime != workOrder.ExpectedCompletionDateTime)
            {
                workOrderInDb.ExpectedCompletionDateTime = workOrder.ExpectedCompletionDateTime;
                sendStatusEmail = true;
            }
            
            workOrderInDb.LocationId = workOrder.LocationId;
            workOrderInDb.ModifiedDateTime = DateTime.Now;
            workOrderInDb.RequestorId = workOrder.RequestorId;
            if(workOrderInDb.StatusId != workOrder.StatusId)
            {
                workOrderInDb.StatusId = workOrder.StatusId;
                workOrderInDb.StatusDateTime = DateTime.Now;
                sendStatusEmail = true;
            }
            workOrderInDb.Subject = workOrder.Subject;
            workOrderInDb.UnitId = workOrder.UnitId;

            _context.SaveChanges();

            if (sendStatusEmail)
            {
                // Send Email Here
            }

            return RedirectToAction("Index");
        }

        public ActionResult Status(string id)
        {
            var status = id;

            List<WorkOrder> workOrders;
            if (User.IsInRole(RoleName.LeaseHolder))
            {
                var userId = User.Identity.GetUserId();

                workOrders = _context.WorkOrders
                .Include(w => w.AssignedUser)
                .Include(w => w.Category)
                .Include(w => w.Location)
                .Include(w => w.Requestor)
                .Include(w => w.Status)
                .Include(w => w.Unit)
                .Where(w => w.Status.Name == status.ToUpper() && w.RequestorId == userId)
                .OrderByDescending(w => w.CreateDateTime)
                .ToList();
            }
            else if (User.IsInRole(RoleName.Manager) || User.IsInRole(RoleName.Technician))
            {
                workOrders = _context.WorkOrders
                .Include(w => w.AssignedUser)
                .Include(w => w.Category)
                .Include(w => w.Location)
                .Include(w => w.Requestor)
                .Include(w => w.Status)
                .Include(w => w.Unit)
                .Where(w => w.Status.Name == status.ToUpper())
                .OrderByDescending(w => w.CreateDateTime)
                .ToList();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }


            return View("Index", workOrders);
        }

        public ActionResult Open()
        {
            List<WorkOrder> workOrders;
            if (User.IsInRole(RoleName.LeaseHolder))
            {
                var userId = User.Identity.GetUserId();

                workOrders = _context.WorkOrders
                .Include(w => w.AssignedUser)
                .Include(w => w.Category)
                .Include(w => w.Location)
                .Include(w => w.Requestor)
                .Include(w => w.Status)
                .Include(w => w.Unit)
                .Where(w => w.Status.IsOpen == true && w.RequestorId == userId)
                .OrderByDescending(w => w.ModifiedDateTime)
                .ThenByDescending(w => w.CreateDateTime)
                .ToList();
            }
            else if(User.IsInRole(RoleName.Manager) || User.IsInRole(RoleName.Technician))
            {
                workOrders = _context.WorkOrders
                .Include(w => w.AssignedUser)
                .Include(w => w.Category)
                .Include(w => w.Location)
                .Include(w => w.Requestor)
                .Include(w => w.Status)
                .Include(w => w.Unit)
                .Where(w => w.Status.IsOpen == true)
                .OrderByDescending(w => w.ModifiedDateTime)
                .ThenByDescending(w => w.CreateDateTime)
                .ToList();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            

            return View("Index", workOrders);
        }
    }
}