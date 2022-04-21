using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirlineReservationSystem.Infrastructure.Models
{
    using static DataConstants;
    public class Passenger
    {
        [Key]
        public string PassengerId { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [StringLength(NameMaxLength)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(NameMaxLength)]
        public string LastName { get; set; }    

        [Required]
        [Column(TypeName = "date")]
        public DateTime DOB { get; set; }

        [Required]
        public string Nationality { get; set; }

        [Required]
        [StringLength(DocumentIdMaxLength)]
        public string DocumentNumber { get; set; }


        public List<Booking> Bookings = new List<Booking>();

        public List<Baggage> Baggages = new List<Baggage>();

    }
}
