using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MedicalR.CustomHelper;
using MedicalR.DataAccessLayer.DAL.RetiredEmployee;
using MedicalR.DataAccessLayer.IDAL.RetiredEmployee;
using MedicalR.Models.RetiredEmployee;

namespace MedicalR.Controllers.RetiredEmp
{

    [CustomAuthorize]
    public class RetiredEmplSpecialCaseController : Controller
    {
        IDALRetiredEmployee objDALReiredEmployee = new DALRetiredEmployee();

        // GET: RetiredEmplSpecialCase
        [CustomAuthorize]
        public ActionResult DataEntrySpecialCase()
        {
            var result = objDALReiredEmployee.GetTransRetiredEmp();
            return View(result);
        }

        [CustomAuthorize]
        [Route("RetiredEmplSpecialCase/DataEntrySpecialCase/{id}/add")]
        public ActionResult DataEntry()
        {
            // ViewBag.Yearlist = objDALReiredEmployee.Getyears();
            return View();
        }

        [CustomAuthorize]
        [Route("RetiredEmplSpecialCase/DataEntrySpecialCase/{id}/{Empcd}/edit")]
        public ActionResult DataEntryEdit(int id, string Empcd)
        {
            RetiredempTransModel model = new RetiredempTransModel();
            model.Id = id;
            model.Empcd = Empcd;
            ViewBag.Yearlist = objDALReiredEmployee.GetYearsforSpecialcaseEdit(Empcd);
            var result = objDALReiredEmployee.GetsingleRemptransdetails(model);
            return View(result);
        }

        [HttpPost]
        public JsonResult GetYears(string empcd)
        {
            var result = objDALReiredEmployee.GetYearsforSpecialcase(empcd);
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetName(string empcd)
        {
            var result = objDALReiredEmployee.GetName(empcd);
            return Json(result);
        }

        [HttpPost]
        public JsonResult RemptransNewAdd(RetiredempTransModel objModel)
        {
            var result = objDALReiredEmployee.RemptransNewAdd(objModel);
            return Json(result);
        }

        [HttpPost]
        public JsonResult UpdateRemptrans(RetiredempTransModel objModel)
        {
            var result = objDALReiredEmployee.UpdateRemptrans(objModel);
            return Json(result);
        }

        [HttpPost]
        public JsonResult RemptransStatus(RetiredempTransModel objModel)
        {
            var result = objDALReiredEmployee.RemptransStatus(objModel);
            return Json(result);
        }
    }
}