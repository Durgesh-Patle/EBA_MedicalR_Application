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
    public class UpdateBatchController : Controller
    {
        // GET: UpdateBatch
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Get_UpdateBatchNo_Grid_data()
        {
            DALBatchCreation bll = new DALBatchCreation();
            var res = bll.Get_UpdateBatchNo_Grid_data();
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Update_BatchDetails(UpdateBatchModel model)
        {
            DALBatchCreation bll = new DALBatchCreation();
            return new JsonNetResult(bll.Update_BatchDetails(model));
        }

        [Route("UpdateBatch/download/{batch_no}")]
        public FileResult download(string batch_no)
        {
            DALBatchCreation bll = new DALBatchCreation();
            string temp = bll.PreparePrintDoc(batch_no);
            byte[] bytes = CommonHelper.Convert2(temp.ToString());
            string Empcode = UserManager.User.Employeecode;
            return File(bytes, "application/pdf", Empcode + "_" + DateTime.Now.ToString("dd_MMM_yyyy") + ".pdf");

        }
    }
}