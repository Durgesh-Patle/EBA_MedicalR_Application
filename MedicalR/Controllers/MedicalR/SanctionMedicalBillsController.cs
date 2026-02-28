using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MedicalR.CustomHelper;
using MedicalR.DataAccessLayer.DAL.MedicalR;
using MedicalR.DataAccessLayer.IDAL.MedicalR;
using MedicalR.Models.MedicalR;

namespace MedicalR.Controllers.MedicalR
{
    [CustomAuthorize]   
    public class SanctionMedicalBillsController : Controller
    {
        // GET: SanctionMedicalBills
        IDALSanctionMedicalBills ObjSanction = new DALSanctionMedicalBills();
        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult GetSanctionMedicalBillsGrid_Data()
        {
            return new JsonNetResult(ObjSanction.SanctionBills_grid_data());
        }
        public ActionResult GetEmployeePastDetails()
        {
            var res = ObjSanction.EmployeePastHistory_grid_data();
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Detail(int id)
        {
            return View();
        }
        public ActionResult GetItem(int id)
        {
            var res = ObjSanction.GetItem(id);
            return  Json(res);
        }

        public JsonResult Save(MedicalRequestModel model, bool is_sanctioned)
        {
            var res = ObjSanction.Save(model, is_sanctioned);
            return Json(res , JsonRequestBehavior.AllowGet);
        }

        public ActionResult OfficeNote()
        {
            var result = ObjSanction.OfficeNoteData();
            return View(result);
        }
        public ActionResult Annexure()
        {
            var result = ObjSanction.AnnexureData();
            return View(result);
        }
        public FileResult download(int id)
        {
            DALSanctionMedicalBills bll = new DALSanctionMedicalBills();
            string temp = bll.PreparePrintDoc(id);
            byte[] bytes = CommonHelper.Convert2(temp.ToString());
            string Empcode = UserManager.User.Employeecode;
            return File(bytes, "application/pdf", Empcode + "_Objection_Letter_" + DateTime.Now.ToString("dd_MMM_yyyy") + ".pdf");

        }
        public FileResult download_sanction_med_bills(int id)
        {
            DALSanctionMedicalBills bll = new DALSanctionMedicalBills();
            string temp = bll.DownloadSanctionedMedicalBills();
            byte[] bytes = CommonHelper.Convert2(temp.ToString());
            string Empcode = UserManager.User.Employeecode;
            return File(bytes, "application/pdf", "SanctionedMedicalBills_" + DateTime.Now.ToString("dd_MMM_yyyy") + ".pdf");

        }
    }
}