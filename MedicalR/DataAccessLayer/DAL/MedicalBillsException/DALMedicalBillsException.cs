using Dapper;
using MedicalR.CustomHelper;
using MedicalR.DataAccessLayer.IDAL.MedicalBillsException;
using MedicalR.Models;
using MedicalR.Models.MedicalBillsException;
using MedicalR.Models.MedicalR;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace MedicalR.DataAccessLayer.DAL.MedicalBillsException
{
    public class DALMedicalBillsException : IDALMedicalBillsException
    {
        public MedicalRequestModel GetItem(int id)
        {
            MedicalRequestModel item = new MedicalRequestModel();
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {
                    con.Open();
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("p_claim_request_id", id);
                    item = con.Query<MedicalRequestModel>("select * from mdcl_sp_get_additional_payment_item_details(:p_claim_request_id)", parameters).FirstOrDefault();
                }

            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
            }
            return item;
        }
        public ResponseModel Save(MedicalRequestModel model)
        {

            NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString);
            ResponseModel Response = new ResponseModel();
            DataTable dt = new DataTable();
            try
            {
                using (con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {
                    con.Open();
                    DynamicParameters m_para = new DynamicParameters();
                    m_para.Add("p_id", model.id);
                    m_para.Add("p_add_pay_amt", model.add_pay_amt);
                    m_para.Add("p_add_pay_remark", string.IsNullOrWhiteSpace(model.add_pay_remark) ? "" : model.add_pay_remark);
                    m_para.Add("p_add_pay_enclosure", string.IsNullOrWhiteSpace(model.add_pay_enclosure) ? "" : model.add_pay_enclosure);
                    con.Query<ResponseData>("select * from  mdcl_ex_sp_update_additional_payment_details(:p_id,:p_add_pay_amt,:p_add_pay_remark,:p_add_pay_enclosure)", m_para).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

            }
            return Response;
        }
        public List<AdditionalSanction> GetAdditionalSanc(string empl_code, string appln_no)
        {
            List<AdditionalSanction> Addlist = new List<AdditionalSanction>();
            DataTable dt = new DataTable();
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {
                    con.Open();
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("p_empl_code", string.IsNullOrWhiteSpace(empl_code) ? "" : empl_code);
                    parameters.Add("p_appln_no", string.IsNullOrWhiteSpace(appln_no) ? "" : appln_no);
                    Addlist = con.Query<AdditionalSanction>("select * from mdcl_sp_get_addl_sanction(:p_empl_code,:p_appln_no)", parameters).ToList();
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
            }
            return Addlist;
        }

        public AdditionalSanction Getsingleempdetails(AdditionalSanction objModel)
        {
            NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString);
            AdditionalSanction RempDetails = new AdditionalSanction();
            DataTable dt = new DataTable();
            int id;
            try
            {
                // NpgsqlConnection con = new NpgsqlConnection();
                id = objModel.id;
                objModel.emplid = id;
                using (con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {

                    con.Open();
                    using (NpgsqlCommand cmd = new NpgsqlCommand("select * from  mdcl_sp_get_single_addl_sanction(:pid)", con))
                    {

                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("pid", objModel.emplid);
                        using (NpgsqlDataAdapter SqDA = new NpgsqlDataAdapter(cmd))
                        {
                            SqDA.Fill(dt);
                        }
                    }

                    if (dt.Rows.Count == 0)
                    {
                        RempDetails = null;
                    }
                    else
                    {
                        foreach (DataRow drow in dt.Rows)
                        {
                            //  HospitalListModel arnobj = new HospitalListModel();
                            RempDetails.id = string.IsNullOrWhiteSpace(drow["id"].ToString()) ? 0 : Convert.ToInt32(drow["id"].ToString());
                            RempDetails.emplid = string.IsNullOrWhiteSpace(drow["emplid"].ToString()) ? 0 : Convert.ToInt32(drow["emplid"].ToString());
                            RempDetails.appln_no = string.IsNullOrWhiteSpace(drow["appln_no"].ToString()) ? "" : drow["appln_no"].ToString();
                            RempDetails.date_of_request = Convert.ToDateTime(drow["date_of_request"].ToString());
                            RempDetails.name_of_patient = string.IsNullOrWhiteSpace(drow["name_of_patient"].ToString()) ? "" : drow["name_of_patient"].ToString();
                            RempDetails.department = string.IsNullOrWhiteSpace(drow["department"].ToString()) ? "" : drow["department"].ToString();
                            RempDetails.band_group = string.IsNullOrWhiteSpace(drow["band_group"].ToString()) ? "" : drow["band_group"].ToString();
                            RempDetails.nature_of_illness = string.IsNullOrWhiteSpace(drow["nature_of_illness"].ToString()) ? "" : drow["nature_of_illness"].ToString();
                            RempDetails.treatment_type = string.IsNullOrWhiteSpace(drow["treatment_type"].ToString()) ? "" : drow["treatment_type"].ToString();
                            RempDetails.relationship = string.IsNullOrWhiteSpace(drow["relationship"].ToString()) ? "" : drow["relationship"].ToString();
                            RempDetails.total_claimed_rs = string.IsNullOrWhiteSpace(drow["total_claimed_rs"].ToString()) ? "" : drow["total_claimed_rs"].ToString();
                            RempDetails.sanction_and_paid_rs = string.IsNullOrWhiteSpace(drow["sanction_and_paid_rs"].ToString()) ? "" : drow["sanction_and_paid_rs"].ToString();
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                RempDetails = null;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return RempDetails;
        }
        public ResponseModel Approve(AdditionalSanction objModel)
        {
            NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString);
            ResponseModel Response = new ResponseModel();
            DataTable dt = new DataTable();

            try
            {
                // NpgsqlConnection con = new NpgsqlConnection();

                using (con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {

                    con.Open();

                    using (NpgsqlCommand cmd = new NpgsqlCommand("select * from mdcl_sp_insert_additional_sanction(:appln_no,:department,:date_of_request,:band_group,:name_of_patient,:relationship,:nature_of_illness,:treatment_type,:total_claimed_rs,:sanction_and_paid_rs,:addl_sanction_rs,:payment_remark,:no_of_enclosure,:emplid)", con))

                    {
                        cmd.Connection = con;

                        cmd.Parameters.AddWithValue("appln_no", objModel.appln_no);
                        cmd.Parameters.AddWithValue("department", objModel.department);
                        cmd.Parameters.AddWithValue("date_of_request", objModel.date_of_request);

                        cmd.Parameters.AddWithValue("band_group", objModel.band_group);
                        cmd.Parameters.AddWithValue("name_of_patient", objModel.name_of_patient);
                        cmd.Parameters.AddWithValue("relationship", objModel.relationship);
                        cmd.Parameters.AddWithValue("nature_of_illness", objModel.nature_of_illness);
                        cmd.Parameters.AddWithValue("treatment_type", objModel.treatment_type);
                        cmd.Parameters.AddWithValue("total_claimed_rs", objModel.total_claimed_rs);
                        cmd.Parameters.AddWithValue("sanction_and_paid_rs", objModel.sanction_and_paid_rs);
                        cmd.Parameters.AddWithValue("addl_sanction_rs", objModel.addl_sanction_rs);
                        cmd.Parameters.AddWithValue("payment_remark", objModel.payment_remark);
                        cmd.Parameters.AddWithValue("no_of_enclosure", objModel.no_of_enclosure);
                        cmd.Parameters.AddWithValue("emplid", objModel.emplid);

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
        public List<HospitilizationCr> GetHospitalizationCr()
        {
            NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString);
            List<HospitilizationCr> Addlist = new List<HospitilizationCr>();
            DataTable dt = new DataTable();

            try
            {

                using (con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {

                    con.Open();
                    using (NpgsqlCommand cmd = new NpgsqlCommand("select * from  mdcl_sp_get_hospitalization_cr()", con))
                    {
                        cmd.Connection = con;

                        using (NpgsqlDataAdapter SqDA = new NpgsqlDataAdapter(cmd))
                        {
                            SqDA.Fill(dt);
                        }
                    }

                    if (dt.Rows.Count == 0)
                    {
                        Addlist = new List<HospitilizationCr>();
                    }
                    else
                    {
                        foreach (DataRow drow in dt.Rows)
                        {
                            HospitilizationCr robj = new HospitilizationCr();
                            //robj.id = string.IsNullOrWhiteSpace(drow["id"].ToString()) ? 0 : Convert.ToInt32(drow["id"].ToString());
                            robj.emplid = string.IsNullOrWhiteSpace(drow["emplid"].ToString()) ? 0 : Convert.ToInt32(drow["emplid"].ToString());
                            robj.appln_no = string.IsNullOrWhiteSpace(drow["appln_no"].ToString()) ? "" : drow["appln_no"].ToString();
                            robj.date_of_request = Convert.ToDateTime(drow["date_of_request"].ToString());
                            robj.amount_paid_to_hospital_in_rs = string.IsNullOrWhiteSpace(drow["amount_paid_to_hospital_in_rs"].ToString()) ? "" : drow["amount_paid_to_hospital_in_rs"].ToString();
                            robj.amount_approved_sanction_tms_rs = string.IsNullOrWhiteSpace(drow["amount_approved_sanction_tms_rs"].ToString()) ? "" : drow["amount_approved_sanction_tms_rs"].ToString();
                            robj.amount_approved_sanction_maf_rs = string.IsNullOrWhiteSpace(drow["amount_approved_sanction_maf_rs"].ToString()) ? "" : drow["amount_approved_sanction_maf_rs"].ToString();


                            Addlist.Add(robj);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                Addlist = new List<HospitilizationCr>();
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return Addlist;
        }
        public HospitilizationCr GetsingleHospitalCr(HospitilizationCr objModel)
        {
            NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString);
            HospitilizationCr RempDetails = new HospitilizationCr();
            DataTable dt = new DataTable();
            int id;
            try
            {
                // NpgsqlConnection con = new NpgsqlConnection();
                id = objModel.id;
                objModel.emplid = id;
                using (con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {

                    con.Open();
                    using (NpgsqlCommand cmd = new NpgsqlCommand("select * from  mdcl_sp_get_single_hospitalization_cr(:pid)", con))
                    {

                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("pid", objModel.emplid);
                        using (NpgsqlDataAdapter SqDA = new NpgsqlDataAdapter(cmd))
                        {
                            SqDA.Fill(dt);
                        }
                    }

                    if (dt.Rows.Count == 0)
                    {
                        RempDetails = null;
                    }
                    else
                    {
                        foreach (DataRow drow in dt.Rows)
                        {
                            //  HospitalListModel arnobj = new HospitalListModel();
                            // RempDetails.id = string.IsNullOrWhiteSpace(drow["id"].ToString()) ? 0 : Convert.ToInt32(drow["id"].ToString());                           
                            RempDetails.appln_no = string.IsNullOrWhiteSpace(drow["appln_no"].ToString()) ? "" : drow["appln_no"].ToString();
                            RempDetails.date_of_request = Convert.ToDateTime(drow["date_of_request"].ToString());
                            RempDetails.band_group = string.IsNullOrWhiteSpace(drow["band_group"].ToString()) ? "" : drow["band_group"].ToString();
                            RempDetails.nature_of_illness = string.IsNullOrWhiteSpace(drow["nature_of_illness"].ToString()) ? "" : drow["nature_of_illness"].ToString();
                            RempDetails.treatment_type = string.IsNullOrWhiteSpace(drow["treatment_type"].ToString()) ? "" : drow["treatment_type"].ToString();
                            RempDetails.relationship = string.IsNullOrWhiteSpace(drow["relationship"].ToString()) ? "" : drow["relationship"].ToString();
                            RempDetails.from_date = Convert.ToDateTime(drow["from_date"].ToString());
                            RempDetails.to_date = Convert.ToDateTime(drow["to_date"].ToString());
                            RempDetails.name_of_hospital = string.IsNullOrWhiteSpace(drow["name_of_hospital"].ToString()) ? "" : drow["name_of_hospital"].ToString();
                            RempDetails.bill_no = string.IsNullOrWhiteSpace(drow["bill_no"].ToString()) ? "" : drow["bill_no"].ToString();
                            RempDetails.bill_date = Convert.ToDateTime(drow["bill_date"].ToString());
                            RempDetails.wheather_hospital_awarded_i_t_rebate = string.IsNullOrWhiteSpace(drow["wheather_hospital_awarded_i_t_rebate"].ToString()) ? "" : drow["wheather_hospital_awarded_i_t_rebate"].ToString();
                            RempDetails.gross_bill_amount_in_rs = string.IsNullOrWhiteSpace(drow["gross_bill_amount_in_rs"].ToString()) ? "" : drow["gross_bill_amount_in_rs"].ToString();
                            RempDetails.amount_paid_to_hospital_in_rs = string.IsNullOrWhiteSpace(drow["amount_paid_to_hospital_in_rs"].ToString()) ? "" : drow["amount_paid_to_hospital_in_rs"].ToString();
                            RempDetails.amount_approved_sanction_tms_rs = string.IsNullOrWhiteSpace(drow["amount_approved_sanction_tms_rs"].ToString()) ? "" : drow["amount_approved_sanction_tms_rs"].ToString();
                            RempDetails.amount_approved_sanction_maf_rs = string.IsNullOrWhiteSpace(drow["amount_approved_sanction_maf_rs"].ToString()) ? "" : drow["amount_approved_sanction_maf_rs"].ToString();
                            RempDetails.emplid = string.IsNullOrWhiteSpace(drow["emplid"].ToString()) ? 0 : Convert.ToInt32(drow["emplid"].ToString());
                            RempDetails.send_date_to_tmo = Convert.ToDateTime(drow["send_date_to_tmo"].ToString());
                            RempDetails.received_date_from_tmo = Convert.ToDateTime(drow["received_date_from_tmo"].ToString());
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                RempDetails = null;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return RempDetails;
        }
        public ResponseModel HospitalCrApprove(HospitilizationCr objModel)
        {
            NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString);
            ResponseModel Response = new ResponseModel();
            DataTable dt = new DataTable();

            try
            {
                // NpgsqlConnection con = new NpgsqlConnection();

                using (con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {

                    con.Open();

                    using (NpgsqlCommand cmd = new NpgsqlCommand("select * from mdcl_sp_insert_hospitalization_empl_recovery(:appln_no,:date_of_request,:band_group,:relationship,:nature_of_illness,:treatment_type,:from_date,:to_date,:name_of_hospital,:bill_no,:bill_date,:wheather_hospital_awarded_i_t_rebate,:gross_bill_amount_in_rs,:amount_paid_to_hospital_in_rs,:amount_approved_sanction_tms_rs,:amount_approved_sanction_maf_rs,:addl_sanction_in_rs,:amount_to_recover_from_empl,:recovery_installment_period,:emi,:send_date_to_tmo,:received_date_from_tmo,:emplid)", con))

                    {
                        cmd.Connection = con;

                        cmd.Parameters.AddWithValue("appln_no", objModel.appln_no);
                        cmd.Parameters.AddWithValue("date_of_request", objModel.date_of_request);
                        cmd.Parameters.AddWithValue("band_group", objModel.band_group);
                        cmd.Parameters.AddWithValue("relationship", objModel.relationship);
                        cmd.Parameters.AddWithValue("nature_of_illness", objModel.nature_of_illness);
                        cmd.Parameters.AddWithValue("treatment_type", objModel.treatment_type);
                        cmd.Parameters.AddWithValue("from_date", objModel.from_date);
                        cmd.Parameters.AddWithValue("to_date", objModel.to_date);
                        cmd.Parameters.AddWithValue("name_of_hospital", objModel.name_of_hospital);
                        cmd.Parameters.AddWithValue("bill_no", objModel.bill_no);
                        cmd.Parameters.AddWithValue("bill_date", objModel.bill_date);
                        cmd.Parameters.AddWithValue("wheather_hospital_awarded_i_t_rebate", objModel.wheather_hospital_awarded_i_t_rebate);
                        cmd.Parameters.AddWithValue("gross_bill_amount_in_rs", objModel.gross_bill_amount_in_rs);
                        cmd.Parameters.AddWithValue("amount_paid_to_hospital_in_rs", objModel.amount_paid_to_hospital_in_rs);
                        cmd.Parameters.AddWithValue("amount_approved_sanction_tms_rs", objModel.amount_approved_sanction_tms_rs);
                        cmd.Parameters.AddWithValue("amount_approved_sanction_maf_rs", objModel.amount_approved_sanction_maf_rs);
                        cmd.Parameters.AddWithValue("addl_sanction_in_rs", objModel.addl_sanction_in_rs);
                        cmd.Parameters.AddWithValue("amount_to_recover_from_empl", objModel.amount_to_recover_from_empl);
                        cmd.Parameters.AddWithValue("recovery_installment_period", objModel.recovery_installment_period);
                        cmd.Parameters.AddWithValue("emi", objModel.emi);
                        cmd.Parameters.AddWithValue("send_date_to_tmo", objModel.send_date_to_tmo);
                        cmd.Parameters.AddWithValue("received_date_from_tmo", objModel.received_date_from_tmo);

                        cmd.Parameters.AddWithValue("emplid", objModel.emplid);

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
        public List<HospitilizationDHRD> GetHospitalizationDHRD()
        {
            NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString);
            List<HospitilizationDHRD> Addlist = new List<HospitilizationDHRD>();
            DataTable dt = new DataTable();

            try
            {

                using (con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {

                    con.Open();
                    using (NpgsqlCommand cmd = new NpgsqlCommand("select * from  mdcl_sp_get_hospitalization_dhrd()", con))
                    {
                        cmd.Connection = con;

                        using (NpgsqlDataAdapter SqDA = new NpgsqlDataAdapter(cmd))
                        {
                            SqDA.Fill(dt);
                        }
                    }

                    if (dt.Rows.Count == 0)
                    {
                        Addlist = new List<HospitilizationDHRD>();
                    }
                    else
                    {
                        foreach (DataRow drow in dt.Rows)
                        {
                            HospitilizationDHRD robj = new HospitilizationDHRD();
                            //robj.id = string.IsNullOrWhiteSpace(drow["id"].ToString()) ? 0 : Convert.ToInt32(drow["id"].ToString());
                            robj.emplid = string.IsNullOrWhiteSpace(drow["emplid"].ToString()) ? 0 : Convert.ToInt32(drow["emplid"].ToString());
                            robj.appln_no = string.IsNullOrWhiteSpace(drow["appln_no"].ToString()) ? "" : drow["appln_no"].ToString();
                            robj.date_of_request = Convert.ToDateTime(drow["date_of_request"].ToString());
                            // robj.medical_claim_forwd_dhrd_inrs = string.IsNullOrWhiteSpace(drow["medical_claim_forwd_dhrd_inrs"].ToString()) ? "" : drow["medical_claim_forwd_dhrd_inrs"].ToString();



                            Addlist.Add(robj);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                Addlist = new List<HospitilizationDHRD>();
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return Addlist;
        }
        public HospitilizationDHRD GetsingleHospitalDHRD(HospitilizationDHRD objModel)
        {
            NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString);
            HospitilizationDHRD RempDetails = new HospitilizationDHRD();
            DataTable dt = new DataTable();
            int id;
            try
            {
                // NpgsqlConnection con = new NpgsqlConnection();
                id = objModel.id;
                objModel.emplid = id;
                using (con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {

                    con.Open();
                    using (NpgsqlCommand cmd = new NpgsqlCommand("select * from  mdcl_sp_get_single_hospitalization_dhrd(:pid)", con))
                    {

                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("pid", objModel.emplid);
                        using (NpgsqlDataAdapter SqDA = new NpgsqlDataAdapter(cmd))
                        {
                            SqDA.Fill(dt);
                        }
                    }

                    if (dt.Rows.Count == 0)
                    {
                        RempDetails = null;
                    }
                    else
                    {
                        foreach (DataRow drow in dt.Rows)
                        {
                            //  HospitalListModel arnobj = new HospitalListModel();
                            // RempDetails.id = string.IsNullOrWhiteSpace(drow["id"].ToString()) ? 0 : Convert.ToInt32(drow["id"].ToString());                           
                            RempDetails.appln_no = string.IsNullOrWhiteSpace(drow["appln_no"].ToString()) ? "" : drow["appln_no"].ToString();
                            RempDetails.date_of_request = Convert.ToDateTime(drow["date_of_request"].ToString());
                            RempDetails.band_group = string.IsNullOrWhiteSpace(drow["band_group"].ToString()) ? "" : drow["band_group"].ToString();
                            RempDetails.patient_id = string.IsNullOrWhiteSpace(drow["patient_id"].ToString()) ? 0 : Convert.ToInt32(drow["name_of_patient"]);
                            RempDetails.relationship = string.IsNullOrWhiteSpace(drow["relationship"].ToString()) ? "" : drow["relationship"].ToString();
                            RempDetails.nature_of_illness = string.IsNullOrWhiteSpace(drow["nature_of_illness"].ToString()) ? "" : drow["nature_of_illness"].ToString();
                            RempDetails.treatment_type = string.IsNullOrWhiteSpace(drow["treatment_type"].ToString()) ? "" : drow["treatment_type"].ToString();
                            RempDetails.from_date = Convert.ToDateTime(drow["from_date"].ToString());
                            RempDetails.to_date = Convert.ToDateTime(drow["to_date"].ToString());
                            RempDetails.name_of_hospital = string.IsNullOrWhiteSpace(drow["name_of_hospital"].ToString()) ? "" : drow["name_of_hospital"].ToString();
                            RempDetails.bill_no = string.IsNullOrWhiteSpace(drow["bill_no"].ToString()) ? "" : drow["bill_no"].ToString();
                            RempDetails.bill_date = Convert.ToDateTime(drow["bill_date"].ToString());
                            RempDetails.wheather_hospital_awarded_i_t_rebate = string.IsNullOrWhiteSpace(drow["wheather_hospital_awarded_i_t_rebate"].ToString()) ? "" : drow["wheather_hospital_awarded_i_t_rebate"].ToString();
                            RempDetails.medical_claim_forwd_dhrd_inrs = string.IsNullOrWhiteSpace(drow["medical_claim_forwd_dhrd_inrs"].ToString()) ? 0 : Convert.ToDecimal(drow["medical_claim_forwd_dhrd_inrs"]);
                            RempDetails.balance_amount = string.IsNullOrWhiteSpace(drow["balance_amount"].ToString()) ? 0 : Convert.ToDecimal(drow["balance_amount"]);
                            RempDetails.send_date_to_tmo = Convert.ToDateTime(drow["send_date_to_tmo"].ToString());
                            RempDetails.received_date_from_tmo = Convert.ToDateTime(drow["received_date_from_tmo"].ToString());
                            RempDetails.status = string.IsNullOrWhiteSpace(drow["status"].ToString()) ? "" : drow["status"].ToString();
                            RempDetails.emplid = string.IsNullOrWhiteSpace(drow["emplid"].ToString()) ? 0 : Convert.ToInt32(drow["emplid"].ToString());
                            RempDetails.send_date_to_tmo = Convert.ToDateTime(drow["send_date_to_tmo"].ToString());
                            RempDetails.received_date_from_tmo = Convert.ToDateTime(drow["received_date_from_tmo"].ToString());
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                RempDetails = null;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return RempDetails;
        }
        public ResponseModel HospitalDHRDSave(HospitilizationDHRD objModel)
        {
            NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString);
            ResponseModel Response = new ResponseModel();
            DataTable dt = new DataTable();

            try
            {
                // NpgsqlConnection con = new NpgsqlConnection();

                using (con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {

                    con.Open();

                    using (NpgsqlCommand cmd = new NpgsqlCommand("select * from mdcl_sp_insert_hospitalization_dhrd(:appln_no,:date_of_request,:band_group,:name_of_patient,:relationship,:nature_of_illness,:treatment_type,:from_date,:to_date,:name_of_hospital,:bill_no,:bill_date,:wheather_hospital_awarded_i_t_rebate,:medical_claim_forwd_dhrd_inrs,:amount_sanction_by_tmo_inrs,:balance_amount,:send_date_to_tmo,:received_date_from_tmo,:status,:emplid)", con))

                    {
                        cmd.Connection = con;

                        cmd.Parameters.AddWithValue("appln_no", objModel.appln_no);
                        cmd.Parameters.AddWithValue("date_of_request", objModel.date_of_request);
                        cmd.Parameters.AddWithValue("band_group", objModel.band_group);
                        cmd.Parameters.AddWithValue("patient_id", objModel.patient_id);
                        cmd.Parameters.AddWithValue("relationship", objModel.relationship);
                        cmd.Parameters.AddWithValue("nature_of_illness", objModel.nature_of_illness);
                        cmd.Parameters.AddWithValue("treatment_type", objModel.treatment_type);
                        cmd.Parameters.AddWithValue("from_date", objModel.from_date);
                        cmd.Parameters.AddWithValue("to_date", objModel.to_date);
                        cmd.Parameters.AddWithValue("name_of_hospital", objModel.name_of_hospital);
                        cmd.Parameters.AddWithValue("bill_no", objModel.bill_no);
                        cmd.Parameters.AddWithValue("bill_date", objModel.bill_date);
                        cmd.Parameters.AddWithValue("wheather_hospital_awarded_i_t_rebate", objModel.wheather_hospital_awarded_i_t_rebate);
                        cmd.Parameters.AddWithValue("medical_claim_forwd_dhrd_inrs", objModel.medical_claim_forwd_dhrd_inrs);
                        cmd.Parameters.AddWithValue("amount_sanction_by_tmo_inrs", objModel.amount_sanction_by_tmo_inrs);
                        cmd.Parameters.AddWithValue("balance_amount", objModel.balance_amount);

                        cmd.Parameters.AddWithValue("send_date_to_tmo", objModel.send_date_to_tmo);
                        cmd.Parameters.AddWithValue("received_date_from_tmo", objModel.received_date_from_tmo);
                        cmd.Parameters.AddWithValue("status", objModel.status);
                        cmd.Parameters.AddWithValue("emplid", objModel.emplid);

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
    }

}