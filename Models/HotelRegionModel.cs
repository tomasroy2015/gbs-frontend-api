using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GBSHotels.API.Models
{
    public class HotelRegionModel
    {
        public string Count { get; set; }
        public string RegionName { get; set; }
        public string CountryName { get; set; }
        public string Code { get; set; }
        public string RegionID { get; set; }
        public string topphoto { get; set; }
        public string topphotoflag { get; set; }
        public string textmessage { get; set; }
        public string HotelID { get; set; }
        public string CountryCount { get; set; }
        public string NavigateURL { get; set; }
    }
}