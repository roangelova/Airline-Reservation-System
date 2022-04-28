using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineReservationSystem.Core.Models.AdminArea.Aircraft
{
    public class GetAircraftDataVM
    {
        public string AircraftMakeAndModel { get; set; }
        public string AircraftId { get; set; }
        public string InUse { get; set; }
        public string Capacity { get; set; } 
        public string ImageUrl { get; set; }

    }
}
