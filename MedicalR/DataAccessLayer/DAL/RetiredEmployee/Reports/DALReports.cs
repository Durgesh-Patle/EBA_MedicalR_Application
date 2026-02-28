using Dapper;
using MedicalR.CustomHelper;
using MedicalR.DataAccessLayer.IDAL.Reports;
using MedicalR.Models.Reports;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MedicalR.DataAccessLayer.DAL.Reports
{
    public class DALReports : IDALReports
    {
        public List<MedicalRDetails> GetPaymentRecordGrid(string band, DateTime? from_date, DateTime? to_date, int treatment_id, int doctor_id, string batch_no, string employeecode, string lot_no, string payment_status)
        {
            List<MedicalRDetails> lst_details = new List<MedicalRDetails>();
            try
            {
                CommonHelper.write_log("DAL: GetPaymentRecordGrid started");

                using (NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {
                    con.Open();
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("p_band", string.IsNullOrWhiteSpace(band) ? "" : band);
                    parameters.Add("p_from_date", Convert.ToDateTime(from_date));
                    parameters.Add("p_to_date", Convert.ToDateTime(to_date));
                    parameters.Add("p_treatment_type", string.IsNullOrWhiteSpace(treatment_id.ToString()) ? 0 : treatment_id);
                    parameters.Add("p_doctor_id", doctor_id);
                    parameters.Add("p_batch_no", batch_no);
                    parameters.Add("p_employee_code", employeecode);
                    parameters.Add("p_lot_no", lot_no);
                    parameters.Add("p_payment_status", payment_status);
                    lst_details = con.Query<MedicalRDetails>("select * from mdcl_sp_mr_payment_report(:p_band,:p_from_date,:p_to_date,:p_treatment_type,:p_doctor_id,:p_batch_no,:p_employee_code,:p_lot_no,:p_payment_status)", parameters).ToList();
                    CommonHelper.write_log($"Records fetched: {lst_details.Count}");
                }
                CommonHelper.write_log("DAL: GetPaymentRecordGrid completed successfully");
            }
            catch (Exception ex)
            {
                CommonHelper.write_log("ERROR in DAL GetPaymentRecordGrid: " + ex.ToString());
                CommonHelper.write_log("StackTrace in DAL GetPaymentRecordGrid: " + ex.StackTrace);
                throw; // Important → controller tak error jaane do
            }

            return lst_details;
        }
        public List<MedicalRDetails> GetBatchSummaryReport(int doctor_id, string batch_no, string payment_status)
        {
            List<MedicalRDetails> lst_details = new List<MedicalRDetails>();
            using (NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString))
            {
                con.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("p_doctor_id", doctor_id);
                parameters.Add("p_batch_no", batch_no);
                parameters.Add("p_payment_status", payment_status);
                lst_details = con.Query<MedicalRDetails>("select * from mdcl_sp_mr_batch_summary_report(:p_doctor_id,:p_batch_no,:p_payment_status)", parameters).ToList();
            }
            return lst_details;
        }
        public List<MedicalRDetails> GetEmployeeWisePaymentReport(string employeecode, DateTime from_date, DateTime to_date)
        {
            List<MedicalRDetails> lst_details = new List<MedicalRDetails>();
            using (NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString))
            {
                con.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("p_employeecode", string.IsNullOrWhiteSpace(employeecode) ? "Select" : employeecode);
                parameters.Add("p_from_date", Convert.ToDateTime(from_date));
                parameters.Add("p_to_date", Convert.ToDateTime(to_date));
                lst_details = con.Query<MedicalRDetails>("select * from mdcl_sp_mr_employee_wise_payment_report(:p_employeecode,:p_from_date,:p_to_date)", parameters).ToList();
            }
            return lst_details;
        }
        public List<MedicalRDetails> GetSettlementReport(string band, DateTime from_date, DateTime to_date)
        {
            List<MedicalRDetails> lst_details = new List<MedicalRDetails>();
            using (NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString))
            {
                con.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("p_band", string.IsNullOrWhiteSpace(band) ? "Select" : band);
                parameters.Add("p_from_date", Convert.ToDateTime(from_date));
                parameters.Add("p_to_date", Convert.ToDateTime(to_date));
                lst_details = con.Query<MedicalRDetails>("select * from mdcl_sp_mr_Settlement_report(:p_band,:p_from_date,:p_to_date)", parameters).ToList();
            }
            return lst_details;
        }
        public List<MedicalRDetails> GetSettlementSummaryReport(string report_status)
        {
            List<MedicalRDetails> lst_details = new List<MedicalRDetails>();
            using (NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString))
            {
                con.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("p_report_status", string.IsNullOrWhiteSpace(report_status) ? "Select" : report_status);
                lst_details = con.Query<MedicalRDetails>("select * from mdcl_sp_mr_settlement_summary_report(:p_report_status)", parameters).ToList();
            }
            return lst_details;
        }
        // download excel 
        public DataTable DownloadPaymentRecord(DateTime? from_date, DateTime? to_date, string band, int treatment_type, int doctor_id, string batch_no, string employeecode, string lot_no, string payment_status)
        {
            DataTable dt = new DataTable();
            List<MedicalRDetails> lst_details = new List<MedicalRDetails>();
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {
                    con.Open();
                    NpgsqlParameter[] parameters =
                   {
                    new NpgsqlParameter("p_band", string.IsNullOrWhiteSpace(band) ? (object)DBNull.Value : band),
                    new NpgsqlParameter("p_from_date", from_date == null ? (object)DBNull.Value : Convert.ToDateTime(from_date)),
                    new NpgsqlParameter("p_to_date", to_date == null ? (object)DBNull.Value : Convert.ToDateTime(to_date)),
                    new NpgsqlParameter("p_treatment_type", treatment_type == 0 ? (object)DBNull.Value : treatment_type),
                    new NpgsqlParameter("p_doctor_id", doctor_id == 0 ? (object)DBNull.Value : doctor_id),
                    new NpgsqlParameter("p_batch_no", string.IsNullOrWhiteSpace(batch_no) ? (object)DBNull.Value : batch_no),
                    new NpgsqlParameter("p_employee_code", string.IsNullOrWhiteSpace(employeecode) ? (object)DBNull.Value : employeecode),
                    new NpgsqlParameter("p_lot_no", string.IsNullOrWhiteSpace(lot_no) ? (object)DBNull.Value : lot_no),
                    new NpgsqlParameter("p_payment_status", string.IsNullOrWhiteSpace(payment_status) ? (object)DBNull.Value : payment_status)
               };
                    dt = ExecuteProcedure("select * from mdcl_sp_mr_payment_report_excel(:p_band,:p_from_date,:p_to_date,:p_treatment_type,:p_doctor_id,:p_batch_no,:p_employee_code,:p_lot_no,:p_payment_status)", parameters);
                    //dt = ExecuteProcedure("select * from mdcl_sp_mr_payment_report_excel(:p_band,:p_from_date,:p_to_date,:p_treatment_type)", parameters);
                }
            }
            catch (Exception ex)
            {
                CommonHelper.write_log("error in DataTable DownloadPaymentRecord() >" + ex.Message);
            }
            return dt;
        }
        public DataTable DownloadBatchSummaryReport(int doctor_id, int batch_id, string payment_status)
        {
            DataTable dt = new DataTable();
            NpgsqlParameter[] parameters =
            {
                new NpgsqlParameter("p_doctor_id", string.IsNullOrWhiteSpace(doctor_id.ToString()) ? 0 : doctor_id),
                new NpgsqlParameter("p_batch_id", string.IsNullOrWhiteSpace(batch_id.ToString()) ? 0 : batch_id),
                new NpgsqlParameter("p_payment_status", string.IsNullOrWhiteSpace(payment_status) ? "Select" : payment_status)
            };
            dt = ExecuteProcedure("select * from mdcl_sp_mr_batch_summary_report_excel(:p_doctor_id,:p_batch_id,:p_payment_status)", parameters);
            return dt;
        }
        public DataTable DownloadEmployeeWisePaymentReport(string employeecode, DateTime from_date, DateTime to_date)
        {
            DataTable dt = new DataTable();
            NpgsqlParameter[] parameters =
            {
                new NpgsqlParameter("p_employeecode", string.IsNullOrWhiteSpace(employeecode) ? "Select" : employeecode),
                new NpgsqlParameter("p_from_date", Convert.ToDateTime(from_date)),
                new NpgsqlParameter("p_to_date", Convert.ToDateTime(to_date))
            };
            dt = ExecuteProcedure("select * from mdcl_sp_mr_employee_wise_payment_report_excel(:p_employeecode,:p_from_date,:p_to_date)", parameters);
            return dt;
        }
        public DataTable DownloadSettelmentReport(string band, DateTime from_date, DateTime to_date)
        {
            DataTable dt = new DataTable();
            NpgsqlParameter[] parameters =
            {
                new NpgsqlParameter("p_band", string.IsNullOrWhiteSpace(band) ? "Select" : band),
                new NpgsqlParameter("p_from_date", Convert.ToDateTime(from_date)),
                new NpgsqlParameter("p_to_date", Convert.ToDateTime(to_date))
            };
            dt = ExecuteProcedure("select * from mdcl_sp_mr_Settlement_report_excel(:p_band,:p_from_date,:p_to_date)", parameters);
            return dt;
        }
        public DataTable DownloadSettelmentSummaryReport(string report_status)
        {
            DataTable dt = new DataTable();
            NpgsqlParameter[] parameters =
            {
                new NpgsqlParameter("p_report_status", string.IsNullOrWhiteSpace(report_status) ? "Select" : report_status),
            };
            dt = ExecuteProcedure("select * from mdcl_sp_mr_settlement_summary_report_excel(:p_report_status)", parameters);
            return dt;
        }
        public static DataTable ExecuteProcedure(string procedure_name, NpgsqlParameter[] parameters)
        {
            DataTable dt = new DataTable();
            using (NpgsqlConnection conn = new NpgsqlConnection(CommonHelper.GetConnectionString))
            {
                try
                {
                    conn.Open();
                    using (NpgsqlCommand cmd = new NpgsqlCommand(procedure_name, conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        if (parameters != null)
                            cmd.Parameters.AddRange(parameters);
                        using (NpgsqlDataAdapter SqDA = new NpgsqlDataAdapter(cmd))
                        {
                            SqDA.Fill(dt);
                        }
                    }
                    return dt;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

    }
}