using System.ComponentModel.DataAnnotations;

namespace AirlineReservationSystem.Infrastructure.Models
{
    using static DataConstants;
    public class Aircraft

    {
        [Key]
        public string AircraftId { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [StringLength(GeneralMaxLength)]
        public string Manufacturer { get; set; }

        [Required]
        [StringLength(GeneralMaxLength)]
        public string Model { get; set; }

        [Required]
        [StringLength(ImageUrlMaxLength)]
        public string ImageUrl { get; set; }

        [Required]
        [Range(MinAircraftCapacity, MaxAircraftCapacity)]
        public int Capacity { get; set; }

        public List<Flight> Flights { get; set; } = new List<Flight>();
    }
}
