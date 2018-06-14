using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HelpCenter.Models;
using System.Data.Entity;
using HelpCenter.Models.ViewModels;

namespace HelpCenter.Controllers
{
    public class LocationController : Controller
    {
        private ApplicationDbContext _context;

        public LocationController()
        {
            _context = new ApplicationDbContext();
        }

        // GET: Location
        
        public ActionResult Index()
        {
            if (User.IsInRole(RoleName.Manager) || User.IsInRole(RoleName.Technician))
            {
                var viewModels = new List<LocationViewModel>();


                var locations = _context.Locations
                    .OrderBy(l => l.Name)
                    .ToList();

                foreach (var location in locations)
                {
                    var viewModel = new LocationViewModel();
                    viewModel.Location= location;
                    viewModel.OpenWorkOrderCount = _context.WorkOrders
                        .Count(w => w.LocationId == location.Id);


                    viewModel.Units = _context.Units.Where(u => u.LocationId == location.Id).ToList();

                    viewModel.UnitCount = viewModel.Units.Count();

                    viewModels.Add(viewModel);
                }

                return View(viewModels);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult Details(int id)
        {
            var viewModels = new List<UnitViewModel>();
            var location = _context.Locations.Single(l => l.Id == id);
            var units = _context.Units.Where(u => u.LocationId == id).ToList();
            var appUsers = _context.AppUsers.Where(l => l is LeaseHolder).ToList();
            var leaseHolders = new List<LeaseHolder>();

            foreach (var leaseHolder in appUsers)
            {
                leaseHolders.Add((LeaseHolder)leaseHolder);
                
            }                       

            foreach (var unit in units)
            {
                var viewModel = new UnitViewModel();
                viewModel.Unit= unit;
                viewModel.LeaseHolder = leaseHolders.Single(l => l.UnitId == viewModel.Unit.Id);
                viewModel.WorkOrders = _context.WorkOrders.Where(w => w.UnitId == viewModel.Unit.Id).ToList();
                viewModel.Location = location;

                viewModels.Add(viewModel);
            }


            return View(viewModels);
        }

        public ActionResult Edit (int id)
        {
            var location = _context.Locations.Where(l => l.Id == id);

            return View(location);
        }

        [HttpPost]
        public ActionResult Edit(int id, Location location)
        {
            var locationInDb = _context.Locations.Single(l => l.Id == id);

            locationInDb.Name = location.Name;
            locationInDb.Address = location.Address;
            locationInDb.City = location.City;
            locationInDb.State = location.State;
            locationInDb.Zip = location.Zip;

            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}