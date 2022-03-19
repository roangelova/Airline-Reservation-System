using AirlineReservationSystem.Infrastructure.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AirlineReservationSystem.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Flight>()
                .HasOne(x => x.From);

           modelBuilder.Entity<Flight>()
                .HasOne(x => x.To);

            modelBuilder.Entity<Aircraft>()
               .HasMany(x => x.Flights);

        }


        public DbSet<Aircraft> Aircrafts { get; set; }

        public DbSet<Baggage> Baggages { get; set; }

        public DbSet<Booking> Bookings { get; set; }

        public DbSet<FlightRoute> FlightRoutes { get; set; }

        public DbSet<Flight> Flights { get; set; }

        public DbSet<Passenger> Passengers { get; set; }


    }
}