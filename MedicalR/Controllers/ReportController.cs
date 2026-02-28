using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClosedXML.Excel;
using MedicalR.CustomHelper;
using MedicalR.DataAccessLayer.DAL.Reports;
using MedicalR.DataAccessLayer.IDAL.Reports;
using MedicalR.Models;
using MedicalR.Models.MedicalR;
using MedicalR.Models.Reports;

namespace MedicalR.Controllers
{
    public class ReportController : Controller
    {
        IDALReports objDALReports = new DALReports();
        //IDALCo objDALReports = new DALReports();
        Common objCommonDAL = new Common();

        [CustomAuthorize]
        //public ActionResult Index()
        //{
        //    return RedirectToAction("Index", "Dashboard");
        //}

        public ActionResult Index()
        {
            ViewBag.Bands = objCommonDAL.GetBandNames();      
            ViewBag.TreatmentTypes = objCommonDAL.GetTreatmentTypes();
            ViewBag.Doctors = objCommonDAL.GetDoctors();    
            ViewBag.LotNos = objCommonDAL.GetQMLotNos();
            return View();
        }

        public ActionResult MedicalRBatchSummaryReport()
        {
            ViewBag.Doctors = objCommonDAL.GetDoctors();      
            ViewBag.Batches = objCommonDAL.GetQMBatches();   

            return View();
        }

        public ActionResult EmployeeWisePaymentReport()
        {
            return View();
        }

        public ActionResult SettlementReport()
        {
            ViewBag.Bands = objCommonDAL.GetBandNames();
            return View();
        }

        public ActionResult SettlementSummaryReport()
        {
            return View();
        }

        // Get Payment Record
        [HttpPost]
        public ActionResult GetPaymentRecordGrid(string band, DateTime? from_date, DateTime? to_date, int treatment_id, int doctor_id, string batch_no, string employeecode, string lot_no, string payment_status)
        {
            try
            {
                CommonHelper.write_log("GetPaymentRecordGrid method started");
                var res = objDALReports.GetPaymentRecordGrid(band, from_date, to_date, treatment_id, doctor_id, batch_no, employeecode, lot_no, payment_status);

                CommonHelper.write_log("GetPaymentRecordGrid executed successfully");
                return Json(res , JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                CommonHelper.write_log("ERROR in GetPaymentRecordGrid: " + ex.ToString());
                return Json(new { error = ex.Message });
            }
        }

        //Get Batch Summary Report
        public ActionResult GetBatchSummaryReport(int doctor_id, string batch_id, string payment_status)
        {
            return Json(objDALReports.GetBatchSummaryReport(doctor_id, batch_id, payment_status));
        }

        //Get Employee Wise Payment Report
        public ActionResult GetEmployeeWisePaymentReport(string employeecode, DateTime from_date, DateTime to_date)
        {
            return Json(objDALReports.GetEmployeeWisePaymentReport(employeecode, from_date, to_date));
        }

        //Get Settlement Report
        public ActionResult GetSettlementReport(string band, DateTime from_date, DateTime to_date)
        {
            return  Json(objDALReports.GetSettlementReport(band, from_date, to_date));
        }

        //Get Settlement Summary Report
        public ActionResult GetSettlementSummaryReport(string report_status)
        {
            return Json(objDALReports.GetSettlementSummaryReport(report_status));
        }

        //download Payment Record
        //[Route("Report/DownloadPaymentRecord/{from_date}/{to_date}/{band}/{treatment_id}/{doctor_id}/{batch_no}/{employeecode}/{lot_no}/{payment_status}")]
        //[Route("Report/DownloadPaymentRecord/{from_date:datetime?}/{to_date:datetime?}/{band?}/{treatment_id?}/{doctor_id?}/{batch_no?}/{employeecode?}/{lot_no?}/{payment_status?}")]
        public FileResult DownloadPaymentRecord(ReportDownloadParaModel para_model)
        {
            DataTable dt = objDALReports.DownloadPaymentRecord(
                para_model.from_date,
                para_model.to_date,
                para_model.band,
                para_model.treatment_type,
                para_model.doctor_id,
                para_model.batch_no,
                para_model.employeecode,
                para_model.lot_no,
                para_model.payment_status
            );

            CommonHelper.write_log($"Payment report > DownloadPaymentRecord count: {dt.Rows.Count}");

            string fileName = $"Payment_Record_{DateTime.Now:dd-MMM-yyyy}.xlsx";

            try
            {
                using (XLWorkbook wb = new XLWorkbook())
                {
                    var ws = wb.Worksheets.Add(dt, "Sheet1");
                    string headerRange = $"A1:{char.ConvertFromUtf32(dt.Columns.Count + 64)}1";
                    ws.Range(headerRange).Style.Font.Bold = true;
                    ws.Range(headerRange).Style.Fill.BackgroundColor = XLColor.LightBlue;

                    string wsRange = $"A1:{char.ConvertFromUtf32(dt.Columns.Count + 64)}{dt.Rows.Count + 1}";
                    ws.Table(0).Theme = XLTableTheme.None;
                    ws.Range(wsRange).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    ws.Range(wsRange).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    ws.Range(wsRange).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    ws.Range(wsRange).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    ws.Columns().AdjustToContents();

                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);
                        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                    }
                }
            }
            catch (Exception ex)
            {
                CommonHelper.write_log($"Error in DownloadPaymentRecord: {ex.Message}");
                // Optionally, return an error file or a message
                throw; // Re-throw the exception if you want to propagate it further
            }
        }


        //download Batch Summary Report
        [Route("Report/DownloadBatchSummaryReport/{doctor_id}/{batch_id}/{payment_status}")]
        public FileResult DownloadBatchSummaryReport(string doctor_id, string batch_id, string payment_status)
        {

            int val_doctor_id = string.IsNullOrWhiteSpace(doctor_id.Replace("1_", "")) ? 0 : Convert.ToInt32(doctor_id.Replace("1_", ""));
            int val_batch_id = string.IsNullOrWhiteSpace(batch_id.Replace("1_", "")) ? 0 : Convert.ToInt32(batch_id.Replace("1_", ""));

            DataTable dt = new DataTable();
            dt = objDALReports.DownloadBatchSummaryReport(val_doctor_id, val_batch_id, payment_status);
            string filnm = "Payment_Record" + "_" + DateTime.Now.ToString("dd-MMM-yyyy");
            using (XLWorkbook wb = new XLWorkbook())
            {
                var ws = wb.Worksheets.Add(dt, "Sheet1");
                string hederRange = "A1:" + char.ConvertFromUtf32(dt.Columns.Count + 64) + "1";
                ws.Range(hederRange).Style.Font.Bold = true;
                ws.Range(hederRange).Style.Fill.BackgroundColor = XLColor.LightBlue;

                string wsRange = "A1:" + char.ConvertFromUtf32(dt.Columns.Count + 64) + (dt.Rows.Count + 1);
                ws.Table(0).Theme = XLTableTheme.None;
                ws.Range(wsRange).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                ws.Range(wsRange).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Range(wsRange).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                ws.Range(wsRange).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                ws.Columns().AdjustToContents();
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);

                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filnm + ".xlsx");
                }
            }
        }
        //dowmload Employee Wise Payment Report
        [Route("Report/EmployeeWisePaymentReport/{from_date}/{to_date}/{employeecode}")]
        public FileResult DownloadEmployeeWisePaymentReport(string from_date, string to_date, string employeecode)
        {
            string val_empl_code = string.IsNullOrWhiteSpace(employeecode.Replace("1_", "")) ? "" : employeecode.Replace("1_", "");
            DateTime val_from_date = Convert.ToDateTime(from_date);
            DateTime val_to_date = Convert.ToDateTime(to_date);

            DataTable dt = new DataTable();
            dt = objDALReports.DownloadEmployeeWisePaymentReport(val_empl_code, val_from_date, val_to_date);
            string filnm = "Payment_Record" + "_" + DateTime.Now.ToString("dd-MMM-yyyy");
            using (XLWorkbook wb = new XLWorkbook())
            {
                var ws = wb.Worksheets.Add(dt, "Sheet1");
                string hederRange = "A1:" + char.ConvertFromUtf32(dt.Columns.Count + 64) + "1";
                ws.Range(hederRange).Style.Font.Bold = true;
                ws.Range(hederRange).Style.Fill.BackgroundColor = XLColor.LightBlue;

                string wsRange = "A1:" + char.ConvertFromUtf32(dt.Columns.Count + 64) + (dt.Rows.Count + 1);
                ws.Table(0).Theme = XLTableTheme.None;
                ws.Range(wsRange).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                ws.Range(wsRange).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Range(wsRange).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                ws.Range(wsRange).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                ws.Columns().AdjustToContents();
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);

                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filnm + ".xlsx");
                }
            }
        }
        //dowmload Settelment Report
        [Route("Report/SettelmentReport/{from_date}/{to_date}/{band}")]
        public FileResult DownloadSettelmentReport(string from_date, string to_date, string band)
        {
            string val_band = band.Replace("1_", "");
            DateTime val_from_date = Convert.ToDateTime(from_date);
            DateTime val_to_date = Convert.ToDateTime(to_date);

            DataTable dt = new DataTable();
            dt = objDALReports.DownloadSettelmentReport(val_band, val_from_date, val_to_date);
            string filnm = "Payment_Record" + "_" + DateTime.Now.ToString("dd-MMM-yyyy");
            using (XLWorkbook wb = new XLWorkbook())
            {
                var ws = wb.Worksheets.Add(dt, "Sheet1");
                string hederRange = "A1:" + char.ConvertFromUtf32(dt.Columns.Count + 64) + "1";
                ws.Range(hederRange).Style.Font.Bold = true;
                ws.Range(hederRange).Style.Fill.BackgroundColor = XLColor.LightBlue;
                string wsRange = "A1:" + char.ConvertFromUtf32(dt.Columns.Count + 64) + (dt.Rows.Count + 1);
                ws.Table(0).Theme = XLTableTheme.None;
                ws.Range(wsRange).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                ws.Range(wsRange).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Range(wsRange).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                ws.Range(wsRange).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                ws.Columns().AdjustToContents();
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);

                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filnm + ".xlsx");
                }
            }
        }
        //dowmload Settelment Summary Report
        [Route("Report/SettelmentSummaryReport/{payment_status}")]
        public FileResult DownloadSettelmentSummaryReport(string payment_status)
        {
            string val_payment_status = payment_status.Replace("1_", payment_status);
            DataTable dt = new DataTable();
            dt = objDALReports.DownloadSettelmentSummaryReport(val_payment_status);
            string filnm = "Payment_Record" + "_" + DateTime.Now.ToString("dd-MMM-yyyy");
            using (XLWorkbook wb = new XLWorkbook())
            {
                var ws = wb.Worksheets.Add(dt, "Sheet1");
                string hederRange = "A1:" + char.ConvertFromUtf32(dt.Columns.Count + 64) + "1";
                ws.Range(hederRange).Style.Font.Bold = true;
                ws.Range(hederRange).Style.Fill.BackgroundColor = XLColor.LightBlue;

                string wsRange = "A1:" + char.ConvertFromUtf32(dt.Columns.Count + 64) + (dt.Rows.Count + 1);
                ws.Table(0).Theme = XLTableTheme.None;
                ws.Range(wsRange).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                ws.Range(wsRange).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Range(wsRange).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                ws.Range(wsRange).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                ws.Columns().AdjustToContents();
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);

                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filnm + ".xlsx");
                }
            }
        }
    }
}