using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GBSHotels.API.Models
{
    public class CultureModel
    {
        public string Description { get; set; }

        public string CultureCode { get; set; }

        public string CultureImageClass { get; set; }

        public string SystemCode { get; set; }
    }

    public enum Cultureenum
    {
        en,
        ar,
        ru,
        tr,
        de,
        fr,
    }
}