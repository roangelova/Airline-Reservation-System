using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirlineReservationSystem.Infrastructure.Models
{
    public class ApplicationUser:IdentityUser
    {

        [StringLength(50)]
        public string? FirstName { get; set; }

        [StringLength(50)]
        public string? LastName { get; set; }

        public Passenger? Passenger { get; set; }

        [ForeignKey(nameof(Passenger))]
        public string? PassengerId { get; set; }

    }
}
