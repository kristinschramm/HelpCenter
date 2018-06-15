using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using HelpCenter.Models;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using HelpCenter.Models.ViewModels;


namespace HelpCenter.Controllers
{
    [Authorize]
    public class LeaseHolderController : Controller
    {
        private ApplicationDbContext _context;

        public LeaseHolderController()
        {
            _context = new ApplicationDbContext();
        }

        // GET: LeaseHolder
        public ActionResult Index()
        {
            if (User.IsInRole(RoleName.LeaseHolder))
            {
                return RedirectToAction("Edit", new { id = User.Identity.GetUserId() });
            }
            else if(User.IsInRole(RoleName.Manager) || User.IsInRole(RoleName.Technician))
            {
                var viewModels = new List<LeaseHolderViewModel>();
                
                
                var leaseHolders = _context.AppUsers
                    .Where(l => l is LeaseHolder)
                    .OrderBy(l => l.NameLast)
                    .ThenBy(l => l.NameFirst)
                    .ToList();
                foreach (var leaseHolder in leaseHolders)
                {
                    var viewModel = new LeaseHolderViewModel();
                    viewModel.LeaseHolder = (LeaseHolder)leaseHolder;
                    viewModel.OpenWorkOrderCount = _context.WorkOrders
                        .Count(w => w.RequestorId == leaseHolder.Id);
                    
                    viewModel.Location = _context.Locations.Single(l => l.Id == viewModel.LeaseHolder.LocationId);

                    viewModel.Unit = _context.Units.Single(u => u.Id == viewModel.LeaseHolder.UnitId);

                    viewModels.Add(viewModel);
                }

                return View(viewModels);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult Details(string id)
        {
            if (User.IsInRole(RoleName.LeaseHolder))
            {
                id = User.Identity.GetUserId();
            }
            var leaseHolder = _context.LeaseHolders
                   .Single(l => l.Id == id);
            var locationList = _context.Locations.ToList();
            var unit = _context.Units.Single(u => u.Id == leaseHolder.UnitId);
            var workOrders = _context.WorkOrders.Include(w => w.Category).Include(w=>w.Status).Where(w => w.RequestorId == leaseHolder.Id).ToList();

            var viewModel = new LeaseHolderViewModel();
            {
                viewModel.LeaseHolder = leaseHolder;
                viewModel.Locations = locationList;
                viewModel.Location = leaseHolder.Location;
                viewModel.Unit = unit;
                viewModel.WorkOrders = workOrders;
            };
            return View(viewModel);
        }

        public ActionResult Edit(string id)
        {
            if (User.IsInRole(RoleName.LeaseHolder))
            {
                id = User.Identity.GetUserId();
            }

            var leaseHolder = _context.LeaseHolders
                    .Single(l => l.Id == id);
            var locationList = _context.Locations.ToList();
            var unit = _context.Units.Single(u => u.Id == leaseHolder.UnitId);

            var viewModel = new LeaseHolderViewModel();
            {
                viewModel.LeaseHolder = leaseHolder;
                viewModel.Locations = locationList;
                viewModel.Location = leaseHolder.Location;
                viewModel.Unit = unit;
            };
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Edit(string id, LeaseHolder leaseHolder)
        {
            if (User.IsInRole(RoleName.LeaseHolder))
            {
                id = User.Identity.GetUserId();

            }
            var leaseHolderInDb = _context.LeaseHolders
                                .Single(l => l.Id == id);

            if (leaseHolderInDb.EmailAddress != leaseHolder.EmailAddress)
            {
                var aspNetUser = Membership.GetUser(leaseHolderInDb.EmailAddress);
                leaseHolderInDb.EmailAddress = leaseHolder.EmailAddress;
                aspNetUser.Email = leaseHolderInDb.EmailAddress;
            }

            leaseHolderInDb.LocationId = leaseHolder.LocationId;
            leaseHolderInDb.NameFirst = leaseHolder.NameFirst;
            leaseHolderInDb.NameLast = leaseHolder.NameLast;
            leaseHolderInDb.PhoneNumber = leaseHolder.PhoneNumber;
            leaseHolderInDb.UnitId = leaseHolder.UnitId;

            _context.SaveChanges();

            TempData["ProfileUpdate"] = "success";

            return RedirectToAction("Index");
        }
    }
}