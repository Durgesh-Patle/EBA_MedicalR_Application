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
    //[CustomAuthorize]
    public class ProcessPaymentController : Controller
    {
        // GET: ProcessPayment
        IDALProcessPayment ObjProcess = new DALProcessPayment();
        // GET: ProcessPayment
        public ActionResult Index()
        {
            var result = ObjProcess.ProcessPayment_grid_data();
            return View(result);
        }

        public ActionResult BankAdvise()
        {

            return View();
        }

        public ActionResult ProcessPayment_grid_data()
        {
            var res = ObjProcess.ProcessPayment_grid_data();
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EmployeeSummary()
        {
            var result = ObjProcess.EmployeeWiseSummary();
            return View(result);
        }

        //[HttpPost]
        //public ActionResult Save(List<ProcessPaymentModel> model, DateTime date_of_payment)
        //{
        //    var res = ObjProcess.Processpayment(model, date_of_payment);
        //    return Json(res);
        //}

        [HttpPost]
        public ActionResult LotDetailsSave(SavePaymentRequest model)
        {
            try
            {
                CommonHelper.write_log("Save Payment Method HIT at: " + DateTime.Now);

                DateTime paymentDate = Convert.ToDateTime(model.date_of_payment);

                CommonHelper.write_log("String Payment Date: " + model.date_of_payment);
                CommonHelper.write_log("Converting Date Payment Date: " + paymentDate);

                if (model != null)
                {
                    CommonHelper.write_log("Model Count Received: " + model);
                }
                else
                {
                    CommonHelper.write_log("Model is NULL");
                }

                var res = ObjProcess.Processpayment(model.modelPayment, paymentDate);

                // 4️⃣ Result Log
                CommonHelper.write_log("Processpayment method executed successfully.");
                CommonHelper.write_log("Result: " + (res != null ? res.ToString() : "NULL"));

                return Json(res);
            }
            catch (Exception ex)
            {
                // 5️⃣ Exception Log
                CommonHelper.write_log("ERROR in Save Method: " + ex.Message);
                CommonHelper.write_log("StackTrace: " + ex.StackTrace);

                return Json(new { success = false, message = "Something went wrong while saving payment." });
            }
        }

        [HttpPost]
        public JsonResult Processpayment(ProcessPaymentModel model)
        {

            var result = ObjProcess.Processpayment(model);
            return Json(result);
        }
    }
}