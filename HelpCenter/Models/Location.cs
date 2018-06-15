using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelpCenter.Models
{
    public class Location
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Zip { get; set; }

        public double Lat { get; set; }

        public double Lng { get; set; }
    }
}