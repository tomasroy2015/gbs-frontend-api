using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GBSHotels.API.Models
{
    public class CityModel
    {
        public int Id { get; set; }

        public string CityName { get; set; }
    }

    public class CityDetailModel
    {
        public string CityImages { get; set; }
        public string Properties { get; set; }
        public string CountryName { get; set; }
        public string CountryID { get; set; }
        public string CountryNameineng { get; set; }
        public string CityName { get; set; }
        public string Properties1 { get; set; }
    }

    public class CityMapModel
    {
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string MapZoomIndex { get; set; }
        public string ParentID { get; set; }
        public string IsCity { get; set; }
    }

    public class CitySearchModel
    {
        public string culture { get; set; }
        public string city { get; set; }
        public string checkInDate { get; set; }
        public string checkOutDate { get; set; }
        public string countryId { get; set; }
    }

    public class CityRoutingModel
    {
        public string RoutingName { get; set; }
        public string Code { get; set; }
    }
}