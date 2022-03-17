using System.ComponentModel.DataAnnotations;

namespace AirlineReservationSystem.Infrastructure.Models
{
    using static DataConstants;
    public class FlightRoute
    {
        [Key]
        public string RouteId { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [StringLength(GeneralMaxLength)]
        public string City { get; set; }

        [Required]
        [StringLength(IATACodeMaxLength)]
        public string IATA { get; set; }

    }
}
