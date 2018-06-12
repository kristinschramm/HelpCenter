using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using HelpCenter.Models;
using System.Data.Entity;
using Microsoft.AspNet.Identity;

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
                var leaseHolders = _context.AppUsers
                    .Where(l => l is LeaseHolder)
                    .OrderBy(l => l.NameLast)
                    .ThenBy(l => l.NameFirst)
                    .ToList();

                return View(leaseHolders);
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
                .Include(l => l.Location)
                .Include(l => l.Unit)
                .Single(l => l.Id == id);

            return View(leaseHolder);
        }

        public ActionResult Edit(string id)
        {
            if (User.IsInRole(RoleName.LeaseHolder))
            {
                id = User.Identity.GetUserId();   
            }

            var leaseHolder = _context.LeaseHolders
                    .Include(l => l.Location)
                    .Include(l => l.Unit)
                    .Single(l => l.Id == id);
            return View(leaseHolder);
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