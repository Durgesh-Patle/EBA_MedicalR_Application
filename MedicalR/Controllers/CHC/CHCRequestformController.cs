using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MedicalR.CustomHelper;
using MedicalR.DataAccessLayer.DAL.CHC;
using MedicalR.DataAccessLayer.DAL.UserManagement;
using MedicalR.DataAccessLayer.IDAL.CHC;
using MedicalR.DataAccessLayer.IDAL.UserManagement;
using MedicalR.Models.CHC;
using MedicalR.Models.MedicalR;
using MedicalR.Models.UserManagement;

namespace MedicalR.Controllers.CHC
{
    public class CHCRequestformController : Controller
    {
        IDALUserManagement objDALUserManagement = new DALUserManagement();
        IDALChcRequest objDALCHC = new DALChcRequest();
        Hospitallist obj1 = new Hospitallist();

        // GET: CHCRequestform
        public ActionResult Index()
        {
            var empcode = UserManager.User.Employeecode;
            var result = objDALCHC.chcrequest_data(empcode);
            return View(result);
        }

        public ActionResult _PastDetail(CHCRequest Objmodel)
        {
            // LoginViewModel result = new LoginViewModel();
            var result = objDALCHC.Pastdetail(Objmodel);
            return PartialView("_PastDetail", result);
        }

        public ActionResult _Notes()
        {
            return PartialView("_Notes");
        }

        public ActionResult Attachments()
        {
            // LoginViewModel result = new LoginViewModel();

            //var result = objDALCHC.Pastdetail(Objmodel);

            return PartialView("Attachments");
        }

        #region Request form detail page

        [CustomAuthorize]
        [Route("CHCRequestform/detail")]
        [Route("CHCRequestform/{emplid}/detail")]
        public ActionResult Detail(Int32 emplid = 0)
        {
            emplid = UserManager.User.UserID;
            var empcode = UserManager.User.Employeecode;
            ViewBag.Client_ID = emplid;
            ViewBag.EmpCode = emplid;
            ViewBag.Emplcode = empcode;

            //AdditionalSanction model = new AdditionalSanction();
            //model.emplid = emplid;
            //var result = objDALException.Getsingleempdetails(model);
            return View();
        }
        #region Request form detail sidebar information
        public ActionResult _Empsidebarinfo()
        {
            SideBarinfoModel obj = new SideBarinfoModel();

            var result = objDALUserManagement.GetSidebarInfo(obj);

            return PartialView("_Empsidebarinfo", result);
        }
        #endregion
        #region Request form info tab
        public ActionResult _NewRequest(CHCRequest Objmodel)
        {
            // Objmodel.emplid = UserManager.SideBarInfor.employeeid;
            var result = objDALCHC.GetEmplAge(Objmodel);
            ViewBag.HospitalName = objDALCHC.GetHospitallist(obj1);
            //AdditionalSanction result = new AdditionalSanction();
            //if (objModel.id > 0)
            //{
            //    result = objDALException.Getsingleempdetails(objModel);
            //}
            // var result = objDALException.Getsingleempdetails(result);

            return PartialView("_NewRequest", result);
        }
        [HttpPost]
        public JsonResult CheckPriviousClaim(CHCRequest objModel)
        {
            var result = objDALCHC.CheckPriviousClaim(objModel);
            return Json(result);
        }
        [HttpPost]
        public JsonResult AddCHCRequest(CHCRequest model)
        {

            var result = objDALCHC.AddCHCRequest(model);
            return Json(result);
        }
        [HttpPost]
        public JsonResult SaveForLater(CHCRequest model)
        {

            var result = objDALCHC.SaveForLater(model);
            return Json(result);
        }
        #endregion
        #endregion

        #region Request form Edit page

        // [CustomAuthorize]
        [Route("CHCRequestform/Edit/{emplid}")]
        [Route("CHCRequestform/{emplid}/Edit")]
        public ActionResult Edit(Int32 emplid = 0)
        {
            //emplid = UserManager.User.UserID;
            ViewBag.Client_ID = emplid;
            ViewBag.EmpCode = emplid;
            var empcode = UserManager.User.Employeecode;
            ViewBag.Employeecode = empcode;
            ViewBag.Employeeid = UserManager.User.UserID;
            return View();
        }
        #region Request form Edit sidebar information
        public ActionResult _Empsidebarinfo_Edit()
        {
            SideBarinfoModel obj = new SideBarinfoModel();

            var result = objDALUserManagement.GetSidebarInfo(obj);

            return PartialView("_Empsidebarinfo_Edit", result);
        }
        #endregion
        #region Request Edit form info tab
        public ActionResult _NewRequest_Edit(CHCRequest Objmodel)
        {
            //ViewBag.HospitalName = objDALCHC.GetHospitallist(obj1);
            CHCRequest result = new CHCRequest();
            ViewBag.HospitalName = objDALCHC.GetHospitallist(obj1);
            if (Objmodel.id > 0)
            {
                result = objDALCHC.Getsingleempdetails(Objmodel);
            }
            return PartialView("_NewRequest_Edit", result);
        }
        //public ActionResult _NewRequest_Edit(CHCRequest Objmodel)
        //{
        //    CHCRequest result = new CHCRequest();

        //    try
        //    {
        //        CommonHelper.write_log("_NewRequest_Edit() called with ID=" + Objmodel.id);

        //        ViewBag.HospitalName = objDALCHC.GetHospitallist(obj1);

        //        if (Objmodel.id > 0)
        //        {
        //            result = objDALCHC.Getsingleempdetails(Objmodel);
        //            CommonHelper.write_log("Getsingleempdetails returned: " +  (result == null ? "NULL" : $"date_of_checkup={result.date_of_checkup}, chc_centerid={result.chc_centerid}, hosname={result.hosname}"));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        CommonHelper.write_log("Error in _NewRequest_Edit: " + ex.ToString());
        //    }

        //    return PartialView("_NewRequest_Edit", result);
        //}

        [HttpPost]
        public JsonResult UpdateCHCRequest(CHCRequest model)
        {

            var result = objDALCHC.ApproveCHCRequest(model);
            return Json(result);
        }
        #endregion
        #endregion
    }
}