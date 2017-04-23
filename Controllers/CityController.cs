using GBSHotels.API.Helper;
using GBSHotels.API.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GBSHotels.API.Controllers
{
    public class CityController : BaseApiController
    {
        private Home homeService;
        public CityController()
        {
            homeService = new Home();
        }
        [HttpGet]
        public HttpResponseMessage GetCity(Int64 cityId, string culture)
        {
            string city = Convert.ToString(cityId);


            List<CityDetailModel> listCity = new List<CityDetailModel>();
            DataSet ds = new DataSet();
            try
            {
                ds = homeService.GetCity(culture, city);
                DataTable dt1 = new DataTable();
                DataTable dt2 = new DataTable();
                DataTable dt3 = new DataTable();
                CityDetailModel cityDetail = new CityDetailModel();
                if (ds != null)
                {
                    dt1 = ds.Tables[0];
                    dt2 = ds.Tables[1];
                    dt3 = ds.Tables[2];

                    if (dt1.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt1.Rows)
                        {
                            cityDetail.CityImages = "/Images/" + dr["CityID"].ToString() + ".jpg";
                            cityDetail.Properties = dr["Properties"].ToString();
                            cityDetail.CountryName = dr["CountryName"].ToString();
                            cityDetail.CountryID = dr["CountryID"].ToString();
                            cityDetail.CountryNameineng = dr["CountryNameineng"].ToString();
                        }

                    }

                    if (dt2.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt2.Rows)
                        {
                            cityDetail.CityName = dr["CityName"].ToString();
                        }

                    }

                    if (dt3.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt3.Rows)
                        {
                            cityDetail.Properties1 = dr["Properties1"].ToString();
                        }

                    }
                    listCity.Add(cityDetail);

                }
                return Request.CreateResponse(HttpStatusCode.OK, listCity);
            }
            catch (Exception ex)
            {
                return LogError(ex);
            }
        }
        [HttpGet]
        public HttpResponseMessage GetHotels(Int64 cityId, string culture, string currency)
        {
            Home obj = new Home();
            DataTable Hotels = new DataTable();
            List<HotelModel> listHotel = new List<HotelModel>();
            string currentCurrency = "";
            string CurrencyFromDB = "";
            string CurrenySym = "";
            double ConveretedPrice = 0;
            currentCurrency = currency;

            double RoomPrice = 0;
            try
            {
                string city = Convert.ToString(cityId);

                Hotels = obj.GetHotels(city, culture);
                string BookNow = obj.GetTextMessagesAsString(culture, "BookNow");
                if (Hotels.Rows.Count > 0)
                {
                    foreach (DataRow dr in Hotels.Rows)
                    {
                        HotelModel hotel = new HotelModel();

                        hotel.Id = Convert.ToInt32(dr["ID"]);
                        hotel.IsPreferred = dr["IsPreferred"].ToString();
                        hotel.Description = homeService.TruncateLongString(dr["Description"].ToString(), 200);
                        hotel.RoutingName = dr["RoutingName"].ToString();
                        hotel.ClosestAirportName = dr["ClosestAirportName"].ToString();
                        hotel.ClosestAirportDistance = dr["ClosestAirportDistance"].ToString();

                        if (dr["MainPhotoName"].ToString() != "" && dr["MainPhotoName"].ToString() != null)
                        {
                            hotel.MainPhotoName = URL.EXTRANET_URLFULL + "Photo/Hotel/" + dr["ID"].ToString() + "/" + dr["MainPhotoName"].ToString();
                        }
                        else
                        {
                            hotel.MainPhotoName = "/Images/image_not_found-hotel-.jpg";
                        }
                        // 
                        hotel.HotelClassValue = dr["HotelClass"].ToString();

                        if (dr["HotelClass"].ToString() == "OneStar")
                        {
                            hotel.HotelClass = "<img src=" + @"""/Images/star-small-active.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img src=" + @"""/Images/star-small-inactive.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img src=" + @"""/Images/star-small-inactive.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img src=" + @"""/Images/star-small-inactive.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img src=" + @"""/Images/star-small-inactive.png""" + "border=" + @"""0""" + ">&nbsp;";
                        }
                        else if (dr["HotelClass"].ToString() == "TwoStar")
                        {
                            hotel.HotelClass = "<img src=" + @"""/Images/star-small-active.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img src=" + @"""/Images/star-small-active.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img src=" + @"""/Images/star-small-inactive.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img src=" + @"""/Images/star-small-inactive.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img src=" + @"""/Images/star-small-inactive.png""" + "border=" + @"""0""" + ">&nbsp;";
                        }
                        else if (dr["HotelClass"].ToString() == "ThreeStar")
                        {
                            hotel.HotelClass = "<img src=" + @"""/Images/star-small-active.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img src=" + @"""/Images/star-small-active.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img src=" + @"""/Images/star-small-active.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img src=" + @"""/Images/star-small-inactive.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img src=" + @"""/Images/star-small-inactive.png""" + "border=" + @"""0""" + ">&nbsp;";
                        }
                        else if (dr["HotelClass"].ToString() == "FourStar")
                        {
                            hotel.HotelClass = "<img src=" + @"""/Images/star-small-active.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img src=" + @"""/Images/star-small-active.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img src=" + @"""/Images/star-small-active.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img src=" + @"""/Images/star-small-active.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img src=" + @"""/Images/star-small-inactive.png""" + "border=" + @"""0""" + ">&nbsp;";
                        }
                        else if (dr["HotelClass"].ToString() == "FiveStar")
                        {
                            hotel.HotelClass = "<img src=" + @"""/Images/star-small-active.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img src=" + @"""/Images/star-small-active.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img src=" + @"""/Images/star-small-active.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img src=" + @"""/Images/star-small-active.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img src=" + @"""/Images/star-small-active.png""" + "border=" + @"""0""" + ">&nbsp;";
                        }
                        else
                        {
                            hotel.HotelClass = "Unrated";
                        }
                        if (dr["HotelRoutingName"].ToString() == "")
                        {
                            hotel.NavigateURL = "#";
                        }
                        else
                        {
                           // hotel.NavigateURL = "/Hotel_en/" + dr["CountryCode"].ToString() + "/" + dr["HotelRoutingName"].ToString();
                            hotel.NavigateURL = "/#!/hoteldetail?hotelId=" + dr["ID"].ToString() + "&hotelname=" + dr["HotelRoutingName"].ToString();
                        }

                        hotel.CityNavigateURL = "/#!/hotelRegion?cityId=" + dr["RegionID"].ToString() + "&city=" + dr["CityRoutingName"].ToString();
                       // hotel.CityNavigateURL = "/Hotel_en/" + dr["CountryRoutingName"].ToString() + "/" + dr["CityRoutingName"].ToString();

                        hotel.CityName = dr["CityName"].ToString();
                        hotel.CurrencyID = dr["CurrencyID"].ToString();
                        hotel.CurrencySymbol = dr["CurrencySymbol"].ToString();
                        hotel.ReviewCount = dr["ReviewCount"].ToString();
                        if ((dr["ReviewCount"]).ToString() == "0")
                        {
                            hotel.HotelStatus = dr["No"].ToString();
                        }

                        else if ((dr["ReviewCount"]).ToString() == "1")
                        {
                            hotel.HotelStatus = dr["New"].ToString(); ;
                        }
                        else
                        {
                            hotel.HotelStatus = dr["VeryGood"].ToString(); ;
                        }
                        hotel.HotelRoomID = dr["RoomId"].ToString();
                        //objMember.CurrencyCode = dr["CurrencyCode"].ToString();
                        if (dr["CurrencyCode"].ToString() != null && dr["CurrencyCode"].ToString() != "")
                        {
                            hotel.CurrencyCode = dr["CurrencyCode"].ToString();
                        }
                        else
                        {
                            hotel.CurrencyCode = "EUR";
                        }
                        RoomPrice = 0;
                        if (dr["RoomPrice"].ToString() != "")
                        {

                            RoomPrice = Convert.ToDouble(dr["RoomPrice"]);
                            hotel.RoomPrice = dr["RoomPrice"].ToString();
                        }

                        hotel.Avgprice = dr["Averagepricepernight"].ToString();
                        hotel.KmFrom = dr["KmFrom"].ToString();
                        hotel.Superb = dr["Superb"].ToString();
                        hotel.Hotel = dr["Hotel"].ToString();
                        hotel.ScoreFrom = dr["ScoreFrom"].ToString();
                        hotel.Reviews = dr["Reviews"].ToString();
                        hotel.DescriptionText = dr["DescriptionText"].ToString();
                        hotel.VeryGood = dr["VeryGood"].ToString();
                        hotel.New = dr["New"].ToString();

                        hotel.Book = BookNow;

                        if (currentCurrency != "" && (currentCurrency != dr["CurrencyCode"].ToString()))
                        {
                            if (CurrencyFromDB != dr["CurrencyCode"].ToString())
                            {
                                CurrencyFromDB = dr["CurrencyCode"].ToString();
                                string Priceconvert = homeService.CurrencyConvertion(CurrencyFromDB, currentCurrency);
                                string[] split = Priceconvert.Split(null);
                                string price = split[0];
                                CurrenySym = split[1];
                                ConveretedPrice = Convert.ToDouble(price);

                                double roomPrice = RoomPrice;

                                double ConvertedRoomPrice = roomPrice * ConveretedPrice;
                                hotel.ConvertedRoomPrice = Math.Round(ConvertedRoomPrice);
                                hotel.NewCurrencySymbol = CurrenySym;

                            }
                            else
                            {
                                double roomPrice = RoomPrice;
                                double ConvertedRoomPrice = roomPrice * ConveretedPrice;
                                hotel.ConvertedRoomPrice = Math.Round(ConvertedRoomPrice);
                                hotel.NewCurrencySymbol = CurrenySym;
                            }
                        }
                        else
                        {
                            hotel.ConvertedRoomPrice = RoomPrice;
                            hotel.NewCurrencySymbol = dr["CurrencySymbol"].ToString();
                        }
                        listHotel.Add(hotel);

                    }

                }
                return Request.CreateResponse(HttpStatusCode.OK, listHotel);
            }

            catch (Exception ex)
            {
                return LogError(ex);
            }
        }

        public HttpResponseMessage GetRegions(string culture, string regionId)
        {
            DataTable dt = new DataTable();
            List<CityMapModel> objlist = new List<CityMapModel>();

            try
            {
                dt = homeService.GetRegions(culture, regionId);

                if (dt != null && dt.Rows.Count > 0)
                {

                    CityMapModel obj = new CityMapModel();

                    obj.Latitude = dt.Rows[0]["Latitude"].ToString();
                    obj.Longitude = dt.Rows[0]["Longitude"].ToString();
                    obj.MapZoomIndex = dt.Rows[0]["MapZoomIndex"].ToString();
                    obj.ParentID = dt.Rows[0]["ParentID"].ToString();
                    obj.IsCity = dt.Rows[0]["IsCity"].ToString();
                    objlist.Add(obj);

                }
                return Request.CreateResponse(HttpStatusCode.OK, objlist);
            }
            catch (Exception ex)
            {
                return LogError(ex);
            }
        }

        [HttpPost]
        public HttpResponseMessage GetHotelsLocation(string culture, string regionId, string hids)
        {
            DataTable dt = new DataTable();


            List<HotelModel> listHotel = new List<HotelModel>();

            try
            {
                DataSet ds = homeService.GetSearchResultsHotels(culture, regionId,hids);

                // DataSet ds = ObjModel.PageLoadGetHotelInformation("ID", CultureID, Int32.MaxValue, 1, 1, null);
                string BookNow = homeService.GetTextMessagesAsString(culture, "BookNow");
                string Close = homeService.GetTextMessagesAsString(culture, "Close");
                if (ds != null)
                {
                    dt = ds.Tables[1];
                }

                if (dt != null && dt.Rows.Count > 0)
                {

                    foreach (DataRow dr in dt.Rows)
                    {
                        HotelModel obj = new HotelModel();

                        obj.Id = Convert.ToInt32(dr["ID"]);
                        obj.CountryID = dr["CountryID"].ToString();
                        obj.CityID = dr["CityID"].ToString();
                        obj.CityName = dr["CityName"].ToString();
                        obj.RegionID = dr["RegionID"].ToString();
                        obj.MainRegionID = dr["MainRegionID"].ToString();
                        obj.Name = dr["Name"].ToString();
                        obj.Description = dr["Description"].ToString();
                        obj.Address = dr["Address"].ToString();
                        obj.Phone = dr["Phone"].ToString();
                        obj.Fax = dr["Fax"].ToString();
                        obj.WebAddress = dr["WebAddress"].ToString();
                        obj.Email = dr["Email"].ToString();
                        obj.CurrencyID = dr["CurrencyID"].ToString();
                        obj.Latitude = dr["Latitude"].ToString();
                        obj.Longitude = dr["Longitude"].ToString();
                        obj.MapZoomIndex = dr["MapZoomIndex"].ToString();
                        obj.RoutingName = dr["RoutingName"].ToString();
                        obj.CountryName = dr["CountryName"].ToString();
                        obj.MainRegionName = dr["MainRegionName"].ToString();
                        obj.RegionName = dr["RegionName"].ToString();
                        if (dr["MainPhotoName"].ToString() != "" && dr["MainPhotoName"].ToString() != null)
                        {
                            obj.MainPhotoName = URL.EXTRANET_URLFULL + "Photo/Hotel/" + dr["ID"].ToString() + "/" + dr["MainPhotoName"].ToString();
                        }
                        else
                        {
                            obj.MainPhotoName = "/Images/image_not_found-hotel-.jpg";
                        }
                        if (dr["HotelRoutingName"].ToString() == "")
                        {
                            obj.NavigateURL = "#";
                        }
                        else
                        {
                            obj.NavigateURL = "/Hotel_en/" + dr["CountryCode"].ToString() + "/" + dr["HotelRoutingName"].ToString();
                        }

                        if (dr["HotelClassCode"].ToString() == "OneStar")
                        {
                            //objMember.HotelClass = "<img src=" + @"""/Images/star-small-active.png""" + "border=" + @"""0""" + ">&nbsp;";
                            obj.HotelClass = "<ul class='icon-group booking-item-rating-stars'>" +
                                                                                    "<li><i class='fa fa-star'></i>" +
                                                                                    "</li>" +
                                                                                    "<li><i class='fa fa-star-o'></i>" +
                                                                                    "</li>" +
                                                                                    "<li><i class='fa fa-star-o'></i>" +
                                                                                    "</li>" +
                                                                                    "<li><i class='fa fa-star-o'></i>" +
                                                                                    "</li>" +
                                                                                    "<li><i class='fa fa-star-o'></i>" +
                                                                                    "</li>" +
                                                                            "</ul>";
                        }
                        else if (dr["HotelClassCode"].ToString() == "TwoStar")
                        {
                            // objMember.HotelClass = "<img src=" + @"""/Images/star-small-active.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img src=" + @"""/Images/star-small-active.png""" + "border=" + @"""0""" + ">&nbsp;";
                            obj.HotelClass = "<ul class='icon-group booking-item-rating-stars'>" +
                                                                                  "<li><i class='fa fa-star'></i>" +
                                                                                  "</li>" +
                                                                                  "<li><i class='fa fa-star'></i>" +
                                                                                  "</li>" +
                                                                                  "<li><i class='fa fa-star-o'></i>" +
                                                                                  "</li>" +
                                                                                  "<li><i class='fa fa-star-o'></i>" +
                                                                                  "</li>" +
                                                                                  "<li><i class='fa fa-star-o'></i>" +
                                                                                  "</li>" +
                                                                          "</ul>";
                        }
                        else if (dr["HotelClassCode"].ToString() == "ThreeStar")
                        {
                            //objMember.HotelClass = "<img src=" + @"""/Images/star-small-active.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img src=" + @"""/Images/star-small-active.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img src=" + @"""/Images/star-small-active.png""" + "border=" + @"""0""" + ">&nbsp;";
                            obj.HotelClass = "<ul class='icon-group booking-item-rating-stars'>" +
                                                                                  "<li><i class='fa fa-star'></i>" +
                                                                                  "</li>" +
                                                                                  "<li><i class='fa fa-star'></i>" +
                                                                                  "</li>" +
                                                                                  "<li><i class='fa fa-star'></i>" +
                                                                                  "</li>" +
                                                                                  "<li><i class='fa fa-star-o'></i>" +
                                                                                  "</li>" +
                                                                                  "<li><i class='fa fa-star-o'></i>" +
                                                                                  "</li>" +
                                                                          "</ul>";
                        }
                        else if (dr["HotelClassCode"].ToString() == "FourStar")
                        {
                            // objMember.HotelClass = "<img src=" + @"""/Images/star-small-active.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img src=" + @"""/Images/star-small-active.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img src=" + @"""/Images/star-small-active.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img src=" + @"""/Images/star-small-active.png""" + "border=" + @"""0""" + ">&nbsp;";
                            obj.HotelClass = "<ul class='icon-group booking-item-rating-stars'>" +
                                                                                  "<li><i class='fa fa-star'></i>" +
                                                                                  "</li>" +
                                                                                  "<li><i class='fa fa-star'></i>" +
                                                                                  "</li>" +
                                                                                  "<li><i class='fa fa-star'></i>" +
                                                                                  "</li>" +
                                                                                  "<li><i class='fa fa-star'></i>" +
                                                                                  "</li>" +
                                                                                  "<li><i class='fa fa-star-o'></i>" +
                                                                                  "</li>" +
                                                                          "</ul>";
                        }
                        else if (dr["HotelClassCode"].ToString() == "FiveStar")
                        {
                            //  objMember.HotelClass = "<img src=" + @"""/Images/star-small-active.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img src=" + @"""/Images/star-small-active.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img src=" + @"""/Images/star-small-active.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img src=" + @"""/Images/star-small-active.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img src=" + @"""/Images/star-small-active.png""" + "border=" + @"""0""" + ">&nbsp;";
                            obj.HotelClass = "<ul class='icon-group booking-item-rating-stars'>" +
                                                                                  "<li><i class='fa fa-star'></i>" +
                                                                                  "</li>" +
                                                                                  "<li><i class='fa fa-star'></i>" +
                                                                                  "</li>" +
                                                                                  "<li><i class='fa fa-star'></i>" +
                                                                                  "</li>" +
                                                                                  "<li><i class='fa fa-star'></i>" +
                                                                                  "</li>" +
                                                                                  "<li><i class='fa fa-star'></i>" +
                                                                                  "</li>" +
                                                                          "</ul>";
                        }
                        else
                        {
                            obj.HotelClass = "";
                        }
                        obj.ClosestAirportName = dr["ClosestAirportDistance"].ToString() + " " + dr["ClosestAirportName"].ToString();

                        obj.Book = BookNow;
                        obj.Close = Close;

                        listHotel.Add(obj);
                    }

                }

                return Request.CreateResponse(HttpStatusCode.OK, listHotel);
            }
            catch (Exception ex)
            {
                return LogError(ex);
            }
        }
        [HttpPost]
        public HttpResponseMessage GetSearchHotels(CitySearchModel search)
        {
            List<HotelModel> Gethotels = new List<HotelModel>();

            DataSet Hotels = new DataSet();

            int RoomCount = 1;
            string Adultcount = "2";
            string Childrencount = "0";
            int PageSize = 15;
            int PageIndex = 1;

            try
            {

                Hotels = homeService.SearchHotels(search.culture, search.city, search.checkInDate, search.checkOutDate, RoomCount, Adultcount, Childrencount, PageSize, PageIndex, search.countryId);
                DataTable dt1 = new DataTable();
                DataTable dt2 = new DataTable();
                string HotelID = "";
                string BookNow = homeService.GetTextMessagesAsString(search.culture, "BookNow");

                if (Hotels != null)
                {
                    dt1 = Hotels.Tables[0];
                    dt2 = Hotels.Tables[1];

                }
                if (dt2 != null)
                {
                    if (dt2.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt2.Rows)
                        {

                            HotelModel objMember = new HotelModel();
                            objMember.Id = Convert.ToInt32(dr["ID"]);
                            HotelID = dr["ID"].ToString();
                            string Datefromm = "";
                            string Datetoo = "";
                            if (search.checkInDate != "" && search.checkOutDate != "")
                            {
                                double MinRoomPrice = 0;

                                try
                                {
                                    DateTime dat = DateTime.ParseExact(search.checkInDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                    DateTime Dateout = DateTime.ParseExact(search.checkInDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                    Datefromm = dat.ToString("yyyy/MM/dd");
                                    Datetoo = dat.AddDays(-1).ToString("yyyy/MM/dd");
                                }
                                catch
                                {

                                    DateTime dat = DateTime.ParseExact(search.checkInDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                    DateTime Dateout = DateTime.ParseExact(search.checkInDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                    Datefromm = dat.ToString("yyyy/MM/dd");
                                    Datetoo = dat.AddDays(-1).ToString("yyyy/MM/dd");
                                }

                                //DateTime dt = DateTime.ParseExact(checkInDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture);
                                //DateTime Dateout = DateTime.ParseExact(checkOutDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture);

                                //string Datefromm = dt.ToString("yyyy/MM/dd");
                                //string Datetoo = Dateout.ToString("yyyy/MM/dd");

                                DataTable HotelDetails = homeService.GetHotelRoomPrice(search.culture, HotelID, Datefromm, Datetoo);
                                if (HotelDetails.Rows.Count > 0)
                                {
                                    foreach (DataRow drRate in HotelDetails.Rows)
                                    {
                                        MinRoomPrice = 0;
                                        double MinRate = Convert.ToDouble(drRate["MinRoomPrice"]);
                                        if (MinRoomPrice == 0)
                                        {
                                            MinRoomPrice = MinRate;
                                        }
                                        else if (MinRoomPrice > MinRate)
                                        {
                                            MinRoomPrice = MinRate;
                                        }
                                    }
                                    objMember.MinumumRoomPrice = MinRoomPrice;
                                }
                            }
                            objMember.Description = homeService.TruncateLongString(dr["Description"].ToString(), 200);
                            objMember.RoutingName = dr["RoutingName"].ToString();
                            objMember.ClosestAirportName = dr["ClosestAirportName"].ToString();
                            objMember.ClosestAirportDistance = dr["ClosestAirportDistance"].ToString();

                            // objMember.MainPhotoName = "http://www.gbsextranet.com/Photo/Hotel/" + dr["ID"].ToString() + "/" + dr["MainPhotoName"].ToString();

                            if (dr["MainPhotoName"].ToString() != "" && dr["MainPhotoName"].ToString() != null)
                            {
                                objMember.MainPhotoName = URL.EXTRANET_URLFULL + "Photo/Hotel/" + dr["ID"].ToString() + "/" + dr["MainPhotoName"].ToString();
                            }
                            else
                            {
                                objMember.MainPhotoName = "/Images/image_not_found-hotel-.jpg";
                            }

                            if (dr["HotelClassCode"].ToString() == "OneStar")
                            {
                                objMember.HotelClass = "<i class='fa fa-star'></i>&nbsp;<i class='fa fa-star-empty'></i>&nbsp;<i class='fa fa-star-empty'></i>&nbsp;<i class='fa fa-star-empty'></i>&nbsp;<i class='fa fa-star-empty'></i>";
                            }
                            else if (dr["HotelClassCode"].ToString() == "TwoStar")
                            {
                                objMember.HotelClass = "<i class='fa fa-star'></i>&nbsp;<i class='fa fa-star'></i>&nbsp;<i class='fa fa-star-half-empty'></i>&nbsp;<i class='fa fa-star-empty'></i>&nbsp;<i class='fa fa-star-empty'></i>";
                            }
                            else if (dr["HotelClassCode"].ToString() == "ThreeStar")
                            {
                                objMember.HotelClass = "<i class='fa fa-star'></i>&nbsp;<i class='fa fa-star'></i>&nbsp;<i class='fa fa-star'></i>&nbsp;<i class='fa fa-star-half-empty'></i>&nbsp;<i class='fa fa-star-empty'></i>";
                            }
                            else if (dr["HotelClassCode"].ToString() == "FourStar")
                            {
                                objMember.HotelClass = "<i class='fa fa-star'></i>&nbsp;<i class='fa fa-star'></i>&nbsp;<i class='fa fa-star'></i>&nbsp;<i class='fa fa-star'></i>&nbsp;<i class='fa fa-star-half-empty'></i>";
                            }
                            else if (dr["HotelClassCode"].ToString() == "FiveStar")
                            {
                                objMember.HotelClass = "<i class='fa fa-star'></i>&nbsp;<i class='fa fa-star'></i>&nbsp;<i class='fa fa-star'></i>&nbsp;<i class='fa fa-star'></i>&nbsp;<i class='fa fa-star'></i>";
                            }
                            else
                            {
                                objMember.HotelClass = "Unrated";
                            }
                            objMember.CityName = dr["CityName"].ToString();

                            if (dr["HotelRoutingName"].ToString() == "")
                            {
                                objMember.NavigateURL = "#";
                            }
                            else
                            {
                                //objMember.NavigateURL = "/Hotel_en/" + dr["CountryCode"].ToString() + "/" + dr["HotelRoutingName"].ToString();
                                objMember.NavigateURL = "/#!/hoteldetail?hotelId=" + dr["ID"].ToString() + "&hotelname=" + dr["HotelRoutingName"].ToString();
                            }
                            objMember.CityNavigateURL = "/#!/hotelRegion?cityId=" + dr["RegionID"].ToString() + "&city=" + dr["CityRoutingName"].ToString();
                            //objMember.CityNavigateURL = "/Hotels_en/" + dr["CountryRoutingName"].ToString() + "/" + dr["CityRoutingName"].ToString();

                            objMember.CurrencyID = dr["CurrencyID"].ToString();
                            objMember.CurrencySymbol = dr["CurrencySymbol"].ToString();
                            objMember.ReviewCount = dr["ReviewCount"].ToString();
                            if ((dr["ReviewCount"]).ToString() == "0")
                            {
                                objMember.HotelStatus = dr["No"].ToString();
                            }

                            else if ((dr["ReviewCount"]).ToString() == "1")
                            {
                                objMember.HotelStatus = dr["New"].ToString(); ;
                            }
                            else
                            {
                                objMember.HotelStatus = dr["VeryGood"].ToString(); ;
                            }
                            // objMember.HotelRoomID = dr["RoomId"].ToString();
                            objMember.Hotel = dr["Hotel"].ToString();
                            objMember.ScoreFrom = dr["ScoreFrom"].ToString();
                            objMember.Reviews = dr["Reviews"].ToString();

                            if (objMember.CurrencyCode != null && objMember.CurrencyCode != "")
                            {
                                objMember.CurrencyCode = dr["CurrencyCode"].ToString();
                            }
                            else
                            {
                                objMember.CurrencyCode = "EUR";
                            }
                            //  objMember.CurrencyCode = dr["CurrencyCode"].ToString();

                            objMember.Avgprice = dr["Averagepricepernight"].ToString();
                            objMember.KmFrom = dr["KmFrom"].ToString();
                            //objMember.Superb = dr["Superb"].ToString();
                            objMember.DescriptionText = dr["DescriptionText"].ToString();
                            //objMember.VeryGood = dr["VeryGood"].ToString();
                            // objMember.New = dr["New"].ToString();
                            objMember.Book = BookNow;
                            Gethotels.Add(objMember);
                        }
                    }

                }

                return Request.CreateResponse(HttpStatusCode.OK, Gethotels);
            }
            catch (Exception ex)
            {
                return LogError(ex);
            }
        }

    }
}
