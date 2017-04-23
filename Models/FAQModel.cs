using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GBSHotels.API.Models
{
    public class FAQModel
    {
        public int Id { get; set; }
        public string Questions { get; set; }
        public string Answers { get; set; }
    }
}