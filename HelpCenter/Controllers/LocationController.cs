using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HelpCenter.Models;

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
            return View();
        }
    }
}