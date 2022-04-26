using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using AirlineReservationSystem.Infrastructure.Repositories;
using AirlineReservationSystem.Core.Services;
using AirlineReservationSystem.Core.Contracts;
using AirlineReservationSystem.Core.Models.AdminArea.Aircraft;
using AirlineReservationSystem.Infrastructure.Models;

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

            var repo = serviceProvider.GetService<IApplicatioDbRepository>();
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


        private async Task SeedDbAsync(IApplicatioDbRepository repo)
        {
            var Aircraft1 = new Aircraft()
            {
                Capacity = 100,
                Manufacturer = "Boeing",
                Model = "737",
                ImageUrl = "https://images.unsplash.com/photo-1520437358207-323b43b50729?ixlib=rb-1.2.1&ixid=MnwxMjA3fDB8MHxzZWFyY2h8Mnx8YWlyY3JhZnR8ZW58MHx8MHx8&w=1000&q=80"
            };

            var Aircraft2 = new Aircraft()
            {
                Capacity = 150,
                Manufacturer = "Airbus",
                Model = "a320",
                ImageUrl = "https://images.unsplash.com/photo-1520437358207-323b43b50729?ixlib=rb-1.2.1&ixid=MnwxMjA3fDB8MHxzZWFyY2h8Mnx8YWlyY3JhZnR8ZW58MHx8MHx8&w=1000&q=80"
            };

            await repo.AddAsync(Aircraft1);
            await repo.AddAsync(Aircraft2);
            await repo.SaveChangesAsync();


        }
    }
}
