using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GBSHotels.API.Models
{
    public class CreditCardType
    {
        public string CreditCardTypeID { get; set; }
        public object SelectText { get; set; }
        public string CreditTypeCode { get; set; }
        public string CreditCardTypeName { get; set; }
        public string CVCLenth { get; set; }
        public string CreditCardTypeImage { get; set; }
        public string CssClass { get; set; }
    }
}