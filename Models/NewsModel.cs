using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GBSHotels.API.Models
{
    public class NewsModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        public string Name { get; set; }

        public string Travel { get; set; }

        public string CreatedDate { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Image { get; set; }   
    }
}