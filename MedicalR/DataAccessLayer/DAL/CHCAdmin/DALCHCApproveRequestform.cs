using Dapper;
using MedicalR.CustomHelper;
using MedicalR.DataAccessLayer.IDAL.CHCAdmin;
using MedicalR.EmailSettings;
using MedicalR.Models;
using MedicalR.Models.CHC;
using Npgsql;
//using Spire.DataExport.PropEditors;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace MedicalR.DataAccessLayer.DAL.CHCAdmin
{
    public class DALCHCApproveRequestform : IDALCHCApproveRequestform
    {
        public CHCRequest Getsingleempdetails(CHCRequest objModel)
        {
            NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString);
            CHCRequest RempDetails = new CHCRequest();
            DataTable dt = new DataTable();
            // int id;
            try
            {

                // id = objModel.id;
                // objModel.emplid = id;
                using (con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {

                    con.Open();
                    using (NpgsqlCommand cmd = new NpgsqlCommand("select * from  mdcl_sp_get_single_chcrequest_data(:pid)", con))
                    {

                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("pid", objModel.id);
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
                            RempDetails.relationship = string.IsNullOrWhiteSpace(drow["relationship"].ToString()) ? "" : drow["relationship"].ToString();
                            if (RempDetails.relationship == "Self and Spouse")
                            {
                                //  HospitalListModel arnobj = new HospitalListModel();
                                //RempDetails.id = string.IsNullOrWhiteSpace(drow["id"].ToString()) ? 0 : Convert.ToInt32(drow["id"].ToString());
                                RempDetails.emplid = string.IsNullOrWhiteSpace(drow["emplid"].ToString()) ? 0 : Convert.ToInt32(drow["emplid"].ToString());
                                RempDetails.employeename = string.IsNullOrWhiteSpace(drow["employeename"].ToString()) ? "" : drow["employeename"].ToString();
                                RempDetails.mob_no = string.IsNullOrWhiteSpace(drow["mob_no"].ToString()) ? "" : drow["mob_no"].ToString();
                                RempDetails.office_tel_no = string.IsNullOrWhiteSpace(drow["office_tel_no"].ToString()) ? "" : drow["office_tel_no"].ToString();
                                RempDetails.date_of_birth = Convert.ToDateTime(drow["date_of_birth"].ToString());
                                RempDetails.age = string.IsNullOrWhiteSpace(drow["age"].ToString()) ? "" : drow["age"].ToString();
                                RempDetails.date_of_checkup = Convert.ToDateTime(drow["date_of_checkup"].ToString());
                                RempDetails.chc_centerid = string.IsNullOrWhiteSpace(drow["chc_centerid"].ToString()) ? 0 : Convert.ToInt32(drow["chc_centerid"].ToString());
                                RempDetails.hosname = string.IsNullOrWhiteSpace(drow["hosname"].ToString()) ? "" : drow["hosname"].ToString();
                                RempDetails.name_of_spouse = string.IsNullOrWhiteSpace(drow["name_of_spouse"].ToString()) ? "" : drow["name_of_spouse"].ToString();
                                RempDetails.dob_spouse = Convert.ToDateTime(drow["dob_spouse"].ToString());
                                RempDetails.spouse_age = string.IsNullOrWhiteSpace(drow["spouse_age"].ToString()) ? "" : drow["spouse_age"].ToString();
                                RempDetails.spouse_dt_of_checkup = Convert.ToDateTime(drow["spouse_dt_of_checkup"].ToString());
                                RempDetails.chc_centid = string.IsNullOrWhiteSpace(drow["chc_centid"].ToString()) ? 0 : Convert.ToInt32(drow["chc_centid"].ToString());
                                RempDetails.phosname = string.IsNullOrWhiteSpace(drow["name"].ToString()) ? "" : drow["name"].ToString();
                                RempDetails.status = string.IsNullOrWhiteSpace(drow["status"].ToString()) ? "" : drow["status"].ToString();
                            }
                            if (RempDetails.relationship == "self")
                            {
                                //  HospitalListModel arnobj = new HospitalListModel();
                                //RempDetails.id = string.IsNullOrWhiteSpace(drow["id"].ToString()) ? 0 : Convert.ToInt32(drow["id"].ToString());
                                RempDetails.emplid = string.IsNullOrWhiteSpace(drow["emplid"].ToString()) ? 0 : Convert.ToInt32(drow["emplid"].ToString());
                                RempDetails.employeename = string.IsNullOrWhiteSpace(drow["employeename"].ToString()) ? "" : drow["employeename"].ToString();
                                RempDetails.mob_no = string.IsNullOrWhiteSpace(drow["mob_no"].ToString()) ? "" : drow["mob_no"].ToString();
                                RempDetails.office_tel_no = string.IsNullOrWhiteSpace(drow["office_tel_no"].ToString()) ? "" : drow["office_tel_no"].ToString();
                                RempDetails.date_of_birth = Convert.ToDateTime(drow["date_of_birth"].ToString());
                                RempDetails.age = string.IsNullOrWhiteSpace(drow["age"].ToString()) ? "" : drow["age"].ToString();
                                RempDetails.date_of_checkup = Convert.ToDateTime(drow["date_of_checkup"].ToString());
                                RempDetails.chc_centerid = string.IsNullOrWhiteSpace(drow["chc_centerid"].ToString()) ? 0 : Convert.ToInt32(drow["chc_centerid"].ToString());
                                RempDetails.hosname = string.IsNullOrWhiteSpace(drow["hosname"].ToString()) ? "" : drow["hosname"].ToString();
                                RempDetails.status = string.IsNullOrWhiteSpace(drow["status"].ToString()) ? "" : drow["status"].ToString();
                            }
                            if (RempDetails.relationship == "spouse")
                            {
                                //  HospitalListModel arnobj = new HospitalListModel();
                                // RempDetails.id = string.IsNullOrWhiteSpace(drow["id"].ToString()) ? 0 : Convert.ToInt32(drow["id"].ToString());
                                RempDetails.emplid = string.IsNullOrWhiteSpace(drow["emplid"].ToString()) ? 0 : Convert.ToInt32(drow["emplid"].ToString());
                                RempDetails.employeename = string.IsNullOrWhiteSpace(drow["employeename"].ToString()) ? "" : drow["employeename"].ToString();
                                RempDetails.name_of_spouse = string.IsNullOrWhiteSpace(drow["name_of_spouse"].ToString()) ? "" : drow["name_of_spouse"].ToString();
                                RempDetails.dob_spouse = Convert.ToDateTime(drow["dob_spouse"].ToString());
                                RempDetails.spouse_age = string.IsNullOrWhiteSpace(drow["spouse_age"].ToString()) ? "" : drow["spouse_age"].ToString();
                                RempDetails.spouse_dt_of_checkup = Convert.ToDateTime(drow["spouse_dt_of_checkup"].ToString());
                                RempDetails.chc_centid = string.IsNullOrWhiteSpace(drow["chc_centid"].ToString()) ? 0 : Convert.ToInt32(drow["chc_centid"].ToString());
                                RempDetails.phosname = string.IsNullOrWhiteSpace(drow["name"].ToString()) ? "" : drow["name"].ToString();
                                RempDetails.status = string.IsNullOrWhiteSpace(drow["status"].ToString()) ? "" : drow["status"].ToString();
                            }
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
        public ResponseModel CHCLotGenerationStart(List<CHCBillProcess> model)
        {
            ResponseModel Response = new ResponseModel();
            NpgsqlTransaction tran = null;
            using (NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString))
            {
                try
                {
                    con.Open();
                    tran = con.BeginTransaction();
                    string lot_no = con.Query<string>("select * from mdcl_sp_get_CHC_last_lotno_details()").FirstOrDefault();
                    CommonHelper.write_log("chc lot no :" + lot_no);
                    if (string.IsNullOrWhiteSpace(lot_no))
                    {
                        lot_no = "Lot-" + DateTime.Now.Year + "-1";
                    }
                    else
                    {
                        int last_number = Convert.ToInt32(lot_no.Split('-')[2]);
                        last_number = last_number + 1;
                        string var_lot = Convert.ToString(last_number);
                        lot_no = "Lot-" + DateTime.Now.Year + "-" + var_lot;
                    }
                    foreach (CHCBillProcess objmodel in model)
                    {
                        if (objmodel.Status2)
                        {
                            DynamicParameters para = new DynamicParameters();
                            para.Add("p_id", objmodel.id);
                            para.Add("p_lot_no", lot_no);

                            con.Query("select * from mdcl_sp_update_chc_lot_no(:p_id,:p_lot_no)", para, tran);
                        }
                    }
                    tran.Commit();
                    Response.Status = true;
                    Response.Message = lot_no + " generated successfully!"; //MessageHelper.RequestStatus;
                }
                catch (Exception ex)
                {
                    Response.Status = false;
                    Response.Message = "Generate lot error :" + ex.Message;

                }
            }
            return Response;
        }
        public List<CHCRequest> chcrequest_data()
        {
            NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString);
            List<CHCRequest> Addlist = new List<CHCRequest>();
            DataTable dt = new DataTable();

            try
            {

                using (con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {

                    con.Open();
                    using (NpgsqlCommand cmd = new NpgsqlCommand("select * from  mdcl_sp_get_chcrequest_for_approve()", con))
                    {
                        cmd.Connection = con;

                        using (NpgsqlDataAdapter SqDA = new NpgsqlDataAdapter(cmd))
                        {
                            SqDA.Fill(dt);
                        }
                    }

                    if (dt.Rows.Count == 0)
                    {
                        Addlist = new List<CHCRequest>();
                    }
                    else
                    {
                        foreach (DataRow drow in dt.Rows)
                        {
                            CHCRequest robj = new CHCRequest();
                            robj.relationship = string.IsNullOrWhiteSpace(drow["relationship"].ToString()) ? "" : drow["relationship"].ToString();
                            robj.emp_location = string.IsNullOrWhiteSpace(drow["emp_location"].ToString()) ? "" : drow["emp_location"].ToString();
                            if (robj.relationship == "Self and Spouse")
                            {
                                robj.id = string.IsNullOrWhiteSpace(drow["id"].ToString()) ? 0 : Convert.ToInt32(drow["id"].ToString());
                                robj.emplid = string.IsNullOrWhiteSpace(drow["emplid"].ToString()) ? 0 : Convert.ToInt32(drow["emplid"].ToString());
                                robj.employeename = string.IsNullOrWhiteSpace(drow["employeename"].ToString()) ? "" : drow["employeename"].ToString();
                                robj.date_of_checkup = Convert.ToDateTime(drow["date_of_checkup"].ToString());
                                robj.chc_centerid = string.IsNullOrWhiteSpace(drow["chc_centerid"].ToString()) ? 0 : Convert.ToInt32(drow["chc_centerid"].ToString());
                                robj.hosname = string.IsNullOrWhiteSpace(drow["hosname"].ToString()) ? "" : drow["hosname"].ToString();
                                robj.name_of_spouse = string.IsNullOrWhiteSpace(drow["name_of_spouse"].ToString()) ? "" : drow["name_of_spouse"].ToString();
                                robj.spouse_dt_of_checkup = Convert.ToDateTime(drow["spouse_dt_of_checkup"].ToString());
                                robj.chc_centid = string.IsNullOrWhiteSpace(drow["chc_centid"].ToString()) ? 0 : Convert.ToInt32(drow["chc_centid"].ToString());
                                robj.phosname = string.IsNullOrWhiteSpace(drow["name"].ToString()) ? "" : drow["name"].ToString();
                                robj.status = string.IsNullOrWhiteSpace(drow["status"].ToString()) ? "" : drow["status"].ToString();
                                robj.relationship = string.IsNullOrWhiteSpace(drow["relationship"].ToString()) ? "" : drow["relationship"].ToString();
                                robj.employeecode = string.IsNullOrWhiteSpace(drow["employeecode"].ToString()) ? "" : drow["employeecode"].ToString();
                                Addlist.Add(robj);
                            }
                            if (robj.relationship == "self")
                            {
                                robj.id = string.IsNullOrWhiteSpace(drow["id"].ToString()) ? 0 : Convert.ToInt32(drow["id"].ToString());
                                robj.emplid = string.IsNullOrWhiteSpace(drow["emplid"].ToString()) ? 0 : Convert.ToInt32(drow["emplid"].ToString());
                                robj.employeename = string.IsNullOrWhiteSpace(drow["employeename"].ToString()) ? "" : drow["employeename"].ToString();
                                robj.date_of_checkup = Convert.ToDateTime(drow["date_of_checkup"].ToString());
                                robj.chc_centerid = string.IsNullOrWhiteSpace(drow["chc_centerid"].ToString()) ? 0 : Convert.ToInt32(drow["chc_centerid"].ToString());
                                robj.hosname = string.IsNullOrWhiteSpace(drow["hosname"].ToString()) ? "" : drow["hosname"].ToString();
                                robj.status = string.IsNullOrWhiteSpace(drow["status"].ToString()) ? "" : drow["status"].ToString();
                                robj.relationship = string.IsNullOrWhiteSpace(drow["relationship"].ToString()) ? "" : drow["relationship"].ToString();
                                robj.employeecode = string.IsNullOrWhiteSpace(drow["employeecode"].ToString()) ? "" : drow["employeecode"].ToString();
                                Addlist.Add(robj);
                            }
                            if (robj.relationship == "spouse")
                            {
                                robj.id = string.IsNullOrWhiteSpace(drow["id"].ToString()) ? 0 : Convert.ToInt32(drow["id"].ToString());
                                robj.emplid = string.IsNullOrWhiteSpace(drow["emplid"].ToString()) ? 0 : Convert.ToInt32(drow["emplid"].ToString());
                                robj.employeename = string.IsNullOrWhiteSpace(drow["employeename"].ToString()) ? "" : drow["employeename"].ToString();
                                robj.name_of_spouse = string.IsNullOrWhiteSpace(drow["name_of_spouse"].ToString()) ? "" : drow["name_of_spouse"].ToString();
                                robj.spouse_dt_of_checkup = Convert.ToDateTime(drow["spouse_dt_of_checkup"].ToString());
                                robj.chc_centid = string.IsNullOrWhiteSpace(drow["chc_centid"].ToString()) ? 0 : Convert.ToInt32(drow["chc_centid"].ToString());
                                robj.phosname = string.IsNullOrWhiteSpace(drow["name"].ToString()) ? "" : drow["name"].ToString();
                                robj.status = string.IsNullOrWhiteSpace(drow["status"].ToString()) ? "" : drow["status"].ToString();
                                robj.relationship = string.IsNullOrWhiteSpace(drow["relationship"].ToString()) ? "" : drow["relationship"].ToString();
                                robj.employeecode = string.IsNullOrWhiteSpace(drow["employeecode"].ToString()) ? "" : drow["employeecode"].ToString();
                                Addlist.Add(robj);
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                Addlist = new List<CHCRequest>();
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return Addlist;
        }
        public ResponseModel ApproveCHCRequest(CHCRequest objModel)
        {
            NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString);
            ResponseModel Response = new ResponseModel();
            DataTable dt = new DataTable();
            string Status = "Approved";
            try
            {
                // NpgsqlConnection con = new NpgsqlConnection();
                objModel.status = Status;
                using (con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {

                    con.Open();
                    //using (NpgsqlCommand cmd = new NpgsqlCommand("select * from mdcl_sp_insert_chcrequestapprove(:pid,:pemplid,:pmob_no,:poffice_tel_no,:pdate_of_checkup,:pchc_centerid,:pappointmentno_empl,:pname_of_spouse,:pdob_spouse,:pspouse_age,:pspouse_dt_of_checkup,:pchc_centid,:pappointmentno_spouse,:pstatus,:relationship)", con))
                    using (NpgsqlCommand cmd = new NpgsqlCommand("select * from mdcl_sp_insert_chcrequestapprove(:pid,:pappointmentno_empl,:pappointmentno_spouse,:pstatus,:relationship)", con))
                    {
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("pid", objModel.id);
                        cmd.Parameters.AddWithValue("pappointmentno_empl", string.IsNullOrEmpty(objModel.appointmentno_empl) ? "" : objModel.appointmentno_empl);
                        cmd.Parameters.AddWithValue("pappointmentno_spouse", string.IsNullOrEmpty(objModel.appointmentno_spouse) ? "" : objModel.appointmentno_spouse);
                        cmd.Parameters.AddWithValue("pstatus", objModel.status);
                        cmd.Parameters.AddWithValue("relationship", objModel.relationship);
                        int Res = (cmd.ExecuteNonQuery());
                        if (Res < 0)
                        {
                            EmailProcess.SendMail_CHC_Approved(objModel.id, con);

                            Response.Status = true;
                            Response.Message = MessageHelper.RequestStatus;
                        }
                        else
                        {
                            Response.Status = false;
                            Response.Message = MessageHelper.ErroeMsg;
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
        public ResponseModel RejectCHCRequest(CHCRequest objModel)
        {
            NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString);
            ResponseModel Response = new ResponseModel();
            DataTable dt = new DataTable();
            string Status = "Reject";
            try
            {
                // NpgsqlConnection con = new NpgsqlConnection();
                objModel.status = Status;
                using (con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {
                    con.Open();
                    using (NpgsqlCommand cmd = new NpgsqlCommand("select * from mdcl_sp_insert_chcrequestreject(:pid,:pstatus,:premark)", con))
                    {
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("pid", objModel.id);
                        cmd.Parameters.AddWithValue("pstatus", objModel.status);
                        cmd.Parameters.AddWithValue("premark", objModel.remark);
                        int Res = (cmd.ExecuteNonQuery());
                        CommonHelper.write_log("rejected log cases :objModel.id :" + objModel.id);
                        if (objModel.id > 0 && Res < 0)
                        {
                            EmailProcess.SendMail_CHC_Rejected(objModel.id, con);
                            Response.Status = true;
                            Response.Message = MessageHelper.RequestStatus;
                        }
                        else
                        {
                            Response.Status = false;
                            Response.Message = MessageHelper.ErroeMsg;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CommonHelper.write_log("error chc reject cases :" + ex.Message);
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
        public List<CHCBillProcess> CHCBillProcess()
        {
            NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString);
            List<CHCBillProcess> Addlist = new List<CHCBillProcess>();
            DataTable dt = new DataTable();

            try
            {

                using (con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {

                    con.Open();
                    using (NpgsqlCommand cmd = new NpgsqlCommand("select * from mdcl_sp_get_chcrequest_for_billprocess()", con))
                    {
                        cmd.Connection = con;

                        using (NpgsqlDataAdapter SqDA = new NpgsqlDataAdapter(cmd))
                        {
                            SqDA.Fill(dt);
                        }
                    }

                    if (dt.Rows.Count == 0)
                    {
                        Addlist = new List<CHCBillProcess>();
                    }
                    else
                    {
                        foreach (DataRow drow in dt.Rows)
                        {
                            CHCBillProcess robj = new CHCBillProcess();
                            robj.id = string.IsNullOrWhiteSpace(drow["id"].ToString()) ? 0 : Convert.ToInt32(drow["id"].ToString());
                            robj.emplid = string.IsNullOrWhiteSpace(drow["emplid"].ToString()) ? 0 : Convert.ToInt32(drow["emplid"].ToString());
                            robj.employeecode = string.IsNullOrWhiteSpace(drow["employeecode"].ToString()) ? "" : drow["employeecode"].ToString();
                            robj.employee_name = string.IsNullOrWhiteSpace(drow["employeename"].ToString()) ? "" : drow["employeename"].ToString();
                            robj.chc_centerid = string.IsNullOrWhiteSpace(drow["chc_centerid"].ToString()) ? 0 : Convert.ToInt32(drow["chc_centerid"].ToString());
                            robj.hosname = string.IsNullOrWhiteSpace(drow["hosname"].ToString()) ? "" : drow["hosname"].ToString();
                            robj.name_of_spouse = string.IsNullOrWhiteSpace(drow["name_of_spouse"].ToString()) ? "" : drow["name_of_spouse"].ToString();

                            robj.spouse_chc_centerid = string.IsNullOrWhiteSpace(drow["chc_centid"].ToString()) ? 0 : Convert.ToInt32(drow["chc_centid"].ToString());
                            robj.phosname = string.IsNullOrWhiteSpace(drow["name"].ToString()) ? "" : drow["name"].ToString();
                            // robj.status = string.IsNullOrWhiteSpace(drow["status"].ToString()) ? "" : drow["status"].ToString();
                            robj.relationship = string.IsNullOrWhiteSpace(drow["relationship"].ToString()) ? "" : drow["relationship"].ToString();
                            robj.spouse_dt_of_checkup = string.IsNullOrWhiteSpace(drow["spouse_dt_of_checkup"].ToString()) ? "" : drow["spouse_dt_of_checkup"].ToString();
                            robj.date_of_checkup = string.IsNullOrWhiteSpace(drow["date_of_checkup"].ToString()) ? "" : drow["date_of_checkup"].ToString();
                            Addlist.Add(robj);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                Addlist = new List<CHCBillProcess>();
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return Addlist;
        }
        public ResponseModel AddBillProcess(CHCBillProcess objModel)
        {
            NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString);
            ResponseModel Response = new ResponseModel();
            DataTable dt = new DataTable();
            string Status = "Payment Process";
            try
            {
                // NpgsqlConnection con = new NpgsqlConnection();
                objModel.status = Status;
                using (con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {

                    con.Open();

                    using (NpgsqlCommand cmd = new NpgsqlCommand("select * from mdcl_sp_insert_chc_billprocess(:pid,:emplid,:employee_name,:chc_centerid,:bill_no,:amount,:name_of_spouse,:spouse_chc_centerid,:spouse_bill_no,:spouse_amount,:pstatus,:relationship)", con))

                    {
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("pid", objModel.id);
                        cmd.Parameters.AddWithValue("emplid", objModel.emplid);
                        cmd.Parameters.AddWithValue("employee_name", string.IsNullOrEmpty(objModel.employee_name) ? "" : objModel.employee_name);
                        cmd.Parameters.AddWithValue("chc_centerid", objModel.chc_centerid);
                        cmd.Parameters.AddWithValue("bill_no", string.IsNullOrEmpty(objModel.bill_no) ? "" : objModel.bill_no);
                        cmd.Parameters.AddWithValue("amount", string.IsNullOrEmpty(objModel.amount) ? "" : objModel.amount);
                        cmd.Parameters.AddWithValue("name_of_spouse", string.IsNullOrEmpty(objModel.name_of_spouse) ? "" : objModel.name_of_spouse);
                        cmd.Parameters.AddWithValue("spouse_chc_centerid", objModel.spouse_chc_centerid);
                        cmd.Parameters.AddWithValue("spouse_bill_no", string.IsNullOrEmpty(objModel.spouse_bill_no) ? "" : objModel.spouse_bill_no);
                        cmd.Parameters.AddWithValue("spouse_amount", string.IsNullOrEmpty(objModel.spouse_amount) ? "" : objModel.spouse_amount);
                        cmd.Parameters.AddWithValue("pstatus", objModel.status);
                        cmd.Parameters.AddWithValue("relationship", objModel.relationship);


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
        public CHCBillProcess GetsingleEmplBill(CHCBillProcess objModel)
        {
            NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString);
            CHCBillProcess RempDetails = new CHCBillProcess();
            DataTable dt = new DataTable();
            //  int id;
            try
            {
                // NpgsqlConnection con = new NpgsqlConnection();
                //  id = objModel.id;
                // objModel.emplid = id;
                using (con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {

                    con.Open();
                    using (NpgsqlCommand cmd = new NpgsqlCommand("select * from  mdcl_sp_get_single_chcrequest_for_billprocess(:pid)", con))
                    {

                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("pid", objModel.id);
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
                            RempDetails.relationship = string.IsNullOrWhiteSpace(drow["relationship"].ToString()) ? "" : drow["relationship"].ToString();
                            if (RempDetails.relationship == "Self and Spouse")
                            {
                                RempDetails.id = string.IsNullOrWhiteSpace(drow["id"].ToString()) ? 0 : Convert.ToInt32(drow["id"].ToString());
                                RempDetails.emplid = string.IsNullOrWhiteSpace(drow["emplid"].ToString()) ? 0 : Convert.ToInt32(drow["emplid"].ToString());
                                RempDetails.employee_name = string.IsNullOrWhiteSpace(drow["employeename"].ToString()) ? "" : drow["employeename"].ToString();
                                RempDetails.employeecode = string.IsNullOrWhiteSpace(drow["employeecode"].ToString()) ? "" : drow["employeecode"].ToString();
                                RempDetails.chc_centerid = string.IsNullOrWhiteSpace(drow["chc_centerid"].ToString()) ? 0 : Convert.ToInt32(drow["chc_centerid"].ToString());
                                RempDetails.hosname = string.IsNullOrWhiteSpace(drow["hosname"].ToString()) ? "" : drow["hosname"].ToString();
                                RempDetails.name_of_spouse = string.IsNullOrWhiteSpace(drow["name_of_spouse"].ToString()) ? "" : drow["name_of_spouse"].ToString();

                                RempDetails.spouse_chc_centerid = string.IsNullOrWhiteSpace(drow["chc_centid"].ToString()) ? 0 : Convert.ToInt32(drow["chc_centid"].ToString());
                                RempDetails.phosname = string.IsNullOrWhiteSpace(drow["name"].ToString()) ? "" : drow["name"].ToString();

                            }
                            if (RempDetails.relationship == "self")
                            {
                                RempDetails.id = string.IsNullOrWhiteSpace(drow["id"].ToString()) ? 0 : Convert.ToInt32(drow["id"].ToString());
                                RempDetails.emplid = string.IsNullOrWhiteSpace(drow["emplid"].ToString()) ? 0 : Convert.ToInt32(drow["emplid"].ToString());
                                RempDetails.employee_name = string.IsNullOrWhiteSpace(drow["employeename"].ToString()) ? "" : drow["employeename"].ToString();
                                RempDetails.employeecode = string.IsNullOrWhiteSpace(drow["employeecode"].ToString()) ? "" : drow["employeecode"].ToString();
                                RempDetails.chc_centerid = string.IsNullOrWhiteSpace(drow["chc_centerid"].ToString()) ? 0 : Convert.ToInt32(drow["chc_centerid"].ToString());
                                RempDetails.hosname = string.IsNullOrWhiteSpace(drow["hosname"].ToString()) ? "" : drow["hosname"].ToString();


                            }
                            if (RempDetails.relationship == "spouse")
                            {
                                RempDetails.id = string.IsNullOrWhiteSpace(drow["id"].ToString()) ? 0 : Convert.ToInt32(drow["id"].ToString());
                                RempDetails.employeecode = string.IsNullOrWhiteSpace(drow["employeecode"].ToString()) ? "" : drow["employeecode"].ToString();
                                RempDetails.emplid = string.IsNullOrWhiteSpace(drow["emplid"].ToString()) ? 0 : Convert.ToInt32(drow["emplid"].ToString());
                                RempDetails.name_of_spouse = string.IsNullOrWhiteSpace(drow["name_of_spouse"].ToString()) ? "" : drow["name_of_spouse"].ToString();

                                RempDetails.spouse_chc_centerid = string.IsNullOrWhiteSpace(drow["chc_centid"].ToString()) ? 0 : Convert.ToInt32(drow["chc_centid"].ToString());
                                RempDetails.phosname = string.IsNullOrWhiteSpace(drow["name"].ToString()) ? "" : drow["name"].ToString();


                            }
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
        public ResponseModel HoldBillProcess(CHCBillProcess objModel)
        {
            NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString);
            ResponseModel Response = new ResponseModel();
            DataTable dt = new DataTable();
            string Status = "Hold";
            try
            {
                // NpgsqlConnection con = new NpgsqlConnection();
                objModel.status = Status;
                using (con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {
                    con.Open();
                    using (NpgsqlCommand cmd = new NpgsqlCommand("select * from mdcl_sp_hold_chc_billprocess(:pid,:pstatus)", con))
                    {
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("pid", objModel.id);
                        cmd.Parameters.AddWithValue("pstatus", objModel.status);
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
        public List<CHCBillProcess> CremplsalAcc()
        {
            NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString);
            List<CHCBillProcess> Addlist = new List<CHCBillProcess>();
            DataTable dt = new DataTable();
            try
            {
                using (con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {
                    con.Open();
                    using (NpgsqlCommand cmd = new NpgsqlCommand("select * from mdcl_sp_get_billprocess()", con))
                    {
                        cmd.Connection = con;
                        using (NpgsqlDataAdapter SqDA = new NpgsqlDataAdapter(cmd))
                        {
                            SqDA.Fill(dt);
                        }
                    }
                    if (dt.Rows.Count == 0)
                    {
                        Addlist = new List<CHCBillProcess>();
                    }
                    else
                    {
                        int count = 0;
                        foreach (DataRow drow in dt.Rows)
                        {
                            count++;
                            CHCBillProcess robj = new CHCBillProcess();
                            robj.srno = count;
                            robj.id = string.IsNullOrWhiteSpace(drow["id"].ToString()) ? 0 : Convert.ToInt32(drow["id"].ToString());
                            robj.emplid = string.IsNullOrWhiteSpace(drow["emplid"].ToString()) ? 0 : Convert.ToInt32(drow["emplid"].ToString());
                            //robj.employee_code = string.IsNullOrWhiteSpace(drow["employee_code"].ToString()) ? "" : drow["employee_code"].ToString();
                            robj.employee_name = string.IsNullOrWhiteSpace(drow["employeename"].ToString()) ? "" : drow["employeename"].ToString();
                            robj.date_of_checkup = string.IsNullOrWhiteSpace(drow["date_of_checkup"].ToString()) ? "" : Convert.ToDateTime(drow["date_of_checkup"]).ToString("dd/MM/yyyy");
                            robj.chc_centerid = string.IsNullOrWhiteSpace(drow["chc_centerid"].ToString()) ? 0 : Convert.ToInt32(drow["chc_centerid"].ToString());
                            robj.hosname = string.IsNullOrWhiteSpace(drow["hosname"].ToString()) ? "" : drow["hosname"].ToString();
                            robj.amount = string.IsNullOrWhiteSpace(drow["amount"].ToString()) ? "" : drow["amount"].ToString();
                            robj.name_of_spouse = string.IsNullOrWhiteSpace(drow["name_of_spouse"].ToString()) ? "" : drow["name_of_spouse"].ToString();
                            robj.spouse_dt_of_checkup = string.IsNullOrWhiteSpace(drow["spouse_dt_of_checkup"].ToString()) ? "" : Convert.ToDateTime(drow["spouse_dt_of_checkup"]).ToString("dd/MM/yyyy");
                            robj.spouse_chc_centerid = string.IsNullOrWhiteSpace(drow["chc_centid"].ToString()) ? 0 : Convert.ToInt32(drow["chc_centid"].ToString());
                            robj.spouse_hosname = string.IsNullOrWhiteSpace(drow["hosname_sps"].ToString()) ? "" : drow["hosname_sps"].ToString();
                            robj.spouse_amount = string.IsNullOrWhiteSpace(drow["spouse_amount"].ToString()) ? "" : drow["spouse_amount"].ToString();
                            robj.total = string.IsNullOrWhiteSpace(drow["total"].ToString()) ? "" : drow["total"].ToString();
                            robj.relationship = string.IsNullOrWhiteSpace(drow["relationship"].ToString()) ? "" : drow["relationship"].ToString();
                            robj.CrsalAcc_status = string.IsNullOrWhiteSpace(drow["creditsalacc_status"].ToString()) ? false : Convert.ToBoolean(drow["creditsalacc_status"].ToString());
                            //robj.spouse_chc_centerid = string.IsNullOrWhiteSpace(drow["chc_centid"].ToString()) ? 0 : Convert.ToInt32(drow["chc_centid"].ToString());
                            //robj.phosname = string.IsNullOrWhiteSpace(drow["name"].ToString()) ? "" : drow["name"].ToString();
                            // robj.status = string.IsNullOrWhiteSpace(drow["status"].ToString()) ? "" : drow["status"].ToString();
                            Addlist.Add(robj);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                Addlist = new List<CHCBillProcess>();
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return Addlist;
        }

        public ResponseModel AddCreaditTosalAcc(List<CHCBillProcess> objModel, DateTime CR_DATE)
        {
            NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString);
            ResponseModel Response = new ResponseModel();
            DataTable dt = new DataTable();

            NpgsqlTransaction tx = null;

            try
            {
                if (CR_DATE != null)
                {

                    // NpgsqlConnection con = new NpgsqlConnection();
                    // objModel.status = Status;
                    using (con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                    {

                        con.Open();
                        tx = con.BeginTransaction();
                        foreach (CHCBillProcess chcobj in objModel)
                        {
                            if (chcobj.CrsalAcc_status)
                            {
                                using (NpgsqlCommand cmd = new NpgsqlCommand("select * from mdcl_sp_update_crsalacc(:p_id,:p_amount,:p_spouse_amount,:p_CrsalAcc_status,:p_crdate)", con, tx))

                                {
                                    cmd.Connection = con;
                                    cmd.Parameters.AddWithValue("p_id", chcobj.id);
                                    cmd.Parameters.AddWithValue("p_amount", string.IsNullOrWhiteSpace(chcobj.amount) ? "" : chcobj.amount.ToString());
                                    cmd.Parameters.AddWithValue("p_spouse_amount", string.IsNullOrWhiteSpace(chcobj.spouse_amount) ? "" : chcobj.spouse_amount.ToString()); ;
                                    cmd.Parameters.AddWithValue("p_CrsalAcc_status", chcobj.CrsalAcc_status);
                                    cmd.Parameters.AddWithValue("p_crdate", Convert.ToDateTime(CR_DATE));
                                    int Res = (int)(cmd.ExecuteScalar());
                                    //bool is_success = false;
                                    //CommonHelper.write_log("save");
                                    //if (Res > 0)
                                    //{

                                    //    is_success = true;
                                    //}
                                    //if (Res > 0 && is_success)
                                    //{
                                    //    Response.Status = true;
                                    //    Response.Message = MessageHelper.RequestStatus;
                                    //}
                                    //else
                                    //{
                                    //    Response.Status = false;
                                    //    Response.Message = MessageHelper.ErroeMsg;
                                    //    tx.Rollback();
                                    //    return Response;
                                    //}
                                }
                            }
                        }
                        tx.Commit();
                        Response.Status = true;
                        Response.Message = MessageHelper.RequestStatus;
                    }
                }
                else
                {
                    Response.Status = false;
                    Response.Message = "Please Select Credit Date !";
                    return Response;
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                Response.Status = false;
                Response.Message = MessageHelper.ExceptionMessage;
                tx.Rollback();
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return Response;
        }

        public ResponseModel SaveforLaterAddCreaditTosalAcc(List<CHCBillProcess> objModel, DateTime CR_DATE)
        {
            NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString);
            ResponseModel Response = new ResponseModel();
            DataTable dt = new DataTable();

            NpgsqlTransaction tx = null;

            try
            {
                if (CR_DATE != null)
                {

                    // NpgsqlConnection con = new NpgsqlConnection();
                    // objModel.status = Status;
                    using (con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                    {

                        con.Open();
                        tx = con.BeginTransaction();
                        foreach (CHCBillProcess chcobj in objModel)
                        {
                            using (NpgsqlCommand cmd = new NpgsqlCommand("select * from mdcl_sp_update_saveforlater_crsalacc(:p_id,:p_amount,:p_spouse_amount,:p_CrsalAcc_status,:p_crdate)", con, tx))

                            {
                                cmd.Connection = con;
                                cmd.Parameters.AddWithValue("p_id", chcobj.id);
                                cmd.Parameters.AddWithValue("p_amount", string.IsNullOrWhiteSpace(chcobj.amount) ? "" : chcobj.amount.ToString());
                                cmd.Parameters.AddWithValue("p_spouse_amount", string.IsNullOrWhiteSpace(chcobj.spouse_amount) ? "" : chcobj.spouse_amount.ToString()); ;
                                cmd.Parameters.AddWithValue("p_CrsalAcc_status", chcobj.saveforlater_status);
                                cmd.Parameters.AddWithValue("p_crdate", Convert.ToDateTime(CR_DATE));
                                int Res = (int)(cmd.ExecuteScalar());
                                bool is_success = false;
                                CommonHelper.write_log("save");
                                if (Res > 0)
                                {

                                    is_success = true;
                                }
                                if (Res > 0 && is_success)
                                {
                                    Response.Status = true;
                                    Response.Message = MessageHelper.RequestStatus;
                                }
                                else
                                {
                                    Response.Status = false;
                                    Response.Message = MessageHelper.ErroeMsg;
                                    tx.Rollback();
                                    return Response;
                                }
                            }
                        }
                        tx.Commit();
                    }
                }
                else
                {
                    Response.Status = false;
                    Response.Message = "Please Select Credit Date !";
                    return Response;
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                Response.Status = false;
                Response.Message = MessageHelper.ExceptionMessage;
                tx.Rollback();
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return Response;
        }
        public CHCBillProcess GetsingleEmplCr(CHCBillProcess objModel)
        {
            NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString);
            CHCBillProcess RempDetails = new CHCBillProcess();
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
                    using (NpgsqlCommand cmd = new NpgsqlCommand("select * from  mdcl_sp_get_single_billprocess(:pid)", con))
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
                            RempDetails.emplid = string.IsNullOrWhiteSpace(drow["emplid"].ToString()) ? 0 : Convert.ToInt32(drow["emplid"].ToString());
                            RempDetails.employee_name = string.IsNullOrWhiteSpace(drow["employeename"].ToString()) ? "" : drow["employeename"].ToString();
                            RempDetails.chc_centerid = string.IsNullOrWhiteSpace(drow["chc_centerid"].ToString()) ? 0 : Convert.ToInt32(drow["chc_centerid"].ToString());
                            RempDetails.hosname = string.IsNullOrWhiteSpace(drow["hosname"].ToString()) ? "" : drow["hosname"].ToString();
                            RempDetails.amount = string.IsNullOrWhiteSpace(drow["amount"].ToString()) ? "" : drow["amount"].ToString();
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
        public ResponseModel CrToEmplAcc(CHCBillProcess objModel)
        {
            NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString);
            ResponseModel Response = new ResponseModel();
            DataTable dt = new DataTable();
            string Status = "Payment Process";
            try
            {
                // NpgsqlConnection con = new NpgsqlConnection();
                objModel.status = Status;
                using (con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {

                    con.Open();

                    using (NpgsqlCommand cmd = new NpgsqlCommand("select * from mdcl_sp_insert_chc_credit_empl(:emplid,:employee_name,:chc_centerid,:amount,:status)", con))

                    {
                        cmd.Connection = con;

                        cmd.Parameters.AddWithValue("emplid", objModel.emplid);
                        cmd.Parameters.AddWithValue("employee_name", objModel.employee_name);
                        cmd.Parameters.AddWithValue("chc_centerid", objModel.chc_centerid);
                        cmd.Parameters.AddWithValue("amount", objModel.amount);
                        cmd.Parameters.AddWithValue("status", objModel.status);


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
        public List<CHCBillProcess> forwardcheque()
        {
            NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString);
            List<CHCBillProcess> Addlist = new List<CHCBillProcess>();
            DataTable dt = new DataTable();

            try
            {

                using (con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {

                    con.Open();
                    using (NpgsqlCommand cmd = new NpgsqlCommand("select * from mdcl_sp_get_billprocess_for_cheque_forward()", con))
                    {
                        cmd.Connection = con;

                        using (NpgsqlDataAdapter SqDA = new NpgsqlDataAdapter(cmd))
                        {
                            SqDA.Fill(dt);
                        }
                    }

                    if (dt.Rows.Count == 0)
                    {
                        Addlist = new List<CHCBillProcess>();
                    }
                    else
                    {
                        foreach (DataRow drow in dt.Rows)
                        {
                            CHCBillProcess robj = new CHCBillProcess();
                            //robj.id = string.IsNullOrWhiteSpace(drow["id"].ToString()) ? 0 : Convert.ToInt32(drow["id"].ToString());
                            robj.emplid = string.IsNullOrWhiteSpace(drow["emplid"].ToString()) ? 0 : Convert.ToInt32(drow["emplid"].ToString());
                            robj.chc_centerid = string.IsNullOrWhiteSpace(drow["chc_centerid"].ToString()) ? 0 : Convert.ToInt32(drow["chc_centerid"].ToString());
                            robj.hosname = string.IsNullOrWhiteSpace(drow["hosname"].ToString()) ? "" : drow["hosname"].ToString();
                            robj.amount = string.IsNullOrWhiteSpace(drow["amount"].ToString()) ? "" : drow["amount"].ToString();

                            //robj.spouse_chc_centerid = string.IsNullOrWhiteSpace(drow["chc_centid"].ToString()) ? 0 : Convert.ToInt32(drow["chc_centid"].ToString());
                            //robj.phosname = string.IsNullOrWhiteSpace(drow["name"].ToString()) ? "" : drow["name"].ToString();
                            // robj.status = string.IsNullOrWhiteSpace(drow["status"].ToString()) ? "" : drow["status"].ToString();

                            Addlist.Add(robj);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                Addlist = new List<CHCBillProcess>();
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return Addlist;
        }
        public CHCBillProcess GetSingleCHC(CHCBillProcess objModel)
        {
            NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString);
            CHCBillProcess RempDetails = new CHCBillProcess();
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
                    using (NpgsqlCommand cmd = new NpgsqlCommand("select * from  mdcl_sp_get_single_billprocess_for_cheque_forward(:pid)", con))
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
                            RempDetails.emplid = string.IsNullOrWhiteSpace(drow["emplid"].ToString()) ? 0 : Convert.ToInt32(drow["emplid"].ToString());
                            // RempDetails.employee_name = string.IsNullOrWhiteSpace(drow["employeename"].ToString()) ? "" : drow["employeename"].ToString();
                            RempDetails.chc_centerid = string.IsNullOrWhiteSpace(drow["chc_centerid"].ToString()) ? 0 : Convert.ToInt32(drow["chc_centerid"].ToString());
                            RempDetails.hosname = string.IsNullOrWhiteSpace(drow["hosname"].ToString()) ? "" : drow["hosname"].ToString();
                            RempDetails.amount = string.IsNullOrWhiteSpace(drow["amount"].ToString()) ? "" : drow["amount"].ToString();
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
        public ResponseModel forwardcheque(CHCBillProcess objModel)
        {
            NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString);
            ResponseModel Response = new ResponseModel();
            DataTable dt = new DataTable();
            string Status = "forwarded cheque";
            try
            {
                // NpgsqlConnection con = new NpgsqlConnection();
                objModel.status = Status;
                using (con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {

                    con.Open();

                    using (NpgsqlCommand cmd = new NpgsqlCommand("select * from mdcl_sp_insert_chc_chequeforward(:lot_no,:lot_date,:chc_centerid,:amount,:cheque_no,:cheque_date,:status)", con))

                    {
                        cmd.Connection = con;

                        cmd.Parameters.AddWithValue("lot_no", objModel.lot_no);
                        cmd.Parameters.AddWithValue("lot_date", objModel.lot_date);
                        cmd.Parameters.AddWithValue("chc_centerid", objModel.chc_centerid);
                        cmd.Parameters.AddWithValue("amount", objModel.amount);
                        cmd.Parameters.AddWithValue("cheque_no", objModel.cheque_no);
                        cmd.Parameters.AddWithValue("cheque_date", objModel.cheque_date);
                        cmd.Parameters.AddWithValue("status", objModel.status);


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
        public List<CHCRequest> Pastdetail(CHCRequest objModel)
        {
            NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString);
            List<CHCRequest> Addlist = new List<CHCRequest>();
            DataTable dt = new DataTable();

            try
            {
                // CHCRequest obj = new CHCRequest();
                int pid;
                pid = objModel.id;
                objModel.emplid = pid;
                using (con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {

                    con.Open();
                    using (NpgsqlCommand cmd = new NpgsqlCommand("select * from  mdcl_sp_get_chc_request_approve_data_for_pastdetail(:pid)", con))
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
                        Addlist = new List<CHCRequest>();
                    }
                    else
                    {
                        foreach (DataRow drow in dt.Rows)
                        {
                            CHCRequest robj = new CHCRequest();
                            //robj.id = string.IsNullOrWhiteSpace(drow["id"].ToString()) ? 0 : Convert.ToInt32(drow["id"].ToString());
                            robj.emplid = string.IsNullOrWhiteSpace(drow["emplid"].ToString()) ? 0 : Convert.ToInt32(drow["emplid"].ToString());
                            robj.employeename = string.IsNullOrWhiteSpace(drow["employeename"].ToString()) ? "" : drow["employeename"].ToString();
                            robj.date_of_checkup = Convert.ToDateTime(drow["date_of_checkup"].ToString());
                            robj.chc_centerid = string.IsNullOrWhiteSpace(drow["chc_centerid"].ToString()) ? 0 : Convert.ToInt32(drow["chc_centerid"].ToString());
                            robj.hosname = string.IsNullOrWhiteSpace(drow["hosname"].ToString()) ? "" : drow["hosname"].ToString();
                            robj.name_of_spouse = string.IsNullOrWhiteSpace(drow["name_of_spouse"].ToString()) ? "" : drow["name_of_spouse"].ToString();
                            robj.spouse_dt_of_checkup = Convert.ToDateTime(drow["spouse_dt_of_checkup"].ToString());
                            robj.chc_centid = string.IsNullOrWhiteSpace(drow["chc_centid"].ToString()) ? 0 : Convert.ToInt32(drow["chc_centid"].ToString());
                            robj.phosname = string.IsNullOrWhiteSpace(drow["name"].ToString()) ? "" : drow["name"].ToString();
                            robj.status = string.IsNullOrWhiteSpace(drow["status"].ToString()) ? "" : drow["status"].ToString();

                            Addlist.Add(robj);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                Addlist = new List<CHCRequest>();
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return Addlist;
        }
        public List<CHCBillProcess> OfficeNoteData(string lotno)
        {

            List<CHCBillProcess> Addlist = new List<CHCBillProcess>();
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {
                    con.Open();
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("plot_no", lotno);
                    Addlist = con.Query<CHCBillProcess>("select * from mdcl_sp_get_data_for_chc_officenote(:plot_no)", parameters).ToList();
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                Addlist = new List<CHCBillProcess>();
            }
            return Addlist;


        }
        public List<CHCBillProcess> BankAdviseData(string lotno)
        {

            List<CHCBillProcess> Addlist = new List<CHCBillProcess>();
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {
                    con.Open();
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("plot_no", lotno);

                    Addlist = con.Query<CHCBillProcess>("select * from mdcl_sp_get_chc_bill_for_bankadvice(:plot_no)", parameters).ToList();

                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                Addlist = new List<CHCBillProcess>();
            }
            return Addlist;


        }
        public List<CHCBillProcess> EnclosureEmplSummary()
        {

            List<CHCBillProcess> Addlist = new List<CHCBillProcess>();
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {
                    con.Open();
                    Addlist = con.Query<CHCBillProcess>("select * from mdcl_sp_get_cr_to_empl_acc()").ToList();
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                Addlist = new List<CHCBillProcess>();
            }
            return Addlist;


        }
        public List<CHCBillProcess> LotGenration()
        {

            List<CHCBillProcess> Addlist = new List<CHCBillProcess>();
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {
                    con.Open();
                    Addlist = con.Query<CHCBillProcess>("select * from mdcl_sp_get_data_chc_lot()").ToList();
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                Addlist = new List<CHCBillProcess>();
            }
            return Addlist;
        }
        public List<CHCBillProcess> GetLoTNo()
        {
            List<CHCBillProcess> Remplist = new List<CHCBillProcess>();
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {
                    con.Open();
                    Remplist = con.Query<CHCBillProcess>("select * from mdcl_sp_get_chc_lotno()").ToList();
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                Remplist = new List<CHCBillProcess>();
            }
            return Remplist;

        }
        public List<CHCBillProcess> GetdatabyLotNo(CHCBillProcess objmodel)
        {
            List<CHCBillProcess> Remplist = new List<CHCBillProcess>();

            string lotno = string.IsNullOrWhiteSpace(objmodel.lot_no) ? "" : objmodel.lot_no;
            string employeecode = string.IsNullOrWhiteSpace(objmodel.employee_code) ? "" : objmodel.employee_code.ToString();
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {
                    con.Open();
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("lotno", lotno);
                    parameters.Add("p_employeecode", employeecode);
                    Remplist = con.Query<CHCBillProcess>("select * from mdcl_sp_get_data_for_chc_querymodel(:lotno,:p_employeecode)", parameters).ToList();

                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                Remplist = new List<CHCBillProcess>();
            }
            return Remplist;

        }

        public byte[] GetCHCOfficeNotepdf(string lotno)
        {
            DataTable dt = new DataTable();
            var lotn = lotno;
            StringBuilder sb = new StringBuilder();
            string TemplatePath = string.Empty;
            using (NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString))
            {
                con.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("plot_no", lotn);

                List<CHCBillProcess> Remplist = con.Query<CHCBillProcess>("select * from mdcl_sp_get_data_for_chc_officenote(:plot_no)", parameters).ToList();

                foreach (CHCBillProcess model in Remplist)
                {
                    if (model.chc_centerid > 0 && model.chc_centerid != 4)
                    {
                        TemplatePath = System.Configuration.ConfigurationManager.AppSettings["CHCOfficeNotehtmlMum"].ToString();
                        break;
                    }
                    else
                    {
                        TemplatePath = System.Configuration.ConfigurationManager.AppSettings["CHCOfficeNotehtml"].ToString();
                        break;
                    }

                }
                using (StreamReader sr = new StreamReader(TemplatePath))
                {

                    int rownumber = 0;
                    decimal total_amount = 0;
                    string str = total_amount.ToString();
                    String line;
                    string lotDAte = string.Empty;
                    if (Remplist.Count > 0)
                    {
                        if (Remplist[0].lotdate != null)
                        {
                            lotDAte = Remplist[0].lotdate;
                        }
                    }


                    while ((line = sr.ReadLine()) != null)
                    {
                        line = line.Replace("#listDate", lotDAte);
                        //line = line.Replace("#listDate", DateTime.Now.ToString("dd-MMM-yyyy"));
                        line = line.Replace("#lot_no", lotn);
                        //  line = line.Replace("#Amount", str);
                        line = line.Replace("#Amount", total_amount.ToString("##,##,###.00"));
                        if ((line.Trim() == ("<tbody>") && rownumber <= dt.Rows.Count))
                        {


                            foreach (CHCBillProcess model in Remplist)
                            {
                                rownumber++;

                                string tr_line = "<tbody>" + "<tr><td>" + rownumber + "</td><td>" + model.employeecode + "</td><td>" + model.employee_name + "</td><td>" + model.hosname + "</td><td>" + model.bill_no + "</td><td>" + Convert.ToDecimal(model.amount).ToString("##,##,###.00") + "</td></tr>";
                                sb.AppendLine(tr_line);
                                total_amount += Convert.ToDecimal(model.amount);
                            }

                        }

                        else if (rownumber == Remplist.Count)
                        {
                            string tr_line = "<tr><td></td><td></td><td></td><td></td><td>Total</td><td>" + total_amount.ToString("##,##,###.00") + "</td></tr>";
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


            }
            return CommonHelper.Convert2(sb.ToString());
        }

        public byte[] GetCHCBankadvicepdf(string lotno)
        {
            DataTable dt = new DataTable();
            var lotn = lotno;
            StringBuilder sb = new StringBuilder();
            using (NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString))
            {
                con.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("plot_no", lotn);

                int is_lot_generated_for_branch = con.Query<int>("select * from mdcl_sp_check_is_lot_generated_for_branch(:plot_no)", parameters).FirstOrDefault();

                if (is_lot_generated_for_branch == 0)
                {
                    List<CHCBillProcess> Remplist = con.Query<CHCBillProcess>("select * from mdcl_sp_get_chc_bill_for_bankadvice_new(:plot_no)", parameters).ToList();
                    using (StreamReader sr = new StreamReader(System.Configuration.ConfigurationManager.AppSettings["CHCBankAdvisehtml_MUMBAI"].ToString()))
                    {

                        int rownumber = 0;
                        decimal total_amount = 0;
                        string str = total_amount.ToString();
                        String line;
                        foreach (CHCBillProcess model in Remplist)
                        {
                            // rownumber++;
                            total_amount += Convert.ToDecimal(model.amount);
                        }
                        string lotDAte = string.Empty;
                        //Added by Amit
                        if (Remplist.Count > 0)
                        {
                            if (Remplist[0].lotdate != null)
                            {
                                lotDAte = Remplist[0].lotdate;
                            }
                            else
                            {
                                lotDAte = DateTime.Now.ToString("dd/MM/yyyy");
                            }
                        }
                        //End

                        while ((line = sr.ReadLine()) != null)
                        {
                            line = line.Replace("#listDate", lotDAte);
                            //line = line.Replace("#listDate", DateTime.Now.ToString("dd-MMM-yyyy"));
                            line = line.Replace("#lot_no", lotn);
                            //  line = line.Replace("#Amount", str);
                            line = line.Replace("#Amount", total_amount.ToString("##,##,###.00"));
                            if ((line.Trim() == ("<tbody>") && rownumber <= dt.Rows.Count))
                            {

                                foreach (CHCBillProcess model in Remplist)
                                {
                                    rownumber++;
                                    string tempstr = (string.IsNullOrWhiteSpace(model.hosp_bank_name) ? "" : model.hosp_bank_name.ToString()) +
                                        (string.IsNullOrWhiteSpace(model.hosp_bank_accno) ? " " : " AC NO:" + model.hosp_bank_accno.ToString()) +
                                        (string.IsNullOrWhiteSpace(model.bank_ifsc_code) ? " " : " IFSC CODE:" + model.bank_ifsc_code.ToString());
                                    //(string.IsNullOrWhiteSpace(model.address) ? " " : " " + model.address.ToString());
                                    string tr_line = "<tbody>" + "<tr><td>" + rownumber + "</td><td><b>" + model.hosname + "</b></td><td><b>" + Convert.ToDecimal(model.amount).ToString("##,##,###.00") + "</b></td><td>" + tempstr + "</td></tr>";
                                    sb.AppendLine(tr_line);
                                    //total_amount += Convert.ToDecimal(model.amount);
                                }

                            }

                            else if (rownumber == Remplist.Count)
                            {
                                string tr_line = "<tr><td></td><td><center>Total</center></td><td>" + total_amount.ToString("##,##,###.00") + "</td><td></td></tr>";
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
                }
                else
                {
                    List<CHCBillProcess> Remplist = con.Query<CHCBillProcess>("select * from mdcl_sp_get_chc_bill_for_bankadvice_branch(:plot_no)", parameters).ToList();
                    using (StreamReader sr = new StreamReader(System.Configuration.ConfigurationManager.AppSettings["CHCBankAdvisehtml_BRANCH"].ToString()))
                    {

                        int rownumber = 0;
                        decimal total_amount = 0;
                        string str = total_amount.ToString();
                        String line;
                        foreach (CHCBillProcess model in Remplist)
                        {
                            // rownumber++;
                            string val_amt = model.amount.Replace('$', ' ').TrimEnd();
                            total_amount += Convert.ToDecimal(val_amt);
                        }
                        string lotDAte = string.Empty;
                        //Added by Amit
                        if (Remplist.Count > 0)
                        {
                            if (Remplist[0].lotdate != null)
                            {
                                lotDAte = Remplist[0].lotdate;
                            }
                            else
                            {
                                lotDAte = DateTime.Now.ToString("dd/MM/yyyy");
                            }
                        }
                        //End
                        while ((line = sr.ReadLine()) != null)
                        {
                            //line = line.Replace("#listDate", DateTime.Now.ToString("dd-MMM-yyyy"));
                            line = line.Replace("#listDate", lotDAte);
                            line = line.Replace("#lot_no", lotn);
                            //  line = line.Replace("#Amount", str);
                            line = line.Replace("#Amount", total_amount.ToString("##,##,###.00"));
                            if ((line.Trim() == ("<tbody>") && rownumber <= dt.Rows.Count))
                            {

                                foreach (CHCBillProcess model in Remplist)
                                {
                                    string val_amt_disp = model.amount.Replace('$', ' ').TrimEnd();
                                    rownumber++;
                                    string tempstr = (string.IsNullOrWhiteSpace(model.emp_bank_name) ? "" : model.emp_bank_name.ToString()) +
                                        (string.IsNullOrWhiteSpace(model.emp_account_no) ? " " : " AC NO:" + model.emp_account_no.ToString()) +
                                        (string.IsNullOrWhiteSpace(model.emp_ifsc_code) ? " " : " IFSC CODE:" + model.emp_ifsc_code.ToString());
                                    //(string.IsNullOrWhiteSpace(model.address) ? " " : " " + model.address.ToString());
                                    //string tr_line = "<tbody>" + "<tr><td>" + rownumber + "</td><td><b>" + model.employee_name + "</b></td><td><b>" + model.hosname + "</b></td><td><b>" + Convert.ToDecimal(val_amt_disp).ToString("##,##,###.00") + "</b></td><td>" + tempstr + "</td></tr>";

                                    string tr_line = "<tbody>" + "<tr><td>" + rownumber + "</td><td><b>" + model.employee_name + "</b></td><td><b>" + Convert.ToDecimal(val_amt_disp).ToString("##,##,###.00") + "</b></td><td>" + tempstr + "</td></tr>";
                                    sb.AppendLine(tr_line);
                                    //total_amount += Convert.ToDecimal(model.amount);
                                }

                            }

                            else if (rownumber == Remplist.Count)
                            {
                                string tr_line = "<tr><td></td><td><center>Total</center></td><td>" + total_amount.ToString("##,##,###.00") + "</td><td></td></tr>";
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
                }
            }
            return CommonHelper.Convert2(sb.ToString());
        }
    }
}