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
using MedicalR.Models.UserManagement;

namespace MedicalR.Controllers.CHCAdmin
{
    [CustomAuthorize]
    public class CHCBillProcessController : Controller
    {
        IDALCHCApproveRequestform objDALAdmin = new DALCHCApproveRequestform();
        IDALUserManagement objDALUserManagement = new DALUserManagement();
        // GET: CHCBillProcess
        public ActionResult Index()
        {
            var result = objDALAdmin.CHCBillProcess();
            return View(result);
        }
        [CustomAuthorize]
        [Route("CHCBillProcess/OfficeNote/{lotno}")]
        public ActionResult OfficeNote(string lotno)
        {
            var result = objDALAdmin.OfficeNoteData(lotno);
            return View(result);
        }
        [CustomAuthorize]
        [Route("CHCBillProcess/BankAdvise/{lotno}")]
        public ActionResult BankAdvise(string lotno)
        {
            var result = objDALAdmin.BankAdviseData(lotno);
            return View(result);
        }
        public ActionResult LetterForBank()
        {

            return View();
        }
        #region Bill Process Detail page

        [CustomAuthorize]
        [Route("CHCBillProcess/detail/{emplid}")]
        [Route("CHCBillProcess/{emplid}/detail")]
        public ActionResult Detail(Int32 emplid = 0)
        {
            //emplid = UserManager.User.UserID;
            ViewBag.Client_ID = emplid;
            ViewBag.EmpCode = emplid;

            return View();
        }
        #region Bill Process Detail sidebar information
        public ActionResult _Empsidebarinfo()
        {
            SideBarinfoModel obj = new SideBarinfoModel();

            var result = objDALUserManagement.GetSidebarInfo(obj);

            return PartialView("_Empsidebarinfo", result);
        }
        #endregion
        #region Bill Process info tab
        public ActionResult _ProcessPayment(CHCBillProcess Objmodel)
        {

            //ViewBag.HospitalName = objDALCHC.GetHospitallist(obj1);
            CHCBillProcess result = new CHCBillProcess();
            if (Objmodel.id > 0)
            {
                result = objDALAdmin.GetsingleEmplBill(Objmodel);
            }


            return PartialView("_ProcessPayment", result);
        }
        [HttpPost]
        public JsonResult AddBillProcess(CHCBillProcess model)
        {

            var result = objDALAdmin.AddBillProcess(model);
            return Json(result);
        }
        [HttpPost]
        public JsonResult HoldBillProcess(CHCBillProcess model)
        {

            var result = objDALAdmin.HoldBillProcess(model);
            return Json(result);
        }
        #endregion
        #endregion
        public ActionResult QueryModel()
        {
            var result = objDALAdmin.GetLoTNo();
            return View(result);
        }
        [HttpPost]
        public JsonResult GetdatabyLotNo(CHCBillProcess objmodel)
        {
            // var Empcd = objmodel.Empcd;
            var result = objDALAdmin.GetdatabyLotNo(objmodel);
            return Json(result);
        }
        public ActionResult Officenotepdf(ExportParaModel epmodel)
        {
            string lotno = epmodel.lotno;
            byte[] bytes = objDALAdmin.GetCHCOfficeNotepdf(lotno);
            return File(bytes, "application/pdf", "OfficeNote" + DateTime.Now.ToString("dd_MMM_yyyy") + ".pdf");
        }
        public ActionResult Bankadvicepdf(ExportParaModel epmodel)
        {
            string lotno = epmodel.lotno;
            byte[] bytes = objDALAdmin.GetCHCBankadvicepdf(lotno);
            return File(bytes, "application/pdf", "BankAdvice" + DateTime.Now.ToString("dd_MMM_yyyy") + ".pdf");
        }
    }
}