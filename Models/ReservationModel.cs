using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GBSHotels.API.Models
{
    public class ReservationModel
    {
        public string ReservationID { get; set; }
        public string PinCode { get; set; }
        public string Culture { get; set; }

    }

    public class ReservationWriteReviewModel
    {
        public int Id { get; set; }
        public string Typereview { get; set; }
        public string Travelertype { get; set; }
        public string Name { get; set; }

    }

    public class ReservationReviewInsertModel
    {
        public string ReservationId { get; set; }
        public string UserId { get; set; }
        public string traveltype { get; set; }
        public string travelerdate { get; set; }
        public string positive { get; set; }
        public string negative { get; set; }
        public string name { get; set; }
        public string Averagepoint { get; set; }
        public string email { get; set; }
        public string location { get; set; }
        public string HotelFirmID { get; set; }
        public string Mynamedisplay { get; set; }
        public string Cleaning { get; set; }
        public string Location1 { get; set; }
        public string Comfort { get; set; }
        public string Service { get; set; }
        public string Facilities { get; set; }
        public string Checkin { get; set; }
        public string Valueofmoney { get; set; }
    }
    public class ReservationHistoryModel
    {
        public string EncReservationId { get; set; }
        public string ReservationId { get; set; }
        public string EncPinCode { get; set; }
        public string PinCode { get; set; }
        public string ReservationDate { get; set; }
        public string CheckInDate { get; set; }
        public string CheckOutDate { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }
        public string CurrencySymbol { get; set; }
        public string PayableAmount { get; set; }
        public string Property { get; set; }

    }
}