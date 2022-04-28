using AirlineReservationSystem.Core.Contracts;
using AirlineReservationSystem.Core.Services;
using AirlineReservationSystem.Infrastructure.Models;
using AirlineReservationSystem.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineReservationSystem.Test
{
    public class BookingServiceTest
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
                .AddSingleton<IBookingService, BookingService>()
                .BuildServiceProvider();
        }

        [TearDown]
        public void TearDown()
        {
            dbContext.Dispose();
        }


        [Test]
        public async Task ShouldCancelABookingByBookingId()
        {
            var service = serviceProvider.GetService<IBookingService>();
            var repo = serviceProvider.GetService<IApplicatioDbRepository>();
            await SeedDbAsync(repo);

            var result = await service.CancelBooking("test");
            Assert.AreEqual(result, true);

            var CanceledBooking = await repo.GetByIdAsync<Booking>("test");
            Assert.AreEqual(CanceledBooking.BookingStatus, Infrastructure.Status.Canceled);
        }

        [Test]
        public async Task ShouldCreateABookingSuccessfully()
        {
            var service = serviceProvider.GetService<IBookingService>();
            var repo = serviceProvider.GetService<IApplicatioDbRepository>();
            await SeedDbAsync(repo);

            Assert.DoesNotThrowAsync(async () => await service.BookPassengerFlight("FlightId", "12344"));
        }


        [Test]
        public async Task ShouldReturnPastBookingsForUser()
        {
            var service = serviceProvider.GetService<IBookingService>();
            var repo = serviceProvider.GetService<IApplicatioDbRepository>();
            await SeedDbAsync(repo);

            var PastBoookings = await service.GetPastUserFlights("12344");
            Assert.That(PastBoookings.ToList().Count().Equals(1));

        }

        private async Task SeedDbAsync(IApplicatioDbRepository repo)
        {
            var Passenger = new Passenger()
            {
                PassengerId = "12344",
                Nationality = "Bulgarian",
                FirstName = "Roslava",
                LastName = "Angelova",
                DOB = DateTime.Now,
                DocumentNumber = "1234567899"
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

            var ExampleFlightInPast = new Flight()
            {
                FlightId = "FlightId",
                FlightStatus = Infrastructure.Status.Scheduled,
                From = SofiaRoute,
                To = VarnaRoute,
                Aircraft = Airbus,
                FlightInformation = DateTime.Now.AddDays(-30),
                StandardTicketPrice = 123
            };

            var ExampleFlightNow = new Flight()
            {
                FlightId = "FlightIdInPast",
                FlightStatus = Infrastructure.Status.Scheduled,
                From = SofiaRoute,
                To = VarnaRoute,
                Aircraft = Airbus,
                FlightInformation = DateTime.Now,
                StandardTicketPrice = 123
            };

            var Booking = new Booking()
            {
                BookingNumber = "test",
                BookingStatus = Infrastructure.Status.Scheduled,
                Flight = ExampleFlightNow,
                Passenger = Passenger
            };

            var BookingInPast = new Booking()
            {
                BookingNumber = "PastBooking",
                BookingStatus = Infrastructure.Status.Scheduled,
                Flight = ExampleFlightInPast,
                Passenger = Passenger
            };

            await repo.AddAsync(Passenger);
            await repo.AddAsync(Airbus);
            await repo.AddAsync(SofiaRoute);
            await repo.AddAsync(VarnaRoute);
            await repo.AddAsync(ExampleFlightNow);
            await repo.AddAsync(ExampleFlightInPast);
            await repo.AddAsync(Booking);
            await repo.AddAsync(BookingInPast);
            await repo.SaveChangesAsync();
        }
    }
}
