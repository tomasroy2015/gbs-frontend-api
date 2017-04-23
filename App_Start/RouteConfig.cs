using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace GBSHotels.API
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            
            //routes.MapRoute(
            //    name: "Default_root",
            //    url: "Help/{action}/{apiId}",
            //    defaults: new { controller = "Help", action = "Index", id = UrlParameter.Optional }
            //);
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

//            routes.MapRoute(
//    name: "Default",
//    url: "{controller}/{action}/{id}",
//    defaults: new { controller = "Browser", action = "Index", id = UrlParameter.Optional }
//);

        }
    }
}
