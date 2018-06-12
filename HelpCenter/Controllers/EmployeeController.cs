using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using HelpCenter.Models;
using System.Data.Entity;

namespace HelpCenter.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private ApplicationDbContext _context;

        public EmployeeController()
        {
            _context = new ApplicationDbContext();
        }

        // GET: Employee
        public ActionResult Index()
        {
            var employees = _context.AppUsers
                .Select(e => !(e is LeaseHolder))
                .ToList();

            return View(employees);
        }
        
        public ActionResult Detail (string id)
        {
            var employee = _context.AppUsers.Single(e => e.Id == id);
            return View(employee);
        }

        [Authorize(Roles = RoleName.Manager)]
        public ActionResult Edit (string id)
        {
            var employee = _context.AppUsers.Single(e => e.Id == id);
            return View(employee);
        }

        [HttpPost]
        [Authorize(Roles = RoleName.Manager)]
        public ActionResult Edit (string id, AppUser employee)
        {
            var employeeInDb = _context.AppUsers.Single(e => e.Id == id);

            if(employeeInDb.EmailAddress != employee.EmailAddress)
            {
                var aspNetUser = Membership.GetUser(employeeInDb.EmailAddress);
                employeeInDb.EmailAddress = employee.EmailAddress;
                aspNetUser.Email = employeeInDb.EmailAddress;
            }
            employeeInDb.NameFirst = employee.NameFirst;
            employeeInDb.NameLast = employee.NameLast;
            employeeInDb.PhoneNumber = employee.PhoneNumber;

            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}