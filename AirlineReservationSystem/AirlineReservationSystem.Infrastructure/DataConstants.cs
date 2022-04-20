namespace AirlineReservationSystem.Infrastructure.Models
{
    public class DataConstants
    {

        public const int NameMaxLength = 150;

        public const int GeneralMaxLength = 100;

        public const int ImageUrlMaxLength = 300;

        public const int IATACodeMaxLength = 3;

        public const int DocumentIdMaxLength = 30;

        public const int NationalityMaxLength = 20;

       //Currently only 1 passenger per booking is supported
       // public const int BookingMinPassengers = 1;

       // public const int BookingMaxPassengers = 8;

        public const int MinAircraftCapacity = 150;

        public const int MaxAircraftCapacity = 450;

        public const int MinTicketPrice =50;

        public const int MaxTicketPrice = 4500;
    }
}
