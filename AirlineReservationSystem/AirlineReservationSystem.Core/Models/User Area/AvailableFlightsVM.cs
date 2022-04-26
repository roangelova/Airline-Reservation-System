namespace AirlineReservationSystem.Core.Models.User_Area
{
    public class AvailableFlightsVM
    {
        public string DepartureDestination { get; set; }
        public string ArrivalDestination { get; set; }

        public string Price { get; set; }

        public string DateAndTime { get; set; }

        public string FlightId { get; set; }
        public int Capacity { get; set; }
    }
}
