using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MedicalR.CustomHelper;
using MedicalR.DataAccessLayer.DAL.MedicalBillsException;
using MedicalR.Models.MedicalBillsException;
using MedicalR.Models.MedicalR;

namespace MedicalR.Controllers.MedicalBillsException
{
    [CustomAuthorize]
    public class HospitalizationDHRDController : Controller
    {
        // GET: HospitalizationDHRD

        DALHospitalizationDHRD bll = new DALHospitalizationDHRD();
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
            return Json(bll.GetItem(id));
        }
        public ActionResult GetHospitalizationDHRDGridData()
        {
            return new JsonNetResult(bll.GetHospitalGridData());
        }
        public ActionResult GetDetailsByEmpl_Code(string empl_code)
        {
            return new JsonNetResult(bll.GetEmployeeDetailsByEmpl_Code(empl_code));
        }

        public JsonResult Save(HospitilizationDHRD model)
        {
            return Json(bll.HospitalDHRDSave(model));
        }
        public FileResult DownloadOfficeNotes(int id)
        {
            string temp = bll.PreparePrintDocFor_ExOfficeNote(id);
            byte[] bytes = CommonHelper.Convert2(temp.ToString());
            return File(bytes, "application/pdf", "EX_OfficeNote_" + DateTime.Now.ToString("dd_MMM_yyyy") + ".pdf");

        }
        public FileResult DownloadBankAdvise(int id)
        {
            string temp = bll.PreparePrintDocFor_ExBankAdvice(id);
            byte[] bytes = CommonHelper.Convert2(temp.ToString());
            return File(bytes, "application/pdf", "EX_OfficeNote_" + DateTime.Now.ToString("dd_MMM_yyyy") + ".pdf");

        }
    }
}