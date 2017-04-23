using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using System.Net.Http.Headers;

namespace GBSHotels.API
{
    public static class WebApiConfig
    {
        private static string GetAllowedOrigins()
        {
            //Make a call to the database to get allowed origins and convert to a comma separated string
            //return "http://www.example.com,http://localhost:59452,http://localhost:25495";
            return @"http://localhost:49376
                    ,http://localhost:49000
                    ,http://localhost:8099
                    ,https://localhost:44301
                    ,http://www.gdsbooking.com
                    ,http://gdsbooking.com
                    ,https://gdsbooking.com
                    ,https://www.gdsbooking.com
                    ,https://www.gbshotels.com
                    ,https://gbshotels.com
                    ,http://gbshotels.com
                    ,http://www.gbshotels.com";
        }

        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            //config.SuppressDefaultHostAuthentication();
            //config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));
            
            string origins = GetAllowedOrigins();
            var cors = new System.Web.Http.Cors.EnableCorsAttribute(origins, "*", "*");
            config.EnableCors(cors);

           // config.EnableCors();

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            var json = config.Formatters.JsonFormatter;
            json.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects;
            config.Formatters.Remove(config.Formatters.XmlFormatter);

        }
    }
}
