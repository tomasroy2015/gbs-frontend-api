using Business;
using GBSHotels.API.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace GBSHotels.API.Controllers
{
    //[EnableCors(origins: "http://localhost:49376", headers: "*", methods: "*")]
    public class PropertyController : BaseApiController
    {
        private Home homeService;
        public PropertyController()
        {
            homeService = new Home();
        }

        public HttpResponseMessage GetCountry(string culture)
        {
            List<CountryModel> listCountry = new List<CountryModel>();
            DataTable dataTableCountry = new DataTable();
            try
            {
                dataTableCountry = homeService.GetCountry(culture);
                if (dataTableCountry != null && dataTableCountry.Rows.Count > 0)
                {
                    foreach (DataRow dr in dataTableCountry.Rows)
                    {
                        CountryModel country = new CountryModel();
                        country.Id = Convert.ToInt32(dr["ID"]);
                        country.CountryName = Convert.ToString(dr["CountryName"]);
                        listCountry.Add(country);
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, listCountry);
                }
                else
                    return Request.CreateResponse(HttpStatusCode.NotFound, "No Record(s) found.");
            }
            catch (Exception ex)
            {
                return LogError(ex);
            }
        }

        public HttpResponseMessage GetCity(string countryId, string culture)
        {

            List<CityModel> listCity = new List<CityModel>();
            DataTable dataTableCity = new DataTable();
            try
            {
                dataTableCity = homeService.GetCityProperty(countryId, culture);
                if (dataTableCity != null && dataTableCity.Rows.Count > 0)
                {

                    foreach (DataRow dr in dataTableCity.Rows)
                    {
                        CityModel obj = new CityModel();
                        obj.Id = Convert.ToInt32(dr["ID"]);
                        obj.CityName = Convert.ToString(dr["CityName"]);
                        listCity.Add(obj);
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, listCity);
                }
                else
                    return Request.CreateResponse(HttpStatusCode.NotFound, "No Record(s) found.");
            }
            catch (Exception ex)
            {
                return LogError(ex);
            }
        }

        public HttpResponseMessage GetPropertyType(string culture)
        {
            List<PropertTypeModel> listPropertyType = new List<PropertTypeModel>();
            DataTable dataTablePropertyType = new DataTable();
            try
            {
                dataTablePropertyType = homeService.GetHotelType(culture);
                if (dataTablePropertyType != null && dataTablePropertyType.Rows.Count > 0)
                {
                    foreach (DataRow dr in dataTablePropertyType.Rows)
                    {
                        PropertTypeModel propertType = new PropertTypeModel();
                        propertType.Id = Convert.ToInt32(dr["ID"]);
                        propertType.TypeName = Convert.ToString(dr["HotelType"]);
                        listPropertyType.Add(propertType);
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, listPropertyType);
                }
                else
                    return Request.CreateResponse(HttpStatusCode.NotFound, "No Record(s) found.");
            }
            catch (Exception ex)
            {
                return LogError(ex);
            }
        }

        public HttpResponseMessage GetPropertyClass(string culture)
        {
            List<PropertyClassModel> listPropertyClass = new List<PropertyClassModel>();
            DataTable dataTableHotelClass = new DataTable();
            try
            {
                dataTableHotelClass = homeService.GetHotelClass(culture);
                if (dataTableHotelClass != null && dataTableHotelClass.Rows.Count > 0)
                {
                    foreach (DataRow dr in dataTableHotelClass.Rows)
                    {
                        PropertyClassModel propertyClass = new PropertyClassModel();
                        propertyClass.Id = Convert.ToInt32(dr["ID"]);
                        propertyClass.Code = Convert.ToString(dr["Code"]);
                        propertyClass.Name = Convert.ToString(dr["Name"]);
                        propertyClass.Sort = Convert.ToString(dr["Sort"]);
                        listPropertyClass.Add(propertyClass);
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, listPropertyClass);
                }
                else
                    return Request.CreateResponse(HttpStatusCode.NotFound, "No Record(s) found.");
            }
            catch (Exception ex)
            {
                return LogError(ex);
            }
        }

        public HttpResponseMessage GetPropertyAccommodation(string culture)
        {
            List<PropertyAccomodationModel> listAccommodation = new List<PropertyAccomodationModel>();
            DataTable dataTablePropertyAccomodation = new DataTable();
            try
            {
                dataTablePropertyAccomodation = homeService.GetHotelAccommodation(culture);
                if (dataTablePropertyAccomodation != null && dataTablePropertyAccomodation.Rows.Count > 0)
                {
                    foreach (DataRow dr in dataTablePropertyAccomodation.Rows)
                    {
                        PropertyAccomodationModel propertyAccomodation = new PropertyAccomodationModel();
                        propertyAccomodation.Id = Convert.ToInt32(dr["ID"]);
                        propertyAccomodation.TypeName = Convert.ToString(dr["HotelAccommodation"]);
                        listAccommodation.Add(propertyAccomodation);
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, listAccommodation);
                }
                else
                    return Request.CreateResponse(HttpStatusCode.NotFound, "No record(s) found.");

            }
            catch (Exception ex)
            {
                return LogError(ex);
            }
        }

        public HttpResponseMessage GetPropertyReservationCurrency(string culture)
        {
            List<CurrencyModel> listPropertyReservationCurrency = new List<CurrencyModel>();
            DataTable dataTablePropertyReservationCurrency = new DataTable();
            try
            {
                dataTablePropertyReservationCurrency = homeService.GetCurrencyValues(culture);
                if (dataTablePropertyReservationCurrency != null && dataTablePropertyReservationCurrency.Rows.Count > 0)
                {
                    foreach (DataRow dr in dataTablePropertyReservationCurrency.Rows)
                    {
                        CurrencyModel currency = new CurrencyModel();
                        currency.Id = Convert.ToInt32(dr["ID"]);
                        currency.CurrencyCode = Convert.ToString(dr["Code"]);
                        currency.CurrencyName = Convert.ToString(dr["CurrencyName"]);
                        listPropertyReservationCurrency.Add(currency);
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, listPropertyReservationCurrency);
                }
                else
                    return Request.CreateResponse(HttpStatusCode.NotFound, "No record(s) found.");
            }
            catch (Exception ex)
            {
                return LogError(ex);
            }
        }

        public HttpResponseMessage GetCreditCards()
        {
            List<CreditCardModel> listCreditCard = new List<CreditCardModel>();
            DataTable dataTableCreditCard = new DataTable();
            try
            {
                dataTableCreditCard = homeService.GetCreditCardDetails();
                if (dataTableCreditCard != null && dataTableCreditCard.Rows.Count > 0)
                {
                    foreach (DataRow dr in dataTableCreditCard.Rows)
                    {
                        CreditCardModel creditCard = new CreditCardModel();
                        creditCard.Name = Convert.ToString(dr["Name"]);
                        creditCard.Id = Convert.ToInt32(dr["ID"]);
                        listCreditCard.Add(creditCard);
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, listCreditCard);
                }
                else
                    return Request.CreateResponse(HttpStatusCode.NotFound, "No Record(s) found.");
            }
            catch (Exception ex)
            {
                return LogError(ex);
            }
        }

        public HttpResponseMessage GetPropertyFacilities(string culture, string attributeHeaderId)
        {
            List<PropertyFacilitiesModel> listPropertyFacilities = new List<PropertyFacilitiesModel>();
            DataTable dataTablePropertyFacilities = new DataTable();
            try
            {
                dataTablePropertyFacilities = homeService.GetPropertyFacilities(culture, attributeHeaderId);
                string addpropetylabels = homeService.GetTextMessagesAsString(culture, "addpropetylabels");
                if (dataTablePropertyFacilities != null && dataTablePropertyFacilities.Rows.Count > 0)
                {
                    foreach (DataRow dr in dataTablePropertyFacilities.Rows)
                    {
                        PropertyFacilitiesModel propertyFacility = new PropertyFacilitiesModel();
                        propertyFacility.FacilityName = Convert.ToString(dr["FacilityName"]);
                        propertyFacility.Id = Convert.ToInt32(dr["ID"]);
                        propertyFacility.Charge = Convert.ToString(dr["Chargeable"]);
                        propertyFacility.PropertyLabelDesign = addpropetylabels;
                        listPropertyFacilities.Add(propertyFacility);
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, listPropertyFacilities);
                }
                else
                    return Request.CreateResponse(HttpStatusCode.NotFound, "No record(s) found.");
            }
            catch (Exception ex)
            {
                return LogError(ex);
            }
        }

        public HttpResponseMessage GetChannelManager()
        {
            List<ChannelManagerModel> listChannelManager = new List<ChannelManagerModel>();
            DataTable dataTableChannelManager = new DataTable();
            try
            {
                dataTableChannelManager = homeService.GetChannelManager();
                if (dataTableChannelManager != null && dataTableChannelManager.Rows.Count > 0)
                {
                    foreach (DataRow dr in dataTableChannelManager.Rows)
                    {
                        ChannelManagerModel channelManager = new ChannelManagerModel();
                        channelManager.Id = Convert.ToInt32(dr["ID"]);
                        channelManager.ManagerName = Convert.ToString(dr["Name"]);
                        listChannelManager.Add(channelManager);
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, listChannelManager);
                }
                else
                    return Request.CreateResponse(HttpStatusCode.NotFound, "No record(s) found.");
            }
            catch (Exception ex)
            {
                return LogError(ex);
            }
        }

        public HttpResponseMessage GetRoomType(string culture)
        {
            List<RoomTypeModel> listRoomType = new List<RoomTypeModel>();
            DataTable dataTabeRoomType = new DataTable();
            try
            {
                dataTabeRoomType = homeService.GetRoomTypeDetails(culture);
                if (dataTabeRoomType != null && dataTabeRoomType.Rows.Count > 0)
                {
                    foreach (DataRow dr in dataTabeRoomType.Rows)
                    {
                        RoomTypeModel roomType = new RoomTypeModel();
                        roomType.Id = Convert.ToInt32(dr["ID"]);
                        roomType.TypeName = Convert.ToString(dr["RoomType"]);
                        listRoomType.Add(roomType);
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, listRoomType);
                }
                else
                    return Request.CreateResponse(HttpStatusCode.NotFound, "No record(s) found.");
            }
            catch (Exception ex)
            {
                return LogError(ex);
            }
        }

        public HttpResponseMessage GetSmokingType(string culture)
        {

            List<SmokingTypeModel> listSmokingType = new List<SmokingTypeModel>();
            DataTable dataTableSmokingType = new DataTable();
            try
            {
                dataTableSmokingType = homeService.GetSmokingTypeDetails(culture);
                if (dataTableSmokingType != null && dataTableSmokingType.Rows.Count > 0)
                {
                    foreach (DataRow dr in dataTableSmokingType.Rows)
                    {
                        SmokingTypeModel smokingType = new SmokingTypeModel();
                        smokingType.Id = Convert.ToInt32(dr["ID"]);
                        smokingType.TypeName = Convert.ToString(dr["SmokingType"]);
                        listSmokingType.Add(smokingType);
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, listSmokingType);
                }
                else
                    return Request.CreateResponse(HttpStatusCode.NotFound, "No result(s) found.");
            }
            catch (Exception ex)
            {
                return LogError(ex);
            }
        }

        public HttpResponseMessage GetRoomViewType(string culture)
        {

            List<RoomViewTypeModel> listRoomViewType = new List<RoomViewTypeModel>();
            DataTable dataTableRoomViewType = new DataTable();
            try
            {
                dataTableRoomViewType = homeService.GetRoomViewDetails(culture);
                if (dataTableRoomViewType != null && dataTableRoomViewType.Rows.Count > 0)
                {
                    foreach (DataRow dr in dataTableRoomViewType.Rows)
                    {
                        RoomViewTypeModel roomViewType = new RoomViewTypeModel();
                        roomViewType.Id = Convert.ToInt32(dr["ID"]);
                        roomViewType.TypeName = Convert.ToString(dr["ViewType"]);
                        listRoomViewType.Add(roomViewType);
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, listRoomViewType);
                }
                else
                    return Request.CreateResponse(HttpStatusCode.NotFound, "No record(s) found.");
            }
            catch (Exception ex)
            {
                return LogError(ex);
            }
        }

        public HttpResponseMessage GetHotelRoomAttribute(string culture)
        {
            List<RoomAttributeModel> listRoomAttribute = new List<RoomAttributeModel>();
            try
            {
                DataTable dataTableAttributeHeader = new DataTable();
                DataTable dataTableAttributes = new DataTable();
                dataTableAttributeHeader = homeService.GetAttributeHeaders(culture);

                if (dataTableAttributeHeader != null && dataTableAttributeHeader.Rows.Count > 0)
                {
                    foreach (DataRow dr in dataTableAttributeHeader.Rows)
                    {
                        dataTableAttributes = homeService.GetAttributes(Convert.ToString(dr["ID"]), culture);
                        if (dataTableAttributes != null && dataTableAttributes.Rows.Count > 0)
                        {
                            foreach (DataRow Attribute in dataTableAttributes.Rows)
                            {
                                RoomAttributeModel roomAttribute = new RoomAttributeModel();
                                roomAttribute.Id = Convert.ToInt32(Attribute["ID"]);
                                roomAttribute.AttributeName = Convert.ToString(Attribute["Name"]);
                                listRoomAttribute.Add(roomAttribute);
                            }
                        }
                    }

                    return Request.CreateResponse(HttpStatusCode.OK, listRoomAttribute);
                }
                else
                    return Request.CreateResponse(HttpStatusCode.NotFound, "No record(s) found.");
            }
            catch (Exception ex)
            {
                return LogError(ex);
            }
        }

        public HttpResponseMessage GetSettingPenaltyRate(string culture)
        {

            List<PenaltyRateModel> listpaneltyRate = new List<PenaltyRateModel>();
            DataTable dataTablePaneltyRate = new DataTable();
            try
            {
                dataTablePaneltyRate = homeService.GetSPenaltyRate(culture);
                if (dataTablePaneltyRate != null && dataTablePaneltyRate.Rows.Count > 0)
                {
                    foreach (DataRow dr in dataTablePaneltyRate.Rows)
                    {
                        PenaltyRateModel obj = new PenaltyRateModel();
                        obj.PaneltyRate = Convert.ToString(dr["PenaltyRate"]);
                        obj.Id = Convert.ToInt32(dr["ID"]);
                        listpaneltyRate.Add(obj);
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, listpaneltyRate);
                }
                else
                    return Request.CreateResponse(HttpStatusCode.NotFound, "No record(s) found.");
            }
            catch (Exception ex)
            {
                return LogError(ex);
            }
        }

        [HttpPost]
        public HttpResponseMessage SavePropertyInfo(PropertyModel property)
        {

            int i = 0;
            Home ObjModel = new Home();
            try
            {
                string hostName = Dns.GetHostName();
                
                string ProHotelChain = "";
                string ProHotelDescription = "";
                string ProHotelPostCode = "";
                bool cbxHotelIsSecret = false;

                string HotelRoomID = "";
                using (BaseRepository baseRepo = new BaseRepository())
                {

                    string userID = null;
                    BizTbl_User user = BizUser.GetUser(baseRepo.BizDB, string.Empty, string.Empty, string.Empty,property.ContactPersonEmail, null, null, true);
                    if (user == null)
                    {
                        userID = Convert.ToString(BizUser.SaveUser(baseRepo.BizDB, string.Empty,property.Salutation,property.ContactPersonName, property.ContactPersonSurname,property.ProCountry, property.ProCity,property.ProCity,property.CityName, null,
                      property.ContactPersonPhone, property.ContactPersonEmail, null, string.Empty, string.Empty, string.Empty, BizCommon.Status.WaitingForApproval, null, string.Empty, false,
                        false, DateTime.Now.Date, Convert.ToInt64(BizCommon.DefaultUserID), property.IpAddress));
                    }
                    else
                        userID = Convert.ToString(user.ID);
                    string firmID = null;
                    string firm = null;
                    firm = ObjModel.GetFirmID(property.FirmName, property.ProCity);
                    if (firm == null || firm == "")
                    {
                        firmID = Convert.ToString(BizFirm.SaveFirm(baseRepo.BizDB, string.Empty,property.FirmName,property.ProCountry, property.ProCity,property.ProCity, null,property.ProPhone,property.ProFax, null,
                        property.ProEmail, null, null,property.Salutation, property.ContactPersonName,property.ContactPersonSurname,property.ContactPersonPosition,property.ContactPersonPhone, property.ContactPersonEmail, BizCommon.Status.WaitingForApproval,
                        false, property.IpAddress, DateTime.Now.Date, Convert.ToInt64(userID)));
                    }
                    else
                    {
                        firmID = Convert.ToString(firm);
                    }


                    if (user == null || (firm == null || firm == ""))
                    {
                        BizUser.SaveUserFirm(baseRepo.BizDB, userID, firmID);
                    }

                    string hotelID = Convert.ToString(BizHotel.SaveHotel(baseRepo.BizDB, string.Empty, firmID,property.ProType, property.ProClass, ProHotelChain,property.ProAccommodation, property.ProCountry,property.ProCity,property.ProCity,
                                    string.Empty,property.HotelName, ProHotelDescription,property.ProAddress,property.ProPhone,property.ProFax, ProHotelPostCode,property.RoomCount, string.Empty, string.Empty,
                                    string.Empty, string.Empty,property.ProWebsite,property.ProEmail, string.Empty, string.Empty, string.Empty, property.ProResCurrency, string.Empty, string.Empty,
                                    string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, BizCommon.Status.WaitingForApproval, cbxHotelIsSecret, false, true,property.ChannelManager,
                                    false, BizCommon.DefaultCultureID, string.Empty, property.IpAddress, DateTime.Now.Date, Convert.ToInt64(userID), property.CultureID));

                    DataTable Comission = ObjModel.GetParameter("HotelMinumumComissionRate", "en");
                    int HotelMinumumComissionRate = 10;
                    if (Comission != null)
                    {
                        if (Comission.Rows.Count > 0)
                        {
                            HotelMinumumComissionRate = Convert.ToInt32(Comission.Rows[0]["Value"]);
                        }
                    }

                    string MinDate = "1901-01-01 00:00:00.000";
                    string MaxDate = "2100-12-31 00:00:00.000";
                    BizHotel.SaveHotelComissions(baseRepo.BizDB, hotelID, HotelMinumumComissionRate, MinDate, MaxDate, userID, true);

                    if (property.RadioRefund != "")
                    {
                        if(property.RadioRefund =="local")
                        {
                            property.RadioRefund ="1";
                            property.RefundCancel = null;
                            property.PenaltyRate = null;
                        }
                        if (property.RadioRefund == "refundable-open")
                        {
                            property.RadioRefund = "2";
                        }
                        BizHotel.SaveHotelCancelPolicy(baseRepo.BizDB, hotelID, property.RadioRefund,property.RefundCancel,property.PenaltyRate, BizCommon.DefaultUserID);
                    }


                    String[] AttributeId =property.HotelAttributeId.Split(',');
                    String[] Charge =property.ChargeType.Split(',');

                    for (int j = 0; j < AttributeId.Length; j++)
                    {
                        int Check = ObjModel.InsertSettingHAttributes(AttributeId[j], Charge[j], hotelID);
                    }

                    BizHotel.DeleteHotelCreditCards(baseRepo.BizDB, hotelID);

                    String[] CreditCardId = property.listcards.Split(',');

                    if (property.listcards != "")
                    {
                        for (int k = 0; k < CreditCardId.Length; k++)
                        {
                            BizHotel.SaveHotelCreditCards(baseRepo.BizDB, hotelID, Convert.ToString(CreditCardId[k]), BizCommon.DefaultUserID);
                        }
                    }


                    HotelRoomID = Convert.ToString(BizHotel.SaveHotelRoom(baseRepo.BizDB, string.Empty, hotelID, property.ProType,property.RoomCount,property.RoomSpace,
                                property.RoomMaxPerson, property.RoomMaxChildren, property.RoomBabyCots,property.RoomExBabyCots, property.RoomView,property.RoomSmoking, string.Empty,
                                property.CultureID, true, DateTime.Now.Date, Convert.ToInt64(BizCommon.DefaultUserID)));

                    string[] FacilityId = property.Facilities.Split(',');
                    Home ObjHotelRoomAttributes = new Home();
                    if (property.Facilities != "")
                    {
                        for (int k = 0; k < FacilityId.Length; k++)
                        {
                            int result = ObjHotelRoomAttributes.InsertRoomFacilities(HotelRoomID, FacilityId[k]);
                        }
                    }
                    BizHotel.CreateHotelRoomAvailability(baseRepo.BizDB, HotelRoomID, Convert.ToString(DateTime.Now.Date), Convert.ToString(new DateTime(DateTime.Now.Year + 1, 12, 31)),property.RoomCount);
                }
                return Request.CreateResponse(HttpStatusCode.OK, true);
            }
            catch (Exception ex)
            {
                return LogError(ex);
            }
        }
    }
}
