using Dapper;
using DocumentFormat.OpenXml.Office.CustomUI;
using MedicalR.CustomHelper;
using MedicalR.Models;
using MedicalR.Models.MedicalBillsException;
using MedicalR.Models.MedicalR;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using static MedicalR.CustomHelper.EmailHelper;

namespace MedicalR.DataAccessLayer.DAL.MedicalBillsException
{
    public class DALHospitalizationDHRD
    {
        public HospitilizationDHRD GetItem(int id)
        {
            HospitilizationDHRD item = new HospitilizationDHRD();
            try
            {
                if (id > 0)
                {
                    using (NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                    {
                        con.Open();
                        DynamicParameters parameters = new DynamicParameters();
                        parameters.Add("p_id", id);
                        item = con.Query<HospitilizationDHRD>("select * from mdcl_ex_hospitalization_dhrd_item_byid(:p_id)", parameters).FirstOrDefault();


                    }
                }

            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
            }
            return item;
        }

        public HospitilizationDHRD GetEmployeeDetailsByEmpl_Code(string empl_code)
        {
            HospitilizationDHRD item = new HospitilizationDHRD();
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {
                    con.Open();
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("p_employeecode", empl_code);
                    item = con.Query<HospitilizationDHRD>("select * from mdcl_ex_sp_get_emp_details_by_emplcode(:p_employeecode)", parameters).FirstOrDefault();
                }

            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
            }
            return item;
        }

        public string GetApplicationNo(NpgsqlConnection con)
        {
            string appl_no = string.Empty;
            appl_no = con.Query<string>("select * from mdcl_ex_dhrd_sp_getlast_applnos()").FirstOrDefault();
            if (string.IsNullOrWhiteSpace(appl_no))
            {
                appl_no = "HR-1";
            }
            else
            {
                string[] last_appl_no = appl_no.Split('-');
                if (last_appl_no.Length > 0)
                {
                    appl_no = "HR-" + (Convert.ToInt32(last_appl_no[1]) + 1);
                }
            }

            return appl_no;
        }
        public List<HospitilizationDHRD> GetHospitalGridData()
        {
            List<HospitilizationDHRD> item_list = new List<HospitilizationDHRD>();
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {
                    con.Open();
                    item_list = con.Query<HospitilizationDHRD>("select * from mdcl_ex_hospitalization_dhrd_grid_data()").ToList();
                }

            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
            }
            return item_list;
        }

        public ResponseModel HospitalDHRDSave(HospitilizationDHRD objModel)
        {
            NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString);
            ResponseModel Response = new ResponseModel();
            DataTable dt = new DataTable();

            try
            {
                using (con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {
                    con.Open();
                    using (NpgsqlCommand cmd = new NpgsqlCommand("select * from mdcl_sp_insert_hospitalization_dhrd(:p_id,:p_appln_no,:p_band_level,:p_patient_id,:p_nature_of_illness,:p_treatment_type,:p_from_date,:p_to_date,:p_name_of_hospital,:p_bill_no,:p_bill_date,:p_wheather_hospital_awarded_i_t_rebate,:p_medical_claim_forwd_dhrd_inrs,:p_amount_sanction_by_tmo_inrs,:p_balance_amount,:p_send_date_to_tmo,:p_received_date_from_tmo,:p_status,:p_emplid)", con))
                    {
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("p_id", objModel.id);
                        cmd.Parameters.AddWithValue("p_appln_no", GetApplicationNo(con));
                        cmd.Parameters.AddWithValue("p_band_level", string.IsNullOrWhiteSpace(objModel.band_level) ? "" : objModel.band_level);
                        cmd.Parameters.AddWithValue("p_patient_id", objModel.patient_id);
                        cmd.Parameters.AddWithValue("p_nature_of_illness", string.IsNullOrWhiteSpace(objModel.nature_of_illness) ? "" : objModel.nature_of_illness);
                        cmd.Parameters.AddWithValue("p_treatment_type", string.IsNullOrWhiteSpace(objModel.treatment_type) ? "" : objModel.treatment_type);
                        cmd.Parameters.AddWithValue("p_from_date", objModel.from_date);
                        cmd.Parameters.AddWithValue("p_to_date", objModel.to_date);
                        cmd.Parameters.AddWithValue("p_name_of_hospital", string.IsNullOrWhiteSpace(objModel.name_of_hospital) ? "" : objModel.name_of_hospital);
                        cmd.Parameters.AddWithValue("p_bill_no", string.IsNullOrWhiteSpace(objModel.bill_no) ? "" : objModel.bill_no);
                        cmd.Parameters.AddWithValue("p_bill_date", objModel.bill_date);
                        cmd.Parameters.AddWithValue("p_wheather_hospital_awarded_i_t_rebate", string.IsNullOrWhiteSpace(objModel.wheather_hospital_awarded_i_t_rebate) ? "" : objModel.wheather_hospital_awarded_i_t_rebate);
                        cmd.Parameters.AddWithValue("p_medical_claim_forwd_dhrd_inrs", string.IsNullOrWhiteSpace(objModel.medical_claim_forwd_dhrd_inrs.ToString()) ? 0 : objModel.medical_claim_forwd_dhrd_inrs);
                        cmd.Parameters.AddWithValue("p_amount_sanction_by_tmo_inrs", string.IsNullOrWhiteSpace(objModel.amount_sanction_by_tmo_inrs.ToString()) ? 0 : objModel.amount_sanction_by_tmo_inrs);
                        cmd.Parameters.AddWithValue("p_balance_amount", string.IsNullOrWhiteSpace(objModel.balance_amount.ToString()) ? 0 : objModel.balance_amount);
                        cmd.Parameters.AddWithValue("p_send_date_to_tmo", objModel.send_date_to_tmo);
                        cmd.Parameters.AddWithValue("p_received_date_from_tmo", objModel.received_date_from_tmo);
                        cmd.Parameters.AddWithValue("p_status", string.IsNullOrWhiteSpace(objModel.status) ? "" : objModel.status);
                        cmd.Parameters.AddWithValue("p_emplid", objModel.employee_id);

                        int Res = (cmd.ExecuteNonQuery());

                        if (Res == 0)
                        {
                            Response.Status = false;
                            Response.Message = MessageHelper.ErroeMsg;
                        }
                        else
                        {
                            Response.Status = true;
                            Response.Message = MessageHelper.RequestStatus;

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                Response.Status = false;
                Response.Message = MessageHelper.ExceptionMessage;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return Response;
        }


        public string PreparePrintDocFor_ExOfficeNote(int id)
        {
            string res = string.Empty;
            StringBuilder sb = new StringBuilder();
            List<DownloadBankAdviseOfficeNoteModel> ack_list = new List<DownloadBankAdviseOfficeNoteModel>();
            DataTable dt = new DataTable();
            decimal Total_claimed_amt = 0;
            decimal Total_sanc_amt = 0;

            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(CustomHelper.CommonHelper.GetConnectionString))
                {
                    con.Open();
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("p_id", id);
                    ack_list = con.Query<DownloadBankAdviseOfficeNoteModel>("select * from mdcl_sp_exception_dhrd_office_note_download(:p_id)", parameters).ToList();
                    if (ack_list.Count > 0)
                    {
                        foreach (DownloadBankAdviseOfficeNoteModel item in ack_list)
                        {
                            using (NpgsqlCommand cmd = new NpgsqlCommand("select * from  mdcl_sp_update_dhrd_exception_officenote_status(:p_id)", con))
                            {
                                cmd.Connection = con;
                                cmd.Parameters.AddWithValue("p_id", item.id);
                                object Res2 = cmd.ExecuteScalar();
                                int Res = Convert.ToInt32(Res2);

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }

            using (StreamReader sr = new StreamReader(CommonHelper.Html_Template_Dir + "\\" + "DHRD_MedicalRExOffice_Note.html"))
            {

                String line;

                while ((line = sr.ReadLine()) != null && ack_list.Count > 0)
                {
                    line = line.Replace("#Date", DateTime.Now.ToString("dd/MM/yyyy"));

                    if (line.Trim() == "<tbody id=\"Exception\">")
                    {
                        int srno = 0;
                        foreach (DownloadBankAdviseOfficeNoteModel batch in ack_list)
                        {
                            string tr_line = string.Empty;
                            tr_line = "<tr><td style='border: 1px solid black; border-collapse: collapse;' align='center'>" + (srno + 1).ToString() + "</td><td style='border: 1px solid black; border-collapse: collapse;' align='center'>" + batch.employeecode + "</td><td style='border: 1px solid black; border-collapse: collapse;' align='center'>" + batch.employeename + "</td><td style='border: 1px solid black; border-collapse: collapse;' align='center'>" + batch.appln_no + "</td><td style='border: 1px solid black; border-collapse: collapse;' align='right'>" + batch.total_claimed_amt.ToString("##,##,###.00") + "</td><td style='border: 1px solid black; border-collapse: collapse;' align='right'>" + batch.amt_sanc_tms.ToString("##,##,###.00") + "</td><td style='border: 1px solid black; border-collapse: collapse;' align='right'>" + batch.amt_sanc_tms.ToString("##,##,###.00") + "</td></tr>";
                            sb.AppendLine(tr_line);
                            srno++;
                            Total_claimed_amt = Total_claimed_amt + Convert.ToDecimal(batch.total_claimed_amt);
                            Total_sanc_amt = Total_sanc_amt + Convert.ToDecimal(batch.amt_sanc_tms);
                        }
                    }
                    line = line.Replace("#TotalSanc", Total_sanc_amt.ToString("##,##,###.00"));
                    sb.AppendLine(line);
                }


            }
            return sb.ToString();
        }
        public string PreparePrintDocFor_ExBankAdvice(int id)
        {
            string res = string.Empty;
            StringBuilder sb = new StringBuilder();
            List<DownloadBankAdviseOfficeNoteModel> ack_list = new List<DownloadBankAdviseOfficeNoteModel>();
            DataTable dt = new DataTable();

            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(CustomHelper.CommonHelper.GetConnectionString))
                {
                    con.Open();
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("p_id", id);
                    ack_list = con.Query<DownloadBankAdviseOfficeNoteModel>("select * from mdcl_sp_exception_dhrd_bank_advice_download(:p_id)", parameters).ToList();

                    if (ack_list.Count > 0)
                    {

                        using (NpgsqlCommand cmd = new NpgsqlCommand("select * from  mdcl_sp_update_exception_dhrd_bankadvise_status()", con))
                        {
                            cmd.Connection = con;
                            object Res2 = cmd.ExecuteScalar();
                            int Res = Convert.ToInt32(Res2);

                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }

            using (StreamReader sr = new StreamReader(CommonHelper.Html_Template_Dir + "\\" + "DHRD_BankAdvise_BRANCH.html"))
            {

                int rownumber = 0;
                decimal total_amount = 0;
                string str = total_amount.ToString();
                String line;
                foreach (DownloadBankAdviseOfficeNoteModel model in ack_list)
                {
                    // rownumber++;
                    string val_amt = model.amount_sanction_by_tmo_inrs.ToString().Replace('$', ' ').TrimEnd();
                    total_amount += Convert.ToDecimal(val_amt);
                }
                while ((line = sr.ReadLine()) != null)
                {
                    line = line.Replace("#listDate", DateTime.Now.ToString("dd-MMM-yyyy"));
                    // line = line.Replace("#lot_no", lotn);
                    //  line = line.Replace("#Amount", str);
                    line = line.Replace("#Amount", total_amount.ToString("##,##,###.00"));
                    if ((line.Trim() == ("<tbody>") && rownumber <= dt.Rows.Count))
                    {

                        foreach (DownloadBankAdviseOfficeNoteModel model in ack_list)
                        {
                            rownumber++;
                            string tempstr = (string.IsNullOrWhiteSpace(model.bank_name) ? "" : model.bank_name.ToString()) +
                                (string.IsNullOrWhiteSpace(model.account_no) ? " " : " AC NO:" + model.account_no.ToString()) +
                                (string.IsNullOrWhiteSpace(model.ifsc_code) ? " " : " IFSC CODE:" + model.ifsc_code.ToString());

                            string tr_line = "<tbody>" + "<tr><td>" + rownumber + "</td><td>" + model.employeecode + "</td><td>" + model.employeename + "</td><td>" + model.amount_sanction_by_tmo_inrs.ToString("##,##,###.00") + "</td><td>" + tempstr + "</td></tr>";
                            sb.AppendLine(tr_line);
                        }

                    }

                    else if (rownumber == ack_list.Count)
                    {
                        string tr_line = "<tr><td></td><td></td><td><center>Total</center></td><td><b>" + total_amount.ToString("##,##,###.00") + "</b></td><td></td></tr>";
                        sb.AppendLine(tr_line);
                        sb.AppendLine(line);
                        rownumber++;
                    }
                    else
                    {
                        sb.AppendLine(line);
                    }

                }
            }
            return sb.ToString();
        }
    }
}