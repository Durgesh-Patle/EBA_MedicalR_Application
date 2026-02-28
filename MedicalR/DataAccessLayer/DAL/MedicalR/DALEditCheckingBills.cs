using Dapper;
using MedicalR.CustomHelper;
using MedicalR.DataAccessLayer.IDAL.MedicalR;
using MedicalR.Models;
using MedicalR.Models.MedicalR;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace MedicalR.DataAccessLayer.DAL.MedicalR
{
    public class DALEditCheckingBills : IDALEditCheckingBills
    {
        public string DownloadEditCheckingBills()
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
                    ack_list = con.Query<EditCheckingBillsModel>("select * from mdcl_sp_get_checking_medical_bills_grid_data()", null).ToList();

                }
            }
            catch (Exception ex)
            {

            }
            using (StreamReader sr = new StreamReader(CommonHelper.Html_Template_Dir + "\\" + "EditCheckingBill.html"))
            {

                decimal total_amount = 0;
                string str = total_amount.ToString();
                String line;
                while ((line = sr.ReadLine()) != null)
                {
                    line = line.Replace("#Date", DateTime.Now.ToString("dd-MMM-yyyy"));
                    // line = line.Replace("#Amount", total_amount.ToString("##,##,###.00"));
                    if (line.Trim() == "<tbody id=\"EditBillChecking\">")
                    {
                        int srno = 0;
                        foreach (EditCheckingBillsModel batch in ack_list)
                        {
                            string tr_line = string.Empty;
                            tr_line = "<tr><td style='border: 1px solid black; border-collapse: collapse;' align='center'>" + (srno + 1).ToString() + "</td><td style='border: 1px solid black; border-collapse: collapse;' align='center'>" + batch.employeecode + "</td><td style='border: 1px solid black; border-collapse: collapse;' align='center'>" + batch.employeename + "</td><td style='border: 1px solid black; border-collapse: collapse;' align='center'>" + batch.appln_no + "</td><td style='border: 1px solid black; border-collapse: collapse;' align='right'>" + batch.date_of_request.ToString("dd/MM/yyyy") + "</td><td style='border: 1px solid black; border-collapse: collapse;' align='right'>" + batch.batch_no + "</td><td style='border: 1px solid black; border-collapse: collapse;' align='right'>" + batch.batch_date.ToString("dd/MM/yyyy") + "</td><td style='border: 1px solid black; border-collapse: collapse;' align='right'>" + batch.total_claimed_rs.ToString("##,##,###.00") + "</td></tr>";
                            sb.AppendLine(tr_line);
                            srno++;
                        }
                    }
                    sb.AppendLine(line);
                }
            }
            return sb.ToString();
        }

        public List<EditCheckingBillsModel> EditChecking_grid_data()
        {
            List<EditCheckingBillsModel> Addlist = new List<EditCheckingBillsModel>();
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {
                    con.Open();
                    Addlist = con.Query<EditCheckingBillsModel>("select * from mdcl_sp_get_checking_medical_bills_grid_data()").ToList();
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                Addlist = new List<EditCheckingBillsModel>();
            }
            return Addlist;
        }

        public List<EditCheckingBillsModel> EditChecking_grid_data(string batch_no, string appl_no)
        {
            List<EditCheckingBillsModel> Addlist = new List<EditCheckingBillsModel>();
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {
                    con.Open();
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("p_batch_no", string.IsNullOrWhiteSpace(batch_no) ? "" : batch_no);
                    parameters.Add("p_appl_no", string.IsNullOrWhiteSpace(appl_no) ? "" : appl_no);
                    Addlist = con.Query<EditCheckingBillsModel>("select * from mdcl_sp_get_checking_medical_bills_grid_data(:p_batch_no,:p_appl_no)", parameters).ToList();
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                Addlist = new List<EditCheckingBillsModel>();
            }
            return Addlist;
        }

        public MedicalRequestModel GetDataforEditChecking(MedicalRequestModel objModel)
        {
            List<RCExpenseTypeDetailsModel> rcexpense_type_list = new List<RCExpenseTypeDetailsModel>();
            MedicalRequestModel bll = new MedicalRequestModel();

            int id = objModel.id;

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

        public MedicalRequestModel GetItem(int id)
        {
            List<RCExpenseTypeDetailsModel> rcexpense_type_list = new List<RCExpenseTypeDetailsModel>();
            MedicalRequestModel bll = new MedicalRequestModel();
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {
                    con.Open();
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("p_claim_request_id", id);
                    bll = con.Query<MedicalRequestModel>("select * from mdcl_sp_get_claim_request_for_editchecking(:p_claim_request_id)", parameters).FirstOrDefault();

                    CommonHelper.write_log("Main claim data fetched: " + (bll != null));

                    rcexpense_type_list = con.Query<RCExpenseTypeDetailsModel>("select * from mdcl_sp_getclaim_request_expensetype_for_editchecking(:p_claim_request_id)", parameters).ToList();

                    CommonHelper.write_log("Expense type count: " + rcexpense_type_list.Count);

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
            }
            catch (Exception ex)
            {
                CommonHelper.write_log("Error in BLL GetItem(): " + ex.Message);
                CommonHelper.write_log("ex.StackTrace: " + ex.StackTrace);
                ExceptionLogging.LogException("BLL GetItem Error", ex);
            }
            return bll;
        }

        //public MedicalRequestModel GetItem(int id)
        //{
        //    CommonHelper.write_log("BLL GetItem() called. ID: " + id);

        //    List<RCExpenseTypeDetailsModel> rcexpense_type_list = new List<RCExpenseTypeDetailsModel>();
        //    MedicalRequestModel bll = new MedicalRequestModel();

        //    try
        //    {
        //        using (NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString))
        //        {
        //            con.Open();
        //            DynamicParameters parameters = new DynamicParameters();
        //            parameters.Add("p_claim_request_id", id);
        //            bll = con.Query<MedicalRequestModel>("select * from mdcl_sp_get_claim_request_for_editchecking(:p_claim_request_id)", parameters).FirstOrDefault();
        //            CommonHelper.write_log("Main claim data fetched: " + (bll != null));

        //            rcexpense_type_list = con.Query<RCExpenseTypeDetailsModel>("select * from mdcl_sp_getclaim_request_expensetype_for_editchecking(:p_claim_request_id)", parameters).ToList();

        //            CommonHelper.write_log("Expense type count: " + rcexpense_type_list.Count);

        //            int k = 0;



        //            foreach (RCExpenseTypeDetailsModel ex_ty_details in rcexpense_type_list)
        //            {
        //                CommonHelper.write_log("Processing ExpenseType Index: " + k + ", ID: " + ex_ty_details.id);

        //                if (bll.expense_type_detalis == null)
        //                {
        //                    CommonHelper.write_log("Initializing expense_type_detalis list");
        //                    bll.expense_type_detalis = new List<RCExpenseTypeDetailsModel>();
        //                }

        //                bll.expense_type_detalis.Add(ex_ty_details);

        //                DynamicParameters para2 = new DynamicParameters();
        //                para2.Add("p_exptype_detail_id", ex_ty_details.id);

        //                CommonHelper.write_log("Fetching items for ExpenseTypeID: " + ex_ty_details.id);

        //                List<RCExpenseTypeDetailsItemsModel> item_list =
        //                    con.Query<RCExpenseTypeDetailsItemsModel>(
        //                        "select * from mdcl_sp_getclaim_request_expensetype_items_for_editchecking(:p_exptype_detail_id)",
        //                        para2
        //                    ).ToList();

        //                CommonHelper.write_log("Item fetch completed. Item Count: " + item_list.Count);

        //                if (item_list == null || item_list.Count == 0)
        //                {
        //                    CommonHelper.write_log("No items found for ExpenseTypeID: " + ex_ty_details.id);
        //                }
        //                else
        //                {
        //                    if (bll.expense_type_detalis[k].request_claim_expense_items == null)
        //                    {
        //                        CommonHelper.write_log("Initializing request_claim_expense_items list for Index: " + k);
        //                        bll.expense_type_detalis[k].request_claim_expense_items = new List<RCExpenseTypeDetailsItemsModel>();
        //                    }

        //                    foreach (RCExpenseTypeDetailsItemsModel item_model in item_list)
        //                    {
        //                        bll.expense_type_detalis[k].request_claim_expense_items.Add(item_model);
        //                        CommonHelper.write_log("Item Added. ItemID: " + item_model.id);
        //                    }
        //                }

        //                k++;
        //            }

        //            CommonHelper.write_log("ExpenseType processing completed.");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        CommonHelper.write_log("Error in BLL GetItem(): " + ex.Message);
        //        CommonHelper.write_log("ex.StackTrace: " + ex.StackTrace);
        //        ExceptionLogging.LogException("BLL GetItem Error", ex);
        //    }

        //    return bll;
        //}

        public ResponseModel Save(MedicalRequestModel model)
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
                    m_para.Add("p_total_sactioned_rs", model.total_sanctioned_rs);
                    con.Query<ResponseData>("select * from  mdcl_sp_update_overall_sactioned(:p_id,:p_total_sactioned_rs)", m_para, trans).FirstOrDefault();

                    foreach (RCExpenseTypeDetailsModel rdm in model.expense_type_detalis)
                    {

                        DynamicParameters parameters_mst = new DynamicParameters();
                        parameters_mst.Add("p_id", rdm.id);
                        parameters_mst.Add("p_total_sactioned_rs", rdm.total_sanctioned_rs);

                        con.Query<ResponseData>("select * from  mdcl_sp_update_expensewise_sactioned(:p_id,:p_total_sactioned_rs)", parameters_mst, trans).FirstOrDefault();



                        foreach (RCExpenseTypeDetailsItemsModel rtdm in rdm.request_claim_expense_items)
                        {
                            DynamicParameters parameters = new DynamicParameters();
                            parameters.Add("p_id", rtdm.id);
                            parameters.Add("p_amt_sanctioned_rs", rtdm.amt_sanctioned_rs);
                            parameters.Add("p_ma_remark", rtdm.ma_remark);
                            parameters.Add("p_objection_code_id", rtdm.objection_code_id);

                            con.Query<ResponseData>("select * from  mdcl_sp_update_batch_checking_verifried(:p_id,:p_amt_sanctioned_rs,:p_ma_remark,:p_objection_code_id)", parameters, trans).FirstOrDefault();

                            Response.Status = true;
                            Response.Message = MessageHelper.Verified;
                        }
                    }

                    DynamicParameters parameters2 = new DynamicParameters();
                    parameters2.Add("p_id", model.id);
                    con.Query<ResponseData>("select * from  mdcl_sp_update_bill_verified_status(:p_id)", parameters2, trans).FirstOrDefault();


                    DynamicParameters para = new DynamicParameters();
                    para.Add("p_employeeid", UserManager.User.UserID);
                    para.Add("p_claim_request_id", model.id);
                    para.Add("p_status_code", "SCURBYDOAA");
                    con.Query("select * from mdcl_sp_insert_empl_activities(:p_employeeid,:p_claim_request_id,:p_status_code)", para, trans);


                    trans.Commit();
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                CommonHelper.write_log("Edit Cheacking Save() ex StackTrace" + ex.StackTrace);
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

    }
}