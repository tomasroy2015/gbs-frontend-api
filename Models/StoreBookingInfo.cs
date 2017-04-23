using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GBSHotels.API.Models
{
    public class StoreBookingInfo
    {
      public  string culture { get; set; }
      public  string checkDatefrom { get; set; }
      public  string checkDateto { get; set; }
      public  string hotelCity { get; set; }
      public  string hotelId { get; set; }
      public  string address { get; set; }
      public  string mainPhotoName { get; set; }
      public  string hotelClass { get; set; }
      public  string hotelname { get; set; }
      public  string roomId { get; set; }
      public  string uniqueId { get; set; }
      public  string accommodationTypeId { get; set; }
      public  string accommodationTypeName { get; set; }
      public  string accommodationTypeDescription { get; set; }
      public  string pricePolicyTypeId { get; set; }
      public  string pricePolicyTypeName { get; set; }
      public  string singleRate { get; set; }
      public  string doubleRate { get; set; }
      public  string dailyRoomPrices { get; set; }
      public  string originalRoomPrice { get; set; }
      public  string currencyId { get; set; }
      public  string currencySymbol { get; set; }
      public  string roomCount { get; set; }
      public  string MaxPeopleCount { get; set; }
      public  string roomPriceVal { get; set; }
      public  string currencyCodeval { get; set; }
      public  string hiddenRoomTypeval { get; set; }
      public  string hiddenPriceTypeval { get; set; }
      public  int creditCardNotRequiredValue { get; set; }
      public  string currentCurrency { get; set; }

    }
}