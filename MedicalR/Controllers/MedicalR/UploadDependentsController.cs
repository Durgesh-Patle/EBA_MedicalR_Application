using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MedicalR.CustomHelper;
using MedicalR.DataAccessLayer.DAL.MedicalR;
using MedicalR.DataAccessLayer.IDAL.MedicalR;
using MedicalR.Models;
using OfficeOpenXml;

namespace MedicalR.Controllers.MedicalR
{
    [CustomAuthorize]
    public class UploadDependentsController : Controller
    {
        IDALEmployeFamilyDetails family = new DALEmployeFamilyDetails();
        // GET: UploadDependents
        ResponseModel res = new ResponseModel();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UploadFile()
        {
            ResponseModel res = new ResponseModel();
            try
            {
                if (Request != null)
                {
                    HttpPostedFileBase file = Request.Files["UploadedFile"];
                    if (file != null && file.ContentLength > 0 && !string.IsNullOrEmpty(file.FileName))
                    {
                        string projectPath = Server.MapPath("~");

                        string uploadPath = Path.Combine(projectPath, "UploadedFiles");
                        CommonHelper.write_log("uploadPath :" + uploadPath);
                        if (!Directory.Exists(uploadPath))
                        {
                            Directory.CreateDirectory(uploadPath);
                        }

                        string filePath = Path.Combine(Server.MapPath("~/UploadedFiles"), file.FileName);
                        CommonHelper.write_log("filePath :" + filePath);
                        try
                        {
                            file.SaveAs(filePath);
                        }
                        catch (Exception ex)
                        {
                            CommonHelper.write_log("error while saving file :" + ex.Message);
                        }
                        var Response = family.ReadExcel(filePath);

                        res = Response;

                    }
                }
            }
            catch (Exception ex)
            {
                res.Message = ex.Message;
            }
            return Json(res.Message, JsonRequestBehavior.AllowGet);

        }

        public ActionResult DownloadExcel()
        {
            try
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;


                using (ExcelPackage excelPackage = new ExcelPackage())
                {
                    ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("family_employee_details");

                    worksheet.Cells[1, 1].Value = "EMPLID";
                    worksheet.Cells[1, 2].Value = "NAME";
                    worksheet.Cells[1, 3].Value = "DOB";
                    worksheet.Cells[1, 4].Value = "RELATION";

                    using (ExcelRange range = worksheet.Cells["A1:D1"])
                    {
                        range.Style.Font.Bold = true;
                    }

                    byte[] excelBytes = excelPackage.GetAsByteArray();

                    HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                    response.Content = new ByteArrayContent(excelBytes);
                    response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                    {
                        FileName = "Depandent_list.xlsx"
                    };
                    response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");

                    return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "family_employee_details.xlsx");
                }
            }
            catch (Exception ex)
            {

                CommonHelper.write_log($"An error occurred while downloading the Excel file: {ex.Message}");
                return RedirectToAction("Index");
            }
        }

    }
}