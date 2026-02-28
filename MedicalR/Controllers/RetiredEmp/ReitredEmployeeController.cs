using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using MedicalR.CustomHelper;
using MedicalR.DataAccessLayer.DAL.RetiredEmployee;
using MedicalR.DataAccessLayer.IDAL.RetiredEmployee;
using MedicalR.Models.MedicalR;
using MedicalR.Models.RetiredEmployee;
using Npgsql;

namespace MedicalR.Controllers.RetiredEmp
{
    [CustomAuthorize]
    public class ReitredEmployeeController : Controller
    {
        IDALRetiredEmployee objDALReiredEmployee = new DALRetiredEmployee();

        // GET: ReitredEmployee
        #region Retired Employee
        [CustomAuthorize]
        public ActionResult RetiredEmp()
        {
            var result = objDALReiredEmployee.GetRetiredEmp();
            return View(result);
        }
        [CustomAuthorize]
        [Route("ReitredEmployee/OfficeNote/{lotno}")]
        public ActionResult OfficeNote(string lotno)
        {

            var result = objDALReiredEmployee.GetTransOfficeNote(lotno);
            return View(result);
        }
        public ActionResult OfficeGetExportToExcel(ExportParaModel epmodel)
        {
            string lotno = epmodel.lotno;
            byte[] bytes = objDALReiredEmployee.GetTransOfficeNoteHtmlstring(lotno);
            return File(bytes, "application/pdf", "OfficeNote" + DateTime.Now.ToString("dd_MMM_yyyy") + ".pdf");
        }
        [CustomAuthorize]
        [Route("ReitredEmployee/BankAdvise/{lotno}")]
        public ActionResult BankAdvise(string lotno)
        {
            var result = objDALReiredEmployee.GetTransBankAdvise(lotno);
            return View(result);
        }
        [HttpPost]
        public JsonResult GetEmplPending(RetiredempTransModel objmodel)
        {
            // var Empcd = objmodel.Empcd;
            var result = objDALReiredEmployee.GetEmplPending(objmodel);
            return Json(result);
        }
        [CustomAuthorize]
        [Route("ReitredEmployee/EmplAnnexure/{lotno}")]
        public ActionResult EmplAnnexure(string lotno)
        {
            var result = objDALReiredEmployee.GetEmpByLoTAnnexure(lotno);
            return View(result);
        }

        public ActionResult AnnexExportToExcel(ExportParaModel epmodel)
        {
            DataTable dt = new DataTable();
            using (NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString))
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("select * from mdcl_sp_get_empl_by_lotno_for_annexure_to_export(:lotno)", con))
                {
                    cmd.Connection = con;
                    cmd.Parameters.AddWithValue("lotno", epmodel.lotno);
                    cmd.Parameters.AddWithValue("lotno", epmodel.lotno);
                    using (NpgsqlDataAdapter SqDA = new NpgsqlDataAdapter(cmd))
                    {
                        SqDA.Fill(dt);
                    }
                }
            }
            if (dt.Rows.Count > 0)
            {

            }
            var sheetNames = new List<string>() { "Sheet1" };
            string fileName = "Employee_wise_Annexure" + ".xlsx";
            XLWorkbook wbook = new XLWorkbook();
            int rowcount = 2;
            decimal Total = 0;
            int k = 0;
            decimal tempTotal = 0;
            decimal GrandTotal = 0;
            IXLWorksheet Sheet = wbook.Worksheets.Add(sheetNames[0]);
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                Sheet.Cell(1, (i + 1)).Value = dt.Columns[i].ColumnName;
                Sheet.Cell(1, (i + 1)).Style.Font.Bold = true;
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (dt.Rows[i][5].ToString() == "AXIS BANK")
                    {
                        if (j == 4)
                        {
                            Sheet.Cell((rowcount), (j + 1)).Value = dt.Rows[i][j].ToString();
                        }
                        else
                        {
                            Sheet.Cell((rowcount), (j + 1)).Value = "'" + dt.Rows[i][j].ToString();
                        }
                        //  Sheet.Cell((rowcount), (j + 1)).Value = "'" + dt.Rows[i][j].ToString();


                    }
                    else
                    {
                        if (k == 0)
                        {
                            Sheet.Cell(rowcount, 4).Value = "Total Amount";
                            Sheet.Cell(rowcount, 5).Value = tempTotal;
                            Sheet.Cell(rowcount, 4).Style.Fill.BackgroundColor = XLColor.Yellow;
                            Sheet.Cell(rowcount, 5).Style.Fill.BackgroundColor = XLColor.Yellow;
                            GrandTotal = tempTotal;
                            tempTotal = 0;
                            rowcount++;
                        }
                        k++;
                        // rowcount++;
                        if (j == 4)
                        {
                            Sheet.Cell((rowcount), (j + 1)).Value = dt.Rows[i][j].ToString();
                        }
                        else
                        {
                            Sheet.Cell((rowcount), (j + 1)).Value = "'" + dt.Rows[i][j].ToString();
                        }

                        // Sheet.Cell((rowcount), (j + 1)).Value = "'" + dt.Rows[i][j].ToString();
                        if (j == 6)
                        {
                            Total = Total + Convert.ToDecimal(dt.Rows[i][4].ToString());
                        }

                    }

                }
                tempTotal = tempTotal + Convert.ToDecimal(dt.Rows[i][4].ToString());
                rowcount++;
            }
            // rowcount++;
            Sheet.Cell(rowcount, 4).Value = "Total Amount";
            Sheet.Cell(rowcount, 5).Value = Total;
            Sheet.Cell(rowcount, 4).Style.Fill.BackgroundColor = XLColor.Yellow;
            Sheet.Cell(rowcount, 5).Style.Fill.BackgroundColor = XLColor.Yellow;
            rowcount++;
            GrandTotal = GrandTotal + Total;
            Sheet.Cell(rowcount, 4).Value = "Grand Total";
            Sheet.Cell(rowcount, 5).Value = GrandTotal;
            Sheet.Cell(rowcount, 4).Style.Fill.BackgroundColor = XLColor.Yellow;
            Sheet.Cell(rowcount, 5).Style.Fill.BackgroundColor = XLColor.Yellow;
            // Sheet.Cell(rowcount, 5).Style.NumberFormat.Format = "$* #,##0.00";
            Sheet.Columns("E").Style.NumberFormat.Format = "* #,##0.00";
            // var range = Sheet.Range("A1" + ":" + "H" + rowcount);
            //  range.Columns.AutoFit();
            Sheet.Columns("A", "H").AdjustToContents();

            Sheet.Range("A1:H" + rowcount).Style.Border.InsideBorder = XLBorderStyleValues.Thin;
            Sheet.Range("A1:H" + rowcount).Style.Border.OutsideBorder = XLBorderStyleValues.Medium;
            Sheet.Range("A1:H" + rowcount).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            Stream spreadsheetStream = new MemoryStream();
            wbook.SaveAs(spreadsheetStream);
            spreadsheetStream.Position = 0;

            return new FileStreamResult(spreadsheetStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") { FileDownloadName = fileName };

        }

        public ActionResult BankAdviseGetExportToExcel(ExportParaModel epmodel)
        {
            string lotno = epmodel.lotno;
            byte[] bytes = objDALReiredEmployee.GetTransBankAdviseHtmlstring(lotno);
            return File(bytes, "application/pdf", "OfficeNote" + DateTime.Now.ToString("dd_MMM_yyyy") + ".pdf");
        }

        public ActionResult GetExportToExcel(ExportParaModel epmodel)
        {

            byte[] bytes = objDALReiredEmployee.GetEditCheckingHtmlString();
            return File(bytes, "application/pdf", "EditCheckingList_" + DateTime.Now.ToString("dd_MMM_yyyy") + ".pdf");
        }

        public ActionResult Sanc_GetExportToExcel(ExportParaModel epmodel)
        {

            byte[] bytes = objDALReiredEmployee.GetApprovalHtmlString();
            return File(bytes, "application/pdf", "ApprovalList" + DateTime.Now.ToString("dd_MMM_yyyy") + ".pdf");
        }
        public ActionResult QueryModel()
        {
            var result = objDALReiredEmployee.GetLoTNo();
            return View(result);
        }
        [HttpPost]
        public JsonResult GetEmpByLot(RetiredempTransModel objmodel)
        {
            var lotno = objmodel.lot_no;
            var result = objDALReiredEmployee.GetEmpByLoT(lotno);
            return Json(result);
        }
        //[HttpPost]
        //public JsonResult GetEmpByEmpcd(RetiredempTransModel objmodel)
        //{
        //    var Empcd = objmodel.Empcd;
        //    var result = objDALReiredEmployee.GetEmpByEmpcd(Empcd);
        //    return Json(result);
        //}
        [HttpPost]
        public JsonResult GetEmpByEmpcd(RetiredempTransModel objmodel)
        {
            // var Empcd = objmodel.Empcd;
            var result = objDALReiredEmployee.GetEmpByEmpcd(objmodel);
            return Json(result);
        }
        [HttpPost]
        public JsonResult CheckInTable(RetiredEmployeeModel objmodel)
        {
            var result = objDALReiredEmployee.CheckInTable(objmodel);
            return Json(result);
        }
        [CustomAuthorize]
        public ActionResult GenrateLot()
        {
            var result = objDALReiredEmployee.GetdataForLot();
            return View(result);
        }

        //public ActionResult Get_Lots_Grid_data()
        //{
        //    return new JsonNetResult(objDALReiredEmployee.GetdataForLot());
        //}

        public ActionResult LotGenerationStart(List<RetiredempTransModel> model)
        {
            var res = objDALReiredEmployee.LotGenerationStart(model).Message;
            return Json(res);
        }
        public ActionResult EditChecking()
        {
            //var result = objDALReiredEmployee.CheckingVerify();
            return View();
        }

        [HttpGet]
        public ActionResult EditCheckingGridData()
        {
            //return new JsonNetResult(objDALReiredEmployee.CheckingVerify());
            var data = objDALReiredEmployee.CheckingVerify(); // List<RetiredempTransModel>
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult EditCheckingStart2(List<RetiredempTransModel> model2)
        {
            //return new JsonNetResult(objDALReiredEmployee.EditCheckingStart(model2).Message);
            var result = objDALReiredEmployee.EditCheckingStart(model2).Message;
            return Json(result);
        }

        [CustomAuthorize]
        // [Route("ReitredEmployee/Trans_RetiredEmp/{id}/edit")]
        public ActionResult CheckingVerify(string id)
        {
            RetiredempTransModel model = new RetiredempTransModel();
            model.Empcd = id;
            string empcd = model.Empcd;

            ViewBag.Yearlist = objDALReiredEmployee.Getyears(empcd);
            var result = objDALReiredEmployee.GetsingleEmpForChecking(model);
            return View(result);
        }
        [HttpPost]
        public JsonResult CheckVerified(RetiredempTransModel model)
        {

            var result = objDALReiredEmployee.CheckVerified(model);
            return Json(result);
        }
        [CustomAuthorize]
        public ActionResult Trans_Sanction_Grid()
        {
            var result = objDALReiredEmployee.Trans_Sanction_Grid();
            return View(result);
        }

        [HttpPost]
        public ActionResult EmplApprovalList()
        {
            RetiredempTransModel trans_list = objDALReiredEmployee.EmplApprovalList();
            DataTable dt = new DataTable();
            StringBuilder sb = new StringBuilder();
            using (System.IO.StreamReader sr = new StreamReader(ConfigurationManager.AppSettings["Approvalhtml"].ToString()))
            {

                int rownumber = 0;

                String line;

                while ((line = sr.ReadLine()) != null)
                {
                    line = line.Replace("#listDate", trans_list.disp_lot_date);

                    if ((line.Trim() == ("<tbody>") && rownumber <= dt.Rows.Count))
                    {
                        foreach (emplSanc model in trans_list.EmplSanc_list)
                        {
                            rownumber++;
                            string tr_line = "<tbody>" + "<tr><td>" + rownumber + "</td><td>" + model.lot_no + "</td><td>" + model.disp_lot_date + "</td><td>" + model.sanc_amt + "</td></tr>";
                            sb.AppendLine(tr_line);
                        }
                    }
                    else
                    {
                        sb.AppendLine(line);
                    }
                }
                sb.ToString();
            }
            return Json(sb.ToString());



        }

        [HttpPost]
        public JsonResult ApproveTrans(RetiredempTransModel objModel)
        {
            var result = objDALReiredEmployee.ApproveTrans(objModel);
            return Json(result);
        }
        [CustomAuthorize]
        public ActionResult Trans_Sanctioned(string id)
        {
            RetiredempTransModel model = new RetiredempTransModel();
            model.Empcd = id;
            var result = objDALReiredEmployee.GetsingleEmpForSanction(model);
            return View(result);
        }
        [HttpPost]
        public JsonResult AddSanction(RetiredempTransModel model)
        {

            var result = objDALReiredEmployee.AddSanction(model);
            return Json(result);
        }
        [HttpPost]
        public JsonResult Reject(RetiredempTransModel model)
        { 
            var result = objDALReiredEmployee.Reject(model);
            return Json(result);
        }

        [CustomAuthorize]
        [Route("ReitredEmployee/ReitredEmployee/{id}/add")]
        public ActionResult Retiredemp_AddNew()
        {
            ViewBag.Banklist = objDALReiredEmployee.GetBank();
            return View();
        }

        [HttpPost]
        public JsonResult RetiredNewAdd(RetiredEmployeeModel objModel)
        {
            var result = objDALReiredEmployee.AddRemployee(objModel);
            return Json(result);
        }

        [CustomAuthorize]
        [Route("ReitredEmployee/ReitredEmployee/{id}/edit")]
        public ActionResult RetiredEmp_Edit(int id)
        {
            RetiredEmployeeModel model = new RetiredEmployeeModel();
            ViewBag.Banklist = objDALReiredEmployee.GetBank();
            model.Id = id;
            var result = objDALReiredEmployee.GetsingleRempdetails(model);
            return View(result);
        }

        [HttpPost]
        public JsonResult UpdateRemp(RetiredEmployeeModel objModel)
        {
            var result = objDALReiredEmployee.UpdateRemp(objModel);
            return Json(result);
        }

        [HttpPost]
        public JsonResult RempStatus(RetiredEmployeeModel objModel)
        {
            var result = objDALReiredEmployee.RempStatus(objModel);
            return Json(result);
        }
        #endregion

        #region Transaction for Retired Employee

        [CustomAuthorize]
        public ActionResult Trans_RetiredEmp()
        {
            var result = objDALReiredEmployee.GetTransRetiredEmp();
            return View(result);
        }

        [CustomAuthorize]
        [Route("ReitredEmployee/Trans_RetiredEmp/{id}/add")]
        public ActionResult Remptrans_AddNew()
        {
            // ViewBag.Yearlist = objDALReiredEmployee.Getyears();
            return View();
        }
        [HttpPost]
        public JsonResult ApproveLotno(RetiredempTransModel objModel)
        {
            var result = objDALReiredEmployee.ApproveLotno(objModel);
            return Json(result);
        }

        [HttpPost]
        public JsonResult RemptransNewAdd(RetiredempTransModel objModel)
        {
            var result = objDALReiredEmployee.RemptransNewAdd(objModel);
            return Json(result);
        }
        [HttpPost]
        public JsonResult GetYears(string empcd)
        {

            var result = objDALReiredEmployee.GetYears(empcd);
            return Json(result);
        }
        [HttpPost]
        public JsonResult GetName(string empcd)
        {

            var result = objDALReiredEmployee.GetName(empcd);
            return Json(result);
        }
        [HttpPost]
        public JsonResult GetAmount(string yearfrom)
        {
            CommonHelper.write_log("yearfrom  :" + yearfrom);
            var result = objDALReiredEmployee.GetAmount(yearfrom);
            return Json(result);
        }
        [CustomAuthorize]
        [Route("ReitredEmployee/Trans_RetiredEmp/{id}/{Empcd}/edit")]
        public ActionResult Remptrans_Edit(int id, string Empcd)
        {
            RetiredempTransModel model = new RetiredempTransModel();
            model.Id = id;
            model.Empcd = Empcd;
            ViewBag.Yearlist = objDALReiredEmployee.Getyears(Empcd);
            var result = objDALReiredEmployee.GetsingleRemptransdetails(model);
            return View(result);
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
        #endregion

        #region Reimbursement Amount Master
        [CustomAuthorize]
        public ActionResult Reimburse_Amt()
        {
            var result = objDALReiredEmployee.GetRetiredReimurseAmt();
            return View(result);
        }

        [CustomAuthorize]
        [Route("ReitredEmployee/Reimburse_Amt/{id}/add")]
        public ActionResult Reimburse_AddNew()
        {
            return View();
        }

        [HttpPost]
        public JsonResult ReimburseNewAdd(ReimburseAmtModel objModel)
        {
            var result = objDALReiredEmployee.AddReimbusamt(objModel);
            return Json(result);
        }

        [CustomAuthorize]
        [Route("ReitredEmployee/Reimburse_Amt/{id}/edit")]
        public ActionResult Reimburse_Edit(int id)
        {
            ReimburseAmtModel model = new ReimburseAmtModel();
            model.Id = id;
            var result = objDALReiredEmployee.GetsingleReimburseamt(model);
            return View(result);
        }

        [HttpPost]
        public JsonResult UpdateReimburse(ReimburseAmtModel objModel)
        {
            var result = objDALReiredEmployee.UpdateReimburse(objModel);
            return Json(result);
        }

        [HttpPost]
        public JsonResult ReimburseamtStatus(ReimburseAmtModel objModel)
        {
            var result = objDALReiredEmployee.ReimburseamtStatus(objModel);
            return Json(result);
        }
        #endregion

        #region RetiredEmp Demise Master
        [CustomAuthorize]
        public ActionResult RetiredEmpDemise()
        {
            var result = objDALReiredEmployee.GetRetiredEmpDemise();
            return View(result);
        }

        [CustomAuthorize]
        [Route("ReitredEmployee/RetiredEmpDemise/{id}/add")]
        public ActionResult RetiredempDemise_AddNew()
        {
            ViewBag.Banklist = objDALReiredEmployee.GetBank();
            return View();
        }

        [HttpPost]
        public JsonResult RetiredempDemiseAddNew(RempDemiseModel objModel)
        {
            var result = objDALReiredEmployee.AddRempDemise(objModel);
            return Json(result);
        }

        [CustomAuthorize]
        [Route("ReitredEmployee/RetiredEmpDemise/{id}/edit")]
        public ActionResult Rempdemise_Edit(int id)
        {
            RempDemiseModel model = new RempDemiseModel();
            model.Id = id;
            ViewBag.Banklist = objDALReiredEmployee.GetBank();
            var result = objDALReiredEmployee.GetsingleRempdemise(model);
            return View(result);
        }

        [HttpPost]
        public JsonResult UpdateRempdemise(RempDemiseModel objModel)
        {
            var result = objDALReiredEmployee.UpdateRempdemise(objModel);
            return Json(result);
        }

        [HttpPost]
        public JsonResult RempDemiseStatus(RempDemiseModel objModel)
        {
            var result = objDALReiredEmployee.RempDemiseStatus(objModel);
            return Json(result);
        }

        #endregion

        public ActionResult YearWisePaymentDetails()
        {
            ViewBag.Yearlist = objDALReiredEmployee.GetYear();
            return View();
        }

        [HttpPost]
        public JsonResult GetyearwiseReport(RetiredempTransModel objmodel)
        {
            // var Empcd = objmodel.Empcd;
            var result = objDALReiredEmployee.GetyearwiseReport(objmodel);
            return Json(result);
        }
        public ActionResult ReportExportToExcel(ExportParaModel epmodel)
        {
            DataTable dt = new DataTable();
            using (NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString))
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("select * from mdcl_sp_get_yearwise_report(:pyear)", con))
                {
                    cmd.Connection = con;
                    cmd.Parameters.AddWithValue("pyear", epmodel.year);
                    using (NpgsqlDataAdapter SqDA = new NpgsqlDataAdapter(cmd))
                    {
                        SqDA.Fill(dt);
                    }
                }
            }
            if (dt.Rows.Count > 0)
            {

            }
            var sheetNames = new List<string>() { "Sheet1" };
            string fileName = "Year_wise_Report" + ".xlsx";
            XLWorkbook wbook = new XLWorkbook();
            int rowcount = 2;
            decimal Total = 0;
            int k = 0;
            decimal tempTotal = 0;
            // decimal GrandTotal = 0;
            IXLWorksheet Sheet = wbook.Worksheets.Add(sheetNames[0]);
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                Sheet.Cell(1, (i + 1)).Value = dt.Columns[i].ColumnName;
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (j == 0)
                    {
                        Sheet.Cell((rowcount), (j + 1)).Value = "'" + dt.Rows[i][j].ToString();
                    }
                    else if (j == 5)
                    {
                        DateTime paid_date = DateTime.ParseExact(dt.Rows[i][j].ToString(), "dd-MM-yyyy", CultureInfo.InvariantCulture);
                        Sheet.Cell((rowcount), (j + 1)).Value = paid_date.ToString("dd/MM/yyyy");
                    }
                    else if (j == 6)
                    {
                        Total = Total + Convert.ToDecimal(dt.Rows[i][6].ToString());
                        Sheet.Cell((rowcount), (j + 1)).Value = Convert.ToDecimal(dt.Rows[i][6].ToString());
                    }
                    else
                    {
                        Sheet.Cell((rowcount), (j + 1)).Value = dt.Rows[i][j].ToString();
                    }

                }
                tempTotal = tempTotal + Convert.ToDecimal(dt.Rows[i][6].ToString());
                rowcount++;
            }
            // rowcount++;
            Sheet.Cell(rowcount, 6).Value = "Total Amount";
            Sheet.Cell(rowcount, 7).Value = Total;
            Sheet.Cell(rowcount, 6).Style.Fill.BackgroundColor = XLColor.Yellow;
            Sheet.Cell(rowcount, 7).Style.Fill.BackgroundColor = XLColor.Yellow;
            rowcount++;
            Sheet.Cell(1, 1).Style.Fill.BackgroundColor = XLColor.Yellow;
            Sheet.Cell(1, 2).Style.Fill.BackgroundColor = XLColor.Yellow;
            Sheet.Cell(1, 3).Style.Fill.BackgroundColor = XLColor.Yellow;
            Sheet.Cell(1, 4).Style.Fill.BackgroundColor = XLColor.Yellow;
            Sheet.Cell(1, 5).Style.Fill.BackgroundColor = XLColor.Yellow;
            Sheet.Cell(1, 6).Style.Fill.BackgroundColor = XLColor.Yellow;
            Sheet.Cell(1, 7).Style.Fill.BackgroundColor = XLColor.Yellow;

            Sheet.Columns("G").Style.NumberFormat.Format = "0.00";
            // var range = Sheet.Range("A1" + ":" + "H" + rowcount);
            //  range.Columns.AutoFit();
            Sheet.Columns("A", "G").AdjustToContents();

            Sheet.Range("A1:G" + rowcount).Style.Border.InsideBorder = XLBorderStyleValues.Thin;
            Sheet.Range("A1:G" + rowcount).Style.Border.OutsideBorder = XLBorderStyleValues.Medium;
            Sheet.Range("A1:G" + rowcount).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;



            Stream spreadsheetStream = new MemoryStream();
            wbook.SaveAs(spreadsheetStream);
            spreadsheetStream.Position = 0;

            return new FileStreamResult(spreadsheetStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") { FileDownloadName = fileName };

        }
    }
}