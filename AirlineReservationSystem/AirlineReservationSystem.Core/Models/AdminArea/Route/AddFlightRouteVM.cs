using AirlineReservationSystem.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineReservationSystem.Core.Models.AdminArea.Route
{
    using static DataConstants;
    public class AddFlightRouteVM
    {

        [Required]
        [Display(Name = "City Name")]
        public string City { get; set; }

        [Required]
        [StringLength(IATACodeMaxLength)]
        public string IATA { get; set; }

    }
}
