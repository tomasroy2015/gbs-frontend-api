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
    public class FAQController : BaseApiController
    {
        private Home homeService;
        public FAQController()
        {
            homeService = new Home();
        }

        public HttpResponseMessage GetFAQ(string culture)
        {
            DataTable dataTableFAQ = new DataTable();
            List<FAQModel> listFAQ = new List<FAQModel>();
            try
            {
                dataTableFAQ = homeService.GetFAQ(culture);
                if (dataTableFAQ != null && dataTableFAQ.Rows.Count > 0)
                {
                    foreach (DataRow dr in dataTableFAQ.Rows)
                    {
                        FAQModel obj = new FAQModel();
                        obj.Questions = Convert.ToString(dr["Questions"]);
                        obj.Answers = Convert.ToString(dr["Answers"]);
                        obj.Id = Convert.ToInt32(dr["ID"]);
                        listFAQ.Add(obj);
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, listFAQ);
                }
                else
                    return Request.CreateResponse(HttpStatusCode.NotFound, "No Record(s) found.");

            }
            catch (Exception ex)
            {
                return LogError(ex);
            }
        }
    }
}
