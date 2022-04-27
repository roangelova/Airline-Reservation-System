using AirlineReservationSystem.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineReservationSystem.Core.Models.AdminArea.Flight
{
    using static DataConstants;
    public class AddFlightVM
    {
        [Required]
        public string DepartureCity { get; set; }
        [Required]
        public string ArrivalCity { get; set; }
        [Required]
        public string FlightInformation { get; set; }

        [Required]
        [Range(MinTicketPrice, MaxTicketPrice, ErrorMessage ="The price per seat must be between {1} and {2}!")]
        public string StandardTicketPrice{ get; set; }
        [Required]
        public string Aircraft{ get; set; }
    }
}
