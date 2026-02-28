using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace MedicalR
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        public class HyphenatedRouteHandler : MvcRouteHandler
        {
            protected override IHttpHandler GetHttpHandler(RequestContext requestContext)
            {
                requestContext.RouteData.Values["controller"] =
                   requestContext.RouteData.Values["controller"].ToString().Replace("-", "_");
                requestContext.RouteData.Values["action"] =
                   requestContext.RouteData.Values["action"].ToString().Replace("-", "_");
                return base.GetHttpHandler(requestContext);
            }
        }
    }
}
