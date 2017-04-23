using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GBSHotels.API.Controllers
{

    [RoutePrefix("api/Auth")]
    public class AuthController : ApiController
    {
        public HttpResponseMessage Google()
        {
            try {return Request.CreateResponse(HttpStatusCode.Accepted, "");
            }
            catch (Exception ex)
            {
                return new BaseApiController().LogError(ex);
            }
        }
    }
}
