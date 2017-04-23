using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GBSHotels.API.Models
{
    public class RecentlyVieweHotelModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SearchId { get; set; }
        public string Description { get; set; }
        public string RoutingName { get; set; }
        public string Address { get; set; }
        public string CountryName { get; set; }
        public string HotelRoutingName { get; set; }
        public string CountryCode { get; set; }
        public string ClosestAirportName { get; set; }
        public string ClosestAirportDistance { get; set; }
        public string MainPhotoName { get; set; }
        public string HotelClassValue { get; set; }
        public string HotelClass { get; set; }
        public string CityName { get; set; }
        public string NavigateURL { get; set; }
        public string CityNavigateURL { get; set; }
        public string CurrencyID { get; set; }
        public string CurrencySymbol { get; set; }
        public string IsPreferred { get; set; }
        public string CurrencyCode { get; set; }
        public int ReviewCount { get; set; }
        public string Superb { get; set; }
        public string Hotel { get; set; }
        public string ScoreFrom { get; set; }
        public string Reviews { get; set; }
        public string DescriptionText { get; set; }
        public string VeryGood { get; set; }
        public string Avgprice { get; set; }
        public string New { get; set; }
        public double ConvertedRoomPrice { get; set; }
        public string NewCurrencySymbol { get; set; }
        public string HotelStatus { get; set; }
        public string AverageReviewPoint { get; set; }
        public string RoomPrice { get; set; }

        public string ReviewValue { get; set; }
        public string ReviewTypeScaleName { get; set; }

    }
}