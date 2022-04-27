using AirlineReservationSystem.Core.Contracts;
using AirlineReservationSystem.Core.Services;
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


    }
}
