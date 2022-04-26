using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirlineReservationSystem.Infrastructure.Models
{
    using static DataConstants;
    public class Flight
    {
        [Key]
        public string FlightId { get; set; } = Guid.NewGuid().ToString();

        [ForeignKey(nameof(From))]
        public string FromId { get; set; }

        [Required]
        public FlightRoute From { get; set; }

        [ForeignKey(nameof(To))]
        public string ToId { get; set; }

        [Required]
        public FlightRoute To { get; set; }

        [Required]
        [Column(TypeName = "datetime")]
        public DateTime FlightInformation { get; set; }

        public Status FlightStatus { get; set; }

        [Required]
        public Aircraft Aircraft { get; set; }

        [ForeignKey(nameof(Aircraft))]
        public string AircraftID { get; set; }

        [Required]
        [Range(MinTicketPrice, MaxTicketPrice)]
        public decimal StandardTicketPrice { get; set; }

        public int Capacity => this.Aircraft.Capacity;

       // public bool isAvailable => this.Capacity - Bookings.Count > 0;

        public List<Booking> Bookings = new List<Booking>();



    }
}
