using DocumentFormat.OpenXml.Spreadsheet;
using MedicalR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.SessionState;
using System.Web.WebPages;
using MedicalR.CustomHelper;
using System.Runtime.InteropServices.ComTypes;
using MedicalR.DataAccessLayer.DAL.UserManagement;
using MedicalR.DataAccessLayer.IDAL.UserManagement;
//using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;
using Org.BouncyCastle.Asn1.Ocsp;

namespace MedicalR.CustomHelper
{
    public class CustomAuthorizeAttribute : FilterAttribute, IAuthorizationFilter
    {
        IDALUserManagement objDALUserManagement = new DALUserManagement();

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            //added new
            var session = filterContext.HttpContext.Session;

            // 🔐 FIX 3: Validate Session Existence
            if (session == null || session["UserDetails"] == null)
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary {
                { "action", "Index" },
                { "controller", "Home" }
                    });
                return;
            }

            // 🔐 FIX 4: Session Hijacking Protection
            if (session["UserIP"] == null || session["UserAgent"] == null ||
                !session["UserIP"].Equals(filterContext.HttpContext.Request.UserHostAddress) ||
                !session["UserAgent"].Equals(filterContext.HttpContext.Request.UserAgent))
            {
                session.Abandon();
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary {
                { "action", "Index" },
                { "controller", "Home" }
                    });
                return;
            }
            //added new

            DateTime var_last_session = (DateTime)SessionManager.GetValue("last_activity_time");
            TimeSpan timeDifference = DateTime.Now - var_last_session;
            int minutesDifference = (int)timeDifference.TotalMinutes;
            if (minutesDifference > 1)
            {
                objDALUserManagement.UpdateUserSession(UserManager.User.Employeecode, true, false);
            }

            SessionManager.SetValue("last_activity_time", DateTime.Now);
            string returnURL = filterContext.HttpContext.Request.RawUrl;
            var request = filterContext.HttpContext.Request;
            string pageUrl = $"{request.Url.Scheme}://{request.Url.Authority}://{request.Url.PathAndQuery}";
            CommonHelper.write_log(pageUrl);
            var SessionId = filterContext.HttpContext.Session.SessionID;


            var user = UserManager.User;
            if (user == null)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary
                {
                    { "action", "Index" },
                    { "controller", "Home" },
                    { "returnUrl", returnURL}
                });
            }
            else
            {
                bool is_url_allowed = true;
                var user_menus = UserManager.UserMenus;
                if (user_menus.Count > 0 && user.is_full_access == false)
                {
                    List<string> lst_menus = new List<string>();
                    foreach (var menu in user_menus)
                    {
                        lst_menus.AddRange(menu.child_menu_list.Select(obj => obj.link.ToLower().Split('/')[1]).ToList().Distinct());
                    }
                    List<string> lst_menus2 = new List<string> {
                          "/SanctionMedicalBills/GetEmployeePastDetails",
                          "/SanctionMedicalBills/GetItem"
                    };

                    if (lst_menus != null && returnURL.ToLower().Contains("/dashboard") == false)
                    {
                        is_url_allowed = lst_menus.Contains(returnURL.Split('/')[1].ToLower());
                        if (is_url_allowed == false)
                        {
                            is_url_allowed = lst_menus.Contains(returnURL);
                        }
                        if (is_url_allowed == false)
                        {
                            is_url_allowed = lst_menus2.Contains(returnURL);
                        }
                    }

                    List<DDLMODEL> lst = new List<DDLMODEL>();
                    if ((returnURL.ToLower().Contains("/download") || returnURL.ToLower().Contains("/edit") || returnURL.ToLower().Contains("/applicationitem")) && is_url_allowed == true)
                    {
                        List<string> lst_ = returnURL.Split('/').ToList();
                        var var_id = lst_[lst_.Count - 1];
                        lst = UserManager.lst_table_ids;
                        if ((lst.Any(person => person.id == int.Parse(var_id)) == false))
                        {
                            is_url_allowed = false;
                        }
                    }
                }

                if (((returnURL.ToLower().Contains("/settings") && !returnURL.ToLower().Contains("/settings/profile") && !returnURL.ToLower().Contains("/settings/change-password") && !returnURL.ToLower().Contains("/settings/contact-us") && !returnURL.ToLower().Contains("/email-configuaration/" + user.UserID + "/edit")) && user.RoleID != 0 && user.RoleRightDetails.Settings.FullAccess == false) || is_url_allowed == false)
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary
                        {
                            { "action", "Index" },
                            { "controller", "Access-Denied" }
                        });
                }
            }
        }

    }
    public class CustomActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
            if (filterContext.Result is ViewResult)
            {
                var actionName = filterContext.ActionDescriptor.ActionName;
                var controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
                var pageUrl = $"{controllerName}/{actionName}";
                if (UserManager.User != null)
                {
                    HelperLogs.LogUserActivity(UserManager.User.Employeecode, "Page Visit", pageUrl);
                }
            }
        }
    }
}