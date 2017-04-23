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
    public class SearchController : BaseApiController
    {
        private Home homeService;
        public SearchController()
        {
            homeService = new Home();
        }
        [HttpGet]
        public HttpResponseMessage GetSortValues(string culture)
        {
            List<SearchSortModel> listSearchSort = new List<SearchSortModel>();
            DataTable dataTableSearch = new DataTable();
            try
            {
                dataTableSearch = homeService.GetSortByValues(culture);

                if (dataTableSearch != null && dataTableSearch.Rows.Count > 0)
                {

                    foreach (DataRow dr in dataTableSearch.Rows)
                    {
                        SearchSortModel objmember = new SearchSortModel();
                        objmember.ID = dr["ID"].ToString();
                        objmember.Name = dr["Name"].ToString();
                        objmember.Sort = dr["SortColumnName"].ToString();
                        listSearchSort.Add(objmember);
                    }
                }
                return Request.CreateResponse(HttpStatusCode.OK, listSearchSort);
            }
            catch (Exception ex)
            {
                return LogError(ex);
            }
        }
        [HttpGet]
        public HttpResponseMessage GetPropertyFacilities(string culture)
        {

            List<PropertTypeModel> listFacility = new List<PropertTypeModel>();
            DataTable dt = new DataTable();
            try
            {
                dt = homeService.GetPropertyFacilities(culture);

                if (dt != null && dt.Rows.Count > 0)
                {

                    foreach (DataRow dr in dt.Rows)
                    {
                        PropertTypeModel facility = new PropertTypeModel();
                        facility.Id = Convert.ToInt32(dr["ID"]);
                        facility.TypeName = dr["Name"].ToString();
                        listFacility.Add(facility);
                    }

                }
                return Request.CreateResponse(HttpStatusCode.OK, listFacility);
            }
            catch (Exception ex)
            {
                return LogError(ex);
            }
        }
        [HttpGet]
        public HttpResponseMessage FilterHotelDetails(string currentCurrency, string culture, string typeHotelClass, string typeHotel, string typeFacilities, string typeRegion, string currencyId,
            int startBudgetValue, int endBudgetValue, string selectedSortValue, string checkInDate, string checkOutDate, string regionID, int pageSize, int pageIndex, int roomCount, string adultcount, string childrencount)
        {

            string Count = "";
            double LowerUSDPrice = 0;
            double UpperUSDPrice = 0;



            List<Home> Gethotels = new List<Home>();
            DataSet Hotels = new DataSet();
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();


            string CurrencyFromDB = "";
            string CurrenySym = "";
            double ConveretedPrice = 0;



            DateTime datee;
            DateTime datee1;
            string Datefromm = "";
            string Datetoo = "";
            double MinRoomPrice = 0;
            try
            {

                if (currencyId != "" && startBudgetValue != 0 && endBudgetValue != 0)
                {
                    string Rate = homeService.CurrencyConvertion(currencyId, "USD");
                    // string Rate = homeService.CurrencyConvertionFilter(currencyId, "USD");

                    string[] Price = Rate.Split(' ');
                    string PriceValue = Price[0];

                    double CurrencyConversionValue = Convert.ToDouble(PriceValue);

                    double StartRange = Convert.ToDouble(startBudgetValue);
                    double EndRange = Convert.ToDouble(endBudgetValue);
                    LowerUSDPrice = CurrencyConversionValue * StartRange;
                    UpperUSDPrice = CurrencyConversionValue * EndRange;
                }

                Hotels = homeService.FilterHotelSearch(culture, regionID, typeHotelClass, typeHotel, typeFacilities, typeRegion, LowerUSDPrice, UpperUSDPrice, selectedSortValue, checkInDate, checkOutDate, pageSize, pageIndex, roomCount, adultcount, childrencount);
                string From = homeService.GetTextMessagesAsString(culture, "from");
                string BookNow = homeService.GetTextMessagesAsString(culture, "BookNow");
                string Prevoius = homeService.GetTextMessagesAsString(culture, "PagingPreviousPage");
                string Next = homeService.GetTextMessagesAsString(culture, "PagingNextPage");

                if (Hotels != null)
                {
                    dt1 = Hotels.Tables[0];
                    dt2 = Hotels.Tables[1];
                }
                if (dt1.Rows.Count > 0)
                {
                    // Home count = new Home();
                    Count = dt1.Rows[0]["Column1"].ToString();
                }
                if (dt2.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt2.Rows)
                    {
                        Home objMember = new Home();

                        List<string> HotelThumbImages = new List<string>();
                        objMember.Count = Count;

                        objMember.ID = dr["ID"].ToString();
                        string HotelID = dr["ID"].ToString();

                        if (!string.IsNullOrEmpty(checkInDate) && !string.IsNullOrEmpty(checkOutDate))
                        {

                            try
                            {
                                datee = DateTime.ParseExact(checkInDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                datee1 = DateTime.ParseExact(checkOutDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                Datefromm = datee.ToString("yyyy/MM/dd");
                                Datetoo = datee1.AddDays(-1).ToString("yyyy/MM/dd");
                            }
                            catch
                            {
                                datee = DateTime.ParseExact(checkInDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                                datee1 = DateTime.ParseExact(checkOutDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                                Datefromm = datee.ToString("yyyy/MM/dd");
                                Datetoo = datee1.AddDays(-1).ToString("yyyy/MM/dd");
                            }

                            DataTable HotelDetails = homeService.GetHotelRoomPrice(culture, HotelID, Datefromm, Datetoo);
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
                            }
                        }
                        else
                        {

                            // MinRoomPrice = obj.GetMinRoomPriceWithoutDate(HotelID);
                            MinRoomPrice = 0;
                            if (dr["MinimumRoomPrice"].ToString() != "")
                            {
                                MinRoomPrice = Convert.ToDouble(dr["MinimumRoomPrice"]);
                            }
                        }
                        objMember.MinumumRoomPrice = MinRoomPrice;
                        int Imagecount = 1;
                        DataTable HotelImages = new DataTable();
                        HotelImages = homeService.GetSlideShowImage(culture, HotelID);
                        if (HotelImages.Rows.Count > 0)
                        {
                            foreach (DataRow drImage in HotelImages.Rows)
                            {
                                if (Imagecount < 13)
                                {
                                    string HotelImage = URL.EXTRANET_URLFULL + "Photo/Hotel/" + drImage["HotelID"].ToString() + "/" + drImage["Photoname"].ToString();

                                    HotelThumbImages.Add(HotelImage);

                                    //if (HotelThumbImages == "")
                                    //{
                                    //    //HotelThumbImages = "<a href=" + "'" + HotelImage + "'" + "  class='preview' style='padding: 3px;'><img  src=" + "'" + HotelImage + "'" + "style='height: 30px; width: 42px;' border='0' ></a>";
                                    //    HotelThumbImages = "<li><a href='" + HotelImage + "' class='image-tooltip-preview popup-gallery-image' data-effect='mfp-zoom-out'>" +
                                    //   "<img src='" + HotelImage + "' alt='gallery thumbnail'></a></li>";
                                    //}
                                    //else
                                    //{
                                    //    HotelThumbImages = HotelThumbImages + "<li><a href='" + HotelImage + "' class='image-tooltip-preview popup-gallery-image' data-effect='mfp-zoom-out'>" +
                                    //   "<img src='" + HotelImage + "' alt='gallery thumbnail'></a></li>";
                                    //    // HotelThumbImages = HotelThumbImages + "<a href=" + "'" + HotelImage + "'" + "  class='preview' style='padding: 3px;'><img  src=" + "'" + HotelImage + "'" + "style='height: 30px; width: 42px;' border='0'></a>";
                                    //}
                                }
                                Imagecount++;
                            }
                        }

                        //objMember.HotelPhotosThumb = "<li><a href='images/image-tooltip/01.jpg' class='image-tooltip-preview popup-gallery-image' data-effect='mfp-zoom-out'>" +
                        //               "<img src='images/image-tooltip/01-sm.jpg' alt='gallery thumbnail'></a></li>";

                        objMember.HotelPhotosThumb = HotelThumbImages;

                        if (dr["MainPhotoName"].ToString() != "" && dr["MainPhotoName"].ToString() != null)
                        {
                            objMember.MainPhotoName = URL.EXTRANET_URLFULL + "Photo/Hotel/" + dr["ID"].ToString() + "/" + dr["MainPhotoName"].ToString();
                        }
                        else
                        {
                            objMember.MainPhotoName = "/Images/image_not_found-hotel-.jpg";
                        }
                        objMember.Name = dr["Name"].ToString();


                        if (dr["HotelClassCode"].ToString() == "OneStar")
                        {
                            //objMember.HotelClass = "<img src=" + @"""/Images/star-small-active.png""" + "border=" + @"""0""" + ">&nbsp;";
                            objMember.HotelClass = "<ul class='icon-group booking-item-rating-stars'>" +
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
                            objMember.HotelClass = "<ul class='icon-group booking-item-rating-stars'>" +
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
                            objMember.HotelClass = "<ul class='icon-group booking-item-rating-stars'>" +
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
                            objMember.HotelClass = "<ul class='icon-group booking-item-rating-stars'>" +
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
                            objMember.HotelClass = "<ul class='icon-group booking-item-rating-stars'>" +
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
                            objMember.HotelClass = "Unrated";
                        }

                        if (dr["HotelRoutingName"].ToString() == "")
                        {
                            objMember.NavigateURL = "#";
                        }
                        else
                        {
                            objMember.NavigateURL = "/#!/hotelRegion?cityId=" + dr["RegionID"].ToString() + "&city=" + dr["HotelRoutingName"].ToString(); //"/Hotel_en/" + dr["CountryCode"].ToString() + "/" + dr["HotelRoutingName"].ToString();
                        }


                        objMember.Address = dr["Address"].ToString();
                        objMember.Description = objMember.TruncateLongString(dr["Description"].ToString(), 120);
                        objMember.CurrencySymbol = dr["CurrencySymbol"].ToString();
                        objMember.CityName = dr["CityName"].ToString();
                        objMember.Hotel = dr["Hotel"].ToString();
                        objMember.CountryName = dr["CountryName"].ToString();
                        objMember.ReviewCountValue = Convert.ToInt32(dr["ReviewCount"]);
                        objMember.AverageReviewPoint = dr["AverageReviewPoint"].ToString();
                        objMember.CurrencyCode = dr["CurrencyCode"].ToString();
                        objMember.ReviewEvaluationName = dr["ReviewEvaluationName"].ToString();
                        objMember.ScoreFrom = dr["ScoreFrom"].ToString();
                        objMember.Reviews = dr["Reviews"].ToString();

                        Home gethotelreviewcount = new Home();
                        double count = 0;
                        if (dr["AveragePoint"].ToString() != "")
                        {
                            count = Convert.ToDouble(dr["AveragePoint"]);
                        }


                        objMember.AverageReviewPointCount = count;
                        string avgPoint = count.ToString();

                        if (avgPoint != "" && avgPoint != "0")
                        {
                            string typescale = homeService.gettypescale(count.ToString(), culture);
                            {
                                objMember.ReviewTypeScaleName = typescale;
                            }
                        }
                        else
                        {
                            objMember.ReviewTypeScaleName = "";
                        }
                        if (count == 10.00)
                        {

                            objMember.AverageReviewPoint = "<a href=" + @"""#""" + "style=" + @"""background-color:#0BB3F9""" + "class=" + @"""btn btn-circle1""" + "><i class=" + @"""fa fa-user""" + "></i></a>" + "<a href=" + @"""#""" + "style=" + @"""background-color:#0BB3F9""" + "class=" + @"""btn btn-circle1""" + "><i class=" + @"""fa fa-user""" + "></i></a>" + "<a href=" + @"""#""" + "style=" + @"""background-color:#0BB3F9""" + "class=" + @"""btn btn-circle1""" + "><i class=" + @"""fa fa-user""" + "></i></a>" + "<a href=" + @"""#""" + "style=" + @"""background-color:#0BB3F9""" + "class=" + @"""btn btn-circle1""" + "><i class=" + @"""fa fa-user""" + "></i></a>" + "<a href=" + @"""#""" + "style=" + @"""background-color:#0BB3F9""" + "class=" + @"""btn btn-circle1""" + "><i class=" + @"""fa fa-user""" + "></i></a>" + "";


                        }
                        else if (count == 7.50)
                        {
                            objMember.AverageReviewPoint = "<a href=" + @"""#""" + "style=" + @"""background-color:#0BB3F9""" + "class=" + @"""btn btn-circle1""" + "><i class=" + @"""fa fa-user""" + "></i></a>" + "<a href=" + @"""#""" + "style=" + @"""background-color:#0BB3F9""" + "class=" + @"""btn btn-circle1""" + "><i class=" + @"""fa fa-user""" + "></i></a>" + "<a href=" + @"""#""" + "class=" + @"""btn btn-circle1""" + "><i class=" + @"""fa fa-user""" + "></i></a>" + "<a href=" + @"""#""" + "class=" + @"""btn btn-circle1""" + "><i class=" + @"""fa fa-user""" + "></i></a>" + "<a href=" + @"""#""" + "class=" + @"""btn btn-circle1""" + "><i class=" + @"""fa fa-user""" + "></i></a>" + "";

                        }
                        else if (count == 5.00)
                        {
                            objMember.AverageReviewPoint = "<a href=" + @"""#""" + "style=" + @"""background-color:#0BB3F9""" + "class=" + @"""btn btn-circle1""" + "><i class=" + @"""fa fa-user""" + "></i></a>" + "<a href=" + @"""#""" + "style=" + @"""background-color:#0BB3F9""" + "class=" + @"""btn btn-circle1""" + "><i class=" + @"""fa fa-user""" + "></i></a>" + "<a href=" + @"""#""" + "style=" + @"""background-color:#0BB3F9""" + "class=" + @"""btn btn-circle1""" + "><i class=" + @"""fa fa-user""" + "></i></a>" + "<a href=" + @"""#""" + "class=" + @"""btn btn-circle1""" + "><i class=" + @"""fa fa-user""" + "></i></a>" + "<a href=" + @"""#""" + "class=" + @"""btn btn-circle1""" + "><i class=" + @"""fa fa-user""" + "></i></a>" + "";

                        }
                        else if (count == 2.50)
                        {
                            objMember.AverageReviewPoint = "<a href=" + @"""#""" + "style=" + @"""background-color:#0BB3F9""" + "class=" + @"""btn btn-circle1""" + "><i class=" + @"""fa fa-user""" + "></i></a>" + "<a href=" + @"""#""" + "class=" + @"""btn btn-circle1""" + "><i class=" + @"""fa fa-user""" + "></i></a>" + "<a href=" + @"""#""" + "class=" + @"""btn btn-circle1""" + "><i class=" + @"""fa fa-user""" + "></i></a>" + "<a href=" + @"""#""" + "class=" + @"""btn btn-circle1""" + "><i class=" + @"""fa fa-user""" + "></i></a>" + "<a href=" + @"""#""" + "class=" + @"""btn btn-circle1""" + "><i class=" + @"""fa fa-user""" + "></i></a>" + "";

                        }

                        else
                        {
                            objMember.AverageReviewPoint = "";

                        }



                        string LastBookingMade = GetLastHotelBookingMessage(HotelID, culture);
                        objMember.LastBookingMadeText = LastBookingMade;
                        try
                        {
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

                                    double roomPrice = MinRoomPrice;

                                    double ConvertedRoomPrice = roomPrice * ConveretedPrice;
                                    objMember.ConvertedRoomPrice = Math.Round(ConvertedRoomPrice);
                                    objMember.NewCurrencySymbol = CurrenySym;



                                }
                                else
                                {
                                    double roomPrice = MinRoomPrice;
                                    double ConvertedRoomPrice = roomPrice * ConveretedPrice;
                                    objMember.ConvertedRoomPrice = Math.Round(ConvertedRoomPrice);
                                    objMember.NewCurrencySymbol = CurrenySym;
                                }
                            }
                            else
                            {
                                objMember.ConvertedRoomPrice = MinRoomPrice;
                                objMember.NewCurrencySymbol = dr["CurrencySymbol"].ToString();
                            }
                        }
                        catch (Exception ex)
                        {
                            objMember.ConvertedRoomPrice = MinRoomPrice;
                            objMember.NewCurrencySymbol = dr["CurrencySymbol"].ToString();
                            CreateErrorLogFile CreateErrorLogFile = new CreateErrorLogFile();
                            CreateErrorLogFile.createerrorlogfiles();
                            CreateErrorLogFile.Errorlog(System.Web.HttpContext.Current.Server.MapPath("\\LogFiles\\Logs"), ex.Source.Length.ToString() + ex.StackTrace + ex.Message);
                        }

                        objMember.ShowMap = dr["ShowMap"].ToString();
                        objMember.KmFrom = dr["KmFrom"].ToString();

                        objMember.ClosestAirportName = dr["ClosestAirportName"].ToString();
                        objMember.ClosestAirportDistance = dr["ClosestAirportDistance"].ToString();
                        objMember.DescriptionText = dr["DescriptionText"].ToString();
                        objMember.Book = BookNow;
                        objMember.froms = From;
                        objMember.PagingPreviousPage = Prevoius;
                        objMember.PagingNextPage = Next;
                        Gethotels.Add(objMember);

                    }
                }
                return Request.CreateResponse(HttpStatusCode.OK, Gethotels);
            }
            catch (Exception ex)
            {
                return LogError(ex);
            }
        }
        public string GetLastHotelBookingMessage(string HotelID, string Cultureid)
        {

            DataTable dt = new DataTable();
            string Status = "";
            try
            {
                dt = homeService.GetLastHotelBookingMade(HotelID, Cultureid);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        try
                        {
                            DateTime ReservationDate = Convert.ToDateTime(dr["ReservationDate"]);
                            DateTime now = DateTime.Now;
                            string Country = dr["Country"].ToString();
                            TimeSpan ts = now - ReservationDate;
                            int differenceInDays = ts.Days;
                            int differenceInHours = ts.Hours;
                            string LatestBookingDate = dr["LatestBookingDate"].ToString();

                            if (differenceInDays < 1)
                            {
                                if (differenceInHours >= 2)
                                {
                                    LatestBookingDate = LatestBookingDate.Replace("#Unit#", dr["Hours"].ToString());
                                    LatestBookingDate = LatestBookingDate.Replace("#Count#", differenceInHours.ToString());
                                }
                                else
                                {
                                    LatestBookingDate = LatestBookingDate.Replace("#Unit#", dr["Hour"].ToString());
                                    LatestBookingDate = LatestBookingDate.Replace("#Count#", differenceInHours.ToString());
                                }
                                Status = LatestBookingDate + " - " + Country;
                            }
                            else if (differenceInDays > 0 && differenceInDays < 3)
                            {
                                if (differenceInDays > 1)
                                {
                                    LatestBookingDate = LatestBookingDate.Replace("#Unit#", dr["Days"].ToString());
                                    LatestBookingDate = LatestBookingDate.Replace("#Count#", differenceInDays.ToString());
                                }
                                else
                                {
                                    LatestBookingDate = LatestBookingDate.Replace("#Unit#", dr["Day"].ToString());
                                    LatestBookingDate = LatestBookingDate.Replace("#Count#", differenceInDays.ToString());
                                }
                                Status = LatestBookingDate + " - " + Country;
                            }
                        }
                        catch
                        {

                        }
                    }

                }

            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return Status;
        }

        [HttpGet]
        public HttpResponseMessage GetFeaturedHotelSearch(string currentCurrency, string culture, string regionId,
            string checkInDate, string checkOutDate, string roomCount, string adultcount, string childrenCount, int pageSize, int pageIndex)
        {
            List<Home> Gethotels = new List<Home>();
            int RoomCountNo = Convert.ToInt32(roomCount);


            string CurrencyFromDB = "";
            string CurrenySym = "";
            string CountryID = "";
            double ConveretedPrice = 0;


            DateTime datee;
            DateTime datee1;
            string Datefromm = "";
            string Datetoo = "";
            string Count = "";
            double MinRoomPrice = 0;

            DataSet Hotels = new DataSet();
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();

            try
            {

                Hotels = homeService.SearchHotels(culture, regionId, checkInDate, checkOutDate, RoomCountNo, adultcount, childrenCount, pageSize, pageIndex, CountryID);


                string From = homeService.GetTextMessagesAsString(culture, "from");
                string BookNow = homeService.GetTextMessagesAsString(culture, "BookNow");
                string Prevoius = homeService.GetTextMessagesAsString(culture, "PagingPreviousPage");
                string Next = homeService.GetTextMessagesAsString(culture, "PagingNextPage");

                if (Hotels != null)
                {
                    dt1 = Hotels.Tables[0];
                    dt2 = Hotels.Tables[1];
                }

                if (dt1.Rows.Count > 0)
                {
                    // Home count = new Home();
                    Count = dt1.Rows[0]["Column1"].ToString();
                }
                if (dt2.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt2.Rows)
                    {
                        Home objMember = new Home();

                        List<string> HotelThumbImages = new List<string>();
                        objMember.Count = Count;

                        objMember.ID = dr["ID"].ToString();
                        string HotelID = dr["ID"].ToString();

                        if (!string.IsNullOrEmpty(checkInDate) && !string.IsNullOrEmpty(checkOutDate))
                        {
                            try
                            {
                                datee = DateTime.ParseExact(checkInDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                datee1 = DateTime.ParseExact(checkOutDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                Datefromm = datee.ToString("yyyy/MM/dd");
                                Datetoo = datee1.AddDays(-1).ToString("yyyy/MM/dd");
                            }
                            catch
                            {
                                datee = DateTime.ParseExact(checkInDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                                datee1 = DateTime.ParseExact(checkOutDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                                Datefromm = datee.ToString("yyyy/MM/dd");
                                Datetoo = datee1.AddDays(-1).ToString("yyyy/MM/dd");
                            }

                            DataTable HotelDetails = homeService.GetHotelRoomPrice(culture, HotelID, Datefromm, Datetoo);
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
                            }
                        }
                        else
                        {

                            // MinRoomPrice = obj.GetMinRoomPriceWithoutDate(HotelID);
                            MinRoomPrice = 0;
                            if (dr["MinimumRoomPrice"].ToString() != "")
                            {
                                MinRoomPrice = Convert.ToDouble(dr["MinimumRoomPrice"]);
                            }
                        }
                        objMember.MinumumRoomPrice = MinRoomPrice;
                        int Imagecount = 1;
                        DataTable HotelImages = new DataTable();
                        HotelImages = homeService.GetSlideShowImage(culture, HotelID);
                        List<HotelImages> images = new List<HotelImages>();
                        if (HotelImages.Rows.Count > 0)
                        {
                            foreach (DataRow drImage in HotelImages.Rows)
                            {
                                HotelImages im = new HotelImages();

                                if (Imagecount < 13)
                                {
                                    im.ID = Imagecount;
                                    im.Image = URL.EXTRANET_URLFULL + "Photo/Hotel/" + drImage["HotelID"].ToString() + "/" + drImage["Photoname"].ToString();
                                    string HotelImage = im.Image;
                                    //HotelThumbImages = "<a href=" + "'" + HotelImage + "'" + "  class='preview' style='padding: 3px;'><img  src=" + "'" + HotelImage + "'" + "style='height: 30px; width: 42px;' border='0' ></a>";
                                    HotelThumbImages.Add(HotelImage);
                                }
                                // images.Add(im);
                                Imagecount++;
                            }
                        }

                        objMember.HotelImageThumb = images;
                        objMember.HotelPhotosThumb = HotelThumbImages;


                        if (dr["MainPhotoName"].ToString() != "" && dr["MainPhotoName"].ToString() != null)
                        {
                            objMember.MainPhotoName = URL.EXTRANET_URLFULL + "Photo/Hotel/" + dr["ID"].ToString() + "/" + dr["MainPhotoName"].ToString();
                        }
                        else
                        {
                            objMember.MainPhotoName = "/Images/image_not_found-hotel-.jpg";
                        }
                        objMember.Name = dr["Name"].ToString();

                        //  objMember.HotelClass = dr["HotelClassName"].ToString();

                        if (dr["HotelClassCode"].ToString() == "OneStar")
                        {
                            //objMember.HotelClass = "<img src=" + @"""/Images/star-small-active.png""" + "border=" + @"""0""" + ">&nbsp;";
                            objMember.HotelClass = "<ul class='icon-group booking-item-rating-stars'>" +
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
                            objMember.HotelClass = "<ul class='icon-group booking-item-rating-stars'>" +
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
                            objMember.HotelClass = "<ul class='icon-group booking-item-rating-stars'>" +
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
                            objMember.HotelClass = "<ul class='icon-group booking-item-rating-stars'>" +
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
                            objMember.HotelClass = "<ul class='icon-group booking-item-rating-stars'>" +
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
                            objMember.HotelClass = "Unrated";

                        if (dr["HotelRoutingName"].ToString() == "")
                            objMember.NavigateURL = "#";
                        else
                        {
                            //objMember.NavigateURL = "#hotelRegion?cityId=" + dr["RegionID"].ToString() + "&hotelname=" + dr["HotelRoutingName"].ToString();//"/Hotel_en/" + dr["CountryCode"].ToString() + "/" + dr["HotelRoutingName"].ToString();
                            objMember.NavigateURL = "/#!/hoteldetail?hotelId=" + dr["ID"].ToString() + "&hotelname=" + dr["HotelRoutingName"].ToString();

                        } objMember.CityNavigateURL = "/#!/hotelRegion?cityId=" + dr["RegionID"].ToString() + "&city=" + dr["CityRoutingName"].ToString();//"/Hotels_en/" + dr["CountryRoutingName"].ToString() + "/" + dr["CityRoutingName"].ToString();
                         
                        objMember.Address = dr["Address"].ToString();
                        objMember.Description = objMember.TruncateLongString(dr["Description"].ToString(), 120);
                        objMember.CurrencySymbol = dr["CurrencySymbol"].ToString();
                        objMember.CityName = dr["CityName"].ToString();
                        objMember.Hotel = dr["Hotel"].ToString();
                        objMember.CountryName = dr["CountryName"].ToString();
                        objMember.ReviewCountValue = Convert.ToInt32(dr["ReviewCount"]);
                        objMember.AverageReviewPoint = dr["AverageReviewPoint"].ToString();
                        objMember.CurrencyCode = dr["CurrencyCode"].ToString();
                        objMember.ReviewEvaluationName = dr["ReviewEvaluationName"].ToString();
                        objMember.ScoreFrom = dr["ScoreFrom"].ToString();
                        objMember.Reviews = dr["Reviews"].ToString();
                        objMember.IsPreferred = dr["IsPreferred"].ToString();
                        Home gethotelreviewcount = new Home();

                        double count = 0;
                        if (dr["AveragePoint"].ToString() != "")
                            count = Convert.ToDouble(dr["AveragePoint"]);

                        objMember.AverageReviewPointCount = count;
                        string avgPoint = count.ToString();

                        if (avgPoint != "" && avgPoint != "0")
                        {
                            string typescale = homeService.gettypescale(count.ToString(), culture);
                            objMember.ReviewTypeScaleName = typescale;
                        }
                        else
                            objMember.ReviewTypeScaleName = "";
                        if (count == 10.00)
                            objMember.AverageReviewPoint = "<a href=" + @"""#""" + "style=" + @"""background-color:#0BB3F9""" + "class=" + @"""btn btn-circle1""" + "><i class=" + @"""fa fa-user""" + "></i></a>" + "<a href=" + @"""#""" + "style=" + @"""background-color:#0BB3F9""" + "class=" + @"""btn btn-circle1""" + "><i class=" + @"""fa fa-user""" + "></i></a>" + "<a href=" + @"""#""" + "style=" + @"""background-color:#0BB3F9""" + "class=" + @"""btn btn-circle1""" + "><i class=" + @"""fa fa-user""" + "></i></a>" + "<a href=" + @"""#""" + "style=" + @"""background-color:#0BB3F9""" + "class=" + @"""btn btn-circle1""" + "><i class=" + @"""fa fa-user""" + "></i></a>" + "<a href=" + @"""#""" + "style=" + @"""background-color:#0BB3F9""" + "class=" + @"""btn btn-circle1""" + "><i class=" + @"""fa fa-user""" + "></i></a>" + "";
                        else if (count == 7.50)
                            objMember.AverageReviewPoint = "<a href=" + @"""#""" + "style=" + @"""background-color:#0BB3F9""" + "class=" + @"""btn btn-circle1""" + "><i class=" + @"""fa fa-user""" + "></i></a>" + "<a href=" + @"""#""" + "style=" + @"""background-color:#0BB3F9""" + "class=" + @"""btn btn-circle1""" + "><i class=" + @"""fa fa-user""" + "></i></a>" + "<a href=" + @"""#""" + "class=" + @"""btn btn-circle1""" + "><i class=" + @"""fa fa-user""" + "></i></a>" + "<a href=" + @"""#""" + "class=" + @"""btn btn-circle1""" + "><i class=" + @"""fa fa-user""" + "></i></a>" + "<a href=" + @"""#""" + "class=" + @"""btn btn-circle1""" + "><i class=" + @"""fa fa-user""" + "></i></a>" + "";
                        else if (count == 5.00)
                            objMember.AverageReviewPoint = "<a href=" + @"""#""" + "style=" + @"""background-color:#0BB3F9""" + "class=" + @"""btn btn-circle1""" + "><i class=" + @"""fa fa-user""" + "></i></a>" + "<a href=" + @"""#""" + "style=" + @"""background-color:#0BB3F9""" + "class=" + @"""btn btn-circle1""" + "><i class=" + @"""fa fa-user""" + "></i></a>" + "<a href=" + @"""#""" + "style=" + @"""background-color:#0BB3F9""" + "class=" + @"""btn btn-circle1""" + "><i class=" + @"""fa fa-user""" + "></i></a>" + "<a href=" + @"""#""" + "class=" + @"""btn btn-circle1""" + "><i class=" + @"""fa fa-user""" + "></i></a>" + "<a href=" + @"""#""" + "class=" + @"""btn btn-circle1""" + "><i class=" + @"""fa fa-user""" + "></i></a>" + "";
                        else if (count == 2.50)
                            objMember.AverageReviewPoint = "<a href=" + @"""#""" + "style=" + @"""background-color:#0BB3F9""" + "class=" + @"""btn btn-circle1""" + "><i class=" + @"""fa fa-user""" + "></i></a>" + "<a href=" + @"""#""" + "class=" + @"""btn btn-circle1""" + "><i class=" + @"""fa fa-user""" + "></i></a>" + "<a href=" + @"""#""" + "class=" + @"""btn btn-circle1""" + "><i class=" + @"""fa fa-user""" + "></i></a>" + "<a href=" + @"""#""" + "class=" + @"""btn btn-circle1""" + "><i class=" + @"""fa fa-user""" + "></i></a>" + "<a href=" + @"""#""" + "class=" + @"""btn btn-circle1""" + "><i class=" + @"""fa fa-user""" + "></i></a>" + "";

                        else
                            objMember.AverageReviewPoint = "";
                        string LastBookingMade = GetLastHotelBookingMessage(HotelID, culture);
                        objMember.LastBookingMadeText = LastBookingMade;
                        try
                        {
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

                                    double roomPrice = MinRoomPrice;

                                    double ConvertedRoomPrice = roomPrice * ConveretedPrice;
                                    objMember.ConvertedRoomPrice = Math.Round(ConvertedRoomPrice);
                                    objMember.NewCurrencySymbol = CurrenySym;



                                }
                                else
                                {
                                    double roomPrice = MinRoomPrice;
                                    double ConvertedRoomPrice = roomPrice * ConveretedPrice;
                                    objMember.ConvertedRoomPrice = Math.Round(ConvertedRoomPrice);
                                    objMember.NewCurrencySymbol = CurrenySym;
                                }
                            }
                            else
                            {
                                objMember.ConvertedRoomPrice = MinRoomPrice;
                                objMember.NewCurrencySymbol = dr["CurrencySymbol"].ToString();
                            }
                        }
                        catch (Exception ex)
                        {
                            objMember.ConvertedRoomPrice = MinRoomPrice;
                            objMember.NewCurrencySymbol = dr["CurrencySymbol"].ToString();
                            CreateErrorLogFile CreateErrorLogFile = new CreateErrorLogFile();
                            CreateErrorLogFile.createerrorlogfiles();
                            CreateErrorLogFile.Errorlog(System.Web.HttpContext.Current.Server.MapPath("\\LogFiles\\Logs"), ex.Source.Length.ToString() + ex.StackTrace + ex.Message);
                        }
                        objMember.ShowMap = dr["ShowMap"].ToString();
                        objMember.KmFrom = dr["KmFrom"].ToString();

                        objMember.ClosestAirportName = dr["ClosestAirportName"].ToString();
                        objMember.ClosestAirportDistance = dr["ClosestAirportDistance"].ToString();
                        objMember.DescriptionText = dr["DescriptionText"].ToString();
                        objMember.Book = BookNow;
                        objMember.froms = From;
                        objMember.PagingPreviousPage = Prevoius;
                        objMember.PagingNextPage = Next;

                        Gethotels.Add(objMember);
                    }
                }
                return Request.CreateResponse(HttpStatusCode.OK, Gethotels);
            }
            catch (Exception ex)
            {
                return LogError(ex);
            }

        }
        [HttpGet]
        public HttpResponseMessage GetRegions(string culture, string regionId)
        {
            DataTable dt = new DataTable();
            List<SearchRegionModel> objlist = new List<SearchRegionModel>();

            try
            {
                dt = homeService.GetRegions(culture, regionId);

                if (dt != null && dt.Rows.Count > 0)
                {

                    SearchRegionModel obj = new SearchRegionModel();

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
        //[Route("api/search/getHotelsLocation/{culture}/{regionId}/{zhids?}")]
        public HttpResponseMessage GetHotelsLocation(string culture, string regionId, string zhids="")
        {
            DataTable dt = new DataTable();

            // string RegionID = Convert.ToString(Session["RegionID"]);
            List<Home> objlist = new List<Home>();
            
            try
            {
                DataSet ds = homeService.GetSearchResultsHotels(culture, regionId, zhids);

                // DataSet ds = ObjModel.PageLoadGetHotelInformation("ID", CultureID, Int32.MaxValue, 1, 1, null);
                string BookNow = homeService.GetTextMessagesAsString(culture, "BookNow");
                string Close = homeService.GetTextMessagesAsString(culture, "Close");
                if (ds != null)
                {
                    dt = ds.Tables[1];
                }

                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            Home obj = new Home();

                            obj.ID = dr["ID"].ToString();
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
                                obj.NavigateURL = "/#!/hotelRegion?cityId=" + dr["RegionID"].ToString() + "&city=" + dr["HotelRoutingName"].ToString();// "/Hotel_en/" + dr["CountryCode"].ToString() + "/" + dr["HotelRoutingName"].ToString();
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

                            objlist.Add(obj);
                        }

                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK, objlist);

            }
            catch (Exception ex)
            {
                return LogError(ex);
            }
        }

    }
}
