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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
//using static MedicalR.CustomHelper.EmailHelper;

namespace MedicalR.DataAccessLayer.DAL.MedicalR
{
    public class DALProcessPayment : IDALProcessPayment
    {
        public List<ProcessPaymentModel> ProcessPayment_grid_data()
        {

            List<ProcessPaymentModel> Addlist = new List<ProcessPaymentModel>();
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {
                    con.Open();
                    Addlist = con.Query<ProcessPaymentModel>("select * from mdcl_sp_get_Process_payment_lot_wise()").ToList();
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                Addlist = new List<ProcessPaymentModel>();
            }
            return Addlist;


        }

        public ResponseModel Processpayment(ProcessPaymentModel objModel)
        {
            NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString);
            ResponseModel Response = new ResponseModel();
            DataTable dt = new DataTable();
            // string Status = "Applied";
            try
            {
                // NpgsqlConnection con = new NpgsqlConnection();
                // objModel.status = Status;
                using (con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {

                    con.Open();

                    using (NpgsqlCommand cmd = new NpgsqlCommand("select * from mdcl_sp_insert_process_payment_lot_wise(:employeecode,:employeename,:total_sanctioned_rs,:claim_request_id,:date_of_payment)", con))

                    {
                        cmd.Connection = con;

                        cmd.Parameters.AddWithValue("employeecode", objModel.employeecode);
                        cmd.Parameters.AddWithValue("employeename", objModel.employeename);
                        cmd.Parameters.AddWithValue("total_sanctioned_rs", objModel.total_sanctioned_rs);
                        cmd.Parameters.AddWithValue("claim_request_id", objModel.claim_request_id);
                        cmd.Parameters.AddWithValue("date_of_payment", objModel.date_of_payment);




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

        public bool RejectionbillMail(NpgsqlConnection con, string lot_no, DateTime date_of_payment, NpgsqlTransaction trans)
        {
            string email_template = string.Empty;
            EmailModel eml_model = new EmailModel();

            try
            {

                List<ProcessPaymentModel> objection_list = con.Query<ProcessPaymentModel>("select * from mdcl_sp_get_mr_objection_list_for_mail()").ToList();

                foreach (ProcessPaymentModel model in objection_list)
                {
                    //CommonHelper.write_log("rejection template :" + model.email_template);
                    string val_chc_rejected = model.email_template;
                    val_chc_rejected = val_chc_rejected.Replace("#{application_no}", string.IsNullOrWhiteSpace(model.appln_no) ? "" : model.appln_no.ToString());
                    val_chc_rejected = val_chc_rejected.Replace("#{claimed_amt}", string.IsNullOrWhiteSpace(model.total_claimed_rs.ToString()) ? "0.00" : model.total_claimed_rs.ToString());
                    val_chc_rejected = val_chc_rejected.Replace("#{objection_code}", string.IsNullOrWhiteSpace(model.objection_code) ? "" : model.objection_code);
                    val_chc_rejected = val_chc_rejected.Replace("#{objection_remark}", string.IsNullOrWhiteSpace(model.objection_remark) ? "" : model.objection_remark);

                    //CommonHelper.write_log("rejection template  val_chc_rejected: " + val_chc_rejected);

                    eml_model.email_subject = "Your claim for reimbursement medical bill is under objection";
                    eml_model.email_body = val_chc_rejected;
                    eml_model.to_emailids = new List<string>();
                    eml_model.cc_emailids = new List<string>();
                    if (!string.IsNullOrEmpty(model.email))
                    {
                        eml_model.to_emailids.Add(model.email);
                    }
                    if (!string.IsNullOrEmpty(model.cc_email_ids))
                    {
                        foreach (string cc_ids in model.cc_email_ids.Split(',').AsList())
                        {
                            eml_model.cc_emailids.Add(cc_ids);
                        }
                    }

                    bool success = EmailSettings.EmailHelper.SendEmail(eml_model, "CONFIRM");
                    if (success)
                    {
                        // update here if objection mail sent successfully

                        DynamicParameters para_objection = new DynamicParameters();
                        para_objection.Add("p_id", model.id);
                        para_objection.Add("p_lot_no", lot_no);
                        para_objection.Add("p_date_of_payment", date_of_payment);
                        con.Query("select * from mdcl_sp_update_status_after_objectmail_success(:p_id,:p_lot_no,:p_date_of_payment)", para_objection, trans).FirstOrDefault();

                        DynamicParameters para2 = new DynamicParameters();
                        para2.Add("p_employeeid", UserManager.User.UserID);
                        para2.Add("p_claim_request_id", model.id);
                        para2.Add("p_status_code", "APLREJECTION");
                        con.Query("select * from mdcl_sp_insert_empl_activities(:p_employeeid,:p_claim_request_id,:p_status_code)", para2, trans);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                return false;
            }
            return true;
        }
        public List<ProcessPaymentModel> EmployeeWiseSummary()
        {

            List<ProcessPaymentModel> Addlist = new List<ProcessPaymentModel>();
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {
                    con.Open();
                    Addlist = con.Query<ProcessPaymentModel>("select * from mdcl_sp_get_employee_wise_summary()").ToList();
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                Addlist = new List<ProcessPaymentModel>();
            }
            return Addlist;


        }

        public bool SanctionedbillMail(int claim_request_id)
        {
            try
            {
                string email_template = string.Empty;
                EmailModel eml_model = new EmailModel();
                StringBuilder sb = new StringBuilder();

                DALSanctionMedicalBills dal_sanction = new DALSanctionMedicalBills();
                MedicalRequestModel model = dal_sanction.GetItem(claim_request_id);


                using (StreamReader sr = new StreamReader(CommonHelper.Html_Template_Dir + "\\" + "MedicalR_Sanctioned.html"))
                {
                    CommonHelper.write_log("sanctioned template found");
                    String line;

                    while ((line = sr.ReadLine()) != null)
                    {
                        line = line.Replace("#{payment_date}", DateTime.Now.ToString("dd-MMM-yyyy"));
                        line = line.Replace("#{appln_no}", model.appln_no);
                        if ((line.Trim() == ("<tbody style='border: 1px solid black; border-collapse: collapse;'>")))
                        {
                            line = line.Replace("<tbody>", "<tbody>");
                            sb.Append(line);
                            int rowcount = 1;
                            foreach (RCExpenseTypeDetailsModel rdm in model.expense_type_detalis)
                            {
                                //if (rowcount == 1)
                                //{
                                //    string rowtr_line = "<tr><td>" + "SR.NO" + "</td><td>" + "Payment Type" + "</td><td>" + "Claimed Amt." + "</td><td>" + "Sanctioned Amt." + "</td></tr>";
                                //    sb.Append(rowtr_line);
                                //}
                                decimal amtclaimed = 0;
                                decimal amtsanctioned = 0;
                                foreach (RCExpenseTypeDetailsItemsModel rtdm in rdm.request_claim_expense_items)
                                {
                                    amtclaimed += string.IsNullOrWhiteSpace(rtdm.amt_claimed_rs.ToString()) ? 0 : Convert.ToDecimal(rtdm.amt_claimed_rs);
                                    amtsanctioned += string.IsNullOrWhiteSpace(rtdm.amt_sanctioned_rs.ToString()) ? 0 : Convert.ToDecimal(rtdm.amt_sanctioned_rs);
                                }
                                string tr_line = "<tr><td style='border: 1px solid black; border-collapse: collapse;'>" + rowcount + "</td><td style='border: 1px solid black; border-collapse: collapse;'>" + rdm.expense_type_name + "</td><td align='right' style='border: 1px solid black; border-collapse: collapse;'>" + Convert.ToDecimal(amtclaimed).ToString("##,##,###.00") + "</td><td align='right' style='border: 1px solid black; border-collapse: collapse;'>" + Convert.ToDecimal(amtsanctioned).ToString("##,##,###.00") + "</td></tr>";
                                sb.Append(tr_line);
                                rowcount++;
                            }
                            line = line.Replace("</tbody>", "</tbody>");
                            sb.Append(line);
                            //string closetablehead = "</tbody>";
                            //sb.Append(closetablehead);
                        }
                        sb.Append(line);
                    }

                }

                string val_emailbody = sb.ToString();
                CommonHelper.write_log("sanctioned emailbody :" + val_emailbody);
                // val_emailbody = val_emailbody.Replace(#{payment_date}", string.IsNullOrWhiteSpace(model.employeename) ? "" : model.employeename);

                eml_model.email_subject = "Your Claim for Reimbursement of Medical Bill - Sanctioned";
                eml_model.email_body = val_emailbody;
                eml_model.to_emailids = new List<string>();
                eml_model.cc_emailids = new List<string>();
                if (!string.IsNullOrEmpty(model.email))
                {
                    eml_model.to_emailids.Add(model.email);
                    //eml_model.to_emailids.Add("anisha.singh@cylsys.com");
                }
                //eml_model.to_emailids.Add("ghanshyam.vishwakarma@cylsys.com");
                string CC_emailids = ConfigurationManager.AppSettings["CC_santioned_mail"].ToString();

                if (!string.IsNullOrEmpty(CC_emailids))
                {
                    foreach (string cc_ids in CC_emailids.Split(',').AsList())
                    {
                        eml_model.cc_emailids.Add(cc_ids);
                    }
                }

                bool success = EmailSettings.EmailHelper.SendEmail(eml_model, "CONFIRM");
                CommonHelper.write_log("send mail success :" + success);
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                return false;
            }
            return true;
        }


        //public ResponseModel Processpayment(List<ProcessPaymentModel> objModel, DateTime date_of_payment)
        //{
        //    ResponseModel Response = new ResponseModel();
        //    DataTable dt = new DataTable();
        //    NpgsqlTransaction tran = null;
        //    // string Status = "Applied";
        //    try
        //    {
        //        // NpgsqlConnection con = new NpgsqlConnection();
        //        // objModel.status = Status;
        //        using (NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString))
        //        {

        //            con.Open();
        //            tran = con.BeginTransaction();
        //            string lot_no = con.Query<string>("select * from mdcl_sp_get_last_lotno_details()").FirstOrDefault();
        //            CommonHelper.write_log("Previouse Lot No :" + lot_no);
        //            if (string.IsNullOrWhiteSpace(lot_no))
        //            {
        //                lot_no = "Lot-1";
        //            }
        //            else
        //            {
        //                string var_lot = Convert.ToString(Convert.ToInt32(lot_no.Split('-')[1]) + 1);
        //                lot_no = "Lot-" + var_lot;
        //            }
        //            CommonHelper.write_log("New Generated Lot No :" + lot_no);
        //            foreach (ProcessPaymentModel model in objModel)
        //            {
        //                if (model.is_process)
        //                {

        //                    DynamicParameters para = new DynamicParameters();
        //                    para.Add("p_lot_no", lot_no);
        //                    para.Add("p_claim_request_id", model.claim_request_id);
        //                    para.Add("p_date_of_payment", date_of_payment);
        //                    //CommonHelper.write_log("claim_request_id payment lot creation :" + model.id + " claim request id :" + model.claim_request_id);
        //                    con.Query("select * from mdcl_sp_insert_process_payment_lot_wise(:p_lot_no,:p_claim_request_id,:p_date_of_payment)", para, tran);
        //                    // CommonHelper.write_log("cl");
        //                    DynamicParameters para2 = new DynamicParameters();
        //                    para2.Add("p_employeeid", UserManager.User.UserID);
        //                    para2.Add("p_claim_request_id", model.claim_request_id);
        //                    para2.Add("p_status_code", "PAYMNTRELEAS");
        //                    con.Query("select * from mdcl_sp_insert_empl_activities(:p_employeeid,:p_claim_request_id,:p_status_code)", para2, tran);
        //                    SanctionedbillMail(model.claim_request_id);
        //                }

        //            }
        //            RejectionbillMail(con, lot_no, date_of_payment, tran);
        //            tran.Commit();

        //            Response.Status = true;
        //            Response.Message = lot_no + " generated successfully!";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        tran.Rollback();
        //        ExceptionLogging.LogException(ex);
        //        Response.Status = false;
        //        Response.Message = MessageHelper.ExceptionMessage;
        //    }

        //    return Response;
        //}


        //public bool SanctionedbillMail(int claim_request_id)
        //{
        //    try
        //    {
        //        DALSanctionMedicalBills dal_sanction = new DALSanctionMedicalBills();
        //        MedicalRequestModel model = dal_sanction.GetItem(claim_request_id);

        //        string templatePath = Path.Combine(CommonHelper.Html_Template_Dir, "MedicalR_Sanctioned.html");
        //        string htmlTemplate = File.ReadAllText(templatePath);

        //        StringBuilder sb = new StringBuilder();
        //        htmlTemplate = htmlTemplate.Replace("#{payment_date}", DateTime.Now.ToString("dd-MMM-yyyy"))
        //                                   .Replace("#{appln_no}", model.appln_no ?? "");

        //        StringBuilder tableRows = new StringBuilder();
        //        int rowcount = 1;
        //        foreach (var rdm in model.expense_type_detalis ?? Enumerable.Empty<RCExpenseTypeDetailsModel>())
        //        {
        //            decimal amtclaimed = 0, amtsanctioned = 0;
        //            foreach (var rtdm in rdm.request_claim_expense_items ?? Enumerable.Empty<RCExpenseTypeDetailsItemsModel>())
        //            {
        //                amtclaimed += rtdm.amt_claimed_rs.GetValueOrDefault();
        //                amtsanctioned += rtdm.amt_sanctioned_rs.GetValueOrDefault();
        //            }


        //            tableRows.AppendLine($@"
        //        <tr>
        //            <td style='border: 1px solid black;'>{rowcount++}</td>
        //            <td style='border: 1px solid black;'>{rdm.expense_type_name}</td>
        //            <td align='right' style='border: 1px solid black;'>{amtclaimed:##,##,###.00}</td>
        //            <td align='right' style='border: 1px solid black;'>{amtsanctioned:##,##,###.00}</td>
        //        </tr>");
        //        }

        //        string finalBody = htmlTemplate.Replace("#{table_rows}", tableRows.ToString());

        //        var emailModel = new EmailModel
        //        {
        //            email_subject = "Your Claim for Reimbursement of Medical Bill - Sanctioned",
        //            email_body = finalBody,
        //            to_emailids = new List<string>(),
        //            cc_emailids = ConfigurationManager.AppSettings["CC_santioned_mail"]?.Split(',').ToList() ?? new List<string>()
        //        };

        //        if (!string.IsNullOrWhiteSpace(model.email))
        //        //    emailModel.to_emailids.Add(model.email);
        //        emailModel.to_emailids.Add("anisha.singh@cylsys.com");

        //        bool sent = EmailSettings.EmailHelper.SendEmail(emailModel, "CONFIRM");
        //        CommonHelper.write_log($"Sanctioned mail sent: {sent}");
        //        return sent;
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionLogging.LogException(ex);
        //        return false;
        //    }
        //}

        public ResponseModel Processpayment(List<ProcessPaymentModel> objModel, DateTime date_of_payment)
        {
            ResponseModel response = new ResponseModel();
            NpgsqlTransaction tran = null;
            Stopwatch sw = new Stopwatch();
            List<int> processedClaimIds = new List<int>();

            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {
                    con.Open();
                    tran = con.BeginTransaction();

                    sw.Restart();
                    // Step 1: Get Lot No
                    string lot_no = con.Query<string>("select * from mdcl_sp_get_last_lotno_details()").FirstOrDefault();
                    if (string.IsNullOrWhiteSpace(lot_no))
                        lot_no = "Lot-1";
                    else
                        lot_no = "Lot-" + (Convert.ToInt32(lot_no.Split('-')[1]) + 1);
                    CommonHelper.write_log($"Lot generation took: {sw.ElapsedMilliseconds}ms");

                    // Step 2: Process each claim
                    sw.Restart();
                    foreach (var model in objModel.Where(m => m.is_process))
                    {
                        // SP 1: Insert payment
                        var para = new DynamicParameters();
                        para.Add("p_lot_no", lot_no);
                        para.Add("p_claim_request_id", model.claim_request_id);
                        para.Add("p_date_of_payment", date_of_payment);
                        con.Query("select * from mdcl_sp_insert_process_payment_lot_wise(:p_lot_no, :p_claim_request_id, :p_date_of_payment)", para, tran);

                        // SP 2: Insert employee activity
                        var para2 = new DynamicParameters();
                        para2.Add("p_employeeid", UserManager.User.UserID);
                        para2.Add("p_claim_request_id", model.claim_request_id);
                        para2.Add("p_status_code", "PAYMNTRELEAS");
                        con.Query("select * from mdcl_sp_insert_empl_activities(:p_employeeid, :p_claim_request_id, :p_status_code)", para2, tran);
                        
                        processedClaimIds.Add(model.claim_request_id);
                    }
                    CommonHelper.write_log($"DB insert loop took: {sw.ElapsedMilliseconds}ms");

                    // Step 3: Rejection mail logic (inside transaction, if required)
                    sw.Restart();
                    //RejectionbillMail(con, lot_no, date_of_payment, tran);
                    CommonHelper.write_log($"Rejection bill mail prep took: {sw.ElapsedMilliseconds}ms");

                    // Step 4: Commit
                    tran.Commit();
                    response.Status = true;
                    response.Message = $"{lot_no} generated successfully!";
                }

                // Step 5: Mail sending (async, after commit)
                sw.Restart();
                Task.Run(() =>
                {
                    Parallel.ForEach(processedClaimIds, claimId =>
                    {
                        try
                        {
                            CommonHelper.write_log($"Mail queued for Claim ID: {claimId}");
                           // SanctionedbillMail(claimId);
                        }
                        catch (Exception ex)
                        {
                            ExceptionLogging.LogException(ex);
                            CommonHelper.write_log($"Mail failed for Claim ID: {claimId}");
                        }
                    });
                });

                CommonHelper.write_log($"Email trigger initiated after commit. Time: {sw.ElapsedMilliseconds}ms");
            }
            catch (Exception ex)
            {
                tran?.Rollback();
                ExceptionLogging.LogException(ex);
                response.Status = false;
                response.Message = MessageHelper.ExceptionMessage;
            }

            return response;
        }


    }
}