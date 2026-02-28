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
    public class TrackingMedicalBillsController : Controller
    {
        IDALTrackingMedicalBills ObjTracking = new DALTrackingMedicalBills();
        // GET: TrackingMedicalBills
        public ActionResult Index()
        {
            var result = ObjTracking.GetTrackingBills_GridData();
            return View(result);
        }
        [CustomAuthorize]
        [Route("TrackingMedicalBills/detail/{id}")]
        [Route("TrackingMedicalBills/{id}/id")]
        public ActionResult Detail(Int32 id = 0)
        {
            TrackingMedicalBillsModel objectmodel = new TrackingMedicalBillsModel();
            objectmodel.id = id;
            var result = ObjTracking.GetSingleMedicalBill(objectmodel);
            return View(result);
        }
    }
}