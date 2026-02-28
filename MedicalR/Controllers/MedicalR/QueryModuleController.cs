using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClosedXML.Excel;
using MedicalR.CustomHelper;
using MedicalR.Models.MedicalR;

namespace MedicalR.DataAccessLayer.DAL.MedicalR
{
    [CustomAuthorize]
    public class QueryModuleController : Controller
    {
        // GET: QueryModule
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetQMGridData(string batch_no, string appl_no, string empl_code, string lot_no, string query_type)
        {
            DALQueryModule bll = new DALQueryModule();
            return Json(bll.GetQMGridData(batch_no, appl_no, empl_code, lot_no, query_type), JsonRequestBehavior.AllowGet);
        }

        [Route("QueryModule/download/{batch_no}/{lot_no}/{query_type}/{from_date}/{to_date}")]
        public FileResult download(string batch_no, string lot_no, string query_type, string from_date, string to_date)
        {
            batch_no = batch_no.Replace("1_", "");
            lot_no = lot_no.Replace("1_", "");
            query_type = query_type.Replace("1_", "");
            DALQueryModule bll = new DALQueryModule();
            //  string temp = bll.PreparePrintDoc(batch_no, lot_no, query_type);
            // byte[] bytes = CommonHelper.Convert2(temp.ToString());
            string Empcode = UserManager.User.Employeecode;
            string FileName = string.Empty;
            DataTable dt = new DataTable();
            int rowcount = 2;
            Stream spreadsheetStream = new MemoryStream();
            if (query_type == "Bank Advise Excel")
            {
                decimal Total = 0;

                dt = bll.PrepareExcelDoc(batch_no, lot_no, query_type);
                if (dt.Rows.Count > 0)
                {
                    var sheetNames = new List<string>() { "Annexure" };
                    // string fileName = "Employee_wise_Annexure" + ".xlsx";
                    FileName = "Bank_advice" + "_" + lot_no + "_" + DateTime.Now.ToString("dd-MM-yyyy") + ".xlsx";
                    XLWorkbook wbook = new XLWorkbook();
                    IXLWorksheet Sheet = wbook.Worksheets.Add(sheetNames[0]);

                    Sheet.Cell(1, (1)).Value = "EmplID";
                    Sheet.Cell(1, (1)).Style.Font.Bold = true;
                    Sheet.Cell(1, (1)).Value = "EmplID";
                    Sheet.Cell(1, (1)).Style.Font.Bold = true;
                    Sheet.Cell(1, (2)).Value = "Name of Employee";
                    Sheet.Cell(1, (2)).Style.Font.Bold = true;
                    Sheet.Cell(1, (3)).Value = "Account No";
                    Sheet.Cell(1, (3)).Style.Font.Bold = true;
                    Sheet.Cell(1, (4)).Value = "Sanctioned Amt(Rs)";
                    Sheet.Cell(1, (4)).Style.Font.Bold = true;

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            if (j == 2 || j == 0)
                            {
                                Sheet.Cell((rowcount), (j + 1)).Value = "'" + dt.Rows[i][j].ToString();
                                //  Sheet.Cell((rowcount), (j + 1)).Value = dt.Rows[i][j].ToString();
                            }
                            else
                            {
                                Sheet.Cell((rowcount), (j + 1)).Value = dt.Rows[i][j].ToString();
                            }

                        }
                        Total = Total + Convert.ToDecimal(dt.Rows[i][3].ToString());
                        rowcount++;
                    }
                    Sheet.Cell(rowcount, 3).Value = "Grand Total";
                    Sheet.Cell(rowcount, 3).Style.Font.Bold = true;
                    Sheet.Cell(rowcount, 4).Value = Total;
                    Sheet.Cell(rowcount, 4).Style.Font.Bold = true;
                    Sheet.Columns("A", "D").AdjustToContents();
                    Sheet.Columns("D").Style.NumberFormat.Format = "* #,##0.00";

                    Sheet.Range("A1:D" + rowcount).Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                    Sheet.Range("A1:D" + rowcount).Style.Border.OutsideBorder = XLBorderStyleValues.Medium;
                    Sheet.Range("A1:D" + rowcount).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;

                    wbook.SaveAs(spreadsheetStream);
                    spreadsheetStream.Position = 0;
                }
                return new FileStreamResult(spreadsheetStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") { FileDownloadName = FileName };
            }
            else if (query_type == "Payment Details For SAP")
            {
                decimal Total = 0;
                dt = bll.PrepareExcelDocForSAP(batch_no, lot_no, query_type);
                if (dt.Rows.Count > 0)
                {
                    var sheetNames = new List<string>() { "Payment Details For SAP-" + lot_no };
                    // string fileName = "Employee_wise_Annexure" + ".xlsx";
                    FileName = "Payment Details For SAP" + "_" + lot_no + "_" + DateTime.Now.ToString("dd-MM-yyyy") + ".xlsx";
                    XLWorkbook wbook = new XLWorkbook();
                    IXLWorksheet Sheet = wbook.Worksheets.Add(sheetNames[0]);
                    //for (int i = 0; i < dt.Columns.Count; i++)
                    //{
                    //    Sheet.Cell(1, (i + 1)).Value = dt.Columns[i].ColumnName;
                    //    Sheet.Cell(1, (i + 1)).Style.Font.Bold = true;
                    //    Sheet.Cell(1, (i + 1)).Style.Border.OutsideBorder = XLBorderStyleValues.Medium;
                    //}

                    Sheet.Cell(1, (0 + 1)).Value = "doc_date";
                    Sheet.Cell(1, (0 + 1)).Style.Font.Bold = true;
                    Sheet.Cell(1, (0 + 1)).Style.Border.OutsideBorder = XLBorderStyleValues.Medium;

                    Sheet.Cell(1, (1 + 1)).Value = "Voucher_no";
                    Sheet.Cell(1, (1 + 1)).Style.Font.Bold = true;
                    Sheet.Cell(1, (1 + 1)).Style.Border.OutsideBorder = XLBorderStyleValues.Medium;

                    Sheet.Cell(1, (2 + 1)).Value = "Postingkey";
                    Sheet.Cell(1, (2 + 1)).Style.Font.Bold = true;
                    Sheet.Cell(1, (2 + 1)).Style.Border.OutsideBorder = XLBorderStyleValues.Medium;

                    Sheet.Cell(1, (3 + 1)).Value = "Account";
                    Sheet.Cell(1, (3 + 1)).Style.Font.Bold = true;
                    Sheet.Cell(1, (3 + 1)).Style.Border.OutsideBorder = XLBorderStyleValues.Medium;

                    Sheet.Cell(1, (4 + 1)).Value = "Amount";
                    Sheet.Cell(1, (4 + 1)).Style.Font.Bold = true;
                    Sheet.Cell(1, (4 + 1)).Style.Border.OutsideBorder = XLBorderStyleValues.Medium;


                    Sheet.Cell(1, (5 + 1)).Value = "Costcentre";
                    Sheet.Cell(1, (5 + 1)).Style.Font.Bold = true;
                    Sheet.Cell(1, (5 + 1)).Style.Border.OutsideBorder = XLBorderStyleValues.Medium;


                    Sheet.Cell(1, (6 + 1)).Value = "Employee";
                    Sheet.Cell(1, (6 + 1)).Style.Font.Bold = true;
                    Sheet.Cell(1, (6 + 1)).Style.Border.OutsideBorder = XLBorderStyleValues.Medium;

                    Sheet.Cell(1, (7 + 1)).Value = "Ba";
                    Sheet.Cell(1, (7 + 1)).Style.Font.Bold = true;
                    Sheet.Cell(1, (7 + 1)).Style.Border.OutsideBorder = XLBorderStyleValues.Medium;

                    Sheet.Cell(1, (8 + 1)).Value = "Narration";
                    Sheet.Cell(1, (8 + 1)).Style.Font.Bold = true;
                    Sheet.Cell(1, (8 + 1)).Style.Border.OutsideBorder = XLBorderStyleValues.Medium;


                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            if (j == 0 || j == 6)
                            {
                                Sheet.Cell((rowcount), (j + 1)).Value = "'" + dt.Rows[i][j].ToString();
                            }
                            else
                            {
                                Sheet.Cell((rowcount), (j + 1)).Value = dt.Rows[i][j].ToString();
                            }
                        }
                        rowcount++;
                    }
                    Sheet.Columns("A", "I").AdjustToContents();
                    Sheet.Columns("E").Style.NumberFormat.Format = "* #,##0.00";


                    Sheet.Range("A1:I" + rowcount).Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                    Sheet.Range("A1:I" + rowcount).Style.Border.OutsideBorder = XLBorderStyleValues.Medium;
                    Sheet.Range("A1:I" + rowcount).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                    wbook.SaveAs(spreadsheetStream);
                    spreadsheetStream.Position = 0;
                }
                return new FileStreamResult(spreadsheetStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") { FileDownloadName = FileName };
            }
            else if (query_type == "Payment Details For Hewitt")
            {
                decimal Total = 0;
                dt = bll.PrepareExcelDocForHewettData(batch_no, lot_no, query_type, from_date, to_date);
                if (dt.Rows.Count > 0)
                {
                    var sheetNames = new List<string>() { "Hewitt " + lot_no };
                    // string fileName = "Employee_wise_Annexure" + ".xlsx";
                    FileName = "Payment Details For Hewitt" + "_" + lot_no + "_" + DateTime.Now.ToString("dd-MM-yyyy") + ".xlsx";
                    XLWorkbook wbook = new XLWorkbook();
                    IXLWorksheet Sheet = wbook.Worksheets.Add(sheetNames[0]);
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        Sheet.Cell(1, (i + 1)).Value = dt.Columns[i].ColumnName;
                        Sheet.Cell(1, (i + 1)).Style.Font.Bold = true;
                        Sheet.Cell(1, (i + 1)).Style.Border.OutsideBorder = XLBorderStyleValues.Medium;
                    }
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            if (j == 0 || j == 9 || j == 12)
                            {
                                Sheet.Cell((rowcount), (j + 1)).Value = "'" + dt.Rows[i][j].ToString();
                            }
                            else
                            {
                                Sheet.Cell((rowcount), (j + 1)).Value = dt.Rows[i][j].ToString();
                            }
                        }
                        rowcount++;
                    }
                    Sheet.Columns("A", "O").AdjustToContents();
                    Sheet.Columns("H").Style.NumberFormat.Format = "* #,##0.00";
                    Sheet.Columns("I").Style.NumberFormat.Format = "* #,##0.00";
                    //Sheet.Columns("I").Style.NumberFormat.Format = "* #,##0";
                    //Sheet.Columns("J").Style.NumberFormat.Format = "* #,##0";
                    Sheet.Range("A1:O" + rowcount).Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                    Sheet.Range("A1:O" + rowcount).Style.Border.OutsideBorder = XLBorderStyleValues.Medium;
                    Sheet.Range("A1:O" + rowcount).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                    wbook.SaveAs(spreadsheetStream);
                    spreadsheetStream.Position = 0;
                }
                return new FileStreamResult(spreadsheetStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") { FileDownloadName = FileName };
            }
            else
            {
                string temp = bll.PreparePrintDoc(batch_no, lot_no, query_type);
                byte[] bytes = CommonHelper.Convert2(temp.ToString());
                if (query_type == "Office Notes")
                {
                    FileName = "Office_Notes" + "_" + lot_no;
                    return File(bytes, "application/pdf", FileName + "_" + DateTime.Now.ToString("dd_MMM_yyyy") + ".pdf");
                }
                else if (query_type == "Bank Advise")
                {
                    FileName = "Bank_advice" + "_" + lot_no;
                    return File(bytes, "application/pdf", FileName + "_" + DateTime.Now.ToString("dd_MMM_yyyy") + ".pdf");
                }
                else if (query_type == "Batch Print")
                {
                    FileName = "BatchWise_print" + "_" + batch_no;
                    return File(bytes, "application/pdf", FileName + "_" + DateTime.Now.ToString("dd_MMM_yyyy") + ".pdf");
                }
                else
                {
                    FileName = "Default";
                    return File(bytes, "application/pdf", FileName + "_" + DateTime.Now.ToString("dd_MMM_yyyy") + ".pdf");
                }
            }


        }
    }
}