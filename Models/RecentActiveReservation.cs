using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GBSHotels.API.Models
{
    public class RecentActiveReservationModel
    {
        public string ReservationId { get; set; }
        public string Property { get; set; }
        public string CurrencySymbol { get; set; }
        public string Amount { get; set; }

    }
}