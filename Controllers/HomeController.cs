using Business;
using GBSHotels.API.Helper;
using GBSHotels.API.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web.Http;
using System.Web.Http.Cors;


namespace GBSHotels.API.Controllers
{
    //[EnableCors(origins: "http://localhost:49376,gdsbooking.com" , headers: "*", methods: "*")]
    public class HomeController : BaseApiController
    {
        private Home homeService;

        public string MailServer { get; set; }
        public string MailUsername { get; set; }
        public int MailPort { get; set; }
        public string MailPassword { get; set; }
        public HomeController()
        {
            this.MailServer = System.Configuration.ConfigurationManager.AppSettings["SMTPServerName"];
            this.MailUsername = System.Configuration.ConfigurationManager.AppSettings["SMTPUsername"];
            if (!string.IsNullOrEmpty(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["SMTPPort"])))
                this.MailPort = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SMTPPort"].ToString());
            else
                this.MailPort = 25;
            this.MailPassword = System.Configuration.ConfigurationManager.AppSettings["SMTPPassword"];
            homeService = new Home();
        }

        public HttpResponseMessage GetLatestNews(string culture)
        {
            List<NewsModel> objList = new List<NewsModel>();
            DataTable dataTable = new DataTable();
            try
            {
                dataTable = homeService.GetLatestNews(culture);


                if (dataTable != null)
                {
                    if (dataTable.Rows.Count > 0)
                    {
                        foreach (DataRow dataRow in dataTable.Rows)
                        {
                            NewsModel news = new NewsModel();
                            news.Id = Convert.ToInt32(dataRow["ID"]);
                            news.UserId = Convert.ToInt32(dataRow["UserID"]);
                            news.Name = Convert.ToString(dataRow["Name"]);
                            news.Travel = Convert.ToString(dataRow["Travel"]);
                            news.CreatedDate = Convert.ToString(dataRow["Createddate"]);
                            news.Title = Convert.ToString(dataRow["Title"]);
                            news.Description = Convert.ToString(dataRow["Description"]);
                            if (!string.IsNullOrEmpty(Convert.ToString(dataRow["PostImage"])))
                                news.Image = URL.EXTRANET_URLFULL + "Upload/LatestNews/" + dataRow["PostImage"].ToString();
                            else
                                news.Image = "/Images/image_not_found.jpg";
                            objList.Add(news);
                        }
                    }
                }

                return Request.CreateResponse(HttpStatusCode.Accepted, objList);
            }
            catch (Exception ex)
            {
                return LogError(ex);
            }


        }
        public HttpResponseMessage GetCulture()
        {
            List<CultureModel> listCulture = new List<CultureModel>();
            DataTable dt = new DataTable();
            try
            {
                dt = homeService.GetCulturecode();
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            CultureModel cultureObj = new CultureModel();
                            cultureObj.Description = Convert.ToString(dr["Description"]);
                            cultureObj.CultureCode = Convert.ToString(dr["Code"]);
                            cultureObj.SystemCode = Convert.ToString(dr["SystemCode"]);
                            listCulture.Add(cultureObj);
                        }
                    }
                }
                return Request.CreateResponse(HttpStatusCode.Accepted, listCulture);
            }
            catch (Exception ex)
            {
                return LogError(ex);
            }
        }

        [HttpPost]
        public HttpResponseMessage PostSubscription(SubscriptionModel subscription)
        {
            try
            {
                if (homeService.InsertSubscription(subscription.username, subscription.email) > 0)
                    return Request.CreateResponse(HttpStatusCode.OK);
                else
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "Error occured Please try again");
            }
            catch (Exception ex)
            {
                return LogError(ex);
            }

        }


        public HttpResponseMessage ShowReservation(ReservationModel reservation)
        {
            Encryption64 objEncryptreservation = new Encryption64();
            if (reservation.ReservationID.IsBase64String())
            {
                string DecryptedReservationID = objEncryptreservation.Decrypt(ConvertHexToString(System.Web.HttpContext.Current.Server.UrlDecode(reservation.ReservationID)), ConstEncrypt.Key);
                string DecryptedPinCode = objEncryptreservation.Decrypt(ConvertHexToString(System.Web.HttpContext.Current.Server.UrlDecode(reservation.PinCode)), ConstEncrypt.Key);
                reservation.ReservationID = DecryptedReservationID;
                reservation.PinCode = DecryptedPinCode;
            }
            string EncryptedReservationid = string.Empty;
            string EncryptedPincode = string.Empty;
            int reservationcount = 0;
            try
            {
                reservationcount = homeService.Showreservation(reservation.ReservationID, reservation.PinCode, reservation.Culture);
                if (reservationcount == 1)
                {
                    EncryptedReservationid = ToHexString(reservation.ReservationID);
                    EncryptedPincode = ToHexString(reservation.PinCode);
                    ReservationModel reservationobj = new ReservationModel();
                    reservationobj.ReservationID = EncryptedReservationid;
                    reservationobj.PinCode = EncryptedPincode;
                    reservationobj.Culture = reservation.Culture;
                    return Request.CreateResponse(HttpStatusCode.OK, reservationobj);
                }
                else
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "No Found");


            }
            catch (Exception ex)
            {
                return LogError(ex);
            }

        }
        public HttpResponseMessage UserShowReservation(ReservationModel reservation)
        {
            List<Home> list = new List<Home>();

            string HotelConditionText = "";
            Encryption64 objEncryptreservation = new Encryption64();
            int totalPeopleCounts = 0;
            DateTime todaydate = DateTime.Now;
            DataSet ds = new DataSet();
            string culture = reservation.Culture;
            string ReservationStatus = homeService.GetTextMessagesAsString(culture, "ReservationStatus");
            string GuestName = homeService.GetTextMessagesAsString(culture, "GuestName");
            string PinCode = homeService.GetTextMessagesAsString(culture, "PinCode");
            string AccommodationType = homeService.GetTextMessagesAsString(culture, "AccommodationType");
            string TotalRoomPrice = homeService.GetTextMessagesAsString(culture, "TotalRoomPrice");
            string PeopleCount = homeService.GetTextMessagesAsString(culture, "PeopleCount");
            string EstimatedArrivalTime = homeService.GetTextMessagesAsString(culture, "EstimatedArrivalTime");
            string TravellerType = homeService.GetTextMessagesAsString(culture, "TravellerType");
            string CancelPolicy = homeService.GetTextMessagesAsString(culture, "CancelPolicy");
            string BedPreference = homeService.GetTextMessagesAsString(culture, "BedPreference");
            string NightPrice = homeService.GetTextMessagesAsString(culture, "NightPrice");
            string RefundableInfo = homeService.GetTextMessagesAsString(culture, "RefundableInfo");
            string reservationId = FromHexString(reservation.ReservationID);
            string ReservationPinCode = FromHexString(reservation.PinCode);
            ds = homeService.UserShowReservation(reservationId, ReservationPinCode, culture);
            try
            {
                DataTable dt1 = new DataTable();
                DataTable dt2 = new DataTable();
                DataTable dt3 = new DataTable();
                DataTable dt = new DataTable();
                DataTable roomRate = new DataTable();
                DataTable traveller = new DataTable();
                Home objsdf = new Home();
                if (ds != null)
                {

                    dt1 = ds.Tables[0];
                    dt2 = ds.Tables[1];
                    dt3 = ds.Tables[2];
                    {
                        if (dt3 != null)
                        {
                            foreach (DataRow dr in dt3.Rows)
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
                    if (dt2 != null)
                    {
                        if (dt2.Rows.Count > 0)
                        {
                            int roomcount = dt2.Rows.Count;


                            foreach (DataRow dr2 in dt2.Rows)
                            {
                                totalPeopleCounts = totalPeopleCounts + (Convert.ToInt32(dr2["PeopleCount"]));
                            }

                            foreach (DataRow dr in dt2.Rows)
                            {
                                Home obj = new Home();
                                obj.ReservationStatuss = ReservationStatus;
                                obj.GuestNames = GuestName;
                                obj.PinCodes = PinCode;
                                obj.AccommodationTypes = AccommodationType;
                                obj.TotalRoomPrices = TotalRoomPrice;
                                obj.PeopleCounts = PeopleCount;
                                obj.EstimatedArrivalTimes = EstimatedArrivalTime;
                                obj.TravellerTypes = TravellerType;
                                obj.CancelPolicy = CancelPolicy;
                                obj.BedPreferences = BedPreference;
                                obj.NightPrice = NightPrice;
                                obj.ReservationIDs = homeService.GetTextMessagesAsString(culture, "ReservationID");
                                obj.Reserver = homeService.GetTextMessagesAsString(culture, "Reserver");
                                obj.ReservationDates = homeService.GetTextMessagesAsString(culture, "ReservationDate");
                                obj.Propertys = homeService.GetTextMessagesAsString(culture, "Property");
                                obj.HotelAddresss = homeService.GetTextMessagesAsString(culture, "HotelAddress");
                                obj.HotelContact = homeService.GetTextMessagesAsString(culture, "HotelContact");
                                obj.CheckInDates = homeService.GetTextMessagesAsString(culture, "CheckInDate");
                                obj.CheckOutDates = homeService.GetTextMessagesAsString(culture, "CheckOutDate");
                                obj.NightCounts = homeService.GetTextMessagesAsString(culture, "NightCount");
                                obj.Emails = homeService.GetTextMessagesAsString(culture, "Email");
                                obj.Countrys = homeService.GetTextMessagesAsString(culture, "Country");

                                obj.Citys = homeService.GetTextMessagesAsString(culture, "City");
                                obj.Postcodes = homeService.GetTextMessagesAsString(culture, "Postcode");
                                obj.RoomCounts = homeService.GetTextMessagesAsString(culture, "RoomCount");
                                obj.PayableAmounts = homeService.GetTextMessagesAsString(culture, "PayableAmount");
                                obj.HotelConditions = homeService.GetTextMessagesAsString(culture, "HotelConditions");
                                obj.GuestNote = homeService.GetTextMessagesAsString(culture, "GuestNote");


                                obj.ID = dr["ReservationID"].ToString();
                                obj.PinCode = dr["PinCode"].ToString();
                                obj.FirmID = dr["FirmID"].ToString();
                                obj.UserID = dr["UserID"].ToString();


                                DateTime ReservationDate = Convert.ToDateTime(dr["ReservationDate"]);
                                obj.ReservationDate = ReservationDate.ToString("dd MMMM yyyy HH:mm");
                                obj.Status = dr["StatusName"].ToString();
                                obj.Country = dr["CountryName"].ToString();
                                obj.City = dr["City"].ToString();
                                obj.FullName = dr["FullName"].ToString();
                                obj.SalutationTypeName = dr["SalutationTypeName"].ToString();

                                obj.Email = dr["Email"].ToString();

                                obj.PostCode = dr["PostCode"].ToString();
                                obj.Address = dr["Address"].ToString();
                                obj.Phone = dr["Phone"].ToString();
                                obj.PayableAmount = Convert.ToString(dr["PayableAmount"].ToString());
                                obj.ReservationAmount = dr["ReservationAmount"].ToString();
                                obj.GeneralPromotionDiscountPercentage = dr["GeneralPromotionDiscountPercentage"].ToString();
                                obj.PromotionDiscountPercentage = dr["PromotionDiscountPercentage"].ToString();
                                obj.CreditCardUsed = dr["CreditCardUsed"].ToString();
                                obj.CurrencyName = dr["CurrencyName"].ToString();
                                obj.Note = dr["Note"].ToString();
                                obj.ComissionRate = dr["ComissionRate"].ToString();
                                obj.Deposit = dr["Deposit"].ToString();
                                obj.HotelName = dr["HotelName"].ToString();
                                obj.HotelAddress = dr["HotelAddress"].ToString();
                                obj.HotelCityName = dr["HotelCityName"].ToString();
                                obj.HotelPhone = dr["HotelPhone"].ToString();
                                obj.HotelEmail = dr["HotelEmail"].ToString();
                                obj.HotelPostCode = dr["HotelPostCode"].ToString();
                                obj.RoomTypeName = dr["RoomTypeName"].ToString();
                                obj.HotelAccommodationTypeName = dr["HotelAccommodationTypeName"].ToString();
                                obj.GuestName = dr["GuestFullName"].ToString();
                                // obj.CheckInDate = dr["CheckInDate"].ToString("dd/MM/yyyy");
                                DateTime chkindate = Convert.ToDateTime(dr["CheckInDate"]);
                                DateTime chkoutdate = Convert.ToDateTime(dr["CheckOutDate"]);
                                string BedTypeNameWithCount = "";
                                List<bedselection> list1 = new List<bedselection>();
                                List<RoomPrice> list2 = new List<RoomPrice>();
                                using (BaseRepository baseRepo = new BaseRepository())
                                {

                                    DataRow[] roomBedInfo = BizHotel.GetHotelRoomBeds(baseRepo.BizDB, "", culture, "", dr["HotelRoomID"].ToString()).Select("OptionNo = " + dr["BedOptionNo"].ToString());
                                    foreach (DataRow dssdr in roomBedInfo)
                                    {
                                        Home objbed = new Home();
                                        BedTypeNameWithCount = BedTypeNameWithCount + dssdr["BedTypeNameWithCount"].ToString() + ",";
                                    }
                                    BizUtil.TrimEnd(ref BedTypeNameWithCount, ",");
                                    //var output = BedTypeNameWithCount.Remove(BedTypeNameWithCount.Length - 1);
                                    obj.BedTypeNameWithCount = BedTypeNameWithCount;

                                    DataTable roomBedInfoDetails = BizHotel.GetHotelRoomBeds(baseRepo.BizDB, string.Empty, culture, null, dr["HotelRoomID"].ToString());
                                    DataTable optionalBedInfo = roomBedInfoDetails.AsDataView().ToTable(true, "OptionNo");

                                    foreach (DataRow optionalBed in optionalBedInfo.Rows)
                                    {
                                        bedselection obj1 = new bedselection();
                                        string optionNo1 = optionalBed["OptionNo"].ToString();
                                        DataRow[] roomBed = roomBedInfoDetails.Select("OptionNo=" + optionNo1);
                                        string roomBedText = string.Empty;
                                        foreach (DataRow bed in roomBed)
                                        {
                                            roomBedText += bed["BedTypeNameWithCount"] + ", ";
                                        }
                                        BizUtil.TrimEnd(ref roomBedText, ", ");
                                        obj1.bedoptiontext = optionNo1;
                                        obj1.bedoptionvalue = roomBedText;
                                        list1.Add(obj1);
                                        // ddlBedPreference.Items.Add(new ListItem(roomBedText, optionNo));
                                    }

                                    obj.beddropdown = list1;
                                    string hotelreservationid = dr["ID"].ToString();
                                    DateTime chksdfgvdsf = chkoutdate.AddDays(-1);
                                    roomRate = BizReservation.GetHotelReservationRate(baseRepo.BizDB, "Date", culture, string.Empty, hotelreservationid, Convert.ToString(chkindate), Convert.ToString(chksdfgvdsf));
                                    foreach (DataRow roomRates in roomRate.Rows)
                                    {
                                        RoomPrice objroomrate = new RoomPrice();
                                        objroomrate.tblheader = "<td align='center' class='NoWrap' style='padding:4px;border-right: 1px solid #ABAAA5'><b>" + roomRates["Day"].ToString() + "&nbsp;" + roomRates["MonthName"].ToString() + "</b></td>";
                                        objroomrate.tblbobyvalues = "<td align='center' class='NoWrap' style='padding:4px;border-right: 1px solid #ABAAA5'><b>" + roomRates["CurrencySymbol"].ToString() + "&nbsp;" + roomRates["Roomprice"].ToString() + "</b></td>";
                                        list2.Add(objroomrate);
                                    }
                                    obj.roomratesinglenight = list2;
                                    // DataRow[] ReservationReview = BizReservation.GetReservationReviews(baseRepo.BizDB, "", "", "", ReservationID, "", true).FirstOrDefault();
                                }



                                obj.CheckInDate = chkindate.ToString("dd MMMM yyyy");
                                obj.CheckOutDate = chkoutdate.ToString("dd MMMM yyyy");

                                if (dr["StatusID"].ToString() == "1" && todaydate < chkindate)
                                {
                                    obj.btnupdate = "update";
                                }
                                else
                                {
                                    obj.btnupdate = "false";
                                }

                                if (dr["StatusID"].ToString() == "1" && todaydate < chkindate && (dr["ReservationOperationID"].ToString() == "2" || dr["ReservationOperationID"].ToString() == "4"))
                                {
                                    obj.CheckInstauts = "creditcard";
                                }
                                else
                                {
                                    obj.CheckInstauts = "false";
                                }

                                DataTable UserValue = homeService.GetParameter("ReservationOperationCloseOutDayCount", "en");
                                string UserRemindLink = "";

                                if (UserValue != null)
                                {
                                    if (UserValue.Rows.Count > 0)
                                    {
                                        UserRemindLink = Convert.ToString(UserValue.Rows[0]["Value"].ToString());
                                    }

                                }

                                if (dr["StatusID"].ToString() == "1" && todaydate >= chkoutdate.AddDays(Convert.ToDouble(UserRemindLink)))
                                {
                                    obj.btnreview = "review";
                                }
                                else
                                {
                                    obj.btnreview = "false";
                                }

                                obj.NightCount = dr["NightCount"].ToString();
                                obj.Amount = dr["Amount"].ToString();
                                obj.HotelReservationPayableAmount = dr["HotelReservationPayableAmount"].ToString();
                                obj.PeopleCount = dr["PeopleCount"].ToString();
                                obj.NonRefundable = dr["NonRefundable"].ToString();
                                obj.RefundableDayCount = dr["RefundableDayCount"].ToString();
                                obj.PenaltyRateTypeID = dr["PenaltyRateTypeID"].ToString();
                                obj.PenaltyRateTypeName = dr["PenaltyRateTypeName"].ToString();
                                obj.PenaltyRateTypeID = dr["PenaltyRateTypeID"].ToString();
                                obj.BedOptionNo = dr["BedOptionNo"].ToString();
                                obj.TravellerTypeName = dr["TravellerTypeName"].ToString();
                                obj.TravellerTypeID = dr["TravellerTypeID"].ToString();

                                obj.HotelReservationStatusName = dr["HotelReservationStatusName"].ToString();
                                obj.ReservationOperationName = dr["ReservationOperationName"].ToString();
                                obj.CancelDateTime = dr["CancelDateTime"].ToString();
                                obj.OpDateTime = dr["OpDateTime"].ToString();
                                obj.OpUserID = dr["OpUserID"].ToString();
                                obj.EstimatedArrivalTime = dr["EstimatedArrivalTime"].ToString();
                                obj.HotelCancelTypeName = dr["HotelCancelTypeName"].ToString();
                                obj.RoomCount = dr["RoomCount"].ToString();
                                obj.HotelConditionText = HotelConditionText;
                                obj.HotelRoomID = dr["HotelRoomID"].ToString();
                                obj.HotelID = dr["HotelID"].ToString();
                                obj.CurrencySymbol = dr["CurrencySymbol"].ToString();

                                obj.Active = dr["Active"].ToString();
                                obj.HotelReservationID = dr["ID"].ToString();
                                obj.totalroomcount = roomcount;



                                bool ActiveStatus = Convert.ToBoolean(dr["Active"]);
                                double DateDiff1 = (Convert.ToDateTime(obj.CheckInDate) - DateTime.Today.Date).TotalDays;
                                bool NonRefundables = Convert.ToBoolean(dr["NonRefundable"]);
                                double RefundableDayCount = Convert.ToDouble(dr["RefundableDayCount"]);

                                if (ActiveStatus && !(NonRefundables) && DateDiff1 > RefundableDayCount)
                                {
                                    obj.cbxHotelReservation = 1;
                                }
                                else
                                {
                                    obj.cbxHotelReservation = 0;
                                }
                                if (obj.cbxHotelReservation == 1)
                                {
                                    obj.btnCancelSelected = 1;
                                }
                                else
                                {
                                    obj.btnCancelSelected = 0;
                                }
                                if (dr["StatusID"].ToString() == "1" && DateTime.Now < Convert.ToDateTime(obj.CheckInDate) && obj.btnCancelSelected == 1)
                                {
                                    obj.btnCancel = 1;
                                }
                                else
                                {
                                    obj.btnCancel = 0;
                                }

                                string HotelRoomID = dr["HotelRoomID"].ToString();
                                string HotelID = dr["HotelID"].ToString();
                                string NonRefundabletext = dr["NonRefundable"].ToString();
                                if (NonRefundabletext == "False")
                                {

                                    string RefundableInfotext0 = RefundableInfo.Replace("#Days#", dr["RefundableDayCount"].ToString());
                                    obj.RefundableInfotext = RefundableInfotext0.Replace("#Penalty#", dr["PenaltyRateTypeName"].ToString());
                                }


                                traveller = homeService.writereview(culture);

                                List<traveller> objlist1 = new List<traveller>();

                                if (traveller != null)
                                {
                                    if (traveller.Rows.Count > 0)
                                    {
                                        foreach (DataRow tr in traveller.Rows)
                                        {
                                            traveller travel = new traveller();
                                            travel.ID = tr["ID"].ToString();
                                            travel.travelertype = tr["travelertype"].ToString();
                                            objlist1.Add(travel);
                                        }

                                    }

                                }
                                obj.travellerdropdown = objlist1;

                                obj.totalPeopleCounts = totalPeopleCounts;

                                list.Add(obj);
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

        [HttpGet]
        public HttpResponseMessage WritereView(string culture)
        {
            DataTable dataTableReviewType = new DataTable();
            List<ReservationWriteReviewModel> listReviewType = new List<ReservationWriteReviewModel>();

            try
            {
                dataTableReviewType = homeService.writereview(culture);



                if (dataTableReviewType != null && dataTableReviewType.Rows.Count > 0)
                {

                    foreach (DataRow dr in dataTableReviewType.Rows)
                    {
                        ReservationWriteReviewModel writeType = new ReservationWriteReviewModel();
                        writeType.Id = Convert.ToInt32(dr["ID"]);
                        writeType.Typereview = dr["Typereview"].ToString();
                        writeType.Travelertype = dr["travelertype"].ToString();
                        writeType.Name = dr["Name"].ToString();
                        listReviewType.Add(writeType);
                    }
                }
                return Request.CreateResponse(HttpStatusCode.OK, listReviewType);
            }
            catch (Exception ex)
            {
                return LogError(ex);
            }
        }
        [HttpGet]

        public HttpResponseMessage Updatereservationdetails(string Culturecode, string reservationid, string guestname, string bedselectionno, string travelertype, string drptime, string RoomID, string reservationEmail)
        {
            int i = 0;
            string mailTemplateID = "";
            string mailFrom = "";
            string mailSubject = "";
            string mailBody = "";
            string status = "";
            try
            {
                Home inserthome = new Home();


                string[] RoomIDValue = RoomID.Split(',');
                string[] guestnames = guestname.Split(',');
                string[] bedselectionnos = bedselectionno.Split(',');
                string[] travelertypes = travelertype.Split(',');
                string[] arrivaltime = drptime.Split(',');

                for (int j = 0; j < RoomIDValue.Length; j++)
                {
                    if (RoomIDValue[j] != null)
                    {
                        i = inserthome.UpdatedReservationdetails(reservationid, Convert.ToString(guestnames[j]), Convert.ToString(bedselectionnos[j]), Convert.ToString(travelertypes[j]), Convert.ToString(arrivaltime[j]), Convert.ToString(RoomIDValue[j]));

                    }
                }
                // i = inserthome.UpdatedReservationdetails(reservationid, guestname, bedselectionno, travelertype, drptime, RoomID);
                if (i != null)
                {
                    DataTable MailTemplate = new DataTable();
                    Home mailtemp = new Home();
                    Encryption64 objEncryptreservation = new Encryption64();
                    MailTemplate = mailtemp.GetMailTemplates("SendHotelReservationModifiedEmail", Culturecode);
                    string Email = Convert.ToString(reservationEmail);
                    string EncryptedReservationid = System.Web.HttpContext.Current.Server.UrlEncode(ConvertStringToHex(objEncryptreservation.Encrypt(reservationid, "58421043")));
                    int EmailCount = 1;
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
                                //string ReservationID = Convert.ToString(Session["RegisterId"]);
                                mailBody = mailBody.Replace("#ReservationID#", reservationid);
                                mailBody = mailBody.Replace("#EncReservationID#", EncryptedReservationid);

                                if (EmailCount == 1)
                                {
                                    string MailTemplateID1 = dr["ID"].ToString();
                                    string MailFrom1 = dr["MailFrom"].ToString();
                                    string MailTo1 = Email;
                                    // string MailCC1="";
                                    string Subject1 = dr["MailSubject"].ToString();

                                    string Body1 = dr["MailBody"].ToString();
                                    DateTime SendingDateTime1 = DateTime.Now;
                                    DateTime OpDateTime1 = DateTime.Now;
                                    long OpUserID1 = 0;

                                    using (BaseRepository baseRepo = new BaseRepository())
                                    {
                                        string Status = Convert.ToString(BizMail.AddMailForSending(baseRepo.BizDB, MailTemplateID1, MailFrom1, MailTo1, string.Empty, Subject1,
        mailBody, DateTime.Now, OpUserID1));
                                    }
                                }
                                EmailCount = EmailCount + 1;

                                string DomainName = System.Configuration.ConfigurationManager.AppSettings["DomainName"];

                                System.Net.Mail.MailAddress from = new MailAddress("info@gbshotels.com", "GBS Hotels");
                                System.Net.Mail.MailAddress to = new MailAddress(Email);
                                System.Net.Mail.MailMessage m = new MailMessage(from, to);
                                m.IsBodyHtml = true;
                                m.Subject = mailSubject;
                                m.Priority = System.Net.Mail.MailPriority.High;
                                SmtpClient smtp = new SmtpClient(this.MailServer, 25);
                                if (this.MailPort != 25 && this.MailPort == 587)
                                {
                                    smtp.Port = this.MailPort;
                                    smtp.EnableSsl = true;
                                }
                                else
                                {
                                    smtp.EnableSsl = false;
                                }
                                smtp.UseDefaultCredentials = false;

                                // m.Body = sb.ToString();
                                m.BodyEncoding = System.Text.Encoding.GetEncoding("utf-8");
                                System.Net.Mail.AlternateView plainView = System.Net.Mail.AlternateView.CreateAlternateViewFromString
                                (System.Text.RegularExpressions.Regex.Replace(mailBody, @"<(.|\n)*?>", string.Empty), null, "text/plain");
                                System.Net.Mail.AlternateView htmlView = System.Net.Mail.AlternateView.CreateAlternateViewFromString(mailBody, null, "text/html");
                                m.AlternateViews.Add(plainView);
                                m.AlternateViews.Add(htmlView);
                                smtp.Credentials = new NetworkCredential(this.MailUsername, this.MailPassword);
                                try
                                {
                                    smtp.Send(m);
                                    status = "success";
                                }
                                catch
                                {
                                    status = "Failure";
                                }

                            }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                return LogError(ex);
            }
            return Request.CreateResponse(HttpStatusCode.OK, i);

        }

        [HttpGet]
        public HttpResponseMessage CancelSelectedItems(string CultureCode, string UserID, string FullName, string HotelID, string ReservationID, string CurrencySymbol, string CheckInDate, string CheckOutDate,
         string HotelRoomID, string HotelReservationID, string ActiveStatus, string amountvalue, string ReservationAmount, string HotelRoomIDChecked)
        {
            try
            {
                using (BaseRepository baseRepo = new BaseRepository())
                {
                    Int16 roomReservationCount = 0;
                    Hashtable cancelledRoomReservations = new Hashtable();
                    double cancelledAmount = 0;
                    Hashtable cancelledRoomAmount = new Hashtable();
                    Hashtable cancelledRoomCount = new Hashtable();


                    Home HotelDetails = new Home();
                    DataTable hotelInfo = new DataTable();
                    hotelInfo = HotelDetails.GetHotelBasicInfo(CultureCode, HotelID);
                    //TB_Hotel hotelInfo = BizHotel.GetHotel(baseRepo.BizDB, BizReservation.GetReservationHotelID(baseRepo.BizDB, ReservationID));
                    string ID = "";
                    string hotelCulture = "";
                    string Email = "";
                    if (hotelInfo != null)
                    {
                        if (hotelInfo.Rows.Count > 0)
                        {
                            ID = hotelInfo.Rows[0]["ID"].ToString();
                            hotelCulture = hotelInfo.Rows[0]["HotelCultureCode"].ToString();
                            Email = hotelInfo.Rows[0]["Email"].ToString();
                        }
                    }

                    // TB_Hotel hotelInfo = BizHotel.GetHotel(baseRepo.BizDB, Convert.ToInt32(HotelID));
                    // BizTbl_Culture hotelCulture = hotelInfo.BizTbl_Culture;
                    System.Globalization.CultureInfo hotelCultureInfo = new System.Globalization.CultureInfo(hotelCulture);
                    string currencySymbol = CurrencySymbol;
                    DataTable hotelRooms = BizHotel.GetHotelRooms(baseRepo.BizDB, "ID", CultureCode, Convert.ToString(ID));

                    string[] HotelRoomIDs = HotelRoomID.Split(',');
                    string[] HotelReservationIDs = HotelReservationID.Split(',');
                    string[] amounts = amountvalue.Split(',');
                    string[] actives = ActiveStatus.Split(',');
                    string[] HotelRoomIDCheckedValue = HotelRoomIDChecked.Split(',');
                    //foreach (RepeaterItem roomReservation in rptRoomReservation.Items)
                    //{

                    //    if (roomReservation.ItemType == ListItemType.Item || roomReservation.ItemType == ListItemType.AlternatingItem)
                    //    {
                    for (int i = 0; i < HotelRoomIDs.Length; i++)
                    {
                        // bool cbxHotelReservation = cbxHotelReservation;
                        string hotelReservationID = HotelReservationIDs[i];
                        string hotelRoomID = HotelRoomIDs[i];
                        string roomTypeName = Convert.ToString(hotelRooms.Select("ID=" + HotelRoomIDs[i])[0]["RoomTypeName"]);
                        bool active = Convert.ToBoolean(actives[i]);
                        double amount = Convert.ToDouble(amounts[i]);

                        if (active)
                        {
                            if (HotelRoomIDCheckedValue.Contains(hotelRoomID))
                            {
                                cancelledRoomReservations.Add(hotelReservationID, hotelRoomID);
                                cancelledAmount += amount;

                                if (!cancelledRoomCount.Contains(roomTypeName))
                                {
                                    cancelledRoomCount[roomTypeName] = 1;
                                    cancelledRoomAmount[roomTypeName] = amount;
                                }
                                else
                                {
                                    cancelledRoomCount[roomTypeName] += Convert.ToString(1);
                                    cancelledRoomAmount[roomTypeName] += Convert.ToString(amount);
                                }
                            }

                            roomReservationCount += 1;
                        }
                    }


                    //    }
                    //}

                    try
                    {


                        bool reservationCancel = false;
                        string hostName = Dns.GetHostName();
                        string IPAddress = Dns.GetHostByName(hostName).AddressList[0].ToString();


                        if (roomReservationCount == cancelledRoomReservations.Count)
                        {
                            //Tüm oda rezervasyonları iptal ediliyor

                            //Rezervasyon durumu güncellenir
                            BizReservation.SaveReservationStatus(baseRepo.BizDB, ReservationID, Convert.ToString(2), BizCommon.DefaultUserID);

                            //Rezervasyon iptal edilir
                            BizReservation.CancelHotelReservation(baseRepo.BizDB, ReservationID, string.Empty, Convert.ToString(2), BizCommon.DefaultUserID);

                            //Rezervasyon tarihçe kaydı atılır
                            BizReservation.AddReservationStatusHistory(baseRepo.BizDB, ReservationID, Convert.ToString(2), BizCommon.DefaultUserID);

                            //Yetkili işlem tarihçesine eklenir
                            //  BizUser.AddUserOperation(baseRepo.BizDB, UserID, Convert.ToString(DateTime.Now), BizCommon.Operation.ReservationCancelled, "1", ReservationID, IPAddress, null);

                            reservationCancel = true;


                        }
                        else
                        {
                            //Belirli oda rezervasyonları iptal ediliyor
                            //Canceling reservations particular room

                            //Oda rezervasyonları iptal edilir
                            //Room reservations will be canceled

                            foreach (string hotelReservationID in cancelledRoomReservations.Keys)
                            {
                                BizReservation.CancelHotelReservation(baseRepo.BizDB, string.Empty, hotelReservationID, Convert.ToString(2), BizCommon.DefaultUserID);

                                //Rezervasyon ücreti güncellenir
                                BizReservation.SaveReservationAmount(baseRepo.BizDB, ReservationID, Convert.ToDouble(Convert.ToDouble(ReservationAmount) - cancelledAmount), BizCommon.DefaultUserID);

                                //Yetkili işlem tarihçesine eklenir
                                // BizUser.AddUserOperation(baseRepo.BizDB, UserID, Convert.ToString(DateTime.Now), BizCommon.Operation.HotelRoomReservationCancelled, Convert.ToString(BizCommon.Part.Hotel), ReservationID, IPAddress, "0");

                            }

                        }

                        //Müsaitlik durumu güncellenir
                        Hashtable roomCount = new Hashtable();
                        foreach (string hotelReservationID in cancelledRoomReservations.Keys)
                        {
                            string hotelRoomID = Convert.ToString(cancelledRoomReservations[hotelReservationID]);
                            if (!roomCount.Contains(hotelRoomID))
                            {
                                roomCount[hotelRoomID] = 1;
                            }
                            else
                            {
                                roomCount[hotelRoomID] += Convert.ToString(1);
                            }
                        }
                        foreach (var hotelRoomID_loopVariable in roomCount.Keys)
                        {
                            string hotelRoomID = Convert.ToString(hotelRoomID_loopVariable);
                            BizHotel.SaveHotelRoomAvailability(baseRepo.BizDB, hotelRoomID, CheckInDate, Convert.ToString(Convert.ToDateTime(CheckOutDate).AddDays(-1)), Convert.ToInt32(roomCount[hotelRoomID]), DateTime.Now, Convert.ToInt64(BizCommon.DefaultUserID));
                        }

                        DataTable MailTemplate = new DataTable();
                        Home homeobj = new Home();
                        if (reservationCancel == true)
                        {
                            MailTemplate = homeobj.GetMailTemplates("SendHotelReservationCancelEmail", CultureCode);
                        }
                        else
                        {
                            MailTemplate = homeobj.GetMailTemplates("SendHotelRoomReservationCancelEmail", CultureCode);
                        }


                        if (MailTemplate != null)
                        {
                            if (MailTemplate.Rows.Count > 0)
                            {
                                foreach (DataRow dr in MailTemplate.Rows)
                                {
                                    string roomInfo = string.Empty;
                                    string mailTemplateID = dr["ID"].ToString();
                                    string mailFrom = dr["MailFrom"].ToString();
                                    string mailSubject = dr["MailSubject"].ToString();
                                    string mailBody = dr["MailBody"].ToString();

                                    foreach (var roomTypeName_loopVariable in cancelledRoomCount.Keys)
                                    {
                                        string roomTypeName = Convert.ToString(roomTypeName_loopVariable);
                                        roomInfo += roomTypeName + BizCommon.HTMLEmptyStr + "<i>(" + cancelledRoomCount[roomTypeName] + ")</i>" + BizCommon.HTMLEmptyStr + currencySymbol + BizCommon.HTMLEmptyStr + FormatToNumber(cancelledRoomAmount[roomTypeName], BizCommon.SystemCultureCode, hotelCulture, 2) + BizCommon.HTMLEmptyStr + "-" + BizCommon.HTMLEmptyStr;
                                    }
                                    BizUtil.TrimEnd(ref roomInfo, "-" + BizCommon.HTMLEmptyStr);

                                    mailBody = mailBody.Replace("#ReservationID#", ReservationID);
                                    mailBody = mailBody.Replace("#ReservationOwnerFullName#", FullName);
                                    mailBody = mailBody.Replace("#CheckInDate#", Convert.ToDateTime(CheckInDate).ToString(BizCommon.DateDisplayFormat, hotelCultureInfo));
                                    mailBody = mailBody.Replace("#CheckOutDate#", Convert.ToDateTime(CheckOutDate).ToString(BizCommon.DateDisplayFormat, hotelCultureInfo));
                                    mailBody = mailBody.Replace("#RoomInfo#", roomInfo);
                                    mailBody = mailBody.Replace("#Amount#", currencySymbol + BizCommon.HTMLEmptyStr + FormatToNumber(cancelledAmount, BizCommon.SystemCultureCode, hotelCulture, 2));
                                    BizMail.AddMailForSending(baseRepo.BizDB, mailTemplateID, mailFrom, Email, string.Empty, mailSubject, mailBody, DateTime.Now, Convert.ToInt64(BizCommon.DefaultUserID));
                                }
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        return LogError(ex);
                    }

                }
            }
            catch (Exception ex)
            {
                return LogError(ex);
            }
            return Request.CreateResponse(HttpStatusCode.OK, 1);
        }
        public static string FormatToNumber(object Value, string InputNumberCultureCode, string FormatNumberCultureCode, Int16 MaxDecimalLength = 5, bool RemoveDecimalZeros = true, double NumericValue = 0)
        {

            string numberStr = string.Empty;
            System.Globalization.CultureInfo inputNumberCultureInfo = new System.Globalization.CultureInfo(InputNumberCultureCode);
            System.Globalization.CultureInfo formatNumberCultureInfo = new System.Globalization.CultureInfo(FormatNumberCultureCode);
            double d = 0;



            if (Value != null && !object.ReferenceEquals(Value, DBNull.Value) && double.TryParse(Convert.ToString(Value), System.Globalization.NumberStyles.Number, inputNumberCultureInfo, out d))
            {
                if (Value is double || Value is decimal || Value is int || Value is long)
                {
                    d = Convert.ToDouble(Value);
                }

                if (d == Math.Floor(d) && RemoveDecimalZeros)
                {
                    formatNumberCultureInfo.NumberFormat.NumberDecimalDigits = 0;
                }
                else
                {
                    formatNumberCultureInfo.NumberFormat.NumberDecimalDigits = MaxDecimalLength;
                }
                numberStr = d.ToString("n", formatNumberCultureInfo);

                if (formatNumberCultureInfo.NumberFormat.NumberDecimalDigits > 0 && RemoveDecimalZeros)
                {

                    numberStr = numberStr.TrimEnd('0');
                    numberStr = numberStr.TrimEnd('.');
                    // numberStr = numberStr.TrimEnd(formatNumberCultureInfo.NumberFormat.NumberDecimalSeparator);
                }

                numberStr = numberStr.Replace(formatNumberCultureInfo.NumberFormat.NumberGroupSeparator, string.Empty);
                NumericValue = double.Parse(numberStr, formatNumberCultureInfo);

            }

            return numberStr;

        }



        [HttpGet]
        public HttpResponseMessage GetTypeReview(string culture)
        {
            DataTable dataTableReviewType = new DataTable();
            List<ReservationWriteReviewModel> listTypeReview = new List<ReservationWriteReviewModel>();

            try
            {
                dataTableReviewType = homeService.GetTypeReview(culture);

                if (dataTableReviewType != null && dataTableReviewType.Rows.Count > 0)
                {

                    foreach (DataRow dr in dataTableReviewType.Rows)
                    {
                        ReservationWriteReviewModel reviewType = new ReservationWriteReviewModel();
                        reviewType.Id = Convert.ToInt32(dr["ID"]);
                        reviewType.Typereview = dr["Typereview"].ToString();
                        listTypeReview.Add(reviewType);
                    }
                }
                return Request.CreateResponse(HttpStatusCode.OK, listTypeReview);
            }
            catch (Exception ex)
            {
                return LogError(ex);
            }
        }

        [HttpPost]
        public HttpResponseMessage InsertReview(ReservationReviewInsertModel review)
        {
            int i = 0;
            int j = 0;


            Home inserthome = new Home();
            try
            {
                if (string.IsNullOrEmpty(review.UserId))
                    review.UserId = "0";
                i = inserthome.insertreview(review.UserId, review.traveltype, review.travelerdate, review.positive, review.negative, review.name, review.Averagepoint, review.email, review.location, review.ReservationId, review.Mynamedisplay);
                if (i != 0)
                {
                    j = inserthome.insertreviewdetails(i, review.Cleaning, review.Location1, review.Comfort, review.Service, review.Facilities, review.Checkin, review.Valueofmoney);
                }
                return Request.CreateResponse(HttpStatusCode.OK, j);
            }
            catch (Exception ex)
            {
                return LogError(ex);
            }

        }

        [HttpPost]
        public HttpResponseMessage GetPopularHotel(string culture, string currency = "")
        {

            DataTable dataTableHotels = new DataTable();
            List<FavouriteHotelModel> listFavouriteHotel = new List<FavouriteHotelModel>();
            double ConveretedPrice = 0;
            double MinRoomPrice = 0;
            string CurrencyFromDB = "";
            string CurrenySym = "";

            string Averagepricepernight = homeService.GetTextMessagesAsString(culture, "DailyRoomRate");

            try
            {
                dataTableHotels = homeService.GetHotels(culture);

                if (dataTableHotels.Rows.Count > 0)
                {
                    foreach (DataRow dr in dataTableHotels.Rows)
                    {
                        FavouriteHotelModel favouriteHotel = new FavouriteHotelModel();
                        favouriteHotel.Id = dr["ID"].ToString();
                        string HotelID = dr["ID"].ToString();
                        Home gethotelreviewcount = new Home();
                        double i = 0;
                        if (dr["AveragePoint"].ToString() != "")
                        {
                            i = Convert.ToDouble(dr["AveragePoint"]);
                        }
                        favouriteHotel.Point = i;
                        favouriteHotel.Description = homeService.TruncateLongString(dr["Description"].ToString(), 200);
                        favouriteHotel.RoutingName = dr["RoutingName"].ToString();
                        favouriteHotel.HotelRoutingName = dr["HotelRoutingName"].ToString();
                        favouriteHotel.CountryCode = dr["CountryCode"].ToString();
                        favouriteHotel.ClosestAirportName = dr["ClosestAirportName"].ToString();
                        favouriteHotel.ClosestAirportDistance = dr["ClosestAirportDistance"].ToString();

                        // objMember.MainPhotoName = "http://www.gbsextranet.com/Photo/Hotel/" + dr["ID"].ToString() + "/" + dr["MainPhotoName"].ToString();

                        if (dr["MainPhotoName"].ToString() != "" && dr["MainPhotoName"].ToString() != null)
                        {
                            favouriteHotel.MainPhotoName = URL.EXTRANET_URLFULL + "Photo/Hotel/" + dr["ID"].ToString() + "/" + dr["MainPhotoName"].ToString();
                        }
                        else
                        {
                            favouriteHotel.MainPhotoName = "/Images/image_not_found-hotel-.jpg";
                        }

                        //string HotelClass1= "<img src="+@"""/Images/star-small-active.png"""+"border="+@"""0"""+">&nbsp;"   <img src="/Images/star-small-active.png" border="0">&nbsp;<img src="/Images/star-small-active.png" border="0">&nbsp;<img src="/Images/star-small-inactive.png" border="0">&nbsp;<img src="/Images/star-small-inactive.png" border="0">
                        favouriteHotel.HotelClassValue = dr["HotelClass"].ToString();
                        if (dr["HotelClass"].ToString() == "OneStar")
                        {
                            favouriteHotel.HotelClass = "<img class='imgPoster'  src=" + @"""/Images/star-small-active.png""" + "border=" + @"""0""" + ">&nbsp;";
                        }
                        else if (dr["HotelClass"].ToString() == "TwoStar")
                        {
                            favouriteHotel.HotelClass = "<img class='imgPoster' src=" + @"""/Images/star-small-active.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img class='imgPoster'  src=" + @"""/Images/star-small-active.png""" + "border=" + @"""0""" + ">&nbsp;";
                        }
                        else if (dr["HotelClass"].ToString() == "ThreeStar")
                        {
                            favouriteHotel.HotelClass = "<img  class='imgPoster' src=" + @"""/Images/star-small-active.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img class='imgPoster'  src=" + @"""/Images/star-small-active.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img class='imgPoster'  src=" + @"""/Images/star-small-active.png""" + "border=" + @"""0""" + ">&nbsp;";
                        }
                        else if (dr["HotelClass"].ToString() == "FourStar")
                        {
                            favouriteHotel.HotelClass = "<img class='imgPoster'  src=" + @"""/Images/star-small-active.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img class='imgPoster'  src=" + @"""/Images/star-small-active.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img class='imgPoster'  src=" + @"""/Images/star-small-active.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img  class='imgPoster' src=" + @"""/Images/star-small-active.png""" + "border=" + @"""0""" + ">&nbsp;";
                        }
                        else if (dr["HotelClass"].ToString() == "FiveStar")
                        {
                            favouriteHotel.HotelClass = "<img src=" + @"""/Images/star-small-active.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img src=" + @"""/Images/star-small-active.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img src=" + @"""/Images/star-small-active.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img src=" + @"""/Images/star-small-active.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img src=" + @"""/Images/star-small-active.png""" + "border=" + @"""0""" + ">&nbsp;";
                        }
                        else
                        {
                            favouriteHotel.HotelClass = "Unrated";
                        }
                        favouriteHotel.CityName = dr["CityName"].ToString();

                        if (dr["HotelRoutingName"].ToString() == "")
                        {
                            favouriteHotel.NavigateURL = "#";
                        }
                        else
                        {
                            //favouriteHotel.NavigateURL = "/Hotel_" + culture + "/" + dr["CountryCode"].ToString() + "/" + dr["HotelRoutingName"].ToString();
                            favouriteHotel.NavigateURL = "/#!/hoteldetail?hotelId=" + dr["ID"].ToString() + "&hotelname=" + dr["HotelRoutingName"].ToString();
                        }
                        favouriteHotel.CityNavigateURL = "/#!/hotelRegion?cityId=" + dr["RegionID"].ToString() + "&city=" + dr["CityRoutingName"].ToString();
                        //favouriteHotel.CityNavigateURL = "/Hotels_en/" + dr["CountryRoutingName"].ToString() + "/" + dr["CityRoutingName"].ToString();

                        favouriteHotel.CurrencyID = dr["CurrencyID"].ToString();
                        favouriteHotel.IsPreferred = dr["IsPreferred"].ToString();
                        favouriteHotel.CurrencySymbol = dr["CurrencySymbol"].ToString();
                        favouriteHotel.RoomPrice = dr["RoomPrice"].ToString();
                        favouriteHotel.CurrencyCode = dr["CurrencyCode"].ToString();
                        favouriteHotel.ReviewCount = dr["ReviewCount"].ToString();
                        favouriteHotel.Superb = dr["Superb"].ToString();
                        favouriteHotel.Hotel = dr["Hotel"].ToString();
                        favouriteHotel.ScoreFrom = dr["ScoreFrom"].ToString();
                        favouriteHotel.Reviews = dr["Reviews"].ToString();
                        favouriteHotel.DescriptionText = dr["DescriptionText"].ToString();
                        favouriteHotel.VeryGood = dr["VeryGood"].ToString();
                        favouriteHotel.Avgprice = Averagepricepernight;
                        favouriteHotel.New = dr["New"].ToString();

                        if (dr["RoomPrice"].ToString() != "")
                        {
                            MinRoomPrice = Convert.ToDouble(dr["RoomPrice"]);
                        }

                        favouriteHotel.RoomPrice = MinRoomPrice.ToString();
                        if (currency != "" && (currency != dr["CurrencyCode"].ToString()))
                        {
                            if (CurrencyFromDB != dr["CurrencyCode"].ToString())
                            {
                                CurrencyFromDB = dr["CurrencyCode"].ToString();
                                string Priceconvert = homeService.CurrencyConvertion(CurrencyFromDB, currency);
                                if (!String.IsNullOrEmpty(Priceconvert))
                                {
                                    string[] split = Priceconvert.Split(null);
                                    string price = split[0];
                                    CurrenySym = split[1];
                                    ConveretedPrice = Convert.ToDouble(price);

                                    double roomPrice = MinRoomPrice;

                                    double ConvertedRoomPrice = roomPrice * ConveretedPrice;
                                    favouriteHotel.ConvertedRoomPrice = Math.Round(ConvertedRoomPrice);
                                    favouriteHotel.NewCurrencySymbol = CurrenySym;
                                }
                                else
                                {
                                    double ConvertedRoomPrice = MinRoomPrice;
                                    favouriteHotel.ConvertedRoomPrice = Math.Round(ConvertedRoomPrice);
                                    favouriteHotel.NewCurrencySymbol = CurrenySym;
                                }
                            }
                            else
                            {
                                double ConvertedRoomPrice = MinRoomPrice;
                                favouriteHotel.ConvertedRoomPrice = Math.Round(ConvertedRoomPrice);
                                favouriteHotel.NewCurrencySymbol = CurrenySym;
                            }
                        }
                        else
                        {
                            favouriteHotel.ConvertedRoomPrice = MinRoomPrice;
                            favouriteHotel.NewCurrencySymbol = dr["CurrencySymbol"].ToString();
                        }


                        if ((dr["ReviewCount"]).ToString() == "0")
                            favouriteHotel.HotelStatus = dr["New"].ToString();
                        else
                            favouriteHotel.HotelStatus = dr["VeryGood"].ToString(); ;

                        if (i == 10.00)
                            favouriteHotel.AverageReviewPoint = "<a href=" + @"""#""" + "style=" + @"""background-color:#0BB3F9""" + "class=" + @"""btn btn-circle1""" + "><i class=" + @"""fa fa-user""" + "></i></a>" + "<a href=" + @"""#""" + "style=" + @"""background-color:#0BB3F9""" + "class=" + @"""btn btn-circle1""" + "><i class=" + @"""fa fa-user""" + "></i></a>" + "<a href=" + @"""#""" + "style=" + @"""background-color:#0BB3F9""" + "class=" + @"""btn btn-circle1""" + "><i class=" + @"""fa fa-user""" + "></i></a>" + "<a href=" + @"""#""" + "style=" + @"""background-color:#0BB3F9""" + "class=" + @"""btn btn-circle1""" + "><i class=" + @"""fa fa-user""" + "></i></a>" + "<a href=" + @"""#""" + "style=" + @"""background-color:#0BB3F9""" + "class=" + @"""btn btn-circle1""" + "><i class=" + @"""fa fa-user""" + "></i></a>" + "";
                        else if (i == 7.50)
                            favouriteHotel.AverageReviewPoint = "<a href=" + @"""#""" + "style=" + @"""background-color:#0BB3F9""" + "class=" + @"""btn btn-circle1""" + "><i class=" + @"""fa fa-user""" + "></i></a>" + "<a href=" + @"""#""" + "style=" + @"""background-color:#0BB3F9""" + "class=" + @"""btn btn-circle1""" + "><i class=" + @"""fa fa-user""" + "></i></a>" + "<a href=" + @"""#""" + "style=" + @"""background-color:#0BB3F9""" + "class=" + @"""btn btn-circle1""" + "><i class=" + @"""fa fa-user""" + "></i></a>" + "<a href=" + @"""#""" + "style=" + @"""background-color:#0BB3F9""" + "class=" + @"""btn btn-circle1""" + "><i class=" + @"""fa fa-user""" + "></i></a>" + "<a href=" + @"""#""" + "class=" + @"""btn btn-circle1""" + "><i class=" + @"""fa fa-user""" + "></i></a>" + "";
                        else if (i == 5.00)
                            favouriteHotel.AverageReviewPoint = "<a href=" + @"""#""" + "style=" + @"""background-color:#0BB3F9""" + "class=" + @"""btn btn-circle1""" + "><i class=" + @"""fa fa-user""" + "></i></a>" + "<a href=" + @"""#""" + "style=" + @"""background-color:#0BB3F9""" + "class=" + @"""btn btn-circle1""" + "><i class=" + @"""fa fa-user""" + "></i></a>" + "<a href=" + @"""#""" + "style=" + @"""background-color:#0BB3F9""" + "class=" + @"""btn btn-circle1""" + "><i class=" + @"""fa fa-user""" + "></i></a>" + "<a href=" + @"""#""" + "class=" + @"""btn btn-circle1""" + "><i class=" + @"""fa fa-user""" + "></i></a>" + "<a href=" + @"""#""" + "class=" + @"""btn btn-circle1""" + "><i class=" + @"""fa fa-user""" + "></i></a>" + "";
                        else if (i == 2.50)
                            favouriteHotel.AverageReviewPoint = "<a href=" + @"""#""" + "style=" + @"""background-color:#0BB3F9""" + "class=" + @"""btn btn-circle1""" + "><i class=" + @"""fa fa-user""" + "></i></a>" + "<a href=" + @"""#""" + "class=" + @"""btn btn-circle1""" + "><i class=" + @"""fa fa-user""" + "></i></a>" + "<a href=" + @"""#""" + "class=" + @"""btn btn-circle1""" + "><i class=" + @"""fa fa-user""" + "></i></a>" + "<a href=" + @"""#""" + "class=" + @"""btn btn-circle1""" + "><i class=" + @"""fa fa-user""" + "></i></a>" + "<a href=" + @"""#""" + "class=" + @"""btn btn-circle1""" + "><i class=" + @"""fa fa-user""" + "></i></a>" + "";
                        else
                            favouriteHotel.AverageReviewPoint = "";

                        listFavouriteHotel.Add(favouriteHotel);
                    }

                }
                return Request.CreateResponse(HttpStatusCode.OK, listFavouriteHotel);
            }
            catch (Exception ex)
            {
                return LogError(ex);
            }


        }

        [HttpPost]
        public HttpResponseMessage GetRecentHotel(string culture, string currency = "")
        {
            DataTable dataTableHotels = new DataTable();
            List<FavouriteHotelModel> listRecentHotel = new List<FavouriteHotelModel>();
            double ConveretedPrice = 0;
            double MinRoomPrice = 0;
            string CurrencyFromDB = "";
            string CurrenySym = "";
            //string Averagepricepernight = homeService.GetTextMessagesAsString(culture, "MinimumPricePerNight");
            string Averagepricepernight = homeService.GetTextMessagesAsString(culture, "DailyRoomRate");

            try
            {
                dataTableHotels = homeService.GetFeaturedHotel(culture);

                if (dataTableHotels.Rows.Count > 0)
                {
                    foreach (DataRow dr in dataTableHotels.Rows)
                    {
                        FavouriteHotelModel recentHotel = new FavouriteHotelModel();
                        recentHotel.Id = dr["ID"].ToString();

                        string HotelID = dr["ID"].ToString();
                        Home gethotelreviewcount = new Home();
                        double count = 0;
                        if (dr["AveragePoint"].ToString() != "")
                        {
                            count = Convert.ToDouble(dr["AveragePoint"]);
                        }
                        if (dr["MinimumRoomPrice"].ToString() != "")
                        {
                            MinRoomPrice = Convert.ToDouble(dr["MinimumRoomPrice"]);
                        }

                        recentHotel.RoomPrice = MinRoomPrice.ToString();
                        if (currency != "" && (currency != dr["CurrencyCode"].ToString()))
                        {
                            if (CurrencyFromDB != dr["CurrencyCode"].ToString())
                            {
                                CurrencyFromDB = dr["CurrencyCode"].ToString();
                                string Priceconvert = homeService.CurrencyConvertion(CurrencyFromDB, currency);
                                if (!String.IsNullOrEmpty(Priceconvert))
                                {
                                    string[] split = Priceconvert.Split(null);
                                    string price = split[0];
                                    CurrenySym = split[1];
                                    ConveretedPrice = Convert.ToDouble(price);

                                    double roomPrice = MinRoomPrice;
                                    double ConvertedRoomPrice = roomPrice * ConveretedPrice;
                                    recentHotel.ConvertedRoomPrice = Math.Round(ConvertedRoomPrice);
                                    recentHotel.NewCurrencySymbol = CurrenySym;
                                }
                                else
                                {
                                    //double roomPrice = MinRoomPrice;
                                    double ConvertedRoomPrice = MinRoomPrice;
                                    recentHotel.ConvertedRoomPrice = Math.Round(ConvertedRoomPrice);
                                    recentHotel.NewCurrencySymbol = CurrencyFromDB;
                                }

                            }
                            else
                            {
                                //double roomPrice = MinRoomPrice;
                                //double ConvertedRoomPrice = roomPrice * ConveretedPrice;
                                double ConvertedRoomPrice = MinRoomPrice;
                                recentHotel.ConvertedRoomPrice = Math.Round(ConvertedRoomPrice);
                                recentHotel.NewCurrencySymbol = CurrencyFromDB;
                            }
                        }
                        else
                        {
                            recentHotel.ConvertedRoomPrice = MinRoomPrice;
                            recentHotel.NewCurrencySymbol = dr["CurrencySymbol"].ToString();
                        }

                        recentHotel.Description = homeService.TruncateLongString(dr["Description"].ToString(), 200);
                        recentHotel.RoutingName = dr["RoutingName"].ToString();
                        recentHotel.ClosestAirportName = dr["ClosestAirportName"].ToString();
                        recentHotel.ClosestAirportDistance = dr["ClosestAirportDistance"].ToString();

                        if (dr["MainPhotoName"].ToString() != "" && dr["MainPhotoName"].ToString() != null)
                        {
                            recentHotel.MainPhotoName = URL.EXTRANET_URLFULL + "Photo/Hotel/" + dr["ID"].ToString() + "/" + dr["MainPhotoName"].ToString();
                        }
                        else
                        {
                            recentHotel.MainPhotoName = "/Images/image_not_found-hotel-.jpg";
                        }
                        recentHotel.HotelClassValue = dr["HotelClass"].ToString();
                        if (dr["HotelClass"].ToString() == "OneStar")
                            recentHotel.HotelClass = "<img src=" + @"""/Images/star-small-active.png""" + "border=" + @"""0""" + ">&nbsp;";
                        else if (dr["HotelClass"].ToString() == "TwoStar")
                            recentHotel.HotelClass = "<img src=" + @"""/Images/star-small-active.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img src=" + @"""/Images/star-small-active.png""" + "border=" + @"""0""" + ">&nbsp;";
                        else if (dr["HotelClass"].ToString() == "ThreeStar")
                            recentHotel.HotelClass = "<img src=" + @"""/Images/star-small-active.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img src=" + @"""/Images/star-small-active.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img src=" + @"""/Images/star-small-active.png""" + "border=" + @"""0""" + ">&nbsp;";
                        else if (dr["HotelClass"].ToString() == "FourStar")
                            recentHotel.HotelClass = "<img src=" + @"""/Images/star-small-active.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img src=" + @"""/Images/star-small-active.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img src=" + @"""/Images/star-small-active.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img src=" + @"""/Images/star-small-active.png""" + "border=" + @"""0""" + ">&nbsp;";
                        else if (dr["HotelClass"].ToString() == "FiveStar")
                            recentHotel.HotelClass = "<img src=" + @"""/Images/star-small-active.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img src=" + @"""/Images/star-small-active.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img src=" + @"""/Images/star-small-active.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img src=" + @"""/Images/star-small-active.png""" + "border=" + @"""0""" + ">&nbsp;" + "<img src=" + @"""/Images/star-small-active.png""" + "border=" + @"""0""" + ">&nbsp;";
                        else
                            recentHotel.HotelClass = "Unrated";

                        recentHotel.CityName = dr["CityName"].ToString();
                        recentHotel.CurrencyID = dr["CurrencyID"].ToString();
                        recentHotel.CurrencySymbol = dr["CurrencySymbol"].ToString();
                        recentHotel.ReviewCount = dr["ReviewCount"].ToString();
                        if ((dr["ReviewCount"]).ToString() == "0")
                        {
                            recentHotel.HotelStatus = dr["New"].ToString();
                        }
                        else
                        {
                            recentHotel.HotelStatus = dr["VeryGood"].ToString(); ;
                        }
                        recentHotel.HotelRoutingName = dr["HotelRoutingName"].ToString();
                        recentHotel.CountryCode = dr["CountryCode"].ToString();
                        recentHotel.Superb = dr["Superb"].ToString();
                        recentHotel.Hotel = dr["Hotel"].ToString();
                        recentHotel.ScoreFrom = dr["ScoreFrom"].ToString();
                        recentHotel.Reviews = dr["Reviews"].ToString();
                        recentHotel.DescriptionText = dr["DescriptionText"].ToString();
                        recentHotel.VeryGood = dr["VeryGood"].ToString();
                        recentHotel.New = dr["New"].ToString();
                        recentHotel.IsPreferred = dr["IsPreferred"].ToString();
                        recentHotel.CurrencyCode = dr["CurrencyCode"].ToString();
                        recentHotel.Avgprice = Averagepricepernight;
                        if (dr["HotelRoutingName"].ToString() == "")
                            recentHotel.NavigateURL = "#";
                        else
                        {
                            //recentHotel.NavigateURL = "/Hotel_en/" + dr["CountryCode"].ToString() + "/" + dr["HotelRoutingName"].ToString();
                            recentHotel.NavigateURL = "/#!/hoteldetail?hotelId=" + dr["ID"].ToString() + "&hotelname=" + dr["HotelRoutingName"].ToString();
                        }
                        recentHotel.CityNavigateURL = "/#!/hotelRegion?cityId=" + dr["RegionID"].ToString() + "&city=" + dr["CountryRoutingName"].ToString();
                        //recentHotel.CityNavigateURL = "/Hotels_en/" + dr["CountryRoutingName"].ToString() + "/" + dr["CityRoutingName"].ToString();
                        if (count == 10.00)
                            recentHotel.AverageReviewPoint = "<a href=" + @"""#""" + "style=" + @"""background-color:#0BB3F9""" + "class=" + @"""btn btn-circle1""" + "><i class=" + @"""fa fa-user""" + "></i></a>" + "<a href=" + @"""#""" + "style=" + @"""background-color:#0BB3F9""" + "class=" + @"""btn btn-circle1""" + "><i class=" + @"""fa fa-user""" + "></i></a>" + "<a href=" + @"""#""" + "style=" + @"""background-color:#0BB3F9""" + "class=" + @"""btn btn-circle1""" + "><i class=" + @"""fa fa-user""" + "></i></a>" + "<a href=" + @"""#""" + "style=" + @"""background-color:#0BB3F9""" + "class=" + @"""btn btn-circle1""" + "><i class=" + @"""fa fa-user""" + "></i></a>" + "<a href=" + @"""#""" + "style=" + @"""background-color:#0BB3F9""" + "class=" + @"""btn btn-circle1""" + "><i class=" + @"""fa fa-user""" + "></i></a>" + "";
                        else if (count == 7.50)
                            recentHotel.AverageReviewPoint = "<a href=" + @"""#""" + "style=" + @"""background-color:#0BB3F9""" + "class=" + @"""btn btn-circle1""" + "><i class=" + @"""fa fa-user""" + "></i></a>" + "<a href=" + @"""#""" + "style=" + @"""background-color:#0BB3F9""" + "class=" + @"""btn btn-circle1""" + "><i class=" + @"""fa fa-user""" + "></i></a>" + "<a href=" + @"""#""" + "class=" + @"""btn btn-circle1""" + "><i class=" + @"""fa fa-user""" + "></i></a>" + "<a href=" + @"""#""" + "class=" + @"""btn btn-circle1""" + "><i class=" + @"""fa fa-user""" + "></i></a>" + "<a href=" + @"""#""" + "class=" + @"""btn btn-circle1""" + "><i class=" + @"""fa fa-user""" + "></i></a>" + "";
                        else if (count == 5.00)
                            recentHotel.AverageReviewPoint = "<a href=" + @"""#""" + "style=" + @"""background-color:#0BB3F9""" + "class=" + @"""btn btn-circle1""" + "><i class=" + @"""fa fa-user""" + "></i></a>" + "<a href=" + @"""#""" + "style=" + @"""background-color:#0BB3F9""" + "class=" + @"""btn btn-circle1""" + "><i class=" + @"""fa fa-user""" + "></i></a>" + "<a href=" + @"""#""" + "style=" + @"""background-color:#0BB3F9""" + "class=" + @"""btn btn-circle1""" + "><i class=" + @"""fa fa-user""" + "></i></a>" + "<a href=" + @"""#""" + "class=" + @"""btn btn-circle1""" + "><i class=" + @"""fa fa-user""" + "></i></a>" + "<a href=" + @"""#""" + "class=" + @"""btn btn-circle1""" + "><i class=" + @"""fa fa-user""" + "></i></a>" + "";
                        else if (count == 2.50)
                            recentHotel.AverageReviewPoint = "<a href=" + @"""#""" + "style=" + @"""background-color:#0BB3F9""" + "class=" + @"""btn btn-circle1""" + "><i class=" + @"""fa fa-user""" + "></i></a>" + "<a href=" + @"""#""" + "class=" + @"""btn btn-circle1""" + "><i class=" + @"""fa fa-user""" + "></i></a>" + "<a href=" + @"""#""" + "class=" + @"""btn btn-circle1""" + "><i class=" + @"""fa fa-user""" + "></i></a>" + "<a href=" + @"""#""" + "class=" + @"""btn btn-circle1""" + "><i class=" + @"""fa fa-user""" + "></i></a>" + "<a href=" + @"""#""" + "class=" + @"""btn btn-circle1""" + "><i class=" + @"""fa fa-user""" + "></i></a>" + "";
                        else
                            recentHotel.AverageReviewPoint = "";

                        listRecentHotel.Add(recentHotel);

                    }
                }
                return Request.CreateResponse(HttpStatusCode.OK, listRecentHotel);
            }
            catch (Exception ex)
            {
                return LogError(ex);
            }


        }
        public HttpResponseMessage GetCurrencyLoad(string culture)
        {

            List<CurrencyModel> listCurrency = new List<CurrencyModel>();
            DataTable dataTableCurrency = new DataTable();
            try
            {
                dataTableCurrency = homeService.GetCurrencycode(culture);
                if (dataTableCurrency != null)
                {
                    if (dataTableCurrency.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dataTableCurrency.Rows)
                        {
                            CurrencyModel currency = new CurrencyModel();
                            currency.Id = Convert.ToInt32(dr["ID"]);
                            currency.CurrencyCode = Convert.ToString(dr["Code"]);
                            currency.CurrencySymbol = Convert.ToString(dr["Symbol"]);
                            currency.CurrencyName = Convert.ToString(dr["CurrencyName"]);
                            listCurrency.Add(currency);
                        }
                    }
                }
                return Request.CreateResponse(HttpStatusCode.OK, listCurrency);
            }
            catch (Exception ex)
            {
                return LogError(ex);
            }
        }

        [HttpPost]
        public HttpResponseMessage GetTextMessage(LabelModel model)
        {
            List<LabelModel> listLabel = new List<LabelModel>();
            DataTable dataTableKeyValueTextMessage = new DataTable();
            try
            {
                dataTableKeyValueTextMessage = homeService.GetTextMessages(model.Culture, model.MessageCode, model.LblId);
                if (dataTableKeyValueTextMessage != null && dataTableKeyValueTextMessage.Rows.Count > 0)
                {
                    foreach (DataRow dr in dataTableKeyValueTextMessage.Rows)
                    {
                        LabelModel lable = new LabelModel();
                        lable.TextMessage = Convert.ToString(dr["TextMessage"]);
                        lable.LblId = Convert.ToString(dr["ControlValues"]);
                        listLabel.Add(lable);
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, listLabel);
                }
                else
                    return Request.CreateResponse(HttpStatusCode.NotFound, "No Record(s) found.");
            }
            catch (Exception ex)
            {
                return LogError(ex);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetRecentlyviewdHotels(string culture, string userId)
        {
            try
            {
                List<RecentlyVieweHotelModel> listRecentlyViewedHotel = new List<RecentlyVieweHotelModel>();
                DataTable dataTableRecentlyViewHotel = new DataTable();

                dataTableRecentlyViewHotel = homeService.GetRecentlyViewedHotels(culture, userId);

                if (dataTableRecentlyViewHotel != null && dataTableRecentlyViewHotel.Rows.Count > 0)
                {
                    foreach (DataRow dr in dataTableRecentlyViewHotel.Rows)
                    {
                        RecentlyVieweHotelModel recentlyVieweHotel = new RecentlyVieweHotelModel();
                        recentlyVieweHotel.Id = Convert.ToInt32(dr["ID"]);
                        string HotelID = Convert.ToString(dr["ID"]);
                        recentlyVieweHotel.SearchId = Convert.ToString(dr["SearchID"]);
                        recentlyVieweHotel.Name = Convert.ToString(dr["Name"]);
                        recentlyVieweHotel.ClosestAirportName = Convert.ToString(dr["ClosestAirportName"]);
                        recentlyVieweHotel.ClosestAirportDistance = Convert.ToString(dr["ClosestAirportDistance"]);
                        recentlyVieweHotel.CityName = Convert.ToString(dr["CityName"]);

                        recentlyVieweHotel.Description = homeService.TruncateLongString(Convert.ToString(dr["Description"]), 200);
                        recentlyVieweHotel.Address = Convert.ToString(dr["Address"]);
                        recentlyVieweHotel.CountryName = Convert.ToString(dr["Countryname"]);
                        recentlyVieweHotel.ReviewCount = Convert.ToInt32(dr["ReviewCount"]);

                        if (dr["HotelClass"].ToString() == "OneStar")
                            recentlyVieweHotel.HotelClass = "<i class='fa fa-star colorhover'></i>&nbsp;";
                        else if (dr["HotelClass"].ToString() == "TwoStar")
                            recentlyVieweHotel.HotelClass = "<i class='fa fa-star colorhover'></i>&nbsp;" + "<i class='fa fa-star colorhover'></i>&nbsp;";
                        else if (dr["HotelClass"].ToString() == "ThreeStar")
                            recentlyVieweHotel.HotelClass = "<i class='fa fa-star colorhover'></i>&nbsp;" + "<i class='fa fa-star colorhover'></i>&nbsp;" + "<i class='fa fa-star colorhover'></i>&nbsp;";
                        else if (dr["HotelClass"].ToString() == "FourStar")
                            recentlyVieweHotel.HotelClass = "<i class='fa fa-star colorhover'></i>&nbsp;" + "<i class='fa fa-star colorhover'></i>&nbsp;" + "<i class='fa fa-star colorhover'></i>&nbsp;" + "<i class='fa fa-star colorhover'></i>&nbsp;";
                        else if (dr["HotelClass"].ToString() == "FiveStar")
                            recentlyVieweHotel.HotelClass = "<i class='fa fa-star colorhover'></i>&nbsp;" + "<i class='fa fa-star colorhover'></i>&nbsp;" + "<i class='fa fa-star colorhover'></i>&nbsp;" + "<i class='fa fa-star colorhover'></i>&nbsp;" + "<i class='fa fa-star colorhover'></i>&nbsp;";
                        else
                            recentlyVieweHotel.HotelClass = "Unrated";

                        if (dr["RoutingName"].ToString() == "")
                            recentlyVieweHotel.NavigateURL = "#";
                        else
                        {
                            //recentlyVieweHotel.NavigateURL = "/Hotel_en/" + dr["CountryCode"].ToString() + "/" + dr["RoutingName"].ToString();
                            recentlyVieweHotel.NavigateURL = "/#!/hoteldetail?hotelId=" + dr["ID"].ToString() + "&hotelname=" + dr["RoutingName"].ToString();
                        }
                        recentlyVieweHotel.CityNavigateURL = "/#!/hotelRegion?cityId=" + dr["RegionID"].ToString() + "&city=" + dr["CityRoutingName"].ToString();
                        //recentlyVieweHotel.CityNavigateURL = "/Hotels_en/" + dr["CountryRoutingName"].ToString() + "/" + dr["CityRoutingName"].ToString();

                        recentlyVieweHotel.Hotel = Convert.ToString(dr["Hotel"]);
                        recentlyVieweHotel.ScoreFrom = Convert.ToString(dr["ScoreFrom"]);
                        recentlyVieweHotel.Reviews = Convert.ToString(dr["Reviews"]);
                        recentlyVieweHotel.IsPreferred = Convert.ToString(dr["IsPreferred"]);

                        string avgPoint = dr["AveragePoint"].ToString();
                        recentlyVieweHotel.ReviewValue = dr["AveragePoint"].ToString();
                        if (avgPoint != "" && avgPoint != "0")
                        {
                            string typescale = homeService.gettypescale(avgPoint.ToString(), culture);
                            {
                                recentlyVieweHotel.ReviewTypeScaleName = typescale;
                            }
                        }
                        else
                        {
                            recentlyVieweHotel.ReviewTypeScaleName = "";
                        }

                        // double AverageReviewCount = Convert.ToDouble(dr["AverageReviewPoint"].ToString());
                        //Home gethotelreviewcount = new Home();
                        //double count = gethotelreviewcount.GetHotelsreviewcount(HotelID);

                        if (dr["MainPhotoName"].ToString() != "" && dr["MainPhotoName"].ToString() != null)
                            recentlyVieweHotel.MainPhotoName = URL.EXTRANET_URLFULL + "Photo/Hotel/" + dr["ID"].ToString() + "/" + dr["MainPhotoName"].ToString();
                        else
                            recentlyVieweHotel.MainPhotoName = "/Images/image_not_found-hotel-.jpg";

                        double count = 0;
                        if (dr["AveragePoint"].ToString() != "")
                            count = Convert.ToDouble(dr["AveragePoint"]);

                        if (count == 10.00)
                            recentlyVieweHotel.AverageReviewPoint = "<a href=" + @"""#""" + "style=" + @"""background-color:#0BB3F9""" + "class=" + @"""btn btn-circle1""" + "></a>" + "<a href=" + @"""#""" + "style=" + @"""background-color:#0BB3F9""" + "class=" + @"""btn btn-circle1""" + "></a>" + "<a href=" + @"""#""" + "style=" + @"""background-color:#0BB3F9""" + "class=" + @"""btn btn-circle1""" + "></a>" + "<a href=" + @"""#""" + "style=" + @"""background-color:#0BB3F9""" + "class=" + @"""btn btn-circle1""" + "></a>" + "<a href=" + @"""#""" + "style=" + @"""background-color:#0BB3F9""" + "class=" + @"""btn btn-circle1""" + "></a>" + "";
                        else if (count == 7.50)
                            recentlyVieweHotel.AverageReviewPoint = "<a href=" + @"""#""" + "style=" + @"""background-color:#0BB3F9""" + "class=" + @"""btn btn-circle1""" + "></a>" + "<a href=" + @"""#""" + "style=" + @"""background-color:#0BB3F9""" + "class=" + @"""btn btn-circle1""" + "></a>" + "<a href=" + @"""#""" + "style=" + @"""background-color:#0BB3F9""" + "class=" + @"""btn btn-circle1""" + "></a>" + "<a href=" + @"""#""" + "style=" + @"""background-color:#0BB3F9""" + "class=" + @"""btn btn-circle1""" + "></a>" + "<a href=" + @"""#""" + "class=" + @"""btn btn-circle1""" + "></a>" + "";
                        else if (count == 5.00)
                            recentlyVieweHotel.AverageReviewPoint = "<a href=" + @"""#""" + "style=" + @"""background-color:#0BB3F9""" + "class=" + @"""btn btn-circle1""" + "></a>" + "<a href=" + @"""#""" + "style=" + @"""background-color:#0BB3F9""" + "class=" + @"""btn btn-circle1""" + "></a>" + "<a href=" + @"""#""" + "style=" + @"""background-color:#0BB3F9""" + "class=" + @"""btn btn-circle1""" + "></a>" + "<a href=" + @"""#""" + "class=" + @"""btn btn-circle1""" + "></a>" + "<a href=" + @"""#""" + "class=" + @"""btn btn-circle1""" + "></a>" + "";
                        else if (count == 2.50)
                            recentlyVieweHotel.AverageReviewPoint = "<a href=" + @"""#""" + "style=" + @"""background-color:#0BB3F9""" + "class=" + @"""btn btn-circle1""" + "></a>" + "<a href=" + @"""#""" + "class=" + @"""btn btn-circle1""" + "></a>" + "<a href=" + @"""#""" + "class=" + @"""btn btn-circle1""" + "></a>" + "<a href=" + @"""#""" + "class=" + @"""btn btn-circle1""" + "></a>" + "<a href=" + @"""#""" + "class=" + @"""btn btn-circle1""" + "></a>" + "";
                        else
                            recentlyVieweHotel.AverageReviewPoint = "";

                        listRecentlyViewedHotel.Add(recentlyVieweHotel);
                    }
                }
                return Request.CreateResponse(HttpStatusCode.OK, listRecentlyViewedHotel);

            }
            catch (Exception ex)
            {

                return LogError(ex);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetMyWishlistsMaster(string culture, string userId)
        {

            DataTable dt = new DataTable();
            List<MyWishlistsModel> objlist = new List<MyWishlistsModel>();

            try
            {
                dt = homeService.GetMyWishlists(culture, userId);
                string from = homeService.GetTextMessagesAsString(culture, "from");
                string Booknow = homeService.GetTextMessagesAsString(culture, "Booknow");
                string night = homeService.GetTextMessagesAsString(culture, "night");
                string Latestbooking = homeService.GetTextMessagesAsString(culture, "Latestbooking");
                string addedon = homeService.GetTextMessagesAsString(culture, "addedon");
                string Remove = homeService.GetTextMessagesAsString(culture, "Remove");

                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        MyWishlistsModel obj = new MyWishlistsModel();
                        obj.Remove = Remove;
                        obj.from = from;
                        obj.Booknow = Booknow;
                        obj.night = night;
                        obj.Latestbooking = Latestbooking;
                        obj.addedon = addedon;
                        obj.RegionName = dr["Regionname"].ToString();
                        obj.Id = Convert.ToInt32(dr["ID"]);
                        string HotelID = dr["ID"].ToString();
                        obj.Name = dr["Name"].ToString();
                        obj.MinimumRoomPrice = dr["MinimumRoomPrice"].ToString();
                        obj.Address = dr["Address"].ToString();
                        obj.Phone = dr["Phone"].ToString();
                        obj.Fax = dr["Fax"].ToString();
                        obj.PostCode = dr["PostCode"].ToString();
                        obj.RoomCount = dr["RoomCount"].ToString();
                        obj.Description = homeService.TruncateLongString(dr["Description"].ToString(), 85);
                        obj.Email = dr["Email"].ToString();
                        obj.CountryName = dr["Countryname"].ToString();
                        obj.ReviewCount = dr["ReviewCount"].ToString();
                        obj.IsPreferred = dr["IsPreferred"].ToString();
                        obj.CreatedDate = dr["CreatedDate"].ToString();
                        obj.Hotel = dr["Hotel"].ToString();
                        obj.ScoreFrom = dr["ScoreFrom"].ToString();
                        obj.Reviews = dr["Reviews"].ToString();
                        // obj.LastReservationDate = dr["LastReservationDate"].ToString();

                        obj.CurrencySymbol = dr["CurrencySymbol"].ToString();
                        //   obj.MainPhotoName = "http://www.gbsextranet.com/Photo/Hotel/" + dr["ID"].ToString() + "/" + dr["MainPhotoName"].ToString();

                        if (dr["MainPhotoName"].ToString() != "" && dr["MainPhotoName"].ToString() != null)
                        {
                            obj.MainPhotoName = URL.EXTRANET_URLFULL + "Photo/Hotel/" + dr["ID"].ToString() + "/" + dr["MainPhotoName"].ToString();
                        }
                        else
                        {
                            obj.MainPhotoName = "/Images/image_not_found-hotel-.jpg";
                        }

                        obj.ReviewCountValue = Convert.ToInt32(dr["ReviewCount"]);

                        if (dr["IsPreferred"].ToString() == "True")
                        {

                            if (dr["HotelClass"].ToString() == "OneStar")
                            {
                                obj.HotelClass = " <li style='margin-right:10px;'><img style='width:10px;' src='/Images/preferred.png' /></li><li><i class='fa fa-star'></i></li>";
                            }
                            else if (dr["HotelClass"].ToString() == "TwoStar")
                            {
                                obj.HotelClass = "<li style='margin-right:10px;'><img style='width:10px;' src='/Images/preferred.png' /></li><li><i class='fa fa-star'></i></li><li><i class='fa fa-star'></i></li>";
                            }
                            else if (dr["HotelClass"].ToString() == "ThreeStar")
                            {
                                obj.HotelClass = "<li style='margin-right:10px;'><img style='width:10px;' src='/Images/preferred.png' /></li><li><i class='fa fa-star'></i></li><li><i class='fa fa-star'></i></li><li><i class='fa fa-star'></i></li>";
                            }
                            else if (dr["HotelClass"].ToString() == "FourStar")
                            {
                                obj.HotelClass = "<li style='margin-right:10px;'><img style='width:10px;' src='/Images/preferred.png' /></li><li><i class='fa fa-star'></i></li><li><i class='fa fa-star'></i></li><li><i class='fa fa-star'></i></li><li><i class='fa fa-star'></i></li>";
                            }
                            else if (dr["HotelClass"].ToString() == "FiveStar")
                            {
                                obj.HotelClass = "<li style='margin-right:10px;'><img style='width:10px;' src='/Images/preferred.png' /></li><li><i class='fa fa-star'></i></li><li><i class='fa fa-star'></i></li><li><i class='fa fa-star'></i></li><li><i class='fa fa-star'></i></li><li><i class='fa fa-star'></i></li>";
                            }
                            else
                            {
                                obj.HotelClass = "<li style='margin-right:10px;'><img style='width:10px;' src='/Images/preferred.png' /></li><li>Unrated</li>";
                            }
                        }
                        else
                        {

                            if (dr["HotelClass"].ToString() == "OneStar")
                            {
                                obj.HotelClass = "<li><i class='fa fa-star'></i></li>";
                            }
                            else if (dr["HotelClass"].ToString() == "TwoStar")
                            {
                                obj.HotelClass = "<li><i class='fa fa-star'></i></li><li><i class='fa fa-star'></i></li>";
                            }
                            else if (dr["HotelClass"].ToString() == "ThreeStar")
                            {
                                obj.HotelClass = "<li><i class='fa fa-star'></i></li><li><i class='fa fa-star'></i></li><li><i class='fa fa-star'></i></li>";
                            }
                            else if (dr["HotelClass"].ToString() == "FourStar")
                            {
                                obj.HotelClass = "<li><i class='fa fa-star'></i></li><li><i class='fa fa-star'></i></li><li><i class='fa fa-star'></i></li><li><i class='fa fa-star'></i></li>";
                            }
                            else if (dr["HotelClass"].ToString() == "FiveStar")
                            {
                                obj.HotelClass = "<li><i class='fa fa-star'></i></li><li><i class='fa fa-star'></i></li><li><i class='fa fa-star'></i></li><li><i class='fa fa-star'></i></li><li><i class='fa fa-star'></i></li>";
                            }
                            else
                            {
                                obj.HotelClass = "<li>Unrated</li>";
                            }
                        }
                        if (dr["RoutingName"].ToString() == "")
                        {
                            obj.NavigateURL = "#";
                        }
                        else
                        {
                            obj.NavigateURL = "/Hotel_en/" + dr["CountryCode"].ToString() + "/" + dr["RoutingName"].ToString();
                        }
                        //    double count = obj.GetHotelsreviewcount(HotelID);
                        double count = 0;
                        if (dr["AveragePoint"].ToString() != "")
                        {
                            count = Convert.ToDouble(dr["AveragePoint"]);
                        }

                        if (count == 10.00)
                        {

                            obj.ReviewStatus = "<a href=" + @"""#""" + "style=" + @"""background-color:#0BB3F9""" + "class=" + @"""btn btn-circle1""" + "></a>" + "<a href=" + @"""#""" + "style=" + @"""background-color:#0BB3F9""" + "class=" + @"""btn btn-circle1""" + "></a>" + "<a href=" + @"""#""" + "style=" + @"""background-color:#0BB3F9""" + "class=" + @"""btn btn-circle1""" + "></a>" + "<a href=" + @"""#""" + "style=" + @"""background-color:#0BB3F9""" + "class=" + @"""btn btn-circle1""" + "></a>" + "<a href=" + @"""#""" + "style=" + @"""background-color:#0BB3F9""" + "class=" + @"""btn btn-circle1""" + "></a>" + " 10.00" + " <span>" + dr["ScoreFrom"].ToString() + " " + dr["ReviewCount"].ToString() + " " + dr["Reviews"].ToString() + "</span>";

                        }
                        else if (count == 7.50)
                        {
                            obj.ReviewStatus = "<a href=" + @"""#""" + "style=" + @"""background-color:#0BB3F9""" + "class=" + @"""btn btn-circle1""" + "></a>" + "<a href=" + @"""#""" + "style=" + @"""background-color:#0BB3F9""" + "class=" + @"""btn btn-circle1""" + "></a>" + "<a href=" + @"""#""" + "style=" + @"""background-color:#0BB3F9""" + "class=" + @"""btn btn-circle1""" + "></a>" + "<a href=" + @"""#""" + "style=" + @"""background-color:#0BB3F9""" + "class=" + @"""btn btn-circle1""" + "></a>" + "<a href=" + @"""#""" + "class=" + @"""btn btn-circle1""" + "></a>" + " 7.50" + " <span>" + dr["ScoreFrom"].ToString() + " " + dr["ReviewCount"].ToString() + " " + dr["Reviews"].ToString() + "</span>";

                        }
                        else if (count == 5.00)
                        {
                            obj.ReviewStatus = "<a href=" + @"""#""" + "style=" + @"""background-color:#0BB3F9""" + "class=" + @"""btn btn-circle1""" + "></a>" + "<a href=" + @"""#""" + "style=" + @"""background-color:#0BB3F9""" + "class=" + @"""btn btn-circle1""" + "></a>" + "<a href=" + @"""#""" + "style=" + @"""background-color:#0BB3F9""" + "class=" + @"""btn btn-circle1""" + "><i class=" + @"""fa fa-user""" + "></i></a>" + "<a href=" + @"""#""" + "class=" + @"""btn btn-circle1""" + "></a>" + "<a href=" + @"""#""" + "class=" + @"""btn btn-circle1""" + "></a>" + " 5.00" + " <span>" + dr["ScoreFrom"].ToString() + " " + dr["ReviewCount"].ToString() + " " + dr["Reviews"].ToString() + "</span>";

                        }
                        else if (count == 2.50)
                        {
                            obj.ReviewStatus = "<a href=" + @"""#""" + "style=" + @"""background-color:#0BB3F9""" + "class=" + @"""btn btn-circle1""" + "></a>" + "<a href=" + @"""#""" + "class=" + @"""btn btn-circle1""" + "></a>" + "<a href=" + @"""#""" + "class=" + @"""btn btn-circle1""" + "><i class=" + @"""fa fa-user""" + "></i></a>" + "<a href=" + @"""#""" + "class=" + @"""btn btn-circle1""" + "><i class=" + @"""fa fa-user""" + "></i></a>" + "<a href=" + @"""#""" + "class=" + @"""btn btn-circle1""" + "></a>" + " 2.50" + " <span>" + dr["ScoreFrom"].ToString() + " " + dr["ReviewCount"].ToString() + " " + dr["Reviews"].ToString() + "</span>";

                        }

                        else
                        {
                            obj.ReviewStatus = "";

                        }


                        objlist.Add(obj);
                    }

                }
                return Request.CreateResponse(HttpStatusCode.OK, objlist);
            }
            catch (Exception ex)
            {
                return LogError(ex);
            }
        }
        [HttpGet]
        public HttpResponseMessage DeleteMyViewedHotels(string Id, string userId)
        {

            int i = 0;
            try
            {
                i = homeService.DeleteMyViewedHotels(Id, userId);

                return Request.CreateResponse(HttpStatusCode.OK, i);
            }
            catch (Exception ex)
            {
                return LogError(ex);
            }
        }
        public HttpResponseMessage GetPopularDestination(string culture)
        {
            List<PopularDestinationModel> listPopularDestination = new List<PopularDestinationModel>();
            DataTable dataTablePopularDestination = new DataTable();
            var Code = "Find somewhere to stay in";
            var Code1 = "hotels";
            try
            {
                dataTablePopularDestination = homeService.Getpopularcity(culture, Code, Code1);

                if (dataTablePopularDestination != null && dataTablePopularDestination.Rows.Count > 0)
                {
                    foreach (DataRow dr in dataTablePopularDestination.Rows)
                    {
                        PopularDestinationModel popularDestination = new PopularDestinationModel();
                        popularDestination.CityName = Convert.ToString(dr["Cityname"]);
                        popularDestination.CityImage = Convert.ToString("/Images/" + Convert.ToString(dr["CityID"]) + ".jpg");
                        popularDestination.Code = Convert.ToString("/Images/flag/" + Convert.ToString(dr["Code"]) + ".png");
                        popularDestination.Count = Convert.ToInt32(dr["Hotelcount"]);
                        popularDestination.Description = Convert.ToString(dr["text"]);
                        popularDestination.Description1 = Convert.ToString(dr["text1"]);
                        popularDestination.CityId = Convert.ToInt32(dr["CityID"]);
                        popularDestination.CountryName = Convert.ToString(dr["CountryName"]);
                        string CountryName = Convert.ToString(dr["CountryName"]);
                        CountryName = CountryName.Replace(" ", "%20");
                        popularDestination.NavigateURL = "/Hotels_" + culture + "/" + CountryName + "/" + Convert.ToString(dr["CityNameEng"]);
                        listPopularDestination.Add(popularDestination);
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, listPopularDestination);
                }
                else
                    return Request.CreateResponse(HttpStatusCode.NotFound, "No Record(s) found.");
            }
            catch (Exception ex)
            {
                return LogError(ex);
            }
        }

        public HttpResponseMessage GetContinentsCountry(string culture)
        {
            List<ContinentDestinationModel> listContinent = new List<ContinentDestinationModel>();
            DataTable dataTableContinent = new DataTable();
            int Counts = 0;
            try
            {
                dataTableContinent = homeService.GetContinentsCountry(culture);
                if (dataTableContinent != null && dataTableContinent.Rows.Count > 0)
                {
                    foreach (DataRow dr in dataTableContinent.Rows)
                    {
                        ContinentDestinationModel continent = new ContinentDestinationModel();
                        continent.ContinentsId = Convert.ToString(dr["ContinentID"]);
                        continent.CountryName = Convert.ToString(dr["CountryName"]);
                        continent.Id = Convert.ToInt32(dr["ID"]);
                        continent.Continents = Convert.ToString(dr["Continents"]);
                        continent.HotelCount = Convert.ToInt32(dr["HotelCount"]);
                        continent.Hotel = Convert.ToString(dr["Hotels"]);
                        continent.Didtext = Convert.ToString(dr["Didtext"]);
                        continent.Destination = Convert.ToString(dr["Destinations"]);
                        continent.Country = Convert.ToString(dr["Country"]);
                        continent.Rowcounts = Counts;
                        listContinent.Add(continent);
                        Counts = Counts + 1;
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, listContinent);
                }
                else
                    return Request.CreateResponse(HttpStatusCode.NotFound, "No record(s) found.");

            }
            catch (Exception ex)
            {
                return LogError(ex);
            }
        }

        public HttpResponseMessage GetContinents(string culture)
        {
            List<ContinentModel> listContient = new List<ContinentModel>();
            DataTable dataTableContient = new DataTable();
            int Counts = 0;
            try
            {
                dataTableContient = homeService.GetContinents(culture);
                if (dataTableContient != null && dataTableContient.Rows.Count > 0)
                {
                    foreach (DataRow dr in dataTableContient.Rows)
                    {
                        ContinentModel continent = new ContinentModel();
                        continent.Id = Convert.ToInt32(dr["ID"]);
                        continent.Name = Convert.ToString(dr["Continent"]);
                        if (Counts == 0)
                            continent.Style = "active";
                        else
                            continent.Style = string.Empty;
                        listContient.Add(continent);
                        Counts = Counts + 1;
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, listContient);
                }
                else
                    return Request.CreateResponse(HttpStatusCode.NotFound, "No record(s) found.");

            }
            catch (Exception ex)
            {
                return LogError(ex);
            }
        }

        public HttpResponseMessage GetDetinationsWithContinents(string culture, int continetId)
        {

            List<ContinentDestinationModel> listContientDestination = new List<ContinentDestinationModel>();
            DataTable dataTableContientDestination = new DataTable();
            int Counts = 0;
            try
            {
                dataTableContientDestination = homeService.GetCountryWithContinents(culture, continetId);
                if (dataTableContientDestination != null && dataTableContientDestination.Rows.Count > 0)
                {
                    foreach (DataRow dr in dataTableContientDestination.Rows)
                    {
                        ContinentDestinationModel continentDestination = new ContinentDestinationModel();
                        continentDestination.ContinentsId = Convert.ToString(dr["ContinentID"]);
                        continentDestination.CountryName = Convert.ToString(dr["CountryName"]);
                        continentDestination.CountryStartChar = continentDestination.CountryName.Substring(0, 1);
                        continentDestination.Id = Convert.ToInt32(dr["ID"]);
                        continentDestination.Continents = Convert.ToString(dr["Continents"]);
                        continentDestination.Destination = Convert.ToString(dr["Destinations"]);
                        continentDestination.Country = Convert.ToString(dr["Country"]);
                        continentDestination.Rowcounts = Counts;
                        listContientDestination.Add(continentDestination);

                        Counts = Counts + 1;
                    }

                    var data = from t in listContientDestination
                               group t by t.CountryStartChar into g
                               select new { countryStartChar = g.Key, country = g.ToList() };

                    return Request.CreateResponse(HttpStatusCode.OK, data.OrderBy(s => s.countryStartChar));
                }
                else
                    return Request.CreateResponse(HttpStatusCode.NotFound, "No Record(s) found.");

            }
            catch (Exception ex)
            {
                return LogError(ex);
            }
        }

        public HttpResponseMessage GetHotelRoomAttribute(string culture)
        {
            List<PropertAttributeModel> HotelRoomAttributeList = new List<PropertAttributeModel>();
            try
            {
                DataTable dt = new DataTable();
                DataTable dataTableAttribute = new DataTable();
                dt = homeService.GetAttributeHeaders(culture);

                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        dataTableAttribute = homeService.GetAttributes(dr["ID"].ToString(), culture);
                        if (dataTableAttribute != null)
                        {
                            if (dataTableAttribute.Rows.Count > 0)
                            {
                                foreach (DataRow Attribute in dataTableAttribute.Rows)
                                {
                                    PropertAttributeModel propertyAttribute = new PropertAttributeModel();
                                    propertyAttribute.Id = Convert.ToInt32(Attribute["ID"]);
                                    propertyAttribute.Name = Convert.ToString(Attribute["Name"]);
                                    HotelRoomAttributeList.Add(propertyAttribute);
                                }
                            }
                        }
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, HotelRoomAttributeList);
                }
                else
                    return Request.CreateResponse(HttpStatusCode.NotFound, "No record(s) found.");
            }
            catch (Exception ex)
            {
                return LogError(ex);
            }
        }

        public HttpResponseMessage GetWhyGbshotelsText(string culture)
        {
            List<string> listGbshotelsWhyText = new List<string>();
            try
            {
                string WhyGbshotelsText = homeService.GetTextMessagesAsString(culture, "WhyGbshotelsText");

                string[] myString = WhyGbshotelsText.Replace("<figure>", "").Split(new string[] { "</figure>" }, StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < myString.Length; i++)
                {
                    string TextMessage = Regex.Replace(myString[i], @"<[^>]+>|&nbsp;", "").Trim();
                    listGbshotelsWhyText.Add(TextMessage);
                }

                return Request.CreateResponse(HttpStatusCode.OK, listGbshotelsWhyText);
            }
            catch (Exception ex)
            {
                return LogError(ex);
            }
        }

        public HttpResponseMessage GetHotelCount()
        {

            List<CountModel> listCount = new List<CountModel>();
            DataSet ds = new DataSet();
            try
            {
                ds = homeService.Gethotles();
                DataTable dataTableCountry = new DataTable();
                DataTable dataTableHotel = new DataTable();
                string CountryCount = "";
                if (ds != null)
                {
                    dataTableCountry = ds.Tables[0];
                    dataTableHotel = ds.Tables[1];
                }
                if (dataTableCountry != null && dataTableCountry.Rows.Count > 0)
                {
                    foreach (DataRow dr in dataTableCountry.Rows)
                    {
                        CountryCount = dr["CountryCount"].ToString();
                    }

                }
                if (dataTableHotel != null && dataTableHotel.Rows.Count > 0)
                {
                    foreach (DataRow dr in dataTableHotel.Rows)
                    {
                        CountModel count = new CountModel();
                        count.HotelCount = Convert.ToInt32(dr["HotelCount"]);
                        count.CountryCount = Convert.ToInt32(CountryCount);
                        listCount.Add(count);
                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK, listCount);
            }
            catch (Exception ex)
            {
                return LogError(ex);
            }
        }

        public HttpResponseMessage UserLogin(LoginModel login)
        {
            string encryptPassword = homeService.Encrypt(login.Password);
            DataTable dt = new DataTable();
            try
            {
                dt = homeService.UserLogin(login.Email, encryptPassword);
                if (dt != null && dt.Rows.Count == 1)
                {

                    DataRow dr = dt.Rows[0];

                    LoginModel loginmodel = new LoginModel();
                    loginmodel.UserName = dr["UserName"].ToString();
                    loginmodel.UserId = Convert.ToString(dr["ID"]);

                    return Request.CreateResponse(HttpStatusCode.OK, loginmodel);
                }
                else
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Invalid Credential");


            }
            catch (Exception ex)
            {
                return LogError(ex);
            }
        }

        public HttpResponseMessage CheckEmail(RegisterModel register)
        {
            int i = 0;
            RegisterModel registeredUser = new RegisterModel();
            try
            {
                i = homeService.CheckEmailId(register.Email);
                if (i <= 0)
                {
                    bool status = InsertNewAccount(register.Email, register.Password, register.UserName, register.Culture, out registeredUser);
                    if (status)
                        return Request.CreateResponse(HttpStatusCode.OK, registeredUser);
                    else
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, "Error Occured while register the user please try again.");
                }
                else
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "Email Address already register please try again with different email address.");
            }
            catch (Exception ex)
            {
                return LogError(ex);
            }

        }

        public bool InsertNewAccount(string Email, string Password, string UserName, string Culturecode, out RegisterModel register)
        {
            string mailTemplateID = "";
            string mailFrom = "";
            string mailSubject = "";
            string mailBody = "";
            bool status = false;
            RegisterModel registerdUser = new RegisterModel();
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrst0123456789";
            var random = new Random();
            var varificationcode = new string(
                Enumerable.Repeat(chars, 10)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());


            string encryptPassword = homeService.Encrypt(Password);
            List<Home> objList = new List<Home>();
            DataTable dt = new DataTable();
            DataTable MailTemplate = new DataTable();

            dt = homeService.InsertNewAccount(Email, encryptPassword, UserName, varificationcode);
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];

                registerdUser.UserId = Convert.ToString(dr["ID"]);
                registerdUser.UserName = UserName;
            }
            MailTemplate = homeService.GetMailTemplates("SendUserVerificationEmail", Culturecode);
            int EmailCount = 1;
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
                        //string ReservationID = Convert.ToString(Session["RegisterId"]);
                        mailBody = mailBody.Replace("#UserFullName#", UserName);
                        mailBody = mailBody.Replace("#VerificationCode#", varificationcode);
                        mailBody = mailBody.Replace("#Culture#", Culturecode);
                        string DomainName = System.Configuration.ConfigurationManager.AppSettings["DomainName"];

                        mailBody = mailBody.Replace("http://www.Gbshotels.com/Home/Index", DomainName + "/#/home");
                        if (EmailCount == 1)
                        {
                            string MailTemplateID1 = dr["ID"].ToString();
                            string MailFrom1 = dr["MailFrom"].ToString();
                            string MailTo1 = Email;
                            // string MailCC1="";
                            string Subject1 = dr["MailSubject"].ToString();

                            string Body1 = dr["MailBody"].ToString();
                            DateTime SendingDateTime1 = DateTime.Now;
                            DateTime OpDateTime1 = DateTime.Now;
                            long OpUserID1 = 0;

                            using (BaseRepository baseRepo = new BaseRepository())
                            {
                                string Status = Convert.ToString(BizMail.AddMailForSending(baseRepo.BizDB, MailTemplateID1, MailFrom1, MailTo1, string.Empty, Subject1,
                                                                 mailBody, DateTime.Now, OpUserID1));
                            }
                        }
                        EmailCount = EmailCount + 1;

                        System.Net.Mail.MailAddress from = new MailAddress("info@gbshotels.com", "GBS Hotels");
                        System.Net.Mail.MailAddress to = new MailAddress(Email);
                        System.Net.Mail.MailMessage m = new MailMessage(from, to);
                        m.IsBodyHtml = true;
                        m.Subject = mailSubject;
                        m.Priority = System.Net.Mail.MailPriority.High;
                        SmtpClient smtp = new SmtpClient(this.MailServer, 25);
                        if (this.MailPort != 25 && this.MailPort == 587)
                        {
                            smtp.Port = this.MailPort;
                            smtp.EnableSsl = true;
                        }
                        else
                        {
                            smtp.EnableSsl = false;
                        }

                        smtp.UseDefaultCredentials = false;
                        // m.Body = sb.ToString();
                        m.BodyEncoding = System.Text.Encoding.GetEncoding("utf-8");
                        System.Net.Mail.AlternateView plainView = System.Net.Mail.AlternateView.CreateAlternateViewFromString
                        (System.Text.RegularExpressions.Regex.Replace(mailBody, @"<(.|\n)*?>", string.Empty), null, "text/plain");
                        System.Net.Mail.AlternateView htmlView = System.Net.Mail.AlternateView.CreateAlternateViewFromString(mailBody, null, "text/html");
                        m.AlternateViews.Add(plainView);
                        m.AlternateViews.Add(htmlView);
                        smtp.Credentials = new NetworkCredential(this.MailUsername, this.MailPassword);

                        try
                        {
                            smtp.Send(m);
                            status = true;
                        }
                        catch (Exception ex)
                        {
                            status = false;
                            LogError(ex);
                        }

                    }
                }
            }
            register = registerdUser;
            return status;

        }

        [HttpGet]
        public HttpResponseMessage CheckVerificationCode(string verificationCode)
        {
            try
            {

                if (verificationCode != "")
                {

                    int i = 0;
                    i = homeService.checkvarificationcode(verificationCode);

                    if (i == 1)
                        return Request.CreateResponse(HttpStatusCode.OK, true);
                    else
                        return Request.CreateResponse(HttpStatusCode.OK, false);

                }
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Verification code is empty.");
            }
            catch (Exception ex)
            {
                return LogError(ex);
            }

        }

        [HttpGet]
        public HttpResponseMessage ForgetPassword(string emailId, string culture)
        {
            string status = string.Empty;
            string mailTemplateID = "";
            string mailFrom = "";
            string mailSubject = "";
            string mailBody = "";
            string Id = "";
            string Name = "";
            string Surname = "";
            string UserName = "";
            string UserRemindLink = "";

            DataTable dt = new DataTable();
            try
            {
                dt = homeService.ForgetPassword(emailId, culture);
                if (dt != null && dt.Rows.Count > 0)
                {

                    Id = Convert.ToString(dt.Rows[0]["ID"].ToString());
                    Name = Convert.ToString(dt.Rows[0]["Name"].ToString());
                    Surname = Convert.ToString(dt.Rows[0]["Surname"].ToString());
                    UserName = Convert.ToString(dt.Rows[0]["UserName"].ToString());

                    dt = homeService.GetParameter("UserRemindLink", culture);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        UserRemindLink = Convert.ToString(dt.Rows[0]["Value"].ToString());
                    }
                    dt = homeService.GetMailTemplates("SendRemindPasswordEmail", culture);
                    int EmailCount = 1;
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            mailTemplateID = dr["ID"].ToString();
                            mailFrom = dr["MailFrom"].ToString();
                            mailSubject = dr["MailSubject"].ToString();
                            mailBody = dr["MailBody"].ToString();
                            mailBody = mailBody.Replace("#UserFullName#", Name + " " + Surname);
                            mailBody = mailBody.Replace("#RemindCode#", Id + " " + UserName);
                            string DomainName = System.Configuration.ConfigurationManager.AppSettings["DomainName"];
                            mailBody = mailBody.Replace("#RemindLink#", DomainName + "/#/home");

                            if (EmailCount == 1)
                            {
                                string MailTemplateID1 = dr["ID"].ToString();
                                string MailFrom1 = dr["MailFrom"].ToString();
                                string MailTo1 = emailId;
                                // string MailCC1="";
                                string Subject1 = dr["MailSubject"].ToString();

                                string Body1 = dr["MailBody"].ToString();
                                DateTime SendingDateTime1 = DateTime.Now;
                                DateTime OpDateTime1 = DateTime.Now;
                                long OpUserID1 = 0;

                                using (BaseRepository baseRepo = new BaseRepository())
                                {
                                    string Status = Convert.ToString(BizMail.AddMailForSending(baseRepo.BizDB, MailTemplateID1, MailFrom1, MailTo1, string.Empty, Subject1,
                                                     mailBody, DateTime.Now, OpUserID1));
                                }
                            }
                            EmailCount = EmailCount + 1;

                            System.Net.Mail.MailAddress from = new MailAddress("info@gbshotels.com", "GBS Hotels");
                            System.Net.Mail.MailAddress to = new MailAddress(emailId);
                            System.Net.Mail.MailMessage m = new MailMessage(from, to);
                            m.IsBodyHtml = false;
                            m.Subject = mailSubject;
                            m.Priority = System.Net.Mail.MailPriority.High;
                            string mailServer = System.Configuration.ConfigurationManager.AppSettings["SMTPServerName"];
                            string mailUsername = System.Configuration.ConfigurationManager.AppSettings["SMTPUsername"];
                            string mailPassword = System.Configuration.ConfigurationManager.AppSettings["SMTPPassword"];
                            SmtpClient smtp = new SmtpClient(mailServer, 25);
                            smtp.UseDefaultCredentials = false;
                            if (this.MailPort != 25 && this.MailPort == 587)
                            {
                                smtp.Port = this.MailPort;
                                smtp.EnableSsl = true;
                            }
                            else
                            {
                                smtp.EnableSsl = false;
                            }
                            // m.Body = sb.ToString();
                            m.BodyEncoding = System.Text.Encoding.GetEncoding("utf-8");
                            System.Net.Mail.AlternateView plainView = System.Net.Mail.AlternateView.CreateAlternateViewFromString
                            (System.Text.RegularExpressions.Regex.Replace(mailBody, @"<(.|\n)*?>", string.Empty), null, "text/plain");
                            System.Net.Mail.AlternateView htmlView = System.Net.Mail.AlternateView.CreateAlternateViewFromString(mailBody, null, "text/html");
                            m.AlternateViews.Add(plainView);
                            m.AlternateViews.Add(htmlView);
                            smtp.Credentials = new NetworkCredential(mailUsername, mailPassword);
                            try
                            {
                                smtp.Send(m);
                                status = "Sent";
                            }
                            catch
                            {
                                status = "Fail";
                            }
                        }
                    }

                }
                else
                    status = "NotFound";
                return Request.CreateResponse(HttpStatusCode.OK, status);
            }
            catch (Exception ex)
            {
                return LogError(ex);
            }
        }

        [HttpGet]
        public HttpResponseMessage ResetPassword(string password, string userId)
        {
            int i = 0;
            try
            {
                string encryptPassword = homeService.Encrypt(password);
                i = homeService.resetPassword(encryptPassword, userId);
                if (i > 0)
                    return Request.CreateResponse(HttpStatusCode.OK, true);
                else
                    return Request.CreateResponse(HttpStatusCode.OK, false);
            }
            catch (Exception ex)
            {
                return LogError(ex);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetDestinationSearchResult(string countryId, string culture, string keyword)
        {
            DestinationAutoSearchWrapper wrapper = new DestinationAutoSearchWrapper();
            List<DestinationAutoCompleteSearch> objvalues = new List<DestinationAutoCompleteSearch>();
            try
            {
                DataTable SearchResults = GetDestinationSearchResults(countryId, culture, keyword);
                foreach (DataRow dr in SearchResults.Rows)
                {
                    objvalues.Add(new DestinationAutoCompleteSearch(ID: dr["ID"].ToString(), DestinationSearchType: dr["DestinationSearchType"].ToString(), CountryID: dr["CountryID"].ToString(), CountryName: dr["CountryName"].ToString(), RegionID: dr["RegionID"].ToString(), RegionName: dr["RegionName"].ToString(), ParentID: dr["ParentID"].ToString(), ParentName: dr["ParentName"].ToString(), SecondParentName: dr["SecondParentName"].ToString(), RegionType: dr["RegionType"].ToString(), Code: dr["Code"].ToString(), HotelID: dr["HotelID"].ToString(), HotelName: dr["HotelName"].ToString(), IsPopular: dr["IsPopular"].ToString()));
                }
                wrapper.items = objvalues;
                return Request.CreateResponse(HttpStatusCode.OK, wrapper);

            }
            catch (Exception ex)
            {
                return LogError(ex);
            }

        }

        [HttpGet]
        public HttpResponseMessage GetDestinationSearchResultById(string culture, string regionId, string regionName)
        {
            List<DestinationAutoCompleteSearch> objvalues = new List<DestinationAutoCompleteSearch>();
            try
            {
                DataTable SearchResults = GetDestinationSearchResults(string.Empty, culture, regionName);

                DataRow[] dataResult = (from myRow in SearchResults.AsEnumerable()
                                        where myRow.Field<Int64>("ID") == Convert.ToInt64(regionId)
                                        select myRow).ToArray();
                foreach (DataRow dr in dataResult)
                {
                    objvalues.Add(new DestinationAutoCompleteSearch(ID: dr["ID"].ToString(), DestinationSearchType: dr["DestinationSearchType"].ToString(), CountryID: dr["CountryID"].ToString(), CountryName: dr["CountryName"].ToString(), RegionID: dr["RegionID"].ToString(), RegionName: dr["RegionName"].ToString(), ParentID: dr["ParentID"].ToString(), ParentName: dr["ParentName"].ToString(), SecondParentName: dr["SecondParentName"].ToString(), RegionType: dr["RegionType"].ToString(), Code: dr["Code"].ToString(), HotelID: dr["HotelID"].ToString(), HotelName: dr["HotelName"].ToString(), IsPopular: dr["IsPopular"].ToString()));
                }


                return Request.CreateResponse(HttpStatusCode.OK, objvalues);

            }
            catch (Exception ex)
            {
                return LogError(ex);
            }

        }


        [HttpGet]
        public HttpResponseMessage GetDestinationSearchResultCity(string CultureCode, string Keyword)
        {
            List<DestinationAutoCompleteSearchCity> objvalues = new List<DestinationAutoCompleteSearchCity>();
            try
            {
                DataTable SearchResults = GetDestinationSearchResultsCity(CultureCode, Keyword);
                foreach (DataRow dr in SearchResults.Rows)
                {
                    objvalues.Add(new DestinationAutoCompleteSearchCity(ID: dr["ID"].ToString(), CityName: dr["CityName"].ToString()));
                }
                return Request.CreateResponse(HttpStatusCode.OK, objvalues);

            }
            catch (Exception ex)
            {
                string hostName = Dns.GetHostName();
                //string GetUserIPAddress = Dns.GetHostByName(hostName).AddressList[0].ToString();
                //string PageName = Convert.ToString(Session["PageName"]);
                ////string GetUserIPAddress = GetUserIPAddress1();
                //using (BaseRepository baseRepo = new BaseRepository())
                //{
                //    BizApplication.AddError(baseRepo.BizDB, PageName, ex.Message, ex.StackTrace, DateTime.Now, GetUserIPAddress);
                //}
                //Session["PageName"] = "";
                CreateErrorLogFile CreateErrorLogFile = new CreateErrorLogFile();
                CreateErrorLogFile.createerrorlogfiles();
                CreateErrorLogFile.Errorlog(System.Web.HttpContext.Current.Server.MapPath("\\LogFiles\\Logs"), ex.Source.Length.ToString() + ex.StackTrace + ex.Message);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Error Occured while register the user please try again.");
            }


        }
        public DataTable GetDestinationSearchResults(string CountryID, string Culture, string Keyword)
        {
            DataTable dt = new DataTable();

            Home obj = new Home();
            try
            {
                dt = obj.GetSearchDestination(Keyword, Culture, CountryID);
            }
            catch (Exception ex)
            {

            }
            return dt;
        }
        public DataTable GetDestinationSearchResultsCity(string Culture, string Keyword)
        {
            DataTable dt = new DataTable();

            Home obj = new Home();
            try
            {
                dt = obj.GetSearchDestinationCity(Keyword, Culture);
            }
            catch (Exception ex)
            {

            }
            return dt;
        }

        [HttpGet]
        public HttpResponseMessage GetTextMessagesAsString(string culture, string title)
        {

            try
            {
                string data = homeService.GetTextMessagesAsString(culture, title);
                return Request.CreateResponse(HttpStatusCode.OK, data);
            }
            catch (Exception ex)
            {
                return LogError(ex);
            }


        }

        /// <summary>
        /// Use at Hotel serach region binding
        /// </summary>
        /// <param name="Cultureid"></param>
        /// <param name="RegionID"></param>
        /// <returns></returns>

        [HttpGet]
        public HttpResponseMessage GetPropertyRegion(string CultureId, string RegionId)
        {
            Home obj = new Home();
            List<Home> GetPropFacilities = new List<Home>();
            DataTable dt = new DataTable();
            string count = "none";
            // string RegionID = Convert.ToString(Session["RegionID"]);
            try
            {
                dt = obj.GetPropertyRegion(CultureId, RegionId);

                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            Home objmember = new Home();
                            count = "";
                            objmember.ID = dr["ID"].ToString();
                            objmember.Name = dr["Name"].ToString();
                            GetPropFacilities.Add(objmember);
                        }

                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK, dt);
            }
            catch (Exception ex)
            {
                return LogError(ex);
            }
        }

        [HttpGet]
        public HttpResponseMessage Checksocialidvalidation(string fbid, string googleid, string emailid)
        {
            LoginModel login = null;

            string userid = "";
            try
            {
                userid = homeService.Checksocialidvalidation(fbid, googleid, emailid);
                if (!string.IsNullOrEmpty(userid))
                {
                    login = new LoginModel();
                    login.UserId = userid;
                    login.UserName = userid;
                    return Request.CreateResponse(HttpStatusCode.OK, login);
                }

                return Request.CreateResponse(HttpStatusCode.OK, login);
            }
            catch (Exception ex)
            {
                return LogError(ex);
            }
        }
        [HttpGet]
        public HttpResponseMessage SocialLoginInsert(string fbid, string googleid, string emailid, string Name, string Gender, string Image, string ipAddress, string culture)
        {
            string userid = "";
            LoginModel login = null;
            try
            {
                userid = homeService.SocialLoginInsert(fbid, googleid, emailid, Name, Gender, Image, ipAddress);

                ForgetPassword(emailid, culture);
                if (!string.IsNullOrEmpty(userid))
                {
                    login = new LoginModel();
                    login.UserId = userid;
                    login.UserName = userid;
                    return Request.CreateResponse(HttpStatusCode.OK, login);
                }
                return Request.CreateResponse(HttpStatusCode.OK, login);
            }
            catch (Exception ex)
            {
                return LogError(ex);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetHotelRoomPopUpDetails(string RoomId, string CultureID, string HotelId)
        {

            List<Home> objlist = new List<Home>();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();

            Home ObjModel = new Home();
            try
            {
                ds = ObjModel.GetHotelRoomPopUpDetails(HotelId, RoomId, CultureID);

                string close = ObjModel.GetTextMessagesAsString(CultureID, "close");
                string beds = ObjModel.GetTextMessagesAsString(CultureID, "beds");
                if (ds != null)
                {
                    dt = ds.Tables[0];
                    dt1 = ds.Tables[1];
                    dt2 = ds.Tables[2];
                }



                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            Home obj = new Home();

                            obj.FacilityName = dr["Facilities"].ToString();
                            obj.Description = dr["Description"].ToString();
                            obj.PenaltyRate = dr["Penalty"].ToString();
                            obj.cancel = dr["Cancel"].ToString();
                            obj.CurrencyName = dr["CurrencyName"].ToString();
                            obj.Description = dr["Description"].ToString();
                            obj.MaxPeopleCount = dr["MaxPeopleCount"].ToString();
                            obj.MaxChildrenCount = dr["MaxChildrenCount"].ToString();
                            obj.CultureDescription = dr["DescriptionName"].ToString();
                            obj.Facilitiesof = dr["FacilitiesName"].ToString();
                            obj.RoomBedInfo = dr["RoomBedInfo"].ToString();
                            obj.Book = dr["BookNow"].ToString();
                            obj.HotelCurrency = dr["HotelCurrency"].ToString();
                            obj.Icon = dr["Icon"].ToString();
                            obj.CancelPolicyText = dr["CancelPolicy"].ToString();
                            //  obj.MinimunRoomRate = dr["MinimumRoomPrice"].ToString();
                            obj.AndName = dr["AndName"].ToString();
                            obj.OrName = dr["OrName"].ToString();
                            obj.Close = close;
                            obj.beds = beds;
                            objlist.Add(obj);
                        }

                    }

                }

                ArrayList OptionArray = new ArrayList();
                ArrayList BedID = new ArrayList();
                ArrayList HotelPhotoArray = new ArrayList();

                string optionNo = "";
                string BedText = "";
                int countValue = 0;


                if (dt1 != null)
                {
                    if (dt1.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt1.Rows)
                        {
                            Home objDetails = new Home();

                            //if (dr["HotelRoomID"].ToString() == RoomId)
                            //{
                            //    if (optionNo != dr["OptionNo"].ToString())
                            //    {
                            //        optionNo = dr["OptionNo"].ToString();
                            //        countValue++;
                            //        if (BedText == "")
                            //        {
                            //            BedText = dr["BedTypeNameWithCount"].ToString();
                            //        }
                            //        else
                            //        {
                            //            BedText = BedText + "  <b>or</b>  " + dr["BedTypeNameWithCount"].ToString();
                            //        }
                            //    }
                            //    else
                            //    {
                            //        BedText = BedText + "," + dr["BedTypeNameWithCount"].ToString();
                            //    }

                            //}

                            if (!OptionArray.Contains(Convert.ToString(dr["OptionNo"])) || !BedID.Contains(Convert.ToString(dr["ID"].ToString())))
                            {

                                OptionArray.Add(dr["OptionNo"].ToString());
                                BedID.Add(dr["ID"].ToString());
                                if (optionNo != dr["OptionNo"].ToString())
                                {
                                    optionNo = dr["OptionNo"].ToString();
                                    countValue++;
                                    if (BedText == "")
                                    {
                                        BedText = dr["BedTypeNameWithCount"].ToString();
                                    }
                                    else
                                    {
                                        BedText = BedText + "  <b>or</b>  " + dr["BedTypeNameWithCount"].ToString();
                                    }


                                }
                                else
                                {
                                    BedText = BedText + "," + dr["BedTypeNameWithCount"].ToString();
                                }
                            }

                            //objDetails.OptionNo = dr["OptionNo"].ToString();
                            //objDetails.HotelRoomID = dr["HotelRoomID"].ToString();
                            //objDetails.BedTypeID = dr["BedTypeID"].ToString();
                            //objDetails.BedTypeName = dr["BedTypeName"].ToString();
                            //objDetails.HotelID = dr["HotelID"].ToString();
                            //objDetails.BedTypeNameWithCount = dr["BedTypeNameWithCount"].ToString();
                            //objDetails.MainPhotoName = dr["Name"].ToString();

                            if (!HotelPhotoArray.Contains(Convert.ToString(dr["Name"])))
                            {
                                HotelPhotoArray.Add(Convert.ToString(dr["Name"]));
                                objDetails.MainPhotoName = URL.EXTRANET_URLFULL + "Photo/Hotel/" + dr["HotelID"].ToString() + "/" + dr["Name"].ToString();
                            }
                            objDetails.BedTypeNameWithCount = BedText;
                            objlist.Add(objDetails);
                            //  objlist.Add(objDetails);
                        }

                    }

                    else
                    {
                        Home ObjImage = new Home();
                        ObjImage.MainPhotoName = "/Images/image_not_found-hotel-.jpg";
                        objlist.Add(ObjImage);
                    }

                }

                if (dt2 != null)
                {
                    if (dt2.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt2.Rows)
                        {
                            Home objRate = new Home();
                            objRate.MinimunRoomRate = dr["RoomRate"].ToString();
                            objRate.CurrencySymbol = dr["CurrencySymbol"].ToString();
                            objRate.froms = dr["FromName"].ToString();
                            objlist.Add(objRate);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return LogError(ex);
            }
            return Request.CreateResponse(HttpStatusCode.OK, objlist);
        }

    }
}





