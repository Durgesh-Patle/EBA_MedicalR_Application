using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MedicalR.CustomHelper;
using MedicalR.DataAccessLayer.DAL.MedicalBillsException;
using MedicalR.DataAccessLayer.DAL.UserManagement;
using MedicalR.DataAccessLayer.IDAL.MedicalBillsException;
using MedicalR.DataAccessLayer.IDAL.UserManagement;
using MedicalR.Models.MedicalBillsException;
using MedicalR.Models.MedicalR;
using MedicalR.Models.UserManagement;

namespace MedicalR.Controllers
{
    [CustomAuthorize]
    public class MedicalBillsExceptionController : Controller
    {
        IDALUserManagement objDALUserManagement = new DALUserManagement();
        IDALMedicalBillsException objDALException = new DALMedicalBillsException();
        // LoginDetailModel result = new LoginDetailModel();
        // GET: MedicalBillsException
        public ActionResult Index()
        {
            return View();

        }
        public ActionResult Item()
        {
            return View();
        }
        public ActionResult GetItem(int id)
        {
            return new JsonNetResult(objDALException.GetItem(id));
        }

        [HttpPost]
        public JsonResult AddionalPaymentGrid_data(string empl_code, string appln_no)
        {

            var res = objDALException.GetAdditionalSanc(empl_code, appln_no);
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Save(MedicalRequestModel model)
        {
            return Json(objDALException.Save(model));
        }

        #region Additional Sanction detail page

        [CustomAuthorize]
        [Route("MedicalBillsException/detail/{id}/")]
        [Route("MedicalBillsException/{emplid}/detail")]
        public ActionResult Detail(int id = 0)
        {
            ViewBag.Client_ID = id;
            // ViewBag.EmpCode = appln_no;
            AdditionalSanction model = new AdditionalSanction();
            model.id = id;
            var result = objDALException.Getsingleempdetails(model);
            return View();
        }
        #region Additional Sanction form detail sidebar information
        public ActionResult _Empsidebarinfo()
        {
            SideBarinfoModel obj = new SideBarinfoModel();

            var result = objDALUserManagement.GetSidebarInfo(obj);

            return PartialView("_Empsidebarinfo", result);
        }
        #endregion
        #region Additional Sanction info tab
        public ActionResult AdditionalPayment(AdditionalSanction objModel)
        {
            AdditionalSanction result = new AdditionalSanction();
            if (objModel.id > 0)
            {
                result = objDALException.Getsingleempdetails(objModel);
            }
            // var result = objDALException.Getsingleempdetails(result);

            return PartialView("AdditionalPayment", result);
        }
        [HttpPost]
        public JsonResult Approve(AdditionalSanction model)
        {

            var result = objDALException.Approve(model);
            return Json(result);
        }
        #endregion
        #endregion
        public ActionResult HospitalizationCrIndex()
        {
            var result = objDALException.GetHospitalizationCr();
            return View(result);

        }
        #region HospitalizationCr detail page

        [CustomAuthorize]
        [Route("MedicalBillsException/HospitalizationCrDetail/{emplid}/")]
        [Route("MedicalBillsException/{emplid}/HospitalizationCrDetail")]
        public ActionResult HospitalizationCrDetail(Int32 emplid = 0)
        {
            ViewBag.Client_ID = emplid;
            // ViewBag.EmpCode = emplid;

            return View();
        }
        #region HospitalizationCr sidebar information
        public ActionResult HospitilizationCrSidebar()
        {
            SideBarinfoModel obj = new SideBarinfoModel();

            var result = objDALUserManagement.GetSidebarInfo(obj);
            // LoginDetailModel result = new LoginDetailModel();
            return PartialView("_Empsidebarinfo", result);
        }
        #endregion
        #region HospitalizationCr info tab
        public ActionResult HospCrRecoveryfrmEmpl(HospitilizationCr objModel)
        {
            HospitilizationCr result = new HospitilizationCr();
            if (objModel.id > 0)
            {
                result = objDALException.GetsingleHospitalCr(objModel);
            }


            return PartialView("HospCrRecoveryfrmEmpl", result);
        }
        [HttpPost]
        public JsonResult HospitalCrApprove(HospitilizationCr model)
        {

            var result = objDALException.HospitalCrApprove(model);
            return Json(result);
        }

        #endregion
        #endregion
        public ActionResult HospitalizationDHRDIndex()
        {
            var result = objDALException.GetHospitalizationDHRD();
            return View(result);

        }
        #region HospitalizationDHRD detail page

        [CustomAuthorize]
        [Route("MedicalBillsException/HospitalizationDHRDDetail/{emplid}/")]
        [Route("MedicalBillsException/{emplid}/HospitalizationDHRDDetail")]
        public ActionResult HospitalizationDHRDDetail(Int32 emplid = 0)
        {
            ViewBag.Client_ID = emplid;
            // ViewBag.EmpCode = emplid;

            return View();
        }
        #region HospitalizationDHRD detail sidebar information
        public ActionResult HospitilizationDHRDSidebar()
        {
            SideBarinfoModel obj = new SideBarinfoModel();

            var result = objDALUserManagement.GetSidebarInfo(obj);


            return PartialView("HospitilizationDHRDSidebar", result);
        }
        #endregion
        #region HospitalizationDHRD info tab
        public ActionResult _HospitalizationDHRDAdd(HospitilizationDHRD objModel)
        {
            HospitilizationDHRD result = new HospitilizationDHRD();
            if (objModel.id > 0)
            {
                result = objDALException.GetsingleHospitalDHRD(objModel);
            }


            return PartialView("_HospitalizationDHRDAdd", result);
        }
        [HttpPost]
        public JsonResult HospitalDHRDSave(HospitilizationDHRD model)
        {

            var result = objDALException.HospitalDHRDSave(model);
            return Json(result);
        }

        #endregion
        #endregion
    }
}