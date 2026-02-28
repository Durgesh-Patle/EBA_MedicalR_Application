using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MedicalR.CustomHelper;
using MedicalR.DataAccessLayer.DAL.CHC;
using MedicalR.DataAccessLayer.DAL.CHCAdmin;
using MedicalR.DataAccessLayer.DAL.UserManagement;
using MedicalR.DataAccessLayer.IDAL.CHC;
using MedicalR.DataAccessLayer.IDAL.CHCAdmin;
using MedicalR.DataAccessLayer.IDAL.UserManagement;
using MedicalR.Models.CHC;
using MedicalR.Models.UserManagement;

namespace MedicalR.Controllers.CHCAdmin
{
    [CustomAuthorize]
    public class CHCApproveRequestformController : Controller
    {
        IDALUserManagement objDALUserManagement = new DALUserManagement();
        IDALChcRequest objDALCHC = new DALChcRequest();
        IDALCHCApproveRequestform objDALAdmin = new DALCHCApproveRequestform();
        // GET: CHCApproveRequestform
        public ActionResult Index()
        {
            var result = objDALAdmin.chcrequest_data();
            return View(result);

        }
        public ActionResult _PastDetail(CHCRequest Objmodel)
        {
            // LoginViewModel result = new LoginViewModel();

            var result = objDALAdmin.Pastdetail(Objmodel);

            return PartialView("_PastDetail", result);
        }
        public ActionResult _Notes()
        {

            return PartialView("_Notes");
        }
        public ActionResult Attachments()
        {

            return PartialView("Attachments");
        }

        #region Request form Detail page

        [CustomAuthorize]
        [Route("CHCApproveRequestform/detail/{emplid}")]
        [Route("CHCApproveRequestform/{emplid}/detail")]
        public ActionResult Detail(Int32 emplid = 0)
        {
            //emplid = UserManager.User.UserID;
            ViewBag.Client_ID = emplid;
            ViewBag.EmpCode = emplid;

            return View();
        }
        #region Request form Detail sidebar information
        public ActionResult _Empsidebarinfo()
        {
            SideBarinfoModel obj = new SideBarinfoModel();

            var result = objDALUserManagement.GetSidebarInfo(obj);

            return PartialView("_Empsidebarinfo", result);
        }
        #endregion

        #region Request Detail form info tab
        public ActionResult _ApproveRequest(CHCRequest Objmodel)
        {

            //ViewBag.HospitalName = objDALCHC.GetHospitallist(obj1);
            CHCRequest result = new CHCRequest();
            if (Objmodel.id > 0)
            {
                result = objDALAdmin.Getsingleempdetails(Objmodel);
            }


            return PartialView("_ApproveRequest", result);
        }
        [HttpPost]
        public JsonResult ApproveCHCRequest(CHCRequest model)
        {

            var result = objDALAdmin.ApproveCHCRequest(model);
            return Json(result);
        }
        [HttpPost]
        public JsonResult RejectCHCRequest(CHCRequest model)
        {

            var result = objDALAdmin.RejectCHCRequest(model);
            return Json(result);
        }
        #endregion
        #endregion
    }
}