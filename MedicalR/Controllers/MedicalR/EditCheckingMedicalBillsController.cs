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
    public class EditCheckingMedicalBillsController : Controller
    {
        // GET: EditCheckingMedicalBills
        IDALEditCheckingBills Objcheck = new DALEditCheckingBills();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetEditCheckingMedicalBillsGrid_Data()
        {
            return Json(Objcheck.EditChecking_grid_data(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Detail(int id)
        {
            MedicalRequestModel objectmodel = new MedicalRequestModel();
            return View();
        }

        public ActionResult GetItem(int id)
        {
            var res = Objcheck.GetItem(id);
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Save(MedicalRequestModel model)
        {
            var res = Objcheck.Save(model);
            return Json(res, JsonRequestBehavior.AllowGet);
        }


        public FileResult download(int id)
        {
            DALEditCheckingBills bll = new DALEditCheckingBills();
            string temp = bll.DownloadEditCheckingBills();
            byte[] bytes = CommonHelper.Convert2(temp.ToString());
            string Empcode = UserManager.User.Employeecode;
            return File(bytes, "application/pdf", "EditCheckingMedicalBills_" + DateTime.Now.ToString("dd_MMM_yyyy") + ".pdf");

        }
    }
}