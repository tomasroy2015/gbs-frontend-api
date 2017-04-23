using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GBSHotels.API.Models
{
    public class ProfileModel
    {
        public string UserPhoto { get; set; }
        public string Image { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Phone { get; set; }
        public string Country { get; set; }
        public int CountryId { get; set; }
        public string City { get; set; }
        public string CityId { get; set; }
        public string Address { get; set; }
        public string PostCode { get; set; }
        public string UserId { get; set; }
        public string CreatedDate { get; set; }
        public int CountryCount { get; set; }
        public int CityCount { get; set; }
        public int TripCount { get; set; }
        public string VerificationCode { get; set; }
        public bool? Genius { get; set; }
    }

    public class MyWishlistsModel
    {
        public int Id { get; set; }
        public string RegionName { get; set; }
        public string Name { get; set; }
        public string MinimumRoomPrice { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string PostCode { get; set; }
        public string RoomCount { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }
        public string CountryName { get; set; }
        public string ReviewCount { get; set; }
        public string IsPreferred { get; set; }
        public string CreatedDate { get; set; }
        public string Hotel { get; set; }
        public string ScoreFrom { get; set; }
        public string Reviews { get; set; }
        public string CurrencySymbol { get; set; }
        public string MainPhotoName { get; set; }
        public int ReviewCountValue { get; set; }
        public string HotelClass { get; set; }
        public string NavigateURL { get; set; }
        public string ReviewStatus { get; set; }

        public string Remove { get; set; }
        public string from { get; set; }
        public string Booknow { get; set; }
        public string night { get; set; }
        public string Latestbooking { get; set; }
        public string addedon { get; set; }
    }

    public class ProfileCreditCardModel
    {
        public string UserId { get; set; }
        public string CreditCardProvider { get; set; }
        public string NameOnCreditcard { get; set; }
        public string CreditCardNumber { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public string ID { get; set; }

    }

    public class UserCreditCardModel
    {
        public string TypeMonth { get; set; }
        public string ID { get; set; }
        public string CreditCardProviderID { get; set; }
        public string ExpiryDate { get; set; }
        public string CreditCardNumber { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public string Savechange { get; set; }
        public string Business { get; set; }
        public string Cancel { get; set; }
        public string Remove { get; set; }
        public string Expirydates { get; set; }
        public string Yourcredit { get; set; }
        public string NameOnCreditCard { get; set; }
        public string cardholder { get; set; }
    }
}