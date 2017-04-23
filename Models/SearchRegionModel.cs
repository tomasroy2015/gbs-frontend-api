using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GBSHotels.API.Models
{
    public class SearchRegionModel
    {
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string MapZoomIndex { get; set; }
        public string ParentID { get; set; }
        public string IsCity { get; set; }
    }
}