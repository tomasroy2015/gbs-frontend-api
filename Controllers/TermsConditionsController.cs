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
    public class TermsConditionsController : BaseApiController
    {
        private Home homeService;
        public TermsConditionsController()
        {
            homeService = new Home();
        }
        public HttpResponseMessage GetTermsCondition(string culture)
        {
            DataTable dataTableTermsCondition = new DataTable();
            List<TearmConditionModel> listTearmCondition = new List<TearmConditionModel>();
            Home ObjModel = new Home();
            try
            {
                dataTableTermsCondition = homeService.GetTearmCondition(culture);

                if (dataTableTermsCondition != null && dataTableTermsCondition.Rows.Count > 0)
                {

                    foreach (DataRow dr in dataTableTermsCondition.Rows)
                    {
                        TearmConditionModel TearmCondition = new TearmConditionModel();
                        TearmCondition.Content = Convert.ToString(dr["Text"]).Replace("*", "<br/> <br/>*").Replace("</b>", "</b><br/> <br/>");
                        TearmCondition.Label = Convert.ToString(dr["termsconditions"]);
                        listTearmCondition.Add(TearmCondition);
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, listTearmCondition);
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
