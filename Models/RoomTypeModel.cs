using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GBSHotels.API.Models
{
    public class RoomTypeModel
    {
        public int Id { get; set; }
        public string TypeName { get; set; }
    }
    public class RoomViewTypeModel
    {
        public int Id { get; set; }
        public string TypeName { get; set; }
    }
    public class SmokingTypeModel
    {
        public int Id { get; set; }
        public string TypeName { get; set; }
    }
    public class RoomAttributeModel
    {
        public int Id { get; set; }
        public string AttributeName { get; set; }
    }
}