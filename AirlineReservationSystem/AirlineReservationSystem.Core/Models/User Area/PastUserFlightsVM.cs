using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineReservationSystem.Core.Models.User_Area
{
    public class PastUserFlightsVM
    {
        public string DepartureDestination { get; set; }
        public string ArrivalDestination { get; set; }

        public string DateAndTime { get; set; }

        public string BookingId { get; set; }
    }
}
