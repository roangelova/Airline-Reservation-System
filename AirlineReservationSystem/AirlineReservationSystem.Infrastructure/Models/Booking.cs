using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirlineReservationSystem.Infrastructure.Models
{
    using static DataConstants;
    public class Booking
    {
        [Key]
        public string BookingNumber { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public Flight Flight { get; set; }

        [ForeignKey(nameof(Flight))]
        public string FlightId { get; set; }

        public Status BookingStatus { get; set; }

        [Required]
        public Passenger Passenger { get; set; }

        [ForeignKey(nameof(Passenger))]
        public string PassengerId { get; set; }


    }
}
