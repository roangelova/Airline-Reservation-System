﻿using AirlineReservationSystem.Core.Contracts;
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

            return user.PassengerId;
        }

        public async Task<IEnumerable<MyBookingsVM>> GetUserBookings(string id)
        {
            return await repo.All<Booking>()
                .Where(x => x.PassengerId == id)
                .Include(x => x.Flight)
                .Select(x => new MyBookingsVM
                {
                    BookingId = x.BookingNumber,
                    DepartureDestination = x.Flight.To.City,
                    ArrivalDestination = x.Flight.From.City,
                    DateAndTime = x.Flight.FlightInformation.ToString(),
                    FlightStatus = x.Flight.FlightStatus.ToString(),
                    FlightId = x.Flight.FlightId
                })
                .ToListAsync();
        }

        public async Task<(bool result, string passengerId)> RegisterPassenger(EditPassengerDataVM model)
        {
            var addedSuccessfully = false;
            var passengerId = "";

            var passenger = new Passenger()
            {
                DOB = DateTime.Parse(model.DateOfBirth),
                Nationality = model.Nationality,
                FirstName = model.FirstName,
                LastName = model.LastName,
                DocumentNumber = model.DocumentId
            };

            try
            {
                await repo.AddAsync(passenger);
                await repo.SaveChangesAsync();

                addedSuccessfully = true;
                passengerId = passenger.PassengerId;
            }
            catch (Exception)
            {
            }

            return (addedSuccessfully, passengerId);

        }
    }
}
