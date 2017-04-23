using GBSHotels.API.Helper;
using GBSHotels.API.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace GBSHotels.API.Controllers
{
    //[EnableCors(origins: "http://localhost:49376", headers: "*", methods: "*")]
    public class ProfileController : BaseApiController
    {
        private Home homeService;
        public ProfileController()
        {
            homeService = new Home();
        }
        public HttpResponseMessage GetUserDashboardDetails(string culture, string userId)
        {
            DataTable dt = new DataTable();

            try
            {
                dt = homeService.GetUserDashBoardDetails(culture, userId);

                if (dt != null && dt.Rows.Count == 1)
                {
                    ProfileModel profile = new ProfileModel();
                    profile.UserId = Convert.ToString(dt.Rows[0]["ID"]);
                    profile.Name = Convert.ToString(dt.Rows[0]["Name"]);
                    profile.UserPhoto = Convert.ToString(dt.Rows[0]["Userphoto"]);
                    profile.CreatedDate = Convert.ToString(dt.Rows[0]["CreatedDate"]);
                    profile.CountryCount = Convert.ToInt32(dt.Rows[0]["CountryCount"]);
                    profile.CityCount = Convert.ToInt32(dt.Rows[0]["CityCount"]);
                    profile.TripCount = Convert.ToInt32(dt.Rows[0]["TripCount"]);
                    profile.VerificationCode = Convert.ToString(dt.Rows[0]["VerificationCode"]);
                    profile.Genius = !String.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["Genius"])) ?Convert.ToBoolean(dt.Rows[0]["Genius"]):false;
                    return Request.CreateResponse(HttpStatusCode.OK, profile);
                }
                else
                    return Request.CreateResponse(HttpStatusCode.NotFound, "No Record(s) found.");
            }
            catch (Exception ex)
            {
                return LogError(ex);
            }
        }
        public HttpResponseMessage GetRecentActiveReservation(string culture, int userId)
        {
            DataTable dataTableRecentActiveReservation = new DataTable();

            List<RecentActiveReservationModel> listRecentActiveReservation = new List<RecentActiveReservationModel>();
            try
            {
                dataTableRecentActiveReservation = homeService.GetRecentActiveReservation(culture, Convert.ToString(userId));

                if (dataTableRecentActiveReservation != null && dataTableRecentActiveReservation.Rows.Count > 0)
                {
                    foreach (DataRow dr in dataTableRecentActiveReservation.Rows)
                    {
                        RecentActiveReservationModel recentActiveReservation = new RecentActiveReservationModel();
                        recentActiveReservation.ReservationId = Convert.ToString(dr["ID"]);
                        recentActiveReservation.Property = Convert.ToString(dr["Name"]);
                        recentActiveReservation.CurrencySymbol = Convert.ToString(dr["Symbol"]);

                        if (!string.IsNullOrEmpty(recentActiveReservation.CurrencySymbol))
                            recentActiveReservation.Amount = Convert.ToString(dr["Symbol"]) + Convert.ToString(dr["Amount"]);
                        else
                            recentActiveReservation.Amount = Convert.ToString(dr["Amount"]);
                        listRecentActiveReservation.Add(recentActiveReservation);
                    }
                   
                }
                return Request.CreateResponse(HttpStatusCode.OK, listRecentActiveReservation);
            }
            catch (Exception ex)
            {
                return LogError(ex);
            }
        }
        public HttpResponseMessage GetYourReservation(string culture, int userId)
        {
            DataTable dataTableReservationHistory = new DataTable();
            List<ReservationHistoryModel> listReservationHistory = new List<ReservationHistoryModel>();

            try
            {
                dataTableReservationHistory = homeService.GetYourReservation(culture, Convert.ToString(userId));

                if (dataTableReservationHistory != null && dataTableReservationHistory.Rows.Count > 0)
                {
                    foreach (DataRow dr in dataTableReservationHistory.Rows)
                    {
                        ReservationHistoryModel reservationHistory = new ReservationHistoryModel();
                        reservationHistory.EncReservationId = Convert.ToString(dr["EncReservationID"]);
                        reservationHistory.EncPinCode = Convert.ToString(dr["EncPinCode"]);
                        reservationHistory.ReservationId = Convert.ToString(dr["ID"]);
                        reservationHistory.PinCode = Convert.ToString(dr["PinCode"]);
                        reservationHistory.ReservationDate = Convert.ToString(dr["ReservationDate"]);
                        reservationHistory.Property = Convert.ToString(dr["Name"]);
                        reservationHistory.CheckInDate = Convert.ToString(dr["CheckInDate"]);
                        reservationHistory.CheckOutDate = Convert.ToString(dr["CheckOutDate"]);
                        reservationHistory.StatusId = Convert.ToInt32(dr["StatusID"]);
                        reservationHistory.Status = Convert.ToString(dr["StatusName"]);
                        reservationHistory.CurrencySymbol = Convert.ToString(dr["Symbol"]);
                        if (!string.IsNullOrEmpty(reservationHistory.CurrencySymbol))
                            reservationHistory.PayableAmount = Convert.ToString(dr["Symbol"]) + Convert.ToString(dr["PayableAmount"]);
                        else
                            reservationHistory.PayableAmount = Convert.ToString(dr["PayableAmount"]);
                        listReservationHistory.Add(reservationHistory);
                    }
                    
                }
                return Request.CreateResponse(HttpStatusCode.OK, listReservationHistory);
            }
            catch (Exception ex)
            {
                return LogError(ex);
            }
        }
        public HttpResponseMessage GetProfileDetails(string culture, string userId)
        {

            DataTable dataTableProfile = new DataTable();
            dataTableProfile = homeService.getProfileDetails(userId, culture);
            try
            {
                if (dataTableProfile != null && dataTableProfile.Rows.Count > 0)
                {
                    DataRow dr = dataTableProfile.Rows[0];
                    ProfileModel profile = new ProfileModel();
                    profile.UserPhoto = Convert.ToString(dr["Userphoto"]);
                    profile.Image = "../Images/120087.jpg";
                    profile.UserName = Convert.ToString(dr["UserName"]);
                    profile.Email = Convert.ToString(dr["Email"]);
                    profile.Name = Convert.ToString(dr["Name"]);
                    profile.Surname = Convert.ToString(dr["Surname"]);
                    profile.Phone = Convert.ToString(dr["Phone"]);
                    profile.Country = Convert.ToString(dr["Country"]);
                    if (!String.IsNullOrEmpty(Convert.ToString(dr["CountryID"])))
                    {
                        profile.CountryId = Convert.ToInt32(dr["CountryID"]);
                    }
                    else 
                        profile.CountryId = 0;
                    profile.City = Convert.ToString(dr["City"]);
                    profile.CityId = Convert.ToString(dr["CityID"]);
                    profile.Address = Convert.ToString(dr["Address"]);
                    profile.PostCode = Convert.ToString(dr["PostCode"]);
                    return Request.CreateResponse(HttpStatusCode.OK, profile);
                }
                else
                    return Request.CreateResponse(HttpStatusCode.NotFound, "No record(s) found.");
            }
            catch (Exception ex)
            {
                return LogError(ex);
            }
        }
        public HttpResponseMessage UpdateProfile(ProfileModel profile)
        {
            int success = 0;
            try
            {
                success = homeService.Updateprofile(profile.UserId, profile.UserName, profile.Email, profile.Name, profile.Surname, profile.Phone, Convert.ToString(profile.CountryId), profile.Country, profile.CityId, profile.City, profile.Address, profile.PostCode);
                if (success > 0)
                    return Request.CreateResponse(HttpStatusCode.OK, true);
                else
                    return Request.CreateResponse(HttpStatusCode.OK, false);
            }

            catch (Exception ex)
            {
                return LogError(ex);
            }
        }
        public HttpResponseMessage ChangePassword(ChangePasswordModel changePassword)
        {
            int success = 0;
            try
            {
                string encryptPassword = homeService.Encrypt(changePassword.NewPassword);
                string encryptCurrentPassword = homeService.Encrypt(changePassword.OldPassword);

                string pass = string.Empty;
                Home inserthome = new Home();

                pass = inserthome.CheckCurrentPassword(changePassword.UserId, encryptCurrentPassword);

                if (pass == "1")
                {
                    success = inserthome.Changepassword(changePassword.UserId, encryptPassword);
                    return Request.CreateResponse(HttpStatusCode.OK, true);
                }
                else
                    return Request.CreateResponse(HttpStatusCode.OK, false);

            }
            catch (Exception ex)
            {
                return LogError(ex);
            }
        }
        public HttpResponseMessage GetMyWishlists(string culture, string userId)
        {
            DataTable dataTableMyWishlist = new DataTable();
            List<MyWishlistsModel> listmywishlist = new List<MyWishlistsModel>();
            try
            {
                dataTableMyWishlist = homeService.GetMyWishlists(culture, userId);
                string from = homeService.GetTextMessagesAsString(culture, "from");
                string Booknow = homeService.GetTextMessagesAsString(culture, "Booknow");
                string night = homeService.GetTextMessagesAsString(culture, "night");
                string Latestbooking = homeService.GetTextMessagesAsString(culture, "Latestbooking");
                string addedon = homeService.GetTextMessagesAsString(culture, "addedon");
                string Remove = homeService.GetTextMessagesAsString(culture, "Remove");
                if (dataTableMyWishlist != null && dataTableMyWishlist.Rows.Count > 0)
                {
                    foreach (DataRow dr in dataTableMyWishlist.Rows)
                    {
                        MyWishlistsModel myWishlist = new MyWishlistsModel();
                        myWishlist.Remove = Remove;
                        myWishlist.from = from;
                        myWishlist.Booknow = Booknow;
                        myWishlist.night = night;
                        myWishlist.Latestbooking = Latestbooking;
                        myWishlist.addedon = addedon;
                        myWishlist.RegionName = Convert.ToString(dr["Regionname"]);
                        myWishlist.Id = Convert.ToInt32(dr["ID"]);
                        string HotelID = Convert.ToString(dr["ID"]);
                        myWishlist.Name = Convert.ToString(dr["Name"]);
                        myWishlist.MinimumRoomPrice = Convert.ToString(dr["MinimumRoomPrice"]);
                        myWishlist.Address = Convert.ToString(dr["Address"]);
                        myWishlist.Phone = Convert.ToString(dr["Phone"]);
                        myWishlist.Fax = Convert.ToString(dr["Fax"]);
                        myWishlist.PostCode = Convert.ToString(dr["PostCode"]);
                        myWishlist.RoomCount = Convert.ToString(dr["RoomCount"]);
                        myWishlist.Description = homeService.TruncateLongString(Convert.ToString(dr["Description"]), 200);
                        myWishlist.Email = Convert.ToString(dr["Email"]);
                        myWishlist.CountryName = Convert.ToString(dr["Countryname"]);
                        myWishlist.ReviewCount = Convert.ToString(dr["ReviewCount"]);
                        myWishlist.IsPreferred = Convert.ToString(dr["IsPreferred"]);
                        myWishlist.CreatedDate = Convert.ToString(dr["CreatedDate"]);
                        myWishlist.Hotel = Convert.ToString(dr["Hotel"]);
                        myWishlist.ScoreFrom = Convert.ToString(dr["ScoreFrom"]);
                        myWishlist.Reviews = Convert.ToString(dr["Reviews"]);
                        myWishlist.CurrencySymbol = Convert.ToString(dr["CurrencySymbol"]);

                        if (!string.IsNullOrEmpty(Convert.ToString(dr["MainPhotoName"])))
                            myWishlist.MainPhotoName = URL.EXTRANET_URLFULL + "Photo/Hotel/" + Convert.ToString(dr["ID"]) + "/" + Convert.ToString(dr["MainPhotoName"]);
                        else
                            myWishlist.MainPhotoName = "/Images/image_not_found-hotel-.jpg";
                        myWishlist.ReviewCountValue = Convert.ToInt32(dr["ReviewCount"]);


                        if (Convert.ToString(dr["HotelClass"]) == "OneStar")
                            myWishlist.HotelClass = "<i style='color: #ed8323 !important; font-size:22px;' class='fa fa-star'></i>&nbsp;";
                        else if (Convert.ToString(dr["HotelClass"]) == "TwoStar")
                            myWishlist.HotelClass = "<i style='color: #ed8323 !important; font-size:22px;' class='fa fa-star'></i>&nbsp;" + "<i style='color: #ed8323 !important; font-size:22px;' class='fa fa-star'></i>&nbsp;";
                        else if (Convert.ToString(dr["HotelClass"]) == "ThreeStar")
                            myWishlist.HotelClass = "<i style='color: #ed8323 !important; font-size:22px;' class='fa fa-star'></i>&nbsp;" + "<i style='color: #ed8323 !important; font-size:22px;' class='fa fa-star'></i>&nbsp;" + "<i style='color: #ed8323 !important; font-size:22px;' class='fa fa-star'></i>&nbsp;";
                        else if (Convert.ToString(dr["HotelClass"]) == "FourStar")
                            myWishlist.HotelClass = "<i style='color: #ed8323 !important; font-size:22px;' class='fa fa-star'></i>&nbsp;" + "<i style='color: #ed8323 !important; font-size:22px;' class='fa fa-star'></i>&nbsp;" + "<i style='color: #ed8323 !important; font-size:22px;' class='fa fa-star'></i>&nbsp;" + "<i style='color: #ed8323 !important; font-size:22px;' class='fa fa-star'></i>&nbsp;";
                        else if (Convert.ToString(dr["HotelClass"]) == "FiveStar")
                            myWishlist.HotelClass = "<i style='color: #ed8323 !important; font-size:22px;' class='fa fa-star'></i>&nbsp;" + "<i style='color: #ed8323 !important; font-size:22px;' class='fa fa-star'></i>&nbsp;" + "<i style='color: #ed8323 !important; font-size:22px;' class='fa fa-star'></i>&nbsp;" + "<i style='color: #ed8323 !important; font-size:22px;' class='fa fa-star'></i>&nbsp;" + "<i style='color: #ed8323 !important; font-size:22px;' class='fa fa-star'></i>&nbsp;";
                        else
                            myWishlist.HotelClass = "Unrated";
                        if (Convert.ToString(dr["RoutingName"]) == "")
                            myWishlist.NavigateURL = "#";
                        else
                            myWishlist.NavigateURL = "/Hotel_en/" + dr["CountryCode"].ToString() + "/" + dr["RoutingName"].ToString();

                        double count = 0;
                        if (!string.IsNullOrEmpty(Convert.ToString(dr["AveragePoint"])))
                            count = Convert.ToDouble(dr["AveragePoint"]);
                        if (count == 10.00)
                            myWishlist.ReviewStatus = "<a href=" + @"""#""" + "style=" + @"""background-color:#0BB3F9""" + "class=" + @"""btn btn-circle1""" + "></a>" + "<a href=" + @"""#""" + "style=" + @"""background-color:#0BB3F9""" + "class=" + @"""btn btn-circle1""" + "></a>" + "<a href=" + @"""#""" + "style=" + @"""background-color:#0BB3F9""" + "class=" + @"""btn btn-circle1""" + "></a>" + "<a href=" + @"""#""" + "style=" + @"""background-color:#0BB3F9""" + "class=" + @"""btn btn-circle1""" + "></a>" + "<a href=" + @"""#""" + "style=" + @"""background-color:#0BB3F9""" + "class=" + @"""btn btn-circle1""" + "></a>" + " 10.00" + " <span>" + dr["ScoreFrom"].ToString() + " " + dr["ReviewCount"].ToString() + " " + dr["Reviews"].ToString() + "</span>";
                        else if (count == 7.50)
                            myWishlist.ReviewStatus = "<a href=" + @"""#""" + "style=" + @"""background-color:#0BB3F9""" + "class=" + @"""btn btn-circle1""" + "></a>" + "<a href=" + @"""#""" + "style=" + @"""background-color:#0BB3F9""" + "class=" + @"""btn btn-circle1""" + "></a>" + "<a href=" + @"""#""" + "style=" + @"""background-color:#0BB3F9""" + "class=" + @"""btn btn-circle1""" + "></a>" + "<a href=" + @"""#""" + "style=" + @"""background-color:#0BB3F9""" + "class=" + @"""btn btn-circle1""" + "></a>" + "<a href=" + @"""#""" + "class=" + @"""btn btn-circle1""" + "></a>" + " 7.50" + " <span>" + dr["ScoreFrom"].ToString() + " " + dr["ReviewCount"].ToString() + " " + dr["Reviews"].ToString() + "</span>";
                        else if (count == 5.00)
                            myWishlist.ReviewStatus = "<a href=" + @"""#""" + "style=" + @"""background-color:#0BB3F9""" + "class=" + @"""btn btn-circle1""" + "></a>" + "<a href=" + @"""#""" + "style=" + @"""background-color:#0BB3F9""" + "class=" + @"""btn btn-circle1""" + "></a>" + "<a href=" + @"""#""" + "style=" + @"""background-color:#0BB3F9""" + "class=" + @"""btn btn-circle1""" + "><i class=" + @"""fa fa-user""" + "></i></a>" + "<a href=" + @"""#""" + "class=" + @"""btn btn-circle1""" + "></a>" + "<a href=" + @"""#""" + "class=" + @"""btn btn-circle1""" + "></a>" + " 5.00" + " <span>" + dr["ScoreFrom"].ToString() + " " + dr["ReviewCount"].ToString() + " " + dr["Reviews"].ToString() + "</span>";
                        else if (count == 2.50)
                            myWishlist.ReviewStatus = "<a href=" + @"""#""" + "style=" + @"""background-color:#0BB3F9""" + "class=" + @"""btn btn-circle1""" + "></a>" + "<a href=" + @"""#""" + "class=" + @"""btn btn-circle1""" + "></a>" + "<a href=" + @"""#""" + "class=" + @"""btn btn-circle1""" + "><i class=" + @"""fa fa-user""" + "></i></a>" + "<a href=" + @"""#""" + "class=" + @"""btn btn-circle1""" + "><i class=" + @"""fa fa-user""" + "></i></a>" + "<a href=" + @"""#""" + "class=" + @"""btn btn-circle1""" + "></a>" + " 2.50" + " <span>" + dr["ScoreFrom"].ToString() + " " + dr["ReviewCount"].ToString() + " " + dr["Reviews"].ToString() + "</span>";
                        else
                            myWishlist.ReviewStatus = "";
                        listmywishlist.Add(myWishlist);
                    }

                }
                return Request.CreateResponse(HttpStatusCode.OK, listmywishlist);
            }
            catch (Exception ex)
            {
                return LogError(ex);
            }
        }

        [HttpGet]
        public HttpResponseMessage InsertIntoWishlist(string userId, string hotelId)
        {
            string i = "";
            try
            {
                i = homeService.InsertIntoWishlist(hotelId, userId);
                return Request.CreateResponse(HttpStatusCode.OK, i);
            }
            catch (Exception ex)
            {
                return LogError(ex);
            }

        }


        //check if user added to wishlist or not
        [HttpGet]
        public HttpResponseMessage CheckuserWishlistStaus(string userId, string hotelId)
        {
            try
            {
                int i  = homeService.CheckuserWishlistStaus(hotelId, userId);
                return Request.CreateResponse(HttpStatusCode.OK, i);
            }
            catch (Exception ex)
            {
                return LogError(ex);
            }

        }



        [HttpPost]
        public HttpResponseMessage CheckCrediCard(ProfileCreditCardModel creditCard)
        {
            string i = "";
            try
            {
                i = homeService.Checkcredicards(creditCard.UserId, homeService.Encrypt(creditCard.CreditCardNumber));
                if (i != "update")
                {
                    bool status = InsertCrediCard(creditCard);
                    if (status)
                        return Request.CreateResponse(HttpStatusCode.OK, "Accept");
                    else
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, "Error Occured while save credit card.");
                }
                else
                    return Request.CreateResponse(HttpStatusCode.OK, "Exist");
            }
            catch (Exception ex)
            {
                return LogError(ex);
            }

        }
        public bool InsertCrediCard(ProfileCreditCardModel creditCard)
        {
            int i = 0;
            bool status = false;
            string CardExpriryDate = "";

            CardExpriryDate = creditCard.Month + "/" + creditCard.Year;
            i = homeService.insertcredicards(creditCard.UserId, creditCard.CreditCardProvider, homeService.Encrypt(creditCard.CreditCardNumber), homeService.Encrypt(creditCard.NameOnCreditcard), homeService.Encrypt(CardExpriryDate));
            if (i > 0)
                status = true;
            else
                status = false;
            return status;
        }
        [HttpGet]
        public HttpResponseMessage RemoveCreditCard(string id)
        {
            int i = 0;
            try
            {
                i = homeService.Removecreditcard(id);
                if (i > 0)
                    return Request.CreateResponse(HttpStatusCode.OK, true);
                else
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "Error occured while removing card");
            }
            catch (Exception ex)
            {
                return LogError(ex);
            }
        }

        [HttpPost]
        public HttpResponseMessage UpdateCreditCard(ProfileCreditCardModel creditCard)
        {
            int i = 0;
            try
            {
                string CardExpriryDate = creditCard.Month + "/" + creditCard.Year;
                i = homeService.Updatecreditcard(creditCard.ID, homeService.Encrypt(CardExpriryDate));
                if (i > 0)
                    return Request.CreateResponse(HttpStatusCode.OK, "Success");
                else
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "Error occured while updating card");

            }
            catch (Exception ex)
            {
                return LogError(ex);
            }
        }
        public HttpResponseMessage GetUserCreditCard(string culture, string userId)
        {
            List<UserCreditCardModel> listCreditCard = new List<UserCreditCardModel>();
            string monthOption = "";
            string savechange = "";
            string business = "";
            string cancel = "";
            string remove = "";
            string yourcredit = "";
            string Expirydate = "";

            try
            {


                DataSet ds = new DataSet();
                DataTable dt1 = new DataTable();
                DataTable dt2 = new DataTable();
                ds = homeService.getusercredicards(userId, culture);
                string holdername = homeService.GetTextMessagesAsString(culture, "cradholder");
                if (ds != null)
                {

                    dt1 = ds.Tables[0];
                    dt2 = ds.Tables[1];
                    if (dt2.Rows.Count > 0)
                    {

                        foreach (DataRow dr in dt2.Rows)
                        {
                            monthOption = monthOption + "<option value='" + dr["id"].ToString() + "'>" + dr["Month"].ToString() + "</option>";
                            savechange = dr["savechage"].ToString();
                            business = dr["business"].ToString();
                            cancel = dr["Cancel"].ToString();
                            remove = dr["Remove"].ToString();
                            yourcredit = dr["yourcredit"].ToString();
                            Expirydate = dr["Expirydate"].ToString();

                        }

                    }
                    if (dt1.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt1.Rows)
                        {
                            UserCreditCardModel obj = new UserCreditCardModel();
                            obj.TypeMonth = monthOption;
                            obj.ID = dr["Id"].ToString();
                            obj.CreditCardProviderID = dr["CreditCardProviderID"].ToString();

                            obj.ExpiryDate = homeService.Decrypt(dr["ExpiryDate"].ToString());
                            obj.NameOnCreditCard = homeService.Decrypt(dr["NameOnCreditCard"].ToString());
                            string creditNumber = homeService.Decrypt(dr["CreditCardNumber"].ToString());
                            obj.CreditCardNumber = creditNumber.Substring(creditNumber.Length - 4, 4);
                            string month = homeService.Decrypt(dr["ExpiryDate"].ToString());
                            string[] month1 = month.Split('/');
                            obj.Month = month1[0];
                            obj.Year = month1[1];
                            obj.Savechange = savechange;
                            obj.Business = business;
                            obj.Cancel = cancel;
                            obj.Remove = remove;
                            obj.Expirydates = Expirydate;
                            obj.Yourcredit = yourcredit;
                            obj.cardholder = holdername;
                            listCreditCard.Add(obj);
                        }
                    }

                }
                return Request.CreateResponse(HttpStatusCode.OK, listCreditCard);
            }
            catch (Exception ex)
            {
                return LogError(ex);
            }
        }


        [HttpPost]
        public async Task<HttpResponseMessage> UpdateProfilePicture(string userId)
        {
           

            int i = 0;
            string ContentfileName = "";
            string filenames = "";

            try
            {
                Home inserthome = new Home();
                string path = "";
                if (!Request.Content.IsMimeMultipartContent())
                {
                    this.Request.CreateResponse(HttpStatusCode.UnsupportedMediaType);
                }

                var provider = GetMultipartProvider();
                var result = await Request.Content.ReadAsMultipartAsync(provider);

                // On upload, files are given a generic name like "BodyPart_26d6abe1-3ae1-416a-9429-b35f15e6e5d5"
                // so this is how you can get the original file name
                var originalFileName = GetDeserializedFileName(result.FileData.First());
                
                // uploadedFileInfo object will give you some additional stuff like file length,
                // creation time, directory name, a few filesystem methods etc..
                var uploadedFileInfo = new FileInfo(result.FileData.First().LocalFileName);
                var file = Path.GetFileName(result.FileData.First().Headers.ContentDisposition.FileName.Replace("'\'",string.Empty).Replace("\"",""));
                string date = DateTime.Now.ToString("yyyyMMddHHmmss");

                if (uploadedFileInfo != null)
                {
                    ContentfileName = string.Format("{0}_{1}",date,file);
                    //path = Path.Combine(HttpContext.Current.Server.MapPath("~/Images/Users/"), ContentfileName);
                    path = Path.Combine( Convert.ToString(ConfigurationManager.AppSettings["UserPhotoPath"]), ContentfileName);

                    uploadedFileInfo.CopyTo(path);
                    filenames = Convert.ToString(ContentfileName);
                }

                i = inserthome.Updateprofile1(userId, filenames);
                return Request.CreateResponse(HttpStatusCode.OK, filenames);
            }
            catch (Exception ex)
            {
                return LogError(ex);
            }
        }
        public class UploadDataModel
        {
            public string userId { get; set; }
        }
        private MultipartFormDataStreamProvider GetMultipartProvider()
        {
            // IMPORTANT: replace "(tilde)" with the real tilde character
            // (our editor doesn't allow it, so I just wrote "(tilde)" instead)
            var uploadFolder = "(tilde)/App_Data/Tmp/FileUploads"; // you could put this to web.config

            // var uploadFolder = ConfigurationManager.AppSettings["UserPhotoTmp"].ToString();
            var root = HttpContext.Current.Server.MapPath(uploadFolder);
           // if(!Directory.Exists(root))
            Directory.CreateDirectory(root);
            return new MultipartFormDataStreamProvider(root);
        }
        private T GetFormData<T>(MultipartFormDataStreamProvider result)
        {
            if (result.FormData.HasKeys())
            {
                var unescapedFormData = Uri.UnescapeDataString(result.FormData
                    .GetValues(0).FirstOrDefault() ?? String.Empty);
                if (!String.IsNullOrEmpty(unescapedFormData))
                    return JsonConvert.DeserializeObject<T>(unescapedFormData);
            }

            var o = new object();
            return (T) o;
        }
        private string GetDeserializedFileName(MultipartFileData fileData)
        {
            var fileName = GetFileName(fileData);
            return JsonConvert.DeserializeObject(fileName).ToString();
        }

        public string GetFileName(MultipartFileData fileData)
        {
            return fileData.Headers.ContentDisposition.FileName;
        }
    }

}
