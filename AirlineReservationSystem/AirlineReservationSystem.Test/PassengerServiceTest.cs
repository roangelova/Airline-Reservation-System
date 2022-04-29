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
    public class PassengerServiceTest
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
                .AddSingleton<IPassengerService, PassengerService>()
                .BuildServiceProvider();
        }

        [TearDown]
        public void TearDown()
        {
            dbContext.Dispose();
        }

        [Test]
        public async Task ShouldRegisterAPassengerSuccessfullyAndReturnThePassengerId()
        {
            var model = new EditPassengerDataVM()
            {
                FirstName = "My Name",
                LastName = "Last Name",
                DateOfBirth = "10.07.1997",
                Nationality = "Korean",
                DocumentId = "1234567"
            };

            var service = serviceProvider.GetService<IPassengerService>();
            (bool added, string id) = await service.RegisterPassenger(model);
            Assert.That(id, Is.Not.EqualTo(""));
        }

        [Test]
        public async Task ShouldReturnFalseAndEmptyPassengerIdIfDOBIsInvalid()
        {
            var model = new EditPassengerDataVM()
            {
                FirstName = "My Name",
                LastName = "Last Name",
                DateOfBirth = "Invalid",
                Nationality = "Korean",
                DocumentId = "1234567"
            };

            var service = serviceProvider.GetService<IPassengerService>();
            var (Result, PassengerId) = await service.RegisterPassenger(model);
            Assert.AreEqual(Result, false);
            Assert.AreEqual(PassengerId, "");
        }

        [Test]
        public async Task ShouldReturnEmptyStringIfThereIsNoPassengerIdYet()
        {
            var service = serviceProvider.GetService<IPassengerService>();

            string InvalidId = "1234456789";
            var result = await service.GetPassengerId(InvalidId);
            Assert.That(result.Equals(""));
        }

        [Test]
        public async Task ShouldReturnthePassengerIdIfSuchExists()
        {
            var service = serviceProvider.GetService<IPassengerService>();
            var repo = serviceProvider.GetService<IApplicatioDbRepository>();

            await SeedDbAsync(repo);

            var user = await repo.All<ApplicationUser>().FirstOrDefaultAsync();
            var PassengerId = await service.GetPassengerId(user.Id);
            Assert.AreEqual(PassengerId, "12344");

        }

        [Test]
        public async Task ShouldReturnAllBookingsForPassengerId()
        {
            var service = serviceProvider.GetService<IPassengerService>();
            var repo = serviceProvider.GetService<IApplicatioDbRepository>();

            await SeedDbAsync(repo);

            var UserBookings = await service.GetUserBookings("12344");
            Assert.That(UserBookings.ToList().Count.Equals(1));

        }

        private async Task SeedDbAsync(IApplicatioDbRepository repo)
        {
            var Passenger = new Passenger()
            {
                PassengerId= "12344",
                Nationality = "Bulgarian",
                FirstName = "Test",
                LastName = "Angelova",
                DOB = DateTime.Now,
                DocumentNumber = "1234567899"
            };

            var user = new ApplicationUser()
            {
                FirstName = "My",
                LastName = "Test",
                Passenger = Passenger
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
                BookingStatus = Infrastructure.Status.Scheduled,
                Flight = ExampleFlight,
                Passenger = Passenger
            };

            await repo.AddAsync(Passenger);
            await repo.AddAsync(user);
            await repo.AddAsync(Airbus);
            await repo.AddAsync(SofiaRoute);
            await repo.AddAsync(VarnaRoute);
            await repo.AddAsync(Booking);
            await repo.SaveChangesAsync();
        }
    }
}
