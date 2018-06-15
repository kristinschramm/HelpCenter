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

            ViewBag.Title = "Work Orders";
            return View(workOrders);
        }

        public ActionResult AddComment(WorkOrderViewModel model)
        {
            if (ModelState.IsValid)
            {
                var newComment = new WorkOrderComment()
                {
                    Comment = model.NewComment,
                    CommentorId = User.Identity.GetUserId(),
                    CreateDateTime = DateTime.Now,
                    WorkOrderId = model.WorkOrder.Id
                };

                _context.WorkOrderComments.Add(newComment);

                var workOrderInDb = _context.WorkOrders.Single(w => w.Id == model.WorkOrder.Id);

                workOrderInDb.ModifiedDateTime = DateTime.Now;

                _context.SaveChanges();

                return RedirectToAction("Details", new { id = model.WorkOrder.Id });
            }
            ModelState.AddModelError(string.Empty, "Comment field is required.");
            return RedirectToAction("Details", new { id = model.WorkOrder.Id });
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

            ViewBag.Title = "Closed Work Orders";
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

            SendEmail("new", newWorkOrder);

            return RedirectToAction("Index", "WorkOrder");
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
                    var comments = _context.WorkOrderComments.Where(w => w.WorkOrderId == id).OrderByDescending(w => w.CreateDateTime).ToList();
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
                var comments = _context.WorkOrderComments
                    .Include(w => w.Commentor)
                    .Where(w => w.WorkOrderId == id)
                    .OrderByDescending(w => w.CreateDateTime)
                    .ToList();
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
            var viewModel = new WorkOrderEditViewModel()
            {
                Id = workOrder.Id,
                AssignedUserId = workOrder.AssignedUserId,
                CategoryId = workOrder.CategoryId,
                CreateDateTime = workOrder.CreateDateTime,
                ExpectedCompletionDateTime = workOrder.ExpectedCompletionDateTime,
                LocationId = workOrder.LocationId,
                ModifiedDateTime = workOrder.ModifiedDateTime,
                RequestorId = workOrder.RequestorId,
                StatusId = workOrder.StatusId,
                StatusDateTime = workOrder.StatusDateTime,
                Subject = workOrder.Subject,
                UnitId = workOrder.UnitId,
                Description = _context.WorkOrderComments.Where(w => w.WorkOrderId == workOrder.Id).OrderBy(w => w.CreateDateTime).FirstOrDefault().Comment
            };

            viewModel.Categories = _context.WorkOrderCategories.OrderBy(w => w.Name).ToList();
            viewModel.Locations = _context.Locations.OrderBy(l => l.Name).ToList();
            viewModel.Statuses = _context.WorkOrderStatus.ToList();
            viewModel.Users = _context.AppUsers.OrderBy(a => a.NameFirst).ThenBy(a => a.NameLast).ToList();
            viewModel.Employees = _context.AppUsers.Where(a => !(a is LeaseHolder)).OrderBy(a => a.NameFirst).ThenBy(a => a.NameLast).ToList();

            return View(viewModel);
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

            workOrderInDb = _context.WorkOrders
                .Include(w => w.AssignedUser)
                .Include(w => w.Category)
                .Include(w => w.Location)
                .Include(w => w.Requestor)
                .Include(w => w.Status)
                .Include(w => w.Unit)
                .Single(w => w.Id == workOrderInDb.Id);

            if (sendStatusEmail)
            {
                if(workOrderInDb.Status.IsOpen)
                {
                    SendEmail("updated", workOrderInDb);
                }
                else
                {
                    SendEmail("closed", workOrderInDb);
                }
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
            ViewBag.Title = "My Work Orders";
            return View("Index", workOrders);
        }

        public void SendEmail (string emailType, WorkOrder workOrder)
        {
            var email = new EMail();
            var workOrderDescription = _context.WorkOrderComments.Where(w => w.Id == workOrder.Id).OrderBy(w => w.CreateDateTime).First().Comment;
            var comments = _context.WorkOrderComments.Where(w => w.WorkOrderId == workOrder.Id).OrderBy(w => w.CreateDateTime).ToList();
            switch (emailType.ToLower())
            {
                case "new":
                    email.CreateDateTime = DateTime.Now;
                    email.ToEmailAddress = workOrder.Requestor.EmailAddress;
                    email.Subject = $"{workOrder.Subject} - Work Order # {workOrder.Id} Created";
                    email.Body = $"Work Order #{workOrder.Id} has been created per your request." + $"\n\r\n\rStatus: {workOrder.Status.Name}";
                    if (workOrder.AssignedUser != null)
                    {
                        email.Body = email.Body + "\n\r\n\r\n\rAssinged To: " + workOrder.AssignedUser.NameFirstLastEmail;
                    }
                    else
                    {
                        email.Body = email.Body + "\n\r\n\r\n\rAssigned To: Not Currently Assigned";
                    }

                    email.Body = workOrder.Location == null ? email.Body + "\n\rLocation: " + "No Location Provided" : email.Body + "\n\r\n\r\n\rLocation: " + workOrder.Location.Name + ", " + workOrder.Location.Address;
                    email.Body = email.Body + " " + workOrder.Unit.Number;
                    email.Body = email.Body + $"\n\r\n\r\n\r\n\rCategory: {workOrder.Category.Name}\n\r\n\r\n\rSubject: {workOrder.Subject}";
                    email.Body = email.Body + $"\n\r\n\r\n\r{workOrderDescription}";
                    email.Sent = false;
                    break;

                case "updated":
                default:
                    email.CreateDateTime = DateTime.Now;
                    email.ToEmailAddress = workOrder.Requestor.EmailAddress;
                    email.Subject = $"{workOrder.Subject} - Work Order # {workOrder.Id} Updated";
                    email.Body = $"Work Order #{workOrder.Id} has been updated." + $"\n\r\n\rStatus: {workOrder.Status.Name}";
                    email.Body = email.Body + "\n\r\n\r\n\r" + comments[comments.Count - 1].Comment;
                    if (workOrder.AssignedUser != null)
                    {
                        email.Body = email.Body + "\n\r\n\r\n\rAssinged To: " + workOrder.AssignedUser.NameFirstLastEmail;
                    }
                    else
                    {
                        email.Body = email.Body + "\n\r\n\r\n\rAssigned To: Not Currently Assigned";
                    }

                    email.Body = email.Body + workOrder.Location == null ? email.Body + "\n\r\n\r\n\rLocation: " + "No Location Provided" : email.Body + "\n\r\n\r\n\rLocation: " + workOrder.Location.Name + ", " + workOrder.Location.Address;
                    email.Body = email.Body + " " + workOrder.Unit.Number;
                    email.Body = email.Body + $"\n\r\n\r\n\r\n\rCategory: {workOrder.Category.Name}\n\r\n\r\n\rSubject: {workOrder.Subject}";
                    email.Body = email.Body + $"\n\r\n\r\n\r{workOrderDescription}";
                    email.Sent = false;
                    break;
                case "closed":
                    email.CreateDateTime = DateTime.Now;
                    email.ToEmailAddress = workOrder.Requestor.EmailAddress;
                    email.Subject = $"{workOrder.Subject} - Work Order # {workOrder.Id} Updated";
                    email.Body = $"Work Order #{workOrder.Id} has been completed." + $"\n\r\n\r\n\r\n\rStatus: {workOrder.Status.Name}";
                    email.Body = email.Body + "\n\r\n\r\n\r\n\r" + comments[comments.Count - 1].Comment;
                    if (workOrder.AssignedUser != null)
                    {
                        email.Body = email.Body + "\n\r\n\r\n\rAssinged To: " + workOrder.AssignedUser.NameFirstLastEmail;
                    }
                    else
                    {
                        email.Body = email.Body + "\n\r\n\r\n\rAssigned To: Not Currently Assigned";
                    }

                    email.Body = email.Body + workOrder.Location == null ? email.Body + "\n\r\n\r\n\rLocation: " + "No Location Provided" : email.Body + "\n\r\n\r\n\rLocation: " + workOrder.Location.Name + ", " + workOrder.Location.Address;
                    email.Body = email.Body + " " +  workOrder.Unit.Number;
                    email.Body = email.Body + $"\n\r\n\r\n\r\n\rCategory: {workOrder.Category.Name}\n\r\n\r\n\rSubject: {workOrder.Subject}";
                    email.Body = email.Body + $"\n\r\n\r\n\r{workOrderDescription}";
                    email.Sent = false;
                    break;

            }
            _context.EMails.Add(email);
            _context.SaveChanges();
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

            ViewBag.Title = status.ToUpper() + " Work Orders";
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

            ViewBag.Title = "Open Work Orders";
            return View("Index", workOrders);
        }
    }
}