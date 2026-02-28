using Dapper;
using MedicalR.CustomHelper;
using MedicalR.DataAccessLayer.IDAL.MedicalR;
using MedicalR.Models;
using MedicalR.Models.MedicalR;
using Npgsql;
//using Spire.Pdf.Exporting.XPS.Schema;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace MedicalR.DataAccessLayer.DAL.MedicalR
{
    public class DALQueryModule
    {
        public List<SanctionMedicalBillsModel> GetQMGridData(string batch_no, string appl_no, string empl_code, string lot_no, string query_type)
        {

            List<SanctionMedicalBillsModel> Addlist = new List<SanctionMedicalBillsModel>();
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {
                    con.Open();
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("p_batch_no", string.IsNullOrWhiteSpace(batch_no) ? "" : batch_no);
                    parameters.Add("p_appl_no", string.IsNullOrWhiteSpace(appl_no) ? "" : appl_no);
                    parameters.Add("p_empl_code", string.IsNullOrWhiteSpace(empl_code) ? "" : empl_code);
                    parameters.Add("p_lot_no", string.IsNullOrWhiteSpace(lot_no) ? "" : lot_no);
                    Addlist = con.Query<SanctionMedicalBillsModel>("select * from mdcl_sp_get_mr_query_module_grid_data(:p_batch_no,:p_appl_no,:p_empl_code,:p_lot_no)", parameters).ToList();
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                Addlist = new List<SanctionMedicalBillsModel>();
            }
            return Addlist;


        }

        public string PreparePrintDoc(string batch_no, string lot_no, string query_type)
        {
            string res = string.Empty;
            StringBuilder sb = new StringBuilder();
            List<SanctionMedicalBillsModel> ack_list = new List<SanctionMedicalBillsModel>();
            List<SanctionMedicalBillsModel> ack_list_ann = new List<SanctionMedicalBillsModel>();
            List<BatchCreationModel> ack_list_batch = new List<BatchCreationModel>();

            DataTable dt = new DataTable();
            decimal Total_claimed_rs = 0;
            decimal Total_sanctioned_rs = 0;
            decimal Total_application_count = 0;
            decimal Total_application_count_sanctioned = 0;
            decimal Total_objection = 0;
            decimal Total_appln_objection = 0;
            string FileName = string.Empty;
            decimal Total = 0;
            string tratement_type = string.Empty;
            string Batch_date = string.Empty;
            int Trcount = 0;
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(CustomHelper.CommonHelper.GetConnectionString))
                {
                    con.Open();
                    if (query_type == "Office Notes")
                    {
                        DynamicParameters parameters = new DynamicParameters();
                        // parameters.Add("p_batch_no", string.IsNullOrWhiteSpace(batch_no) ? "" : batch_no);
                        // parameters.Add("p_appl_no", string.IsNullOrWhiteSpace(appl_no) ? "" : appl_no);
                        //  parameters.Add("p_empl_code", string.IsNullOrWhiteSpace(empl_code) ? "" : empl_code);
                        parameters.Add("p_lot_no", string.IsNullOrWhiteSpace(lot_no) ? "" : lot_no);
                        ack_list = con.Query<SanctionMedicalBillsModel>("select * from mdcl_sp_get_mr_office_note_summary(:p_lot_no)", parameters).ToList();
                        ack_list_ann = con.Query<SanctionMedicalBillsModel>("select * from mdcl_sp_get_mr_office_note_annexure(:p_lot_no)", parameters).ToList();
                        Total = 0;
                        foreach (SanctionMedicalBillsModel batch in ack_list_ann)
                        {
                            Total = Total + batch.total_sanctioned_rs;
                        }
                    }
                    else if (query_type == "Bank Advise")
                    {
                        DynamicParameters parameters = new DynamicParameters();
                        parameters.Add("p_lot_no", string.IsNullOrWhiteSpace(lot_no) ? "" : lot_no);
                        ack_list = con.Query<SanctionMedicalBillsModel>("select * from mdcl_sp_get_mr_bank_advise_summary(:p_lot_no)", parameters).ToList();
                        ack_list_ann = con.Query<SanctionMedicalBillsModel>("select * from mdcl_sp_get_mr_bank_advise_annexure(:p_lot_no)", parameters).ToList();
                        Total = 0;
                        foreach (SanctionMedicalBillsModel batch in ack_list_ann)
                        {
                            Total = Total + batch.total_sanctioned_rs;
                        }

                    }
                    else if (query_type == "Batch Print")
                    {
                        DynamicParameters parameters = new DynamicParameters();
                        parameters.Add("p_batch_no", batch_no);
                        ack_list_batch = con.Query<BatchCreationModel>("select * from mdcl_sp_get_batchitems_grid_data_for_print(:p_batch_no)", parameters).ToList();

                        Trcount = ack_list.Count;
                        foreach (BatchCreationModel batch in ack_list_batch)
                        {
                            Total = Total + batch.total_claimed_rs;
                            tratement_type = batch.treatment_type;
                            Batch_date = batch.batch_created_date.ToString("dd/MM/yyyy");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
            }
            if (query_type == "Office Notes")
            {
                FileName = "MedicalROffice_Note.html";
            }
            else if (query_type == "Bank Advise")
            {
                FileName = "MedicalRBank_Advise.html";
            }
            else if (query_type == "Batch Print")
            {
                FileName = "BatchDetail.html";
            }
            else
            {
                FileName = "Default.html";
            }
            using (StreamReader sr = new StreamReader(CommonHelper.Html_Template_Dir + "\\" + FileName))
            {

                String line;
                if (query_type == "Office Notes")
                {
                    string strFy = Helper.GetCurrentFY();

                    while ((line = sr.ReadLine()) != null && ack_list.Count > 0)
                    {
                        line = line.Replace("#FY", strFy);
                        line = line.Replace("#lotno", lot_no);
                        line = line.Replace("#Date", ack_list[0].lot_generate_date?.ToString("dd/MM/yyyy"));
                        line = line.Replace("#BatchNo", batch_no.ToString());
                        line = line.Replace("#Amount", Total.ToString("##,##,###.00"));
                        line = line.Replace("#TotalClaim", Total_claimed_rs.ToString("##,##,###.00"));
                        line = line.Replace("#TotalApp", Total_application_count.ToString());
                        line = line.Replace("#TotalSan", Total.ToString("##,##,###.00"));
                        line = line.Replace("#applnsan", Total_application_count_sanctioned.ToString());
                        line = line.Replace("#object", Total_objection.ToString());
                        line = line.Replace("#applnObj", Total_appln_objection.ToString());
                        if (line.Trim() == "<tbody id=\"batchwise\">")
                        {
                            int srno = 0;
                            foreach (SanctionMedicalBillsModel batch in ack_list)
                            {
                                string tr_line = string.Empty;
                                tr_line = "<tr><td style='border: 1px solid black; border-collapse: collapse;' align='center'>" + (srno + 1).ToString() + "</td><td style='border: 1px solid black; border-collapse: collapse;' align='center'>" + batch.batch_no + "</td><td style='border: 1px solid black; border-collapse: collapse;' align='center'>" + batch.status + "</td><td style='border: 1px solid black; border-collapse: collapse;' align='center'>" + batch.application_count + "</td><td style='border: 1px solid black; border-collapse: collapse;' align='right'>" + Convert.ToDecimal(batch.total_claimed_rs).ToString("##,##,###.00") + "</td><td style='border: 1px solid black; border-collapse: collapse;' align='right'>" + batch.total_sanctioned_rs.ToString("##,##,###.00") + "</td></tr>";
                                sb.AppendLine(tr_line);
                                srno++;
                                Total_claimed_rs = Total_claimed_rs + Convert.ToDecimal(batch.total_claimed_rs);
                                Total_sanctioned_rs = batch.total_sanctioned_rs_lotwise;

                                Total_application_count = Total_application_count + batch.application_count;
                                CommonHelper.write_log("batch status :" + batch.status);
                                if (batch.status == "Sanctioned")
                                {
                                    Total_application_count_sanctioned = Total_application_count_sanctioned + batch.total_sanctioned_count;
                                }
                                if (batch.status == "Objection")
                                {
                                    //Total_appln_objection = Total_appln_objection + batch.total_objection_count;
                                    Total_appln_objection = batch.total_objection_count;
                                    Total_objection = batch.total_objection_amt;
                                }

                            }
                        }
                        if (line.Trim() == "<tbody id=\"annex\">")
                        {
                            int srno = 0;
                            foreach (SanctionMedicalBillsModel batch in ack_list_ann)
                            {
                                if (batch.is_sanctioned)
                                {
                                    string tr_line = string.Empty;
                                    tr_line = "<tr><td style='border: 1px solid black; border-collapse: collapse;' align='center'>" + (srno + 1).ToString() + "</td><td style='border: 1px solid black; border-collapse: collapse;' align='center'>" + batch.employeecode + "</td><td style='border: 1px solid black; border-collapse: collapse;' align='center'>" + batch.employeename + "</td><td style='border: 1px solid black; border-collapse: collapse;' align='center'>" + batch.batch_no + "</td><td style='border: 1px solid black; border-collapse: collapse;' align='center'>" + batch.appln_no + "</td><td style='border: 1px solid black; border-collapse: collapse;' align='right'>" + Convert.ToDecimal(batch.total_claimed_rs).ToString("##,##,###.00") + "</td><td style='border: 1px solid black; border-collapse: collapse;' align='right'>" + batch.total_sanctioned_rs.ToString("##,##,###.00") + "</td></tr>";
                                    sb.AppendLine(tr_line);
                                    srno++;
                                }
                            }
                        }
                        if (line.Trim() == "<tbody id=\"annex_objection\">")
                        {
                            int srno = 0;
                            foreach (SanctionMedicalBillsModel batch in ack_list_ann)
                            {
                                if (batch.is_objection)
                                {
                                    string tr_line = string.Empty;
                                    tr_line = "<tr><td style='border: 1px solid black; border-collapse: collapse;' align='center'>" + (srno + 1).ToString() + "</td><td style='border: 1px solid black; border-collapse: collapse;' align='center'>" + batch.employeecode + "</td><td style='border: 1px solid black; border-collapse: collapse;' align='center'>" + batch.employeename + "</td><td style='border: 1px solid black; border-collapse: collapse;' align='center'>" + batch.batch_no + "</td><td style='border: 1px solid black; border-collapse: collapse;' align='center'>" + batch.appln_no + "</td><td style='border: 1px solid black; border-collapse: collapse;' align='right'>" + Convert.ToDecimal(batch.total_claimed_rs).ToString("##,##,###.00") + "</td><td style='border: 1px solid black; border-collapse: collapse;' align='right'>" + batch.total_sanctioned_rs.ToString("##,##,###.00") + "</td></tr>";
                                    sb.AppendLine(tr_line);
                                    srno++;
                                }
                            }
                        }
                        sb.AppendLine(line);
                    }
                }
                else if (query_type == "Bank Advise")
                {
                    string strFy = Helper.GetCurrentFY();
                    while ((line = sr.ReadLine()) != null && ack_list.Count > 0)
                    {
                        //line = line.Replace("#Date", DateTime.Now.ToString("dd/MM/yyyy"));
                        line = line.Replace("#Date", ack_list[0].lot_generate_date?.ToString("dd/MM/yyyy"));
                        line = line.Replace("#FY", strFy);
                        line = line.Replace("#Amount", Total.ToString("##,##,###.00"));
                        line = line.Replace("#LotNo", lot_no.ToString());
                        //line = line.Replace("#TotalApp", Total_application_count.ToString());
                        line = line.Replace("#TotalSan", Total_sanctioned_rs.ToString("##,##,###.00"));
                        //line = line.Replace("#applnsan", Total_application_count_sanctioned.ToString());
                        //line = line.Replace("#object", Total_objection.ToString());
                        //line = line.Replace("#applnObj", Total_appln_objection.ToString());
                        if (line.Trim() == "<tbody id=\"bankad\">")
                        {
                            int srno = 0;
                            foreach (SanctionMedicalBillsModel batch in ack_list)
                            {
                                string tr_line = string.Empty;
                                tr_line = "<tr><td style='border: 1px solid black; border-collapse: collapse;' align='center'>" + batch.batch_no + "</td><td style='border: 1px solid black; border-collapse: collapse;' align='center'>" + batch.empl_class + "</td><td style='border: 1px solid black; border-collapse: collapse;' align='right'>" + batch.total_sanctioned_rs.ToString("##,##,###.00") + "</td></tr>";
                                sb.AppendLine(tr_line);
                                srno++;
                                // Total_claimed_rs = Total_claimed_rs + Convert.ToDecimal(batch.total_claimed_rs);
                                Total_sanctioned_rs = Total_sanctioned_rs + Convert.ToDecimal(batch.total_sanctioned_rs);
                                // Total_application_count = Total_application_count + batch.application_count;
                                // Total_application_count_sanctioned = Total_application_count_sanctioned + batch.application_count;
                            }
                        }
                        if (line.Trim() == "<tbody id=\"annexba\">")
                        {
                            int srno = 0;
                            foreach (SanctionMedicalBillsModel batch in ack_list_ann)
                            {
                                string tr_line = string.Empty;
                                tr_line = "<tr><td style='border: 1px solid black; border-collapse: collapse;' align='center'>" + (srno + 1).ToString() + "</td><td style='border: 1px solid black; border-collapse: collapse;' align='center'>" + batch.employeecode + "</td><td style='border: 1px solid black; border-collapse: collapse;' align='center'>" + batch.employeename + "</td><td style='border: 1px solid black; border-collapse: collapse;' align='center'>" + batch.account_no + "</td><td style='border: 1px solid black; border-collapse: collapse;' align='right'>" + batch.total_sanctioned_rs.ToString("##,##,###.00") + "</td></tr>";
                                sb.AppendLine(tr_line);
                                srno++;
                                Total = Total + Convert.ToDecimal(batch.total_sanctioned_rs);
                            }
                        }
                        sb.AppendLine(line);
                    }
                }
                else if (query_type == "Batch Print")
                {
                    decimal total_amt = 0;
                    while ((line = sr.ReadLine()) != null && ack_list_batch.Count > 0)
                    {
                        line = line.Replace("#BatchNo", batch_no.ToString());
                        line = line.Replace("#Total", Total.ToString());
                        line = line.Replace("#Count", Trcount.ToString());
                        line = line.Replace("#TreatMentType", ack_list_batch[0].treatment_type.ToString());
                        line = line.Replace("#BatchDate", Batch_date);
                        int n;
                        bool isNumeric = int.TryParse(ack_list_batch[0].band, out n);


                        if (isNumeric)
                        {
                            if (Convert.ToInt32(ack_list_batch[0].band) > 0)
                            {
                                line = line.Replace("#BAND_CLASS", "Officer");
                            }
                            else
                            {
                                line = line.Replace("#BAND_CLASS", "Non Officer");
                            }
                        }
                        else
                        {
                            line = line.Replace("#BAND_CLASS", "Non Officer");
                        }
                        // line = line.Replace("#Count", application_count.ToString());
                        // line = line.Replace("#total", Total + Convert.ToDecimal(bll.total_claimed_rs).ToString("##,##,###.00"));

                        if (line.Trim() == "<tbody>")
                        {
                            int srno = 0;

                            foreach (BatchCreationModel batch in ack_list_batch)
                            {
                                string tr_line = string.Empty;
                                tr_line = "<tr><td style='border: 1px solid black; border-collapse: collapse;' align='center'>" + (srno + 1).ToString() + "</td><td style='border: 1px solid black; border-collapse: collapse;' align='center'>" + batch.employeecode + "</td><td style='border: 1px solid black; border-collapse: collapse;' align='center'>" + batch.employeename + "</td><td style='border: 1px solid black; border-collapse: collapse;' align='center'>" + batch.appln_no + "</td><td style='border: 1px solid black; border-collapse: collapse;' align='center'>" + batch.patient_name + "</td><td style='border: 1px solid black; border-collapse: collapse;' align='right'>" + batch.total_claimed_rs.ToString("##,##,###.00") + "</td></tr>";
                                sb.AppendLine(tr_line);
                                srno++;
                                total_amt += batch.total_claimed_rs;
                            }

                        }
                        line = line.Replace("#{total_amt}", total_amt.ToString("##,##,###.00"));
                        sb.AppendLine(line);
                    }
                }
                else
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        sb.AppendLine(line);
                    }
                }
            }
            return sb.ToString();
        }
        public DataTable PrepareExcelDoc(string batch_no, string lot_no, string query_type)
        {
            DataTable dt = new DataTable();
            try
            {


                using (NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {
                    using (NpgsqlCommand cmd = new NpgsqlCommand("select * from mdcl_sp_get_mr_bank_advise_annexure(:p_lot_no)", con))
                    {
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("p_lot_no", string.IsNullOrWhiteSpace(lot_no) ? "" : lot_no);
                        using (NpgsqlDataAdapter SqDA = new NpgsqlDataAdapter(cmd))
                        {
                            SqDA.Fill(dt);
                        }
                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                dt = null;
                return dt;
            }

        }
        public DataTable PrepareExcelDocForSAP(string batch_no, string lot_no, string query_type)
        {
            DataTable dt = new DataTable();
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {
                    using (NpgsqlCommand cmd = new NpgsqlCommand("select * from mdcl_sp_get_payment_details_for_sap_excel(:p_lot_no)", con))
                    {
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("p_lot_no", string.IsNullOrWhiteSpace(lot_no) ? "" : lot_no);
                        using (NpgsqlDataAdapter SqDA = new NpgsqlDataAdapter(cmd))
                        {
                            SqDA.Fill(dt);
                        }
                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                dt = null;
                return dt;
            }

        }

        public DataTable PrepareExcelDocForHewettData(string batch_no, string lot_no, string query_type, string from_date, string to_date)
        {
            DataTable dt = new DataTable();
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {
                    con.Open();
                    using (NpgsqlCommand cmd = new NpgsqlCommand("select * from mdcl_sp_get_mr_hewitt_data_for_excel(:p_lot_no,:p_from_date,:p_to_date)", con))
                    {
                        cmd.Connection = con;
                        cmd.CommandTimeout = 180;
                        cmd.Parameters.AddWithValue("p_lot_no", string.IsNullOrWhiteSpace(lot_no) ? "" : lot_no);
                        cmd.Parameters.AddWithValue("p_from_date", string.IsNullOrWhiteSpace(from_date) ? "" : from_date);
                        cmd.Parameters.AddWithValue("p_to_date", string.IsNullOrWhiteSpace(to_date) ? "" : to_date);
                        using (NpgsqlDataAdapter SqDA = new NpgsqlDataAdapter(cmd))
                        {
                            SqDA.Fill(dt);
                        }
                    }
                    if (dt.Rows.Count > 0)
                    {
                        using (NpgsqlCommand cmd_log = new NpgsqlCommand("select * from mdcl_sp_insert_last_hewitt_download(:p_lot_no,:p_last_download_date)", con))
                        {
                            cmd_log.Connection = con;
                            cmd_log.Parameters.AddWithValue("p_lot_no", string.IsNullOrWhiteSpace(lot_no) ? "" : lot_no);
                            cmd_log.Parameters.AddWithValue("p_last_download_date", string.IsNullOrWhiteSpace(to_date) ? "" : to_date);
                            cmd_log.ExecuteNonQuery();
                        }

                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                dt = null;
                return dt;

            }

        }
    }
}