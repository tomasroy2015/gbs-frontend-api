using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GBSHotels.API.Models
{
    public class PopularDestinationModel
    {
        public string CityName { get; set; }

        public string CityImage { get; set; }
        public string Code { get; set; }
        public int Count { get; set; }
        public string Description { get; set; }
        public string Description1 { get; set; }
        public int CityId { get; set; }
        public string CountryName { get; set; }
        public string NavigateURL { get; set; }
    }
}