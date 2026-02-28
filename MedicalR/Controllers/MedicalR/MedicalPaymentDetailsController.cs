using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MedicalR.DataAccessLayer.DAL.MedicalR;
using MedicalR.DataAccessLayer.IDAL.MedicalR;

namespace MedicalR.Controllers.MedicalR
{
    public class MedicalPaymentDetailsController : Controller
    {
        IDALMedicalPaymentDetails ObjPayDetail = new DALMedicalPaymentDetails();
        // GET: MedicalPaymentDetails
        public ActionResult Index()
        {
            var result = ObjPayDetail.GetPaymentDetail_GridData();
            return View(result);
        }
    }
}