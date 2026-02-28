using System.Collections.Generic;
using System.Web.Mvc;
using MedicalR.DataAccessLayer.DAL.CHCAdmin;
using MedicalR.DataAccessLayer.IDAL.CHCAdmin;
using MedicalR.Models.CHC;

namespace MedicalR.Controllers.CHCAdmin
{
    public class GenrateLotController : Controller
    {
        IDALCHCApproveRequestform objDALAdmin = new DALCHCApproveRequestform();
        // GET: GenrateLot
        public ActionResult LotGenration()
        {
            //var result = objDALAdmin.LotGenration();
            return View();
        }

        public ActionResult Get_CHC_Lots_Grid_data()
        {
            var res = objDALAdmin.LotGenration();
            return Json(res);
        }

        [HttpPost]
        public ActionResult CHCLotGenerationStart(List<CHCBillProcess> model)
        {
            var res = objDALAdmin.CHCLotGenerationStart(model);
            return  Json(res);
        }
    }
}