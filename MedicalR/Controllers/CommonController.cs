using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MedicalR.CustomHelper;
using MedicalR.Models.MedicalR;
using MedicalR.Models;

namespace MedicalR.Controllers
{
    public class CommonController : Controller
    {
        // GET: Common
        public ActionResult GetTreatmentTypes()
        {
            Common bll = new Common();
            return new JsonNetResult(bll.GetTreatmentTypes());
        }
        public ActionResult GetBands()
        {
            Common bll = new Common();
            return new JsonNetResult(bll.GetBandNames());
        }

        public ActionResult LoadDropdown(string Category_Code)
        {
            Common bll = new Common();
            return new JsonNetResult(bll.LoadDropdown(Category_Code));
        }
        public ActionResult GetPendingBatches_ForEditChecking()
        {
            Common bll = new Common();
            return new JsonNetResult(bll.GetPendingBatches_ForEditChecking());
        }
        public ActionResult GetPendingBatches_ForSanctionMedical()
        {
            Common bll = new Common();
            return new JsonNetResult(bll.GetPendingBatches_ForSanctionMedicals());
        }
        public ActionResult GetQMBatches()
        {
            Common bll = new Common();
            return Json(bll.GetQMBatches(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetQMLotNos()
        {
            Common bll = new Common();
            return Json(bll.GetQMLotNos(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetDoctors()
        {
            Common bll = new Common();
            return Json(bll.GetDoctors(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetObjectionCodes()
        {
            Common bll = new Common();
            var res = bll.GetObjectionCodes();
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetPatients()
        {
            Common bll = new Common();
            return new JsonNetResult(bll.GetPatients(UserManager.User.UserID));
        }
        //public ActionResult GetHospExPatients(int employee_id)
        //{
        //    Common bll = new Common();
        //    return new JsonNetResult(bll.GetPatients(employee_id));
        //} 


        public ActionResult GetHospExPatients(int employee_id)
        {
            Common bll = new Common();
            var list = bll.GetPatients(employee_id);
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetCFHospitals()
        {
            Common bll = new Common();
            var hospitals = bll.GetCFHospitals();
            return Json(hospitals, JsonRequestBehavior.AllowGet);
        }


        //public ActionResult GetCFHospitals()
        //{
        //    Common bll = new Common();
        //    return new JsonNetResult(bll.GetCFHospitals());
        //}

        public ActionResult GetExpemseTypes(int treatment_type_id)
        {
            CommonHelper.write_log("Hit GetExpemseTypes() : treatment_type_id" + treatment_type_id);

            Common bll = new Common();
            //return new JsonNetResult(bll.GetExpenseTypes(treatment_type_id));
            return Json(bll.GetExpenseTypes(treatment_type_id), JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        //public ActionResult GetExpemseTypes(int treatment_type_id)
        //{
        //    try
        //    {
        //        // 1️⃣ Log method hit
        //        CommonHelper.write_log("Hit GetExpemseTypes() : treatment_type_id = " + treatment_type_id);

        //        if (treatment_type_id <= 0)
        //        {
        //            CommonHelper.write_log("Invalid treatment_type_id received.");
        //            return Json(new { Status = false, Message = "Invalid treatment type id" }, JsonRequestBehavior.AllowGet);
        //        }

        //        Common bll = new Common();

        //        var data = bll.GetExpenseTypes(treatment_type_id);

        //        if (data == null)
        //        {
        //            CommonHelper.write_log("GetExpenseTypes returned NULL.");
        //        }
        //        else
        //        {
        //            CommonHelper.write_log("GetExpenseTypes returned count = " + data.Count);
        //        }

        //        return Json(data, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        CommonHelper.write_log("ERROR in GetExpemseTypes(): " + ex.Message);
        //        CommonHelper.write_log("STACK TRACE: " + ex.StackTrace);

        //        return Json(new
        //        {
        //            Status = false,
        //            Message = "Server error occurred.",
        //            Error = ex.Message
        //        }, JsonRequestBehavior.AllowGet);
        //    }
        //}
        public ActionResult GetHeadOfExpenses(int expense_type_id)
        {
            Common bll = new Common();
            return new JsonNetResult(bll.GetHeadOfExpenses(expense_type_id));
        }
        public ActionResult GetHosptailList()
        {
            Common bll = new Common();
            return new JsonNetResult(bll.GetHospitalList());
        }
        public ActionResult GetEmployeeSideBarDetails()
        {
            Common bll = new Common();
            return Json(bll.GeEmployeeSideBarDetails());
        }
        public ActionResult GetUserMenus()
        {
            return new JsonNetResult(UserManager.UserMenus);
        }
    }

}