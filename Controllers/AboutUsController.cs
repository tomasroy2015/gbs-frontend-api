using GBSHotels.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace GBSHotels.API.Controllers
{
    //[EnableCors(origins: "http://localhost:49376", headers: "*", methods: "*")]
    public class AboutUsController : BaseApiController
    {
        private Home homeService;
        public AboutUsController()
        {
            homeService = new Home();
        }
        public HttpResponseMessage GetAboutUs(string culture)
        {
            try
            {
                string content = homeService.GetTextMessagesAsString(culture, "AboutGbshotelsText").Replace("\n", "</br>");
                if (!string.IsNullOrEmpty(content))
                    return Request.CreateResponse(HttpStatusCode.OK, content);
                else
                    return Request.CreateResponse(HttpStatusCode.NotFound, "No record(s) found.");
            }
            catch (Exception ex)
            {
                return LogError(ex);
            }
        }
    }
}
