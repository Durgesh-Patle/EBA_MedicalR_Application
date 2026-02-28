using Dapper;
using MedicalR.CustomHelper;
using MedicalR.EmailSettings;
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
using EmailHelper = MedicalR.EmailSettings.EmailHelper;

namespace MedicalR.DataAccessLayer.DAL.MedicalBillsException
{
    public class DALHospitalizationFC
    {
        public HospitalizationFCModel GetItem(int id)
        {
            HospitalizationFCModel item = new HospitalizationFCModel();
            try
            {
                if (id > 0)
                {
                    using (NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                    {
                        con.Open();
                        DynamicParameters parameters = new DynamicParameters();
                        parameters.Add("p_id", id);
                        item = con.Query<HospitalizationFCModel>("select * from mdcl_sp_get_hospitalization_cf_byid(:p_id)", parameters).FirstOrDefault();
                    }
                }

            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
            }
            return item;
        }
        public ResponseModel SendLetterToEmpl_N_Hospital_PRE(HospitalizationFCModel model)
        {
            ResponseModel item = new ResponseModel();
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {
                    con.Open();
                    // save
                    string query_para = @" :p_id,:p_employee_id,:p_employee_code,:p_band,:p_patient_id,:p_nature_of_illness,:p_treatment_type,:p_from_date,:p_to_date
                                            ,:p_hospital_id,:p_bill_no,:p_bill_date,:p_is_rebate_certificate,:p_created_by,:p_appln_no,:p_send_letter_status";

                    DynamicParameters m_para = new DynamicParameters();
                    m_para.Add("p_id", model.id);
                    m_para.Add("p_employee_id", model.employee_id);
                    m_para.Add("p_employee_code", string.IsNullOrWhiteSpace(model.employee_code) ? "" : model.employee_code);
                    m_para.Add("p_band", string.IsNullOrWhiteSpace(model.band_level) ? "" : model.band_level);
                    m_para.Add("p_patient_id", model.patient_id);
                    m_para.Add("p_nature_of_illness", string.IsNullOrWhiteSpace(model.nature_of_illness) ? "" : model.nature_of_illness);
                    m_para.Add("p_treatment_type", string.IsNullOrWhiteSpace(model.treatment_type) ? "" : model.treatment_type);
                    m_para.Add("p_from_date", model.from_date);
                    m_para.Add("p_to_date", model.to_date);
                    m_para.Add("p_hospital_id", model.hospital_id);
                    m_para.Add("p_bill_no", string.IsNullOrWhiteSpace(model.bill_no) ? "" : model.bill_no);
                    m_para.Add("p_bill_date", model.bill_date);
                    m_para.Add("p_is_rebate_certificate", string.IsNullOrWhiteSpace(model.is_rebate_certificate) ? "" : model.is_rebate_certificate);
                    m_para.Add("p_created_by", UserManager.User.Employeecode);
                    if (model.id == 0)
                    {
                        m_para.Add("p_appln_no", GetApplicationNo(con));
                    }
                    else
                    {
                        m_para.Add("p_appln_no", string.IsNullOrWhiteSpace(model.appln_no) ? "" : model.appln_no);
                    }

                    m_para.Add("p_send_letter_status", string.IsNullOrWhiteSpace(model.send_letter_status) ? "" : model.send_letter_status);

                    ResponseData resp = con.Query<ResponseData>("select * from  mdcl_sp_insert_hospitalization_credit_fac_data_on_send_letter(" + query_para + ")", m_para).FirstOrDefault();
                    if (model.send_letter_status == "Yes")
                    {
                        item = SendLetterToEmpl_N_Hospital(resp.mdcl_sp_insert_hospitalization_credit_fac_data_on_send_letter, con, "PRE_LETTER");
                    }
                    item.Status = true;
                    item.Message = "Data save successfully!";
                }

            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                item.Status = false;
                item.Message = "error :" + ex.Message;
            }
            return item;
        }

        public HospitalizationFCModel GetEmployeeDetailsByEmpl_Code(string empl_code)
        {
            HospitalizationFCModel item = new HospitalizationFCModel();
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {
                    con.Open();
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("p_employeecode", empl_code);
                    item = con.Query<HospitalizationFCModel>("select * from mdcl_ex_sp_get_emp_details_by_emplcode(:p_employeecode)", parameters).FirstOrDefault();
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
            appl_no = con.Query<string>("select * from mdcl_ex_sp_getlast_applnos()").FirstOrDefault();
            if (string.IsNullOrWhiteSpace(appl_no))
            {
                appl_no = "CA-1";
            }
            else
            {
                string[] last_appl_no = appl_no.Split('-');
                if (last_appl_no.Length > 0)
                {
                    appl_no = "CA-" + (Convert.ToInt32(last_appl_no[1]) + 1);
                }
            }

            return appl_no;
        }
        public List<HospitalizationFCModel> GetHospitalGridData()
        {
            List<HospitalizationFCModel> item_list = new List<HospitalizationFCModel>();
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {
                    con.Open();
                    item_list = con.Query<HospitalizationFCModel>("select * from mdcl_ex_hospitalization_cf_grid_data()").ToList();
                }

            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
            }
            return item_list;
        }

        public ResponseModel SendLetterToEmpl_N_Hospital(int id, NpgsqlConnection con, string CHECK_POINT)
        {
            ResponseModel resp = new ResponseModel();
            HospitalizationFCModel item = new HospitalizationFCModel();
            EmailModel eml_model = new EmailModel();
            CF_SEND_LETTERMODEL PRE_ITEM = new CF_SEND_LETTERMODEL();
            try
            {
                switch (CHECK_POINT)
                {
                    case "PRE_LETTER":
                        {

                            DynamicParameters parameters = new DynamicParameters();
                            parameters.Add("p_id", id);
                            PRE_ITEM = con.Query<CF_SEND_LETTERMODEL>("select * from mdcl_sp_get_hospitalization_empl_details_byempid(:p_id)", parameters).FirstOrDefault();
                            eml_model.to_emailids = new List<string>();
                            eml_model.cc_emailids = new List<string>();
                            if (!string.IsNullOrWhiteSpace(PRE_ITEM.empl_email))
                            {
                                eml_model.to_emailids.Add(PRE_ITEM.empl_email);
                            }
                            if (!string.IsNullOrWhiteSpace(PRE_ITEM.hospi_email1))
                            {
                                eml_model.to_emailids.Add(PRE_ITEM.hospi_email1);
                            }
                            if (!string.IsNullOrWhiteSpace(PRE_ITEM.hospi_email2))
                            {
                                eml_model.to_emailids.Add(PRE_ITEM.hospi_email2);
                            }
                            if (!string.IsNullOrWhiteSpace(PRE_ITEM.cc_email_ids))
                            {
                                foreach (string str_cc_email in PRE_ITEM.cc_email_ids.Split(','))
                                {
                                    if (!string.IsNullOrWhiteSpace(str_cc_email))
                                    {
                                        eml_model.cc_emailids.Add(str_cc_email);
                                    }
                                }
                            }
                            eml_model.email_subject = PRE_ITEM.email_subject;
                            eml_model.email_body = PRE_ITEM.email_body;
                            string val_pre_letter = PRE_ITEM.cf_letter_template;

                            val_pre_letter = val_pre_letter.Replace("#{Hospital_Name}", PRE_ITEM.hospital_name);
                            val_pre_letter = val_pre_letter.Replace("#{Address}", PRE_ITEM.hosp_address);
                            val_pre_letter = val_pre_letter.Replace("#{City}", PRE_ITEM.hosp_city);
                            val_pre_letter = val_pre_letter.Replace("#{Pincode}", PRE_ITEM.hosp_pincode.ToString());
                            val_pre_letter = val_pre_letter.Replace("#{emp_name}", PRE_ITEM.employeename);
                            val_pre_letter = val_pre_letter.Replace("#{emp_designation}", PRE_ITEM.designation);
                            val_pre_letter = val_pre_letter.Replace("#{emp_code}", PRE_ITEM.employee_code);
                            val_pre_letter = val_pre_letter.Replace("#{patient_name}", PRE_ITEM.patient_name);
                            val_pre_letter = val_pre_letter.Replace("#{family_relation_code}", PRE_ITEM.family_relation_code);
                            val_pre_letter = val_pre_letter.Replace("#{nature_of_illness}", PRE_ITEM.nature_of_illness);
                            val_pre_letter = val_pre_letter.Replace("#{letter_date}", PRE_ITEM.letter_date.ToString("dd/MM/yyyy"));
                            eml_model.eml_attachemet = CommonHelper.Convert2(val_pre_letter);

                            break;
                        }
                    case "FINAL_LETTER":
                        {
                            DynamicParameters parameters = new DynamicParameters();
                            parameters.Add("p_id", id);
                            item = con.Query<HospitalizationFCModel>("select * from mdcl_ex_sp_get_detail_for_send_letter_toemp_n_hospi(:p_id)", parameters).FirstOrDefault();

                            eml_model.to_emailids = new List<string>();
                            eml_model.cc_emailids = new List<string>();
                            if (!string.IsNullOrWhiteSpace(item.empl_email))
                            {
                                eml_model.to_emailids.Add(item.empl_email);
                            }
                            if (!string.IsNullOrWhiteSpace(item.hospi_email1))
                            {
                                eml_model.to_emailids.Add(item.hospi_email1);
                            }
                            if (!string.IsNullOrWhiteSpace(item.hospi_email2))
                            {
                                eml_model.to_emailids.Add(item.hospi_email2);
                            }
                            if (!string.IsNullOrWhiteSpace(item.cc_email_ids))
                            {
                                foreach (string str_cc_email in item.cc_email_ids.Split(','))
                                {
                                    if (!string.IsNullOrWhiteSpace(str_cc_email))
                                    {
                                        eml_model.cc_emailids.Add(str_cc_email);
                                    }
                                }
                            }
                            eml_model.email_subject = item.email_subject;
                            eml_model.email_body = item.email_body;
                            string val_final_letter = item.cf_letter_template;

                            val_final_letter = val_final_letter.Replace("#{empl_name}", item.employeename);
                            val_final_letter = val_final_letter.Replace("#{ufc_name}", item.ufc_name);
                            val_final_letter = val_final_letter.Replace("#{amt_claimed_rs}", item.gross_bill_amt_rs.ToString());
                            val_final_letter = val_final_letter.Replace("#{amt_sanc_under_tms}", item.amt_approved_sanc_tms_rs.ToString());
                            val_final_letter = val_final_letter.Replace("#{amt_sanc_under_maf}", item.amt_approved_sanc_maf_rs.ToString());
                            val_final_letter = val_final_letter.Replace("#{amt_tobe_recover_frm_sal}", item.amt_tobe_recover_frm_empl.ToString());
                            val_final_letter = val_final_letter.Replace("#{installments}", item.recovery_installments_period.ToString());
                            val_final_letter = val_final_letter.Replace("#{letter_date}", item.letter_date.ToString("dd/MM/yyyy"));

                            eml_model.eml_attachemet = CommonHelper.Convert2(val_final_letter);
                            break;
                        }
                }


                if (eml_model.to_emailids.Count > 0)
                {
                    if (EmailHelper.SendEmail(eml_model, "CONFIRM"))
                    {
                        resp.Status = true;
                        resp.Message = "Letter sent successfully!";
                    }
                    else
                    {
                        resp.Status = false;
                        resp.Message = "failed to send mail";
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                resp.Status = false;
                resp.Message = "error :" + ex.Message;
            }
            return resp;
        }
        public ResponseModel Save(HospitalizationFCModel model)
        {

            NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString);
            ResponseModel Response = new ResponseModel();
            try
            {
                using (con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {
                    con.Open();

                    string query_para = @" :p_id,:p_gross_bill_amt_rs,:p_amt_paid_to_hospital_rs,:p_amt_approved_sanc_tms_rs
                                            ,:p_amt_approved_sanc_maf_rs,:p_addl_sanction_rs,:p_amt_tobe_recover_frm_empl,:p_recovery_installments_period,:p_send_date_to_tmo
                                            ,:p_received_date_from_tmo,:p_modify_by,:p_is_save_for_later,:p_bill_no,:p_bill_date";

                    DynamicParameters m_para = new DynamicParameters();
                    m_para.Add("p_id", model.id);
                    m_para.Add("p_gross_bill_amt_rs", model.gross_bill_amt_rs);
                    m_para.Add("p_amt_paid_to_hospital_rs", model.amt_paid_to_hospital_rs);
                    m_para.Add("p_amt_approved_sanc_tms_rs", model.amt_approved_sanc_tms_rs);
                    m_para.Add("p_amt_approved_sanc_maf_rs", model.amt_approved_sanc_maf_rs);
                    m_para.Add("p_addl_sanction_rs", model.addl_sanction_rs);
                    m_para.Add("p_amt_tobe_recover_frm_empl", model.amt_tobe_recover_frm_empl);
                    m_para.Add("p_recovery_installments_period", model.recovery_installments_period);
                    m_para.Add("p_send_date_to_tmo", model.send_date_to_tmo);
                    m_para.Add("p_received_date_from_tmo", model.received_date_from_tmo);
                    m_para.Add("p_modify_by", UserManager.User.Employeecode);
                    m_para.Add("p_is_save_for_later", model.is_save_for_later);
                    m_para.Add("p_bill_no", string.IsNullOrWhiteSpace(model.bill_no) ? "" : model.bill_no);
                    m_para.Add("p_bill_date", model.bill_date);

                    con.Query<ResponseData>("select * from  mdcl_sp_insert_hospitalization_credit_fac_final_update(" + query_para + ")", m_para).FirstOrDefault();
                    if (model.send_letter_status == "Yes" && !model.is_save_for_later)
                    {
                        Response = SendLetterToEmpl_N_Hospital(model.id, con, "FINAL_LETTER");
                    }
                    Response.Status = true;
                    Response.Message = "Data updated successfully !";
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                Response.Status = false;
                Response.Message = "Something went wrong !" + ex.Message;
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
                    ack_list = con.Query<DownloadBankAdviseOfficeNoteModel>("select * from mdcl_sp_Exception_Office_note_download(:p_id)", parameters).ToList();
                    if (ack_list.Count > 0)
                    {
                        foreach (DownloadBankAdviseOfficeNoteModel item in ack_list)
                        {
                            using (NpgsqlCommand cmd = new NpgsqlCommand("select * from  mdcl_sp_update_Exception_officeNote_status(:p_id)", con))
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

            using (StreamReader sr = new StreamReader(CommonHelper.Html_Template_Dir + "\\" + "MedicalRExOffice_Note.html"))
            {

                String line;

                while ((line = sr.ReadLine()) != null && ack_list.Count > 0)
                {
                    line = line.Replace("#Date", DateTime.Now.ToString("dd/MM/yyyy"));
                    line = line.Replace("#TotalSanc", Total_sanc_amt.ToString("##,##,###.00"));
                    if (line.Trim() == "<tbody id=\"Exception\">")
                    {
                        int srno = 0;
                        foreach (DownloadBankAdviseOfficeNoteModel batch in ack_list)
                        {
                            string tr_line = string.Empty;
                            tr_line = "<tr><td style='border: 1px solid black; border-collapse: collapse;' align='center'>" + (srno + 1).ToString() + "</td><td style='border: 1px solid black; border-collapse: collapse;' align='center'>" + batch.employeecode + "</td><td style='border: 1px solid black; border-collapse: collapse;' align='center'>" + batch.employeename + "</td><td style='border: 1px solid black; border-collapse: collapse;' align='center'>" + batch.appln_no + "</td><td style='border: 1px solid black; border-collapse: collapse;' align='right'>" + batch.total_claimed_amt.ToString("##,##,###.00") + "</td><td style='border: 1px solid black; border-collapse: collapse;' align='right'>" + batch.amt_sanc_tms.ToString("##,##,###.00") + "</td><td style='border: 1px solid black; border-collapse: collapse;' align='right'>" + batch.amt_sanc_maf.ToString("##,##,###.00") + "</td><td style='border: 1px solid black; border-collapse: collapse;' align='right'>" + batch.amt_sanc_tms.ToString("##,##,###.00") + "</td></tr>";
                            sb.AppendLine(tr_line);
                            srno++;
                            Total_claimed_amt = Total_claimed_amt + Convert.ToDecimal(batch.total_claimed_amt);
                            Total_sanc_amt = Total_sanc_amt + Convert.ToDecimal(batch.total_sanc);
                        }
                    }

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
                    ack_list = con.Query<DownloadBankAdviseOfficeNoteModel>("select * from mdcl_sp_exception_bank_advice_download(:p_id)", parameters).ToList();

                    if (ack_list.Count > 0)
                    {

                        using (NpgsqlCommand cmd = new NpgsqlCommand("select * from  mdcl_sp_update_exception_bankadvise_status(:p_id)", con))
                        {
                            cmd.Parameters.AddWithValue("p_id", id);
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
            using (StreamReader sr = new StreamReader(CommonHelper.Html_Template_Dir + "\\" + "ExceptioBankAdvise_MUMBAI.html"))
            {

                int rownumber = 0;
                decimal total_amount = 0;
                string str = total_amount.ToString();
                String line;
                foreach (DownloadBankAdviseOfficeNoteModel model in ack_list)
                {
                    // rownumber++;
                    total_amount += Convert.ToDecimal(model.total_sanc);
                }
                while ((line = sr.ReadLine()) != null)
                {
                    line = line.Replace("#listDate", DateTime.Now.ToString("dd-MMM-yyyy"));
                    line = line.Replace("#Amount", total_amount.ToString("##,##,###.00"));
                    if ((line.Trim() == ("<tbody>") && rownumber <= dt.Rows.Count))
                    {
                        foreach (DownloadBankAdviseOfficeNoteModel model in ack_list)
                        {
                            rownumber++;
                            string tempstr = (string.IsNullOrWhiteSpace(model.hosp_bank_name) ? " " : model.hosp_bank_name.ToString()) +
                                (string.IsNullOrWhiteSpace(model.hosp_bank_accno) ? " " : " AC NO:" + model.hosp_bank_accno.ToString()) +
                                (string.IsNullOrWhiteSpace(model.bank_ifsc_code) ? " " : " IFSC CODE:" + model.bank_ifsc_code.ToString());

                            string tr_line = "<tbody style='border: 1px solid black; border-collapse: collapse;'>" + "<tr><td style='border: 1px solid black; border-collapse: collapse;' align='center'>" + rownumber + "</td><td style='border: 1px solid black; border-collapse: collapse;' align='center'>" + model.hosp_name + "</td><td style='border: 1px solid black; border-collapse: collapse;' align='right'>" + Convert.ToDecimal(model.total_sanc).ToString("##,##,###.00") + "</td><td style='border: 1px solid black; border-collapse: collapse;' align='center'>" + tempstr + "</td></tr>";
                            sb.AppendLine(tr_line);
                        }

                    }

                    else if (rownumber == ack_list.Count)
                    {
                        string tr_line = "<tr style='border: 1px solid black; border-collapse: collapse;' ><td></td><td align='right'>Total :=</td><td align='left'>" + total_amount.ToString("##,##,###.00") + "</td><td></td></tr>";
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