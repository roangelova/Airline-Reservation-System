using AirlineReservationSystem.Core.Contracts;
using AirlineReservationSystem.Core.Models.AdminArea.Aircraft;
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
    public class AircraftServiceTest
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
                .AddSingleton<IAircraftService, AircraftService>()
                .BuildServiceProvider();
        }

        [TearDown]
        public void TearDown()
        {
            dbContext.Dispose();
        }

        [Test]
        public async Task AircraftShouldBeAddedSuccessfully()
        {
            var aircraftVM = new AddAircraftVM()
            {
                Capacity = "100",
                Manufacturer = "Boeing",
                AircraftModel = "737",
                ImageUrl = "https://images.unsplash.com/photo-1520437358207-323b43b50729?ixlib=rb-1.2.1&ixid=MnwxMjA3fDB8MHxzZWFyY2h8Mnx8YWlyY3JhZnR8ZW58MHx8MHx8&w=1000&q=80"
            };

            var service = serviceProvider.GetService<IAircraftService>();
            var repo = serviceProvider.GetService<IApplicatioDbRepository>();

            Assert.DoesNotThrowAsync(async () => await service.AddAircraft(aircraftVM));

            var availableAircraft = await repo.All<Aircraft>().ToListAsync();
            Assert.That(availableAircraft.Count.Equals(1));
        }

        [Test]
        public async Task ShouldReturnAllAvailableAircrafts()
        {
            var service = serviceProvider.GetService<IAircraftService>();
            var repo = serviceProvider.GetService<IApplicatioDbRepository>();

            await SeedDbAsync(repo);

            var availableAircraft = await repo.All<Aircraft>().ToListAsync();
            Assert.That(availableAircraft.Count.Equals(2));
        }

        [Test]
        public async Task ShouldReturnTrueIfAircraftIsInUse()
        {
            var service = serviceProvider.GetService<IAircraftService>();
            var repo = serviceProvider.GetService<IApplicatioDbRepository>();

            await SeedDbAsync(repo);

            bool ShouldBeTrue = await service.CheckIfInUse("Used");

            Assert.That(ShouldBeTrue.Equals(true));
        }

        [Test]
        public async Task ShouldReturnFalseIfAircraftIsNotInUse()
        {
            var service = serviceProvider.GetService<IAircraftService>();
            var repo = serviceProvider.GetService<IApplicatioDbRepository>();

            await SeedDbAsync(repo);

            bool ShouldBeFalse = await service.CheckIfInUse("NotUsed");

            Assert.That(ShouldBeFalse.Equals(false));
        }

        [Test]
        public async Task ShouldReturnAllAvailableAircraftForCancellation()
        {
            var service = serviceProvider.GetService<IAircraftService>();
            var repo = serviceProvider.GetService<IApplicatioDbRepository>();

            await SeedDbAsync(repo);

           var list = await service.GetAllAircraftForCancellation();
            var FlightInUse = list.ToList().FirstOrDefault(x => x.AircraftId == "Used");
            Assert.AreEqual(FlightInUse.InUse, "Yes");
            Assert.AreEqual(FlightInUse.AircraftMakeAndModel, "Boeing 737");
        }

        [Test]
        public async Task ShouldRemoveAircraftById()
        {
            var service = serviceProvider.GetService<IAircraftService>();
            var repo = serviceProvider.GetService<IApplicatioDbRepository>();

            await SeedDbAsync(repo);

            Assert.DoesNotThrowAsync(async () => await service.RemoveAircraft("Used"));

            var AllAircraft = await repo.All<Aircraft>().ToListAsync();
            Assert.AreEqual(AllAircraft.Count, 1);

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

            var SofiaRoute = new FlightRoute() { City = "Sofia", IATA = "SOF" };
            var VarnaRoute = new FlightRoute() { City = "Varna", IATA = "VAR" };

            var Aircraft1 = new Aircraft()
            {
                AircraftId = "Used",
                Capacity = 100,
                Manufacturer = "Boeing",
                Model = "737",
                ImageUrl = "https://images.unsplash.com/photo-1520437358207-323b43b50729?ixlib=rb-1.2.1&ixid=MnwxMjA3fDB8MHxzZWFyY2h8Mnx8YWlyY3JhZnR8ZW58MHx8MHx8&w=1000&q=80"
            };
            
            var Flight = new Flight()
            {
                FlightId = "FlightId",
                FlightStatus = Infrastructure.Status.Scheduled,
                From = SofiaRoute,
                To = VarnaRoute,
                Aircraft = Aircraft1,
                FlightInformation = DateTime.Now,
                StandardTicketPrice = 123
            };

            var Aircraft2 = new Aircraft()
            {
                AircraftId = "NotUsed",
                Capacity = 150,
                Manufacturer = "Airbus",
                Model = "a320",
                ImageUrl = "https://images.unsplash.com/photo-1520437358207-323b43b50729?ixlib=rb-1.2.1&ixid=MnwxMjA3fDB8MHxzZWFyY2h8Mnx8YWlyY3JhZnR8ZW58MHx8MHx8&w=1000&q=80"
            };

            await repo.AddAsync(Aircraft1);
            await repo.AddAsync(Aircraft2);
            await repo.AddAsync(Passenger);
            await repo.AddAsync(Flight);
            await repo.AddAsync(SofiaRoute);
            await repo.AddAsync(VarnaRoute);
            await repo.SaveChangesAsync();


        }
    }
}
