using Business;
using GBSHotels.API.Helper;
using GBSHotels.API.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace GBSHotels.API.Controllers
{
    //[EnableCors(origins: "http://localhost:49376", headers: "*", methods: "*")]
    public class HotelController : BaseApiController
    {
        private Home homeService;
        public HotelController()
        {
            homeService = new Home();
        }
        BizContext BizContext = new BizContext();
        public void AssignBizContext()
        {
            if (HttpContext.Current.Session[BizCommon.BizContextName] != null)
            {
                BizContext = (BizContext)HttpContext.Current.Session[BizCommon.BizContextName];
            }
            HttpContext.Current.Session[BizCommon.BizContextName] = BizContext;
        }
        [HttpGet]
        public HttpResponseMessage CheckRoomAvailbility(string culture, string hotelId, string dateFrom, string dateTo, string currentCurrency)
        {

            DataTable HotelDetails = new DataTable();
            List<Home> Gethotels = new List<Home>();
            string CurrencyFromDB = "";
            string CurrenySym = "";
            double ConveretedPrice = 0;
            int CreditCardNotRequired = 0;

            DataTable HotelInfor = homeService.GetHotelBasicInfo(culture, hotelId);

            string Select = homeService.GetTextMessagesAsString(culture, "Select");
            string Discount = homeService.GetTextMessagesAsString(culture, "Discount");
            string For1RoomText = homeService.GetTextMessagesAsString(culture, "for1room");
            int RoomOptionCountLoop = 0;

            if (HotelInfor.Rows.Count > 0)
            {
                foreach (DataRow getHotelInfo in HotelInfor.Rows)
                {
                    CreditCardNotRequired = Convert.ToInt32(getHotelInfo["CreditCardNotRequired"]);
                }
            }

            DateTime dt;
            DateTime dt1;
            DateTime dt2;

           
            if (dateFrom.Contains('/'))
            {
                try
                {

                    dt = DateTime.ParseExact(dateFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    dt1 = DateTime.ParseExact(dateTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    dt2 = DateTime.ParseExact(dateTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    dt1 = dt1.AddDays(-1);
                }
                catch
                {
                    dt = DateTime.ParseExact(dateFrom, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    dt1 = DateTime.ParseExact(dateTo, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    dt2 = DateTime.ParseExact(dateTo, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    dt1 = dt1.AddDays(-1);
                }
            }
            else
            {
                try
                {
                    dt = DateTime.ParseExact(dateFrom, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                    dt2 = DateTime.ParseExact(dateTo, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                    dt1 = DateTime.ParseExact(dateTo, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                    dt1 = dt1.AddDays(-1);
                }
                catch
                {
                    dt = DateTime.ParseExact(dateFrom, "MM-dd-yyyy", CultureInfo.InvariantCulture);
                    dt1 = DateTime.ParseExact(dateTo, "MM-dd-yyyy", CultureInfo.InvariantCulture);
                    dt2 = DateTime.ParseExact(dateTo, "MM-dd-yyyy", CultureInfo.InvariantCulture);
                    dt1 = dt1.AddDays(-1);
                }
            }
            double DateDiff = (dt2 - dt).Days;
            double DateDiff1 = (dt2 - dt).TotalDays;
            string Datefromm = dt.ToString("yyyy/MM/dd");
            string Datetoo = dt1.ToString("yyyy/MM/dd");

            string dateFromDisplay = dt.ToString("dd-MMM-yyyy");
            string datedTODisplay = dt2.ToString("dd-MMM-yyyy");


            string guid = System.Guid.NewGuid().ToString();

            string NowdateTim = DateTime.Now.ToString("yyyy/MM/dd");

            DataTable RoomBedDetails = homeService.getBedSelection(culture, hotelId, "");
            DataTable HotelPromotion = homeService.GetHotelPromotionsDailyDiscountPercentage(culture, hotelId, NowdateTim, Datefromm, Datetoo);
            
            HotelDetails = homeService.CheckroomAvailbility(culture, hotelId, Datefromm, Datetoo, BizContext.ShowSecretDeals);
           
            int dataRowCount = 0;
            DataRow lastDataRow;
            if (HotelDetails.Rows.Count > 0)
            {
                lastDataRow = HotelDetails.Rows[HotelDetails.Rows.Count - 1];
                foreach (DataRow dr in HotelDetails.Rows)
                {
                    DataRow NextDataRow = HotelDetails.Rows[dataRowCount];
                    if (dr != lastDataRow)
                    {
                        NextDataRow = HotelDetails.Rows[dataRowCount + 1];
                    }
                    if (dr == lastDataRow || dr["ID"].ToString() != NextDataRow["ID"].ToString() || dr["RoomPrice"].ToString() != NextDataRow["RoomPrice"].ToString() )
                    {
                        
                        Home objRoom = new Home();
                        objRoom.ID = Convert.ToString(dr["ID"]);
                        string optionNo = "";
                        string BedText = "";
                        int countValue = 0;
                        DataTable dticon = new DataTable();
                        List<RoomIcons> objlist = new List<RoomIcons>();
                        dticon = homeService.GetHotelRoomIcons(hotelId, dr["ID"].ToString(), culture);
                        if (dticon.Rows.Count > 0)
                        {
                            foreach (DataRow dricon in dticon.Rows)
                            {
                                RoomIcons objicon = new RoomIcons();

                                objicon.FacilityName = dricon["Facilities"].ToString().Trim();
                                objicon.Icon = dricon["Icon"].ToString();

                                //string str13 = "<i class='im @icon'></i>";
                          
                                //if (objicon.FacilityName.ToLower() == "air conditioning")
                                //{
                                //    objicon.Icon = str13.Replace("@icon", "im-air");
                                //}
                                //else if (objicon.FacilityName.ToLower() == "iron")
                                //{
                                //    objicon.Icon = str13.Replace("@icon", "im-electric");
                                //}
                                //else if (objicon.FacilityName.ToLower() == "ironing facilities")
                                //{
                                //    objicon.Icon = str13.Replace("@icon", "im-lines");
                                //}
                                //else if (objicon.FacilityName.ToLower() == "bath")
                                //{
                                //    objicon.Icon = "<i class='im'>\ud83d\udec0</i>";
                                //}
                                //else if (objicon.FacilityName.ToLower() == "shower")
                                //{
                                //    objicon.Icon = str13.Replace("@icon", "im-shower");
                                //}
                                //else if (objicon.FacilityName.ToLower() == "toilet")
                                //{
                                //    objicon.Icon = "<i class='im'>\ud83d\udebd</i>";
                                //}
                                //else if (objicon.FacilityName.ToLower() == "flat-screen tv")
                                //{
                                //    objicon.Icon = str13.Replace("@icon", "im-tv");
                                //}
                                //else if (objicon.FacilityName.ToLower() == "satellite channels")
                                //{
                                //    objicon.Icon = str13.Replace("@icon", "im-satellite");
                                //}
                                //else if (objicon.FacilityName.ToLower() == "kitchen")
                                //{
                                //    objicon.Icon = str13.Replace("@icon", "im-kitchen");
                                //}
                                //else
                                {
                                    objicon.Icon = "<img src='/Images/Icons/" + dricon["Icon"].ToString() + "' style='" + (Convert.ToString( dricon["Icon"].ToString()).ToLower().Contains("facility.png") ?"width:26px;" : "width:14px;") + "' />";
                                }
                                objlist.Add(objicon);
                            }

                        }

                        objRoom.HotelRoomIcons = objlist;

                        if (RoomBedDetails.Rows.Count > 0)
                        {
                            foreach (DataRow Roomdr in RoomBedDetails.Rows)
                            {
                                if (dr["ID"].ToString() == Roomdr["HotelRoomID"].ToString())
                                {
                                    if (optionNo != Roomdr["OptionNo"].ToString())
                                    {
                                        optionNo = Roomdr["OptionNo"].ToString();
                                        countValue++;
                                        if (BedText == "")
                                        {
                                            BedText = Roomdr["BedTypeNameWithCount"].ToString();
                                        }
                                        else
                                        {
                                            BedText = BedText + "  <b>or</b>  " + Roomdr["BedTypeNameWithCount"].ToString();
                                        }
                                    }
                                    else
                                    {
                                        BedText = BedText + "," + Roomdr["BedTypeNameWithCount"].ToString();
                                    }

                                }
                            }
                        }
                        else
                        {
                            objRoom.RoomBedInfo = "";
                        }
                        if (countValue > 1)
                        {
                            objRoom.RoomBedInfo = BedText;
                        }
                        else
                        {
                            objRoom.RoomBedInfo = "";
                        }

                        if (dr["MaxPeopleCount"].ToString() == "1")
                        {
                            objRoom.MaxPeopleCount = "<i class='fa fa-male'></i>";
                        }
                        else if (dr["MaxPeopleCount"].ToString() == "2")
                        {
                            objRoom.MaxPeopleCount = "<i class='fa fa-male'></i><i class='fa fa-male'></i>";
                        }
                        else if (dr["MaxPeopleCount"].ToString() == "3")
                        {
                            objRoom.MaxPeopleCount = "<i class='fa fa-male'></i><i class='fa fa-male'></i><i class='fa fa-male'></i>";
                        }
                        else if (dr["MaxPeopleCount"].ToString() == "4")
                        {
                            objRoom.MaxPeopleCount = "<i class='fa fa-male'></i><i class='fa fa-male'></i><i class='fa fa-male'></i><i class='fa fa-male'></i>";
                        }
                        else if (dr["MaxPeopleCount"].ToString() == "5")
                        {
                            objRoom.MaxPeopleCount = "<i class='fa fa-male'></i><i class='fa fa-male'></i><i class='fa fa-male'></i><i class='fa fa-male'></i><i class='fa fa-male'></i>";
                        }
                        else if (dr["MaxPeopleCount"].ToString() == "6")
                        {
                            objRoom.MaxPeopleCount = "<i class='fa fa-male'></i><i class='fa fa-male'></i><i class='fa fa-male'></i><i class='fa fa-male'></i><i class='fa fa-male'></i><i class='fa fa-male'></i>";
                        }
                        else
                        {
                            objRoom.MaxPeopleCount = "";
                        }

                        string RoomCountOption = "<option value='0'> " + Select + "</option>";
                        string RadiobtnStyle = "";
                        int RoomCount = Convert.ToInt32(dr["RoomCount"]);

                        for (int k = 1; k <= RoomCount; k++)
                        {
                            if (k < 4)
                            {
                                if (RoomOptionCountLoop == 0)
                                {
                                    RadiobtnStyle = RadiobtnStyle + "<label class='btn btn-primary active'> <input  type='radio'   data-price='" + dr["RoomPrice"] + "'   value='" + k + "' name='options' />" + k + "</label>";
                                }
                                else
                                {
                                    RadiobtnStyle = RadiobtnStyle + "<label class='btn btn-primary' > <input  data-price='" + dr["RoomPrice"] + "'  type='radio'  value='" + k + "' name='options' />" + k + "</label>";
                                }

                            }
                            if (k == 4)
                            {
                                RadiobtnStyle = RadiobtnStyle + "<label class='btn btn-primary'> <input   data-price='" + dr["RoomPrice"] + "'   type='radio' value='" + k + "' name='options' />3+</label>";
                            }
                            RoomCountOption = RoomCountOption + "<option value='" + k + "'>" + k + "</option>";

                            RoomOptionCountLoop++;
                        }

                        objRoom.RoomCountRadioStyle = RadiobtnStyle;
                        objRoom.For1RoomText = For1RoomText;
                        objRoom.CreditCardNotRequired = CreditCardNotRequired;
                        objRoom.OptionRoomCount = RoomCountOption;
                        objRoom.MaxPeopleCountval = dr["MaxPeopleCount"].ToString();
                        objRoom.RoomDataTableType = "RoomDetails";
                        objRoom.CheckDatefrom = dateFromDisplay;
                        objRoom.CheckDateto = datedTODisplay;
                        objRoom.NoCreditCards = dr["NoCreditCards"].ToString();
                        objRoom.PriceType = dr["PriceType"].ToString();
                        objRoom.HotelID = dr["HotelID"].ToString();
                        objRoom.HotelRoomID = dr["ID"].ToString();
                        objRoom.HotelRoomDescription = dr["RoomDescription"].ToString();
                        objRoom.HotelTypeName = dr["RoomTypeName"].ToString();
                        objRoom.SmokingTypeName = dr["SmokingTypeName"].ToString();
                        objRoom.RoomCount = dr["RoomCount"].ToString();
                        objRoom.RoomPrice = dr["RoomPrice"].ToString();
                        objRoom.RoomPriceHistry = dr["RoomPriceHistory"].ToString();
                        objRoom.CurrencyCode = dr["CurrencyCode"].ToString();
                        objRoom.RoomMaxPeopleCount = dr["RoomMaxPeopleCount"].ToString();
                        objRoom.MaxChildrenCount = dr["MaxChildrenCount"].ToString();
                        objRoom.RoomSize = dr["RoomSize"].ToString();
                        objRoom.FREEcancellation = dr["FREEcancellation"].ToString();
                        objRoom.RoomFacilities = dr["RoomFacilities"].ToString();
                        objRoom.UniqueID = dr["UniqueID"].ToString();
                        objRoom.AccommodationTypeID = dr["AccommodationTypeID"].ToString();
                        objRoom.AccommodationTypeName = dr["AccommodationTypeName"].ToString();
                        objRoom.AccommodationTypeDescription = dr["AccommodationTypeDescription"].ToString();
                        objRoom.PricePolicyTypeID = dr["PricePolicyTypeID"].ToString();
                        objRoom.PricePolicyTypeName = dr["PricePolicyTypeName"].ToString();
                        objRoom.VAT = dr["VAT"].ToString();
                        string CityTax = dr["CityTax"].ToString();
                        if (CityTax != "" && CityTax != null)
                        {
                            string[] taxtext = CityTax.Split('&');
                            objRoom.CityTax = taxtext[1];
                        }
                        else
                        {
                            string[] taxtext = CityTax.Split('&');
                            objRoom.CityTax = "";
                        }
                        string BreakfastText = dr["BreakfastText"].ToString();
                        if (BreakfastText != "" && BreakfastText != null)
                        {
                            string[] BreakfastText1 = BreakfastText.Split('&');
                            objRoom.BreakfastText = BreakfastText1[1];
                        }
                        else
                        {
                            string[] BreakfastText1 = BreakfastText.Split('&');
                            objRoom.BreakfastText = "";
                        }


                        objRoom.RoomPriceWarning = dr["RoomPriceWarning"].ToString();
                        objRoom.Included = dr["Included"].ToString();
                        objRoom.Excluded = dr["Excluded"].ToString();
                        objRoom.RefundableInfo = dr["RefundableInfo"].ToString();
                        objRoom.NonRefundableInfo = dr["NonRefundableInfo"].ToString();
                        objRoom.RoomTypeText = dr["RoomType"].ToString();
                        objRoom.Reservation = dr["Reservation"].ToString();
                        objRoom.InstantConfirmation = dr["InstantConfirmation"].ToString();

                        if (DateDiff <= 1)
                        {
                            objRoom.RoomPriceText = dr["RoomPriceText"].ToString();
                        }
                        else
                        {
                            string RoomPriceOverOneNight = dr["RoomPriceOverOneNight"].ToString();
                            RoomPriceOverOneNight = RoomPriceOverOneNight.Replace("#NightCount#", DateDiff.ToString());
                            objRoom.RoomPriceText = RoomPriceOverOneNight;
                        }
                        objRoom.Quantity = dr["Quantity"].ToString();
                        objRoom.Conditions = dr["Conditions"].ToString();

                        if (dr["RoomPhotoName"].ToString() != "" && dr["RoomPhotoName"].ToString() != null)
                        {
                            objRoom.RoomPhotoName = URL.EXTRANET_URLFULL + "Photo/Hotel/" + hotelId + "/" + dr["RoomPhotoName"].ToString();
                        }
                        else
                        {
                            objRoom.RoomPhotoName = "/Images/image_not_found-hotel-.jpg";
                        }

                        if (dr["PricePolicyTypeID"].ToString() == "1")
                        {
                            Home objCancel = new Home();
                            DataTable dtcancel = new DataTable();
                            string RefundableInfo = "";
                            dtcancel = objCancel.GetCancellationPolicy(hotelId, culture);
                            if (dtcancel.Rows.Count > 0)
                            {
                                foreach (DataRow drcancel in dtcancel.Rows)
                                {
                                    RefundableInfo = dr["RefundableInfo"].ToString();
                                    string RefundableInfo1 = RefundableInfo.Replace("#Days#", drcancel["RefundableDayCount"].ToString());
                                    string RefundableInfo2 = RefundableInfo1.Replace("#Penalty#", drcancel["PenaltyRateTypeName"].ToString());
                                    RefundableInfo2 = RefundableInfo2.Replace("\"", "");
                                    objRoom.RoomTypeInfo = RefundableInfo2;

                                    objRoom.PricePolicyTypeName = dr["RefundableText"].ToString();
                                }
                            }
                            else
                            {
                                objRoom.RoomTypeInfo = RefundableInfo;
                                objRoom.PricePolicyTypeName = dr["RefundableText"].ToString();
                            }

                        }
                        else if (dr["PricePolicyTypeID"].ToString() == "2")
                            objRoom.RoomTypeInfo = dr["NonRefundableInfo"].ToString();
                        else
                            objRoom.RoomTypeInfo = "";
                        objRoom.SingleRate = dr["SingleRate"].ToString();
                        objRoom.DoubleRate = dr["DoubleRate"].ToString();
                        objRoom.DailyRoomPrices = dr["DailyRoomPrices"].ToString();
                        objRoom.OriginalRoomPrice = dr["OriginalRoomPrice"].ToString();
                        objRoom.CurrencyID = dr["CurrencyID"].ToString();
                        objRoom.CurrencySymbol = dr["CurrencySymbol"].ToString();



                        if (currentCurrency != "" && (currentCurrency != dr["CurrencyCode"].ToString()))
                        {
                            if (CurrencyFromDB != dr["CurrencyCode"].ToString())
                            {
                                CurrencyFromDB = dr["CurrencyCode"].ToString();
                                string Priceconvert = CurrencyConvertion(CurrencyFromDB, currentCurrency);
                                string[] split = Priceconvert.Split(null);
                                string price = split[0];
                                CurrenySym = split[1];
                                ConveretedPrice = Convert.ToDouble(price);

                                double roomPrice = Convert.ToDouble(dr["RoomPrice"]);
                                double ConvertedRoomPrice = roomPrice * ConveretedPrice;
                                objRoom.ConvertedRoomPrice = Math.Round(ConvertedRoomPrice);
                                objRoom.NewCurrencySymbol = CurrenySym;

                                double roomPriceHistry = Convert.ToDouble(dr["RoomPriceHistory"]);
                                double ConvertedRoomPriceHistry = roomPriceHistry * ConveretedPrice;
                                if (roomPriceHistry > 0 && roomPriceHistry > roomPrice)
                                {

                                    objRoom.ConvertedRoomPriceHistry = Math.Round(ConvertedRoomPriceHistry);
                                    objRoom.PromotionPercentage = "% " + Convert.ToInt32((1 - ConvertedRoomPrice / ConvertedRoomPriceHistry) * 100) + "  " + Discount + "";
                                }
                                else
                                {
                                    objRoom.ConvertedRoomPriceHistry = 0;
                                    objRoom.PromotionPercentage = "";
                                }



                            }
                            else
                            {
                                double roomPrice = Convert.ToDouble(dr["RoomPrice"]);
                                double ConvertedRoomPrice = roomPrice * ConveretedPrice;
                                objRoom.ConvertedRoomPrice = Math.Round(ConvertedRoomPrice);
                                objRoom.NewCurrencySymbol = CurrenySym;

                                double roomPriceHistry = Convert.ToDouble(dr["RoomPriceHistory"]);
                                double ConvertedRoomPriceHistry = roomPriceHistry * ConveretedPrice;

                                if (roomPriceHistry > 0 && roomPriceHistry > roomPrice)
                                {
                                    objRoom.ConvertedRoomPriceHistry = Math.Round(ConvertedRoomPriceHistry);
                                    objRoom.PromotionPercentage = "% " + Convert.ToInt32((1 - ConvertedRoomPrice / ConvertedRoomPriceHistry) * 100) + "  " + Discount + "";
                                }
                                else
                                {
                                    objRoom.ConvertedRoomPriceHistry = 0;
                                    objRoom.PromotionPercentage = "";
                                }


                            }
                        }
                        else
                        {
                            objRoom.ConvertedRoomPrice = Convert.ToDouble(dr["RoomPrice"]);
                            objRoom.NewCurrencySymbol = dr["CurrencySymbol"].ToString();
                            if (Convert.ToDouble(dr["RoomPriceHistory"]) > 0 && Convert.ToDouble(dr["RoomPriceHistory"]) > Convert.ToDouble(dr["RoomPrice"]))
                            {
                                objRoom.ConvertedRoomPriceHistry = Convert.ToDouble(dr["RoomPriceHistory"]);
                                objRoom.PromotionPercentage = "% " + Convert.ToInt32((1 - Convert.ToDouble(dr["RoomPrice"]) / Convert.ToDouble(dr["RoomPriceHistory"])) * 100) + "  " + Discount + "";
                            }
                            else
                            {
                                objRoom.ConvertedRoomPriceHistry = 0;
                                objRoom.PromotionPercentage = "";
                            }
                        }
                        
                        Gethotels.Add(objRoom);
                    }
                    dataRowCount++;
                }
            }
            var s = from p in Gethotels
                    group p by p.ID into g
                    select new { room= g.ToList() };
            return Request.CreateResponse(HttpStatusCode.OK, s);
        }
       
        [HttpGet] 
        public HttpResponseMessage GetHotelPromotion(string culture, string hotelId, string dateFrom, string dateTo)
        {
              DateTime dt;
            DateTime dt1;
            DateTime dt2;

           
            if (dateFrom.Contains('/'))
            {
                try
                {

                    dt = DateTime.ParseExact(dateFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    dt1 = DateTime.ParseExact(dateTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    dt2 = DateTime.ParseExact(dateTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    dt1 = dt1.AddDays(-1);
                }
                catch
                {
                    dt = DateTime.ParseExact(dateFrom, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    dt1 = DateTime.ParseExact(dateTo, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    dt2 = DateTime.ParseExact(dateTo, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    dt1 = dt1.AddDays(-1);
                }
            }
            else
            {
                try
                {
                    dt = DateTime.ParseExact(dateFrom, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                    dt2 = DateTime.ParseExact(dateTo, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                    dt1 = DateTime.ParseExact(dateTo, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                    dt1 = dt1.AddDays(-1);
                }
                catch
                {
                    dt = DateTime.ParseExact(dateFrom, "MM-dd-yyyy", CultureInfo.InvariantCulture);
                    dt1 = DateTime.ParseExact(dateTo, "MM-dd-yyyy", CultureInfo.InvariantCulture);
                    dt2 = DateTime.ParseExact(dateTo, "MM-dd-yyyy", CultureInfo.InvariantCulture);
                    dt1 = dt1.AddDays(-1);
                }
            }
            string NowdateTim = DateTime.Now.ToString("yyyy/MM/dd");
            string Datefromm = dt.ToString("yyyy/MM/dd");
            string Datetoo = dt1.ToString("yyyy/MM/dd");
            DataTable HotelPromotion = homeService.GetHotelPromotionsDailyDiscountPercentage(culture, hotelId, NowdateTim, Datefromm, Datetoo);
            return Request.CreateResponse(HttpStatusCode.OK, HotelPromotion);
            
        }
          

        [HttpGet]
        public HttpResponseMessage GetHotelsLocationByHotelID(string culture, string hotelId)
        {
            DataTable dt = new DataTable();
            List<Home> objlist = new List<Home>();
            try
            {
                DataSet ds = homeService.PageLoadGetHotelInformation("ID", culture, Int32.MaxValue, 1, 1, hotelId);

                if (ds != null)
                {
                    dt = ds.Tables[1];
                }

                if (dt != null && dt.Rows.Count > 0)
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

                        objlist.Add(obj);
                    }

                    return Request.CreateResponse(HttpStatusCode.OK, objlist);
                }
                else
                    return Request.CreateResponse(HttpStatusCode.NotFound, "No Record(s) found.");
            }
            catch (Exception ex)
            {
                return LogError(ex);
            }
        }
        public List<Home> LocalGetHotelsLocationByHotelID(string culture, string hotelId)
        {
            DataTable dt = new DataTable();
            List<Home> objlist = new List<Home>();
            try
            {
                DataSet ds = homeService.PageLoadGetHotelInformation("ID", culture, Int32.MaxValue, 1, 1, hotelId);

                if (ds != null)
                {
                    dt = ds.Tables[1];
                }

                if (dt != null && dt.Rows.Count > 0)
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

                        objlist.Add(obj);
                    }

                    return objlist;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public HttpResponseMessage GetHotelSlideShowImage(string culture, string hotelId)
        {

            Home obj = new Home();
            DataTable HotelImages = new DataTable();
            List<HotelImageModel> listHotelImages = new List<HotelImageModel>();
            try
            {
                HotelImages = obj.GetSlideShowImage(culture, hotelId);
                if (HotelImages != null && HotelImages.Rows.Count > 0)
                {
                    foreach (DataRow dr in HotelImages.Rows)
                    {
                        HotelImageModel hotelImage = new HotelImageModel();
                        hotelImage.HotelImage = URL.EXTRANET_URLFULL + "Photo/Hotel/" + Convert.ToString(dr["HotelID"]) + "/" + Convert.ToString(dr["Photoname"]);
                        hotelImage.HotelImageId = Convert.ToInt32(dr["ID"]);
                        hotelImage.HotelId = Convert.ToInt32(dr["HotelID"]);
                        listHotelImages.Add(hotelImage);
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, listHotelImages);
                }
                else
                    return Request.CreateResponse(HttpStatusCode.NotFound, "No Record(s) found.");
            }
            catch (Exception ex)
            {
                return LogError(ex);
            }
        }

        public HttpResponseMessage GetReservationReviews(string culture, string hotelId)
        {
            List<HotelReservationReviewModel> listHotelReservationReview = new List<HotelReservationReviewModel>();
            int i = 1;
            DataTable dataTableHotelReview = new DataTable();
            try
            {
                dataTableHotelReview = homeService.GetReservationReviews(culture, hotelId);

                if (dataTableHotelReview != null && dataTableHotelReview.Rows.Count > 0)
                {

                    foreach (DataRow dr in dataTableHotelReview.Rows)
                    {
                        HotelReservationReviewModel hotelReservationReview = new HotelReservationReviewModel();
                        hotelReservationReview.ReviewScaleTypeId = Convert.ToInt32(dr["ReviewScaleTypeID11"]);
                        hotelReservationReview.ReviewScale = Convert.ToString(dr["ReviewScale"]);
                        float Count = Convert.ToInt32(dr["ReviewCount"]);
                        float Count11 = Convert.ToInt32(dr["Count" + i]);

                        if (Count != 0.0 && Count11 != 0.0)
                        {
                            if (Convert.ToInt32(dr["ReviewScaleTypeID11"]) == 5)
                            {
                                hotelReservationReview.ExcellantWidth = "";
                                string width1 = (Count11 / Count * 100).ToString();
                                hotelReservationReview.ExcellantWidth = width1 + '%';
                            }
                            else if (Convert.ToInt32(dr["ReviewScaleTypeID11"]) == 4)
                            {
                                hotelReservationReview.ExcellantWidth = "";
                                string width1 = (Count11 / Count * 100).ToString();
                                hotelReservationReview.ExcellantWidth = width1 + '%';
                            }
                            else if (Convert.ToInt32(dr["ReviewScaleTypeID11"]) == 3)
                            {
                                hotelReservationReview.ExcellantWidth = "";
                                string width1 = (Count11 / Count * 100).ToString();
                                hotelReservationReview.ExcellantWidth = width1 + '%';
                            }
                            else if (Convert.ToInt32(dr["ReviewScaleTypeID11"]) == 2)
                            {
                                hotelReservationReview.ExcellantWidth = "";
                                string width1 = (Count11 / Count * 100).ToString();
                                hotelReservationReview.ExcellantWidth = width1 + '%';
                            }
                            else if (Convert.ToInt32(dr["ReviewScaleTypeID11"]) == 1)
                            {
                                hotelReservationReview.ExcellantWidth = "";
                                string width1 = (Count11 / Count * 100).ToString();
                                hotelReservationReview.ExcellantWidth = width1 + '%';
                            }
                        }
                        else
                            hotelReservationReview.ExcellantWidth = "0" + "%";// Convert.ToString(0 + '%');

                        listHotelReservationReview.Add(hotelReservationReview);
                        i++;
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, listHotelReservationReview);
                }
                else
                    return Request.CreateResponse(HttpStatusCode.NotFound, "No Record(s) found.");
            }
            catch (Exception ex)
            {
                return LogError(ex);
            }
        }

        public HttpResponseMessage GetSummary(string culture, string hotelId)
        {

            List<HotelReservationReviewModel> listReservationReviewList = new List<HotelReservationReviewModel>();

            int i = 1;
            DataTable dataTableReservationReviewList = new DataTable();
            try
            {
                dataTableReservationReviewList = homeService.GetSummary(culture, hotelId);
                if (dataTableReservationReviewList != null && dataTableReservationReviewList.Rows.Count > 0)
                {

                    foreach (DataRow dr in dataTableReservationReviewList.Rows)
                    {
                        HotelReservationReviewModel hotelReservation = new HotelReservationReviewModel();
                        hotelReservation.ReviewScaleTypeId = Convert.ToInt32(dr["ID"]);
                        hotelReservation.ReviewScaleType = Convert.ToString(dr["ReviewScaleType"]);
                        int type = 1;
                        string reviewID = Convert.ToString(dr["ReviewTypeID" + i]);
                        if (!string.IsNullOrEmpty(reviewID))
                        {
                            if (!string.IsNullOrEmpty(Convert.ToString(dr["ReviewScaleType"])))
                                type = Convert.ToInt32(dr["ReviewTypeID" + i]);
                            if (type == 1)
                                hotelReservation.ExcellantWidth = "<i style='color: #ed8323'; class='fa fa-smile-o'></i><i class='fa fa-smile-o'></i><i class='fa fa-smile-o'></i><i class='fa fa-smile-o'></i><i class='fa fa-smile-o'></i>";
                            else if (type == 2)                    
                                hotelReservation.ExcellantWidth = "<i style='color: #ed8323'; class='fa fa-smile-o'></i><i style='color: #ed8323' class='fa fa-smile-o'></i><i class='fa fa-smile-o'></i><i class='fa fa-smile-o'></i><i class='fa fa-smile-o'></i>";
                            else if (type == 3)                    
                                hotelReservation.ExcellantWidth = "<i style='color: #ed8323'; class='fa fa-smile-o'></i><i style='color: #ed8323' class='fa fa-smile-o'></i><i style='color: #ed8323' class='fa fa-smile-o'></i><i class='fa fa-smile-o'></i><i class='fa fa-smile-o'></i>";
                            else if (type == 4)                    
                                hotelReservation.ExcellantWidth = "<i style='color: #ed8323'; class='fa fa-smile-o'></i><i style='color: #ed8323' class='fa fa-smile-o'></i><i style='color: #ed8323' class='fa fa-smile-o'></i><i style='color: #ed8323' class='fa fa-smile-o'></i><i class='fa fa-smile-o'></i>";
                            else if (type == 5)                    
                                hotelReservation.ExcellantWidth = "<i style='color: #ed8323'; class='fa fa-smile-o'></i><i style='color: #ed8323' class='fa fa-smile-o'></i><i style='color: #ed8323' class='fa fa-smile-o'></i><i style='color: #ed8323' class='fa fa-smile-o'></i><i style='color: #ed8323' class='fa fa-smile-o'></i>";
                        }
                        else
                            hotelReservation.ExcellantWidth = "<i class='fa fa-smile-o'></i><i class='fa fa-smile-o'></i><i class='fa fa-smile-o'></i><i class='fa fa-smile-o'></i><i class='fa fa-smile-o'></i>";
                        i++;
                        listReservationReviewList.Add(hotelReservation);
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, listReservationReviewList);
                }
                else
                    return Request.CreateResponse(HttpStatusCode.NotFound, "No record(s) found.");
            }
            catch (Exception ex)
            {
                return LogError(ex);
            }
        }

        public HttpResponseMessage GetAllReviews(string culture, string hotelId)
        {
            DataTable dataTableAllReviews = new DataTable();
            List<HotelReviewModel> listReview = new List<HotelReviewModel>();
            try
            {
                dataTableAllReviews = homeService.Getallreviews(culture, hotelId);
                string averegepoint1 = "";
                decimal avg = 0;
                int count = 1;
                decimal average;
                if (dataTableAllReviews != null && dataTableAllReviews.Rows.Count > 0)
                {

                    foreach (DataRow dr in dataTableAllReviews.Rows)
                    {
                        HotelReviewModel obj = new HotelReviewModel();
                        obj.Id = Convert.ToInt32(dr["ID"]);
                        decimal point = Convert.ToDecimal(dr["AveragePoint"]);
                        decimal DEBITAMT = Math.Round(point, 2);


                        obj.Point = Convert.ToString(dr["AveragePoint"]);

                        avg = avg + point;
                        average = avg / count;
                        obj.AveragePoint = Convert.ToString(average);
                        averegepoint1 = Convert.ToString(average);
                        obj.ReviewStatusName = Convert.ToString(dr["ReviewStatusName"]);
                        obj.Anonymous = Convert.ToString(dr["Anonymous"]);
                        obj.CreateDateTime = Convert.ToString(dr["CreateDateTime"]);
                        obj.OpDateTime = Convert.ToString(dr["OpDateTime"]);
                        obj.TravelerTypeName = Convert.ToString(dr["TravelerTypeName"]);

                        if (Convert.ToString(dr["Anonymous"]) == "False")
                            obj.UserFullName = Convert.ToString(dr["UserFullName"]);
                        else
                            obj.UserFullName = "XXXXXXXXX";

                        obj.CountryName = Convert.ToString(dr["CountryName"]);
                        obj.City = Convert.ToString(dr["City"]);
                        obj.TotalReviewCount = Convert.ToInt32(dr["totalReviewCount"]);
                        obj.UserPhoto = "/Images/Users/" + Convert.ToString(dr["Userphoto"]);
                        obj.ReviewPositive = Convert.ToString(dr["ReviewPositive"]);
                        obj.ReviewNegative = Convert.ToString(dr["Reviewnegative"]);
                        obj.Total = Convert.ToString(dr["total"]);
                        obj.Froms = Convert.ToString(dr["froms"]);
                        obj.Review = Convert.ToString(dr["review"]);
                        if (averegepoint1 != "")
                            obj.ReviewTypeScaleName = homeService.gettypescale(averegepoint1, culture);

                        count++;
                        listReview.Add(obj);
                    }

                }
                return Request.CreateResponse(HttpStatusCode.OK, listReview);

            }
            catch (Exception ex)
            {
                return LogError(ex);
            }
        }

        public HttpResponseMessage GetHotelBasicInfo(string culture, string hotelId)
        {
            DataTable dataTableHotelInfo = new DataTable();

            List<HotelModel> hotelInfoList = new List<HotelModel>();
            try
            {
                dataTableHotelInfo = homeService.GetHotelBasicInfo(culture, hotelId);
                if (dataTableHotelInfo.Rows.Count > 0)
                {
                    foreach (DataRow dr in dataTableHotelInfo.Rows)
                    {
                        HotelModel hotelInfo = new HotelModel();
                        hotelInfo.Id = Convert.ToInt32(dr["ID"]);

                        string Description = dr["Description"].ToString().Trim('"');
                        hotelInfo.Description = Description.Replace("\n", "<br/>");
                        hotelInfo.RoutingName = dr["RoutingName"].ToString();
                        hotelInfo.ClosestAirportName = dr["ClosestAirportName"].ToString();
                        hotelInfo.ClosestAirportDistance = dr["ClosestAirportDistance"].ToString();
                        if (dr["MainPhotoName"].ToString() != "" && dr["MainPhotoName"].ToString() != null)
                        {
                            hotelInfo.MainPhotoName = URL.EXTRANET_URLFULL + "Photo/Hotel/" + dr["ID"].ToString() + "/" + dr["MainPhotoName"].ToString();
                        }
                        else
                        {
                            hotelInfo.MainPhotoName = "/Images/image_not_found-hotel-.jpg";
                        }
                        hotelInfo.RegionID = dr["RegionID"].ToString();
                        double i = 0;
                        if (dr["AveragePoint"].ToString() != "")
                            i = Convert.ToDouble(dr["AveragePoint"]);

                        if (i == 10.00)
                            hotelInfo.HotelClass = "<a href=" + @"""#""" + "style=" + @"""background-color:#0BB3F9;margin-right:4px;""" + "class=" + @"""btn btn-circle1""" + "><i class=" + @"""fa fa-user""" + "></i></a>" + "<a href=" + @"""#""" + "style=" + @"""background-color:#0BB3F9;margin-right:4px""" + "class=" + @"""btn btn-circle1""" + "><i class=" + @"""fa fa-user""" + "></i></a>" + "<a href=" + @"""#""" + "style=" + @"""background-color:#0BB3F9;margin-right:4px""" + "class=" + @"""btn btn-circle1""" + "><i class=" + @"""fa fa-user""" + "></i></a>" + "<a href=" + @"""#""" + "style=" + @"""background-color:#0BB3F9;margin-right:4px""" + "class=" + @"""btn btn-circle1""" + "><i class=" + @"""fa fa-user""" + "></i></a>" + "<a href=" + @"""#""" + "style=" + @"""background-color:#0BB3F9""" + "class=" + @"""btn btn-circle1""" + "><i class=" + @"""fa fa-user""" + "></i></a>" + " 10.00" + " <span>" + dr["ScoreFrom"].ToString() + " " + dr["ReviewCount"].ToString() + " " + dr["Reviews"].ToString() + "</span>";
                        else if (i == 7.50)
                            hotelInfo.HotelClass = "<a href=" + @"""#""" + "style=" + @"""background-color:#0BB3F9;margin-right:4px""" + "class=" + @"""btn btn-circle1""" + "><i class=" + @"""fa fa-user""" + "></i></a>" + "<a href=" + @"""#""" + "style=" + @"""background-color:#0BB3F9;margin-right:4px""" + "class=" + @"""btn btn-circle1""" + "><i class=" + @"""fa fa-user""" + "></i></a>" + "<a href=" + @"""#""" + "style=" + @"""background-color:#0BB3F9;margin-right:4px""" + "class=" + @"""btn btn-circle1""" + "><i class=" + @"""fa fa-user""" + "></i></a>" + "<a href=" + @"""#""" + "style=" + @"""background-color:#0BB3F9;margin-right:4px""" + "class=" + @"""btn btn-circle1""" + "><i class=" + @"""fa fa-user""" + "></i></a>" + "<a href=" + @"""#""" + "class=" + @"""btn btn-circle1""" + "><i class=" + @"""fa fa-user""" + "></i></a>" + "";
                        else if (i == 5.00)
                            hotelInfo.HotelClass = "<a href=" + @"""#""" + "style=" + @"""background-color:#0BB3F9;margin-right:3px""" + "class=" + @"""btn btn-circle1""" + "><i class=" + @"""fa fa-user""" + "></i></a>" + "<a href=" + @"""#""" + "style=" + @"""background-color:#0BB3F9;margin-right:4px""" + "class=" + @"""btn btn-circle1""" + "><i class=" + @"""fa fa-user""" + "></i></a>" + "<a href=" + @"""#""" + "style=" + @"""background-color:#0BB3F9;margin-right:4px""" + "class=" + @"""btn btn-circle1""" + "><i class=" + @"""fa fa-user""" + "></i></a>" + "<a href=" + @"""#""" + "class=" + @"""btn btn-circle1""" + "><i class=" + @"""fa fa-user""" + "></i></a>" + "<a href=" + @"""#""" + "class=" + @"""btn btn-circle1""" + "><i class=" + @"""fa fa-user""" + "></i></a>" + " 5.00" + " <span>" + dr["ScoreFrom"].ToString() + " " + dr["ReviewCount"].ToString() + " " + dr["Reviews"].ToString() + "</span>";
                        else if (i == 2.50)
                            hotelInfo.HotelClass = "<a href=" + @"""#""" + "style=" + @"""background-color:#0BB3F9;margin-right:4px""" + "class=" + @"""btn btn-circle1""" + "><i class=" + @"""fa fa-user""" + "></i></a>" + "<a href=" + @"""#""" + "class=" + @"""btn btn-circle1""" + "><i class=" + @"""fa fa-user""" + "></i></a>" + "<a href=" + @"""#""" + "class=" + @"""btn btn-circle1""" + "><i class=" + @"""fa fa-user""" + "></i></a>" + "<a href=" + @"""#""" + "class=" + @"""btn btn-circle1""" + "><i class=" + @"""fa fa-user""" + "></i></a>" + "<a href=" + @"""#""" + "class=" + @"""btn btn-circle1""" + "><i class=" + @"""fa fa-user""" + "></i></a>" + " 2.50" + " <span>" + dr["ScoreFrom"].ToString() + " " + dr["ReviewCount"].ToString() + " " + dr["Reviews"].ToString() + "</span>";
                        else
                            hotelInfo.HotelClass = "";
                        hotelInfo.HotelClassValue = dr["HotelClass"].ToString();
                        hotelInfo.CityName = dr["CityName"].ToString();
                        hotelInfo.CountryName = dr["CountryName"].ToString();
                        hotelInfo.CityID = dr["CityID"].ToString();
                        hotelInfo.CountryID = dr["ID1"].ToString();
                        HotelModel HotelCount = GetCount(hotelInfo.CityID, hotelInfo.CountryID);
                        if (HotelCount != null)
                        {
                            hotelInfo.Properties = HotelCount.Properties;
                            hotelInfo.Properties1 = HotelCount.Properties1;
                        }
                        hotelInfo.HotelStar = dr["HotelStar"].ToString();
                        hotelInfo.Address = dr["Address"].ToString();
                        hotelInfo.CurrencyID = dr["CurrencyID"].ToString();
                        hotelInfo.CurrencySymbol = dr["CurrencySymbol"].ToString();
                        hotelInfo.ReviewCount = dr["ReviewCount"].ToString();
                        hotelInfo.CountryNameineng = dr["CountryName_en"].ToString();
                        hotelInfo.CityNavigateURL = dr["CityName_en"].ToString();

                        hotelInfo.ReviewEvaluationName = dr["ReviewEvaluationName"].ToString();
                        hotelInfo.AverageReviewPoint = dr["AverageReviewPoint"].ToString();
                        hotelInfo.RatingBasedOnReview = dr["RatingBasedOnReview"].ToString();
                        string RatingBasedOnReview = dr["RatingBasedOnReview"].ToString();
                        if (RatingBasedOnReview != "")
                        {
                            try
                            {
                                RatingBasedOnReview = RatingBasedOnReview.Replace("#ReviewCount#", dr["ReviewCount"].ToString());
                            }
                            catch
                            {
                                RatingBasedOnReview = "";
                            }

                        }
                        hotelInfo.RatingBasedOnReview = RatingBasedOnReview;

                        hotelInfo.WishListAdded = dr["WishListCount"].ToString() + " " + dr["WishListAdded"].ToString();


                        if ((dr["ReviewCount"]).ToString() == "0")
                            hotelInfo.HotelStatus = dr["New"].ToString();
                        else
                            hotelInfo.HotelStatus = dr["VeryGood"].ToString(); ;

                        hotelInfo.Superb = dr["Superb"].ToString();
                        hotelInfo.Hotel = dr["Hotel"].ToString();
                        hotelInfo.IsPreferred = dr["IsPreferred"].ToString();
                        hotelInfo.ScoreFrom = dr["ScoreFrom"].ToString();
                        hotelInfo.Reviews = dr["Reviews"].ToString();
                        hotelInfo.DescriptionText = dr["DescriptionText"].ToString();
                        hotelInfo.VeryGood = dr["VeryGood"].ToString();
                        hotelInfo.New = dr["New"].ToString();
                        hotelInfo.Policiesof = dr["Policiesof"].ToString();
                        hotelInfo.Facilitiesof = dr["Facilitiesof"].ToString();
                        hotelInfo.ShowMap = dr["ShowMap"].ToString();

                        hotelInfoList.Add(hotelInfo);
                    }

                }
                return Request.CreateResponse(HttpStatusCode.OK, hotelInfoList);
            }
            catch (Exception ex)
            {
                return LogError(ex);
            }
        }

        public HttpResponseMessage GetNearbyCities(string culture, string HotelId, string countryId)
        {
            List<HotelNearByCityModel> listHotelNearCity = new List<HotelNearByCityModel>();
            DataTable dataTableHotelNearCity = new DataTable();
            try
            {
                dataTableHotelNearCity = homeService.GetNearbyCities(culture, countryId, HotelId);
                if (dataTableHotelNearCity != null)
                {
                    if (dataTableHotelNearCity.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dataTableHotelNearCity.Rows)
                        {
                            HotelNearByCityModel hotelNearCity = new HotelNearByCityModel();
                            hotelNearCity.RegionName = dr["Region"].ToString();
                            hotelNearCity.CountryName = dr["CountryName"].ToString();
                            hotelNearCity.Code = dr["Code"].ToString();
                            hotelNearCity.RegionID = dr["ID"].ToString();
                            hotelNearCity.TopPhoto = "/Images/Region/" + dr["ID"].ToString() + ".jpg";
                            hotelNearCity.TopPhotoFlag = "/Images/" + dr["Code"].ToString() + ".png";
                            hotelNearCity.NavigateURL = "/Hotels_en/" + dr["CountryNameineng"].ToString() + "/" + dr["Regionnameineng"].ToString();
                            listHotelNearCity.Add(hotelNearCity);
                        }
                    }
                }
                return Request.CreateResponse(HttpStatusCode.OK, listHotelNearCity);
            }

            catch (Exception ex)
            {
                return LogError(ex);
            }
        }
        public HttpResponseMessage GetHotelSpecifications(string culture, string hotelId)
        {
            DataTable dataTableHotelSpecifications = new DataTable();
            List<HotelSpecificationsModel> listHotelSpecifications = new List<HotelSpecificationsModel>();
            dataTableHotelSpecifications = homeService.GetHotelSpecifications(culture, hotelId);
            try
            {
                if (dataTableHotelSpecifications != null && dataTableHotelSpecifications.Rows.Count > 0)
                {
                    foreach (DataRow dr in dataTableHotelSpecifications.Rows)
                    {
                        HotelSpecificationsModel obj = new HotelSpecificationsModel();
                        obj.HotelRoomID = dr["ID"].ToString();
                        obj.HotelID = dr["HotelID"].ToString();
                        obj.NameASCII = dr["Name_en"].ToString();
                        if (dr["RoomPhotoName"].ToString() != "" && dr["RoomPhotoName"].ToString() != null)
                            obj.HotelImage = URL.EXTRANET_URLFULL + "Photo/Hotel/" + dr["HotelID"].ToString() + "/" + dr["RoomPhotoName"].ToString();
                        else
                            obj.HotelImage = "/Images/image_not_found-hotel-.jpg";

                        obj.Name = dr["Name"].ToString();
                        listHotelSpecifications.Add(obj);
                    }
                }
                return Request.CreateResponse(HttpStatusCode.OK, listHotelSpecifications);
            }
            catch (Exception ex)
            {
                return LogError(ex);
            }
        }

        [HttpGet]
        public HttpResponseMessage UpdateHotelSearchHistory(string userId, string hotelId, string ipAddress)
        {

            string Id = string.Empty;

            try
            {
                if (userId == "0")
                    userId = string.Empty;
                Id = homeService.UpdateHotelSearchHistory(userId, hotelId, ipAddress);

                return Request.CreateResponse(HttpStatusCode.OK, Id);
            }
            catch (Exception ex)
            {
                return LogError(ex);
            }
        }
        public HttpResponseMessage GetTypeReview(string culture)
        {
            DataTable dataTableReviewType = new DataTable();
            List<HotelReviewTypeModel> objlist = new List<HotelReviewTypeModel>();
            try
            {
                dataTableReviewType = homeService.GetTypeReview(culture);

                if (dataTableReviewType != null && dataTableReviewType.Rows.Count > 0)
                {
                    foreach (DataRow dr in dataTableReviewType.Rows)
                    {
                        HotelReviewTypeModel hotelReviewType = new HotelReviewTypeModel();
                        hotelReviewType.ID = dr["ID"].ToString();
                        hotelReviewType.TypeReview = dr["Typereview"].ToString();
                        objlist.Add(hotelReviewType);
                    }
                }
                return Request.CreateResponse(HttpStatusCode.OK, objlist);
            }
            catch (Exception ex)
            {
                return LogError(ex);
            }
        }
        public HotelModel GetCount(string CityId, string CountryID)
        {
            DataSet ds = new DataSet();

            ds = homeService.GetCount(CityId, CountryID);
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            HotelModel hotelInfo = new HotelModel();
            if (ds != null)
            {
                dt = ds.Tables[0];
                dt1 = ds.Tables[1];

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        hotelInfo.Properties = dr["Properties"].ToString();
                    }

                }
                if (dt1.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt1.Rows)
                    {
                        hotelInfo.Properties1 = dr["Properties1"].ToString();
                    }

                }
            }
            return hotelInfo;

        }
        public HttpResponseMessage old_GetHotelMainregion(string CurrentCulture, string HotelID, string RegionID, string CountryID)
        {
            SqlConnection SQLCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Connection"].ConnectionString);
            DataTable Hotelregionsbyid = new DataTable();
            Home Hotelregions = new Home();
            Hotelregionsbyid = Hotelregions.GetHotelMainregionbyid(CurrentCulture, Convert.ToInt32(HotelID));
            RegionID = string.Empty;
            DataTable dt = new DataTable();


            List<HotelDistanceModel> list = new List<HotelDistanceModel>();
            dt = Hotelregions.GetHotelMainregion(CurrentCulture, CountryID, RegionID);
            try
            {
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];

                    Home drpObj = new Home();
                    drpObj.IDs = Convert.ToInt64(dr["ID"]);
                    drpObj.Name = dr["Name"].ToString();
                    string Latitude = dr["Latitude"].ToString();
                    string Longitude = dr["Longitude"].ToString();

                    DataTable regionDW = new DataTable();
                    if (Latitude != "" && Longitude != "" && Latitude != null && Longitude != null)
                    {
                        SQLCon.Open();
                        SqlCommand cmd = new SqlCommand("[TB_SP_GetRegionsInVicinity]", SQLCon);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CenterLatitude", Latitude);
                        cmd.Parameters.AddWithValue("@CenterLongitude", Longitude);
                        cmd.Parameters.AddWithValue("@Radius", 50000);
                        cmd.Parameters.AddWithValue("@Culture", CurrentCulture);
                        cmd.Parameters.AddWithValue("@OrderBy", "Name Asc");
                        cmd.Parameters.AddWithValue("@CountryID", CountryID);
                        SqlDataAdapter sda = new SqlDataAdapter(cmd);
                        sda.Fill(regionDW);

                        SQLCon.Close();
                    }

                    if (Latitude != "" && Longitude != "" && Latitude != null && Longitude != null)
                    {
                        regionDW.Columns.Add("Distance");
                        foreach (DataRow dr1 in regionDW.Rows)
                        {
                            if (dr1["Latitude"].ToString() != "" && dr1["Longitude"].ToString() != "")
                            {
                                double distance = GetDistanceBetweenCoordinates(Double.Parse(Latitude), Double.Parse(Longitude), Double.Parse(dr1["Latitude"].ToString()), Double.Parse(dr1["Longitude"].ToString()));
                                // regionDW
                                DataRow[] rows = regionDW.Select("ID = '" + dr1["ID"].ToString() + "'");
                                distance = Math.Round(distance, 1);
                                rows[0]["Distance"] = distance;
                                rows[0]["Name"] = dr1["Name"].ToString() + " - " + distance + " km";
                            }
                            else
                            {
                                DataRow[] rows = regionDW.Select("ID = '" + dr1["ID"].ToString() + "'");
                                rows[0]["Distance"] = 0;
                                rows[0]["Name"] = dr1["Name"].ToString();

                            }
                        }

                    }
                    if (Latitude != "" && Longitude != "" && Latitude != null && Longitude != null)
                    {

                        if (regionDW.Rows.Count > 0)
                        {
                            DataView dv = regionDW.DefaultView;
                            dv.Sort = "Distance";
                            regionDW = dv.ToTable();

                            foreach (DataRow hotelregion in Hotelregionsbyid.Rows)
                            {
                                foreach (DataRow dr2 in regionDW.Rows)
                                {
                                    if (Convert.ToInt64(dr2["ID"]) == Convert.ToInt64(hotelregion["RegionID"]))
                                    {
                                        HotelDistanceModel obj = new HotelDistanceModel();
                                        obj.IDs = Convert.ToInt64(dr2["ID"]);
                                        obj.Name = dr2["Name"].ToString();
                                        obj.Latitude = dr2["Latitude"].ToString();
                                        obj.Longitude = dr2["Longitude"].ToString();
                                        list.Add(obj);
                                    }
                                }
                            }

                        }
                    }
                }
                return Request.CreateResponse(HttpStatusCode.OK, list);
            }
            catch (Exception ex)
            {
                return LogError(ex);
            }
        }

        public HttpResponseMessage GetHotelMainregion(string CurrentCulture, string HotelID, string RegionID, string CountryID)
        {
            SqlConnection SQLCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Connection"].ConnectionString);
            DataTable Hotelregionsbyid = new DataTable();
            Home Hotelregions = new Home();
            Hotelregionsbyid = Hotelregions.GetHotelMainregionbyid(CurrentCulture, Convert.ToInt32(HotelID));
            RegionID = string.Empty;
            DataTable dt = new DataTable();

            List<HotelDistanceModel> listDw = new List<HotelDistanceModel>();
            List<HotelDistanceModel> list = new List<HotelDistanceModel>();
            dt = Hotelregions.GetHotelMainregion(CurrentCulture, CountryID, RegionID);
            try
            {
                

                //if (dt.Rows.Count > 0)
                //{
                //    foreach (DataRow dr in dt.Rows)
                //    {
                //        {
                //            Home drpObj = new Home();
                //            drpObj.IDs = Convert.ToInt64(dr["ID"]);
                //            drpObj.Name = dr["Name"].ToString();
                //            drpObj.Latitude = dr["Latitude"].ToString();
                //            drpObj.Longitude = dr["Longitude"].ToString();
                //              Latitude = dr["Latitude"].ToString();
                //              Longitude = dr["Longitude"].ToString();
                //            list.Add(drpObj);
                //        }
                //    }
                //}

                List<Home> lst=  LocalGetHotelsLocationByHotelID(CurrentCulture, HotelID);
                if (lst == null)
                    return null;

                string Latitude = lst[lst.Count-1].Latitude,
                  Longitude = lst[lst.Count - 1].Longitude;

                DataTable regionDW = new DataTable();
                if (Latitude != "" && Longitude != "" && Latitude != null && Longitude != null)
                {
                    SQLCon.Open();
                    SqlCommand cmd = new SqlCommand("[TB_SP_GetRegionsInVicinity]", SQLCon);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CenterLatitude", Latitude);
                    cmd.Parameters.AddWithValue("@CenterLongitude", Longitude);
                    cmd.Parameters.AddWithValue("@Radius", 50000);
                    cmd.Parameters.AddWithValue("@Culture", CurrentCulture);
                    cmd.Parameters.AddWithValue("@OrderBy", "Name Asc");
                    cmd.Parameters.AddWithValue("@CountryID", CountryID);
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(regionDW);

                    SQLCon.Close();
                }

                if (Latitude != "" && Longitude != "" && Latitude != null && Longitude != null)
                {
                    regionDW.Columns.Add("Distance");
                    foreach (DataRow dr in regionDW.Rows)
                    {
                        if (dr["Latitude"].ToString() != "" && dr["Longitude"].ToString() != "")
                        {
                            double distance = GetDistanceBetweenCoordinates(Double.Parse(Latitude), Double.Parse(Longitude), Double.Parse(dr["Latitude"].ToString()), Double.Parse(dr["Longitude"].ToString()));
                            // regionDW
                            DataRow[] rows = regionDW.Select("ID = '" + dr["ID"].ToString() + "'");
                            distance = Math.Round(distance, 1);
                            rows[0]["Distance"] = distance;
                            rows[0]["Name"] = dr["Name"].ToString() + " - " + distance + " km";
                        }
                        else
                        {
                            DataRow[] rows = regionDW.Select("ID = '" + dr["ID"].ToString() + "'");
                            rows[0]["Distance"] = 0;
                            rows[0]["Name"] = dr["Name"].ToString();

                        }
                    }

                }
                if (Latitude != "" && Longitude != "" && Latitude != null && Longitude != null)
                {

                    if (regionDW.Rows.Count > 0)
                    {
                        DataView dv = regionDW.DefaultView;
                        dv.Sort = "Distance";
                        regionDW = dv.ToTable();

                        foreach (DataRow hotelregion in Hotelregionsbyid.Rows)
                        {
                            foreach (DataRow dr in regionDW.Rows)
                            {
                                if (Convert.ToInt64(dr["ID"]) == Convert.ToInt64(hotelregion["RegionID"]))
                                {
                                    HotelDistanceModel drpObj = new HotelDistanceModel();
                                    drpObj.IDs = Convert.ToInt64(dr["ID"]);
                                    drpObj.Name = dr["Name"].ToString();
                                    drpObj.Latitude = dr["Latitude"].ToString();
                                    drpObj.Longitude = dr["Longitude"].ToString();
                                    listDw.Add(drpObj);
                                }
                            }
                        }
                        list = listDw;
                    }


                }
            }
            catch (Exception ex)
            {
                return LogError(ex);
            }
            return Request.CreateResponse(HttpStatusCode.OK, list);
        }
        
        public static double GetDistanceBetweenCoordinates(double Latitude1, double Longitude1, double Latitude2, double Longitude2)
        {
            double e = (3.1415926538
                        * (Latitude1 / 180));
            double f = (3.1415926538
                        * (Longitude1 / 180));
            double g = (3.1415926538
                        * (Latitude2 / 180));
            double h = (3.1415926538
                        * (Longitude2 / 180));
            double i = ((Math.Cos(e)
                        * (Math.Cos(g)
                        * (Math.Cos(f) * Math.Cos(h))))
                        + ((Math.Cos(e)
                        * (Math.Sin(f)
                        * (Math.Cos(g) * Math.Sin(h))))
                        + (Math.Sin(e) * Math.Sin(g))));
            double j = Math.Acos(i);
            double k = (6371 * j);
            return k;
        }
        public HttpResponseMessage GetRoomAvailabilityDetails(string culture, string hotelId, string currentCurrency)
        {

            DataSet HotelRoomSet = new DataSet();
            DataTable HotelRooms = new DataTable();
            DataTable HotelRoomsConditions = new DataTable();
            DataTable HotelRoomCard = new DataTable();
            DataTable CancelPolicyWarning = new DataTable();
            DataTable HotelCheckInDetails = new DataTable();
            HotelFacilityModel hotelfacility = new HotelFacilityModel();
            string HotelCreditCard = "";
            string CurrencyFromDB = "";
            string CurrenySym = "";
            string CancelPolicy = "";
            double ConveretedPrice = 0;


            HotelRoomSet = homeService.GetRoomAvailabilityDetails(culture, hotelId);

            string Paid = homeService.GetTextMessagesAsString(culture, "Charged");
            CancelPolicy = homeService.GetTextMessagesAsString(culture, "CancelPolicy");
            HotelCreditCard = homeService.GetTextMessagesAsString(culture, "HotelCreditCard");

            if (HotelRoomSet != null)
            {
                HotelRooms = HotelRoomSet.Tables[0];
                HotelRoomsConditions = HotelRoomSet.Tables[1];
                HotelRoomCard = HotelRoomSet.Tables[2];
                CancelPolicyWarning = HotelRoomSet.Tables[3];
                HotelCheckInDetails = HotelRoomSet.Tables[4]; 
            }
            if (HotelRooms.Rows.Count > 0)
            {
                List<HotelRoomAvailabilityModel> hoteRoom = new List<HotelRoomAvailabilityModel>();
                foreach (DataRow dr in HotelRooms.Rows)
                {
                    HotelRoomAvailabilityModel objRoom = new HotelRoomAvailabilityModel();
                    CancelPolicy = dr["CancelPolicy"].ToString();
                    HotelCreditCard = dr["HotelCreditCard"].ToString();
                    objRoom.RoomDataTableType = "RoomDetails";
                    objRoom.HotelID = dr["HotelID"].ToString();
                    objRoom.HotelRoomID = dr["ID"].ToString();
                    objRoom.HotelRoomDescription = dr["Description"].ToString();
                    objRoom.HotelTypeName = dr["HotelTypeName"].ToString();
                    objRoom.SmokingTypeName = dr["SmokingTypeName"].ToString();
                    objRoom.RoomCount = dr["RoomCount"].ToString();
                    if (dr["MaxPeopleCount"].ToString() == "1")
                    {
                        objRoom.MaxPeopleCount = "<span class=" + @"""glyphicon glyphicon-user""" + "></span>";
                    }
                    else if (dr["MaxPeopleCount"].ToString() == "2")
                    {
                        objRoom.MaxPeopleCount = "<span class=" + @"""glyphicon glyphicon-user""" + "></span>" + "<span class=" + @"""glyphicon glyphicon-user""" + "></span>";
                    }
                    else if (dr["MaxPeopleCount"].ToString() == "3")
                    {
                        objRoom.MaxPeopleCount = "<span class=" + @"""glyphicon glyphicon-user""" + "></span>" + "<span class=" + @"""glyphicon glyphicon-user""" + "></span>" + "<span class=" + @"""glyphicon glyphicon-user""" + "></span>";
                    }
                    else if (dr["MaxPeopleCount"].ToString() == "4")
                    {
                        objRoom.MaxPeopleCount = "<span class=" + @"""glyphicon glyphicon-user""" + "></span>" + "<span class=" + @"""glyphicon glyphicon-user""" + "></span>" + "<span class=" + @"""glyphicon glyphicon-user""" + "></span>" + "<span class=" + @"""glyphicon glyphicon-user""" + "></span>";
                    }
                    else
                    {
                        objRoom.MaxPeopleCount = "";
                    }
                    objRoom.ExtraBedCount = dr["ExtraBedCount"].ToString();
                    objRoom.Checkin = dr["Checkin"].ToString();
                    objRoom.Checkout = dr["Checkout"].ToString();
                    objRoom.RoomTypeText = dr["RoomTypeText"].ToString();
                    objRoom.RoomPriceText = dr["RoomPriceText"].ToString();
                    objRoom.RoomMaxPeopleCount = dr["RoomMaxPeopleCount"].ToString();
                    objRoom.Quantity = dr["Quantity"].ToString();
                    objRoom.FREEcancellation = dr["FREEcancellation"].ToString();
                    objRoom.RoomFacilities = dr["RoomFacilities"].ToString();
                    objRoom.CheckinStart = "";
                    objRoom.CheckoutStart = "";
                    if (dr["CheckinStart"].ToString() != "")
                    {
                        string Fromtime = dr["UntilTime"].ToString();
                        string Checkin = Fromtime.Replace("#Time#", dr["CheckinStart"].ToString());
                        objRoom.CheckinStart = Checkin;
                    }
                    if (dr["CheckinEnd"].ToString() != "")
                    {
                        string Fromtime = dr["FromTime"].ToString();
                        string Checkin = Fromtime.Replace("#Time#", dr["CheckinEnd"].ToString());
                        objRoom.CheckinStart = Checkin;
                    }
                    if (dr["CheckinStart"].ToString() != "" && dr["CheckinEnd"].ToString() != "")
                    {
                        string Fromtime = dr["UntilTime"].ToString();
                        string Checkin1 = Fromtime.Replace("#Time#", dr["CheckinStart"].ToString());
                        string Fromtime1 = dr["FromTime"].ToString();
                        string Checkin = Fromtime1.Replace("#Time#", dr["CheckinEnd"].ToString());
                        objRoom.CheckinStart = Checkin + " - " + Checkin1;
                    }

                    if (dr["CheckoutStart"].ToString() != "")
                    {
                        string Fromtime = dr["UntilTime"].ToString();
                        string Checkout = Fromtime.Replace("#Time#", dr["CheckoutStart"].ToString());
                        objRoom.CheckoutStart = Checkout;
                    }
                    if (dr["CheckoutEnd"].ToString() != "")
                    {
                        string Fromtime = dr["FromTime"].ToString();
                        string Checkout = Fromtime.Replace("#Time#", dr["CheckoutEnd"].ToString());
                        objRoom.CheckoutStart = Checkout;
                    }

                    if (dr["CheckoutStart"].ToString() != "" && dr["CheckoutEnd"].ToString() != "")
                    {
                        string Fromtime = dr["UntilTime"].ToString();
                        string Checkin1 = Fromtime.Replace("#Time#", dr["CheckoutStart"].ToString());
                        string Fromtime1 = dr["FromTime"].ToString();
                        string Checkin = Fromtime1.Replace("#Time#", dr["CheckoutEnd"].ToString());
                        objRoom.CheckoutStart = Checkin + " - " + Checkin1;
                    }

                    hoteRoom.Add(objRoom);
                }

                hotelfacility.HotelRoomAvailability = hoteRoom;
            }
            //Below only required when no room available, it shows checkin chekout policy details to front ends
            else if (HotelCheckInDetails.Rows.Count > 0)
            {
                List<HotelRoomAvailabilityModel> hoteRoom = new List<HotelRoomAvailabilityModel>();
                foreach (DataRow dr in HotelCheckInDetails.Rows)
                {
                    HotelRoomAvailabilityModel objRoom = new HotelRoomAvailabilityModel();
                    CancelPolicy = dr["CancelPolicy"].ToString();
                    HotelCreditCard = dr["HotelCreditCard"].ToString();
                    objRoom.RoomDataTableType = "RoomDetails";
                    objRoom.HotelID = dr["HotelID"].ToString();
                    objRoom.MaxPeopleCount = "";
                    objRoom.Checkin = dr["Checkin"].ToString();
                    objRoom.Checkout = dr["Checkout"].ToString();
                    objRoom.RoomTypeText = dr["RoomTypeText"].ToString();
                    objRoom.RoomPriceText = dr["RoomPriceText"].ToString();
                    objRoom.RoomMaxPeopleCount = dr["RoomMaxPeopleCount"].ToString();
                    objRoom.Quantity = dr["Quantity"].ToString();
                    objRoom.FREEcancellation = dr["FREEcancellation"].ToString();
                    objRoom.RoomFacilities = dr["RoomFacilities"].ToString();
                    objRoom.CheckinStart = "";
                    objRoom.CheckoutStart = "";
                    if (dr["CheckinStart"].ToString() != "")
                    {
                        string Fromtime = dr["UntilTime"].ToString();
                        string Checkin = Fromtime.Replace("#Time#", dr["CheckinStart"].ToString());
                        objRoom.CheckinStart = Checkin;
                    }
                    if (dr["CheckinEnd"].ToString() != "")
                    {
                        string Fromtime = dr["FromTime"].ToString();
                        string Checkin = Fromtime.Replace("#Time#", dr["CheckinEnd"].ToString());
                        objRoom.CheckinStart = Checkin;
                    }
                    if (dr["CheckinStart"].ToString() != "" && dr["CheckinEnd"].ToString() != "")
                    {
                        string Fromtime = dr["UntilTime"].ToString();
                        string Checkin1 = Fromtime.Replace("#Time#", dr["CheckinStart"].ToString());
                        string Fromtime1 = dr["FromTime"].ToString();
                        string Checkin = Fromtime1.Replace("#Time#", dr["CheckinEnd"].ToString());
                        objRoom.CheckinStart = Checkin + " - " + Checkin1;
                    }

                    if (dr["CheckoutStart"].ToString() != "")
                    {
                        string Fromtime = dr["UntilTime"].ToString();
                        string Checkout = Fromtime.Replace("#Time#", dr["CheckoutStart"].ToString());
                        objRoom.CheckoutStart = Checkout;
                    }
                    if (dr["CheckoutEnd"].ToString() != "")
                    {
                        string Fromtime = dr["FromTime"].ToString();
                        string Checkout = Fromtime.Replace("#Time#", dr["CheckoutEnd"].ToString());
                        objRoom.CheckoutStart = Checkout;
                    }

                    if (dr["CheckoutStart"].ToString() != "" && dr["CheckoutEnd"].ToString() != "")
                    {
                        string Fromtime = dr["UntilTime"].ToString();
                        string Checkin1 = Fromtime.Replace("#Time#", dr["CheckoutStart"].ToString());
                        string Fromtime1 = dr["FromTime"].ToString();
                        string Checkin = Fromtime1.Replace("#Time#", dr["CheckoutEnd"].ToString());
                        objRoom.CheckoutStart = Checkin + " - " + Checkin1;
                    }

                    hoteRoom.Add(objRoom);
                }

                hotelfacility.HotelRoomAvailability = hoteRoom;
            }

            if (HotelRoomsConditions.Rows.Count > 0)
            {
                List<HotelRoomConditionModel> roomcondition = new List<HotelRoomConditionModel>();
                foreach (DataRow dr in HotelRoomsConditions.Rows)
                {
                    HotelRoomConditionModel objRoom = new HotelRoomConditionModel();
                    double count = 0;
                    double MinRoomPrice = 0;

                    //double count = gethotelreviewcount.GetHotelsreviewcount(HotelID);
                    //double MinRoomPrice = gethotelreviewcount.GetMinRoomPriceWithoutDate(HotelID);

                    if (dr["Charge"].ToString() != "")
                    {
                        MinRoomPrice = Convert.ToDouble(dr["Charge"]);
                    }
                    objRoom.RoomPrice = MinRoomPrice.ToString();
                    if (currentCurrency != "" && (currentCurrency != dr["CurrencyCode"].ToString()))
                    {
                        if (CurrencyFromDB != dr["CurrencyCode"].ToString())
                        {
                            CurrencyFromDB = dr["CurrencyCode"].ToString();
                            string Priceconvert = CurrencyConvertion(CurrencyFromDB, currentCurrency);
                            string[] split = Priceconvert.Split(null);
                            string price = split[0];
                            CurrenySym = split[1];
                            ConveretedPrice = Convert.ToDouble(price);

                            double roomPrice = MinRoomPrice;

                            double ConvertedRoomPrice = roomPrice * ConveretedPrice;
                            objRoom.ConvertedRoomPrice = Math.Round(ConvertedRoomPrice);
                            objRoom.NewCurrencySymbol = CurrenySym;

                        }
                        else
                        {
                            double roomPrice = MinRoomPrice;
                            double ConvertedRoomPrice = roomPrice * ConveretedPrice;
                            objRoom.ConvertedRoomPrice = Math.Round(ConvertedRoomPrice);
                            objRoom.NewCurrencySymbol = CurrenySym;
                        }
                    }
                    else
                    {
                        objRoom.ConvertedRoomPrice = MinRoomPrice;
                        objRoom.NewCurrencySymbol = dr["CurrencySymbol"].ToString();
                    }
                    objRoom.RoomDataTableType = "RoomConditions";
                    //objRoom.AttributeName = dr["AttributeName"].ToString();
                    //objRoom.AttributeName = dr["Charged"].ToString();

                    if (dr["Charged"].ToString() == "True" && dr["AttributeHeaderID"].ToString() == "1")
                    {
                        objRoom.AttributeName = dr["AttributeName"].ToString() + " (" + Paid + ")";
                    }
                    else
                    {
                        objRoom.AttributeName = dr["AttributeName"].ToString();
                    }
                    objRoom.icons = "/Images/Icons/" + dr["Icons"].ToString();

                    objRoom.AttributeTypeID = dr["AttributeTypeID"].ToString();
                    objRoom.AttributeHeaderID = dr["AttributeHeaderID"].ToString();
                    objRoom.AttributeHeaderName = dr["AttributeHeaderName"].ToString();
                    objRoom.AttributeID = dr["AttributeID"].ToString();
                    objRoom.UnitID = dr["UnitID"].ToString();
                    objRoom.UnitName = dr["UnitName"].ToString();
                    objRoom.HotelUnitID = dr["HotelUnitID"].ToString();
                    objRoom.HotelUnitName = dr["HotelUnitName"].ToString();
                    objRoom.UnitValue = dr["UnitValue"].ToString();
                    // objRoom.Charge = dr["Charge"].ToString();
                    objRoom.CurrencyID = dr["CurrencyID"].ToString();
                    objRoom.CurrencyCode = dr["CurrencyCode"].ToString();
                    //objRoom.CurrencyName = dr["CurrencyName"].ToString();
                    roomcondition.Add(objRoom);
                }
                List<HotelRoomConditionModelGroup> results = (from p in roomcondition
                                                              group p by p.AttributeHeaderName into g
                                                              select new HotelRoomConditionModelGroup()
                                                              {
                                                                  AttributeHeaderID = g.Key,
                                                                  HotelRoomCondition = g.ToList()
                                                              }).ToList();
                hotelfacility.HotelRoomCondition = results;
            }

            if (HotelRoomCard.Rows.Count > 0)
            {
                List<HotelCardModel> card = new List<HotelCardModel>();
                string Cardtypes = "";
                List<HotelCreditCards> CreditCardImage = new List<HotelCreditCards>();
                foreach (DataRow dr in HotelRoomCard.Rows)
                {
                    if (Cardtypes != "")
                    {
                        Cardtypes = Cardtypes + "," + dr["CreditCardTypeName"].ToString();
                    }
                    else
                    {
                        Cardtypes = dr["CreditCardTypeName"].ToString();
                    }
                    HotelCreditCards CardIMg = new HotelCreditCards();
                    CardIMg.CardImage = dr["CreditCardTypeCode"].ToString() + ".png";
                    CardIMg.CardName = dr["CreditCardTypeName"].ToString();
                    CreditCardImage.Add(CardIMg);
                }
                HotelCardModel objRoom = new HotelCardModel();
                objRoom.HotelCreditCardImage = CreditCardImage;
                objRoom.HotelCreditCard = HotelCreditCard;
                objRoom.RoomDataTableType = "RoomCarddetails";
                objRoom.CreditCardTypeName = Cardtypes;
                card.Add(objRoom);
                hotelfacility.HotelCard = card;
            }

            if (CancelPolicyWarning.Rows.Count > 0)
            {
                List<HotelCancellationModel> cancel = new List<HotelCancellationModel>();
                string HotelCancelPolicyWarning = "";

                foreach (DataRow dr in CancelPolicyWarning.Rows)
                {

                    HotelCancelPolicyWarning = dr["CancelPolicyWarning"].ToString();

                }
                HotelCancellationModel objcancel = new HotelCancellationModel();
                objcancel.RoomDataTableType = "Cancellation";

                objcancel.HotelCancelPolicy = CancelPolicy;
                objcancel.CancelPolicyWarning = HotelCancelPolicyWarning;
                cancel.Add(objcancel);
                hotelfacility.HotelCancllaton = cancel;
            }

            return Request.CreateResponse(HttpStatusCode.OK, hotelfacility);
        }
        public string CurrencyConvertion(string CurrencyFrom, string CurrencyTo)
        {

            WebClient web = new WebClient();
            string url = string.Format("http://api.fixer.io/latest");
            string response = web.DownloadString(url);
            Regex regex = new Regex(@"(,)");
            string[] arrDigits = regex.Split(response);
            string currency = "";
            // string currency1 = "";
            string Currencysymbol = "";
            string Currencysymbol1 = "";
            string res = "";
            if (CurrencyTo != "EUR")
            {
                for (int i = 1; i <= arrDigits.Length; i++)
                {
                    if (arrDigits[i].Contains(CurrencyTo))
                    {

                        string gfdfgd = arrDigits[i];
                        Regex regex1 = new Regex(@"(:)");
                        string[] currencyarr = regex1.Split(gfdfgd);
                        Currencysymbol = currencyarr[0].ToString();
                        currency = currencyarr[2].ToString();
                        string[] currency2 = Currencysymbol.Split('"');
                        Currencysymbol1 = currency2[1].ToString();
                        // currency1 = currency2[0].ToString();
                        res = currency + " " + Currencysymbol1;
                        break;
                    }

                }
            }
            else
                res = "76.51" + " " + "EUR";

            try
            {
                Home newHomeObj = new Home();
                string[] split = res.Split(null);
                string price = split[0];
                string Symbol = newHomeObj.GetCurrencySymbolBYCode(split[1]);
                res = price + " " + Symbol;

            }
            catch
            {

            }
            return res;
        }
    }
}
