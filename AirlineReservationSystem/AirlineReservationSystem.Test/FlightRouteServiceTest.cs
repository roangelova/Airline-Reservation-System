using AirlineReservationSystem.Core.Contracts;
using AirlineReservationSystem.Core.Models.AdminArea.Route;
using AirlineReservationSystem.Core.Services;
using AirlineReservationSystem.Infrastructure.Models;
using AirlineReservationSystem.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace AirlineReservationSystem.Test
{
    public class FlightRouteServiceTest
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
                .AddSingleton<IFlightRouteService, FlightRouteService>()
                .BuildServiceProvider();
        }

        [TearDown]
        public void TearDown()
        {
            dbContext.Dispose();
        }


        [Test]
        public void FlightRouteShouldBeAddedToDbSuccessfully()
        {
            var route = new AddFlightRouteVM()
            {
                IATA = "var",
                City = "Varna"
            };

            var service = serviceProvider.GetService<IFlightRouteService>();

            Assert.DoesNotThrowAsync(async () => await service.AddFlightRoute(route));
        }

        [Test]
        public async Task FlightRouteShouldAddCapitalizedIATA_Code()
        {
            var route = new AddFlightRouteVM()
            {
                IATA = "var",
                City = "Varna"
            };

            var service = serviceProvider.GetService<IFlightRouteService>();

            await service.AddFlightRoute(route);

            var repo = serviceProvider.GetService<IApplicatioDbRepository>();
            var CreatedRoute = await repo.All<FlightRoute>().FirstOrDefaultAsync();

            Assert.AreEqual(CreatedRoute.IATA, "VAR");
        }

        [Test]
        public async Task ShouldReturnAllAvailableFlightRoutes()
        {
            var service = serviceProvider.GetService<IFlightRouteService>();
            var repo = serviceProvider.GetService<IApplicatioDbRepository>();

            await SeedDbAsync(repo);

            var routes = await repo.All<Flight>().ToListAsync();
            Assert.That(routes.Count.Equals(2));

        }


        [Test]
        public async Task ShouldReturnFalseIfFlightRouteIsNotInUse()
        {
            var service = serviceProvider.GetService<IFlightRouteService>();
            var repo = serviceProvider.GetService<IApplicatioDbRepository>();

            await SeedDbAsync(repo);

            var FlightRoute3 = new FlightRoute() { City = "Munich", IATA = "MUC" };
            await repo.AddAsync(FlightRoute3);
            await repo.SaveChangesAsync();

            var IsInUse = await service.CheckIfRouteInUse(FlightRoute3.RouteId);
            Assert.True(!IsInUse);

        }

        [Test]
        public async Task ShouldReturnFalseIfFlightRouteIsCanceled()
        {
            var service = serviceProvider.GetService<IFlightRouteService>();
            var repo = serviceProvider.GetService<IApplicatioDbRepository>();

            await SeedDbAsync(repo);
            var MUCRoute = await repo.All<FlightRoute>()
                .FirstOrDefaultAsync(x=> x.City =="Munich");

            var IsInUse = await service.CheckIfRouteInUse(MUCRoute.RouteId);
            Assert.True(!IsInUse);

        }

        [Test]
        public async Task ShouldRemoveARouteFromDbSuccessfully()
        {
            var service = serviceProvider.GetService<IFlightRouteService>();
            var repo = serviceProvider.GetService<IApplicatioDbRepository>();

            await SeedDbAsync(repo);
            var MUCRoute = await repo.All<FlightRoute>()
                .FirstOrDefaultAsync(x => x.City == "Munich");

           Assert.DoesNotThrowAsync(async () => await service.RemoveRoute(MUCRoute.RouteId));

            var routes = await repo.All<FlightRoute>().ToListAsync();
            Assert.That(routes.Count.Equals(2));
        }

        private async Task SeedDbAsync(IApplicatioDbRepository repo)
        {
            var FlightRoute1 = new FlightRoute() { City = "Sofia", IATA = "SOF" };
            var FlightRoute2 = new FlightRoute() { City = "Varna", IATA = "VAR" };
            var FlightRoute3 = new FlightRoute() { City = "Munich", IATA = "MUC" };
            
            var Aircraft = new Aircraft()
            {
                Capacity = 100,
                Manufacturer= "Boeing",
                Model = "737",
                ImageUrl= "https://images.unsplash.com/photo-1520437358207-323b43b50729?ixlib=rb-1.2.1&ixid=MnwxMjA3fDB8MHxzZWFyY2h8Mnx8YWlyY3JhZnR8ZW58MHx8MHx8&w=1000&q=80"
            };

           var FlightToSof = new Flight()
           {
               FlightStatus = Infrastructure.Status.Scheduled,
               From = FlightRoute1,
               To = FlightRoute2,
               Aircraft = Aircraft,
               FlightInformation = DateTime.Now,
               StandardTicketPrice = 123
           };

            var CanceledFlightToSof = new Flight()
            {
                FlightStatus = Infrastructure.Status.Canceled,
                From = FlightRoute3,
                To = FlightRoute2,
                Aircraft = Aircraft,
                FlightInformation = DateTime.Now,
                StandardTicketPrice = 123
            };

            await repo.AddAsync(FlightRoute1);
            await repo.AddAsync(FlightRoute2);
            await repo.AddAsync(FlightRoute3);
            await repo.AddAsync(FlightToSof);
            await repo.AddAsync(CanceledFlightToSof);
            await repo.SaveChangesAsync();


        }
    }
}