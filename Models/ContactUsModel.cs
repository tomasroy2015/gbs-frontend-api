using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GBSHotels.API.Models
{
    public class ContactUsModel
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Contact { get; set; }
        public string Subject { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public int CountryId { get; set; }
        public string Satulation { get; set; }
        public string IPAddress { get; set; }
        public int UserId { get; set; }

    }
}