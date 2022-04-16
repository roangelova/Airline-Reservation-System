using AirlineReservationSystem.Core.Models.User_Area;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineReservationSystem.Core.Contracts
{
    public interface IBookingService
    {
        Task<IEnumerable<AvailableFlightsVM>> GetAllAvailableFlights();
    }
}
