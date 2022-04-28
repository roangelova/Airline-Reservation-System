namespace AirlineReservationSystem.Core.Models.User_Area
{
    public class MyBookingsVM
    {
        public string BookingId { get; set; }
        public string DepartureDestination { get; set; }
        public string ArrivalDestination { get; set; }
        public string DateAndTime { get; set; }
        public string FlightStatus { get; set; }
        public string BookingStatus { get; set; }

        public string BaggagePiecesCount { get; set; }
        public string FlightId { get; set; }
    }
}
