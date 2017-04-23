using Business;
using GBSHotels.API.Helper;
using GBSHotels.API.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;

namespace GBSHotels.API.Controllers
{
    public class BookingController : BaseApiController
    {
        private Home homeService;
        public string MailServer { get; set; }
        public string MailUsername { get; set; }
        public string MailPassword { get; set; }
        BizContext BizContext = new BizContext();
        public BookingController()
        {
            this.MailServer = System.Configuration.ConfigurationManager.AppSettings["SMTPServerName"];
            this.MailUsername = System.Configuration.ConfigurationManager.AppSettings["SMTPUsername"];
            this.MailPassword = System.Configuration.ConfigurationManager.AppSettings["SMTPPassword"];
            homeService = new Home();
        }

        //public List<Home> GetHotelTax(string culture, string hotelId, string CurrentCurrency, string checkDatefrom, string checkDateto, out double ConvertedRoomPrice, out double totalvalue, out double TotalValueInHotelCurrency)
        public List<Home> GetHotelTax(string culture, string hotelId, string CurrentCurrency, string checkDatefrom, string checkDateto, int Session_Personcount, double Session_totalamount, double Session_totalamountHotelCur, out double total, out double totalInHotelCurrency)
        {

            List<Home> list = new List<Home>();
            DataTable Tax = new DataTable();

            //totalvalue = 0.0;
            ////double TotalValueInHotelCurrency = 0;
            //TotalValueInHotelCurrency = 0;

            Double totalvalue = 0;
            double TotalValueInHotelCurrency = 0;

            string currentCurrency = "";
            string CurrencyFromDB = "";
            string CurrenySym = "";
            double ConveretedPrice = 0;
            DateTime dt01 = DateTime.ParseExact(checkDatefrom, "dd-MMM-yyyy", CultureInfo.InvariantCulture);
            DateTime dt02 = DateTime.ParseExact(checkDateto, "dd-MMM-yyyy", CultureInfo.InvariantCulture);
            String Daydiff = (dt02 - dt01).TotalDays.ToString();
            String Hourdiff = (dt02 - dt01).TotalHours.ToString();
            String Mindiff = (dt02 - dt01).TotalMinutes.ToString();
            try
            {
                Tax = homeService.GetHoteltax(culture, hotelId);
                if (Tax != null && Tax.Rows.Count > 0)
                {

                    foreach (DataRow dr in Tax.Rows)
                    {
                        Home obj = new Home();
                        if (dr["UnitID"].ToString() != "" && dr["Charge"].ToString() != "" && dr["AttributeHeaderID"].ToString() == "11")
                        {
                            obj.AttributeID = dr["AttributeID"].ToString();

                            Double Charges = Convert.ToDouble(dr["Charge"]);

                            obj.CurrencySymbol = dr["Symbol"].ToString();
                            string UnitIDs = dr["UnitID"].ToString();
                            if (UnitIDs == "4")
                            {
                                obj.attributeCharge = (Convert.ToDouble(Mindiff)) * (Convert.ToDouble(dr["Charge"]));
                            }
                            else if (UnitIDs == "5")
                            {
                                obj.attributeCharge = (Convert.ToDouble(Hourdiff)) * (Convert.ToDouble(dr["Charge"]));
                            }
                            else if (UnitIDs == "6" || UnitIDs == "9")
                            {
                                obj.attributeCharge = (Convert.ToDouble(Daydiff)) * (Convert.ToDouble(dr["Charge"]));
                            }
                            else if (UnitIDs == "10" || UnitIDs == "12")
                            {
                                obj.attributeCharge = (Convert.ToDouble(Daydiff) * ((Convert.ToDouble(Session_Personcount)) * (Convert.ToDouble(dr["Charge"]))));
                            }
                            else if (UnitIDs == "8" || UnitIDs == "11")
                            {
                                obj.attributeCharge = (Convert.ToDouble(Session_Personcount)) * (Convert.ToDouble(dr["Charge"]));
                            }
                            else
                            {
                                obj.attributeCharge = (Convert.ToDouble(dr["Charge"]));
                            }
                            //obj.UnitID = dr["UnitID"].ToString();
                            obj.UnitName = dr["Unit"].ToString();
                            obj.AttributeHeaderName = dr["HeaderName"].ToString();
                            double roomPrice = Convert.ToDouble(obj.attributeCharge);

                            TotalValueInHotelCurrency = TotalValueInHotelCurrency + Convert.ToDouble(obj.attributeCharge);

                            //comment: Saved to save Total in Hotel Currency
                            //obj.ValueInHotelCurrency = Convert.ToDouble(obj.attributeCharge);
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
                                    double ConvertedRoomPrice = roomPrice * ConveretedPrice;
                                    obj.attributeCharge = Math.Round(ConvertedRoomPrice);
                                    //objMember.NewCurrencySymbol = CurrenySym;

                                }
                                else
                                {

                                    double ConvertedRoomPrice = roomPrice * ConveretedPrice;
                                    obj.attributeCharge = Math.Round(ConvertedRoomPrice);
                                }
                            }

                            roomPrice = Convert.ToDouble(obj.attributeCharge);
                            // totalvalue = (totalvalue) + Convert.ToDouble(dr["Charge"]); 
                            totalvalue = (totalvalue) + roomPrice;
                            list.Add(obj);
                        }

                    }
                }

                var roomtotalamoun = Session_totalamount;
                total = totalvalue + Convert.ToDouble(roomtotalamoun);
                var PriceinHotelCurrency = Session_totalamountHotelCur;
                totalInHotelCurrency = TotalValueInHotelCurrency + Convert.ToDouble(PriceinHotelCurrency);

                return list;
            }
            catch (Exception ex)
            {
                total = 0;
                totalInHotelCurrency = 0;
                return null;
            }
        }

        [HttpPost]
        public HttpResponseMessage StoreHotelRoomInfo(StoreBookingInfo storeBookingInfo)
        {
            string culture = storeBookingInfo.culture;
            string checkDatefrom = storeBookingInfo.checkDatefrom;
            string checkDateto = storeBookingInfo.checkDateto;
            string hotelCity = storeBookingInfo.hotelCity;
            string hotelId = storeBookingInfo.hotelId;
            string address = storeBookingInfo.address;
            string mainPhotoName = storeBookingInfo.mainPhotoName;
            string hotelClass = storeBookingInfo.hotelClass;
            string hotelname = storeBookingInfo.hotelname;
            string roomId = storeBookingInfo.roomId;
            string uniqueId = storeBookingInfo.uniqueId;
            string accommodationTypeId = storeBookingInfo.accommodationTypeId;
            string accommodationTypeName = storeBookingInfo.accommodationTypeName;
            string accommodationTypeDescription = storeBookingInfo.accommodationTypeDescription;
            string pricePolicyTypeId = storeBookingInfo.pricePolicyTypeId;
            string pricePolicyTypeName = storeBookingInfo.pricePolicyTypeName;
            string singleRate = storeBookingInfo.singleRate;
            string doubleRate = storeBookingInfo.doubleRate;
            string dailyRoomPrices = storeBookingInfo.dailyRoomPrices;
            string originalRoomPrice = storeBookingInfo.originalRoomPrice;
            string currencyId = storeBookingInfo.currencyId;
            string currencySymbol = storeBookingInfo.currencySymbol;
            string roomCount = storeBookingInfo.roomCount;
            string roomMaxPeopleCount = storeBookingInfo.MaxPeopleCount;
            string roomPriceVal = storeBookingInfo.roomPriceVal;
            string currencyCodeval = storeBookingInfo.currencyCodeval;
            string hiddenRoomTypeval = storeBookingInfo.hiddenRoomTypeval;
            string hiddenPriceTypeval = storeBookingInfo.hiddenPriceTypeval;
            int creditCardNotRequiredValue = storeBookingInfo.creditCardNotRequiredValue;
            string currentCurrency = storeBookingInfo.currentCurrency;

            DateTime dt01 = DateTime.ParseExact(checkDatefrom, "dd-MMM-yyyy", CultureInfo.InvariantCulture);
            DateTime dt02 = DateTime.ParseExact(checkDateto, "dd-MMM-yyyy", CultureInfo.InvariantCulture);
            String Daydiff = (dt02 - dt01).TotalDays.ToString();
            String Hourdiff = (dt02 - dt01).TotalHours.ToString();
            String Mindiff = (dt02 - dt01).TotalMinutes.ToString();



            DataTable dt = new DataTable();
            List<Home> objlist = new List<Home>();
            Home ObjModel = new Home();
            try
            {
                dt = ObjModel.GetRequiredMessageCode(culture);

                decimal price = 0;

                string CurrencyFromDB = "";
                string CurrenySym = "";
                double ConveretedPrice = 0, Session_totalamount = 0.0;//set the object instead of session 
                int Session_Personcount = 0;
                double Session_totalamountHotelCur = 0.0;

                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {

                            if (roomId.Contains(','))
                            {

                                string[] RoomID1 = roomId.Split(',');
                                string[] UniqueID1 = uniqueId.Split(',');
                                string[] AccommodationTypeID1 = accommodationTypeId.Split(',');
                                string[] AccommodationTypeName1 = accommodationTypeName.Split(',');
                                string[] AccommodationTypeDescription1 = accommodationTypeDescription.Split(',');
                                string[] PricePolicyTypeID1 = pricePolicyTypeId.Split(',');
                                string[] PricePolicyTypeName1 = pricePolicyTypeName.Split(',');
                                string[] SingleRate1 = singleRate.Split(',');
                                string[] DoubleRate1 = doubleRate.Split(',');
                                string[] DailyRoomPrices1 = dailyRoomPrices.Split(',');
                                string[] OriginalRoomPrice1 = originalRoomPrice.Split(',');
                                string[] CurrencyID1 = currencyId.Split(',');
                                string[] CurrencySymbol1 = currencySymbol.Split(',');
                                string[] RoomCount1 = roomCount.Split(',');
                                string[] RoomMaxPeopleCount1 = roomMaxPeopleCount.Split(',');
                                string[] RoomPriceVal1 = roomPriceVal.Split(',');
                                string[] CurrencyCodeval1 = currencyCodeval.Split(',');
                                string[] hiddenRoomTypeval1 = hiddenRoomTypeval.Split(',');
                                string[] hiddenPriceTypeval1 = hiddenPriceTypeval.Split(',');


                                for (int i = 0; i < RoomID1.Length; i++)
                                {
                                    Home obj = new Home();
                                    obj.RoomTypeText = dr["RoomType"].ToString();
                                    obj.RoomPriceText = dr["RoomPrice"].ToString();
                                    obj.RoomMaxPeopleCount = dr["RoomMaxPeopleCount"].ToString();
                                    obj.RoomCounttext = dr["RoomCount"].ToString();
                                    obj.NightCount = Daydiff;
                                    obj.Conditions = dr["Conditions"].ToString();
                                    obj.GuestName = dr["GuestName"].ToString();
                                    obj.BedPreference = dr["BedPreference"].ToString();
                                    obj.TravellerType = dr["TravellerType"].ToString();
                                    obj.EstimatedArrivalTime = dr["EstimatedArrivalTime"].ToString();
                                    obj.SpecialNote = dr["SpecialNote"].ToString();
                                    obj.PayableAmount = dr["PayableAmount"].ToString();
                                    obj.BookingTotalPriceWarning = dr["BookingTotalPriceWarning"].ToString();
                                    obj.NonRefundableInfo = dr["NonRefundableInfo"].ToString();
                                    obj.HotelID = hotelId;
                                    obj.Address = address;
                                    obj.Cityname = hotelCity;
                                    obj.MainPhotoName = mainPhotoName;
                                    obj.HotelClassValue = hotelClass;
                                    obj.CheckDatefrom = checkDatefrom;
                                    obj.CheckDateto = checkDateto;
                                    obj.CreditCardNotRequired = creditCardNotRequiredValue;
                                    obj.BedSelectionList = GetBedSelectionList(culture, hotelId, RoomID1[i]);


                                    if (hotelClass == "OneStar")
                                    {
                                        obj.HotelClass = "<img src=" + @"""/Images/star-small-active.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img src=" + @"""/Images/star-small-inactive.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img src=" + @"""/Images/star-small-inactive.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img src=" + @"""/Images/star-small-inactive.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img src=" + @"""/Images/star-small-inactive.png""" + "border=" + @"""0""" + ">&nbsp;";
                                    }
                                    else if (hotelClass == "TwoStar")
                                    {
                                        obj.HotelClass = "<img src=" + @"""/Images/star-small-active.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img src=" + @"""/Images/star-small-active.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img src=" + @"""/Images/star-small-inactive.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img src=" + @"""/Images/star-small-inactive.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img src=" + @"""/Images/star-small-inactive.png""" + "border=" + @"""0""" + ">&nbsp;";
                                    }
                                    else if (hotelClass == "ThreeStar")
                                    {
                                        obj.HotelClass = "<img src=" + @"""/Images/star-small-active.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img src=" + @"""/Images/star-small-active.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img src=" + @"""/Images/star-small-active.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img src=" + @"""/Images/star-small-inactive.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img src=" + @"""/Images/star-small-inactive.png""" + "border=" + @"""0""" + ">&nbsp;";
                                    }
                                    else if (hotelClass == "FourStar")
                                    {
                                        obj.HotelClass = "<img src=" + @"""/Images/star-small-active.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img src=" + @"""/Images/star-small-active.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img src=" + @"""/Images/star-small-active.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img src=" + @"""/Images/star-small-active.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img src=" + @"""/Images/star-small-inactive.png""" + "border=" + @"""0""" + ">&nbsp;";
                                    }
                                    else if (hotelClass == "FiveStar")
                                    {
                                        obj.HotelClass = "<img src=" + @"""/Images/star-small-active.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img src=" + @"""/Images/star-small-active.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img src=" + @"""/Images/star-small-active.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img src=" + @"""/Images/star-small-active.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img src=" + @"""/Images/star-small-active.png""" + "border=" + @"""0""" + ">&nbsp;";
                                    }
                                    else
                                    {
                                        obj.HotelClass = "";
                                    }

                                    decimal decimald = Convert.ToDecimal(RoomPriceVal1[i]);
                                    price = price + decimald;
                                    obj.RoomPriceTotal = price.ToString();
                                    obj.RoomPrice = RoomPriceVal1[i];
                                    obj.CurrencyCode = CurrencyCodeval1[i];
                                    obj.HotelTypeName = hiddenRoomTypeval1[i];
                                    obj.MaxPeopleCountval = RoomMaxPeopleCount1[i];
                                    Session_totalamountHotelCur = Session_totalamountHotelCur + Convert.ToDouble(Convert.ToInt32(RoomCount1[i]) * decimald);

                                    if (currentCurrency != "" && (currentCurrency != CurrencyCodeval1[i]))
                                    {
                                        if (CurrencyFromDB != CurrencyCodeval1[i])
                                        {
                                            CurrencyFromDB = CurrencyCodeval1[i];
                                            string Priceconvert = homeService.CurrencyConvertion(CurrencyFromDB, currentCurrency);
                                            string[] split = Priceconvert.Split(null);
                                            string priceVal = split[0];
                                            CurrenySym = split[1];
                                            ConveretedPrice = Convert.ToDouble(priceVal);

                                            double roomPrice = Convert.ToDouble(RoomPriceVal1[i]);
                                            double TotroomPrice = Convert.ToDouble(price);

                                            double ConvertedRoomPrice = roomPrice * ConveretedPrice;
                                            double ConvertedRoomPriceTot = TotroomPrice * ConveretedPrice;
                                            int rooomCount = Convert.ToInt32(RoomCount1[i]);
                                            if (rooomCount > 1)
                                            {
                                                ConvertedRoomPriceTot = ConvertedRoomPriceTot * rooomCount;
                                            }

                                            obj.ConvertedRoomPrice = Math.Round(ConvertedRoomPrice);
                                            obj.ConvertedRoomPriceTotal = Math.Round(ConvertedRoomPriceTot);
                                            obj.NewCurrencySymbol = CurrenySym;

                                        }
                                        else
                                        {
                                            double roomPrice = Convert.ToDouble(RoomPriceVal1[i]);
                                            double TotroomPrice = Convert.ToDouble(price);

                                            double ConvertedRoomPrice = roomPrice * ConveretedPrice;
                                            int rooomCount = Convert.ToInt32(RoomCount1[i]);
                                            double ConvertedRoomPriceTot = TotroomPrice * ConveretedPrice;

                                            if (rooomCount > 1)
                                            {
                                                ConvertedRoomPriceTot = ConvertedRoomPriceTot * rooomCount;
                                            }

                                            obj.ConvertedRoomPrice = Math.Round(ConvertedRoomPrice);
                                            obj.ConvertedRoomPriceTotal = Math.Round(ConvertedRoomPriceTot);

                                            obj.NewCurrencySymbol = CurrenySym;


                                        }
                                    }
                                    else
                                    {
                                        obj.ConvertedRoomPrice = Convert.ToDouble(RoomPriceVal1[i]);
                                        int rooomCount = Convert.ToInt32(RoomCount1[i]);
                                        if (rooomCount > 1)
                                        {
                                            obj.ConvertedRoomPriceTotal = Convert.ToDouble(obj.ConvertedRoomPrice * rooomCount);
                                        }
                                        else
                                        {
                                            obj.ConvertedRoomPriceTotal = Convert.ToDouble(obj.ConvertedRoomPrice);
                                        }
                                        obj.NewCurrencySymbol = CurrencyCodeval1[i];

                                    }


                                    if (RoomMaxPeopleCount1[i] == "1")
                                    {
                                        obj.MaxPeopleCount = "<span class=" + @"""glyphicon glyphicon-user""" + "></span>";
                                    }
                                    else if (RoomMaxPeopleCount1[i] == "2")
                                    {
                                        obj.MaxPeopleCount = "<span class=" + @"""glyphicon glyphicon-user""" + "></span>" + "<span class=" + @"""glyphicon glyphicon-user""" + "></span>";
                                    }
                                    else if (RoomMaxPeopleCount1[i] == "3")
                                    {
                                        obj.MaxPeopleCount = "<span class=" + @"""glyphicon glyphicon-user""" + "></span>" + "<span class=" + @"""glyphicon glyphicon-user""" + "></span>" + "<span class=" + @"""glyphicon glyphicon-user""" + "></span>";
                                    }
                                    else if (RoomMaxPeopleCount1[i] == "4")
                                    {
                                        obj.MaxPeopleCount = "<span class=" + @"""glyphicon glyphicon-user""" + "></span>" + "<span class=" + @"""glyphicon glyphicon-user""" + "></span>" + "<span class=" + @"""glyphicon glyphicon-user""" + "></span>" + "<span class=" + @"""glyphicon glyphicon-user""" + "></span>";
                                    }
                                    else
                                    {
                                        obj.MaxPeopleCount = "";
                                    }
                                    obj.HotelName = hotelname;
                                    obj.PriceType = hiddenPriceTypeval1[i];
                                    obj.HotelRoomID = RoomID1[i];
                                    obj.UniqueID = UniqueID1[i];
                                    obj.AccommodationTypeID = AccommodationTypeID1[i];
                                    obj.AccommodationTypeName = AccommodationTypeName1[i];
                                    obj.AccommodationTypeDescription = AccommodationTypeDescription1[i];
                                    obj.PricePolicyTypeName = PricePolicyTypeName1[i];
                                    obj.PricePolicyTypeID = PricePolicyTypeID1[i];


                                    if (PricePolicyTypeID1[i].ToString() == "1")
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
                                                obj.RoomTypeInfo = RefundableInfo2;
                                            }
                                        }
                                        else
                                        {
                                            obj.RoomTypeInfo = RefundableInfo;
                                        }

                                    }
                                    else if (PricePolicyTypeID1[i].ToString() == "2")
                                    {
                                        obj.RoomTypeInfo = dr["NonRefundableInfo"].ToString();
                                    }
                                    else
                                    {
                                        obj.RoomTypeInfo = "";
                                    }


                                    obj.SingleRate = SingleRate1[i];
                                    obj.DoubleRate = DoubleRate1[i];
                                    obj.DailyRoomPrices = DailyRoomPrices1[i];
                                    obj.OriginalRoomPrice = OriginalRoomPrice1[i];
                                    obj.CurrencyID = CurrencyID1[i];
                                    obj.CurrencySymbol = CurrencySymbol1[i];
                                    obj.RoomCount = RoomCount1[i];
                                    obj.LblId = homeService.GetTextMessagesAsString(culture, "booklabels");
                                    objlist.Add(obj);

                                    //make total
                                    Session_totalamount += (obj.ConvertedRoomPrice * Convert.ToInt32(obj.RoomCount));
                                }
                            }
                            else
                            {
                                Home obj = new Home();
                                obj.RoomTypeText = dr["RoomType"].ToString();
                                obj.RoomPriceText = dr["RoomPrice"].ToString();
                                obj.RoomMaxPeopleCount = dr["RoomMaxPeopleCount"].ToString();
                                obj.RoomCounttext = dr["RoomCount"].ToString();
                                obj.Conditions = dr["Conditions"].ToString();
                                obj.GuestName = dr["GuestName"].ToString();
                                obj.BedPreference = dr["BedPreference"].ToString();
                                obj.TravellerType = dr["TravellerType"].ToString();
                                obj.EstimatedArrivalTime = dr["EstimatedArrivalTime"].ToString();
                                obj.SpecialNote = dr["SpecialNote"].ToString();
                                obj.PayableAmount = dr["PayableAmount"].ToString();
                                obj.BookingTotalPriceWarning = dr["BookingTotalPriceWarning"].ToString();
                                obj.NonRefundableInfo = dr["NonRefundableInfo"].ToString();
                                obj.HotelID = hotelId;
                                obj.Address = address;
                                obj.Cityname = hotelCity;
                                obj.MainPhotoName = mainPhotoName;
                                obj.HotelClassValue = hotelClass;
                                obj.CheckDatefrom = checkDatefrom;
                                obj.CheckDateto = checkDateto;
                                obj.CreditCardNotRequired = creditCardNotRequiredValue;
                                obj.NightCount = Daydiff;
                                if (hotelClass == "OneStar")
                                {
                                    obj.HotelClass = "<img src=" + @"""/Images/star-small-active.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img src=" + @"""/Images/star-small-inactive.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img src=" + @"""/Images/star-small-inactive.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img src=" + @"""/Images/star-small-inactive.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img src=" + @"""/Images/star-small-inactive.png""" + "border=" + @"""0""" + ">&nbsp;";
                                }
                                else if (hotelClass == "TwoStar")
                                {
                                    obj.HotelClass = "<img src=" + @"""/Images/star-small-active.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img src=" + @"""/Images/star-small-active.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img src=" + @"""/Images/star-small-inactive.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img src=" + @"""/Images/star-small-inactive.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img src=" + @"""/Images/star-small-inactive.png""" + "border=" + @"""0""" + ">&nbsp;";
                                }
                                else if (hotelClass == "ThreeStar")
                                {
                                    obj.HotelClass = "<img src=" + @"""/Images/star-small-active.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img src=" + @"""/Images/star-small-active.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img src=" + @"""/Images/star-small-active.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img src=" + @"""/Images/star-small-inactive.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img src=" + @"""/Images/star-small-inactive.png""" + "border=" + @"""0""" + ">&nbsp;";
                                }
                                else if (hotelClass == "FourStar")
                                {
                                    obj.HotelClass = "<img src=" + @"""/Images/star-small-active.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img src=" + @"""/Images/star-small-active.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img src=" + @"""/Images/star-small-active.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img src=" + @"""/Images/star-small-active.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img src=" + @"""/Images/star-small-inactive.png""" + "border=" + @"""0""" + ">&nbsp;";
                                }
                                else if (hotelClass == "FiveStar")
                                {
                                    obj.HotelClass = "<img src=" + @"""/Images/star-small-active.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img src=" + @"""/Images/star-small-active.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img src=" + @"""/Images/star-small-active.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img src=" + @"""/Images/star-small-active.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img src=" + @"""/Images/star-small-active.png""" + "border=" + @"""0""" + ">&nbsp;";
                                }
                                else
                                {
                                    obj.HotelClass = "";
                                }


                                Session_totalamountHotelCur = Session_totalamountHotelCur + Convert.ToDouble(Convert.ToDouble(roomPriceVal) * Convert.ToInt32(roomCount));

                                if (currentCurrency != "" && (currentCurrency != currencyCodeval))
                                {
                                    if (CurrencyFromDB != currencyCodeval)
                                    {
                                        CurrencyFromDB = currencyCodeval;
                                        string Priceconvert = homeService.CurrencyConvertion(CurrencyFromDB, currentCurrency);
                                        if (!String.IsNullOrEmpty(Priceconvert))
                                        {
                                            string[] split = Priceconvert.Split(null);
                                            string priceVal = split[0];
                                            CurrenySym = split[1];
                                            ConveretedPrice = Convert.ToDouble(priceVal);

                                            double roomPrice = Convert.ToDouble(roomPriceVal);

                                            double ConvertedRoomPrice = roomPrice * ConveretedPrice;
                                            obj.ConvertedRoomPrice = Math.Round(ConvertedRoomPrice);
                                            obj.ConvertedRoomPriceTotal = Math.Round(ConvertedRoomPrice);

                                            obj.NewCurrencySymbol = CurrenySym;

                                        }
                                        else
                                        {
                                            obj.ConvertedRoomPrice = Convert.ToDouble(roomPriceVal);
                                            obj.ConvertedRoomPriceTotal = Convert.ToDouble(roomPriceVal);
                                            obj.NewCurrencySymbol = currencyCodeval;
                                        }
                                    }
                                    else
                                    {
                                        double roomPrice = Convert.ToDouble(roomPriceVal); ;
                                        double ConvertedRoomPrice = roomPrice * ConveretedPrice;
                                        obj.ConvertedRoomPrice = Math.Round(ConvertedRoomPrice);
                                        obj.ConvertedRoomPriceTotal = Math.Round(ConvertedRoomPrice);
                                        obj.NewCurrencySymbol = CurrenySym;
                                    }
                                }
                                else
                                {
                                    obj.ConvertedRoomPrice = Convert.ToDouble(roomPriceVal);
                                    obj.ConvertedRoomPriceTotal = Convert.ToDouble(roomPriceVal);
                                    obj.NewCurrencySymbol = currencyCodeval;
                                }

                                obj.RoomPrice = roomPriceVal;
                                obj.RoomPriceTotal = roomPriceVal;

                                obj.CurrencyCode = currencyCodeval;

                                if (roomMaxPeopleCount == "1")
                                {
                                    obj.MaxPeopleCount = "<span class=" + @"""glyphicon glyphicon-user""" + "></span>";
                                }
                                else if (roomMaxPeopleCount == "2")
                                {
                                    obj.MaxPeopleCount = "<span class=" + @"""glyphicon glyphicon-user""" + "></span>" + "<span class=" + @"""glyphicon glyphicon-user""" + "></span>";
                                }
                                else if (roomMaxPeopleCount == "3")
                                {
                                    obj.MaxPeopleCount = "<span class=" + @"""glyphicon glyphicon-user""" + "></span>" + "<span class=" + @"""glyphicon glyphicon-user""" + "></span>" + "<span class=" + @"""glyphicon glyphicon-user""" + "></span>";
                                }
                                else if (roomMaxPeopleCount == "4")
                                {
                                    obj.MaxPeopleCount = "<span class=" + @"""glyphicon glyphicon-user""" + "></span>" + "<span class=" + @"""glyphicon glyphicon-user""" + "></span>" + "<span class=" + @"""glyphicon glyphicon-user""" + "></span>" + "<span class=" + @"""glyphicon glyphicon-user""" + "></span>";
                                }
                                else
                                {
                                    obj.MaxPeopleCount = "";
                                }
                                obj.PriceType = hiddenPriceTypeval;
                                obj.HotelTypeName = hiddenRoomTypeval;
                                obj.HotelName = hotelname;
                                obj.BedSelectionList = GetBedSelectionList(culture, hotelId, roomId);
                                obj.HotelRoomID = roomId;
                                obj.UniqueID = uniqueId;
                                obj.AccommodationTypeID = accommodationTypeId;
                                obj.AccommodationTypeName = accommodationTypeName;
                                obj.AccommodationTypeDescription = accommodationTypeDescription;
                                obj.PricePolicyTypeName = pricePolicyTypeName;
                                obj.PricePolicyTypeID = pricePolicyTypeId;
                                obj.MaxPeopleCountval = roomMaxPeopleCount;

                                if (pricePolicyTypeId.ToString() == "1")
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
                                            obj.RoomTypeInfo = RefundableInfo2;
                                        }
                                    }
                                    else
                                    {
                                        obj.RoomTypeInfo = RefundableInfo;
                                    }

                                }
                                else if (pricePolicyTypeId.ToString() == "2")
                                {
                                    obj.RoomTypeInfo = dr["NonRefundableInfo"].ToString();
                                }
                                else
                                {
                                    obj.RoomTypeInfo = "";
                                }

                                obj.SingleRate = singleRate;
                                obj.DoubleRate = doubleRate;
                                obj.DailyRoomPrices = dailyRoomPrices;
                                obj.OriginalRoomPrice = originalRoomPrice;
                                obj.CurrencyID = currencyId;
                                obj.CurrencySymbol = currencySymbol;
                                obj.RoomCount = roomCount;
                                obj.LblId = homeService.GetTextMessagesAsString(culture, "booklabels");

                                //set totla and totalperson count

                                Session_Personcount = Convert.ToInt32(roomCount) * (Convert.ToInt32(obj.MaxPeopleCountval));
                                //Session_totalamount = obj.ConvertedRoomPriceTotal;

                                //make total
                                Session_totalamount += obj.ConvertedRoomPrice * Convert.ToInt32(obj.RoomCount);

                                objlist.Add(obj);
                            }
                        }
                    }
                }
                //double ConvertedRoomPriceTax=0.0, totalvalueTax = 0.0, TotalValueInHotelCurrency;
                double total = 0.0, totalInHotelCurrency = 0.0;

                //List<Home> listHotelTax = GetHotelTax(culture, hotelId, currentCurrency, checkDatefrom, checkDateto, out ConvertedRoomPriceTax, out totalvalueTax, out TotalValueInHotelCurrency);
                List<Home> listHotelTax = GetHotelTax(culture, hotelId, currentCurrency, checkDatefrom, checkDateto, Session_Personcount, Session_totalamount, Session_totalamountHotelCur, out total, out totalInHotelCurrency);
                //return Request.CreateResponse(HttpStatusCode.OK, new { list = objlist, HotelTax = listHotelTax, ConvertedRoomPriceTax = ConvertedRoomPriceTax, totalvalueTax = totalvalueTax, ConvertedRoomPriceTotal = TotalValueInHotelCurrency });
                return Request.CreateResponse(HttpStatusCode.OK, new { list = objlist, HotelTax = listHotelTax, convertedRoomPriceTotal = total, total = total, totalInHotelCurrency = totalInHotelCurrency });
            }
            catch (Exception ex)
            {
                return LogError(ex);
            }
        }

        [HttpPost]
        public HttpResponseMessage changeBookingDate(string checkin, string checkout, string Session_PolicyTypeID, string Session_StoreHotelID, string Session_Code, string Session_RoomPriceTypeID, string Session_RoomCountHid, string Session_HotelRoomIDAry)
        {
            DateTime time;
            DateTime time2;
            int num = 0;
            try
            {
                time = DateTime.ParseExact(checkin, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                time2 = DateTime.ParseExact(checkout, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                DateTime.ParseExact(checkout, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                time2 = time2.AddDays(-1.0);
            }
            catch
            {
                time = DateTime.ParseExact(checkin, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                time2 = DateTime.ParseExact(checkout, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                DateTime.ParseExact(checkout, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                time2 = time2.AddDays(-1.0);
            }
            string checkDatefrom = time.ToString("yyyy/MM/dd");
            string checkDateto = time2.ToString("yyyy/MM/dd");
            try
            {
                if ((((Session_PolicyTypeID != null) && (Session_StoreHotelID != null)) && ((Session_Code != null) && (Session_RoomPriceTypeID != null))) && ((Session_RoomCountHid != null) && (Session_HotelRoomIDAry != null)))
                {
                    string str3 = Session_RoomCountHid.ToString();
                    string str4 = Session_PolicyTypeID.ToString();
                    string str5 = Session_HotelRoomIDAry.ToString();
                    char[] separator = new char[] { ',' };
                    string[] strArray = Session_RoomPriceTypeID.ToString().Split(separator);
                    char[] chArray2 = new char[] { ',' };
                    str3.Split(chArray2);
                    char[] chArray3 = new char[] { ',' };
                    string[] strArray2 = str4.Split(chArray3);
                    char[] chArray4 = new char[] { ',' };
                    string[] strArray3 = str5.Split(chArray4);
                    for (int i = 0; i < strArray.Length; i++)
                    {
                        DataTable table = new Home().GetvaluesWhileChangelang(Session_Code.ToString(), Session_StoreHotelID.ToString(), strArray[i], checkDatefrom, checkDateto, strArray2[i], strArray3[i], this.BizContext.ShowSecretDeals);
                        if (table != null)
                        {
                            if (table.Rows.Count <= 0)
                            {
                                num = 0;
                                return base.Request.CreateResponse<int>(HttpStatusCode.OK, num);
                            }
                            num = 2;
                        }
                        else
                        {
                            num = 0;
                            return base.Request.CreateResponse<int>(HttpStatusCode.OK, num);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                base.LogError(exception);
            }
            return base.Request.CreateResponse<int>(HttpStatusCode.OK, num);
        }


        [HttpGet]
        public HttpResponseMessage GetProfileDetailsIfAvailable(string culture, string userId)
        {

            try
            {

                DataTable dataTableProfile = new DataTable();
                BookingProfileModel bookingProfile = new BookingProfileModel();
                if (!userId.StartsWith("GUEST"))
                {
                    dataTableProfile = homeService.getProfileDetails(userId, culture);
                    if (dataTableProfile != null && dataTableProfile.Rows.Count > 0)
                    {

                        DataRow dr = dataTableProfile.Rows[0];

                        bookingProfile.UserID = userId;
                        bookingProfile.Userphoto = "../Images/" + dr["Userphoto"].ToString();
                        bookingProfile.SalutationTypeID = dr["SalutationTypeID"].ToString();
                        bookingProfile.Image = "../Images/120087.jpg";
                        bookingProfile.UserName = dr["UserName"].ToString();
                        bookingProfile.Email = dr["Email"].ToString();
                        bookingProfile.Name = dr["Name"].ToString();
                        bookingProfile.Surname = dr["Surname"].ToString();
                        bookingProfile.Phone = dr["Phone"].ToString();
                        bookingProfile.Country = dr["Country"].ToString();
                        bookingProfile.CountryID = dr["CountryID"].ToString();
                        bookingProfile.City = dr["City"].ToString();
                        bookingProfile.CityID = dr["CityID"].ToString();
                        bookingProfile.Address = dr["Address"].ToString();
                        bookingProfile.PostCode = dr["PostCode"].ToString();

                    }
                }
                return Request.CreateResponse(HttpStatusCode.OK, bookingProfile);

            }
            catch (Exception ex)
            {
                return LogError(ex);
            }
        }
        [HttpGet]
        public HttpResponseMessage GetUserSavedCardDetailsByHotel(string userId, string hotelId)
        {
            List<BookingCreditCard> listbookingCreditcard = new List<BookingCreditCard>();

            try
            {


                DataTable dt = new DataTable();

                dt = homeService.GetUserCreditCardByHotel(userId, hotelId);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        BookingCreditCard creditCard = new BookingCreditCard();
                        creditCard.CreditCardProviderID = dr["CreditCardProviderID"].ToString();
                        creditCard.ExpiryDate = homeService.Decrypt(dr["ExpiryDate"].ToString());
                        creditCard.NameOnCreditCard = homeService.Decrypt(dr["NameOnCreditCard"].ToString());
                        creditCard.CreditCardNumber = homeService.Decrypt(dr["CreditCardNumber"].ToString());
                        creditCard.CreditCardTypeName = dr["Name"].ToString();
                        string month = homeService.Decrypt(dr["ExpiryDate"].ToString());
                        string[] month1 = month.Split('/');
                        creditCard.month = month1[0];
                        creditCard.year = month1[1];
                        listbookingCreditcard.Add(creditCard);
                    }
                }
                return Request.CreateResponse(HttpStatusCode.OK, listbookingCreditcard);
            }
            catch (Exception ex)
            {
                return LogError(ex);
            }
        }

        public HttpResponseMessage GetValuesWhileChangelang(string currentCurrency, string culture, string hotelId, string roomPriceTypeId, string checkDatefrom,
        string checkDateTo, string roomCountHId, string policyTypeId, string hotelRoomIdAry)
        {
            List<Home> objlist = new List<Home>();
            try
            {
                objlist = GetValuesWhileChangelang_local(currentCurrency, culture, hotelId, roomPriceTypeId, checkDatefrom, checkDateTo, roomCountHId, policyTypeId, hotelRoomIdAry);
                return Request.CreateResponse(HttpStatusCode.OK, objlist);
            }
            catch (Exception ex)
            {
                return LogError(ex);
            }
        }

         [System.Web.Http.Description.ApiExplorerSettings(IgnoreApi = true)] 
        private List<Home> GetValuesWhileChangelang_local(string currentCurrency, string culture, string hotelId, string roomPriceTypeId, string checkDatefrom,
        string checkDateTo, string roomCountHId, string policyTypeId, string hotelRoomIdAry)
        {

            List<Home> objlist = new List<Home>();

            DateTime dt = DateTime.ParseExact(checkDatefrom, "dd-MMM-yyyy", CultureInfo.InvariantCulture);
            DateTime dt1 = DateTime.ParseExact(checkDateTo, "dd-MMM-yyyy", CultureInfo.InvariantCulture);
            String Daydiff = (dt1 - dt).TotalDays.ToString();
            String Hourdiff = (dt1 - dt).TotalHours.ToString();
            String Mindiff = (dt1 - dt).TotalMinutes.ToString();


            string Datefromm = dt.ToString("yyyy/MM/dd");
            string Datetoo = dt1.ToString("yyyy/MM/dd");
            string Checkoutday = dt1.AddDays(-1).ToString("yyyy/MM/dd");
            DataTable RoomDetails = new DataTable();
            DataTable MessageCode = new DataTable();
            DataTable Tax = new DataTable();

            string RoomTypeText = "";
            string RoomPriceText = "";
            string RoomMaxPeopleCount = "";
            string RoomCounttext = "";
            string Conditions = "";
            string GuestName = "";
            string BedPreference = "";
            string TravellerType = "";
            string EstimatedArrivalTime = "";
            string SpecialNote = "";
            string PayableAmount = "";
            string BookingTotalPriceWarning = "";
            string NonRefundableInfo = "";
            string ID = "";
            string Description = "";
            string RoutingName = "";
            string MainPhotoName = "";
            string HotelClassValue = "";
            string CityName = "";
            string HotelStar = "";
            string Address = "";
            string CurrencyID = "";
            string CurrencySymbol = "";
            string HotelClass = "";
            decimal price = 0;
            int CreditCardNotRequired = 0;
            int SessionPeopleCount = 0;
            double SessionPrice = 0;
            double SessionPriceinHotelCur = 0;
            string booklabels = homeService.GetTextMessagesAsString(culture, "booklabels");


            DataTable HotelDetails = new DataTable();
            try
            {
                HotelDetails = homeService.GetHotelBasicInfo(culture, hotelId);
                if (HotelDetails.Rows.Count > 0)
                {
                    foreach (DataRow dr in HotelDetails.Rows)
                    {
                        Home objMember = new Home();
                        ID = dr["ID"].ToString();
                        Description = objMember.TruncateLongString(dr["Description"].ToString(), 200);
                        RoutingName = dr["RoutingName"].ToString();
                        MainPhotoName = URL.EXTRANET_URLFULL + "Photo/Hotel/" + dr["ID"].ToString() + "/" + dr["MainPhotoName"].ToString();
                        HotelClassValue = dr["HotelClass"].ToString();
                        CityName = dr["CityName"].ToString();
                        HotelStar = dr["HotelStar"].ToString();
                        Address = dr["Address"].ToString();
                        CreditCardNotRequired = Convert.ToInt32(dr["CreditCardNotRequired"]);

                        CurrencyID = dr["CurrencyID"].ToString();
                        CurrencySymbol = dr["CurrencySymbol"].ToString();
                        //string HotelClass1= "<img src="+@"""/Images/star-small-active.png"""+"border="+@"""0"""+">&nbsp;"   <img src="/Images/star-small-active.png" border="0">&nbsp;<img src="/Images/star-small-active.png" border="0">&nbsp;<img src="/Images/star-small-inactive.png" border="0">&nbsp;<img src="/Images/star-small-inactive.png" border="0">

                        if (dr["HotelClass"].ToString() == "OneStar")
                        {
                            HotelClass = "<img src=" + @"""/Images/star-small-active.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img src=" + @"""/Images/star-small-inactive.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img src=" + @"""/Images/star-small-inactive.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img src=" + @"""/Images/star-small-inactive.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img src=" + @"""/Images/star-small-inactive.png""" + "border=" + @"""0""" + ">&nbsp;";
                        }
                        else if (dr["HotelClass"].ToString() == "TwoStar")
                        {
                            HotelClass = "<img src=" + @"""/Images/star-small-active.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img src=" + @"""/Images/star-small-active.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img src=" + @"""/Images/star-small-inactive.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img src=" + @"""/Images/star-small-inactive.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img src=" + @"""/Images/star-small-inactive.png""" + "border=" + @"""0""" + ">&nbsp;";
                        }
                        else if (dr["HotelClass"].ToString() == "ThreeStar")
                        {
                            HotelClass = "<img src=" + @"""/Images/star-small-active.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img src=" + @"""/Images/star-small-active.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img src=" + @"""/Images/star-small-active.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img src=" + @"""/Images/star-small-inactive.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img src=" + @"""/Images/star-small-inactive.png""" + "border=" + @"""0""" + ">&nbsp;";
                        }
                        else if (dr["HotelClass"].ToString() == "FourStar")
                        {
                            HotelClass = "<img src=" + @"""/Images/star-small-active.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img src=" + @"""/Images/star-small-active.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img src=" + @"""/Images/star-small-active.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img src=" + @"""/Images/star-small-active.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img src=" + @"""/Images/star-small-inactive.png""" + "border=" + @"""0""" + ">&nbsp;";
                        }
                        else if (dr["HotelClass"].ToString() == "FiveStar")
                        {
                            HotelClass = "<img src=" + @"""/Images/star-small-active.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img src=" + @"""/Images/star-small-active.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img src=" + @"""/Images/star-small-active.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img src=" + @"""/Images/star-small-active.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img src=" + @"""/Images/star-small-active.png""" + "border=" + @"""0""" + ">&nbsp;";
                        }
                        else
                        {
                            HotelClass = "Unrated";
                        }

                    }
                }

                MessageCode = homeService.GetRequiredMessageCode(culture);

                if (MessageCode != null)
                {
                    if (MessageCode.Rows.Count > 0)
                    {
                        foreach (DataRow dr in MessageCode.Rows)
                        {
                            Home obj = new Home();
                            RoomTypeText = dr["RoomType"].ToString();
                            RoomPriceText = dr["RoomPrice"].ToString();
                            RoomMaxPeopleCount = dr["RoomMaxPeopleCount"].ToString();
                            RoomCounttext = dr["RoomCount"].ToString();
                            Conditions = dr["Conditions"].ToString();
                            GuestName = dr["GuestName"].ToString();
                            BedPreference = dr["BedPreference"].ToString();
                            TravellerType = dr["TravellerType"].ToString();
                            EstimatedArrivalTime = dr["EstimatedArrivalTime"].ToString();
                            SpecialNote = dr["SpecialNote"].ToString();
                            PayableAmount = dr["PayableAmount"].ToString();
                            BookingTotalPriceWarning = dr["BookingTotalPriceWarning"].ToString();
                            NonRefundableInfo = dr["NonRefundableInfo"].ToString();
                        }
                    }
                }


                string CurrencyFromDB = "";
                string CurrenySym = "";
                double ConveretedPrice = 0;


                if (roomPriceTypeId.Contains(','))
                {

                    string[] Code = roomPriceTypeId.Split(',');
                    string[] RoomCountH = roomCountHId.Split(',');
                    string[] PolicyTypeIDArry = policyTypeId.Split(',');
                    string[] HotelRoomIDArrray = hotelRoomIdAry.Split(',');

                    for (int i = 0; i < Code.Length; i++)
                    {

                        RoomDetails = homeService.GetvaluesWhileChangelang(culture, hotelId, Code[i], Datefromm, Checkoutday, PolicyTypeIDArry[i], HotelRoomIDArrray[i], BizContext.ShowSecretDeals);

                        if (RoomDetails != null)
                        {
                            if (RoomDetails.Rows.Count > 0)
                            {
                                foreach (DataRow dr in RoomDetails.Rows)
                                {


                                    Home objRoom = new Home();
                                    objRoom.BedSelectionList = GetBedSelectionList(culture, hotelId, HotelRoomIDArrray[i]);
                                    objRoom.RoomCount = RoomCountH[i];
                                    objRoom.OptionRoomCount = dr["RoomCount"].ToString();
                                    objRoom.NightCount = Daydiff;
                                    objRoom.RoomTypeText = RoomTypeText;
                                    objRoom.RoomPriceText = RoomPriceText;
                                    objRoom.RoomMaxPeopleCount = RoomMaxPeopleCount;
                                    objRoom.RoomCounttext = RoomCounttext;
                                    objRoom.Conditions = Conditions;
                                    objRoom.GuestName = GuestName;
                                    objRoom.BedPreference = BedPreference;
                                    objRoom.TravellerType = TravellerType;
                                    objRoom.EstimatedArrivalTime = EstimatedArrivalTime;
                                    objRoom.SpecialNote = SpecialNote;
                                    objRoom.PayableAmount = PayableAmount;
                                    objRoom.BookingTotalPriceWarning = BookingTotalPriceWarning;
                                    objRoom.NonRefundableInfo = NonRefundableInfo;
                                    objRoom.HotelClass = HotelClass;
                                    objRoom.Description = Description;
                                    objRoom.RoutingName = RoutingName;
                                    objRoom.MainPhotoName = MainPhotoName;
                                    objRoom.RoomPhotoName = URL.EXTRANET_URLFULL + "Photo/Hotel/" + hotelId + "/" + dr["RoomPhotoName"].ToString();
                                    string booklabelstext = booklabels;
                                    objRoom.LblId = booklabelstext;
                                    objRoom.HotelClassValue = HotelClassValue;
                                    objRoom.Cityname = CityName;
                                    objRoom.HotelStar = HotelStar;
                                    objRoom.Address = Address;
                                    objRoom.CurrencyID = CurrencyID;
                                    objRoom.CurrencySymbol = CurrencySymbol;
                                    objRoom.CreditCardNotRequired = CreditCardNotRequired;
                                    string MaxPeopleCountIconDesign = "";
                                    for (int ro = 0; ro < Convert.ToInt32(dr["MaxPeopleCount"]); ro++)
                                    {
                                        MaxPeopleCountIconDesign += "<span class=" + @"""glyphicon glyphicon-user""" + "></span>";
                                    }
                                    objRoom.MaxPeopleCount = MaxPeopleCountIconDesign;

                                    objRoom.MaxPeopleCountval = dr["MaxPeopleCount"].ToString();
                                    SessionPeopleCount += (Convert.ToInt32(dr["MaxPeopleCount"]) * Convert.ToInt32(RoomCountH[i]));

                                    objRoom.RoomDataTableType = "RoomDetails";
                                    objRoom.CheckDatefrom = checkDatefrom;
                                    objRoom.CheckDateto = checkDateTo;
                                    objRoom.PriceType = dr["PriceType"].ToString();
                                    objRoom.HotelID = dr["HotelID"].ToString();
                                    objRoom.HotelRoomID = dr["ID"].ToString();
                                    objRoom.HotelRoomDescription = dr["RoomDescription"].ToString();
                                    objRoom.HotelTypeName = dr["RoomTypeName"].ToString();
                                    objRoom.SmokingTypeName = dr["SmokingTypeName"].ToString();
                                    objRoom.RoomCount = RoomCountH[i];
                                    decimal decimald = Convert.ToDecimal(dr["RoomPrice"]) * Convert.ToInt32(RoomCountH[i]);

                                    SessionPriceinHotelCur += Convert.ToDouble(decimald);
                                    price = price + decimald;
                                    objRoom.RoomPriceTotal = price.ToString();
                                    objRoom.RoomPrice = dr["RoomPrice"].ToString();


                                    objRoom.CurrencyCode = dr["CurrencyCode"].ToString();
                                    objRoom.RoomMaxPeopleCount = dr["RoomMaxPeopleCount"].ToString();
                                    objRoom.FREEcancellation = dr["FREEcancellation"].ToString();
                                    objRoom.RoomFacilities = dr["RoomFacilities"].ToString();
                                    objRoom.UniqueID = dr["UniqueID"].ToString();
                                    objRoom.AccommodationTypeID = dr["AccommodationTypeID"].ToString();
                                    objRoom.AccommodationTypeName = dr["AccommodationTypeName"].ToString();
                                    objRoom.AccommodationTypeDescription = dr["AccommodationTypeDescription"].ToString();
                                    objRoom.PricePolicyTypeID = dr["PricePolicyTypeID"].ToString();

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
                                                objRoom.RoomTypeInfo = RefundableInfo2;
                                            }
                                        }
                                        else
                                            objRoom.RoomTypeInfo = RefundableInfo;
                                    }
                                    else if (dr["PricePolicyTypeID"].ToString() == "2")
                                        objRoom.RoomTypeInfo = dr["NonRefundableInfo"].ToString();
                                    else
                                        objRoom.RoomTypeInfo = "";



                                    objRoom.PricePolicyTypeName = dr["PricePolicyTypeName"].ToString();
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
                                            string Priceconvert = homeService.CurrencyConvertion(CurrencyFromDB, currentCurrency);
                                            string[] split = Priceconvert.Split(null);
                                            string priceVal = split[0];
                                            CurrenySym = split[1];
                                            ConveretedPrice = Convert.ToDouble(priceVal);

                                            double roomPrice = Convert.ToDouble(dr["RoomPrice"]);
                                            double TotroomPrice = Convert.ToDouble(price);
                                            double ConvertedRoomPrice = roomPrice * ConveretedPrice;
                                            double ConvertedRoomPriceTot = TotroomPrice * ConveretedPrice;

                                            objRoom.ConvertedRoomPrice = Math.Round(ConvertedRoomPrice);
                                            objRoom.ConvertedRoomPriceTotal = Math.Round(ConvertedRoomPriceTot);
                                            objRoom.NewCurrencySymbol = CurrenySym;

                                        }
                                        else
                                        {
                                            double roomPrice = Convert.ToDouble(dr["RoomPrice"]);
                                            double TotroomPrice = Convert.ToDouble(price);
                                            double ConvertedRoomPrice = roomPrice * ConveretedPrice;
                                            double ConvertedRoomPriceTot = TotroomPrice * ConveretedPrice;

                                            objRoom.ConvertedRoomPrice = Math.Round(ConvertedRoomPrice);
                                            objRoom.ConvertedRoomPriceTotal = Math.Round(ConvertedRoomPriceTot);

                                            objRoom.NewCurrencySymbol = CurrenySym;
                                        }
                                    }
                                    else
                                    {
                                        objRoom.ConvertedRoomPrice = Convert.ToDouble(dr["RoomPrice"]);
                                        objRoom.ConvertedRoomPriceTotal = Convert.ToDouble(price);
                                        objRoom.NewCurrencySymbol = dr["CurrencySymbol"].ToString();
                                    }
                                    SessionPrice += Convert.ToDouble(objRoom.ConvertedRoomPrice) * Convert.ToInt32(RoomCountH[i]);
                                    objlist.Add(objRoom);
                                }
                            }
                        }
                    }

                }
                else
                {

                    RoomDetails = homeService.GetvaluesWhileChangelang(culture, hotelId, roomPriceTypeId, Datefromm, Checkoutday, policyTypeId, hotelRoomIdAry, BizContext.ShowSecretDeals);
                    if (RoomDetails != null)
                    {
                        if (RoomDetails.Rows.Count > 0)
                        {
                            foreach (DataRow dr in RoomDetails.Rows)
                            {
                                int roomcountvaaal = 1;
                                Home objRoom = new Home();
                                objRoom.BedSelectionList = GetBedSelectionList(culture, hotelId, hotelRoomIdAry);
                                objRoom.RoomCount = roomCountHId;
                                roomcountvaaal = Convert.ToInt32(roomCountHId);
                                SessionPeopleCount += (Convert.ToInt32(dr["MaxPeopleCount"]) * Convert.ToInt32(roomCountHId));
                                objRoom.OptionRoomCount = dr["RoomCount"].ToString();
                                objRoom.NightCount = Daydiff;
                                objRoom.RoomTypeText = RoomTypeText;
                                objRoom.RoomPriceText = RoomPriceText;
                                objRoom.RoomMaxPeopleCount = RoomMaxPeopleCount;
                                objRoom.RoomCounttext = RoomCounttext;
                                objRoom.Conditions = Conditions;
                                objRoom.GuestName = GuestName;
                                objRoom.BedPreference = BedPreference;
                                objRoom.TravellerType = TravellerType;
                                objRoom.EstimatedArrivalTime = EstimatedArrivalTime;
                                objRoom.SpecialNote = SpecialNote;
                                objRoom.PayableAmount = PayableAmount;
                                objRoom.BookingTotalPriceWarning = BookingTotalPriceWarning;
                                objRoom.NonRefundableInfo = NonRefundableInfo;
                                objRoom.HotelClass = HotelClass;
                                objRoom.Description = Description;
                                objRoom.RoutingName = RoutingName;
                                objRoom.MainPhotoName = MainPhotoName;
                                objRoom.RoomPhotoName = URL.EXTRANET_URLFULL + "Photo/Hotel/" + hotelId + "/" + dr["RoomPhotoName"].ToString();
                                string booklabelstext = booklabels;
                                objRoom.LblId = booklabelstext;
                                objRoom.HotelClassValue = HotelClassValue;
                                objRoom.Cityname = CityName;
                                objRoom.HotelStar = HotelStar;
                                objRoom.Address = Address;
                                objRoom.CurrencyID = CurrencyID;
                                objRoom.CurrencySymbol = CurrencySymbol;
                                objRoom.CreditCardNotRequired = CreditCardNotRequired;
                                if (dr["MaxPeopleCount"].ToString() == "1")
                                    objRoom.MaxPeopleCount = "<span class=" + @"""glyphicon glyphicon-user""" + "></span>";
                                else if (dr["MaxPeopleCount"].ToString() == "2")
                                    objRoom.MaxPeopleCount = "<span class=" + @"""glyphicon glyphicon-user""" + "></span>" + "<span class=" + @"""glyphicon glyphicon-user""" + "></span>";
                                else if (dr["MaxPeopleCount"].ToString() == "3")
                                    objRoom.MaxPeopleCount = "<span class=" + @"""glyphicon glyphicon-user""" + "></span>" + "<span class=" + @"""glyphicon glyphicon-user""" + "></span>" + "<span class=" + @"""glyphicon glyphicon-user""" + "></span>";
                                else if (dr["MaxPeopleCount"].ToString() == "4")
                                    objRoom.MaxPeopleCount = "<span class=" + @"""glyphicon glyphicon-user""" + "></span>" + "<span class=" + @"""glyphicon glyphicon-user""" + "></span>" + "<span class=" + @"""glyphicon glyphicon-user""" + "></span>" + "<span class=" + @"""glyphicon glyphicon-user""" + "></span>";
                                else
                                    objRoom.MaxPeopleCount = "";
                                objRoom.MaxPeopleCountval = dr["MaxPeopleCount"].ToString();
                                objRoom.RoomDataTableType = "RoomDetails";
                                objRoom.CheckDatefrom = checkDatefrom;
                                objRoom.CheckDateto = checkDateTo;
                                objRoom.PriceType = dr["PriceType"].ToString();
                                objRoom.HotelID = dr["HotelID"].ToString();
                                objRoom.HotelRoomID = dr["ID"].ToString();
                                objRoom.HotelRoomDescription = dr["RoomDescription"].ToString();
                                objRoom.HotelTypeName = dr["RoomTypeName"].ToString();
                                objRoom.SmokingTypeName = dr["SmokingTypeName"].ToString();
                                objRoom.RoomPrice = dr["RoomPrice"].ToString();
                                SessionPriceinHotelCur = Convert.ToDouble(dr["RoomPrice"]) * roomcountvaaal;
                                objRoom.RoomPriceTotal = SessionPriceinHotelCur.ToString();
                                objRoom.CurrencyCode = dr["CurrencyCode"].ToString();
                                objRoom.RoomMaxPeopleCount = dr["RoomMaxPeopleCount"].ToString();
                                objRoom.FREEcancellation = dr["FREEcancellation"].ToString();
                                objRoom.RoomFacilities = dr["RoomFacilities"].ToString();
                                objRoom.UniqueID = dr["UniqueID"].ToString();
                                objRoom.AccommodationTypeID = dr["AccommodationTypeID"].ToString();
                                objRoom.AccommodationTypeName = dr["AccommodationTypeName"].ToString();
                                objRoom.AccommodationTypeDescription = dr["AccommodationTypeDescription"].ToString();
                                objRoom.PricePolicyTypeID = dr["PricePolicyTypeID"].ToString();

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
                                            objRoom.RoomTypeInfo = RefundableInfo2;
                                        }
                                    }
                                    else
                                        objRoom.RoomTypeInfo = RefundableInfo;

                                }
                                else if (dr["PricePolicyTypeID"].ToString() == "2")
                                    objRoom.RoomTypeInfo = dr["NonRefundableInfo"].ToString();
                                else
                                    objRoom.RoomTypeInfo = "";

                                objRoom.PricePolicyTypeName = dr["PricePolicyTypeName"].ToString();
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
                                        string Priceconvert = homeService.CurrencyConvertion(CurrencyFromDB, currentCurrency);
                                        string[] split = Priceconvert.Split(null);
                                        string priceVal = split[0];
                                        CurrenySym = split[1];
                                        ConveretedPrice = Convert.ToDouble(priceVal);

                                        double roomPrice = Convert.ToDouble(dr["RoomPrice"]);
                                        double ConvertedRoomPrice = roomPrice * ConveretedPrice;
                                        objRoom.ConvertedRoomPrice = Math.Round(ConvertedRoomPrice);
                                        if (roomcountvaaal > 1)
                                        {
                                            objRoom.ConvertedRoomPriceTotal = Math.Round(ConvertedRoomPrice * roomcountvaaal);

                                        }
                                        else
                                        {
                                            objRoom.ConvertedRoomPriceTotal = Math.Round(ConvertedRoomPrice);

                                        }

                                        objRoom.NewCurrencySymbol = CurrenySym;

                                    }
                                    else
                                    {
                                        double roomPrice = Convert.ToDouble(dr["RoomPrice"]);
                                        double ConvertedRoomPrice = roomPrice * ConveretedPrice;
                                        objRoom.ConvertedRoomPrice = Math.Round(ConvertedRoomPrice);
                                        if (roomcountvaaal > 1)
                                        {
                                            objRoom.ConvertedRoomPriceTotal = Math.Round(ConvertedRoomPrice * roomcountvaaal);

                                        }
                                        else
                                        {
                                            objRoom.ConvertedRoomPriceTotal = Math.Round(ConvertedRoomPrice);

                                        }
                                        objRoom.NewCurrencySymbol = CurrenySym;
                                    }
                                }
                                else
                                {
                                    objRoom.ConvertedRoomPrice = Convert.ToDouble(dr["RoomPrice"]);
                                    if (roomcountvaaal > 1)
                                    {
                                        double ConvertedRoomPrice1 = Convert.ToDouble(dr["RoomPrice"]);
                                        objRoom.ConvertedRoomPriceTotal = Math.Round(ConvertedRoomPrice1 * roomcountvaaal);
                                    }
                                    else
                                    {
                                        objRoom.ConvertedRoomPriceTotal = Convert.ToDouble(dr["RoomPrice"]);
                                    }

                                    objRoom.NewCurrencySymbol = dr["CurrencySymbol"].ToString();
                                }
                                SessionPrice += Convert.ToDouble(objRoom.ConvertedRoomPriceTotal);
                                objlist.Add(objRoom);
                            }
                        }
                    }
                }
                return objlist;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public HttpResponseMessage GetValuesWhileChangelangRe(string currentCurrency, string culture, string hotelId, string roomPriceTypeId, string checkDatefrom, string checkDateTo, string roomCountHId, string policyTypeId, string hotelRoomIdAry)
        {
            List<Home> list = new List<Home>();
            DateTime time = DateTime.ParseExact(checkDatefrom, "dd-MMM-yyyy", CultureInfo.InvariantCulture);
            DateTime time2 = DateTime.ParseExact(checkDateTo, "dd-MMM-yyyy", CultureInfo.InvariantCulture);
            TimeSpan span = (TimeSpan)(time2 - time);
            string str = span.TotalDays.ToString();
            span = (TimeSpan)(time2 - time);
            span.TotalHours.ToString();
            span = (TimeSpan)(time2 - time);
            span.TotalMinutes.ToString();
            string str2 = time.ToString("yyyy/MM/dd");
            time2.ToString("yyyy/MM/dd");
            string checkDateto = time2.AddDays(-1.0).ToString("yyyy/MM/dd");
            DataTable table = new DataTable();
            DataTable requiredMessageCode = new DataTable();
            new DataTable();
            string RoomTypeText = "";
            string RoomPriceText = "";
            string RoomMaxPeopleCount = "";
            string RoomCounttext = "";
            string Conditions = "";
            string GuestName = "";
            string BedPreference = "";
            string TravellerType = "";
            string EstimatedArrivalTime = "";
            string SpecialNote = "";
            string PayableAmount = "";
            string BookingTotalPriceWarning = "";
            string NonRefundableInfo = "";
            string Description = "";
            string RoutingName = "";
            string ID = "";
            string HotelClassValue = "";
            string CityName = "";
            string HotelStar = "";
            string Address = "";
            string CurrencyID = "";
            string CurrencySymbol = "";
            string HotelClass = "";
            decimal num = new decimal();
            int num2 = 0;
            int num3 = 0;
            double num4 = 0.0;
            double num5 = 0.0;
            string textMessagesAsString = this.homeService.GetTextMessagesAsString(culture, "booklabels");
            DataTable hotelBasicInfo = new DataTable();
            try
            {
                hotelBasicInfo = this.homeService.GetHotelBasicInfo(culture, hotelId);
                if (hotelBasicInfo.Rows.Count > 0)
                {
                    foreach (DataRow row in hotelBasicInfo.Rows)
                    {
                        row["ID"].ToString();
                        Description = new Home().TruncateLongString(row["Description"].ToString(), 200);
                        RoutingName = row["RoutingName"].ToString();
                        ID = URL.EXTRANET_URLFULL + "Photo/Hotel/" + row["ID"].ToString() + "/" + row["MainPhotoName"].ToString();
                        HotelClassValue = row["HotelClass"].ToString();
                        CityName = row["CityName"].ToString();
                        HotelStar = row["HotelStar"].ToString();
                        Address = row["Address"].ToString();
                        num2 = Convert.ToInt32(row["CreditCardNotRequired"]);
                        CurrencyID = row["CurrencyID"].ToString();
                        CurrencySymbol = row["CurrencySymbol"].ToString();
                        if (row["HotelClass"].ToString() == "OneStar")
                        {
                            HotelClass = "<img src=\"/Images/star-small-active.png\"border=\"0\">&nbsp;<img src=\"/Images/star-small-inactive.png\"border=\"0\">&nbsp;<img src=\"/Images/star-small-inactive.png\"border=\"0\">&nbsp;<img src=\"/Images/star-small-inactive.png\"border=\"0\">&nbsp;<img src=\"/Images/star-small-inactive.png\"border=\"0\">&nbsp;";
                        }
                        else if (row["HotelClass"].ToString() == "TwoStar")
                        {
                            HotelClass = "<img src=\"/Images/star-small-active.png\"border=\"0\">&nbsp;<img src=\"/Images/star-small-active.png\"border=\"0\">&nbsp;<img src=\"/Images/star-small-inactive.png\"border=\"0\">&nbsp;<img src=\"/Images/star-small-inactive.png\"border=\"0\">&nbsp;<img src=\"/Images/star-small-inactive.png\"border=\"0\">&nbsp;";
                        }
                        else if (row["HotelClass"].ToString() == "ThreeStar")
                        {
                            HotelClass = "<img src=\"/Images/star-small-active.png\"border=\"0\">&nbsp;<img src=\"/Images/star-small-active.png\"border=\"0\">&nbsp;<img src=\"/Images/star-small-active.png\"border=\"0\">&nbsp;<img src=\"/Images/star-small-inactive.png\"border=\"0\">&nbsp;<img src=\"/Images/star-small-inactive.png\"border=\"0\">&nbsp;";
                        }
                        else if (row["HotelClass"].ToString() == "FourStar")
                        {
                            HotelClass = "<img src=\"/Images/star-small-active.png\"border=\"0\">&nbsp;<img src=\"/Images/star-small-active.png\"border=\"0\">&nbsp;<img src=\"/Images/star-small-active.png\"border=\"0\">&nbsp;<img src=\"/Images/star-small-active.png\"border=\"0\">&nbsp;<img src=\"/Images/star-small-inactive.png\"border=\"0\">&nbsp;";
                        }
                        else if (row["HotelClass"].ToString() == "FiveStar")
                        {
                            HotelClass = "<img src=\"/Images/star-small-active.png\"border=\"0\">&nbsp;<img src=\"/Images/star-small-active.png\"border=\"0\">&nbsp;<img src=\"/Images/star-small-active.png\"border=\"0\">&nbsp;<img src=\"/Images/star-small-active.png\"border=\"0\">&nbsp;<img src=\"/Images/star-small-active.png\"border=\"0\">&nbsp;";
                        }
                        else
                        {
                            HotelClass = "Unrated";
                        }
                    }
                }
                requiredMessageCode = this.homeService.GetRequiredMessageCode(culture);
                if ((requiredMessageCode != null) && (requiredMessageCode.Rows.Count > 0))
                {
                    foreach (DataRow current in requiredMessageCode.Rows)
                    {
                        RoomTypeText = current["RoomType"].ToString();
                        RoomPriceText = current["RoomPrice"].ToString();
                        RoomMaxPeopleCount = current["RoomMaxPeopleCount"].ToString();
                        RoomCounttext = current["RoomCount"].ToString();
                        Conditions = current["Conditions"].ToString();
                        GuestName = current["GuestName"].ToString();
                        BedPreference = current["BedPreference"].ToString();
                        TravellerType = current["TravellerType"].ToString();
                        EstimatedArrivalTime = current["EstimatedArrivalTime"].ToString();
                        SpecialNote = current["SpecialNote"].ToString();
                        PayableAmount = current["PayableAmount"].ToString();
                        BookingTotalPriceWarning = current["BookingTotalPriceWarning"].ToString();
                        NonRefundableInfo = current["NonRefundableInfo"].ToString();

                    }
                }
                string currencyFrom = "";
                string str29 = "";
                double num7 = 0.0;
                if (roomPriceTypeId.Contains<char>(','))
                {
                    char[] separator = new char[] { ',' };
                    string[] strArray = roomPriceTypeId.Split(separator);
                    char[] chArray2 = new char[] { ',' };
                    string[] strArray2 = roomCountHId.Split(chArray2);
                    char[] chArray3 = new char[] { ',' };
                    string[] strArray3 = policyTypeId.Split(chArray3);
                    char[] chArray4 = new char[] { ',' };
                    string[] strArray4 = hotelRoomIdAry.Split(chArray4);
                    for (int i = 0; i < strArray.Length; i++)
                    {
                        table = this.homeService.GetvaluesWhileChangelang(culture, hotelId, strArray[i], str2, checkDateto, strArray3[i], strArray4[i], this.BizContext.ShowSecretDeals);
                        if ((table != null) && (table.Rows.Count > 0))
                        {
                            foreach (DataRow row2 in table.Rows)
                            {
                                Home item = new Home
                                {
                                    BedSelectionList = this.GetBedSelectionList(culture, hotelId, strArray4[i]),
                                    RoomCount = strArray2[i],
                                    OptionRoomCount = row2["RoomCount"].ToString(),
                                    NightCount = str,
                                    RoomTypeText = RoomTypeText,
                                    RoomPriceText = RoomPriceText,
                                    RoomMaxPeopleCount = RoomMaxPeopleCount,
                                    RoomCounttext = RoomCounttext,
                                    Conditions = Conditions,
                                    GuestName = GuestName,
                                    BedPreference = BedPreference,
                                    TravellerType = TravellerType,
                                    EstimatedArrivalTime = EstimatedArrivalTime,
                                    SpecialNote = SpecialNote,
                                    PayableAmount = PayableAmount,
                                    BookingTotalPriceWarning = BookingTotalPriceWarning,
                                    NonRefundableInfo = NonRefundableInfo,
                                    HotelClass = HotelClass,
                                    Description = Description,
                                    RoutingName = RoutingName,
                                    MainPhotoName = ID,
                                    RoomPhotoName = URL.EXTRANET_URLFULL + "Photo/Hotel/" + hotelId + "/" + row2["RoomPhotoName"].ToString()
                                };
                                string str30 = textMessagesAsString;
                                item.LblId = str30;
                                item.HotelClassValue = HotelClassValue;
                                item.Cityname = CityName;
                                item.HotelStar = HotelStar;
                                item.Address = Address;
                                item.CurrencyID = CurrencyID;
                                item.CurrencySymbol = CurrencySymbol;
                                item.CreditCardNotRequired = num2;
                                string str31 = "";
                                for (int j = 0; j < Convert.ToInt32(row2["MaxPeopleCount"]); j++)
                                {
                                    str31 = str31 + "<span class=\"glyphicon glyphicon-user\"></span>";
                                }
                                item.MaxPeopleCount = str31;
                                item.MaxPeopleCountval = row2["MaxPeopleCount"].ToString();
                                num3 += Convert.ToInt32(row2["MaxPeopleCount"]) * Convert.ToInt32(strArray2[i]);
                                item.RoomDataTableType = "RoomDetails";
                                item.CheckDatefrom = checkDatefrom;
                                item.CheckDateto = checkDateTo;
                                item.PriceType = row2["PriceType"].ToString();
                                item.HotelID = row2["HotelID"].ToString();
                                item.HotelRoomID = row2["ID"].ToString();
                                item.HotelRoomDescription = row2["RoomDescription"].ToString();
                                item.HotelTypeName = row2["RoomTypeName"].ToString();
                                item.SmokingTypeName = row2["SmokingTypeName"].ToString();
                                item.RoomCount = strArray2[i];
                                decimal num11 = Convert.ToDecimal(row2["RoomPrice"]) * Convert.ToInt32(strArray2[i]);
                                num5 += Convert.ToDouble(num11);
                                num += num11;
                                item.RoomPriceTotal = num.ToString();
                                item.RoomPrice = row2["RoomPrice"].ToString();
                                item.HotelName = RoutingName;
                                item.CurrencyCode = row2["CurrencyCode"].ToString();
                                item.RoomMaxPeopleCount = row2["RoomMaxPeopleCount"].ToString();
                                item.FREEcancellation = row2["FREEcancellation"].ToString();
                                item.RoomFacilities = row2["RoomFacilities"].ToString();
                                item.UniqueID = row2["UniqueID"].ToString();
                                item.AccommodationTypeID = row2["AccommodationTypeID"].ToString();
                                item.AccommodationTypeName = row2["AccommodationTypeName"].ToString();
                                item.AccommodationTypeDescription = row2["AccommodationTypeDescription"].ToString();
                                item.PricePolicyTypeID = row2["PricePolicyTypeID"].ToString();
                                if (row2["PricePolicyTypeID"].ToString() == "1")
                                {
                                    DataTable cancellationPolicy = new DataTable();
                                    string str32 = "";
                                    cancellationPolicy = new Home().GetCancellationPolicy(hotelId, culture);
                                    if (cancellationPolicy.Rows.Count > 0)
                                    {
                                        foreach (DataRow row3 in cancellationPolicy.Rows)
                                        {
                                            str32 = row2["RefundableInfo"].ToString();
                                            string str33 = str32.Replace("#Days#", row3["RefundableDayCount"].ToString()).Replace("#Penalty#", row3["PenaltyRateTypeName"].ToString());
                                            item.RoomTypeInfo = str33;
                                        }
                                    }
                                    else
                                    {
                                        item.RoomTypeInfo = str32;
                                    }
                                }
                                else if (row2["PricePolicyTypeID"].ToString() == "2")
                                {
                                    item.RoomTypeInfo = row2["NonRefundableInfo"].ToString();
                                }
                                else
                                {
                                    item.RoomTypeInfo = "";
                                }
                                item.PricePolicyTypeName = row2["PricePolicyTypeName"].ToString();
                                item.SingleRate = row2["SingleRate"].ToString();
                                item.DoubleRate = row2["DoubleRate"].ToString();
                                item.DailyRoomPrices = row2["DailyRoomPrices"].ToString();
                                item.OriginalRoomPrice = row2["OriginalRoomPrice"].ToString();
                                item.CurrencyID = row2["CurrencyID"].ToString();
                                item.CurrencySymbol = row2["CurrencySymbol"].ToString();
                                if ((currentCurrency != "") && (currentCurrency != row2["CurrencyCode"].ToString()))
                                {
                                    if (currencyFrom != row2["CurrencyCode"].ToString())
                                    {
                                        currencyFrom = row2["CurrencyCode"].ToString();
                                        string[] textArray1 = this.homeService.CurrencyConvertion(currencyFrom, currentCurrency).Split(null);
                                        string str34 = textArray1[0];
                                        str29 = textArray1[1];
                                        num7 = Convert.ToDouble(str34);
                                        double num13 = Convert.ToDouble(num);
                                        double a = Convert.ToDouble(row2["RoomPrice"]) * num7;
                                        double num15 = num13 * num7;
                                        item.ConvertedRoomPrice = Math.Round(a);
                                        item.ConvertedRoomPriceTotal = Math.Round(num15);
                                        item.NewCurrencySymbol = str29;
                                    }
                                    else
                                    {
                                        double num16 = Convert.ToDouble(num);
                                        double num17 = Convert.ToDouble(row2["RoomPrice"]) * num7;
                                        double num18 = num16 * num7;
                                        item.ConvertedRoomPrice = Math.Round(num17);
                                        item.ConvertedRoomPriceTotal = Math.Round(num18);
                                        item.NewCurrencySymbol = str29;
                                    }
                                }
                                else
                                {
                                    item.ConvertedRoomPrice = Convert.ToDouble(row2["RoomPrice"]);
                                    item.ConvertedRoomPriceTotal = Convert.ToDouble(num);
                                    item.NewCurrencySymbol = row2["CurrencySymbol"].ToString();
                                }
                                num4 += Convert.ToDouble(item.ConvertedRoomPrice) * Convert.ToInt32(strArray2[i]);
                                list.Add(item);
                            }
                        }
                    }
                }
                else
                {
                    table = this.homeService.GetvaluesWhileChangelang(culture, hotelId, roomPriceTypeId, str2, checkDateto, policyTypeId, hotelRoomIdAry, this.BizContext.ShowSecretDeals);
                    if ((table != null) && (table.Rows.Count > 0))
                    {
                        foreach (DataRow row4 in table.Rows)
                        {
                            int num19 = 1;
                            Home home2 = new Home
                            {
                                BedSelectionList = this.GetBedSelectionList(culture, hotelId, hotelRoomIdAry),
                                RoomCount = roomCountHId
                            };
                            num19 = Convert.ToInt32(roomCountHId);
                            num3 += Convert.ToInt32(row4["MaxPeopleCount"]) * Convert.ToInt32(roomCountHId);
                            home2.OptionRoomCount = row4["RoomCount"].ToString();
                            home2.NightCount = str;
                            home2.RoomTypeText = RoomTypeText;
                            home2.RoomPriceText = RoomPriceText;
                            home2.RoomMaxPeopleCount = RoomMaxPeopleCount;
                            home2.RoomCounttext = RoomCounttext;
                            home2.Conditions = Conditions;
                            home2.GuestName = GuestName;
                            home2.BedPreference = BedPreference;
                            home2.TravellerType = TravellerType;
                            home2.EstimatedArrivalTime = EstimatedArrivalTime;
                            home2.SpecialNote = SpecialNote;
                            home2.PayableAmount = PayableAmount;
                            home2.BookingTotalPriceWarning = BookingTotalPriceWarning;
                            home2.NonRefundableInfo = NonRefundableInfo;
                            home2.HotelClass = HotelClass;
                            home2.Description = Description;
                            home2.RoutingName = RoutingName;
                            home2.MainPhotoName = ID;
                            home2.RoomPhotoName = URL.EXTRANET_URLFULL + "Photo/Hotel/" + hotelId + "/" + row4["RoomPhotoName"].ToString();
                            string str35 = textMessagesAsString;
                            home2.LblId = str35;
                            home2.HotelClassValue = HotelClassValue;
                            home2.Cityname = CityName;
                            home2.HotelStar = HotelStar;
                            home2.Address = Address;
                            home2.CurrencyID = CurrencyID;
                            home2.CurrencySymbol = CurrencySymbol;
                            home2.CreditCardNotRequired = num2;
                            if (row4["MaxPeopleCount"].ToString() == "1")
                            {
                                home2.MaxPeopleCount = "<span class=\"glyphicon glyphicon-user\"></span>";
                            }
                            else if (row4["MaxPeopleCount"].ToString() == "2")
                            {
                                home2.MaxPeopleCount = "<span class=\"glyphicon glyphicon-user\"></span><span class=\"glyphicon glyphicon-user\"></span>";
                            }
                            else if (row4["MaxPeopleCount"].ToString() == "3")
                            {
                                home2.MaxPeopleCount = "<span class=\"glyphicon glyphicon-user\"></span><span class=\"glyphicon glyphicon-user\"></span><span class=\"glyphicon glyphicon-user\"></span>";
                            }
                            else if (row4["MaxPeopleCount"].ToString() == "4")
                            {
                                home2.MaxPeopleCount = "<span class=\"glyphicon glyphicon-user\"></span><span class=\"glyphicon glyphicon-user\"></span><span class=\"glyphicon glyphicon-user\"></span><span class=\"glyphicon glyphicon-user\"></span>";
                            }
                            else
                            {
                                home2.MaxPeopleCount = "";
                            }
                            home2.MaxPeopleCountval = row4["MaxPeopleCount"].ToString();
                            home2.RoomDataTableType = "RoomDetails";
                            home2.CheckDatefrom = checkDatefrom;
                            home2.CheckDateto = checkDateTo;
                            home2.PriceType = row4["PriceType"].ToString();
                            home2.HotelID = row4["HotelID"].ToString();
                            home2.HotelRoomID = row4["ID"].ToString();
                            home2.HotelRoomDescription = row4["RoomDescription"].ToString();
                            home2.HotelTypeName = row4["RoomTypeName"].ToString();
                            home2.SmokingTypeName = row4["SmokingTypeName"].ToString();
                            home2.RoomPrice = row4["RoomPrice"].ToString();
                            num5 = Convert.ToDouble(row4["RoomPrice"]) * num19;
                            home2.RoomPriceTotal = num5.ToString();
                            home2.HotelName = RoutingName;
                            home2.CurrencyCode = row4["CurrencyCode"].ToString();
                            home2.RoomMaxPeopleCount = row4["RoomMaxPeopleCount"].ToString();
                            home2.FREEcancellation = row4["FREEcancellation"].ToString();
                            home2.RoomFacilities = row4["RoomFacilities"].ToString();
                            home2.UniqueID = row4["UniqueID"].ToString();
                            home2.AccommodationTypeID = row4["AccommodationTypeID"].ToString();
                            home2.AccommodationTypeName = row4["AccommodationTypeName"].ToString();
                            home2.AccommodationTypeDescription = row4["AccommodationTypeDescription"].ToString();
                            home2.PricePolicyTypeID = row4["PricePolicyTypeID"].ToString();
                            if (row4["PricePolicyTypeID"].ToString() == "1")
                            {
                                DataTable table5 = new DataTable();
                                string str36 = "";
                                table5 = new Home().GetCancellationPolicy(hotelId, culture);
                                if (table5.Rows.Count > 0)
                                {
                                    foreach (DataRow row5 in table5.Rows)
                                    {
                                        str36 = row4["RefundableInfo"].ToString();
                                        string str37 = str36.Replace("#Days#", row5["RefundableDayCount"].ToString()).Replace("#Penalty#", row5["PenaltyRateTypeName"].ToString());
                                        home2.RoomTypeInfo = str37;
                                    }
                                }
                                else
                                {
                                    home2.RoomTypeInfo = str36;
                                }
                            }
                            else if (row4["PricePolicyTypeID"].ToString() == "2")
                            {
                                home2.RoomTypeInfo = row4["NonRefundableInfo"].ToString();
                            }
                            else
                            {
                                home2.RoomTypeInfo = "";
                            }
                            home2.PricePolicyTypeName = row4["PricePolicyTypeName"].ToString();
                            home2.SingleRate = row4["SingleRate"].ToString();
                            home2.DoubleRate = row4["DoubleRate"].ToString();
                            home2.DailyRoomPrices = row4["DailyRoomPrices"].ToString();
                            home2.OriginalRoomPrice = row4["OriginalRoomPrice"].ToString();
                            home2.CurrencyID = row4["CurrencyID"].ToString();
                            home2.CurrencySymbol = row4["CurrencySymbol"].ToString();
                            if ((currentCurrency != "") && (currentCurrency != row4["CurrencyCode"].ToString()))
                            {
                                if (currencyFrom != row4["CurrencyCode"].ToString())
                                {
                                    currencyFrom = row4["CurrencyCode"].ToString();
                                    string[] textArray2 = this.homeService.CurrencyConvertion(currencyFrom, currentCurrency).Split(null);
                                    string str38 = textArray2[0];
                                    str29 = textArray2[1];
                                    num7 = Convert.ToDouble(str38);
                                    double num20 = Convert.ToDouble(row4["RoomPrice"]) * num7;
                                    home2.ConvertedRoomPrice = Math.Round(num20);
                                    if (num19 > 1)
                                    {
                                        home2.ConvertedRoomPriceTotal = Math.Round((double)(num20 * num19));
                                    }
                                    else
                                    {
                                        home2.ConvertedRoomPriceTotal = Math.Round(num20);
                                    }
                                    home2.NewCurrencySymbol = str29;
                                }
                                else
                                {
                                    double num21 = Convert.ToDouble(row4["RoomPrice"]) * num7;
                                    home2.ConvertedRoomPrice = Math.Round(num21);
                                    if (num19 > 1)
                                    {
                                        home2.ConvertedRoomPriceTotal = Math.Round((double)(num21 * num19));
                                    }
                                    else
                                    {
                                        home2.ConvertedRoomPriceTotal = Math.Round(num21);
                                    }
                                    home2.NewCurrencySymbol = str29;
                                }
                            }
                            else
                            {
                                home2.ConvertedRoomPrice = Convert.ToDouble(row4["RoomPrice"]);
                                if (num19 > 1)
                                {
                                    double num22 = Convert.ToDouble(row4["RoomPrice"]);
                                    home2.ConvertedRoomPriceTotal = Math.Round((double)(num22 * num19));
                                }
                                else
                                {
                                    home2.ConvertedRoomPriceTotal = Convert.ToDouble(row4["RoomPrice"]);
                                }
                                home2.NewCurrencySymbol = row4["CurrencySymbol"].ToString();
                            }
                            num4 += Convert.ToDouble(home2.ConvertedRoomPriceTotal);
                            list.Add(home2);
                        }
                    }
                }
                double total = 0.0;
                double totalInHotelCurrency = 0.0;
                List<Home> list2 = this.GetHotelTax(culture, hotelId.ToString(), currentCurrency.ToString(), time.ToString("dd-MMM-yyyy"), time2.ToString("dd-MMM-yyyy"), num3, num4, num5, out total, out totalInHotelCurrency);
                return base.Request.CreateResponse(HttpStatusCode.OK, new { list = list, HotelTax = list2, convertedRoomPriceTotal = num4, total = total, totalInHotelCurrency = totalInHotelCurrency });
            }
            catch (Exception exception)
            {
                base.LogError(exception);
                return null;
            }
        }

        public List<Home> GetBedSelectionList(string Cultureid, string HotelID, string HotelRoomID)
        {
            Home homeobj = new Home();
            List<Home> list = new List<Home>();
            DataTable dt = new DataTable();
            string optionNo = "";
            string BedText = "";
            int countValue = 0;
            try
            {
                string SelectValue = homeobj.GetTextMessagesAsString(Cultureid, "Select");
                dt = homeobj.getBedSelection(Cultureid, HotelID, HotelRoomID);
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        Home obj1 = new Home();
                        foreach (DataRow dr in dt.Rows)
                        {
                            Home obj = new Home();

                            if (optionNo != dr["OptionNo"].ToString())
                            {

                                countValue++;
                                if (BedText == "")
                                {
                                    BedText = dr["BedTypeNameWithCount"].ToString();
                                }
                                else
                                {
                                    obj.BedTypeNameWithCount = BedText;
                                    obj.BedTypeID = optionNo;
                                    obj.SelectText = SelectValue;
                                    list.Add(obj);
                                    BedText = "";
                                    BedText = dr["BedTypeNameWithCount"].ToString();
                                    //BedText = BedText + "  <b>or</b>  " + dr["BedTypeNameWithCount"].ToString();
                                }
                                optionNo = dr["OptionNo"].ToString();
                            }
                            else
                            {
                                BedText = BedText + "," + dr["BedTypeNameWithCount"].ToString();
                            }

                            //obj.BedTypeID = dr["BedTypeID"].ToString();
                            ////Session["SalutationTypeID"] = dr["ID"].ToString();
                            //obj.BedTypeNameWithCount = dr["BedTypeNameWithCount"].ToString();
                            //list.Add(obj);
                        }
                        obj1.BedTypeNameWithCount = BedText;
                        obj1.BedTypeID = optionNo;
                        obj1.SelectText = SelectValue;
                        list.Add(obj1);
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return list;
        }

        public static string Encrypt128New(string clearText, string key, string IV)
        {
            //string EncryptionKey = "MAKV2SPBNI99212";
            try
            {
                byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
                using (Aes encryptor = Aes.Create())
                {

                    encryptor.Key = Encoding.UTF8.GetBytes(key);
                    encryptor.IV = Encoding.UTF8.GetBytes(IV);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(clearBytes, 0, clearBytes.Length);
                            cs.Close();
                        }
                        clearText = Convert.ToBase64String(ms.ToArray());
                    }
                }
            }
            catch (Exception ex)
            {
            }

            return clearText;
        }

        public string Decrypt128New(string cipherText, string key, string IV)
        {
            //  string EncryptionKey = "MAKV2SPBNI99212";
            try
            {
                byte[] cipherBytes = Convert.FromBase64String(cipherText);
                using (Aes encryptor = Aes.Create())
                {
                    encryptor.Key = Encoding.UTF8.GetBytes(key);
                    encryptor.IV = Encoding.UTF8.GetBytes(IV);

                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(cipherBytes, 0, cipherBytes.Length);
                            cs.Close();
                        }
                        cipherText = Encoding.Unicode.GetString(ms.ToArray());
                    }
                }

            }
            catch (Exception ex)
            {

            }
            return cipherText;
        }

        [HttpPost]
        public HttpResponseMessage HotelReservationFunc(
                string CVCCode
                , string CheckOutDate
                , string CheckinDate
                , string Country
                , int CreditCardNotRequired
                , string CreditcardNumber
                , string CreditcardProvider
                , string Culturecode
                , string Email
                , string EstimatedArrivalTimeSelect
                , string ExpirationMonth
                , string ExpirationYear
                , string Guestname
                , string HasDiscount
                , string HiidenCurrencyCode
                , string HotelID
                , string HotelName
                , string HotelRoomID
                , string LoggedUserID
                , string Name
                , string NameOnCreditcard
                , string PayableAmount
                , string Phone
                , string PricePolicyType
                , string PriceType
                , string RoomCount
                , string SelectBedType
                , string SelectBedTypeID
                , string SpecialNote
                , string SurName
                , string TavellerTypeSelect
                , string TavellerTypeSelectID
                , string TotalPricewithTax
                , string culture
                , string title
             )
        {
            List<Home> list = new List<Home>();
            try
            {
                if (NameOnCreditcard == "")
                {
                    NameOnCreditcard = Name;
                }

                Home homeobj = new Home();
                Encryption64 objEncrypt = new Encryption64();
                string ReservationID = "";
                string hostName = Dns.GetHostName();
                string myIP = Dns.GetHostByName(hostName).AddressList[0].ToString();
                string UserID = LoggedUserID;
                DataTable MailTemplate = new DataTable();

                DataTable HotelInfo = new DataTable();

                DataTable HotelRoomInfo = new DataTable();


                string CardExpriryDate = ExpirationMonth + "/" + ExpirationYear;
                if (string.IsNullOrEmpty(LoggedUserID))
                {
                    string UserIDNew = homeobj.InsertNewUserWhileReservation(title, Name, SurName, Email, Phone, Country, myIP);
                    UserID = UserIDNew;
                }

                var chars = "01234ABCDEFGHIJK56789LMNOPQRSTUVWXYZ";
                var random = new Random();
                var PinCode = "";
                int count = 1;
                while (count == 1)
                {
                    PinCode = new string(
                   Enumerable.Repeat(chars, 8)
                             .Select(s => s[random.Next(s.Length)])
                             .ToArray());
                    count = homeobj.CheckPincode(PinCode);
                }
                string mailTemplateID = "";
                string mailFrom = "";
                string mailSubject = "";
                string mailBody = "";
                string HotelAddress = "";
                string HotelContactInfo = "";
                string HotelPromotion = "";
                string MaxpeopleCount = "";
                string RoomInfo = "";
                string RoomInfoInHotelCulture = "";
                string HotelCulture = "";
                string HotelEmail = "";
                string DailyRoomPrices = "";
                string HotelFirmID = "";
                string CurrencyCode = "";

                HotelInfo = homeobj.GetHotelBasicInfo(Culturecode, HotelID);
                if (HotelInfo != null)
                {
                    if (HotelInfo.Rows.Count > 0)
                    {
                        foreach (DataRow dr in HotelInfo.Rows)
                        {
                            HotelAddress = dr["Address"].ToString();
                            HotelContactInfo = dr["Phone"].ToString() + ",  " + dr["Email"].ToString();
                            HotelCulture = dr["HotelCultureCode"].ToString();

                            HotelEmail = dr["Email"].ToString();
                            HotelFirmID = dr["FirmID"].ToString();

                            CurrencyCode = dr["CurrencySymbol"].ToString();
                            //HiidenCurrencyCode = dr["CurrencyCode"].ToString();
                        }
                    }
                }

                DateTime dt = DateTime.ParseExact(CheckinDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture);
                DateTime dt1 = DateTime.ParseExact(CheckOutDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture);
                DateTime CheckOutDT = dt1.AddDays(-1);
                double Datdifffff = (dt1 - dt).TotalDays;
                string NoofNights = Datdifffff.ToString();
                string Datefromm = dt.ToString("yyyy/MM/dd");
                string Datetoo = dt1.ToString("yyyy/MM/dd");
                string CheckoutDate = CheckOutDT.ToString("yyyy/MM/dd");
                string HotelAccommodationTypeID = "";
                string PricePolicyTypeID = "";
                string SingleRate = "";
                string DoubleRate = "";
                string RoomPrice = "";

                string CancelpolicyID = "";
                string Nonrefund = "0";

                string RoomID = "";
                int TotalRoomCount = 0;
                double TotalPayableAmount = 0;

                double ComissionRate = 0;
                double ComissionAmount = 0;
                DataTable ComissionsTable = new DataTable();
                DateTime CurrentDate = DateTime.Now;
                string NowDateFormat = CurrentDate.ToString("yyyy/MM/dd");
                ComissionsTable = homeobj.GetHotelComissions(Culturecode, HotelID, NowDateFormat);
                if (ComissionsTable.Rows.Count > 0)
                {
                    foreach (DataRow drComission in ComissionsTable.Rows)
                    {
                        try
                        {
                            ComissionRate = Convert.ToDouble(drComission["Comission"]);
                        }
                        catch
                        {

                        }
                    }
                }


                List<Home> objlist = new List<Home>();
                int TotalSelectedRoomCount = 0;
                int TotalPeopleCount = 0;

                string[] AryRoomCount = RoomCount.Split(',');
                string[] AryPriceType = PriceType.Split(',');
                if(string.IsNullOrEmpty(Convert.ToString(Guestname)))
                { Guestname = ""; }
                string[] AryGuestname = Guestname.Split(',');
                string[] ArySelectBedType = SelectBedType.Split(',');
                string[] AryTavellerTypeSelect = TavellerTypeSelect.Split(',');
                string[] AryEstimatedArrivalTimeSelect = EstimatedArrivalTimeSelect.Split(',');
                string[] AryHotelRoomID = HotelRoomID.Split(',');
                string[] ArySelectBedTypeID = SelectBedTypeID.Split(',');
                string[] AryTavellerTypeSelectID = TavellerTypeSelectID.Split(',');
                string[] AryPricePolicyType = PricePolicyType.Split(',');


                for (int u = 0; u < AryRoomCount.Length; u++)
                {

                    if (AryTavellerTypeSelectID[u] == "0")
                    {
                        AryTavellerTypeSelect[u] = "";
                    }
                    if (ArySelectBedTypeID[u] == "0")
                    {
                        ArySelectBedType[u] = "";
                    }
                    if (  AryGuestname[u] == "")
                    {
                        AryGuestname[u] = Name + " " + SurName;
                    }
                    TotalSelectedRoomCount += Convert.ToInt32(AryRoomCount[u]);
                    // PricePolicyTypeID = AryPricePolicyType[u];
                    try
                    {
                        objlist = GetValuesWhileChangelang_local(HiidenCurrencyCode, Culturecode, HotelID, PriceType, CheckinDate, CheckOutDate, RoomCount, PricePolicyType, HotelRoomID);
                        if (objlist != null)
                        {
                            foreach (Home item in objlist)
                            {
                                if (item.HotelRoomID == AryHotelRoomID[u] && item.PriceType == AryPriceType[u])
                                {
                                    MaxpeopleCount = item.MaxPeopleCountval;
                                    TotalPeopleCount += Convert.ToInt32(item.MaxPeopleCountval);
                                    string HotelTypename = item.HotelTypeName;
                                    string AccommodationType = item.AccommodationTypeName;
                                    HotelAccommodationTypeID = item.AccommodationTypeID;
                                    PricePolicyTypeID = item.PricePolicyTypeID;
                                    CurrencyCode = item.CurrencySymbol;
                                    HiidenCurrencyCode = item.CurrencyCode;
                                    // NewCurrencyCode = item.NewCurrencySymbol;
                                    DailyRoomPrices = item.DailyRoomPrices;
                                    SingleRate = item.SingleRate;
                                    DoubleRate = item.DoubleRate;
                                    RoomID = item.HotelRoomID;
                                    PayableAmount = item.RoomPrice.ToString();
                                    TotalRoomCount = Convert.ToInt32(item.OptionRoomCount);
                                    TotalPayableAmount = Convert.ToDouble(item.RoomPriceTotal);
                                    if (TotalPricewithTax == "")
                                    {
                                        TotalPricewithTax = item.ConvertedRoomPriceTotal.ToString();
                                    }

                                    //  TotalPayableAmount = TotalPayableAmount * TotalRoomCount;
                                    string GuestName = homeobj.GetTextMessagesAsString(Culturecode, "GuestName");
                                    string PeopleCount = homeobj.GetTextMessagesAsString(Culturecode, "PeopleCount");
                                    string BedPreference = homeobj.GetTextMessagesAsString(Culturecode, "BedPreference");
                                    string TotalRoomPrice = homeobj.GetTextMessagesAsString(Culturecode, "TotalRoomPrice");
                                    string NightPrice = homeobj.GetTextMessagesAsString(Culturecode, "NightPrice");
                                    string TravellerType = homeobj.GetTextMessagesAsString(Culturecode, "TravellerType");
                                    string EstimatedArrivalTime = homeobj.GetTextMessagesAsString(Culturecode, "EstimatedArrivalTime");
                                    string CancelPolicy = "";
                                    string CancelPolicyText = homeobj.GetTextMessagesAsString(Culturecode, "CancelPolicy");
                                    string AccommodationTypeText = homeobj.GetTextMessagesAsString(Culturecode, "Accommodation Type");
                                    Home objCancel = new Home();
                                    DataTable dtcancel = new DataTable();
                                    dtcancel = objCancel.GetCancellationPolicy(HotelID, Culturecode);
                                    if (dtcancel.Rows.Count > 0)
                                    {
                                        foreach (DataRow drcancel in dtcancel.Rows)
                                        {
                                            CancelpolicyID = drcancel["ID"].ToString();
                                        }

                                    }
                                    CancelPolicy = item.RoomTypeInfo;



                                    RoomInfo += "<tr height='25px'><td colspan='3' align='left' style='background-color:navy;color:white'>" + HotelTypename + "</td></tr>" +
                                        "<tr height='20px'><td width='25%' align='left'><b>" + AccommodationTypeText + "</b></td><td>" + "" + "</td><td width='75%' align='left'>" + AccommodationType + "</td></tr>" +
                                        "<tr height='20px'><td align='left'><b>" + GuestName + "</b></td><td>" + "" + "</td><td align='left'>" + AryGuestname[u] + "</td></tr>" +
                                        "<tr height='20px'><td align='left'><b>" + PeopleCount + "</b></td><td>" + "" + "</td><td align='left'>" + MaxpeopleCount + "</td></tr>" +
                                        "<tr height='20px'><td align='left'><b>" + BedPreference + "</b></td><td>" + "" + "</td><td align='left'>" + ArySelectBedType[u] + "</td></tr>" +
                                        "<tr height='20px'><td align='left'><b>" + TotalRoomPrice + "</b></td><td>" + "" + "</td><td align='left'>" + CurrencyCode + "  " + (Convert.ToDouble(PayableAmount) * Convert.ToDouble(AryRoomCount[u])) + "</td></tr>" +
                                        "<tr height='20px'><td align='left'><b>" + NightPrice + "</b></td><td>" + "" + "</td><td align='left'>" + CurrencyCode + "  " + PayableAmount + "</td></tr>" +
                                        "<tr height='20px'><td align='left'><b>" + TravellerType + "</b></td><td>" + "" + "</td><td align='left'>" + AryTavellerTypeSelect[u] + "</td></tr>" +
                                        "<tr height='20px'><td align='left'><b>" + EstimatedArrivalTime + "</b></td><td>" + "" + "</td><td align='left'>" + AryEstimatedArrivalTimeSelect[u] + "</td></tr>" +
                                        "<tr height='20px'><td align='left'><b>" + CancelPolicyText + "</b></td><td>" + "" + "</td><td align='left'>" + CancelPolicy + "</td></tr>" +
                                        "<tr><td colspan='3'>" + " " + "</td></tr>";
                                    GuestName = homeobj.GetTextMessagesAsString(HotelCulture, "GuestName");
                                    PeopleCount = homeobj.GetTextMessagesAsString(HotelCulture, "PeopleCount");
                                    BedPreference = homeobj.GetTextMessagesAsString(HotelCulture, "BedPreference");
                                    TotalRoomPrice = homeobj.GetTextMessagesAsString(HotelCulture, "TotalRoomPrice");
                                    NightPrice = homeobj.GetTextMessagesAsString(HotelCulture, "NightPrice");
                                    TravellerType = homeobj.GetTextMessagesAsString(HotelCulture, "TravellerType");
                                    EstimatedArrivalTime = homeobj.GetTextMessagesAsString(HotelCulture, "EstimatedArrivalTime");
                                    CancelPolicyText = homeobj.GetTextMessagesAsString(HotelCulture, "CancelPolicy");
                                    AccommodationTypeText = homeobj.GetTextMessagesAsString(HotelCulture, "Accommodation Type");

                                    List<Home> objlist1 = new List<Home>();
                                    string AccommodationTypeinHotelCulture = "";
                                    string RefundableInfoinHotelCulture = "";
                                    string HotelTypenameinHotelCulture = "";

                                     if (item.HotelRoomID == AryHotelRoomID[u] && item.PriceType == AryPriceType[u])
                                     {
                                         AccommodationTypeinHotelCulture = item.AccommodationTypeName;
                                         RefundableInfoinHotelCulture = item.RoomTypeInfo;
                                         HotelTypenameinHotelCulture = item.HotelTypeName;
                                     }
                                    //objlist1 = (List<Home>)Session["RoomDetailsForBookForHotelCurrency"];
                                    //if (objlist1 != null)
                                    //{
                                    //    foreach (Home item1 in objlist1)
                                    //    {
                                    //        if (item1.HotelRoomID == AryHotelRoomID[u] && item1.PriceType == AryPriceType[u])
                                    //        {
                                    //            AccommodationTypeinHotelCulture = item1.AccommodationTypeName;
                                    //            RefundableInfoinHotelCulture = item1.RoomTypeInfo;
                                    //            HotelTypenameinHotelCulture = item1.HotelTypeName;

                                    //        }
                                    //    }
                                    //}
                                    string BedTypeNameWithCount = "";
                                    string TravellerTypeinHotelCulture = "";
                                    using (BaseRepository baseRepo = new BaseRepository())
                                    {
                                        if (ArySelectBedTypeID[u] != "0")
                                        {
                                            DataRow[] roomBedInfo = BizHotel.GetHotelRoomBeds(baseRepo.BizDB, "", HotelCulture, "", RoomID).Select("OptionNo = " + ArySelectBedTypeID[u].ToString());
                                            foreach (DataRow dssdr in roomBedInfo)
                                            {
                                                Home objbed = new Home();
                                                BedTypeNameWithCount = BedTypeNameWithCount + dssdr["BedTypeNameWithCount"].ToString() + ",";
                                            }
                                            BizUtil.TrimEnd(ref BedTypeNameWithCount, ",");
                                        }
                                    }

                                    if (AryTavellerTypeSelectID[u] != "0")
                                    TravellerTypeinHotelCulture = homeobj.GetTravelllerTypeByCulture(HotelCulture, AryTavellerTypeSelectID[u]);

                                    RoomInfoInHotelCulture += "<tr height='25px'><td colspan='3' align='left' style='background-color:navy;color:white'>" + HotelTypenameinHotelCulture + "</td></tr>" +
                                   "<tr height='20px'><td width='25%' align='left'><b>" + AccommodationTypeText + "</b></td><td>" + "" + "</td><td width='75%' align='left'>" + AccommodationTypeinHotelCulture + "</td></tr>" +
                                   "<tr height='20px'><td align='left'><b>" + GuestName + "</b></td><td>" + "" + "</td><td align='left'>" + AryGuestname[u] + "</td></tr>" +
                                   "<tr height='20px'><td align='left'><b>" + PeopleCount + "</b></td><td>" + "" + "</td><td align='left'>" + MaxpeopleCount + "</td></tr>" +
                                   "<tr height='20px'><td align='left'><b>" + BedPreference + "</b></td><td>" + "" + "</td><td align='left'>" + BedTypeNameWithCount + "</td></tr>" +
                                   "<tr height='20px'><td align='left'><b>" + TotalRoomPrice + "</b></td><td>" + "" + "</td><td align='left'>" + CurrencyCode + "  " + (Convert.ToDouble(PayableAmount) * Convert.ToDouble(AryRoomCount[u])) + "</td></tr>" +
                                   "<tr height='20px'><td align='left'><b>" + NightPrice + "</b></td><td>" + "" + "</td><td align='left'>" + CurrencyCode + "  " + PayableAmount + "</td></tr>" +
                                   "<tr height='20px'><td align='left'><b>" + TravellerType + "</b></td><td>" + "" + "</td><td align='left'>" + TravellerTypeinHotelCulture + "</td></tr>" +
                                   "<tr height='20px'><td align='left'><b>" + EstimatedArrivalTime + "</b></td><td>" + "" + "</td><td align='left'>" + AryEstimatedArrivalTimeSelect[u] + "</td></tr>" +
                                   "<tr height='20px'><td align='left'><b>" + CancelPolicyText + "</b></td><td>" + "" + "</td><td align='left'>" + RefundableInfoinHotelCulture + "</td></tr>" +
                                   "<tr><td colspan='3'>" + " " + "</td></tr>";

                                }
                            }
                        }
                    }
                    catch
                    {



                    }


                    double NewCommisionAmt = Convert.ToDouble(TotalPricewithTax);
                    if (ComissionRate > 0 && NewCommisionAmt > 0)
                    {
                        ComissionAmount = (ComissionRate * NewCommisionAmt) / 100;
                    }
                    string PromotionID = "";
                    string HotelPromotionID = "";
                    string PromotionPercentage = "0";
                    string PromotionText = "";
                    string HotelPromotionText = "";
                    if (!string.IsNullOrEmpty(HasDiscount))
                    {
                        using (BaseRepository baseRepo = new BaseRepository())
                        {
                            DataTable HotelPromotions = BizHotel.GetHotelPromotions(baseRepo.BizDB, "ValidForAllRoomTypes DESC, PromotionSort", Culturecode, null, HotelID, null, DateTime.Now.Date.ToString(), "true", Datefromm, CheckoutDate, null, AryHotelRoomID[u]);
                            // DataTable HotelPromotionsHotelCulture = BizHotel.GetHotelPromotions(baseRepo.BizDB, "ValidForAllRoomTypes DESC, PromotionSort", Culturecode, null, HotelID, null, DateTime.Now.Date.ToString(), Datefromm, CheckoutDate, null, AryHotelRoomID[u]);
                            if (HotelPromotions.Rows.Count > 0)
                            {
                                foreach (DataRow HotelPromotionsRow in HotelPromotions.Rows)
                                {
                                    PromotionID = HotelPromotionsRow["PromotionID"].ToString();
                                    HotelPromotionID = HotelPromotionsRow["ID"].ToString();
                                    PromotionPercentage = HotelPromotionsRow["DiscountPercentage"].ToString();
                                    PromotionText = HotelPromotionsRow["PromotionDescription"].ToString();
                                }
                            }
                        }
                    }


                    if (RoomID == "")
                    {
                        RoomID = AryHotelRoomID[u];
                    }

                    int roomRemaining = 0;
                    try
                    {
                        if (TotalRoomCount > 0)
                        {
                            roomRemaining = TotalRoomCount - (TotalSelectedRoomCount);
                        }
                        if (roomRemaining < 0)
                        {
                            roomRemaining = 0;
                        }
                    }
                    catch
                    {

                    }
                    if (CreditCardNotRequired == 1)
                    {
                        CreditcardNumber = "";
                        CVCCode = "";
                        CardExpriryDate = "";
                    }
                    if (ReservationID == "")
                    {

                        ReservationID = homeobj.InsertReservationFunc(PinCode, title, Name, SurName, Email, Phone, Country, CreditcardProvider,
                               Encrypt128New(NameOnCreditcard, "2164285821854754", "5436265039712626"),
                               Encrypt128New(CreditcardNumber, "6164285828955421", "6485880454987489"),
                               Encrypt128New(CVCCode, "5267912096542731", "6359629697944359"),
                               Encrypt128New(CardExpriryDate, "5216428540391821", "6961584652179891"),
                               UserID, HotelFirmID, HotelID, TotalPricewithTax, myIP, Culturecode,
                               HiidenCurrencyCode, SpecialNote, roomRemaining, Datefromm, CheckoutDate, RoomID, Datetoo, HotelAccommodationTypeID, AryGuestname[u],
                               MaxpeopleCount, NoofNights, PricePolicyTypeID, SingleRate, DoubleRate, ArySelectBedTypeID[u], AryEstimatedArrivalTimeSelect[u],
                               AryTavellerTypeSelectID[u], TotalPricewithTax, CancelpolicyID, Nonrefund, ComissionRate, ComissionAmount, PromotionPercentage);

                    }

                    if (ReservationID != "")
                    {
                        string HotelRevervationID = homeobj.InsertHotelReservationFunc(ReservationID, PinCode, title, Name, SurName, Email, Phone, Country, CreditcardProvider,
                              Encrypt128New(NameOnCreditcard, "2164285821854754", "5436265039712626"),
                              Encrypt128New(CreditcardNumber, "6164285828955421", "6485880454987489"),
                              Encrypt128New(CVCCode, "5267912096542731", "6359629697944359"),
                              Encrypt128New(CardExpriryDate, "5216428540391821", "6961584652179891"),
                              UserID, HotelFirmID, HotelID, PayableAmount.ToString(), myIP, Culturecode,
                              HiidenCurrencyCode, SpecialNote, roomRemaining, Datefromm, CheckoutDate, RoomID, Datetoo, HotelAccommodationTypeID, AryGuestname[u],
                              MaxpeopleCount, NoofNights, PricePolicyTypeID, SingleRate, DoubleRate, ArySelectBedTypeID[u], AryEstimatedArrivalTimeSelect[u],
                              AryTavellerTypeSelectID[u], PayableAmount, CancelpolicyID, Nonrefund, ComissionRate, ComissionAmount, PromotionPercentage);

                        if (!string.IsNullOrEmpty(HasDiscount))
                        {
                            using (BaseRepository baseRepo = new BaseRepository())
                            {
                                try
                                {
                                    BizReservation.AddHotelReservationPromotion(baseRepo.BizDB, ReservationID, HotelRevervationID, PromotionID, HotelPromotionID, UserID);
                                }
                                catch
                                {

                                }

                            }
                        }
                        if (HotelRevervationID != "")
                        {
                            foreach (string roomPricesWithDates in DailyRoomPrices.Split(';'))
                            {
                                string[] roomPricesWithDates1 = roomPricesWithDates.Split('-');
                                homeobj.UpdateRoomRateAfterReservation(HotelRevervationID, roomPricesWithDates1[0], roomPricesWithDates1[1], HiidenCurrencyCode, UserID);
                            }
                        }
                    }
                }
                string HotelConditionText = "";
                string HotelConditionTextinHotelCulture = "";
                try
                {
                    Home ConObj = new Home();
                    DataTable HCondition = ConObj.GetHotelAttributesByReservationDate(Culturecode, HotelID);
                    if (HCondition.Rows.Count > 0)
                    {
                        foreach (DataRow dr in HCondition.Rows)
                        {
                            string ConditionDiscription = dr["AttributeName"].ToString();
                            if (dr["UnitValue"].ToString() != "")
                            {
                                ConditionDiscription += " " + "(" + dr["UnitName"].ToString() + ":" + " " + dr["UnitValue"].ToString() + ")";
                            }
                            if (dr["Charge"].ToString() != "")
                            {
                                ConditionDiscription += " " + "-" + dr["HotelUnitName"].ToString() + " " + dr["Charge"].ToString() + " " + dr["CurrencyName"].ToString();
                            }
                            HotelConditionText += "<li>" + ConditionDiscription + "</li>";
                        }
                    }
                }
                catch
                {

                }

                try
                {
                    Home ConObj = new Home();
                    DataTable HConditioninHotelCulture = ConObj.GetHotelAttributesByReservationDate(HotelCulture, HotelID);
                    if (HConditioninHotelCulture.Rows.Count > 0)
                    {
                        foreach (DataRow dr in HConditioninHotelCulture.Rows)
                        {
                            string ConditionDiscription = dr["AttributeName"].ToString();
                            if (dr["UnitValue"].ToString() != "")
                            {
                                ConditionDiscription += " " + "(" + dr["UnitName"].ToString() + ":" + " " + dr["UnitValue"].ToString() + ")";
                            }
                            if (dr["Charge"].ToString() != "")
                            {
                                ConditionDiscription += " " + "-" + dr["HotelUnitName"].ToString() + " " + dr["Charge"].ToString() + " " + dr["CurrencyName"].ToString();
                            }
                            HotelConditionTextinHotelCulture += "<li>" + ConditionDiscription + "</li>";
                        }
                    }
                }
                catch
                {

                }


                var EmailCount = 1;
                if (ReservationID != "")
                {




                    string EncryptedReservationID = System.Web.HttpContext.Current.Server.UrlEncode(ConvertStringToHex(objEncrypt.Encrypt(ReservationID, "58421043")));
                    string EncryptedPinCode = System.Web.HttpContext.Current.Server.UrlEncode(ConvertStringToHex(objEncrypt.Encrypt(PinCode, "58421043")));
                    string EncryptedHotelID = System.Web.HttpContext.Current.Server.UrlEncode(ConvertStringToHex(objEncrypt.Encrypt(HotelID, "58421043")));

                    int staus = homeobj.UpdatedReservation(ReservationID, EncryptedReservationID, EncryptedPinCode);
                    MailTemplate = homeobj.GetMailTemplates("SendHotelReservationEmail", Culturecode);


                    string DomainName = System.Configuration.ConfigurationManager.AppSettings["DomainName"];
                    string SMTPServerName = System.Configuration.ConfigurationManager.AppSettings["SMTPServerName"];
                    int Port = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SMTPPort"]);
                    string SMTPUsername = System.Configuration.ConfigurationManager.AppSettings["SMTPUsername"];
                    string SMTPPassword = System.Configuration.ConfigurationManager.AppSettings["SMTPPassword"];
                    if (MailTemplate != null)
                    {
                        if (MailTemplate.Rows.Count > 0)
                        {
                            foreach (DataRow dr in MailTemplate.Rows)
                            {
                                mailTemplateID = dr["ID"].ToString();
                                mailFrom = dr["MailFrom"].ToString();
                                mailSubject = dr["MailSubject"].ToString();
                                mailBody = dr["MailBody"].ToString();
                                mailSubject = mailSubject.Replace("#HotelName#", HotelName);
                                mailBody = mailBody.Replace("#ReservationID#", ReservationID);
                                mailBody = mailBody.Replace("#PinCode#", PinCode);
                                mailBody = mailBody.Replace("#ReservationOwnerFullName#", Name + " " + SurName);
                                mailBody = mailBody.Replace("#ReservationDate#", (DateTime.Now).ToString());
                                mailBody = mailBody.Replace("#HotelName#", HotelName);

                                mailBody = mailBody.Replace("http://www.Gbshotels.com/Hotel_en/fr/#HotelRoutingName#", DomainName + "#!/hoteldetail?hotelId=" + HotelID + "&hotelname=" + HotelName);
                                //mailBody = mailBody.Replace("#HotelRoutingName#", HotelName);
                                mailBody = mailBody.Replace("#HotelAddress#", HotelAddress);
                                mailBody = mailBody.Replace("#HotelContactInfo#", HotelContactInfo);
                                mailBody = mailBody.Replace("#CheckInDate#", CheckinDate);
                                mailBody = mailBody.Replace("#CheckOutDate#", CheckOutDate);
                                mailBody = mailBody.Replace("#NightCount#", NoofNights);
                                mailBody = mailBody.Replace("#ReservationOwnerEmail#", Email);
                                mailBody = mailBody.Replace("#ReservationOwnerPhone#", Phone);
                                mailBody = mailBody.Replace("#ReservationOwnerAddress#", "");
                                mailBody = mailBody.Replace("#RoomCount#", TotalSelectedRoomCount.ToString());
                                mailBody = mailBody.Replace("#PeopleCount#", TotalPeopleCount.ToString());
                                mailBody = mailBody.Replace("#Note#", SpecialNote);
                                mailBody = mailBody.Replace("#Amount#", CurrencyCode + " " + TotalPricewithTax);
                                mailBody = mailBody.Replace("#DiscountText#", String.Empty);
                                mailBody = mailBody.Replace("#PayableAmount#", CurrencyCode + " " + TotalPricewithTax);
                                mailBody = mailBody.Replace("#HotelCondition#", HotelConditionText);
                                mailBody = mailBody.Replace("#HotelPromotion#", HotelPromotion);
                                mailBody = mailBody.Replace("#EncReservationID#", EncryptedReservationID);
                                mailBody = mailBody.Replace("#EncPinCode#", EncryptedPinCode);
                                mailBody = mailBody.Replace("#EncHotelID#", EncryptedHotelID);
                                mailBody = mailBody.Replace("#CultureID#", Culturecode);
                                mailBody = mailBody.Replace("#RoomInfo#", RoomInfo);
                                mailBody = mailBody.Replace("#EncReservationID#", EncryptedReservationID);
                                mailBody = mailBody.Replace("#EncPinCode#", EncryptedPinCode);
                                mailBody = mailBody.Replace("#cid#", Culturecode);


                                if (EmailCount == 1)
                                {
                                    string MailTemplateID1 = dr["ID"].ToString();
                                    string MailFrom1 = dr["MailFrom"].ToString();
                                    string MailTo1 = Email;
                                    // string MailCC1="";
                                    string Subject1 = dr["MailSubject"].ToString();

                                    //string Body1 = dr["MailBody"].ToString();
                                    DateTime SendingDateTime1 = DateTime.Now;
                                    DateTime OpDateTime1 = DateTime.Now;
                                    long OpUserID1 = 0;

                                    using (BaseRepository baseRepo = new BaseRepository())
                                    {
                                        string Status = Convert.ToString(BizMail.AddMailForSending(baseRepo.BizDB, MailTemplateID1, MailFrom1, MailTo1, string.Empty, Subject1,
        mailBody, DateTime.Now, OpUserID1));
                                    }
                                }
                                EmailCount++;
                               // string DomainName = System.Configuration.ConfigurationManager.AppSettings["DomainName"];

                                 
                                mailBody = mailBody.Replace("http://www.Gbshotels.com/Home/HotelReservationsNew?", DomainName + "#!/manageReservation?");

                                System.Net.Mail.MailAddress from = new MailAddress(SMTPUsername, "GBS Hotels");
                                System.Net.Mail.MailAddress to = new MailAddress(Email);
                                System.Net.Mail.MailMessage m = new MailMessage(from, to);
                                //System.Net.Mail.MailMessage m = new System.Net.Mail.MailMessage(SenderMail, EmailId);
                                //StringBuilder sb = new StringBuilder();


                                m.IsBodyHtml = true;


                                //sb.Append(" \n\n");
                                ////sb.Append("You have requested to reset your password\n");
                                //sb.Append("Comments :" + Comments);
                                m.Subject = mailSubject;
                                m.Priority = System.Net.Mail.MailPriority.High;
                                SmtpClient smtp = new SmtpClient(SMTPServerName, Port);
                                smtp.UseDefaultCredentials = false;
                                smtp.EnableSsl = true;
                                // m.Body = sb.ToString();
                                m.BodyEncoding = System.Text.Encoding.GetEncoding("utf-8");
                                System.Net.Mail.AlternateView plainView = System.Net.Mail.AlternateView.CreateAlternateViewFromString
                                (System.Text.RegularExpressions.Regex.Replace(mailBody, @"<(.|\n)*?>", string.Empty), null, "text/plain");
                                System.Net.Mail.AlternateView htmlView = System.Net.Mail.AlternateView.CreateAlternateViewFromString(mailBody, null, "text/html");
                                m.AlternateViews.Add(plainView);
                                m.AlternateViews.Add(htmlView);
                                smtp.Credentials = new NetworkCredential(SMTPUsername, SMTPPassword);
                                try
                                {
                                    smtp.Send(m);
                                }
                                catch
                                {

                                }


                            }
                        }
                    }


                    DataTable MailTemplateForHotel = homeobj.GetMailTemplates("SendHotelReservationMadeEmail", HotelCulture);
                    int EmailCount1 = 1;
                    if (MailTemplateForHotel != null)
                    {
                        if (MailTemplateForHotel.Rows.Count > 0)
                        {
                            foreach (DataRow dr in MailTemplateForHotel.Rows)
                            {
                                mailTemplateID = dr["ID"].ToString();
                                mailFrom = dr["MailFrom"].ToString();
                                mailSubject = dr["MailSubject"].ToString();
                                mailBody = dr["MailBody"].ToString();

                                mailSubject = mailSubject.Replace("#HotelName#", HotelName);
                                mailBody = mailBody.Replace("#ReservationID#", ReservationID);
                                mailBody = mailBody.Replace("#ReservationOwnerFullName#", Name + " " + SurName);
                                mailBody = mailBody.Replace("#ReservationDate#", (DateTime.Now).ToString());
                                mailBody = mailBody.Replace("#HotelName#", HotelName);
                                mailBody = mailBody.Replace("#CheckInDate#", CheckinDate);
                                mailBody = mailBody.Replace("#CheckOutDate#", CheckOutDate);
                                mailBody = mailBody.Replace("#NightCount#", NoofNights);
                                mailBody = mailBody.Replace("#ReservationOwnerEmail#", Email);
                                mailBody = mailBody.Replace("#ReservationOwnerPhone#", Phone);
                                mailBody = mailBody.Replace("#ReservationOwnerAddress#", "");
                                mailBody = mailBody.Replace("#RoomCount#", TotalSelectedRoomCount.ToString());
                                mailBody = mailBody.Replace("#PeopleCount#", TotalPeopleCount.ToString());
                                mailBody = mailBody.Replace("#Note#", SpecialNote);
                                mailBody = mailBody.Replace("#Amount#", CurrencyCode + " " + TotalPricewithTax);
                                mailBody = mailBody.Replace("#DiscountText#", String.Empty);
                                mailBody = mailBody.Replace("#PayableAmount#", CurrencyCode + " " + TotalPricewithTax);
                                mailBody = mailBody.Replace("#HotelCondition#", HotelConditionTextinHotelCulture);
                                mailBody = mailBody.Replace("#HotelPromotion#", HotelPromotion);
                                mailBody = mailBody.Replace("#EncReservationID#", EncryptedReservationID);
                                mailBody = mailBody.Replace("#RoomInfo#", RoomInfoInHotelCulture);

                                if (EmailCount1 == 1)
                                {
                                    string MailTemplateID1 = dr["ID"].ToString();
                                    string MailFrom1 = dr["MailFrom"].ToString();
                                    string MailTo1 = Email;
                                    // string MailCC1="";
                                    string Subject1 = dr["MailSubject"].ToString();

                                    //string Body1 = dr["MailBody"].ToString();
                                    DateTime SendingDateTime1 = DateTime.Now;
                                    DateTime OpDateTime1 = DateTime.Now;
                                    long OpUserID1 = 0;

                                    using (BaseRepository baseRepo = new BaseRepository())
                                    {
                                        string Status = Convert.ToString(BizMail.AddMailForSending(baseRepo.BizDB, MailTemplateID1, MailFrom1, MailTo1, string.Empty, Subject1,
        mailBody, DateTime.Now, OpUserID1));
                                    }
                                }
                                EmailCount1++;

                                System.Net.Mail.MailAddress from = new MailAddress(SMTPUsername, "GBS Hotels");
                                string toEmail = HotelEmail;
                                // toEmail = "rajadurai@balstechnology.com";
                                //toEmail="abhishek@shinesoftcorp.com";
                                System.Net.Mail.MailAddress to = new MailAddress(toEmail);
                                System.Net.Mail.MailMessage m = new MailMessage(from, to);
                                //System.Net.Mail.MailMessage m = new System.Net.Mail.MailMessage(SenderMail, EmailId);
                                m.IsBodyHtml = true;


                                //sb.Append(" \n\n");
                                ////sb.Append("You have requested to reset your password\n");
                                //sb.Append("Comments :" + Comments);
                                m.Subject = mailSubject;
                                m.Priority = System.Net.Mail.MailPriority.High;
                                SmtpClient smtp = new SmtpClient(SMTPServerName, Port);
                                smtp.UseDefaultCredentials = false;
                                smtp.EnableSsl = true;
                                // m.Body = sb.ToString();
                                m.BodyEncoding = System.Text.Encoding.GetEncoding("utf-8");
                                System.Net.Mail.AlternateView plainView = System.Net.Mail.AlternateView.CreateAlternateViewFromString
                                (System.Text.RegularExpressions.Regex.Replace(mailBody, @"<(.|\n)*?>", string.Empty), null, "text/plain");
                                System.Net.Mail.AlternateView htmlView = System.Net.Mail.AlternateView.CreateAlternateViewFromString(mailBody, null, "text/html");
                                m.AlternateViews.Add(plainView);
                                m.AlternateViews.Add(htmlView);
                                smtp.Credentials = new NetworkCredential(SMTPUsername, SMTPPassword);
                                try
                                {
                                    smtp.Send(m);
                                }
                                catch
                                {

                                }


                            }
                        }
                    }

                }
                if (ReservationID != "" && PinCode != "")
                {
                    Home obj = new Home();
                    obj.ReservationID = ReservationID;
                    obj.PinCode = PinCode;
                    obj.HotelName = HotelName;
                    list.Add(obj);
                }
                return Request.CreateResponse(HttpStatusCode.OK, list);
            }
            catch (Exception ex)
            {
                return LogError(ex);
            }
        }


        [HttpGet]
        public HttpResponseMessage typeCreditCardByHotel(string Cultureid, string HotelID)
        {
            List<CreditCardType> list = new List<CreditCardType>();
            DataTable dt = new DataTable();
            try
            {
                string SelectValue = homeService.GetTextMessagesAsString(Cultureid, "Select");
                dt = homeService.typeCreditCardByHotel(Cultureid, HotelID);
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            CreditCardType obj = new CreditCardType();
                            obj.CreditCardTypeID = dr["CreditCardTypeID"].ToString();
                            obj.SelectText = SelectValue;
                            obj.CreditTypeCode = dr["Code"].ToString();
                            obj.CVCLenth = dr["CVCLength"].ToString();
                            obj.CreditCardTypeName = dr["CreditCardTypeName"].ToString();

                            obj.CssClass = "card-select-img";
                            if (obj.CreditTypeCode == "Visa")
                            {
                                obj.CreditCardTypeImage = "Images/payment/visa-curved-64px.png";
                            }
                            else if (obj.CreditTypeCode == "MasterCard")
                            {
                                obj.CreditCardTypeImage = "Images/payment/mastercard-straight-64px.png";
                            }
                            else if (obj.CreditTypeCode == "AmEx")
                            {
                                obj.CreditCardTypeImage = "Images/payment/american-express-curved-64px.png";
                            }
                            else if (obj.CreditTypeCode == "DinersClub")
                            {
                                obj.CreditCardTypeImage = "Images/payment/diners_club_64.png";
                            }
                            else if (obj.CreditTypeCode == "JCB")
                            {
                                obj.CreditCardTypeImage = "Images/payment/jcb_card_payment-128.png";
                            }
                            else if (obj.CreditTypeCode == "Maestro")
                            {
                                obj.CreditCardTypeImage = "Images/payment/maestro-curved-64px.png";
                            }
                            else if (obj.CreditTypeCode == "Discover")
                            {
                                obj.CreditCardTypeImage = "Images/payment/discover-curved-64px.png";
                            }
                            list.Add(obj);
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

        [HttpGet]
        public HttpResponseMessage TypeMonth(string Cultureid)
        {
            List<MonthModel> list = new List<MonthModel>();
            DataTable dt = new DataTable();
            try
            {

                string SelectValue = homeService.GetTextMessagesAsString(Cultureid, "Select");
                dt = homeService.TypeMonth(Cultureid);
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            MonthModel obj = new MonthModel();
                            obj.MonthID = dr["id"].ToString();
                            obj.MonthName = dr["Month"].ToString();
                            obj.SelectText = SelectValue;
                            list.Add(obj);
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


        [HttpGet]
        public HttpResponseMessage TypeTraveller(string culture)
        {

            List<TravelTypeModel> listTravelType = new List<TravelTypeModel>();
            DataTable dataTableTravelType = new DataTable();
            try
            {
                string SelectValue = homeService.GetTextMessagesAsString(culture, "NotCertain");
                dataTableTravelType = homeService.TypeTraveller(culture);
                if (dataTableTravelType != null && dataTableTravelType.Rows.Count > 0)
                {
                    foreach (DataRow dr in dataTableTravelType.Rows)
                    {
                        TravelTypeModel obj = new TravelTypeModel();
                        obj.TravellerID = dr["id"].ToString();
                        obj.TravellerType = dr["TravellerType"].ToString();
                        obj.SelectText = SelectValue;
                        listTravelType.Add(obj);
                    }
                }
                return Request.CreateResponse(HttpStatusCode.OK, listTravelType);
            }
            catch (Exception ex)
            {
                return LogError(ex);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetSalutation(string culture)
        {

            List<SalutationModel> listSalutation = new List<SalutationModel>();
            DataTable dataTableSalutation = new DataTable();
            try
            {
                string SelectValue = homeService.GetTextMessagesAsString(culture, "Select");
                dataTableSalutation = homeService.GetSalutation(culture);
                if (dataTableSalutation != null && dataTableSalutation.Rows.Count > 0)
                {
                    foreach (DataRow dr in dataTableSalutation.Rows)
                    {
                        SalutationModel obj = new SalutationModel();
                        obj.SalutationTypeID = dr["ID"].ToString();
                        obj.Salutation = dr["SalutationName"].ToString();
                        obj.SelectText = SelectValue;
                        listSalutation.Add(obj);
                    }
                }
                return Request.CreateResponse(HttpStatusCode.OK, listSalutation);
            }
            catch (Exception ex)
            {
                return LogError(ex);
            }
        }
    }
}
