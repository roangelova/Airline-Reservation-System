using AirlineReservationSystem.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineReservationSystem.Core.Models.User_Area
{
    using static DataConstants;
    public class EditPassengerDataVM
    {
        [Required]
        [StringLength(NameMaxLength, ErrorMessage = "The input should not exceed {1}!")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(NameMaxLength, ErrorMessage = "The input should not exceed {1}!")]
        public string LastName { get; set; }

        [Required]
        public string DateOfBirth { get; set; }

        [Required]
        [StringLength(NationalityMaxLength, ErrorMessage ="The input should not exceed {1}!")]
        public string Nationality { get; set; }

        [Required]
        [StringLength(DocumentIdMaxLength, ErrorMessage = "The input should not exceed {1}!")]
        public string DocumentId { get; set; }
    }
}
