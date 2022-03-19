using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;


namespace AirlineReservationSystem.Infrastructure.Models
{
    public class ApplicationUser:IdentityUser
    {

        [StringLength(50)]
        public string? FirstName { get; set; }

        [StringLength(50)]
        public string? LastName { get; set; }


    }
}
