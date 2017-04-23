using GBSHotels.API.Helper;
using GBSHotels.API.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GBSHotels.API.Controllers
{
    public class RegionController : BaseApiController
    {
        private Home homeService;
        public RegionController()
        {
            homeService = new Home();
        }
        [HttpGet]
        public HttpResponseMessage GetHotelRegions(string culture, string countryId)
        {
            List<HotelRegionModel> listHotelRegion = new List<HotelRegionModel>();
            DataSet ds = new DataSet();
            try
            {
                string TxtCode = "Properties";
                string CountryCount = "";


                ds = homeService.GetHotelRegions(culture, countryId, TxtCode);
                DataTable dt1 = new DataTable();
                DataTable dt2 = new DataTable();
                DataTable dt3 = new DataTable();

                if (ds != null)
                {
                    dt1 = ds.Tables[0];
                    dt2 = ds.Tables[1];
                    dt3 = ds.Tables[2];

                    string TxtMessage = "";
                    if (dt1 != null)
                    {
                        if (dt1.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt1.Rows)
                            {
                                TxtMessage = dr["textMessage"].ToString();
                            }
                        }
                    }
                    if (dt3 != null && dt3.Rows.Count > 0)
                    {

                        foreach (DataRow dr in dt3.Rows)
                        {
                            // Home objHome = new Home();
                            CountryCount = dr["CountryCount"].ToString();
                        }
                    }
                    if (dt2 != null && dt2.Rows.Count > 0)
                    {

                        foreach (DataRow dr in dt2.Rows)
                        {
                            HotelRegionModel hotelRegion = new HotelRegionModel();
                            hotelRegion.Count = dr["Count"].ToString();
                            hotelRegion.RegionName = dr["Region"].ToString();
                            hotelRegion.CountryName = dr["CountryName"].ToString();
                            hotelRegion.Code = dr["Code"].ToString();
                            hotelRegion.RegionID = dr["ID"].ToString();
//                            hotelRegion.topphoto = "/Images/Region/" + dr["ID"].ToString() + ".jpg";
                            hotelRegion.topphoto = ConfigurationManager.AppSettings[ConstAppSettings.URL_RegionImages].ToString()+ "/Images/Region/" + dr["ID"].ToString() + ".jpg";

                            hotelRegion.topphotoflag = "/Images/" + dr["Code"].ToString() + ".png";
                            hotelRegion.textmessage = TxtMessage;

                            hotelRegion.CountryCount = CountryCount;
                            string countryineng = dr["countryineng"].ToString();
                            countryineng = countryineng.Trim();
                            //hotelRegion.NavigateURL = "/Hotels_en/" + countryineng + "/" + dr["Nameineng"].ToString();
                            hotelRegion.NavigateURL = "/#!/hotelRegion?cityId=" + hotelRegion.RegionID.ToString() + "&city=" + dr["Nameineng"].ToString(); 
                            listHotelRegion.Add(hotelRegion);
                        }

                    }

                }
                return Request.CreateResponse(HttpStatusCode.OK, listHotelRegion);
            }
            catch (Exception ex)
            {
                return LogError(ex);
            }

        }

        [HttpGet]
        public HttpResponseMessage GetCountryRegions(string culture, string countryId)
        {

            DataTable dataTableHHotel = new DataTable();
            List<HotelModel> listHotel = new List<HotelModel>();

            try
            {
                DataSet ds = homeService.GetCountryRegions("ID", culture, Int32.MaxValue, 1, null, countryId);
                if (ds != null)
                {
                    dataTableHHotel = ds.Tables[1];
                }

                if (dataTableHHotel != null)
                {
                    if (dataTableHHotel.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dataTableHHotel.Rows)
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

                            listHotel.Add(obj);
                        }

                    }
                }
                return Request.CreateResponse(HttpStatusCode.OK, listHotel);
            }
            catch (Exception ex)
            {
                return LogError(ex);
            }
        }
        
        [HttpGet]
        public HttpResponseMessage GetRoutingDetails(string HotelId, string DestinationName, string GuestCountdetails)
        {
           
            DataTable dataTableRounting = new DataTable();
            List<CityRoutingModel> listRouting = new List<CityRoutingModel>();
            try
            {
                dataTableRounting = homeService.objGetRoutingInfo(HotelId);

                if (dataTableRounting.Rows.Count > 0)
                {
                    CityRoutingModel routing = new CityRoutingModel();
                    routing.RoutingName = dataTableRounting.Rows[0]["RoutingName"].ToString();
                    routing.Code = dataTableRounting.Rows[0]["Code"].ToString();
                    listRouting.Add(routing);
                }

                return Request.CreateResponse(HttpStatusCode.OK, listRouting);
            }
            catch (Exception ex)
            {
                return LogError(ex);
            }
        }
    }
}
