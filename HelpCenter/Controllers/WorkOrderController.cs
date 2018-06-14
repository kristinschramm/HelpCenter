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

        public ActionResult Closed()
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
                .Where(w => w.Status.IsOpen == false && w.RequestorId == userId)
                .OrderByDescending(w => w.ModifiedDateTime)
                .ThenByDescending(w => w.CreateDateTime)
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
                .Where(w => w.Status.IsOpen == false)
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

        [Authorize]
        public ActionResult Create()
        {
            var viewModel = new CreateWorkOrderViewModel()
            {
                Categories = _context.WorkOrderCategories.OrderBy(w => w.Name).ToList(),
                Locations = _context.Locations.OrderBy(l => l.Name).ToList(),
                Statues = _context.WorkOrderStatus.ToList(),
                Employees = _context.AppUsers.Where(a => !(a is LeaseHolder)).ToList(),
                Users = _context.AppUsers.ToList(),
                StatusId = WorkOrderStatus.New,
                RequestorId = User.Identity.GetUserId()
            };

            var appUser = _context.AppUsers.Where(u => u.Id == User.Identity.GetUserId());

            if(appUser is LeaseHolder)
            {
                var leaseHolder = (LeaseHolder)appUser;
                viewModel.LocationId = leaseHolder.LocationId;
                viewModel.UnitId = leaseHolder.UnitId;
            }

            return View(viewModel);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Create(CreateWorkOrderViewModel model)
        {
            var userId = User.Identity.GetUserId();
            var appUser = _context.AppUsers.Single(a => a.Id == userId);
            if (User.IsInRole(RoleName.LeaseHolder))
            {
                var leaseHolder = (LeaseHolder)appUser;
                model.RequestorId = User.Identity.GetUserId();
                model.AssignedUserId = null;
                model.ExpectedCompletionDateTime = null;
                model.LocationId = leaseHolder.LocationId;
                model.UnitId = leaseHolder.UnitId;
                model.StatusId = WorkOrderStatus.New;
            }

            if (!ModelState.IsValid)
            {
                model.Categories = _context.WorkOrderCategories.OrderBy(w => w.Name).ToList();
                model.Locations = _context.Locations.OrderBy(l => l.Name).ToList();
                model.Statues = _context.WorkOrderStatus.ToList();
                model.Employees = _context.AppUsers.Where(a => !(a is LeaseHolder)).ToList();
                model.Users = _context.AppUsers.ToList();

                return View(model);
            }

            var newWorkOrder = new WorkOrder()
            {
                AssignedUserId = model.AssignedUserId,
                CategoryId = model.CategoryId,
                CreateDateTime = DateTime.Now,
                ExpectedCompletionDateTime = model.ExpectedCompletionDateTime,
                LocationId = model.LocationId,
                ModifiedDateTime = DateTime.Now,
                RequestorId = model.RequestorId,
                StatusDateTime = DateTime.Now,
                StatusId = model.StatusId,
                Subject = model.Subject,
                UnitId = model.UnitId
            };

            _context.WorkOrders.Add(newWorkOrder);
            _context.SaveChanges();

            var newWorkOrderComment = new WorkOrderComment()
            {
                Comment = model.WorkOrderDescription,
                CommentorId = User.Identity.GetUserId(),
                CreateDateTime = DateTime.Now,
                WorkOrderId = newWorkOrder.Id
            };

            _context.WorkOrderComments.Add(newWorkOrderComment);
            _context.SaveChanges();

            newWorkOrder = _context.WorkOrders
                .Include(w => w.AssignedUser)
                .Include(w => w.Category)
                .Include(w => w.Location)
                .Include(w => w.Requestor)
                .Include(w => w.Status)
                .Include(w => w.Unit)
                .Single(w => w.Id == newWorkOrder.Id);

            var email = new EMail();
            email.CreateDateTime = DateTime.Now;
            email.ToEmailAddress = newWorkOrder.Requestor.EmailAddress;
            email.Subject = $"{newWorkOrder.Subject} - Work Order # {newWorkOrder.Id} Created";
            email.Body = $"Work Order #{newWorkOrder.Id} has been created per your request.";
            email.Body = $"Work Order #{newWorkOrder.Id} has been created per your request." + $"\n\nStatus: {newWorkOrder.Status.Name}";
            if(newWorkOrder.AssignedUser != null)
            {
                email.Body = email.Body + "\nAssinged To: " + newWorkOrder.AssignedUser.NameFirstLastEmail;
            }
            else
            {
                email.Body = email.Body + "\nAssigned To: Not Currently Assigned";
            }
            
            email.Body = email.Body + "\nLocation: " + newWorkOrder.Location == null ? "No Location Provided" : newWorkOrder.Location.Name + ", " + newWorkOrder.Location.Address;
            email.Body = email.Body + newWorkOrder.Unit.Number;
            email.Body = email.Body + $"\n\nCategory: {newWorkOrder.Category.Name}\nSubject: {newWorkOrder.Subject}";
            email.Body = email.Body + $"\n{newWorkOrderComment.Comment}";
            email.Sent = false;

            _context.EMails.Add(email);
            _context.SaveChanges();

            return RedirectToAction("Index");
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

        public JsonResult GetUnitsByLocationId(int locationId)
        {
            var units = _context.Units.Where(u => u.LocationId == locationId).ToList();

            return Json(units, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles =RoleName.EmployeeRoles)]
        public ActionResult MyWorkOrders()
        {
            var appUserId = User.Identity.GetUserId();

            var workOrders = _context.WorkOrders
                    .Include(w => w.AssignedUser)
                    .Include(w => w.Category)
                    .Include(w => w.Location)
                    .Include(w => w.Unit)
                    .Include(w => w.Requestor)
                    .Include(w => w.Status)
                    .Where(w => w.AssignedUserId == appUserId)
                    .ToList();

            return View("Index", workOrders);
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