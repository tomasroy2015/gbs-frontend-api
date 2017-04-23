using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GBSHotels.API.App_Start
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class EnableCorsAttribute : FilterAttribute, IActionFilter
    {
        private const string IncomingOriginHeader = "Origin";
        private const string OutgoingOriginHeader = "Access-Control-Allow-Origin";
        private const string OutgoingMethodsHeader = "Access-Control-Allow-Methods";
        private const string OutgoingAgeHeader = "Access-Control-Max-Age";

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            // Do nothing
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var isLocal = filterContext.HttpContext.Request.IsLocal;
            var originHeader =
                 filterContext.HttpContext.Request.Headers.Get(IncomingOriginHeader);
            var response = filterContext.HttpContext.Response;

            if (!String.IsNullOrWhiteSpace(originHeader) &&
                (isLocal || IsAllowedOrigin(originHeader)))
            {
                response.AddHeader(OutgoingOriginHeader, originHeader);
                response.AddHeader(OutgoingMethodsHeader, "GET,POST,OPTIONS");
                response.AddHeader(OutgoingAgeHeader, "3600");
            }
        }

        protected bool IsAllowedOrigin(string origin)
        {
            // ** replace with your own logic to check the origin header
            return true;
        }
    }

}