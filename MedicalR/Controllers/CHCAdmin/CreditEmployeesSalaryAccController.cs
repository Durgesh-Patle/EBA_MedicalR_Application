using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MedicalR.CustomHelper;
using MedicalR.DataAccessLayer.DAL.CHCAdmin;
using MedicalR.DataAccessLayer.DAL.UserManagement;
using MedicalR.DataAccessLayer.IDAL.CHCAdmin;
using MedicalR.DataAccessLayer.IDAL.UserManagement;
using MedicalR.Models.CHC;
using MedicalR.Models.MedicalR;
using MedicalR.Models.UserManagement;

namespace MedicalR.Controllers.CHCAdmin
{
    [CustomAuthorize]
    public class CreditEmployeesSalaryAccController : Controller
    {
        IDALCHCApproveRequestform objDALAdmin = new DALCHCApproveRequestform();
        IDALUserManagement objDALUserManagement = new DALUserManagement();
        // GET: CreditEmployeesSalaryAcc

        public ActionResult Index()
        {
            // var result = objDALAdmin.CremplsalAcc();
            return View();
        }
        public ActionResult Get_CHC_CRemptoacc_data()
        {
            var res = objDALAdmin.CremplsalAcc();
            return  Json(res, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Save(List<CHCBillProcess> model, DateTime CR_DATE)
        {
            return new JsonNetResult(objDALAdmin.AddCreaditTosalAcc(model, CR_DATE).Message);
        }

        public ActionResult Saveforlater(List<CHCBillProcess> model, DateTime CR_DATE)
        {
            return new JsonNetResult(objDALAdmin.SaveforLaterAddCreaditTosalAcc(model, CR_DATE).Message);
        }
        public ActionResult BankAdvise()
        {

            return View();
        }
        #region Cr empl Sal Acc info Detail page

        [CustomAuthorize]
        [Route("CreditEmployeesSalaryAcc/detail/{emplid}")]
        [Route("CreditEmployeesSalaryAcc/{emplid}/detail")]
        public ActionResult Detail(Int32 emplid = 0)
        {
            //emplid = UserManager.User.UserID;
            ViewBag.Client_ID = emplid;
            ViewBag.EmpCode = emplid;

            return View();
        }
        #region Cr empl Sal Acc info Detail sidebar information
        public ActionResult _Empsidebarinfo()
        {
            SideBarinfoModel obj = new SideBarinfoModel();

            var result = objDALUserManagement.GetSidebarInfo(obj);

            return PartialView("_Empsidebarinfo", result);
        }
        #endregion
        #region Cr empl Sal Acc info tab
        public ActionResult _ProcessPayment(CHCBillProcess Objmodel)
        {

            //ViewBag.HospitalName = objDALCHC.GetHospitallist(obj1);
            CHCBillProcess result = new CHCBillProcess();
            if (Objmodel.id > 0)
            {
                result = objDALAdmin.GetsingleEmplCr(Objmodel);
            }


            return PartialView("_ProcessPayment", result);
        }
        [HttpPost]
        public JsonResult CrToEmplAcc(CHCBillProcess model)
        {

            var result = objDALAdmin.CrToEmplAcc(model);
            return Json(result);
        }
        public ActionResult EnclosureEmplSummary()
        {
            var result = objDALAdmin.EnclosureEmplSummary();
            return View(result);
        }
        //[HttpPost]
        //public JsonResult HoldBillProcess(CHCBillProcess model)
        //{

        //    var result = objDALAdmin.HoldBillProcess(model);
        //    return Json(result);
        //}
        #endregion
        #endregion
    }
}