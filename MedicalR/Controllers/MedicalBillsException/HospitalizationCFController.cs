using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MedicalR.CustomHelper;
using MedicalR.DataAccessLayer.DAL.MedicalBillsException;
using MedicalR.Models;
using MedicalR.Models.MedicalBillsException;
using MedicalR.Models.MedicalR;

namespace MedicalR.Controllers.MedicalBillsException
{
    public class HospitalizationCFController : Controller
    {
        DALHospitalizationFC bll = new DALHospitalizationFC();
        //CommonController bll = new CommonController();
        Common com = new Common();

        // GET: HospitalizationCF
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
            var res = bll.GetItem(id);
            return Json(res , JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetHospitalCFGridData()
        {
            return Json(bll.GetHospitalGridData());
        }
        public ActionResult GetDetailsByEmpl_Code(string empl_code)
        {
            return Json(bll.GetEmployeeDetailsByEmpl_Code(empl_code));
        }
        public ActionResult SendLetterToEmpl_N_Hospital(HospitalizationFCModel model)
        {
            return Json(bll.SendLetterToEmpl_N_Hospital_PRE(model));
        }
        public JsonResult Save(HospitalizationFCModel model)
        {
            return Json(bll.Save(model));
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