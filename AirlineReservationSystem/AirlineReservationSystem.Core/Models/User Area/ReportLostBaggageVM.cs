using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineReservationSystem.Core.Models.User_Area
{
    public class ReportLostBaggageVM
    {
        public string BaggageId { get; set; }

        public string Size { get; set; }
        public string IsReportedLost { get; set; }
    }
}
