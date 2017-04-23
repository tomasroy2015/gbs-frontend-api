using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GBSHotels.API.Models
{
    public class ContinentDestinationModel
    {
        public int Id { get; set; }
        public string ContinentsId { get; set; }
        public string CountryName { get; set; }
        public string CountryStartChar { get; set; }
        public string Continents { get; set; }
        public int HotelCount { get; set; }
        public string Hotel { get; set; }
        public string Didtext { get; set; }
        public string Destination { get; set; }
        public string Country { get; set; }
        public int Rowcounts { get; set; }
    }


    public class ContinentModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Style { get; set; }
    }
}