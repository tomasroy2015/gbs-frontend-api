using Business;
using GBSHotels.API.Controllers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace GBSHotels.API.Models
{
    public class Encryption64
    {

        private byte[] key = { };
        //private byte[] IV = { 10, 20, 30, 40, 50, 60, 70, 80 };

        private byte[] IV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
       
        public string Decrypt(string stringToDecrypt, string sEncryptionKey)
        {
            if (!string.IsNullOrEmpty(stringToDecrypt))
            {
                byte[] inputByteArray = new byte[stringToDecrypt.Length];
                try
                {
                    key = Encoding.UTF8.GetBytes(sEncryptionKey.Substring(0, 8));
                    DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                    inputByteArray = Convert.FromBase64String(stringToDecrypt);
                    MemoryStream ms = new MemoryStream();
                    CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(key, IV),
                                                                      CryptoStreamMode.Write);
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    Encoding encoding = Encoding.UTF8;
                    return encoding.GetString(ms.ToArray());
                }
                catch (Exception ex)
                {
                    return stringToDecrypt;
                    // return ex.Message;
                }
            }
            else
            {
                return stringToDecrypt;
            }
        }


        public string Encrypt(string stringToEncrypt, string sEncryptionKey)
        {
            try
            {
                key = Encoding.UTF8.GetBytes(sEncryptionKey.Substring(0, 8));
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                Byte[] inputByteArray = Encoding.UTF8.GetBytes(stringToEncrypt);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(key, IV),
                                                                  CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                return Convert.ToBase64String(ms.ToArray());
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }

    public class bedselection
    {
        public string bedoptionvalue { get; set; }
        public string bedoptiontext { get; set; }
    }

    public class RoomPrice
    {
        public string tblheader { get; set; }
        public string tblbobyvalues { get; set; }
    }


    public class traveller
    {
        public string ID { get; set; }

        public string travelertype { get; set; }

    }

    public class RoomIcons
    {

        public string FacilityName { get; set; }

        public string Icon { get; set; }
    }
    public class HotelCreditCards
    {
        public string CardImage { get; set; }
        public string CardName { get; set; }
    }
    public class HotelImages
	{
		public string Image { get; set; }
        public int ID { get; set; }
	}
    public class Home
    {
        public List<Home> RoomSubDetail { get; set; }
        public List<HotelImages> HotelImageThumb { get; set; }
        public List<HotelCreditCards> HotelCreditCardImage { get; set; }
        public List<RoomIcons> HotelRoomIcons { get; set; }
        public string ID { get; set; }
        public string Description { get; set; }
        public string RoutingName { get; set; }
        public string ClosestAirportName { get; set; }
        public string ClosestAirportDistance { get; set; }
        public string MainPhotoName { get; set; }
        public string CityName { get; set; }
        public string HotelClass { get; set; }
        public string MinumumRoomRate { get; set; }
        public string CurrencyID { get; set; }
        public string CurrencySymbol { get; set; }
        public string CurrencyName { get; set; }
        public string ReviewCount { get; set; }
        public string CityNavigateURL { get; set; }

        public int ReviewCountValue { get; set; }
        public string AverageReviewPoint { get; set; }
        public string ReviewEvaluationName { get; set; }
        public string ReviewStatus { get; set; }

        public string Superb { get; set; }
        public string Hotel { get; set; }
        public string ScoreFrom { get; set; }
        public string Reviews { get; set; }
        public string DescriptionText { get; set; }
        public string New { get; set; }
        public string HotelStatus { get; set; }
        public string VeryGood { get; set; }



        public string TextMessage { get; set; }
        public string LblId { get; set; }

        public string Code { get; set; }
        public string RegionType { get; set; }
        public string ParentID { get; set; }
        public string SecondParentID { get; set; }

        public string ParentName { get; set; }
        public string SecondParentName { get; set; }

        public string IsCity { get; set; }

        public string MainPageDisplay { get; set; }
        public string Sort { get; set; }

        public string CountryID { get; set; }
        public string CityID { get; set; }
        public string RegionID { get; set; }
        public string MainRegionID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string WebAddress { get; set; }
        public string Email { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string MapZoomIndex { get; set; }
        public string CountryName { get; set; }
        public string MainRegionName { get; set; }
        public string RegionName { get; set; }

        public string DestinationSearchType { get; set; }
        public string NameASCII { get; set; }
        public string Population { get; set; }
        public string HotelID { get; set; }
        public string HotelName { get; set; }
        public string IsPopular { get; set; }

        public string CultureCode { get; set; }
        public string CultureDescription { get; set; }
        public string CultureImageClass { get; set; }


        public string Countryname { get; set; }
        public string topphoto { get; set; }
        public string topphotoflag { get; set; }
        public string textmessage { get; set; }
        public string Count { get; set; }


        public string Properties { get; set; }
        public string CityImages { get; set; }
        public string Cityname { get; set; }

        public string textmessage1 { get; set; }

        public string Continents { get; set; }
        public string destination { get; set; }
        public string country { get; set; }

        public string HotelImage { get; set; }
        public string HotelImageID { get; set; }

        public string HotelStar { get; set; }
        public string Avgprice { get; set; }

        public string RoomPrice { get; set; }

        public string KmFrom { get; set; }

        public string RoomDataTableType { get; set; }
        public string HotelRoomID { get; set; }
        public string HotelRoomDescription { get; set; }
        public string HotelTypeName { get; set; }
        public string SmokingTypeName { get; set; }
        public string RoomCount { get; set; }
        public string MaxPeopleCount { get; set; }
        public string ExtraBedCount { get; set; }
        public string AttributeName { get; set; }
        public string AttributeHeaderID { get; set; }
        public string AttributeHeaderName { get; set; }
        public string UnitID { get; set; }
        public string UnitName { get; set; }
        public string Charged { get; set; }
        public string HotelUnitID { get; set; }
        public string HotelUnitName { get; set; }
        public string Charge { get; set; }
        public string CurrencyCode { get; set; }
        public string CreditCardTypeID { get; set; }
        public string CreditCardTypeName { get; set; }

        public string AttributeID { get; set; }
        public string Checkin { get; set; }
        public string Checkout { get; set; }
        public string RoomTypeText { get; set; }
        public string RoomPriceText { get; set; }
        public string RoomMaxPeopleCount { get; set; }
        public string Quantity { get; set; }
        public string CheckinStart { get; set; }
        public string CheckinEnd { get; set; }
        public string CheckoutStart { get; set; }
        public string CheckoutEnd { get; set; }
        public string UntilTime { get; set; }
        public string FromTime { get; set; }
        public string FREEcancellation { get; set; }
        public string RoomFacilities { get; set; }
        public string Policiesof { get; set; }
        public string Facilitiesof { get; set; }
        public string ShowMap { get; set; }
        public string HotelCreditCard { get; set; }
        public string CheckDatefrom { get; set; }
        public string CheckDateto { get; set; }


        public string SalutationTypeID { get; set; }
        public string Salutation { get; set; }

        public string ReservationID { get; set; }
        public string TravellerTypeID { get; set; }
        public string TravelerTypeName { get; set; }
        public string AveragePoint { get; set; }
        public string ReviewStatusName { get; set; }
        public string Anonymous { get; set; }
        public string Active { get; set; }
        public string CreateDateTime { get; set; }
        public string OpDateTime { get; set; }
        public string IPAddress { get; set; }
        public string ReviewTypeID { get; set; }
        public string ReviewTypeName { get; set; }
        public string ReviewTypeEvaluationName { get; set; }
        public string ReviewTypeScaleName { get; set; }
        public string UserFullName { get; set; }
        public string Point { get; set; }
        public string FirmName { get; set; }
        public string PartID { get; set; }
        public string totalReviewCount { get; set; }
        public string City { get; set; }
        public string MaxCount { get; set; }
        public string Userphoto { get; set; }

        public string Questions { get; set; }
        public string Answers { get; set; }
        public string termsconditions { get; set; }

        public string Typereview { get; set; }
        public string travelertype { get; set; }


        public string ReviewPositive { get; set; }
        public string Reviewnegative { get; set; }
        public string total { get; set; }
        public string froms { get; set; }
        public string review { get; set; }

        public string UniqueID { get; set; }
        public string AccommodationTypeID { get; set; }
        public string AccommodationTypeName { get; set; }
        public string AccommodationTypeDescription { get; set; }
        public string PricePolicyTypeID { get; set; }
        public string PricePolicyTypeName { get; set; }
        public string SingleRate { get; set; }
        public string DoubleRate { get; set; }
        public string DailyRoomPrices { get; set; }
        public string OriginalRoomPrice { get; set; }
        public string HotelClassValue { get; set; }
        public string RoomCounttext { get; set; }
        public string Conditions { get; set; }
        public string GuestName { get; set; }
        public string BedPreference { get; set; }
        public string TravellerType { get; set; }
        public string EstimatedArrivalTime { get; set; }
        public string SpecialNote { get; set; }
        public string PayableAmount { get; set; }
        public string BookingTotalPriceWarning { get; set; }
        public string MaxPeopleCountval { get; set; }
        public string NonRefundableInfo { get; set; }
        public string RoomColumnID { get; set; }


        public string Image { get; set; }

        public string UserName { get; set; }

        public string Surname { get; set; }

        public string PostCode { get; set; }
        public string Country { get; set; }


        public string MonthID { get; set; }
        public string MonthName { get; set; }
        public string TravellerID { get; set; }
        public string BedTypeID { get; set; }
        public string BedTypeNameWithCount { get; set; }
        public string RoomPriceTotal { get; set; }

        public string UserID { get; set; }

        public string PinCode { get; set; }
        public string ReservationDate { get; set; }
        public string Property { get; set; }
        public string CheckInDate { get; set; }
        public string CheckOutDate { get; set; }
        public string Status { get; set; }

        public string CountryCount { get; set; }
        public string HotelCount { get; set; }
        public string ContinentsID { get; set; }
        public string Properties1 { get; set; }
        public string MimeType { get; set; }
        public string FileName { get; set; }

        public string PeopleCount { get; set; }

        public string BedOptionNo { get; set; }

        public string ReservationStatus { get; set; }

        public string NightCount { get; set; }

        public string VerificationCode { get; set; }
        public string SearchID { get; set; }
        public string OptionRoomCount { get; set; }

        public string Amount { get; set; }
        public string icons { get; set; }
        public string month { get; set; }
        public string year { get; set; }
        public string CreditCardNumber { get; set; }
        public string ExpiryDate { get; set; }
        public string NameOnCreditCard { get; set; }

        public string LastBookingMadeText { get; set; }

        public double MinumumRoomPrice { get; set; }

        public string MinimunRoomRate { get; set; }

        public string typemonth { get; set; }

        public string yourcredit { get; set; }

        public string remove { get; set; }

        public string cancel { get; set; }

        public string business { get; set; }

        public string savechange { get; set; }
        public double point { get; set; }

        public double ConvertedRoomPrice { get; set; }
        public string NewCurrencySymbol { get; set; }
        public double ConvertedRoomPriceTotal { get; set; }
        public string Expirydates { get; set; }
        public string MaxChildrenCount { get; set; }


        public string RoomBedInfo { get; set; }
        public string HotelCurrency { get; set; }
        public string CancelPolicyText { get; set; }
        public string AndName { get; set; }
        public string OrName { get; set; }
        public string OptionNo { get; set; }
        public string BedTypeName { get; set; }

        public string HotelRoutingName { get; set; }

        public string CountryCode { get; set; }

        public string NavigateURL { get; set; }

        public string VAT { get; set; }

        public string RoomPriceWarning { get; set; }

        public string Included { get; set; }

        public string Excluded { get; set; }

        public string RefundableInfo { get; set; }

        public string CityTax { get; set; }

        public string BreakfastText { get; set; }

        public string RoomPhotoName { get; set; }
        public string UnitValue { get; set; }

        public string NightwisePrice { get; set; }

        public string NightpriceCount { get; set; }
        public string NoCreditCards { get; set; }

        public string PagingPreviousPage { get; set; }
        public string PagingNextPage { get; set; }
        public string Close { get; set; }
        public object SelectText { get; set; }
        public double attributeCharge { get; set; }

        // used to prior to update attributeCharges.. Saved to save Total in Hotel Currency
        //public double ValueInHotelCurrency { get; set; }
        public  string Encrypt(string clearText)
        {
            //  string EncryptionKey = "MAKV2SPBNI99212";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                //Rfc2898DeriveBytes pdb = new
                //    Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                //encryptor.Key = pdb.GetBytes(32);
                //encryptor.IV = pdb.GetBytes(16);

                encryptor.Key = Encoding.UTF8.GetBytes("2428598755421637");
                encryptor.IV = Encoding.UTF8.GetBytes("5369523205842148");

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
            return clearText;
        }
        public string CurrencyConvertion(string CurrencyFrom, string CurrencyTo)
        {
            // Session["CurrentCurrency"] = CurrencyTo;
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
                for (int i = 0; i < arrDigits.Length; i++)
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
            {
                res = "76.51" + " " + "EUR";
            }

            try
            {
                Home newHomeObj = new Home();
                string[] split = res.Split(null);
                string price = split[0];
                string Symbol = newHomeObj.GetCurrencySymbolBYCode(split[1]);

            }
            catch
            {
            }
            return res;
        }
      
        public string CurrencyConvertionFilter(string CurrencyFrom, string CurrencyTo)
        {
            WebClient web = new WebClient();
            string url = string.Format("https://www.google.com/finance/converter?a=1&from={0}&to={1}", CurrencyFrom.ToUpper(), CurrencyTo.ToUpper());
            string response = web.DownloadString(url);
            Regex regex = new Regex(@"( \ )");
            string[] arrDigits = regex.Split(response);
            string rate = arrDigits[680];
            string[] rate1 = rate.Split('<');
            string rate2 = rate1[5];
            string[] rate3 = rate2.Split('>');
            string rate4 = rate3[1];
            return rate4;
        }
        public DataTable GetCultureCount(string Culture)
        {
            DataTable dt = new DataTable();
            con.Open();
            SqlCommand cmd = new SqlCommand("B_GetCultureAvailableCount_BizTbl_Culture_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Culture", Culture);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            return dt;
        }

        public string GetHotelIDByRoutingName(string RoutingName)
        {
            DataTable dt = new DataTable();
            con.Open();
            SqlCommand cmd = new SqlCommand("B_GetHotelIDByRoutingName_TB_Hotel_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@RoutingName", RoutingName);
            string HotelID = "";
            HotelID = Convert.ToString(cmd.ExecuteScalar());
            return HotelID;
        }


        public string TruncateLongString(string str, int maxLength)
        {
            return str.Substring(0, Math.Min(str.Length, maxLength));
        }

        SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Connection"].ConnectionString);

        public DataTable GetHotels(string Culture)
        {
            DataTable dt = new DataTable();
            con.Open();

            SqlCommand cmd = new SqlCommand("[dbo].[B_GetPopularHotel_TBHotel_SP]", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Culture", Culture);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            return dt;
        }

        public string CheckCurrentPassword(string UserId1, string encryptCurrentPassword)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("B_GetCurrentPassword_BizTbl_User_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserId", UserId1);
            cmd.Parameters.AddWithValue("@CurrentPassword", encryptCurrentPassword);
            string i = Convert.ToString(cmd.ExecuteScalar());
            con.Close();
            return i;
        }

        public string GetCurrencySymbolBYCode(string Code)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("B_GetCurrencySymbol_TB_Currency_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CurrencyCode", Code);
            string i = Convert.ToString(cmd.ExecuteScalar());
            con.Close();
            return i;
        }
        public DataTable GetFeaturedHotel(string Culture)
        {
            DataTable dt = new DataTable();
            con.Open();
            SqlCommand cmd = new SqlCommand("[B_GetFeaturedHotel_TB_Hotel_SP]", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Culture", Culture);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            //testinsert();
            return dt;
        }

        public int testinsert()
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("Update BizTbl_Message  set Description_ar='وجهات مهمة', Description_ru='Нью-Йорк' where Code='TOPDESTINATIONS'", con);

            int i = cmd.ExecuteNonQuery();
            con.Close();
            return i;
        }

        public DataTable GetHotelsMap(string CultureID)
        {


            con.Open();

            SqlCommand cmd = new SqlCommand("B_GetHotelsforMap_SP", con);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Culture", CultureID);

            SqlDataAdapter da = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();

            da.Fill(dt);

            con.Close();

            return dt;
        }

        public void UpdateHotelHitCount(string ID, string Description)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("B_Update_TB_HitCount_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", ID);
            cmd.Parameters.AddWithValue("@Description", Description);
            cmd.ExecuteNonQuery();
        }



        public DataTable GetCurrentHitCount(Enum PartID, string RecordID, string date)
        {
            DataTable dt = new DataTable();
            con.Open();
            SqlCommand cmd = new SqlCommand("B_GetCurrentHitcount_TB_HitCount_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PartID", PartID);
            cmd.Parameters.AddWithValue("@RecordID", RecordID);
            cmd.Parameters.AddWithValue("@date", date);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            return dt;
        }

        public DataTable GetRegions(string CultureID, string RegionID)
        {
            con.Open();

            SqlCommand cmd = new SqlCommand("TB_SP_GetRegions", con);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Culture", CultureID);

            cmd.Parameters.AddWithValue("@ID", RegionID);

            SqlDataAdapter da = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();

            da.Fill(dt);

            con.Close();

            return dt;
        }

        public DataTable GetHotelSearchMap(string CultureID, string DestinationName, string RegionID)
        {


            con.Open();

            SqlCommand cmd = new SqlCommand("[B_GetHotelsforMapSearch_SP]", con);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Culture", CultureID);

            cmd.Parameters.AddWithValue("@DestinationName", DestinationName);

            cmd.Parameters.AddWithValue("@RegionID", RegionID);

            SqlDataAdapter da = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();

            da.Fill(dt);

            con.Close();

            return dt;
        }


        #region AutoSearch

        public DataTable GetSearchDestination(string Keyword, string CultureCode, string CountryID)
        {
            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Connection"].ConnectionString);

            con.Open();

            SqlCommand cmd = new SqlCommand("[dbo].[TB_SP_GetDestinationSearchResult]", con);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Culture", CultureCode);

            cmd.Parameters.AddWithValue("@Keyword", Keyword);

            //cmd.Parameters.AddWithValue("@CountryID",CountryID);

            SqlDataAdapter da = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();

            da.Fill(dt);

            con.Close();

            return dt;
        }

        #endregion AutoSearch


        #region HotelRegion

        public DataSet GetCountryRegions(string OrderBy, string Culture, int PagingSize, int PageIndex, string HotelID, string CountryId)
        {

            DataSet ds = new DataSet();
            con.Open();
            SqlCommand cmd = new SqlCommand("TB_SP_GetHotels", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@OrderBy", OrderBy);
            cmd.Parameters.AddWithValue("@Culture", Culture);
            cmd.Parameters.AddWithValue("@PagingSize", PagingSize);
            cmd.Parameters.AddWithValue("@PageIndex", PageIndex);
            cmd.Parameters.AddWithValue("@HotelID", HotelID);
            cmd.Parameters.AddWithValue("@CountryID", CountryId);
            cmd.Parameters.AddWithValue("@Active", true);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            return ds;

        }

        public int GetCountryIdByName(string CountryName)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("B_GetCountryId_TB_Country_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CountryName", CountryName);
            int i = Convert.ToInt32(cmd.ExecuteScalar());
            con.Close();
            return i;
        }

        public DataSet GetHotelRegions(string CultureCode, string CountryId, string TxtCode)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("B_GetRegions_TB_Region_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CultureCode", CultureCode);
            cmd.Parameters.AddWithValue("@CountryName", CountryId);
            cmd.Parameters.AddWithValue("@TxtCode", TxtCode);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            con.Close();
            return ds;
        }

        public DataTable GetNearbyCities(string CultureCode, string CountryId, string HotelID)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("[B_GetNearbyCities_TB_Region_SP]", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CultureCode", CultureCode);
            cmd.Parameters.AddWithValue("@CountryName", CountryId);
            cmd.Parameters.AddWithValue("@HotelID", HotelID);
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();

            da.Fill(dt);

            con.Close();

            return dt;
        }

        #endregion

        #region AddProperty

        public string FacilityName { get; set; }
        public string FacilityId { get; set; }
        public string ManagerName { get; set; }
        public string ManagerId { get; set; }
        public string CurrencyId { get; set; }
        public string SystemCode { get; set; }

        public string TypeId { get; set; }
        public string TypeName { get; set; }




        #region ContactInformation

        public string FirmId { get; set; }

        public DataTable insertContactInfo(string MgrName, string MgrEmail, string MgrPhone, string ActName, string ActEmail, string ActPhone, string ActFax, string ResName, string ResEmail, string ResPhone, string ResFax, string IPAddress)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("B_Insert_ContactInfo_TB_Firm_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@MgrName", MgrName);
            cmd.Parameters.AddWithValue("@MgrEmail", MgrEmail);
            cmd.Parameters.AddWithValue("@MgrPhone", MgrPhone);
            cmd.Parameters.AddWithValue("@ActName", ActName);
            cmd.Parameters.AddWithValue("@ActEmail", ActEmail);
            cmd.Parameters.AddWithValue("@ActPhone", ActPhone);
            cmd.Parameters.AddWithValue("@ActFax", ActFax);
            cmd.Parameters.AddWithValue("@ResName", ResName);
            cmd.Parameters.AddWithValue("@ResEmail", ResEmail);
            cmd.Parameters.AddWithValue("@ResPhone", ResPhone);
            cmd.Parameters.AddWithValue("@ResFax", ResFax);
            cmd.Parameters.AddWithValue("@IPAddress", IPAddress);
            //int i = Convert.ToInt32(cmd.ExecuteScalar());
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            return dt;
        }

        #endregion

        #region PropertyDetails

        public string GetUserID(string ContactPersonEmail)
        {
            string Id = "";
            con.Open();
            SqlCommand cmd = new SqlCommand("B_GetUserInfo_BizTbl_User_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@EMail", ContactPersonEmail);
            Id = Convert.ToString(cmd.ExecuteScalar());
            con.Close();
            return Id;
        }

        public string SaveUser(string Salutation, string ContactPersonName, string ContactPersonSurname, string ProCountry, string ProCity, string CityName,
            string ContactPersonPhone, string ContactPersonEmail, string IPAddress)
        {
            string Id = "";

            con.Open();
            SqlCommand cmd = new SqlCommand("B_SaveUser_BizTbl_User_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Salutation", Salutation);
            cmd.Parameters.AddWithValue("@ContactPersonName", ContactPersonName);
            cmd.Parameters.AddWithValue("@ContactPersonSurname", ContactPersonSurname);
            cmd.Parameters.AddWithValue("@ProCountry", ProCountry);
            cmd.Parameters.AddWithValue("@ProCity", ProCity);
            cmd.Parameters.AddWithValue("@RegionID", ProCity);
            cmd.Parameters.AddWithValue("@CityName", CityName);
            cmd.Parameters.AddWithValue("@ContactPersonPhone", ContactPersonPhone);
            cmd.Parameters.AddWithValue("@ContactPersonEmail", ContactPersonEmail);
            cmd.Parameters.AddWithValue("@IPAddress", IPAddress);
            cmd.Parameters.AddWithValue("@Active", false);
            cmd.Parameters.AddWithValue("@Locked", false);
            cmd.Parameters.AddWithValue("@StatusID", 1);

            Id = Convert.ToString(cmd.ExecuteScalar());
            con.Close();

            return Id;
        }

        public string GetFirmID(string FirmName, string ProCity)
        {
            string Id = "";
            con.Open();
            SqlCommand cmd = new SqlCommand("B_GetfirmID_TB_Firm_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@FirmName", FirmName);
            cmd.Parameters.AddWithValue("@RegionID", ProCity);
            Id = Convert.ToString(cmd.ExecuteScalar());
            con.Close();
            return Id;
        }

        public string SaveFirm(string FirmName, string ProCountry, string ProCity, string ProPhone, string ProFax,
            string ProEmail, string Salutation, string ContactPersonName, string ContactPersonSurname, string ContactPersonPosition, string ContactPersonPhone,
            string ContactPersonEmail, string IPAddress, string userID)
        {
            string Id = "";

            con.Open();
            SqlCommand cmd = new SqlCommand("B_SaveFirm_TB_Firm_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Name", FirmName);
            cmd.Parameters.AddWithValue("@CountryID", ProCountry);
            cmd.Parameters.AddWithValue("@RegionID", ProCity);
            cmd.Parameters.AddWithValue("@CityID", ProCity);
            cmd.Parameters.AddWithValue("@Phone", ProPhone);
            cmd.Parameters.AddWithValue("@Fax", ProFax);
            // cmd.Parameters.AddWithValue("@PostCode", CityName);
            cmd.Parameters.AddWithValue("@Email", ProEmail);
            cmd.Parameters.AddWithValue("@ContactPersonSalutationTypeID", Salutation);
            cmd.Parameters.AddWithValue("@ContactPersonName", ContactPersonName);
            cmd.Parameters.AddWithValue("@ContactPersonSurname", ContactPersonSurname);
            cmd.Parameters.AddWithValue("@ContactPersonPosition", ContactPersonPosition);
            cmd.Parameters.AddWithValue("@ContactPersonPhone", ContactPersonPhone);
            cmd.Parameters.AddWithValue("@ContactPersonEmail", ContactPersonEmail);
            cmd.Parameters.AddWithValue("@StatusID", 1);
            cmd.Parameters.AddWithValue("@Active", false);
            cmd.Parameters.AddWithValue("@OpUserID", userID);
            cmd.Parameters.AddWithValue("@IPAddress", IPAddress);

            Id = Convert.ToString(cmd.ExecuteScalar());
            con.Close();

            return Id;
        }

        public void SaveUserFirm(string userID, string firmID)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("B_InsertPropertyDetails_TB_Hotel_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@userID", userID);
            cmd.Parameters.AddWithValue("@firmID", firmID);
            con.Close();
        }

        //public string SaveHotel(string firmID,string ProType,string ProClass,string ProAccommodation,string ProCountry,string ProCity,
        //        string HotelName,string ProAddress,string ProFax,string RoomCount,string ProWebsite,string ProEmail,string ProResCurrency,
        //         string ChannelManager, string CultureID, string IPAddress, string userID, string Culture)
        //{
        //    string id = "";




        //    return id;
        //}

        public int InsertProDetails(string ProName, string ProCountry, string ProCity, string ProAddress, string ProPhone, string ProFax, string ProType, string ProClass, string ProAccommodation, string ProRoomCount, string ProEmail, string ProResCurrency, string ProWebsite, string FirmId, string CityName, string IPAddress, string ProUserId)
        {
            DataTable dt = new DataTable();
            con.Open();
            SqlCommand cmd = new SqlCommand("B_InsertPropertyDetails_TB_Hotel_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ProName", ProName);
            cmd.Parameters.AddWithValue("@ProCountry", ProCountry);
            cmd.Parameters.AddWithValue("@ProCity", ProCity);
            cmd.Parameters.AddWithValue("@ProAddress", ProAddress);
            cmd.Parameters.AddWithValue("@ProPhone", ProPhone);
            cmd.Parameters.AddWithValue("@ProFax", ProFax);
            cmd.Parameters.AddWithValue("@ProType", ProType);
            cmd.Parameters.AddWithValue("@ProClass", ProClass);
            cmd.Parameters.AddWithValue("@ProAccommodation", ProAccommodation);
            cmd.Parameters.AddWithValue("@ProRoomCount", ProRoomCount);
            cmd.Parameters.AddWithValue("@ProEmail", ProEmail);
            cmd.Parameters.AddWithValue("@ProResCurrency", ProResCurrency);
            cmd.Parameters.AddWithValue("@ProWebsite", ProWebsite);
            cmd.Parameters.AddWithValue("@FirmId", FirmId);
            cmd.Parameters.AddWithValue("@CityName", CityName);
            cmd.Parameters.AddWithValue("@IPAddress", IPAddress);
            cmd.Parameters.AddWithValue("@ProUserId", ProUserId);
            //int i = cmd.ExecuteNonQuery();
            int i = Convert.ToInt32(cmd.ExecuteScalar());
            con.Close();
            return i;
        }
        #endregion

        #region PropertyFacilities

        public DataTable GetAttributeHeaders(string CultureId)
        {
            DataTable dt = new DataTable();
            con.Open();
            SqlCommand cmd = new SqlCommand("TB_SP_GetAttributeHeaders", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PartID", 5);
            cmd.Parameters.AddWithValue("@AttributeTypeID", 1);
            cmd.Parameters.AddWithValue("@Active", 1);
            cmd.Parameters.AddWithValue("@Culture", CultureId);
            cmd.Parameters.AddWithValue("@OrderBy", "Sort");

            SqlDataAdapter sda = new SqlDataAdapter(cmd);

            sda.Fill(dt);
            con.Close();
            return dt;
        }

        public DataTable GetAttributes(string AttributeHeaderId, string CultureId)
        {
            // string PropertyConditions = "";
            DataTable dt = new DataTable();
            con.Open();
            SqlCommand cmd = new SqlCommand("TB_SP_GetAttributes", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PartID", 5);
            cmd.Parameters.AddWithValue("@AttributeTypeID", 1);
            cmd.Parameters.AddWithValue("@AttributeHeaderID", AttributeHeaderId);
            cmd.Parameters.AddWithValue("@Active", 1);
            cmd.Parameters.AddWithValue("@Culture", CultureId);
            cmd.Parameters.AddWithValue("@OrderBy", "Name");

            SqlDataAdapter sda = new SqlDataAdapter(cmd);

            sda.Fill(dt);
            con.Close();
            return dt;
        }

        public int InsertGeneralFacilities(string HotelId, string FacilitiesId, string FacilitiesCharge)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("B_InsertGeneralFacilities_TB_HotelAttribute_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HotelId", HotelId);
            cmd.Parameters.AddWithValue("@FacilitiesId", FacilitiesId);
            cmd.Parameters.AddWithValue("@FacilitiesCharge", FacilitiesCharge);
            int i = cmd.ExecuteNonQuery();
            con.Close();
            return i;
        }

        public DataTable GenSearch(string GenarlField, string Cultureid, string AttributeHeaderID)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("B_GenSearch_TB_Attribute_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@GenarlField", GenarlField);
            cmd.Parameters.AddWithValue("@Cultureid", Cultureid);
            cmd.Parameters.AddWithValue("@AttributeHeaderID", AttributeHeaderID);
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(dt);
            con.Close();
            return dt;
        }



        #endregion

        #region RoomDetails
        public int InsertRoomDetails(string HotelId, string RoomType, string RoomCount, string RoomSpace, string RoomMaxPerson, string RoomMaxChildren, string RoomBabyCots, string RoomExBabyCots, string RoomSmoking, string RoomView, string Language, string RoomDescription)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("B_InsertRoomDetails_TB_HotelRoom_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HotelId", HotelId);
            cmd.Parameters.AddWithValue("@RoomType", RoomType);
            cmd.Parameters.AddWithValue("@RoomCount", RoomCount);
            cmd.Parameters.AddWithValue("@RoomSpace", RoomSpace);
            cmd.Parameters.AddWithValue("@RoomMaxPerson", RoomMaxPerson);
            cmd.Parameters.AddWithValue("@RoomMaxChildren", RoomMaxChildren);
            cmd.Parameters.AddWithValue("@RoomBabyCots", RoomBabyCots);
            cmd.Parameters.AddWithValue("@RoomExBabyCots", RoomExBabyCots);
            cmd.Parameters.AddWithValue("@RoomSmoking", RoomSmoking);
            cmd.Parameters.AddWithValue("@RoomView", RoomView);
            cmd.Parameters.AddWithValue("@Language", Language);
            cmd.Parameters.AddWithValue("@RoomDescription", RoomDescription);
            //int i = cmd.ExecuteNonQuery();
            int i = Convert.ToInt32(cmd.ExecuteScalar());
            con.Close();
            return i;
        }

        public int InsertRoomBedDetails(string BedId, string BedCount, int i)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("B_InsertRoomBedDetails_TB_HotelRoomBed_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BedId", BedId);
            cmd.Parameters.AddWithValue("@BedCount", BedCount);
            cmd.Parameters.AddWithValue("@RoomId", i);
            int c = cmd.ExecuteNonQuery();
            con.Close();
            return c;
        }

        #endregion

        #region roomFacilities

        public DataTable GetRoomFacilitiesCount(string HotelId)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("B_GetRoomFacilitiesCount_TB_HotelRoom_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HotelId", HotelId);
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            return dt;
        }

        public int InsertRoomFacilities(string RoomId, string FacilityId)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("B_InsertRoomFacilities_TB_HotelRoomAttribute_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@RoomId", RoomId);
            cmd.Parameters.AddWithValue("@FacilityId", FacilityId);
            cmd.CommandTimeout = 5000;
            int i = cmd.ExecuteNonQuery();
            con.Close();
            return i;
        }
        #endregion

        #region Settings

        public string PenaltyRate { get; set; }

        public int InsertSettingDetails(string ChannelManager, string RadioRefund, string RefundCancel, string PenaltyRate, string StartDate, string EndDate, string Commission, string HotelId)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("B_InsertSettingDetails_TB_HotelCancelPolicy_HotelComission_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ChannelManager", ChannelManager);
            cmd.Parameters.AddWithValue("@CancelTypeId", RadioRefund);
            cmd.Parameters.AddWithValue("@RefundDayCount", RefundCancel);
            cmd.Parameters.AddWithValue("@PenaltyRate", PenaltyRate);
            cmd.Parameters.AddWithValue("@StartDate", StartDate);
            cmd.Parameters.AddWithValue("@EndDate", EndDate);
            cmd.Parameters.AddWithValue("@Commission", Commission);
            cmd.Parameters.AddWithValue("@HotelId", HotelId);
            int i = cmd.ExecuteNonQuery();
            con.Close();
            return i;
        }

        public int InsertCreditCards(string CreditCardId, string HotelId)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("B_InsertCreditCardDetails_TB_HotelCreditCard_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CreditCardId", CreditCardId);
            cmd.Parameters.AddWithValue("@HotelId", HotelId);
            int i = cmd.ExecuteNonQuery();
            con.Close();
            return i;
        }


        public int InsertSettingHAttributes(string AttributeId, string Charge, string HotelId)
        {

            DateTime Endate = DateTime.ParseExact("2100-12-31", "yyyy-MM-dd", CultureInfo.InvariantCulture);
            con.Open();
            SqlCommand cmd = new SqlCommand("B_InsertSettingHAttributes_TB_HotelAttribute_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@AttributeId", AttributeId);
            cmd.Parameters.AddWithValue("@Charge", Charge);
            cmd.Parameters.AddWithValue("@HotelId", HotelId);
            cmd.Parameters.AddWithValue("@StartDate", DateTime.Now.Date);
            cmd.Parameters.AddWithValue("@Endate", Endate.Date);
            int i = cmd.ExecuteNonQuery();
            con.Close();
            return i;
        }


        public DataTable GetSPenaltyRate(string CultureId)
        {
            DataTable dt = new DataTable();
            con.Open();
            SqlCommand cmd = new SqlCommand("B_GetPenaltyRate_TB_TypePenaltyRate_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CultureId", CultureId);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            return dt;
        }

        public DataTable GetCreditCardDetails()
        {
            DataTable dt = new DataTable();
            con.Open();
            SqlCommand cmd = new SqlCommand("B_GetCreditCard_TB_TypeCreditCard_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            return dt;
        }



        #endregion

        #region City

        public Int64 GetRegionIdByName(string RegionName)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("B_GetRegionId_TB_Region_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@RegionName", RegionName);
            Int64 i = Convert.ToInt64(cmd.ExecuteScalar());
            con.Close();
            return i;
        }

        //public string GetRegionName(string Regionid)
        //{
        //    con.Open();
        //    SqlCommand cmd = new SqlCommand("B_GetRegionName_TB_Region_SP", con);
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.Parameters.AddWithValue("@Regionid", Regionid);
        //    string i =Convert.ToString(cmd.ExecuteScalar());
        //    con.Close();
        //    return i;
        //}
        public DataSet getsearchhotels(string Cultureid, string city, string checkInDate, string checkOutDate)
        {
            DataSet ds = new DataSet();
            con.Open();
            SqlCommand cmd = new SqlCommand("B_GetSearchHotelsResults_TB_Hotel_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Culture", Cultureid);
            cmd.Parameters.AddWithValue("@OrderBy", "Sort");
            cmd.Parameters.AddWithValue("@PagingSize", 200);
            cmd.Parameters.AddWithValue("@PageIndex", 1);
            cmd.Parameters.AddWithValue("@HotelID", null);
            cmd.Parameters.AddWithValue("@Name", null);
            cmd.Parameters.AddWithValue("@FirmID", null);
            cmd.Parameters.AddWithValue("@CountryID", null);
            cmd.Parameters.AddWithValue("@CityID", city);
            if (checkInDate != "")
            {
                DateTime checkinDT = DateTime.ParseExact(checkInDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                string Datefromm = checkinDT.ToString("yyyy/MM/dd");
                cmd.Parameters.AddWithValue("@CheckInDate", Datefromm);
            }
            else
            {
                cmd.Parameters.AddWithValue("@CheckInDate", null);
            }
            if (checkOutDate != "")
            {
                DateTime checkoutDT = DateTime.ParseExact(checkOutDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                string Dateout = checkoutDT.ToString("yyyy/MM/dd");
                cmd.Parameters.AddWithValue("@CheckOutDate", Dateout);
            }
            else
            {
                cmd.Parameters.AddWithValue("@CheckOutDate", null);
            }
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();

            return ds;
        }


        #endregion

        public DataTable GetPropertyFacilities(string culture, string AttributeHeaderID)
        {
            DataTable dt = new DataTable();
            con.Open();
            SqlCommand cmd = new SqlCommand("B_GetPropertyFacilities_TB_Attribute_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CultureId", culture);
            cmd.Parameters.AddWithValue("@AttributeHeaderID", AttributeHeaderID);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            return dt;
        }

        public DataTable GetRoomFacilities(string CultureId)
        {
            DataTable dt = new DataTable();
            con.Open();
            SqlCommand cmd = new SqlCommand("B_GetRoomFacilities_TB_Attribute_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Culture", CultureId);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            return dt;
        }

        public DataTable GetChannelManager()
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("B_GetChannelManager_TB_ChannelManager_Sp", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();
            return dt;
        }

        public DataTable GetSettingCulture()
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("B_Getculture_BizTbl_Culture", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();
            return dt;
        }

        public DataTable GetCurrencyValues(string culture)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("B_GetCurrency_TB_Currency_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Cultureid", culture);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();
            return dt;
        }

        public DataTable GetRoomTypeDetails(string culture)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("B_GetRoomType_TB_TypeRoom_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Cultureid", culture);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();
            return dt;
        }

        public DataTable GetSmokingTypeDetails(string CultureId)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("B_GetSmokingType_TB_TypeSmoking_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Cultureid", CultureId);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();
            return dt;
        }

        public DataTable GetRoomViewDetails(string CultureId)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("B_GetRoomView_TB_TypeView_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Cultureid", CultureId);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();
            return dt;
        }

        public DataTable GetHotelType(string culture)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("B_GetHotelType_TB_TypeHotel_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Cultureid", culture);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();
            return dt;
        }

        public DataTable GetHotelAccommodation(string culture)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("B_GetHotelAccommodation_TB_TypeHotelAccommodation_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Cultureid", culture);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();
            return dt;
        }

        #endregion


        #region Searchbox

        //public DataTable GetTextMessages(string CultureCode, string MessageCode)
        //{
        //    DataTable dt = new DataTable();
        //    con.Open();
        //    SqlCommand cmd = new SqlCommand("B_TextMessage_BizTbl_Message_SP", con);
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.Parameters.AddWithValue("@CultureCode", CultureCode);
        //    cmd.Parameters.AddWithValue("@MessageCode", MessageCode);
        //    SqlDataAdapter da = new SqlDataAdapter(cmd);
        //    da.Fill(dt);
        //    con.Close();
        //    return dt;
        //}

        public DataTable GetTextMessages(string CultureCode, string MessageCode, string lblId)
        {
            DataTable dt = new DataTable();
            con.Open();
            SqlCommand cmd = new SqlCommand("B_TextMessage_BizTbl_Message_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CultureCode", CultureCode);
            cmd.Parameters.AddWithValue("@MessageCode", MessageCode);
            cmd.Parameters.AddWithValue("@lblId", lblId);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            return dt;
        }

        public int GetEndIndexValue(string Code)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("B_GetEndIndex_BizTbl_Parameter_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Code", Code);
            int i = Convert.ToInt32(cmd.ExecuteScalar());
            con.Close();
            return i;
        }

        //public DataTable GetddlGuests(string CultureCode, string MessageCode1, string MessageCode2, string MessageCode3)
        //{
        //    DataTable dt = new DataTable();
        //    con.Open();
        //    SqlCommand cmd = new SqlCommand("B_GetdrpGuests_BizTbl_Message_SP", con);
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.Parameters.AddWithValue("@CultureCode", CultureCode);
        //    cmd.Parameters.AddWithValue("@MessageCode1", MessageCode1);
        //    cmd.Parameters.AddWithValue("@MessageCode2", MessageCode2);
        //    cmd.Parameters.AddWithValue("@MessageCode3", MessageCode3);
        //    SqlDataAdapter da = new SqlDataAdapter(cmd);
        //    da.Fill(dt);
        //    con.Close();
        //    return dt;
        //}

        #endregion

        public DataTable GetCulturecode()
        {
            DataTable dt = new DataTable();
            con.Open();
            SqlCommand cmd = new SqlCommand("B_Getculture_BizTbl_Culture", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            return dt;
        }


        #region Language

        public DataSet GetCity(string Cultureid, string city)
        {

            con.Open();

            SqlCommand cmd = new SqlCommand("B_GetCity_TB_Hotel_SP", con);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Cultureid", Cultureid);

            cmd.Parameters.AddWithValue("@city", city);

            SqlDataAdapter da = new SqlDataAdapter(cmd);

            DataSet ds = new DataSet();

            da.Fill(ds);

            con.Close();

            return ds;
        }


        public DataTable GetHotels(string city, string Cultureid)
        {
            con.Open();

            SqlCommand cmd = new SqlCommand("B_GetHotels_TB_Hotel_SP", con);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@city", city);

            cmd.Parameters.AddWithValue("@Cultureid", Cultureid);

            SqlDataAdapter sda = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();

            sda.Fill(dt);

            con.Close();

            return dt;

        }


        #endregion


        public DataTable Getpopularcity(string CultureCode, string Code, string Code1)
        {
            DataTable dt = new DataTable();
            con.Open();
            SqlCommand cmd = new SqlCommand("B_GetCity_Region_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CultureCode", CultureCode);
            cmd.Parameters.AddWithValue("@Code", Code);
            cmd.Parameters.AddWithValue("@Code1", Code1);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            return dt;
        }



        #region topdestination

        public DataSet Gettopdestination(string CultureCode, string Code)
        {
            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Connection"].ConnectionString);

            con.Open();
            SqlCommand cmd = new SqlCommand("B_Getcountry_Country_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CultureCode", CultureCode);
            cmd.Parameters.AddWithValue("@Code", Code);

            SqlDataAdapter da = new SqlDataAdapter(cmd);

            DataSet ds = new DataSet();

            da.Fill(ds);

            con.Close();

            return ds;
        }


        public DataTable GetContinents(string CultureCode)
        {
            DataTable dt = new DataTable();
            con.Open();
            SqlCommand cmd = new SqlCommand("BT_GetContinents_TB_Continents", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CultureCode", CultureCode);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            return dt;
        }
        public DataTable GetContinentsCountry(string culture)
        {
            DataTable dt = new DataTable();
            con.Open();
            SqlCommand cmd = new SqlCommand("B_continentscountry_country_sp", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CultureCode", culture);
            cmd.Parameters.AddWithValue("@Continetid", 9);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            return dt;
        }

        public DataTable GetCountryWithContinents(string culture, int continetId)
        {
            DataTable dt = new DataTable();
            con.Open();
            SqlCommand cmd = new SqlCommand("B_continentscountry_country_sp", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CultureCode", culture);
            cmd.Parameters.AddWithValue("@Continetid", continetId);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            return dt;
        }

        #endregion  topdestination


        #region SearchResult
        //public string guestcount { get; set; }

        public DataTable GetHotelRoomPrice(string CultureCode, string HotelID, string Datefrom, string Dateto)
        {
            DataTable dt = new DataTable();
            con.Open();
            SqlCommand cmd = new SqlCommand("B_GetHotelRoomPrice_TB_HotelRate_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Culture", CultureCode);
            cmd.Parameters.AddWithValue("@OrderBy", "ID");
            cmd.Parameters.AddWithValue("@HotelID", HotelID);
            cmd.Parameters.AddWithValue("@StartDate", Datefrom);
            cmd.Parameters.AddWithValue("@EndDate", Dateto);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            return dt;
        }

        public DataSet GetAllAvailableHotels(string CultureID, int PageSize, int PageIndex, string HotelType)
        {
            DataSet ds = new DataSet();
            // DataTable dt = new DataTable();
            con.Open();
            SqlCommand cmd = new SqlCommand("[B_GetHotelsAll_TB_Hotel_SP]", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Culture", CultureID);
            cmd.Parameters.AddWithValue("@OrderBy", "ID");
            cmd.Parameters.AddWithValue("@PagingSize", PageSize);
            cmd.Parameters.AddWithValue("@PageIndex", PageIndex);
            cmd.Parameters.AddWithValue("@HotelType", HotelType);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            return ds;
        }

        public string GetPeopleCountText(int RoomCountNo, string Adultcount, string Childrencount)
        {
            string peopleCountText = string.Empty;
            // List<string> Guestlist = new List<string>();
            if (Adultcount != "" && Childrencount != "")
            {
                //for (int i = 0; i < RoomCountNo; i++)
                //{
                //    Guestlist.Add(Adultcount + "," + Childrencount);
                //}
                for (int i = 1; i <= RoomCountNo; i++)
                {
                    peopleCountText += (Adultcount + "," + Childrencount + ";");
                }

            }
            return peopleCountText;
        }

        public DataSet SearchHotels(string CultureID, string RegionID, string CheckInDate, string CheckOutDate, int RoomCountNo, string Adultcount, string Childrencount, int PageSize, int PageIndex, string CountryID)
        {
            DataSet ds = new DataSet();
            // DataTable dt = new DataTable();
            con.Open();
            SqlCommand cmd = new SqlCommand("B_GetSearchHotelsResults_TB_Hotel_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Culture", CultureID);
            cmd.Parameters.AddWithValue("@OrderBy", "Sort");
            cmd.Parameters.AddWithValue("@PagingSize", PageSize);
            cmd.Parameters.AddWithValue("@PageIndex", PageIndex);
            cmd.Parameters.AddWithValue("@HotelID", null);
            cmd.Parameters.AddWithValue("@Name", null);
            cmd.Parameters.AddWithValue("@FirmID", null);
            cmd.Parameters.AddWithValue("@CountryID", null);
            cmd.Parameters.AddWithValue("@CityID", null);
            cmd.Parameters.AddWithValue("@RegionIDs", RegionID);

            cmd.Parameters.AddWithValue("@RoomCount", RoomCountNo);


            string Peoplecount = GetPeopleCountText(RoomCountNo, Adultcount, Childrencount);

            if (Adultcount != "" && Childrencount != "")
            {
                cmd.Parameters.AddWithValue("@GuestCount", Peoplecount);
                //int GuestCount = Convert.ToInt32(Adultcount) + Convert.ToInt32(Childrencount);
                //cmd.Parameters.AddWithValue("@GuestCount", GuestCount);
            }


            if (!string.IsNullOrEmpty(CheckInDate))
            {
                SqlParameter parameter = cmd.Parameters.Add("@CheckInDate", System.Data.SqlDbType.Date);
                parameter.Value = DateTime.ParseExact(CheckInDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            else
            {
                cmd.Parameters.AddWithValue("@CheckInDate", null);
            }
            if (!string.IsNullOrEmpty(CheckOutDate))
            {
                SqlParameter parameter1 = cmd.Parameters.Add("@CheckOutDate", System.Data.SqlDbType.Date);
                parameter1.Value = DateTime.ParseExact(CheckOutDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            }
            else
            {
                cmd.Parameters.AddWithValue("@CheckOutDate", null);
            }

            if (!string.IsNullOrEmpty(CountryID))
            {
                cmd.Parameters.AddWithValue("@UserCountryID", Convert.ToInt32(CountryID));
            }



            //if (CheckInDate != "")
            //{
            //    DateTime checkinDT = DateTime.ParseExact(CheckInDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            //    cmd.Parameters.AddWithValue("@CheckInDate", checkinDT);
            //}
            //else
            //{
            //    cmd.Parameters.AddWithValue("@CheckInDate", null);
            //}

            //if (CheckOutDate != "")
            //{
            //    DateTime checkoutDT = DateTime.ParseExact(CheckOutDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            //    cmd.Parameters.AddWithValue("@CheckOutDate", checkoutDT);
            //}
            //else
            //{
            //    cmd.Parameters.AddWithValue("@CheckOutDate", null);
            //}


            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();

            return ds;
        }


        public DataTable GetPropertyType(string Cultureid)
        {
            DataTable dt = new DataTable();
            con.Open();

            SqlCommand cmd = new SqlCommand("B_GetTypeHotel_TB_TypeHotel_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CultureId", Cultureid);

            SqlDataAdapter sda = new SqlDataAdapter(cmd);

            sda.Fill(dt);
            con.Close();
            return dt;

        }


        public DataTable GetHotelClass(string culture)
        {
            DataTable dt = new DataTable();
            con.Open();
            SqlCommand cmd = new SqlCommand("B_GetHotelClass_TB_TypeHotelClass_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CultureId", culture);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(dt);
            con.Close();
            return dt;
        }

        public DataTable GetPropertyFacilities(string Cultureid)
        {
            DataTable dt = new DataTable();
            con.Open();
            SqlCommand cmd = new SqlCommand("B_GetAttributes_TB_Attribute_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CultureId", Cultureid);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(dt);
            con.Close();
            return dt;
        }

        public DataTable GetPropertyRegion(string Cultureid, string RegionID)
        {
            DataTable dt = new DataTable();
            con.Open();
            SqlCommand cmd = new SqlCommand("TB_SP_GetRegions", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ID", null);
            cmd.Parameters.AddWithValue("@CountryID", null);
            cmd.Parameters.AddWithValue("@ParentID", RegionID);
            cmd.Parameters.AddWithValue("@SecondParentID", null);
            cmd.Parameters.AddWithValue("@Name", null);
            cmd.Parameters.AddWithValue("@RegionType", null);
            cmd.Parameters.AddWithValue("@IsPopular", null);
            cmd.Parameters.AddWithValue("@IsFilter", true);
            cmd.Parameters.AddWithValue("@IsMainPageDisplay", null);
            cmd.Parameters.AddWithValue("@IsIncludedInDestinationSearch", null);
            cmd.Parameters.AddWithValue("@IsCity", null);
            cmd.Parameters.AddWithValue("@Culture", Cultureid);
            cmd.Parameters.AddWithValue("@OrderBy", "Name");
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(dt);
            con.Close();
            return dt;
        }


        public DataSet GetSearchResultsHotels(string CultureID, string RegionID,string hids="")
        {

            DataSet ds = new DataSet();
            con.Open();

            SqlCommand cmd = new SqlCommand("TB_SP_GetHotels", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@OrderBy", "ID");
            cmd.Parameters.AddWithValue("@Culture", CultureID);
            cmd.Parameters.AddWithValue("@PagingSize", Int32.MaxValue);
            cmd.Parameters.AddWithValue("@PageIndex", 1);
            cmd.Parameters.AddWithValue("@RegionIDs", RegionID);
            if (!string.IsNullOrEmpty(hids))
                cmd.Parameters.AddWithValue("@HotelIDs", hids);
            cmd.Parameters.AddWithValue("@Active", true);

            //SqlCommand cmd = new SqlCommand("B_GetHotelsMapForSearch_TB_Hotel_SP", con);
            //cmd.CommandType = CommandType.StoredProcedure;
            //cmd.Parameters.AddWithValue("@Culture", CultureID);
            //cmd.Parameters.AddWithValue("@RegionID", RegionID);
            //SqlDataAdapter da = new SqlDataAdapter(cmd);
            //DataTable dt = new DataTable();
            //da.Fill(dt);  
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            return ds;
        }

        public DataSet FilterHotelSearch(string CultureID, string RegionID, string TypeHotelClass, string TypeHotel, string TypeFacilities, string TypeRegion, double LowerUSDPrice, double UpperUSDPrice, string SelectedSortValue, string CheckInDate, string CheckOutDate, int PageSize, int PageIndex, int RoomCount, string Adultcount, string Childrencount)
        {
            try
            {
                DataSet ds = new DataSet();
                // DataTable dt = new DataTable();
                con.Open();
                SqlCommand cmd = new SqlCommand("B_GetSearchHotelsResults_TB_Hotel_SP", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Culture", CultureID);
                cmd.Parameters.AddWithValue("@OrderBy", SelectedSortValue);
                cmd.Parameters.AddWithValue("@PagingSize", PageSize);
                cmd.Parameters.AddWithValue("@PageIndex", PageIndex);
                cmd.Parameters.AddWithValue("@HotelID", null);
                cmd.Parameters.AddWithValue("@Name", null);
                cmd.Parameters.AddWithValue("@FirmID", null);
                cmd.Parameters.AddWithValue("@CountryID", null);
                cmd.Parameters.AddWithValue("@CityID", null);
                // cmd.Parameters.AddWithValue("@RegionIDs", RegionID);

                cmd.Parameters.AddWithValue("@LowerUSDPrice", LowerUSDPrice);
                cmd.Parameters.AddWithValue("@UpperUSDPrice", UpperUSDPrice);

                if (!string.IsNullOrEmpty(TypeHotelClass))
                {
                    cmd.Parameters.AddWithValue("@HotelClassIDs", TypeHotelClass);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@HotelClassIDs", null);
                }
                if (!string.IsNullOrEmpty(TypeHotel))
                {
                    cmd.Parameters.AddWithValue("@HotelTypeIDs", TypeHotel);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@HotelTypeIDs", null);
                }
                if (!string.IsNullOrEmpty(TypeFacilities))
                {
                    cmd.Parameters.AddWithValue("@HotelAttributeIDs", TypeFacilities);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@HotelAttributeIDs", null);
                }
                if (!string.IsNullOrEmpty(TypeRegion))
                {
                    cmd.Parameters.AddWithValue("@RegionIDs", TypeRegion);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@RegionIDs", RegionID);
                }

                if (!string.IsNullOrEmpty(CheckInDate))
                {
                    DateTime checkinDT = DateTime.ParseExact(CheckInDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    cmd.Parameters.AddWithValue("@CheckInDate", checkinDT);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@CheckInDate", null);
                }

                if (!string.IsNullOrEmpty(CheckOutDate))
                {
                    DateTime checkoutDT = DateTime.ParseExact(CheckOutDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    cmd.Parameters.AddWithValue("@CheckOutDate", checkoutDT);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@CheckOutDate", null);
                }

                cmd.Parameters.AddWithValue("@RoomCount", RoomCount);


                string Peoplecount = GetPeopleCountText(RoomCount, Adultcount, Childrencount);

                if (Adultcount != "" && Childrencount != "")
                {
                    cmd.Parameters.AddWithValue("@GuestCount", Peoplecount);
                    //int GuestCount = Convert.ToInt32(Adultcount) + Convert.ToInt32(Childrencount);
                    //cmd.Parameters.AddWithValue("@GuestCount", GuestCount);
                }


                //cmd.Parameters.AddWithValue("@CheckInDate", DateTime.Now);
                //cmd.Parameters.AddWithValue("@CheckOutDate", DateTime.Now);

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    cmd.CommandTimeout = 0;
                    da.Fill(ds);
                }
                con.Close();
                return ds;
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }
        }


        public DataTable GetSortByValues(string Cultureid)
        {
            DataTable dt = new DataTable();
            con.Open();
            SqlCommand cmd = new SqlCommand("B_GetSortBy_TB_TypeHotelSearchSort_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CultureID", Cultureid);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(dt);
            con.Close();
            return dt;
        }

        public DataTable GetMyViewedHotels(string Cultureid, string UserID)
        {
            DataTable dt = new DataTable();
            con.Open();
            SqlCommand cmd = new SqlCommand("B_GetMyViewedHotels_TB_HotelSearchHistory_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CultureID", Cultureid);
            cmd.Parameters.AddWithValue("@UserID", UserID);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(dt);
            con.Close();
            return dt;
        }
        public int DeleteMyViewedHotels(string ID, string User)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("B_DeleteMyViewedhotels_TB_HotelSearchHistory", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@searchID", ID);
            cmd.Parameters.AddWithValue("@UserID", User);
            int status = Convert.ToInt32(cmd.ExecuteNonQuery());
            return status;
        }

        #endregion



        #region HotelInformation

        public string LocationDetails { get; set; }
        public string RegionDetails { get; set; }
        public string ClosestAirportID { get; set; }
        public string ClosestAirportNameWithParentNameAndCode { get; set; }
        public string TypeID { get; set; }
        public string Distance { get; set; }


        public List<string> HotelPhotosThumb { get; set; }
        public string TotalRecords { get; set; }
        public double AverageReviewPointCount { get; set; }
        public string RoomTypeInfo { get; set; }


        public DataTable GetCancellationPolicy(string HotelID, string Culture)
        {
            DataTable dt = new DataTable();
            con.Open();
            SqlCommand cmd = new SqlCommand("[TB_SP_GetHotelCancelPolicy]", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HotelID", HotelID);
            cmd.Parameters.AddWithValue("@Culture", Culture);
            cmd.Parameters.AddWithValue("@OrderBy", "ID");
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            return dt;
        }

        public DataTable objGetRoutingInfo(string HotelID)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("B_GetRoutingDetails_TB_Hotel_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HotelID", HotelID);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();
            return dt;
        }


        public DataTable GetLastHotelBookingMade(string HotelID, string CultureCode)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("B_LastBookingMade_TB_Reservation_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Culture", CultureCode);
            cmd.Parameters.AddWithValue("@HotelID", HotelID);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();
            return dt;
        }

        public DataSet GetCount(string CityId, string CountryID)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("B_GetCount_TB_Region_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CityId", CityId);
            cmd.Parameters.AddWithValue("@CountryID", CountryID);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            con.Close();
            return ds;
        }

        public int CheckuserWishlistStaus(string HotelID, string UserID)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("B_CheckWishlistByUser_TB_WishLists_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserID", UserID);
            cmd.Parameters.AddWithValue("@HotelID", HotelID);
            int status = Convert.ToInt32(cmd.ExecuteScalar());
            con.Close();
            return status;
        }
        public string InsertIntoWishlist(string HotelID, string UserID)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("B_Insert_TB_WishLists_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserID", UserID);
            cmd.Parameters.AddWithValue("@HotelID", HotelID);
            string status = Convert.ToString(cmd.ExecuteScalar());
            con.Close();
            return status;
        }


        public string CheckUserReservationReviewStatus(string UserID, string HotelID)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("B_CheckReviewStatusByUser_TB_ReservationReview_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserID", UserID);
            cmd.Parameters.AddWithValue("@HotelID", HotelID);
            string status = Convert.ToString(cmd.ExecuteScalar());
            con.Close();
            return status;
        }

        public DataTable GetRequiredMessageCode(string CultureCode)
        {
            DataTable dt = new DataTable();
            con.Open();
            SqlCommand cmd = new SqlCommand("B_GetMessagecodeValues_BizTbl_Message_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Culture", CultureCode);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            return dt;
        }
        public int insertreview(string UserId, string traveltype, string travelerdate, string positive, string negative, string name, string Averagepoint, string email, string location, string ReservationId, string Mynamedisplay)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("B_insertreview_Reservationreview_sp", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserId", UserId);
            cmd.Parameters.AddWithValue("@traveltype", traveltype);
            cmd.Parameters.AddWithValue("@Name", name);
            cmd.Parameters.AddWithValue("@travelerdate", travelerdate);
            cmd.Parameters.AddWithValue("@positive", positive);
            cmd.Parameters.AddWithValue("@negative", negative);
            cmd.Parameters.AddWithValue("@Averagepoint", Averagepoint);
            cmd.Parameters.AddWithValue("@email", email);
            cmd.Parameters.AddWithValue("@location", location);
            cmd.Parameters.AddWithValue("@ReservationId", ReservationId);
            cmd.Parameters.AddWithValue("@Anonymous", Convert.ToBoolean(Mynamedisplay));
            int i = Convert.ToInt32(cmd.ExecuteScalar());
            con.Close();
            return i;
        }

        public int insertreviewdetails(int ReviewID, string Cleaning, string Location1, string Comfort, string Service, string Facilities, string Checkin, string Valueofmoney)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("B_InsertReviewDetails_TB_ReservationReviewDetail_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ReviewID", ReviewID);
            //cmd.Parameters.AddWithValue("@Anonymous", Convert.ToBoolean(Mynamedisplay));
            cmd.Parameters.AddWithValue("@Cleaning", Cleaning);
            cmd.Parameters.AddWithValue("@Location1", Location1);
            cmd.Parameters.AddWithValue("@Comfort", Comfort);
            cmd.Parameters.AddWithValue("@Service", Service);
            cmd.Parameters.AddWithValue("@Facilities", Facilities);
            cmd.Parameters.AddWithValue("@Checkin", Checkin);
            cmd.Parameters.AddWithValue("@Valueofmoney", Valueofmoney);
            int i = cmd.ExecuteNonQuery();
            con.Close();
            return i;
        }

        public DataTable writereview(string Cultureid)
        {


            con.Open();

            SqlCommand cmd = new SqlCommand("B_WriteReview_sp", con);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@CultureCode", Cultureid);

            SqlDataAdapter da = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();

            da.Fill(dt);

            con.Close();

            return dt;
        }

        public DataTable GetTypeReview(string Cultureid)
        {


            con.Open();

            SqlCommand cmd = new SqlCommand("B_GetTypeReview_TB_TypeReview", con);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@CultureCode", Cultureid);

            SqlDataAdapter da = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();

            da.Fill(dt);

            con.Close();

            return dt;
        }



        public DataTable GetSlideShowImage(string culture, string hotelId)
        {
            DataTable dt = new DataTable();
            con.Open();
            SqlCommand cmd = new SqlCommand("B_GetHotelImage_TB_Photo", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CultureCode", culture);
            cmd.Parameters.AddWithValue("@HotelID", hotelId);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            return dt;
        }

        public string UpdateHotelSearchHistory(string UserID, string HotelID, string IPAddress)
        {
            //int i = 0;
            con.Open();
            SqlCommand cmd = new SqlCommand("B_HotelHistory_TB_HotelSearchHistory_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserID", UserID);
            cmd.Parameters.AddWithValue("@HotelID", HotelID);
            //cmd.Parameters.AddWithValue("@GuestID", GuestID);
            cmd.Parameters.AddWithValue("@IPAddress", IPAddress);
            //  i = Convert.ToInt32(cmd.ExecuteNonQuery());
            string ID = Convert.ToString(cmd.ExecuteScalar());
            return ID;
        }


        public DataTable GetHotelBasicInfo(string CultureCode, string HotelID)
        {
            DataTable dt = new DataTable();
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            con.Open();
            SqlCommand cmd = new SqlCommand("[B_GetHotelByID_TB_Hotel_SP]", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Culture", CultureCode);
            cmd.Parameters.AddWithValue("@HotelID", HotelID);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            return dt;
        }

        public DataSet GetRoomAvailabilityDetails(string CultureCode, string HotelID)
        {
            DataSet ds = new DataSet();
            con.Open();
            SqlCommand cmd = new SqlCommand("B_GetHotelRoomDetails_TB_HotelRoom_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Culture", CultureCode);
            cmd.Parameters.AddWithValue("@HotelID", HotelID);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            return ds;
        }

        public DataTable CheckroomAvailbility(string CultureCode, string HotelID, string Datefrom, string Dateto, bool ShowSecret)
        {
            DataTable dt = new DataTable();
            con.Open();
            SqlCommand cmd = new SqlCommand("[B_GetHotelRoomAvailability_TB_HotelRate_SP]", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Culture", CultureCode);
            cmd.Parameters.AddWithValue("@OrderBy", "ID,RoomPrice");
            cmd.Parameters.AddWithValue("@HotelID", HotelID);
            cmd.Parameters.AddWithValue("@StartDate", Datefrom);
            cmd.Parameters.AddWithValue("@EndDate", Dateto);
            cmd.Parameters.AddWithValue("@SecretDeal", ShowSecret);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            try
            {
                da.Fill(dt);
            }
            catch
            {
            }
            con.Close();
            return dt;
        }

        public DataTable GetHotelsbyHotelID(string CultureID, string HotelID)
        {


            con.Open();

            SqlCommand cmd = new SqlCommand("[B_GetHotelsMapByHotelId_TB_Hotel_SP]", con);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Culture", CultureID);

            cmd.Parameters.AddWithValue("@HotelID", HotelID);

            SqlDataAdapter da = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();

            da.Fill(dt);

            con.Close();

            return dt;
        }


        public DataTable GetHoteltax(string CultureCode, string HotelID)
        {
            DataTable dt = new DataTable();
            con.Open();
            SqlCommand cmd = new SqlCommand("BT_GETExtratax_TB_HotelAttribute_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Culture", CultureCode);
            cmd.Parameters.AddWithValue("@HotelID", HotelID);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            return dt;
        }

        public DataTable Getallreviews(string CultureID, string HotelID)
        {


            con.Open();

            SqlCommand cmd = new SqlCommand("TB_SP_GetReservationReviews1", con);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Culture", CultureID);

            cmd.Parameters.AddWithValue("@HotelID", HotelID);
            // cmd.Parameters.AddWithValue("@PartID", 1);
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();

            da.Fill(dt);

            con.Close();

            return dt;
        }

        public string gettypescale(string averegepoint1, string CultureID)
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            con.Open();
            SqlCommand cmd = new SqlCommand("B_GetTypescalename_TB_TypeReviewScale_Sp", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@averegepoint", averegepoint1);
            cmd.Parameters.AddWithValue("@Culture", CultureID);
            string i = Convert.ToString(cmd.ExecuteScalar());
            con.Close();
            return i;

        }
        public DataTable GetHotelSpecifications(string CultureID, string HotelID)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("B_GetSpecification_TB_HotelRoom_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Culture", CultureID);
            cmd.Parameters.AddWithValue("@HotelID", HotelID);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();
            return dt;
        }


        public DataTable GetHotelMainregionbyid(string CurrentCulture, int HotelID)
        {
            DataTable dt = new DataTable();
            con.Open();
            SqlCommand cmd = new SqlCommand("TB_SP_GetHotelRegions", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Culture", CurrentCulture);
            cmd.Parameters.AddWithValue("@OrderBy", "ID");
            cmd.Parameters.AddWithValue("@HotelID", HotelID);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(dt);
            con.Close();
            return dt;
        }

        public DataTable GetHotelMainregion(string CultureID, string CountryID, string RegionID)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("B_Ex_GetRegions_TB_Region_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CountryID", CountryID);
            cmd.Parameters.AddWithValue("@Culture", CultureID);
            cmd.Parameters.AddWithValue("@OrderBy", "Name Asc");
            cmd.Parameters.AddWithValue("@SecondParentID", RegionID);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();
            return dt;
        }




        public DataTable GetHotelDiscover(string CultureID, string HotelID)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("B_GetHotelDiscover_TB_HotelRoom_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Culture", CultureID);
            cmd.Parameters.AddWithValue("@HotelID", HotelID);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();
            return dt;
        }
        public DataTable GetRegionsInVicinity(string Cultureid, string CenterLatitude, string CenterLongitude, string radius, string CountryId, string RegionType)
        {
            DataTable dt = new DataTable();
            con.Open();
            SqlCommand cmd = new SqlCommand("TB_SP_GetRegionsInVicinity", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Culture", Cultureid);
            cmd.Parameters.AddWithValue("@CenterLatitude", CenterLatitude);
            cmd.Parameters.AddWithValue("@CenterLongitude", CenterLongitude);
            cmd.Parameters.AddWithValue("@Radius", radius);
            cmd.Parameters.AddWithValue("@CountryID", CountryId);
            cmd.Parameters.AddWithValue("@RegionType", RegionType);

            if (radius == "20000")
            {
                cmd.Parameters.AddWithValue("@IsCity", false);
            }
            else
            {
                cmd.Parameters.AddWithValue("@IsCity", null);
            }
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            return dt;
        }


        public DataTable GetHotelRegionsData(string Id, string Cultureid, string HotelID)
        {
            DataTable dt = new DataTable();
            con.Open();
            SqlCommand cmd = new SqlCommand("TB_SP_GetHotelRegions", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@OrderBy", Id);
            cmd.Parameters.AddWithValue("@Culture", Cultureid);
            cmd.Parameters.AddWithValue("@HotelID", HotelID);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            return dt;

        }


        public DataSet PageLoadGetHotelInformation(string OrderBy, string Culture, int PagingSize, int PageIndex, int TotalRecordCount, string HotelID)
        {
            DataSet ds = new DataSet();
            con.Open();
            SqlCommand cmd = new SqlCommand("TB_SP_GetHotels", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@OrderBy", OrderBy);
            cmd.Parameters.AddWithValue("@Culture", Culture);
            cmd.Parameters.AddWithValue("@PagingSize", PagingSize);
            cmd.Parameters.AddWithValue("@PageIndex", PageIndex);
            cmd.Parameters.AddWithValue("@HotelID", HotelID);
            cmd.Parameters.AddWithValue("@Active", true);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            return ds;

        }




        public DataSet GetHotelRoomPopUpDetails(string HotelId, string RoomId, string CultureID)
        {
            DataSet ds = new DataSet();
            con.Open();
            SqlCommand cmd = new SqlCommand("B_GetRoomDetailsByID_HotelRoom_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HotelId", HotelId);
            cmd.Parameters.AddWithValue("@RoomId", RoomId);
            cmd.Parameters.AddWithValue("@CultureID", CultureID);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            return ds;
        }

        public DataTable GetHotelRoomIcons(string HotelId, string RoomId, string CultureID)
        {
            DataTable ds = new DataTable();
            con.Open();
            SqlCommand cmd = new SqlCommand("[B_GetHotelAttributeIcon_HotelAttribute_SP]", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HotelId", HotelId);
            cmd.Parameters.AddWithValue("@RoomId", RoomId);
            cmd.Parameters.AddWithValue("@CultureID", CultureID);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            return ds;
        }




        #endregion

        #region Book

        public string GetTextMessagesAsString(string CultureCode, string MessageCode)
        {
            DataTable dt = new DataTable();
            con.Open();
            SqlCommand cmd = new SqlCommand("B_SingleTextMessage_BizTbl_Message_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CultureCode", CultureCode);
            cmd.Parameters.AddWithValue("@MessageCode", MessageCode);
            string Message = Convert.ToString(cmd.ExecuteScalar());
            con.Close();
            return Message;
        }
        public DataTable typeCreditCardByHotel(string Cultureid, string HotelID)
        {
            con.Open();
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand("B_GetHotelCreditCards_HotelCreditCard_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Culture", Cultureid);
            cmd.Parameters.AddWithValue("@OrderBy", "ID");
            cmd.Parameters.AddWithValue("@HotelID", HotelID);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(dt);
            con.Close();
            return dt;
        }

        public DataTable GetHotelAttributesByReservationDate(string Cultureid, string HotelID)
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            con.Open();
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand("TB_SP_GetHotelAttributes", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Culture", Cultureid);
            cmd.Parameters.AddWithValue("@OrderBy", "ID");
            cmd.Parameters.AddWithValue("@HotelID", HotelID);
            cmd.Parameters.AddWithValue("@Date", DateTime.Now.ToString("yyyy/MM/dd"));
            cmd.Parameters.AddWithValue("@AttributeTypeID", 2);
            cmd.Parameters.AddWithValue("@Active", 1);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(dt);
            con.Close();
            return dt;
        }

        public DataTable GetMailTemplates(string MaiTemplateCode, string Culturecode)
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            con.Open();
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand("B_GetMailTemplate_BizTbl_MailTemplate_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@MailTemplateCode", MaiTemplateCode);
            cmd.Parameters.AddWithValue("@Culture", Culturecode);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(dt);
            con.Close();
            return dt;
        }

        public DataTable TypeMonth(string Cultureid)
        {
            con.Open();
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand("B_GetMonth_TB_TypeMonth_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Culture", Cultureid);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(dt);
            con.Close();
            return dt;
        }

        public DataTable TypeTraveller(string Cultureid)
        {
            con.Open();
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand("B_GetTraveller_TB_TypeTraveller_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Culture", Cultureid);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(dt);
            con.Close();
            return dt;
        }

        public DataTable GetvaluesWhileChangelang(string Cultureid, string HotelID, string RoomPriceType, string CheckDatefrom, string CheckDateto, string PolicyTypeIDArry, string HotelRoomID, bool ShowSecret)
        {
            con.Open();
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand("[B_GetHotelRoomAvailabilityByRoomColumn_TB_HotelRate_SP]", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Culture", Cultureid);
            cmd.Parameters.AddWithValue("@OrderBy", "ID");
            cmd.Parameters.AddWithValue("@HotelID", HotelID);
            cmd.Parameters.AddWithValue("@StartDate", CheckDatefrom);
            cmd.Parameters.AddWithValue("@EndDate", CheckDateto);
            cmd.Parameters.AddWithValue("@RoomID", HotelRoomID);
            cmd.Parameters.AddWithValue("@PriceType", RoomPriceType);
            cmd.Parameters.AddWithValue("@PricePolicyType", PolicyTypeIDArry);
            cmd.Parameters.AddWithValue("@SecretDeal", ShowSecret);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(dt);
            con.Close();
            return dt;
        }

        public DataTable getBedSelection(string Cultureid, string HotelID, string HotelRoomnID)
        {
            con.Open();
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand("[dbo].[TB_SP_GetHotelRoomBeds]", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HotelID", HotelID);
            if (HotelRoomnID == "")
            {
                cmd.Parameters.AddWithValue("@HotelRoomID", null);
            }
            else
            {
                cmd.Parameters.AddWithValue("@HotelRoomID", HotelRoomnID);
            }
            cmd.Parameters.AddWithValue("@OrderBy", "ID");
            cmd.Parameters.AddWithValue("@Culture", Cultureid);

            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(dt);
            con.Close();
            return dt;
        }


        public string InsertReservationFunc(string PinCode, string title, string Name, string SurName, string Email, string Phone, string Country,
                             string CreditcardProvider, string NameOnCreditcard, string CreditcardNumber, string CVCCode, string CardExpriryDate, string LoggedUserID, string HotelFirmID, string HotelID, string PayableAmount, string myIP, string Culturecode,
                             string HiidenCurrencyCode, string SpecialNote, int roomRemaining, string Datefromm, string Datetoo, string RoomID,
                             string Datetoo1, string HotelAccommodationTypeID, string Guestname, string MaxpeopleCount,
                             string NoofNights, string PricePolicyTypeID, string SingleRate, string DoubleRate, string BedOptionNo,
                             string EstimatedArrivalTimeSelect, string TravellerTypeID, string RoomPrice, string CancelpolicyID,
                             string Nonrefund, double ComissionRate, double ComissionAmount, string PromotionPercentage)
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            con.Open();
            SqlCommand cmd = new SqlCommand("B_InsertReservation_TB_Reservation_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PinCode", PinCode);
            cmd.Parameters.AddWithValue("@PartID", "1");
            cmd.Parameters.AddWithValue("@FirmID", HotelFirmID);
            cmd.Parameters.AddWithValue("@HotelID", HotelID);
            cmd.Parameters.AddWithValue("@LogUserID", LoggedUserID);
            cmd.Parameters.AddWithValue("@StatusID", "1");
            cmd.Parameters.AddWithValue("@CountryID", Country);
            cmd.Parameters.AddWithValue("@SalutationTypeID", title);
            cmd.Parameters.AddWithValue("@Name", Name);
            cmd.Parameters.AddWithValue("@Surname", SurName);
            cmd.Parameters.AddWithValue("@Email", Email);
            cmd.Parameters.AddWithValue("@Phone", Phone);
            cmd.Parameters.AddWithValue("@Amount", PayableAmount);
            cmd.Parameters.AddWithValue("@PayableAmount", PayableAmount);
            cmd.Parameters.AddWithValue("@ActualAmount", "True");
            cmd.Parameters.AddWithValue("@CurrencyID", HiidenCurrencyCode);
            cmd.Parameters.AddWithValue("@CreditCardUsed", HotelID);
            if (CreditcardProvider == "0")
            {
                cmd.Parameters.AddWithValue("@CCTypeID", null);
            }
            else
            {
                cmd.Parameters.AddWithValue("@CCTypeID", CreditcardProvider);
            }
            cmd.Parameters.AddWithValue("@CCFullName", NameOnCreditcard);
            cmd.Parameters.AddWithValue("@CCNo", CreditcardNumber);
            cmd.Parameters.AddWithValue("@CCExpiration", CardExpriryDate);
            cmd.Parameters.AddWithValue("@CCCVC", CVCCode);
            cmd.Parameters.AddWithValue("@Active", "1");
            cmd.Parameters.AddWithValue("@CultureID", Culturecode);
            cmd.Parameters.AddWithValue("@IPAddress", myIP);
            cmd.Parameters.AddWithValue("@OpUserID", LoggedUserID);

            if (SpecialNote == "")
            {
                cmd.Parameters.AddWithValue("@SpecialNote", null);
            }
            else
            {
                cmd.Parameters.AddWithValue("@SpecialNote", SpecialNote);
            }
            cmd.Parameters.AddWithValue("@GeneralPromotionDiscountPercentage", "0");
            cmd.Parameters.AddWithValue("@PromotionDiscountPercentage", PromotionPercentage);
            cmd.Parameters.AddWithValue("@Address", "Adresss");
            cmd.Parameters.AddWithValue("@ReservationMode", "3");
            cmd.Parameters.AddWithValue("@RoomCountRemaining", roomRemaining);
            cmd.Parameters.AddWithValue("@Checkindate", Datefromm);
            cmd.Parameters.AddWithValue("@CheckoutDate", Datetoo);
            cmd.Parameters.AddWithValue("@RoomID", RoomID);
            // cmd.Parameters.AddWithValue("@CheckoutDate", Datetoo);
            cmd.Parameters.AddWithValue("@HotelAccommodationTypeID", HotelAccommodationTypeID);
            cmd.Parameters.AddWithValue("@GuestFullName", Guestname);
            cmd.Parameters.AddWithValue("@PeopleCount", MaxpeopleCount);
            cmd.Parameters.AddWithValue("@NightCount", NoofNights);
            cmd.Parameters.AddWithValue("@PricePolicyTypeID", PricePolicyTypeID);
            cmd.Parameters.AddWithValue("@SingleRate", SingleRate);
            cmd.Parameters.AddWithValue("@DoubleRate", DoubleRate);
            if (BedOptionNo == "0")
            {
                cmd.Parameters.AddWithValue("@BedOptionNo", null);
            }
            else
            {
                cmd.Parameters.AddWithValue("@BedOptionNo", BedOptionNo);
            }
            if (TravellerTypeID == "0")
            {
                cmd.Parameters.AddWithValue("@TravellerTypeID", null);
            }
            else
            {
                cmd.Parameters.AddWithValue("@TravellerTypeID", TravellerTypeID);
            }
            if (EstimatedArrivalTimeSelect == "0")
            {
                cmd.Parameters.AddWithValue("@EstimatedArrivalTime", null);
            }
            else
            {
                cmd.Parameters.AddWithValue("@EstimatedArrivalTime", EstimatedArrivalTimeSelect);
            }
            cmd.Parameters.AddWithValue("@CheckoutDate1", Datetoo1);
            cmd.Parameters.AddWithValue("@RoomPrice", RoomPrice);
            cmd.Parameters.AddWithValue("@NonRefundable", Nonrefund);
            cmd.Parameters.AddWithValue("@ReservationOperationID", "1");
            cmd.Parameters.AddWithValue("@HotelCancelPolicyID", CancelpolicyID);
            cmd.Parameters.AddWithValue("@ComissionRate", ComissionRate);
            cmd.Parameters.AddWithValue("@ComissionAmount", ComissionAmount);
            //DataTable dt = new DataTable();
            //SqlDataAdapter sda = new SqlDataAdapter(cmd);
            //sda.Fill(dt);
            string Staus = Convert.ToString(cmd.ExecuteScalar());
            con.Close();
            return Staus;
        }

        public string InsertHotelReservationFunc(string ReservationID, string PinCode, string title, string Name, string SurName, string Email, string Phone, string Country,
                             string CreditcardProvider, string NameOnCreditcard, string CreditcardNumber, string CVCCode, string CardExpriryDate, string LoggedUserID, string HotelFirmID, string HotelID, string PayableAmount, string myIP, string Culturecode,
                             string HiidenCurrencyCode, string SpecialNote, int roomRemaining, string Datefromm, string Datetoo, string RoomID,
                             string Datetoo1, string HotelAccommodationTypeID, string Guestname, string MaxpeopleCount,
                             string NoofNights, string PricePolicyTypeID, string SingleRate, string DoubleRate, string BedOptionNo,
                             string EstimatedArrivalTimeSelect, string TravellerTypeID, string RoomPrice, string CancelpolicyID,
                             string Nonrefund, double ComissionRate, double ComissionAmount, string PromotionPercentage)
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            con.Open();
            SqlCommand cmd = new SqlCommand("[B_InsertHotelReservation_TB_Reservation_SP]", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ReservationID", ReservationID);
            cmd.Parameters.AddWithValue("@PinCode", PinCode);
            cmd.Parameters.AddWithValue("@PartID", "1");
            cmd.Parameters.AddWithValue("@FirmID", HotelFirmID);
            cmd.Parameters.AddWithValue("@HotelID", HotelID);
            cmd.Parameters.AddWithValue("@LogUserID", LoggedUserID);
            cmd.Parameters.AddWithValue("@StatusID", "1");
            cmd.Parameters.AddWithValue("@CountryID", Country);
            cmd.Parameters.AddWithValue("@SalutationTypeID", title);
            cmd.Parameters.AddWithValue("@Name", Name);
            cmd.Parameters.AddWithValue("@Surname", SurName);
            cmd.Parameters.AddWithValue("@Email", Email);
            cmd.Parameters.AddWithValue("@Phone", Phone);
            cmd.Parameters.AddWithValue("@Amount", PayableAmount);
            cmd.Parameters.AddWithValue("@PayableAmount", PayableAmount);
            cmd.Parameters.AddWithValue("@ActualAmount", "True");
            cmd.Parameters.AddWithValue("@CurrencyID", HiidenCurrencyCode);
            cmd.Parameters.AddWithValue("@CreditCardUsed", HotelID);
            if (CreditcardProvider == "0")
            {
                cmd.Parameters.AddWithValue("@CCTypeID", null);
            }
            else
            {
                cmd.Parameters.AddWithValue("@CCTypeID", CreditcardProvider);
            }
            cmd.Parameters.AddWithValue("@CCFullName", NameOnCreditcard);
            cmd.Parameters.AddWithValue("@CCNo", CreditcardNumber);
            cmd.Parameters.AddWithValue("@CCExpiration", CardExpriryDate);
            cmd.Parameters.AddWithValue("@CCCVC", CVCCode);
            cmd.Parameters.AddWithValue("@Active", "1");
            cmd.Parameters.AddWithValue("@CultureID", Culturecode);
            cmd.Parameters.AddWithValue("@IPAddress", myIP);
            cmd.Parameters.AddWithValue("@OpUserID", LoggedUserID);

            if (SpecialNote == "")
            {
                cmd.Parameters.AddWithValue("@SpecialNote", null);
            }
            else
            {
                cmd.Parameters.AddWithValue("@SpecialNote", SpecialNote);
            }
            cmd.Parameters.AddWithValue("@GeneralPromotionDiscountPercentage", "0");
            cmd.Parameters.AddWithValue("@PromotionDiscountPercentage", PromotionPercentage);
            cmd.Parameters.AddWithValue("@Address", "Adresss");
            cmd.Parameters.AddWithValue("@ReservationMode", "3");
            cmd.Parameters.AddWithValue("@RoomCountRemaining", roomRemaining);
            cmd.Parameters.AddWithValue("@Checkindate", Datefromm);
            cmd.Parameters.AddWithValue("@CheckoutDate", Datetoo);
            cmd.Parameters.AddWithValue("@RoomID", RoomID);
            // cmd.Parameters.AddWithValue("@CheckoutDate", Datetoo);
            cmd.Parameters.AddWithValue("@HotelAccommodationTypeID", HotelAccommodationTypeID);
            cmd.Parameters.AddWithValue("@GuestFullName", Guestname);
            cmd.Parameters.AddWithValue("@PeopleCount", MaxpeopleCount);
            cmd.Parameters.AddWithValue("@NightCount", NoofNights);
            cmd.Parameters.AddWithValue("@PricePolicyTypeID", PricePolicyTypeID);
            cmd.Parameters.AddWithValue("@SingleRate", SingleRate);
            cmd.Parameters.AddWithValue("@DoubleRate", DoubleRate);
            if (BedOptionNo == "0")
            {
                cmd.Parameters.AddWithValue("@BedOptionNo", null);
            }
            else
            {
                cmd.Parameters.AddWithValue("@BedOptionNo", BedOptionNo);
            }
            if (TravellerTypeID == "0")
            {
                cmd.Parameters.AddWithValue("@TravellerTypeID", null);
            }
            else
            {
                cmd.Parameters.AddWithValue("@TravellerTypeID", TravellerTypeID);
            }
            if (EstimatedArrivalTimeSelect == "0")
            {
                cmd.Parameters.AddWithValue("@EstimatedArrivalTime", null);
            }
            else
            {
                cmd.Parameters.AddWithValue("@EstimatedArrivalTime", EstimatedArrivalTimeSelect);
            }
            cmd.Parameters.AddWithValue("@CheckoutDate1", Datetoo1);
            cmd.Parameters.AddWithValue("@RoomPrice", RoomPrice);
            cmd.Parameters.AddWithValue("@NonRefundable", Nonrefund);
            cmd.Parameters.AddWithValue("@ReservationOperationID", "1");
            cmd.Parameters.AddWithValue("@HotelCancelPolicyID", CancelpolicyID);
            cmd.Parameters.AddWithValue("@ComissionRate", ComissionRate);
            cmd.Parameters.AddWithValue("@ComissionAmount", ComissionAmount);
            //DataTable dt = new DataTable();
            //SqlDataAdapter sda = new SqlDataAdapter(cmd);
            //sda.Fill(dt);
            string Staus = Convert.ToString(cmd.ExecuteScalar());
            con.Close();
            return Staus;
        }

        public int UpdateRoomRateAfterReservation(string ReservedID, string DateID, string RoomPrice, string CurrencyID, string OpUserID)
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            con.Open();
            SqlCommand cmd = new SqlCommand("B_InsertHotelRoomRate_TB_HotelRoomRate_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ReservedID", ReservedID);
            cmd.Parameters.AddWithValue("@DateID", DateID);
            cmd.Parameters.AddWithValue("@RoomPrice", RoomPrice);
            cmd.Parameters.AddWithValue("@CurrencyID", CurrencyID);
            cmd.Parameters.AddWithValue("@OpUserID", OpUserID);
            int i = cmd.ExecuteNonQuery();
            con.Close();
            return i;
        }



        public DataTable GetHotelComissions(string Cultureid, string HotelID, string Date)
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            con.Open();
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand("[dbo].[TB_SP_GetHotelComissions]", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HotelID", HotelID);
            cmd.Parameters.AddWithValue("@OrderBy", "ID");
            cmd.Parameters.AddWithValue("@Culture", Cultureid);
            cmd.Parameters.AddWithValue("@Date", Date);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(dt);
            con.Close();
            return dt;
        }

        public int UpdatedReservation(string ReservationID, string EncriptedReservationID, string EncriptedPinCode)
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            con.Open();
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand("B_UpadateReservation_Reservation_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@EnCriptedRid", EncriptedReservationID);
            cmd.Parameters.AddWithValue("@EncriptedPincode", EncriptedPinCode);
            cmd.Parameters.AddWithValue("@RID", ReservationID);
            int Status = cmd.ExecuteNonQuery();
            con.Close();
            return Status;
        }


        public string InsertNewUserWhileReservation(string title, string Name, string SurName, string Email, string Phone,
            string Country, string myIP)
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            con.Open();
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand("B_InsertNewUserWhileReservation_BizTbl_User_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@SalutationTypeID", title);
            cmd.Parameters.AddWithValue("@Name", Name);
            cmd.Parameters.AddWithValue("@Surname", SurName);
            cmd.Parameters.AddWithValue("@CountryID", Country);
            cmd.Parameters.AddWithValue("@Email", Email);
            cmd.Parameters.AddWithValue("@Phone", Phone);
            cmd.Parameters.AddWithValue("@IPAddress", myIP);
            string Status = Convert.ToString(cmd.ExecuteScalar());
            con.Close();
            return Status;
        }

        public int CheckPincode(string Pincode)
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            con.Open();
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand("B_ChecKPincode_TB_Reservation_SB", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Pincode", Pincode);
            int Status = Convert.ToInt32(cmd.ExecuteScalar());
            con.Close();
            return Status;
        }

        #endregion Book



        #region Registeration



        public int InsertSubject(string Name, string contact, string Email, string Subject, string Message, string IPAddress, string Userid, string Surname, string Country, string Satulation)
        {
            DataTable dt = new DataTable();
            con.Open();
            SqlCommand cmd = new SqlCommand("B_InsertContactUsInfo_TB_Message_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Subject", Subject);
            cmd.Parameters.AddWithValue("@MessageStatusId", 1);
            cmd.Parameters.AddWithValue("@SatulationId", Satulation);
            cmd.Parameters.AddWithValue("@Name", Name);
            cmd.Parameters.AddWithValue("@SurName", Surname);
            cmd.Parameters.AddWithValue("@Email", Email);
            cmd.Parameters.AddWithValue("@Phone", contact);
            cmd.Parameters.AddWithValue("@countryID", Country);
            cmd.Parameters.AddWithValue("@Text", Message);
            cmd.Parameters.AddWithValue("@Active", 1);
            cmd.Parameters.AddWithValue("@IpAddress", IPAddress);
            cmd.Parameters.AddWithValue("@Createddate", DateTime.Now);
            cmd.Parameters.AddWithValue("@Updateddate", DateTime.Now);
            cmd.Parameters.AddWithValue("@OpUserID", Userid);
            int i = Convert.ToInt32(cmd.ExecuteNonQuery());
            con.Close();
            return i;
        }
        public string Decrypt(string cipherText)
        {
            //  string EncryptionKey = "MAKV2SPBNI99212";
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                //Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                //encryptor.Key = pdb.GetBytes(32);
                //encryptor.IV = pdb.GetBytes(16);

                encryptor.Key = Encoding.UTF8.GetBytes("2428598755421637");
                encryptor.IV = Encoding.UTF8.GetBytes("5369523205842148");

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
            return cipherText;
        }
        public DataTable GetSubject(string Cultureid)
        {
            con.Open();
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand("B_Ex_GetMessageSubject_TB_TypeMessageSubject_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CultureCode", Cultureid);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(dt);
            con.Close();
            return dt;
        }
        public DataTable GetCountry(string culture)
        {
            con.Open();
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand("B_GetCountry_TB_Country_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Cultureid", culture);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(dt);
            con.Close();
            return dt;
        }
        public int GetCountryID(string CountryCode)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("B_GetCountryCode_TB_Country_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CountryCode", CountryCode);
            int CountryID = Convert.ToInt32(cmd.ExecuteScalar());
            con.Close();
            return CountryID;
        }

        public DataTable GetHotelPromotionsDailyDiscountPercentage(string Cultureid, string HotelID, string DateNow, string CheckIn, string Checkout)
        {
            con.Open();
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand("TB_SP_GetHotelPromotionsDailyDiscountPercentage", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Culture", Cultureid);
            cmd.Parameters.AddWithValue("@OrderBy", "ValidForAllRoomTypes DESC, PromotionSort");
            cmd.Parameters.AddWithValue("@HotelIDs", HotelID);
            cmd.Parameters.AddWithValue("@Date", DateNow);
            cmd.Parameters.AddWithValue("@AccommodationStartDate", CheckIn);
            cmd.Parameters.AddWithValue("@AccommodationEndDate", Checkout);

            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(dt);
            con.Close();
            return dt;
        }


        public DataTable GetCityProperty(string CountryID, string culture)
        {
            con.Open();
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand("B_GetCity_TB_Region_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CountryID", CountryID);
            cmd.Parameters.AddWithValue("@Cultureid", culture);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(dt);
            con.Close();
            return dt;
        }


        public DataTable GetSalutation(string Cultureid)
        {
            con.Open();
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand("B_GetSalutation_TB_TypeSalutation_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Cultureid", Cultureid);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(dt);
            con.Close();
            return dt;
        }



        public DataTable Updatedata(string UserId, string SalutationTypeID, string Name, string SurName, string Phone, string CountryID, string Country, string CityID, string City, string Address, string Postcode, string IPaddress)
        {
            DataTable dt = new DataTable();
            con.Open();
            SqlCommand cmd = new SqlCommand("B_Updatedata_BizTbl_User_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserId", UserId);
            cmd.Parameters.AddWithValue("@SalutationTypeID", SalutationTypeID);
            cmd.Parameters.AddWithValue("@Name", Name);
            cmd.Parameters.AddWithValue("@SurName", SurName);
            cmd.Parameters.AddWithValue("@Phone", Phone);
            cmd.Parameters.AddWithValue("@CountryID", CountryID);
            cmd.Parameters.AddWithValue("@Country", Country);
            cmd.Parameters.AddWithValue("@CityID", CityID);
            cmd.Parameters.AddWithValue("@City", City);
            cmd.Parameters.AddWithValue("@Address", Address);
            cmd.Parameters.AddWithValue("@Postcode", Postcode);
            cmd.Parameters.AddWithValue("@IPaddress", IPaddress);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(dt);
            con.Close();
            return dt;
        }
        #endregion


        public DataTable GetCurrencycode(string Cultureid)
        {
            DataTable dt = new DataTable(); if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            con.Open();
            SqlCommand cmd = new SqlCommand("B_GetCurrency_TB_Currency_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Cultureid", Cultureid);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            return dt;
        }

        public string GetTravelllerTypeByCulture(string CultureCode, string TypeID)
        {
            DataTable dt = new DataTable(); if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            con.Open();
            SqlCommand cmd = new SqlCommand("[B_GetHotelTravellerTypeByID_SP]", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Cultureid", CultureCode);
            cmd.Parameters.AddWithValue("@TypeID", TypeID);
            string Message = Convert.ToString(cmd.ExecuteScalar());
            con.Close();
            return Message;
        }
        public int CheckEmailId(string Email)
        {
            DataTable dt = new DataTable(); if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            con.Open();
            SqlCommand cmd = new SqlCommand("B_CheckEmail_BizTbl_User_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Email", Email);
            int i = Convert.ToInt32(cmd.ExecuteScalar());
            con.Close();
            return i;
        }


        public DataTable InsertNewAccount(string Email, string Password, string UserName, string varificationcode)
        {
            DataTable dt = new DataTable(); if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            con.Open();
            SqlCommand cmd = new SqlCommand("B_InsertNewAccount_BizTbl_User_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Email", Email);
            cmd.Parameters.AddWithValue("@Password", Password);
            cmd.Parameters.AddWithValue("@UserName", UserName);
            cmd.Parameters.AddWithValue("@varificationcode", varificationcode);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            return dt;
        }

        public int CheckUsername(string UserName)
        {
            DataTable dt = new DataTable(); if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            con.Open();
            SqlCommand cmd = new SqlCommand("B_CheckUsername_BizTbl_User_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserName", UserName);
            int i = Convert.ToInt32(cmd.ExecuteScalar());
            con.Close();
            return i;
        }


        public DataTable ForgetPassword(string EmailID, string CultureCode)
        {
            DataTable dt = new DataTable();
            con.Open();
            SqlCommand cmd = new SqlCommand("B_ForgetPassword_BizTbl_User_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@EmailID", EmailID);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(dt);
            con.Close();
            return dt;
        }

        public int resetPassword(string Password, string UserId)
        {
            DataTable dt = new DataTable();
            con.Open();
            SqlCommand cmd = new SqlCommand("B_ResetPassword_BizTbl_User_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Password", Password);
            cmd.Parameters.AddWithValue("@UserId", UserId);
            int i = Convert.ToInt32(cmd.ExecuteNonQuery());
            con.Close();
            return i;
        }


        public DataTable GetParameter(string UserRemindLink, string Culturecode)
        {
            con.Open();
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand("B_GetParameter_BizTbl_Parameter_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserRemindLink", UserRemindLink);
            cmd.Parameters.AddWithValue("@Culture", Culturecode);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(dt);
            con.Close();
            return dt;
        }


        #region FAQ

        public DataTable GetFAQ(string culture)
        {
            DataTable dt = new DataTable();
            con.Open();
            SqlCommand cmd = new SqlCommand("B_GetFAQ_FAQ_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CultureCode", culture);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            return dt;
        }
        #endregion


        #region UserDashboard

        public DataTable GetUserDashBoardDetails(string CultureID, string UserID)
        {
            DataTable dt = new DataTable();
            con.Open();
            SqlCommand cmd = new SqlCommand("B_GetUserDetails_BizTbl_User_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            //cmd.Parameters.AddWithValue("@CultureCode", CultureID);
            cmd.Parameters.AddWithValue("@UserID", UserID);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            return dt;
        }

        public DataTable GetRecentActiveReservation(string culture, string userId)
        {
            DataTable dt = new DataTable();
            con.Open();
            SqlCommand cmd = new SqlCommand("B_GetRecentActiveReservation_TB_Reservation_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            //cmd.Parameters.AddWithValue("@CultureCode", CultureID);
            cmd.Parameters.AddWithValue("@UserID", userId);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            return dt;
        }


        public DataTable GetYourReservation(string culture, string userId)
        {
            DataTable dt = new DataTable();
            con.Open();
            SqlCommand cmd = new SqlCommand("B_GetYourReservation_TB_Reservation_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CultureID", culture);
            cmd.Parameters.AddWithValue("@UserID", userId);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            return dt;
        }

        public int DeleteRecentlyViewedDetails(string UserID)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("B_DeleteRecentlyViewDetails_BizTbl_UserOperation_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserID", UserID);
            int i = Convert.ToInt32(cmd.ExecuteNonQuery());
            return i;
        }

        public int SeeAllPastBookings(string UserID)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("B_PastBookings_TB_ReservationHistory_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserID", UserID);
            int i = Convert.ToInt32(cmd.ExecuteScalar());
            return i;
        }

        public int SubscribeSecretDeals(string EmailID, string IPAddress)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("B_SecretDeals_BizTbl_User_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Email", EmailID);
            cmd.Parameters.AddWithValue("@IPAddress", IPAddress);
            int i = cmd.ExecuteNonQuery();
            return i;
        }

        public int RecentlyviewdHotels(string UserID)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("B_RecentlyViewedHotels_TB_HotelSearchHistory_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserID", UserID);
            int i = Convert.ToInt32(cmd.ExecuteScalar());
            return i;
        }

        public DataTable getresendverification(string UserID, string varificationcode)
        {
            DataTable dt = new DataTable();
            con.Open();
            SqlCommand cmd = new SqlCommand("B_Resendverification_BizTbl_User_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserID", UserID);
            cmd.Parameters.AddWithValue("@varificationcode", varificationcode);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            return dt;
        }

        #endregion UserDashboard


        #region MyWishlists


        public DataTable GetMyWishlists(string CultureID, string UserID)
        {
            DataTable dt = new DataTable();
            con.Open();
            SqlCommand cmd = new SqlCommand("B_GetWishlists_TB_WishLists_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CultureID", CultureID);
            cmd.Parameters.AddWithValue("@UserID", UserID);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            return dt;
        }


        #endregion MyWishlists

        #region Bookings

        public string Book { get; set; }
        public string Remove { get; set; }
        public string Price { get; set; }

        public DataTable GetBookingsDetails(string CultureID, string UserID)
        {
            DataTable dt = new DataTable();
            con.Open();
            SqlCommand cmd = new SqlCommand("B_GetBookings_TB_Reservation_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CultureID", CultureID);
            cmd.Parameters.AddWithValue("@UserID ", UserID);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            return dt;
        }

        public int RemoveBookingDetails(string UserID, string ReservationID)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("B_RemoveReservation_TB_Reservation_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserID ", UserID);
            cmd.Parameters.AddWithValue("@ReservationID", ReservationID);
            int i = Convert.ToInt32(cmd.ExecuteNonQuery());
            return i;
        }

        #endregion Bookings


        #region RecentlyViewedHotels

        public DataTable GetRecentlyViewedHotels(string CultureID, string UserID)
        {

            //if (UserID == "" || UserID == null)
            //{
            //    UserID = "GUEST00022";
            //}
            con.Open();
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand("B_GetRecentlyViewedHotels_TB_HotelSearchHistory_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CultureID", CultureID);
            cmd.Parameters.AddWithValue("@UserID", UserID);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(dt);
            con.Close();
            return dt;
        }

        #endregion RecentlyViewedHotels



        public DataTable GetTearmCondition(string culture)
        {
            DataTable dt = new DataTable();
            con.Open();
            SqlCommand cmd = new SqlCommand("B_Gettermsandcondtions_BizTbl_Message_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CultureCode", culture);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            return dt;
        }


        public DataTable UserLogin(string email, string Password)
        {
            DataTable dt = new DataTable();
            con.Open();
            SqlCommand cmd = new SqlCommand("B_UserLogin_BizTbl_User_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Emailid", email);
            cmd.Parameters.AddWithValue("@Password", Password);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            return dt;
        }


        #region MyProfile
        public DataTable getProfileDetails(string userId, string culture)
        {
            DataTable dt = new DataTable();
            con.Open();
            SqlCommand cmd = new SqlCommand("B_GetDetails_BizTbl_User_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserId", userId);
            cmd.Parameters.AddWithValue("@CultureCode", culture);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            return dt;
        }

        public DataTable GetUserCreditCardByHotel(string UserId, string HotelID)
        {
            DataTable dt = new DataTable();
            con.Open();
            SqlCommand cmd = new SqlCommand("B_GetSaveCreditCardsByUser_TB_CreditCardsDetails_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserID", UserId);
            cmd.Parameters.AddWithValue("@HotelID", HotelID);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            return dt;
        }

        public DataTable GetSearchDestinationCity(string Keyword, string CultureCode)
        {
            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Connection"].ConnectionString);

            con.Open();

            SqlCommand cmd = new SqlCommand("B_GetCitySearchResult_TB_Region_SP", con);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Culture", CultureCode);

            cmd.Parameters.AddWithValue("@Keyword", Keyword);

            SqlDataAdapter da = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();

            da.Fill(dt);

            con.Close();

            return dt;
        }

        public int Updateprofile(string UserId, string UserName, string Email, string Name, string SurName, string Phone, string CountryId, string Country, string CityId, string City, string Address, string Postcode)
        {
            DataTable dt = new DataTable();
            con.Open();
            SqlCommand cmd = new SqlCommand("B_Updateprofile_BizTbl_User_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserId", UserId);
            cmd.Parameters.AddWithValue("@UserName", UserName);
            cmd.Parameters.AddWithValue("@Email", Email);
            cmd.Parameters.AddWithValue("@Name", Name);
            cmd.Parameters.AddWithValue("@SurName", SurName);
            cmd.Parameters.AddWithValue("@Phone", Phone);
            cmd.Parameters.AddWithValue("@CountryID", CountryId=="0"?null:CountryId);
            cmd.Parameters.AddWithValue("@Country", Country);
            cmd.Parameters.AddWithValue("@CityID", CityId);
            cmd.Parameters.AddWithValue("@City", City);
            cmd.Parameters.AddWithValue("@Address", Address);
            cmd.Parameters.AddWithValue("@Postcode", Postcode);

            int i = cmd.ExecuteNonQuery();
            con.Close();
            return i;
        }
        public int Updateprofile1(string UserId, string ContentfileName)
        {
            DataTable dt = new DataTable();
            con.Open();
            SqlCommand cmd = new SqlCommand("B_Updateprofile1_BizTbl_User_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserId", UserId);
            cmd.Parameters.AddWithValue("@filename", ContentfileName);
            int i = cmd.ExecuteNonQuery();
            con.Close();
            return i;
        }

        public int insertcredicards(string UserID, string CreditcardProvider, string CreditcardNumber, string NameOnCreditcard, string CardExpriryDate)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("B_InsertCreditCards_TB_CreditCardsDetails_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserId", UserID);
            cmd.Parameters.AddWithValue("@CreditCardId", CreditcardProvider);
            cmd.Parameters.AddWithValue("@Creditcardnumber", CreditcardNumber);
            cmd.Parameters.AddWithValue("@Cardholdename", NameOnCreditcard);
            cmd.Parameters.AddWithValue("@CardExpriryDate", CardExpriryDate);

            int i = cmd.ExecuteNonQuery();
            con.Close();
            return i;
        }

        public string Checkcredicards(string UserID, string CreditcardNumber)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("B_checkCreditCards_TB_CreditCardsDetails_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserId", UserID);
            cmd.Parameters.AddWithValue("@Creditcardnumber", CreditcardNumber);
            string i = Convert.ToString(cmd.ExecuteScalar());
            con.Close();
            return i;
        }


        public DataSet getusercredicards(string UserId, string Cultureid)
        {
            DataSet ds = new DataSet();
            con.Open();
            SqlCommand cmd = new SqlCommand("B_GetuserCreditCards_TB_CreditCardsDetails_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserId", UserId);
            cmd.Parameters.AddWithValue("@Culture", Cultureid);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            return ds;
        }

        public int Removecreditcard(string ID)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("B_RemovecarditCards_TB_CreditCardsDetails_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", ID);
            int i = cmd.ExecuteNonQuery();
            con.Close();
            return i;

        }
        public int Updatecreditcard(string ID, string CardExpriryDate)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("B_UpdatecarditCards_TB_CreditCardsDetails_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", ID);
            cmd.Parameters.AddWithValue("@CardExpriryDate", CardExpriryDate);
            int i = cmd.ExecuteNonQuery();
            con.Close();
            return i;

        }




        public int Changepassword(string UserId1, string encryptPassword)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("B_updatepassword_BizTbl_User_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserId", UserId1);
            cmd.Parameters.AddWithValue("@newpassword", encryptPassword);
            int i = cmd.ExecuteNonQuery();
            con.Close();
            return i;

        }

        #endregion

        #region Reviewtimline

        public string Reviewpending { get; set; }
        public string Review { get; set; }

        public DataTable reviewtimedetail(string UserId)
        {
            DataTable dt = new DataTable();
            con.Open();
            SqlCommand cmd = new SqlCommand("B_getreviewcount_ReservationReview_sp", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@id", UserId);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            return dt;
        }


        public int reviewtimerename(string UserId, string name)
        {
            DataTable dt = new DataTable();
            con.Open();
            SqlCommand cmd = new SqlCommand("B_Updateusername_BizTbl_User_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserId", UserId);
            cmd.Parameters.AddWithValue("@UserName", name);

            int i = cmd.ExecuteNonQuery();
            con.Close();
            return i;
        }


        public DataTable GetReservationReviews(string culture, string hotelId)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("B_GetReviews_TB_ReservationReviewDetail_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CultureCode", culture);
            cmd.Parameters.AddWithValue("@HotelID", hotelId);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();
            return dt;
        }

        public DataTable GetSummary(string CultureCode, string HotelID)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("[B_GetSummary_TB_ReservationReviewDetail_SP]", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CultureCode", CultureCode);
            cmd.Parameters.AddWithValue("@HotelID", HotelID);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();
            return dt;
        }


        #endregion

        public int Showreservation(string Reservationid, string pincode, string CultureCode)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("BT_Showreservation_Reservation_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CultureCode", CultureCode);
            cmd.Parameters.AddWithValue("@Reservationid", Reservationid);
            cmd.Parameters.AddWithValue("@pincode", pincode);
            int i = Convert.ToInt32(cmd.ExecuteScalar());
            con.Close();
            return i;
        }


        public DataSet UserShowReservation(string reservationId, string pincode, string culture)
        {
            DataSet ds = new DataSet();
            con.Open();
            //SqlCommand cmd = new SqlCommand("[dbo].[TB_SP_GetHotelReservations]", con);
            SqlCommand cmd = new SqlCommand("[dbo].[BT_GetHotelReservations_SP]", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Culture", culture);
            cmd.Parameters.AddWithValue("@ReservationID", reservationId);
            // cmd.Parameters.AddWithValue("@pincode", pincode);
            cmd.Parameters.AddWithValue("@OrderBy", "ID");
            cmd.Parameters.AddWithValue("@PagingSize", Int32.MaxValue);
            cmd.Parameters.AddWithValue("@PageIndex", 1);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            con.Close();
            return ds;

        }

        public int UpdatedReservationdetails(string reservationid, string guestname, string bedselectionno, string travelertype, string drptime, string RoomID)
        {
            con.Open();
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand("B_UpadateReservationDetails_HotelReservation_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BedOptionNo", bedselectionno);
            cmd.Parameters.AddWithValue("@RoomID", RoomID);
            cmd.Parameters.AddWithValue("@EstimatedArrivalTime", drptime);
            cmd.Parameters.AddWithValue("@TravellerTypeID", travelertype);
            cmd.Parameters.AddWithValue("@GuestFullName", guestname);
            cmd.Parameters.AddWithValue("@ReservationID", reservationid);
            int Status = cmd.ExecuteNonQuery();
            con.Close();
            return Status;


        }

        public int InsertSubscription(string username, string Emailaddress)
        {
            DataTable dt = new DataTable();
            con.Open();
            SqlCommand cmd = new SqlCommand("B_InsertSubscription_TB_Subscription_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserName", username);
            cmd.Parameters.AddWithValue("@Emailaddress", Emailaddress);
            int i = Convert.ToInt32(cmd.ExecuteNonQuery());
            con.Close();
            return i;
        }

        public DataSet Gethotles()
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("B_Gethotles_TB_Hotel_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            con.Close();
            return ds;
        }


        public int ReviewCount1()
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("B_ReviewCount_TB_ReservationReview_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            //cmd.Parameters.AddWithValue("@HotelID", HotelID);
            int i = Convert.ToInt32(cmd.ExecuteScalar());
            con.Close();
            return i;
        }

        public int GetHotelsreviewcount(string HotelID)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("B_GetHotelReviewCount_HotelReservation_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HotelID", HotelID);
            int i = Convert.ToInt32(cmd.ExecuteScalar());
            con.Close();
            return i;
        }


        public DataTable GetLatestNews(string Culture)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("B_GetLatestNews_TB_LatestNews_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Culture", Culture);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();
            return dt;
        }
        #region SocialLogin

        public string Checksocialidvalidation(string fbid, string googleid, string emailid)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("B_CheckSocialLogin_BizTbl_User_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@GoogleID", googleid);
            cmd.Parameters.AddWithValue("@FacebookID", fbid);
            cmd.Parameters.AddWithValue("@Emailid", emailid);
            string i = Convert.ToString(cmd.ExecuteScalar());
            con.Close();
            return i;
        }
        public string SocialLoginInsert(string fbid, string googleid, string emailid, string Name, string Gender, string Image, string myIP)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("B_InsertSocialLogin_BizTbl_User_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@GoogleID", googleid);
            cmd.Parameters.AddWithValue("@FacebookID", fbid);
            cmd.Parameters.AddWithValue("@Email", emailid);
            cmd.Parameters.AddWithValue("@SalutationTypeID", Gender);
            cmd.Parameters.AddWithValue("@ImageUrl", Image);
            cmd.Parameters.AddWithValue("@IPAddress", myIP);
            cmd.Parameters.AddWithValue("@Name", Name);
            string i = Convert.ToString(cmd.ExecuteScalar());
            con.Close();
            return i;
        }



        #endregion SocialLogin


        public int checkvarificationcode(string checkvarificationcode)
        {
            DataTable dt = new DataTable();
            con.Open();
            SqlCommand cmd = new SqlCommand("B_checkvarificationcode_BizTbl_User_SP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@checkvarificationcode", checkvarificationcode);
            int i = Convert.ToInt32(cmd.ExecuteScalar());
            con.Close();
            return i;
        }


        public DataSet getfullreservastionsdetails(string Reservationid, string Culture)
        {
            DataTable dt = new DataTable();
            con.Open();
            SqlCommand cmd = new SqlCommand("TB_SP_GetReservationDetail", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ReservationID", Reservationid);
            cmd.Parameters.AddWithValue("@Culture", Culture);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            con.Close();
            return ds;
        }










        public string IsPreferred { get; set; }

        public string PriceType { get; set; }

        public string RegionId { get; set; }

        public int DefaultCountryCode { get; set; }

        public double ConvertedRoomPriceHistry { get; set; }

        public string PromotionPercentage { get; set; }

        public string RoomPriceHistry { get; set; }

        public string ReservationAmount { get; set; }

        public string GeneralPromotionDiscountPercentage { get; set; }

        public string PromotionDiscountPercentage { get; set; }

        public string CreditCardUsed { get; set; }

        public string Note { get; set; }

        public string ComissionRate { get; set; }

        public string Deposit { get; set; }

        public string HotelAddress { get; set; }

        public string HotelCityName { get; set; }

        public string HotelPhone { get; set; }

        public string HotelEmail { get; set; }

        public string HotelPostCode { get; set; }

        public string RoomTypeName { get; set; }

        public string HotelAccommodationTypeName { get; set; }

        public string HotelReservationPayableAmount { get; set; }

        public string NonRefundable { get; set; }

        public string RefundableDayCount { get; set; }

        public string PenaltyRateTypeID { get; set; }

        public string PenaltyRateTypeName { get; set; }

        public string TravellerTypeName { get; set; }

        public string HotelReservationStatusName { get; set; }

        public string ReservationOperationName { get; set; }

        public string CancelDateTime { get; set; }

        public string OpUserID { get; set; }

        public string FullName { get; set; }

        public string FirmID { get; set; }

        public string HotelCancelTypeName { get; set; }

        public string HotelConditionText { get; set; }

        public string CreditTypeCode { get; set; }

        public string CVCLenth { get; set; }

        public int CreditCardNotRequired { get; set; }

        public string CancelPolicyWarning { get; set; }

        public string HotelCancelPolicy { get; set; }

        public string Reservation { get; set; }

        public string InstantConfirmation { get; set; }

        public string ReviewValue { get; set; }
        public List<Home> BedSelectionList { get; set; }

        public string Continent { get; set; }

        public string style { get; set; }

        public int Rowcounts { get; set; }

        public string CountryNameineng { get; set; }

        public string validthru { get; set; }

        public string CreatedDate { get; set; }

        public string MinimumRoomPrice { get; set; }

        public string from { get; set; }

        public string Booknow { get; set; }

        public string night { get; set; }

        public string Latestbooking { get; set; }

        public string addedon { get; set; }

        public string LastReservationDate { get; set; }

        public string CreditCardProviderID { get; set; }

        public string FAQ { get; set; }

        public string cardholder { get; set; }

        public string CityCount { get; set; }

        public string TripCount { get; set; }



        public string ReservationIdHead { get; set; }

        public string Propertyhead { get; set; }

        public string TotalRoomPricehead { get; set; }

        public string PinCodehead { get; set; }

        public string ReservationDatehead { get; set; }

        public string CheckInDatehead { get; set; }

        public string CheckOutDatehead { get; set; }

        public string PayableAmounthead { get; set; }

        public string Statushead { get; set; }

        public string beds { get; set; }

        public string StatusId { get; set; }

        public string RatingBasedOnReview { get; set; }

        public string WishListAdded { get; set; }

        public string Didtext { get; set; }

        public string Icon { get; set; }

        public string RoomSize { get; set; }

        public string Travel { get; set; }

        public string Createddate { get; set; }

        public string Title { get; set; }

        public string CheckInstauts { get; set; }

        public string btnreview { get; set; }

        public string btnupdate { get; set; }

        public List<bedselection> beddropdown { get; set; }
        public List<traveller> travellerdropdown { get; set; }


        public string HotelReservationID { get; set; }

        public string RefundableInfotext { get; set; }

        public string ReservationStatuss { get; set; }

        public string GuestNames { get; set; }

        public string PinCodes { get; set; }

        public string AccommodationTypes { get; set; }

        public string TotalRoomPrices { get; set; }

        public string PeopleCounts { get; set; }

        public string EstimatedArrivalTimes { get; set; }

        public string TravellerTypes { get; set; }

        public string BedPreferences { get; set; }

        public string CancelPolicy { get; set; }

        public string NightPrice { get; set; }

        public int cbxHotelReservation { get; set; }

        public int btnCancelSelected { get; set; }

        public int btnCancel { get; set; }

        public string PostImage { get; set; }

        public string Subject { get; set; }

        public string For1RoomText { get; set; }

        public string ReservationIDs { get; set; }

        public string Reserver { get; set; }

        public string ReservationDates { get; set; }

        public string HotelAddresss { get; set; }

        public string HotelContact { get; set; }

        public string CheckInDates { get; set; }

        public string CheckOutDates { get; set; }

        public string Propertys { get; set; }

        public string NightCounts { get; set; }

        public string Emails { get; set; }

        public string Citys { get; set; }

        public string Countrys { get; set; }

        public string Postcodes { get; set; }

        public string RoomCounts { get; set; }

        public string PayableAmounts { get; set; }

        public string HotelConditions { get; set; }

        public string GuestNote { get; set; }

        public string RoomCountRadioStyle { get; set; }


        public string ReviewScale { get; set; }

        public string ReviewID { get; set; }

        public int ReviewScaleID { get; set; }

        public string width { get; set; }

        public string ReviewScalePoint { get; set; }

        public string Excellantwidth { get; set; }

        public string Averagewidth { get; set; }

        public string Poorwidth { get; set; }

        public string Terriblewidth { get; set; }

        public string Goodwidth { get; set; }

        public double Elsewidth { get; set; }

        public string ReviewScaleType { get; set; }

        public string EncReservationID { get; set; }

        public string EncPinCode { get; set; }

        public int totalroomcount { get; set; }

        public List<RoomPrice> roomratesinglenight { get; set; }

        public string SalutationTypeName { get; set; }

        public int totalPeopleCounts { get; set; }

        public string ReviewScaleTypeID { get; set; }

        public string Excellantwidth1 { get; set; }

        public int gdfgd { get; set; }

        public long IDs { get; set; }

        public string AttributeTypeID { get; set; }


    }
}