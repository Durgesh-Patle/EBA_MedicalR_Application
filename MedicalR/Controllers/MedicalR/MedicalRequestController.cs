using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MedicalR.CustomHelper;
using MedicalR.DataAccessLayer.DAL.MedicalR;
using MedicalR.DataAccessLayer.IDAL.MedicalR;
using MedicalR.Models.MedicalR;
using MedicalR.Models;

namespace MedicalR.Controllers.MedicalR
{
    public class MedicalRequestController : Controller
    {
        IDALSanctionMedicalBills ObjSanction = new DALSanctionMedicalBills();
        // GET: MedicalRequest
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Item()
        {
            return View();
        }

        public ActionResult GetClaimRequestGridData()
        {
            Session["lst_ids"] = Helper.GetEmployeeWise_table_ids();
            MedicalRequest bll = new MedicalRequest();
            return new JsonNetResult(bll.GetClaimRequestGridData());
        }

        public ActionResult GetClaimRequestPastDetailsGridData()
        {
            MedicalRequest bll = new MedicalRequest();
            return new JsonNetResult(bll.GetClaimRequestPastDetailsGridData());
        }

        public ActionResult _NewRequest()
        {
            return PartialView("_NewRequest");
        }

        public ActionResult ApplicationHistory()
        {
            return View();
        }

        public ActionResult ApplicationItem()
        {
            return View();
        }

        public ActionResult _PastDetails()
        {
            return PartialView("_PastDetails");
        }

        public ActionResult GetItem(int id)
        {
            MedicalRequest bll = new MedicalRequest();
            return new JsonNetResult(bll.GetItem(id));
        }

        public ActionResult Save(MedicalRequestModel model)
        {
            //UserModel modelObj = Helper.Deserialize<UserModel>(model);
            MedicalRequest bll = new MedicalRequest();
            return new JsonNetResult(bll.Save(model));
        }

        public FileResult download(int id)
        {
            CommonHelper.write_log("MedicalRequest/download method hit!! id = " + id);
            MedicalRequest bll = new MedicalRequest();
            string temp = bll.PreparePrintDoc(id);
            byte[] bytes = CommonHelper.Convert2(temp.ToString());
            string Empcode = UserManager.User.Employeecode;
            return File(bytes, "application/pdf", Empcode + "_" + DateTime.Now.ToString("dd_MMM_yyyy") + ".pdf");

        }
    }
}