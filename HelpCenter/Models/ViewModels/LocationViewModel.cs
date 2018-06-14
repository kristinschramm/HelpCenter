using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelpCenter.Models.ViewModels
{
    public class LocationViewModel
    {
        public int OpenWorkOrderCount { get; set; }
        public Location Location { get; set; }

        public IEnumerable<UnitViewModel> Units { get; set; }

        public int UnitCount { get; set; }
    }
}