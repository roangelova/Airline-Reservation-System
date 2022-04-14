using AirlineReservationSystem.Infrastructure.Models;
using System.ComponentModel.DataAnnotations;

namespace AirlineReservationSystem.Core.Models.AdminArea.Aircraft
{
    using static DataConstants;
    public class AddAircraftVM
    {
        public string Id { get; set; }

        [Required]
        [StringLength(GeneralMaxLength, ErrorMessage = "{0} length must not exceed {1}!")]
        [Display(Name = "Aircraft Manufacturer")]
        public string Manufacturer { get; set; }

        [Required]
        [StringLength(GeneralMaxLength, ErrorMessage = "{0} length must not exceed {1}!")]
        [Display(Name = "Aircraft Model")]
        public string AircraftModel { get; set; }

        [Required]
        [Range(MinAircraftCapacity, MaxAircraftCapacity, ErrorMessage = "Aircrafts {0} must be between {1} and {2}!")]
        [Display(Name = "Aircraft Capacity")]
        public string Capacity { get; set; }

        [Url]
        [Required]
        [StringLength(ImageUrlMaxLength, ErrorMessage = "The {0} length must not exceed {1}!")]
        [Display(Name = "Aircraft image url")]
        public string ImageUrl { get; set; }
    }
}
