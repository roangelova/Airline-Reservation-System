namespace AirlineReservationSystem.Core.Models.AdminArea.Route
{
    using System.ComponentModel.DataAnnotations;
    
    public class ListFlightRouteVM
    {
        [Required]
        public string City { get; set; }
        public string Id { get; set; }
        public string IATA { get; set; }
    }
}
