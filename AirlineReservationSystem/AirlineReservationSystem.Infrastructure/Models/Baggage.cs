using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineReservationSystem.Infrastructure.Models
{
    public class Baggage
    {
        [Key]
        public string BaggageId { get; set; } = Guid.NewGuid().ToString();

        public BaggageSize Size { get; set; }

        public bool IsReportedLost { get; set; } = false;

        public Passenger Passenger { get; set; }

        [ForeignKey(nameof(Passenger))]
        public string PassengerId { get; set; }

        public Booking Booking { get; set; }

        [ForeignKey(nameof(Booking))]
        public string BookingId { get; set; }

    }
}
