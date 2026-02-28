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
    public class MedicalAcknowlledgeController : Controller
    {
        IDALMedicalRequest objDALMedicalR = new DALMedicalRequest();
        // GET: MedicalAcknowlledge
        public ActionResult Index()
        {
            //var result = objDALMedicalR.GetAcknowledgelist();
            return View();

        }
        public ActionResult _AckNewRequest()
        {
            return PartialView("_AckNewRequest");
        }
        public ActionResult Item()
        {
            return View();
        }

        public ActionResult GetItem(int id)
        {
            MedicalRequest bll = new MedicalRequest();
            return new JsonNetResult(bll.GetItem(id));
        }
        public ActionResult GetAcknowledgeGridData()
        {
            DALAcknowledge bll = new DALAcknowledge();
            return Json(bll.GetAcknowledgeGridData());
        }
        public ActionResult AcknowledgeStart(List<MedicalAcknowledgeModel> model)
        {
            DALAcknowledge bll = new DALAcknowledge();
            return new JsonNetResult(bll.AcknowledgeStart(model));
        }
        public ActionResult SingleAcknowledgeStart(MedicalAcknowledgeModel model)
        {
            List<MedicalAcknowledgeModel> model_list = new List<MedicalAcknowledgeModel>();
            model_list.Add(model);
            DALAcknowledge bll = new DALAcknowledge();
            return new JsonNetResult(bll.AcknowledgeStart(model_list));
        }
    }
}