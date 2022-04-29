using AirlineReservationSystem.Core.Contracts;
using AirlineReservationSystem.Core.Models.User_Area;
using AirlineReservationSystem.Infrastructure.Models;
using AirlineReservationSystem.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace AirlineReservationSystem.Core.Services
{
    public class PassengerService : IPassengerService
    {
        private readonly IApplicatioDbRepository repo;

        public PassengerService(IApplicatioDbRepository _repo)
        {
            repo = _repo;
        }

        public async Task<string> GetPassengerId(string UserId)
        {
            var user = await repo.All<ApplicationUser>()
                .FirstOrDefaultAsync(x => x.Id == UserId);

            if (user == null)
            {
                return "";
            }
            else
            {
                if (user.PassengerId is null)
                {
                    return "";
                }
                return user.PassengerId;
            }

        }

        /// <summary>
        /// Gets the bookings for the user with given Passenger Id. Also includes canceled bookings and baggage
        /// pieces count.
        /// </summary>
        public async Task<IEnumerable<MyBookingsVM>> GetUserBookings(string id)
        {
            var BaggageCount = await repo.All<Baggage>()
                .Where(x => x.PassengerId == id).ToListAsync();

            var Today = DateTime.Today;

            var Result = await repo.All<Booking>()
                            .Where(x => x.PassengerId == id)
                            .Include(x => x.Flight)
                            .Where(x => x.Flight.FlightInformation.Date >= Today)
                            .Select(x => new MyBookingsVM
                            {
                                BookingId = x.BookingNumber,
                                DepartureDestination = x.Flight.To.City,
                                ArrivalDestination = x.Flight.From.City,
                                DateAndTime = x.Flight.FlightInformation.ToString(),
                                FlightStatus = x.Flight.FlightStatus.ToString(),
                                FlightId = x.Flight.FlightId,
                                BookingStatus = x.BookingStatus.ToString()
                            })
                            .ToListAsync();

            Result.ForEach(x => x.BaggagePiecesCount = BaggageCount.Where(y => y.BookingId == x.BookingId).Count().ToString());

            return Result;


        }

        //Register a passenger for the user with the given id. Returns the Id, which is later used to set/match
        //the Passenger Id in the User 
        public async Task<(bool result, string passengerId)> RegisterPassenger(EditPassengerDataVM model)
        {
            var addedSuccessfully = false;
            var passengerId = "";

            DateTime date;
            if (!DateTime.TryParse(model.DateOfBirth, out date))
            {
                return (addedSuccessfully, passengerId);
            }

            try
            {
                var passenger = new Passenger()
                {
                    DOB = date,
                    Nationality = model.Nationality,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    DocumentNumber = model.DocumentId
                };

                await repo.AddAsync(passenger);
                await repo.SaveChangesAsync();

                addedSuccessfully = true;
                passengerId = passenger.PassengerId;
            }
            catch (Exception)
            {
                throw;
            }

            return (addedSuccessfully, passengerId);

        }
    }
}
