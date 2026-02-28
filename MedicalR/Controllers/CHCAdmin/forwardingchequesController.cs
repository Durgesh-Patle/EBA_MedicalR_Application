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
    public class forwardingchequesController : Controller
    {
        IDALCHCApproveRequestform objDALAdmin = new DALCHCApproveRequestform();
        IDALUserManagement objDALUserManagement = new DALUserManagement();
        // GET: forwardingcheques
        public ActionResult Index()
        {
            var result = objDALAdmin.forwardcheque();
            return View(result);
        }
        #region Forward Cheque Detail page

        [CustomAuthorize]
        [Route("forwardingcheques/detail/{emplid}")]
        [Route("forwardingcheques/{emplid}/detail")]
        public ActionResult Detail(Int32 emplid = 0)
        {
            //emplid = UserManager.User.UserID;
            ViewBag.Client_ID = emplid;
            ViewBag.EmpCode = emplid;

            return View();
        }
        #region Forward Cheque info Detail sidebar information
        public ActionResult _Empsidebarinfo()
        {
            SideBarinfoModel obj = new SideBarinfoModel();

            var result = objDALUserManagement.GetSidebarInfo(obj);

            return PartialView("_Empsidebarinfo", result);
        }
        #endregion
        #region Forward Cheque info tab
        public ActionResult _forwardingcheques(CHCBillProcess Objmodel)
        {

            //ViewBag.HospitalName = objDALCHC.GetHospitallist(obj1);
            CHCBillProcess result = new CHCBillProcess();
            if (Objmodel.id > 0)
            {
                result = objDALAdmin.GetSingleCHC(Objmodel);
            }


            return PartialView("_forwardingcheques", result);
        }
        [HttpPost]
        public JsonResult forwardcheque(CHCBillProcess model)
        {

            var result = objDALAdmin.forwardcheque(model);
            return Json(result);
        }

        #endregion
        #endregion
    }
}