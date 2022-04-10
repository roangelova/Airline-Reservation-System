using System.ComponentModel.DataAnnotations;

namespace AirlineReservationSystem.Core.Models.AdminArea.Aircraft
{
    public class AddAircraftVM
    {
        [Required]
        [Display(Name = "Aircraft Manufacturer")]
        public string Manufacturer { get; set; }

        [Required]
        [Display(Name = "Aircraft Model")]
        public string AircraftModel { get; set; }

        [Required]
        [Display(Name = "Aircraft Capacity")]
        public string Capacity { get; set; }
    }
}
