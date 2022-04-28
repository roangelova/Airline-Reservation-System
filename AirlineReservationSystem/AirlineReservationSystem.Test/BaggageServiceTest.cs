using AirlineReservationSystem.Core.Contracts;
using AirlineReservationSystem.Core.Models.User_Area;
using AirlineReservationSystem.Core.Services;
using AirlineReservationSystem.Infrastructure.Models;
using AirlineReservationSystem.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineReservationSystem.Test
{
    public class BaggageServiceTest
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
                .AddSingleton<IBaggageService, BaggageService>()
                .BuildServiceProvider();
        }

        [TearDown]
        public void TearDown()
        {
            dbContext.Dispose();
        }


        [Test]
        public void ShouldReturnAllAvailableBaggageSizes()
        {
            var service = serviceProvider.GetService<IBaggageService>();
            var repo = serviceProvider.GetService<IApplicatioDbRepository>();

            var sizes = service.GetAvailableBaggageSizes().ToList();
            Assert.That(sizes.Count.Equals(3));
            Assert.That(sizes.First().Size == BaggageSize.Small.ToString());
        }


        [Test]
        public async Task ShouldAddBaggageToBookingSuccessfully()
        {
            var service = serviceProvider.GetService<IBaggageService>();
            var repo = serviceProvider.GetService<IApplicatioDbRepository>();

            await SeedDbAsync(repo);

            var Passenger = new Passenger()
            {
                PassengerId = "12344",
                Nationality = "Bulgarian",
                FirstName = "Test",
                LastName = "Angelova",
                DOB = DateTime.Now,
                DocumentNumber = "1234567899"
            };

            var Baggage = new AddBaggageVM()
            {
                Size = BaggageSize.Small.ToString(),
            };

            Assert.DoesNotThrowAsync(async () => await service.AddBaggageToBoooking("test", "12344", Baggage));
        }

        [Test]
        public async Task ShouldReturnAllBookesBaggagesForGivenBookingAndPassengerId()
        {
            var service = serviceProvider.GetService<IBaggageService>();
            var repo = serviceProvider.GetService<IApplicatioDbRepository>();
            await SeedDbAsync(repo);

            var resultWithoutBaggage = await service.GetBaggagesForBooking("test", "12344");
            Assert.That(resultWithoutBaggage.ToList().Count.Equals(0));

            var resultWithBaggage = await service.GetBaggagesForBooking("BookingWithBaggage", "12344");
            Assert.That(resultWithBaggage.ToList().Count.Equals(1));
        }

        [Test]
        public async Task ShouldReportExistingBaggageAsLostSuccessfully()
        {
            var service = serviceProvider.GetService<IBaggageService>();
            var repo = serviceProvider.GetService<IApplicatioDbRepository>();
            await SeedDbAsync(repo);

            Assert.DoesNotThrowAsync(async () => await service.ReportAsLost("BaggageId"));

            var BaggageReportedAsLost = await repo.All<Baggage>().ToListAsync();
            Assert.That(BaggageReportedAsLost.First().IsReportedLost == true);
        }

        private async Task SeedDbAsync(IApplicatioDbRepository repo)
        {
            var Passenger = new Passenger()
            {
                PassengerId = "12344",
                Nationality = "Bulgarian",
                FirstName = "Test",
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

            var ExampleFlight = new Flight()
            {
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
                Flight = ExampleFlight,
                Passenger = Passenger
            };

            var BookingWithBaggage = new Booking()
            {
                BookingNumber = "BookingWithBaggage",
                BookingStatus = Infrastructure.Status.Scheduled,
                Flight = ExampleFlight,
                Passenger = Passenger
            };

            var Baggage = new Baggage()
            {
                Size = BaggageSize.Medium,
                BaggageId = "BaggageId",
                IsReportedLost = false,
                Passenger = Passenger,
                Booking = BookingWithBaggage
            };

            await repo.AddAsync(Passenger);
            await repo.AddAsync(Airbus);
            await repo.AddAsync(SofiaRoute);
            await repo.AddAsync(VarnaRoute);
            await repo.AddAsync(ExampleFlight);
            await repo.AddAsync(Booking);
            await repo.AddAsync(BookingWithBaggage);
            await repo.AddAsync(Baggage);
            await repo.SaveChangesAsync();
        }
    }
}
