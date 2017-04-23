using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GBSHotels.API.Models
{
    public class PropertTypeModel
    {
        public int Id { get; set; }
        public string TypeName { get; set; }
    }
    public class PropertAttributeModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}