using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GBSHotels.API.Models
{
    public class CurrencyModel
    {
        public int Id { get; set; }
        public string CurrencySymbol { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencyCode { get; set; }
    }
}