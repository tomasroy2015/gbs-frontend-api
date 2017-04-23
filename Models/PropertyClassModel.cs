using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GBSHotels.API.Models
{
    public class PropertyClassModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Sort { get; set; }
    }

    public class PenaltyRateModel
    {
        public int Id { get; set; }
        public string PaneltyRate { get; set; }
    }
}