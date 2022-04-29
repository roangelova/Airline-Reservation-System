using AirlineReservationSystem.Core.Contracts;
using AirlineReservationSystem.Core.Models.User_Area;
using AirlineReservationSystem.Core.Models.Users;
using AirlineReservationSystem.Core.Services;
using AirlineReservationSystem.Infrastructure.Models;
using AirlineReservationSystem.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AirlineReservationSystem.Test
{
    public class UserServiceTest
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
                .AddSingleton<IUserService, UserService>()
                .BuildServiceProvider();
        }

        [TearDown]
        public void TearDown()
        {
            dbContext.Dispose();
        }


        [Test]
        public async Task ShouldReturnTheUserById()
        {
            var service = serviceProvider.GetService<IUserService>();
            var repo = serviceProvider.GetService<IApplicatioDbRepository>();
            await SeedDbAsync(repo);

            Assert.DoesNotThrowAsync(async () => await service.GetUserById("1234"));
        }

        [Test]
        public async Task ShouldReturnUserEditData()
        {
            var service = serviceProvider.GetService<IUserService>();
            var repo = serviceProvider.GetService<IApplicatioDbRepository>();
            await SeedDbAsync(repo);

            var data = await service.GetUserForEdit("1234");

            Assert.AreEqual(data.FirstName, null);
            Assert.AreEqual(data.LastName, null);
            Assert.AreEqual(data.Id, "1234");
        }

        [Test]
        public async Task ShouldReturnAllAvailableUsersAsViewModel()
        {
            var service = serviceProvider.GetService<IUserService>();
            var repo = serviceProvider.GetService<IApplicatioDbRepository>();
            await SeedDbAsync(repo);

            var users = await service.GetUsers();
            Assert.That(users.ToList().Count.Equals(1));

            var OneUser = users.First();
            Assert.AreEqual(OneUser.Email, "raangelova97@gmail.com");
        }


        [Test]
        public async Task ShouldUpdateUserDataSuccessfully()
        {
            var service = serviceProvider.GetService<IUserService>();
            var repo = serviceProvider.GetService<IApplicatioDbRepository>();
            await SeedDbAsync(repo);

            UserEditVM model = new()
            {
                FirstName = "Test",
                LastName = "Test",
                Id = "1234"
            };

           var result=  await service.UpdateUser(model);
            Assert.IsTrue(result);

            var user = await repo.GetByIdAsync<ApplicationUser>("1234");
            Assert.AreEqual("Test", user.FirstName);

        }

        [Test]
        public async Task ShouldReturnFalseIfUserDoesntExist()
        {
            var service = serviceProvider.GetService<IUserService>();
            var repo = serviceProvider.GetService<IApplicatioDbRepository>();
            await SeedDbAsync(repo);

            UserEditVM model = new()
            {
                FirstName = "Test",
                LastName = "Test",
                Id = "NonExistent"
            };

            var result = await service.UpdateUser(model);
            Assert.IsFalse(result);


        }

      //[Test]
      //public async Task ShouldSetUserDataSuccessfully()
      //{
      //    var service = serviceProvider.GetService<IUserService>();
      //    var repo = serviceProvider.GetService<IApplicatioDbRepository>();
      //    await SeedDbAsync(repo);
      //
      //    EditPassengerDataVM model = new EditPassengerDataVM()
      //    {
      //        FirstName = "Roslava",
      //        LastName = "Angelova",
      //        DateOfBirth = "10.07.1997",
      //        DocumentId = "47693",
      //        Nationality = "Bulgarian"
      //    };
      //
      //    string IdToSet = "999";
      //
      //   Assert.DoesNotThrowAsync(async () => await service.SetUserData("1234", IdToSet, model));
      //
      //    var user = await repo.GetByIdAsync<ApplicationUser>("1234");
      //    Assert.AreEqual(user.PassengerId, IdToSet);
      //    Assert.AreEqual(user.FirstName, model.FirstName);
      //}


        private async Task SeedDbAsync(IApplicatioDbRepository repo)
        {
            Passenger Passenger = new Passenger

            {
                PassengerId = "1234",
                Nationality = "Bulgarian",
                DOB = DateTime.Now,
                DocumentNumber = "1234567899",
                FirstName = "Roslava",
                LastName = "Angelova"
            };

            var user = new ApplicationUser()
            {
                Id = "1234",
                Email = "raangelova97@gmail.com"

            };

            var AnotherUser = new ApplicationUser()
            {
                Id = "0000",
                Email = "raangelova97@gmail.com"

            };

            await repo.AddAsync(Passenger);
            await repo.AddAsync(user);
            await repo.AddAsync(AnotherUser);
            await repo.SaveChangesAsync();
        }
    }
}
