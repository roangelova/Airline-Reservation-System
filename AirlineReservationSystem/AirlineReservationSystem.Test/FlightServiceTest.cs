using AirlineReservationSystem.Core.Contracts;
using AirlineReservationSystem.Core.Services;
using AirlineReservationSystem.Infrastructure.Models;
using AirlineReservationSystem.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AirlineReservationSystem.Test
{
    public class FlightServiceTest
    {
        private ServiceProvider serviceProvider;
        private InMemoryDbContext dbContext;

        [SetUp]
        public async Task Setup()
        {

            dbContext = new InMemoryDbContext();
            var serviceCollection = new ServiceCollection();

            serviceProvider = serviceCollection
                .AddSingleton(sp => dbContext.CreateContext())
                .AddSingleton<IApplicatioDbRepository, ApplicatioDbRepository>()
                .AddSingleton<IFlightService, FlightService>()
                .BuildServiceProvider();

        }

        [TearDown]
        public void TearDown()
        {
            dbContext.Dispose();
        }

        //TODO: Fix AddFlight and add Unit tests


        [Test]
        public async Task ShouldThrowAnExceptionIfFlightIdUnknown()
        {
            var service = serviceProvider.GetService<IFlightService>();

            string InvalidId = "ThisIsNotAValidId";

            Assert.CatchAsync<Exception>(async () => await service.CancelFlight(InvalidId));
        }

        [Test]
        public async Task ShouldCancelAFlightById()
        {
            var service = serviceProvider.GetService<IFlightService>();
            var repo = serviceProvider.GetService<IApplicatioDbRepository>();

            await SeedDbAsync(repo);

            var FlightToCancel = repo.All<Flight>()
                .FirstOrDefault();

            await service.CancelFlight(FlightToCancel.FlightId);

            var TotalCanceledFlights = await repo.All<Flight>()
                .Where(x => x.FlightStatus == Infrastructure.Status.Canceled)
                .ToListAsync();

            Assert.That(TotalCanceledFlights.Count.Equals(1));

        }


        /// <summary>
        /// Available flights are those that are not canceled or not at full capacity
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task ShouldReturnAllAvailableFlights()
        {
            var service = serviceProvider.GetService<IFlightService>();
            var repo = serviceProvider.GetService<IApplicatioDbRepository>();

            await SeedDbAsync(repo);

            var TotalAvailableFlights = await service.GetAllAvailableFlights();

            Assert.That(TotalAvailableFlights.ToList().Count.Equals(1));
        }

        [Test]
        public async Task ShouldReturnAllAvailableFlightsForCancellationAndTheNumberOfBookings()
        {
            var service = serviceProvider.GetService<IFlightService>();
            var repo = serviceProvider.GetService<IApplicatioDbRepository>();

            await SeedDbAsync(repo);

            var TotalAvailableFlights = await service.GetFlightsForCancellation();

            Assert.That(TotalAvailableFlights.ToList().Count.Equals(1));
            Assert.That(TotalAvailableFlights.FirstOrDefault().NumberOfBookings.Equals("1"));
        }


        private async Task SeedDbAsync(IApplicatioDbRepository repo)
        {
            var Boeing = new Aircraft()
            {
                Capacity = 100,
                Manufacturer = "Boeing",
                Model = "737",
                ImageUrl = "https://images.unsplash.com/photo-1520437358207-323b43b50729?ixlib=rb-1.2.1&ixid=MnwxMjA3fDB8MHxzZWFyY2h8Mnx8YWlyY3JhZnR8ZW58MHx8MHx8&w=1000&q=80"
            };

            var Airbus = new Aircraft()
            {
                Capacity = 150,
                Manufacturer = "Airbus",
                Model = "a320",
                ImageUrl = "https://images.unsplash.com/photo-1520437358207-323b43b50729?ixlib=rb-1.2.1&ixid=MnwxMjA3fDB8MHxzZWFyY2h8Mnx8YWlyY3JhZnR8ZW58MHx8MHx8&w=1000&q=80"
            };

            var SofiaRoute = new FlightRoute() { City = "Sofia", IATA = "SOF" };
            var VarnaRoute = new FlightRoute() { City = "Varna", IATA = "VAR" };

            var ExampleFlight = new Flight()
            {
                FlightStatus = Infrastructure.Status.Scheduled,
                From = SofiaRoute,
                To = VarnaRoute,
                Aircraft = Airbus,
                FlightInformation = DateTime.Now,
                StandardTicketPrice = 123
            };

            var Passenger = new Passenger()
            {
                Nationality = "Bulgarian",
                FirstName = "Roslava",
                LastName = "Angelova",
                DOB= DateTime.Now,
                DocumentNumber = "1234567899"
            };

            var Booking = new Booking()
            {
                BookingStatus = Infrastructure.Status.Scheduled,
                Flight = ExampleFlight,
                Passenger = Passenger
            };

            await repo.AddAsync(Airbus);
            await repo.AddAsync(Boeing);
            await repo.AddAsync(SofiaRoute);
            await repo.AddAsync(VarnaRoute);
            await repo.AddAsync(ExampleFlight);
            await repo.AddAsync(Passenger);
            await repo.AddAsync(Booking);
            await repo.SaveChangesAsync();


        }
    }
}
