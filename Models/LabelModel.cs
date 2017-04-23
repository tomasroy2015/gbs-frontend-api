using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GBSHotels.API.Models
{
    public class LabelModel
    {
        public string Culture { get; set; }
        public string TextMessage { get; set; }
        public string LblId { get; set; }
        public string MessageCode { get; set; }
    }

    public class CountModel
    {
        public int HotelCount { get; set; }
        public int CountryCount { get; set; }
    }
}