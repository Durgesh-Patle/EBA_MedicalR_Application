using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MedicalR.CustomHelper;

namespace MedicalR.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Dashboard

        [CustomAuthorize]
        public ActionResult Index()
        {
            return View();
        }
    }
}