using AirlineReservationSystem.Core.Models.AdminArea.Flight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineReservationSystem.Core.Contracts
{
    public interface IFlightService
    {
        Task<bool> AddFlight(AddFlightVM model);
    }
}
