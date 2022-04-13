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
        [StringLength(GeneralMaxLength, ErrorMessage = "{0} must not exceed {1}")]
        [Display(Name = "City Name")]
        public string City { get; set; }

        [Required]
        [StringLength(IATACodeMaxLength, ErrorMessage = "{0} must not exceed {1}")]
        public string IATA { get; set; }

    }
}
