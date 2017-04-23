using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GBSHotels.API.Models
{
    public class PropertyModel
    {
        public string HotelName { get; set; }
        public string ProCountry { get; set; }
        public string ProCity { get; set; }
        public string ProAddress { get; set; }
        public string ProPhone { get; set; }
        public string ProFax { get; set; }
        public string ProType { get; set; }
        public string ProClass { get; set; }
        public string ProAccommodation { get; set; }

        public string ProRoomCount { get; set; }
        public string ProWebsite { get; set; }
        public string ProEmail { get; set; }
        public string ProResCurrency { get; set; }
        public string listcards { get; set; }
        public string HotelAttributeId { get; set; }
        public string ChargeType { get; set; }
        public string RadioRefund { get; set; }
        public string RefundCancel { get; set; }
        public string PenaltyRate { get; set; }
        public string ChannelManager { get; set; }
        public string RoomType { get; set; }
        public string RoomCount { get; set; }
        public string RoomSpace { get; set; }
        public string RoomMaxPerson { get; set; }
        public string RoomMaxChildren { get; set; }
        public string RoomBabyCots { get; set; }
        public string RoomExBabyCots { get; set; }
        public string RoomSmoking { get; set; }
        public string RoomView { get; set; }
        public string Facilities { get; set; }
        public string FirmName { get; set; }
        public string Salutation { get; set; }
        public string ContactPersonName { get; set; }
        public string ContactPersonSurname { get; set; }
        public string ContactPersonPhone { get; set; }
        public string ContactPersonEmail { get; set; }
        public string ContactPersonPosition { get; set; }
        public string CityName { get; set; }
        public string CultureID { get; set; }
        public string IpAddress { get; set; }
    }
}