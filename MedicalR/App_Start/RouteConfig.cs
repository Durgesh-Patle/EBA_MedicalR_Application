using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MedicalR
{
    public class RouteConfig
    {
        //    public static void RegisterRoutes(RouteCollection routes)
        //    {
        //        routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
        //        routes.MapMvcAttributeRoutes();

        //        //routes.MapRoute(
        //        //    name: "Default",
        //        //    url: "{controller}/{action}/{id}",
        //        //    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
        //        //);

        //        routes.Add(
        //            new Route("{controller}/{action}/{id}",
        //            new RouteValueDictionary(
        //            new { controller = "Home", action = "Index", id = "" }),
        //            new MvcApplication.HyphenatedRouteHandler())
        //        );
        //    }


        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapMvcAttributeRoutes(); 

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }

    }
}
