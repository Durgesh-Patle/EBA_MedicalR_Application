using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MedicalR.CustomHelper;
using MedicalR.DataAccessLayer.DAL.MedicalR;
using MedicalR.Models.MedicalR;

namespace MedicalR.Controllers.MedicalR
{
    [CustomAuthorize]
    public class BatchCreationController : Controller
    {
        // GET: BatchCreation
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult BatchItems()
        {
            return View();
        }
        public ActionResult GetBatchCreationGridData()
        {
            DALBatchCreation bll = new DALBatchCreation();
            return new JsonNetResult(bll.Get_BatchCreation_Grid_Data());
        }
        public ActionResult GetBatchItemsGridData(string batch_no)
        {
            DALBatchCreation bll = new DALBatchCreation();
            var res = bll.Get_BatchItems_Grid_Data(batch_no);
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public ActionResult BatchCreationStart(List<BatchCreationModel> model)
        {
            DALBatchCreation bll = new DALBatchCreation();
            return new JsonNetResult(bll.BatchCreationStart(model));
        }
    }
}