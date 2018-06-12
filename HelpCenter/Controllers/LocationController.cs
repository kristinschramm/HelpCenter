using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HelpCenter.Models;
using System.Data.Entity;

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
            var locations = _context.Locations
                .OrderBy(l => l.Name)
                .ToList();

            return View(locations);
        }

        public ActionResult Details(int id)
        {
            var location = _context.Locations.Single(l => l.Id == id);

            return View(location);
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