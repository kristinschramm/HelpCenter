using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelpCenter.Models
{
    public static class RoleName
    {
        public const string Manager = "Manager";
        public const string Technician = "Technician";
        public const string LeaseHolder = "LeaseHolder";
        public const string EmployeeRoles = Manager + ", " + Technician;
    }
}