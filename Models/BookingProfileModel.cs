using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GBSHotels.API.Models
{
    public class BookingProfileModel
    {
        public string UserID { get; set; }
        public string Userphoto { get; set; }
        public string SalutationTypeID { get; set; }
        public string Image { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Phone { get; set; }
        public string Country { get; set; }
        public string CountryID { get; set; }
        public string City { get; set; }
        public string CityID { get; set; }
        public string Address { get; set; }
        public string PostCode { get; set; }
    }

    public class BookingCreditCard
    {
        public string CreditCardProviderID { get; set; }
        public string ExpiryDate { get; set; }
        public string NameOnCreditCard { get; set; }
        public string CreditCardNumber { get; set; }
        public string CreditCardTypeName { get; set; }
        public string month { get; set; }
        public string year { get; set; }
    }
}