using Dapper;
using MedicalR.CustomHelper;
using MedicalR.DataAccessLayer.IDAL.MedicalR;
using MedicalR.EmailSettings;
using MedicalR.Models;
using MedicalR.Models.MedicalR;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace MedicalR.DataAccessLayer.DAL.MedicalR
{
    public class DALSanctionMedicalBills : IDALSanctionMedicalBills
    {
        public List<SanctionMedicalBillsModel> SanctionBills_grid_data(string batch_no, string appl_no)
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
                    Addlist = con.Query<SanctionMedicalBillsModel>("select * from mdcl_sp_get_sanction_medical_bills_grid_data(:p_batch_no,:p_appl_no)", parameters).ToList();
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                Addlist = new List<SanctionMedicalBillsModel>();
            }
            return Addlist;


        }
        public List<SanctionMedicalBillsModel> SanctionBills_grid_data()
        {

            List<SanctionMedicalBillsModel> Addlist = new List<SanctionMedicalBillsModel>();
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {
                    con.Open();
                    Addlist = con.Query<SanctionMedicalBillsModel>("select * from mdcl_sp_get_sanction_medical_bills_grid_data()").ToList();
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                Addlist = new List<SanctionMedicalBillsModel>();
            }
            return Addlist;


        }
        public List<MedicalRequestModel> EmployeePastHistory_grid_data()
        {
            List<MedicalRequestModel> Addlist = new List<MedicalRequestModel>();
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {
                    con.Open();
                    DynamicParameters para = new DynamicParameters();
                    para.Add("p_employee_id", UserManager.User.UserID);
                    Addlist = con.Query<MedicalRequestModel>("select * from mdcl_sp_get_past_claim_request_grid_data(:p_employee_id)", para).ToList();
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                Addlist = new List<MedicalRequestModel>();
            }
            return Addlist;
        }
        public MedicalRequestModel GetItem(int id)
        {
            List<RCExpenseTypeDetailsModel> rcexpense_type_list = new List<RCExpenseTypeDetailsModel>();
            MedicalRequestModel bll = new MedicalRequestModel();

            using (NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString))
            {
                con.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("p_claim_request_id", id);
                bll = con.Query<MedicalRequestModel>("select * from mdcl_sp_get_claim_request_for_editchecking(:p_claim_request_id)", parameters).FirstOrDefault();
                rcexpense_type_list = con.Query<RCExpenseTypeDetailsModel>("select * from mdcl_sp_getclaim_request_expensetype_for_editchecking(:p_claim_request_id)", parameters).ToList();

                int k = 0;
                foreach (RCExpenseTypeDetailsModel ex_ty_details in rcexpense_type_list)
                {
                    if (bll.expense_type_detalis == null)
                    {
                        bll.expense_type_detalis = new List<RCExpenseTypeDetailsModel>();
                    }
                    bll.expense_type_detalis.Add(ex_ty_details);
                    DynamicParameters para2 = new DynamicParameters();
                    para2.Add("p_exptype_detail_id", ex_ty_details.id);
                    List<RCExpenseTypeDetailsItemsModel> item_list = con.Query<RCExpenseTypeDetailsItemsModel>("select * from mdcl_sp_getclaim_request_expensetype_items_for_editchecking(:p_exptype_detail_id)", para2).ToList();

                    foreach (RCExpenseTypeDetailsItemsModel item_model in item_list)
                    {
                        if (bll.expense_type_detalis[k].request_claim_expense_items == null)
                        {
                            bll.expense_type_detalis[k].request_claim_expense_items = new List<RCExpenseTypeDetailsItemsModel>();
                        }
                        bll.expense_type_detalis[k].request_claim_expense_items.Add(item_model);
                    }
                    k++;
                }


            }

            return bll;
        }
        public ResponseModel Save(MedicalRequestModel model, bool is_sanctioned)
        {
            NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString);
            ResponseModel Response = new ResponseModel();
            DataTable dt = new DataTable();
            NpgsqlTransaction trans = null;
            try
            {
                // NpgsqlConnection con = new NpgsqlConnection();
                using (con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {
                    con.Open();
                    trans = con.BeginTransaction();

                    DynamicParameters m_para = new DynamicParameters();
                    m_para.Add("p_id", model.id);
                    if (is_sanctioned)
                    {
                        m_para.Add("p_total_sanctioned_rs", model.total_sanctioned_rs);
                    }
                    else
                    {
                        m_para.Add("p_total_sanctioned_rs", 0);
                    }
                    con.Query<ResponseData>("select * from  mdcl_sp_update_overall_sactioned(:p_id,:p_total_sanctioned_rs)", m_para, trans).FirstOrDefault();

                    foreach (RCExpenseTypeDetailsModel rdm in model.expense_type_detalis)
                    {

                        DynamicParameters parameters_mst = new DynamicParameters();
                        parameters_mst.Add("p_id", rdm.id);
                        if (is_sanctioned)
                        {
                            parameters_mst.Add("p_total_sactioned_rs", rdm.total_sanctioned_rs);
                        }
                        else
                        {
                            parameters_mst.Add("p_total_sactioned_rs", 0);

                        }

                        con.Query<ResponseData>("select * from  mdcl_sp_update_expensewise_sactioned(:p_id,:p_total_sactioned_rs)", parameters_mst, trans).FirstOrDefault();



                        foreach (RCExpenseTypeDetailsItemsModel rtdm in rdm.request_claim_expense_items)
                        {
                            DynamicParameters parameters = new DynamicParameters();
                            parameters.Add("p_id", rtdm.id);
                            if (is_sanctioned)
                            {
                                parameters.Add("p_amt_sactioned_rs", rtdm.amt_sanctioned_rs);
                            }
                            else
                            {
                                parameters.Add("p_amt_sactioned_rs", 0);
                            }
                            parameters.Add("p_ma_remark", rtdm.ma_remark);
                            parameters.Add("p_objection_code_id", rtdm.objection_code_id);

                            con.Query<ResponseData>("select * from  mdcl_sp_update_batch_checking_verifried(:p_id,:p_amt_sactioned_rs,:p_ma_remark,:p_objection_code_id)", parameters, trans).FirstOrDefault();

                            Response.Status = true;
                            if (is_sanctioned)
                            {
                                Response.Message = MessageHelper.Sanctioned;
                            }
                            else
                            {
                                Response.Message = MessageHelper.Claim_Rejected;
                            }
                        }
                    }

                    DynamicParameters parameters2 = new DynamicParameters();
                    parameters2.Add("p_id", model.id);
                    if (is_sanctioned)
                    {
                        parameters2.Add("p_is_sanctioned", true);
                        parameters2.Add("p_objection_code_id", 0);
                    }
                    else
                    {
                        parameters2.Add("p_is_sanctioned", false);
                        parameters2.Add("p_objection_code_id", model.objection_code_id);
                    }
                    parameters2.Add("p_objection_remark", string.IsNullOrEmpty(model.objection_remark) ? "" : model.objection_remark);
                    con.Query<ResponseData>("select * from  mdcl_sp_update_sanction_status(:p_id,:p_is_sanctioned,:p_objection_code_id,:p_objection_remark)", parameters2, trans).FirstOrDefault();


                    DynamicParameters para = new DynamicParameters();
                    para.Add("p_employeeid", UserManager.User.UserID);
                    para.Add("p_claim_request_id", model.id);
                    if (is_sanctioned)
                    {
                        para.Add("p_status_code", "SANCBYAUTH");
                    }
                    else
                    {
                        para.Add("p_status_code", "APLREJECTION");
                    }
                    con.Query("select * from mdcl_sp_insert_empl_activities(:p_employeeid,:p_claim_request_id,:p_status_code)", para, trans);


                    trans.Commit();
                    CommonHelper.write_log("is_sanctioned :" + is_sanctioned);
                    //if (is_sanctioned)
                    //{
                    //    SanctionedbillMail(model);
                    //}
                    //else
                    //{
                    //    RejectionbillMail(model);
                    //}
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                Response.Status = false;
                Response.Message = MessageHelper.ExceptionMessage;
                trans.Rollback();
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return Response;

        }
        public List<SanctionMedicalBillsModel> OfficeNoteData()
        {

            List<SanctionMedicalBillsModel> Addlist = new List<SanctionMedicalBillsModel>();
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {
                    con.Open();
                    Addlist = con.Query<SanctionMedicalBillsModel>("select * from mdcl_sp_get_medicalr_officenote()").ToList();
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                Addlist = new List<SanctionMedicalBillsModel>();
            }
            return Addlist;


        }
        public string PreparePrintDoc(int request_id)
        {
            StringBuilder sb = new StringBuilder();

            using (NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString))
            {
                con.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("p_claim_request_id", request_id);
                List<MR_OBJECTION_LETTER_MODEL> objection_letters = con.Query<MR_OBJECTION_LETTER_MODEL>("select * from mdcl_sp_get_data_for_mr_objection_letter(:p_claim_request_id)", parameters).ToList();

                foreach (MR_OBJECTION_LETTER_MODEL model in objection_letters)
                {
                    string val_objection_letter = model.email_template;
                    val_objection_letter = val_objection_letter.Replace("#{appln_no}", string.IsNullOrWhiteSpace(model.appln_no) ? "" : model.appln_no);
                    val_objection_letter = val_objection_letter.Replace("#{letter_date}", model.rejection_date.ToString("dd/MM/yyyy"));
                    val_objection_letter = val_objection_letter.Replace("#{obejection_code}", string.IsNullOrWhiteSpace(model.objection_remark) ? "" : model.objection_remark);
                    val_objection_letter = val_objection_letter.Replace("#{emp_name}", string.IsNullOrWhiteSpace(model.employee_name) ? "" : model.employee_name);
                    val_objection_letter = val_objection_letter.Replace("#{add_line1}", string.IsNullOrWhiteSpace(model.address1) ? "" : model.address1);
                    val_objection_letter = val_objection_letter.Replace("#{add_line2}", string.IsNullOrWhiteSpace(model.address2) ? "" : model.address2);
                    val_objection_letter = val_objection_letter.Replace("#{add_line3}", string.IsNullOrWhiteSpace(model.address3) ? "" : model.address3);
                    val_objection_letter = val_objection_letter.Replace("#{city}", string.IsNullOrWhiteSpace(model.city) ? "" : model.city);
                    val_objection_letter = val_objection_letter.Replace("#{postal}", string.IsNullOrWhiteSpace(model.postal) ? "" : model.postal);

                    return val_objection_letter;
                }
            }
            return "";
        }

        public List<SanctionMedicalBillsModel> AnnexureData()
        {

            List<SanctionMedicalBillsModel> Addlist = new List<SanctionMedicalBillsModel>();
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {
                    con.Open();
                    Addlist = con.Query<SanctionMedicalBillsModel>("select * from mdcl_sp_get_medicalr_officenote_annexure()").ToList();
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                Addlist = new List<SanctionMedicalBillsModel>();
            }
            return Addlist;
        }

        public string DownloadSanctionedMedicalBills()
        {
            string res = string.Empty;
            StringBuilder sb = new StringBuilder();
            List<EditCheckingBillsModel> ack_list = new List<EditCheckingBillsModel>();
            DataTable dt = new DataTable();

            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(CustomHelper.CommonHelper.GetConnectionString))
                {
                    con.Open();
                    DynamicParameters parameters = new DynamicParameters();
                    ack_list = con.Query<EditCheckingBillsModel>("select * from mdcl_sp_get_sanctioned_medical_bills_download()", null).ToList();

                }
            }
            catch (Exception ex)
            {

            }
            using (StreamReader sr = new StreamReader(CommonHelper.Html_Template_Dir + "\\" + "MedicalR_SanctionMedicalBills.html"))
            {

                decimal total_amount = 0;
                decimal total_sanctioned_rs = 0;
                string str = total_amount.ToString();
                String line;
                while ((line = sr.ReadLine()) != null)
                {
                    line = line.Replace("#Date", DateTime.Now.ToString("dd-MMM-yyyy"));
                    line = line.Replace("#{total_sanctioned}", Convert.ToString(ack_list.Sum(item => item.total_sanctioned_rs)));
                    // line = line.Replace("#Amount", total_amount.ToString("##,##,###.00"));
                    if (line.Trim() == "<tbody id=\"EditBillChecking\">")
                    {
                        int srno = 0;
                        foreach (EditCheckingBillsModel batch in ack_list)
                        {
                            string tr_line = string.Empty;
                            tr_line = "<tr><td style='border: 1px solid black; border-collapse: collapse;' align='center'>" + (srno + 1).ToString() + "</td><td style='border: 1px solid black; border-collapse: collapse;' align='center'>" + batch.employeecode + "</td><td style='border: 1px solid black; border-collapse: collapse;' align='center'>" + batch.employeename + "</td><td style='border: 1px solid black; border-collapse: collapse;' align='center'>" + batch.appln_no + "</td><td style='border: 1px solid black; border-collapse: collapse;' align='right'>" + batch.date_of_request.ToString("dd/MM/yyyy") + "</td><td style='border: 1px solid black; border-collapse: collapse;' align='right'>" + batch.batch_no + "</td><td style='border: 1px solid black; border-collapse: collapse;' align='right'>" + batch.batch_date.ToString("dd/MM/yyyy") + "</td><td style='border: 1px solid black; border-collapse: collapse;' align='right'>" + batch.total_claimed_rs.ToString("##,##,###.00") + "</td><td style='border: 1px solid black; border-collapse: collapse;' align='right'>" + batch.total_sanctioned_rs.ToString("##,##,###.00") + "</td></tr>";
                            sb.AppendLine(tr_line);

                            total_sanctioned_rs = total_sanctioned_rs + batch.total_sanctioned_rs;
                            srno++;
                        }
                    }
                    sb.AppendLine(line);
                }
            }
            return sb.ToString();
        }


    }
}