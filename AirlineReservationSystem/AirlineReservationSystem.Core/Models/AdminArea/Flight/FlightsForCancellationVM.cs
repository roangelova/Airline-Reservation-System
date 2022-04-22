namespace AirlineReservationSystem.Core.Models.AdminArea.Flight
{
    public class FlightsForCancellationVM
    {
        public string DepartureDestination { get; set; }
        public string ArrivalDestination { get; set; }
        public string Price { get; set; }
        public string DateAndTime { get; set; }
        public string FlightId { get; set; }
        public string NumberOfBookings { get; set; }
    }
}
