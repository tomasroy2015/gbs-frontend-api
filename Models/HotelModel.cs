using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GBSHotels.API.Models
{
    public class HotelModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string RoutingName { get; set; }
        public string ClosestAirportName { get; set; }
        public string ClosestAirportDistance { get; set; }
        public string MainPhotoName { get; set; }
        public string HotelClass { get; set; }
        public string HotelClassValue { get; set; }
        public string CityName { get; set; }
        public string CountryName { get; set; }
        public string CityID { get; set; }
        public string CountryID { get; set; }
        public string HotelStar { get; set; }
        public string Address { get; set; }
        public string CurrencyID { get; set; }
        public string CurrencySymbol { get; set; }
        public string ReviewCount { get; set; }
        public string CountryNameineng { get; set; }
        public string CityNavigateURL { get; set; }
        public string ReviewEvaluationName { get; set; }
        public string AverageReviewPoint { get; set; }
        public string RatingBasedOnReview { get; set; }
        public string WishListAdded { get; set; }
        public string HotelStatus { get; set; }
        public string Superb { get; set; }
        public string Hotel { get; set; }
        public string IsPreferred { get; set; }
        public string ScoreFrom { get; set; }
        public string Reviews { get; set; }
        public string DescriptionText { get; set; }
        public string VeryGood { get; set; }
        public string New { get; set; }
        public string Policiesof { get; set; }
        public string Facilitiesof { get; set; }
        public string ShowMap { get; set; }
        public string Properties { get; set; }
        public string Properties1 { get; set; }
        public string NavigateURL { get; set; }
        public string HotelRoomID { get; set; }
        public string CurrencyCode { get; set; }
        public string RoomPrice { get; set; }
        public string Avgprice { get; set; }
        public string KmFrom { get; set; }
        public string Book { get; set; }
        public string NewCurrencySymbol { get; set; }
        public double ConvertedRoomPrice { get; set; }
        public string RegionID { get; set; }
        public string MainRegionID { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string WebAddress { get; set; }
        public string Email { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string MapZoomIndex { get; set; }
        public string MainRegionName { get; set; }
        public string RegionName { get; set; }
        public string Close { get; set; }
        public double MinumumRoomPrice { get; set; }
    }

    public class HotelDistanceModel
    {
        public Int64 IDs { get; set; }
        public string Name { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }
    public class HotelNearByCityModel
    {
        public string RegionName { get; set; }
        public string CountryName { get; set; }
        public string Code { get; set; }
        public string RegionID { get; set; }
        public string TopPhoto { get; set; }
        public string TopPhotoFlag { get; set; }
        public string NavigateURL { get; set; }

    }

    public class HotelSpecificationsModel
    {
        public string HotelRoomID { get; set; }
        public string HotelID { get; set; }
        public string NameASCII { get; set; }
        public string HotelImage { get; set; }
        public string Name { get; set; }
    }
    public class HotelImageModel
    {
        public string HotelImage { get; set; }
        public int HotelImageId { get; set; }
        public int HotelId { get; set; }
    }
    public class HotelReservationReviewModel
    {
        public int ReviewScaleTypeId { get; set; }
        public string ReviewScale { get; set; }
        public string ReviewScaleType { get; set; }
        public string ExcellantWidth { get; set; }
    }
    public class HotelReviewModel
    {
        public int Id { get; set; }
        public string Point { get; set; }
        public string AveragePoint { get; set; }
        public string ReviewStatusName { get; set; }
        public string Anonymous { get; set; }
        public string CreateDateTime { get; set; }
        public string OpDateTime { get; set; }
        public string TravelerTypeName { get; set; }
        public string UserFullName { get; set; }
        public string CountryName { get; set; }
        public string City { get; set; }
        public int TotalReviewCount { get; set; }
        public string UserPhoto { get; set; }
        public string ReviewPositive { get; set; }
        public string ReviewNegative { get; set; }
        public string Total { get; set; }
        public string Froms { get; set; }
        public string Review { get; set; }
        public string ReviewTypeScaleName { get; set; }

    }

    public class HotelReviewTypeModel
    {
        public string ID { get; set; }
        public string TypeReview { get; set; }
    }

    public class HotelRoomAvailabilityModel
    {
        public string RoomDataTableType { get; set; }
        public string HotelID { get; set; }
        public string HotelRoomID { get; set; }
        public string HotelRoomDescription { get; set; }
        public string HotelTypeName { get; set; }
        public string SmokingTypeName { get; set; }
        public string RoomCount { get; set; }
        public string MaxPeopleCount { get; set; }
        public string ExtraBedCount { get; set; }
        public string Checkin { get; set; }
        public string Checkout { get; set; }
        public string RoomTypeText { get; set; }
        public string RoomPriceText { get; set; }
        public string RoomMaxPeopleCount { get; set; }
        public string Quantity { get; set; }
        public string FREEcancellation { get; set; }
        public string RoomFacilities { get; set; }
        public string CheckinStart { get; set; }
        public string CheckoutStart { get; set; }

    }

    public class HotelRoomConditionModel
    {

        public string RoomDataTableType { get; set; }
        public string RoomPrice { get; set; }
        public double ConvertedRoomPrice { get; set; }
        public string NewCurrencySymbol { get; set; }
        public string AttributeName { get; set; }
        public string icons { get; set; }
        public string AttributeTypeID { get; set; }
        public string AttributeHeaderID { get; set; }
        public string AttributeHeaderName { get; set; }
        public string AttributeID { get; set; }
        public string UnitID { get; set; }
        public string UnitName { get; set; }
        public string HotelUnitID { get; set; }
        public string HotelUnitName { get; set; }
        public string UnitValue { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencyID { get; set; }


    }

    public class HotelCardModel
    {
        public string RoomDataTableType { get; set; }
        public string HotelCreditCard { get; set; }
        public string CreditCardTypeName { get; set; }
        public List<HotelCreditCards> HotelCreditCardImage { get; set; }


    }
    public class HotelCancellationModel
    {
        public string RoomDataTableType { get; set; }
        public string HotelCancelPolicy { get; set; }
        public string CancelPolicyWarning { get; set; }
    }
    public class HotelFacilityModel
    {
        public List<HotelRoomAvailabilityModel> HotelRoomAvailability { get; set; }
        public List<HotelCardModel> HotelCard { get; set; }
        public List<HotelRoomConditionModelGroup> HotelRoomCondition { get; set; }
        public List<HotelCancellationModel> HotelCancllaton { get; set; }

    }

    public class HotelRoomConditionModelGroup
    {
        public string AttributeHeaderID { get; set; }
        public List<HotelRoomConditionModel> HotelRoomCondition { get; set; }
    }
}