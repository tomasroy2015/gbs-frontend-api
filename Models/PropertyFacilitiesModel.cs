using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GBSHotels.API.Models
{
    public class PropertyFacilitiesModel
    {
        public string FacilityName { get; set; }
        public int Id { get; set; }
        public string Charge { get; set; }
        public string PropertyLabelDesign { get; set; }
    }
}