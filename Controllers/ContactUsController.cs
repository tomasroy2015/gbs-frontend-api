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
    public class ContactUsController : BaseApiController
    {

        private Home homeService;
        public ContactUsController()
        {
            homeService = new Home();
        }
        public HttpResponseMessage GetSubject(string culture)
        {
            List<SubjectModel> listSubject = new List<SubjectModel>();
            DataTable dataTableSubject = new DataTable();
            try
            {

                dataTableSubject = homeService.GetSubject(culture);
                if (dataTableSubject != null && dataTableSubject.Rows.Count > 0)
                {

                    foreach (DataRow dr in dataTableSubject.Rows)
                    {
                        SubjectModel subject = new SubjectModel();
                        subject.Id = Convert.ToInt32(dr["ID"]);
                        subject.Subject = Convert.ToString(dr["Name"]);
                        listSubject.Add(subject);
                    }

                    return Request.CreateResponse(HttpStatusCode.OK, listSubject);
                }
                else
                    return Request.CreateResponse(HttpStatusCode.NotFound, "No Record(s) found.");
            }
            catch (Exception ex)
            {
                return LogError(ex);
            }
        }

        public HttpResponseMessage InsertContactUs(ContactUsModel contactus)
        {
            int success = 0;

            try
            {
                success = homeService.InsertSubject(contactus.Name, contactus.Contact, contactus.Email, contactus.Subject, contactus.Description, contactus.IPAddress, Convert.ToString(contactus.UserId), contactus.Surname, Convert.ToString(contactus.CountryId), contactus.Satulation);
                if (success == 1)
                    return Request.CreateResponse(HttpStatusCode.OK, success);
                else
                    return Request.CreateResponse(HttpStatusCode.BadRequest, success);
            }
            catch (Exception ex)
            {
                return LogError(ex);
            }
        }
    }
}
