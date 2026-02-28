using DocumentFormat.OpenXml.InkML;
using MedicalR.Models.Reports;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace MedicalR.DataAccessLayer.IDAL.Reports
{
    public interface IDALReports
    {
        List<MedicalRDetails> GetPaymentRecordGrid(string band, DateTime? from_date, DateTime? to_date, int treatment_id, int doctor_id, string batch_no, string employeecode, string lot_no, string payment_status);
        List<MedicalRDetails> GetBatchSummaryReport(int doctor_id, string batch_no, string payment_status);
        List<MedicalRDetails> GetEmployeeWisePaymentReport(string employeecode, DateTime from_date, DateTime to_date);
        List<MedicalRDetails> GetSettlementReport(string band, DateTime from_date, DateTime to_date);
        List<MedicalRDetails> GetSettlementSummaryReport(string report_status);
        DataTable DownloadPaymentRecord(DateTime? from_date, DateTime? to_date, string band, int treatment_type, int doctor_id, string batch_no, string employeecode, string lot_no, string payment_status);
        DataTable DownloadBatchSummaryReport(int doctor_id, int batch_id, string payment_status);
        DataTable DownloadEmployeeWisePaymentReport(string employeecode, DateTime from_date, DateTime to_date);
        DataTable DownloadSettelmentReport(string band, DateTime from_date, DateTime to_date);
        DataTable DownloadSettelmentSummaryReport(string report_status);
    }
}