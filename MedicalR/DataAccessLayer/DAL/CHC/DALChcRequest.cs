using Dapper;
using MedicalR.CustomHelper;
using MedicalR.DataAccessLayer.IDAL.CHC;
using MedicalR.Models;
using MedicalR.Models.CHC;
using MedicalR.Models.MedicalR;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using MedicalR.EmailSettings;

namespace MedicalR.DataAccessLayer.DAL.CHC
{
    public class DALChcRequest : IDALChcRequest
    {
        public List<Hospitallist> GetHospitallist(Hospitallist objModel)
        {
            List<Hospitallist> GetHospitallist = new List<Hospitallist>();
            NpgsqlConnection con = new NpgsqlConnection(CustomHelper.CommonHelper.GetConnectionString);
            try
            {


                NpgsqlCommand cmd = new NpgsqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "Select id, name from mdcl_tbl_hospital_mast where isactive=true order by name asc";
                cmd.CommandType = System.Data.CommandType.Text;
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                if (ds.Tables[0].Rows.Count == 0)
                {
                    GetHospitallist = null;
                }
                else
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Hospitallist hospitallist = new Hospitallist();
                        hospitallist.hospital_id = Convert.ToInt32(ds.Tables[0].Rows[i]["id"].ToString());
                        hospitallist.name = ds.Tables[0].Rows[i]["name"].ToString();
                        GetHospitallist.Add(hospitallist);

                    }

                }
            }
            catch (Exception ex)

            {
                ExceptionLogging.LogException(ex);
                GetHospitallist = new List<Hospitallist>();
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return GetHospitallist;
        }

        public ResponseModel AddCHCRequest(CHCRequest objModel)
        {
            NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString);
            ResponseModel Response = new ResponseModel();
            DataTable dt = new DataTable();
            string Status = "Applied";

            try
            {
                ResponseModel resp = CheckPriviousClaim(objModel);
                if (!resp.Status)
                {
                    return resp;
                }
                // NpgsqlConnection con = new NpgsqlConnection();
                objModel.status = Status;
                using (con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {

                    con.Open();
                    using (NpgsqlCommand cmd = new NpgsqlCommand("select * from mdcl_sp_insert_chcrequest(:pid,:emplid,:mob_no,:office_tel_no,:date_of_birth,:age,:date_of_checkup,:chc_centerid,:name_of_spouse,:dob_spouse,:spouse_age,:spouse_dt_of_checkup,:chc_centid,:status,:relationship,:created_by,:otherchc_empl,:otherchc_sps,:is_save_for_later)", con))

                    {
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("pid", objModel.id);
                        cmd.Parameters.AddWithValue("emplid", objModel.emplid);
                        cmd.Parameters.AddWithValue("mob_no", string.IsNullOrEmpty(objModel.mob_no) ? "" : objModel.mob_no);
                        cmd.Parameters.AddWithValue("office_tel_no", string.IsNullOrEmpty(objModel.office_tel_no) ? "" : objModel.office_tel_no);
                        //cmd.Parameters.AddWithValue("date_of_birth", objModel.date_of_birth);
                        if (objModel.date_of_birth != null)
                        {
                            cmd.Parameters.AddWithValue("date_of_birth", objModel.date_of_birth);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("date_of_birth", new DateTime(1900, 01, 01));
                        }
                        cmd.Parameters.AddWithValue("age", string.IsNullOrEmpty(objModel.age) ? "" : objModel.age);

                        if (objModel.date_of_checkup != null)
                        {
                            cmd.Parameters.AddWithValue("date_of_checkup", objModel.date_of_checkup);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("date_of_checkup", new DateTime(1900, 01, 01));
                        }
                        cmd.Parameters.AddWithValue("chc_centerid", objModel.chc_centerid);
                        cmd.Parameters.AddWithValue("name_of_spouse", string.IsNullOrEmpty(objModel.name_of_spouse) ? "" : objModel.name_of_spouse);

                        if (objModel.dob_spouse != null)
                        {
                            cmd.Parameters.AddWithValue("dob_spouse", objModel.dob_spouse);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("dob_spouse", new DateTime(1900, 01, 01));
                        }
                        cmd.Parameters.AddWithValue("spouse_age", string.IsNullOrEmpty(objModel.spouse_age) ? "" : objModel.spouse_age);
                        cmd.Parameters.AddWithValue("spouse_dt_of_checkup", objModel.spouse_dt_of_checkup);
                        if (objModel.spouse_dt_of_checkup != null)
                        {
                            cmd.Parameters.AddWithValue("spouse_dt_of_checkup", objModel.spouse_dt_of_checkup);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("spouse_dt_of_checkup", new DateTime(1900, 01, 01));
                        }
                        cmd.Parameters.AddWithValue("chc_centid", objModel.chc_centid);
                        cmd.Parameters.AddWithValue("status", objModel.status);
                        cmd.Parameters.AddWithValue("relationship", objModel.relationship);
                        cmd.Parameters.AddWithValue("created_by", objModel.created_by);
                        cmd.Parameters.AddWithValue("otherchc_empl", string.IsNullOrEmpty(objModel.otherchc_empl) ? "" : objModel.otherchc_empl);
                        cmd.Parameters.AddWithValue("otherchc_sps", string.IsNullOrEmpty(objModel.otherchc_sps) ? "" : objModel.otherchc_sps);
                        cmd.Parameters.AddWithValue("is_save_for_later", Convert.ToBoolean(objModel.is_save_for_later));
                        //cmd.Parameters.AddWithValue("is_save_for_later", 0);

                        int Res = (int)(cmd.ExecuteScalar());
                        bool is_success = false;
                        CommonHelper.write_log("save");
                        if (Res > 0)
                        {
                            if (!objModel.is_save_for_later)
                            {
                                is_success = EmailProcess.SendMail_CHC_Applied(Res, objModel.relationship, con);
                            }
                            else
                            {
                                is_success = true;
                            }

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
        public List<CHCRequest> chcrequest_data(string empcode)
        {

            CommonHelper.write_log("=== chcrequest_data START ===");
            CommonHelper.write_log("Input empcode: " + empcode);

            NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString);
            List<CHCRequest> Addlist = new List<CHCRequest>();
            DataTable dt = new DataTable();

            try
            {
                using (con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {

                    con.Open();
                    using (NpgsqlCommand cmd = new NpgsqlCommand("select * from  mdcl_sp_get_chcrequest_data(:pid)", con))
                    {
                        CommonHelper.write_log("Executing SP: mdcl_sp_get_chcrequest_data");

                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("pid", empcode);
                        using (NpgsqlDataAdapter SqDA = new NpgsqlDataAdapter(cmd))
                        {
                            SqDA.Fill(dt);

                            CommonHelper.write_log("chcrequest_data(emplcode) : dt count :" + dt.Rows.Count);
                        }
                    }

                    if (dt.Rows.Count == 0)
                    {
                        Addlist = new List<CHCRequest>();
                        CommonHelper.write_log("No Data Found for empcode: " + empcode);
                    }
                    else
                    {
                        foreach (DataRow drow in dt.Rows)
                        {
                            CHCRequest robj = new CHCRequest();
                            robj.relationship = string.IsNullOrWhiteSpace(drow["relationship"].ToString()) ? "" : drow["relationship"].ToString();

                            if (robj.relationship == "Self and Spouse")
                            {
                                CommonHelper.write_log("Entering Self and Spouse Block");

                                robj.id = string.IsNullOrWhiteSpace(drow["id"].ToString()) ? 0 : Convert.ToInt32(drow["id"].ToString());
                                robj.emplid = string.IsNullOrWhiteSpace(drow["emplid"].ToString()) ? 0 : Convert.ToInt32(drow["emplid"].ToString());
                                robj.employeename = string.IsNullOrWhiteSpace(drow["employeename"].ToString()) ? "" : drow["employeename"].ToString();
                                robj.date_of_checkup = Convert.ToDateTime(drow["date_of_checkup"].ToString());

                                // Addeddd BY Durgesh 
                                //robj.date_of_checkup = drow["date_of_checkup"] == DBNull.Value? (DateTime?)null: Convert.ToDateTime(drow["date_of_checkup"]);


                                robj.chc_centerid = string.IsNullOrWhiteSpace(drow["chc_centerid"].ToString()) ? 0 : Convert.ToInt32(drow["chc_centerid"].ToString());
                                robj.hosname = string.IsNullOrWhiteSpace(drow["hosname"].ToString()) ? "" : drow["hosname"].ToString();
                                robj.name_of_spouse = string.IsNullOrWhiteSpace(drow["name_of_spouse"].ToString()) ? "" : drow["name_of_spouse"].ToString();
                                robj.spouse_dt_of_checkup = Convert.ToDateTime(drow["spouse_dt_of_checkup"].ToString());
                                robj.chc_centid = string.IsNullOrWhiteSpace(drow["chc_centid"].ToString()) ? 0 : Convert.ToInt32(drow["chc_centid"].ToString());
                                robj.phosname = string.IsNullOrWhiteSpace(drow["name"].ToString()) ? "" : drow["name"].ToString();
                                robj.created_by = string.IsNullOrWhiteSpace(drow["created_by"].ToString()) ? "" : drow["created_by"].ToString();

                                Addlist.Add(robj);
                            }
                            if (robj.relationship == "self")
                            {
                                CommonHelper.write_log("Entering Self Block");
                                robj.id = string.IsNullOrWhiteSpace(drow["id"].ToString()) ? 0 : Convert.ToInt32(drow["id"].ToString());
                                robj.emplid = string.IsNullOrWhiteSpace(drow["emplid"].ToString()) ? 0 : Convert.ToInt32(drow["emplid"].ToString());
                                robj.employeename = string.IsNullOrWhiteSpace(drow["employeename"].ToString()) ? "" : drow["employeename"].ToString();
                                robj.date_of_checkup = Convert.ToDateTime(drow["date_of_checkup"].ToString());
                                robj.chc_centerid = string.IsNullOrWhiteSpace(drow["chc_centerid"].ToString()) ? 0 : Convert.ToInt32(drow["chc_centerid"].ToString());
                                robj.hosname = string.IsNullOrWhiteSpace(drow["hosname"].ToString()) ? "" : drow["hosname"].ToString();
                                robj.created_by = string.IsNullOrWhiteSpace(drow["created_by"].ToString()) ? "" : drow["created_by"].ToString();

                                Addlist.Add(robj);
                            }
                            if (robj.relationship == "spouse")
                            {
                                CommonHelper.write_log("Entering Spouse Block");
                                robj.id = string.IsNullOrWhiteSpace(drow["id"].ToString()) ? 0 : Convert.ToInt32(drow["id"].ToString());
                                robj.emplid = string.IsNullOrWhiteSpace(drow["emplid"].ToString()) ? 0 : Convert.ToInt32(drow["emplid"].ToString());
                                robj.employeename = string.IsNullOrWhiteSpace(drow["employeename"].ToString()) ? "" : drow["employeename"].ToString();
                                robj.name_of_spouse = string.IsNullOrWhiteSpace(drow["name_of_spouse"].ToString()) ? "" : drow["name_of_spouse"].ToString();
                                robj.spouse_dt_of_checkup = Convert.ToDateTime(drow["spouse_dt_of_checkup"].ToString());
                                robj.chc_centid = string.IsNullOrWhiteSpace(drow["chc_centid"].ToString()) ? 0 : Convert.ToInt32(drow["chc_centid"].ToString());
                                robj.phosname = string.IsNullOrWhiteSpace(drow["name"].ToString()) ? "" : drow["name"].ToString();
                                robj.created_by = string.IsNullOrWhiteSpace(drow["created_by"].ToString()) ? "" : drow["created_by"].ToString();

                                Addlist.Add(robj);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CommonHelper.write_log("ERROR OCCURRED in chcrequest_data");
                CommonHelper.write_log("Message: " + ex.Message);
                CommonHelper.write_log("StackTrace: " + ex.StackTrace);
                ExceptionLogging.LogException("chcrequest_data(emplcode) :", ex);
                Addlist = new List<CHCRequest>();
            }
            finally
            {
                CommonHelper.write_log("Closing DB Connection...");
                con.Close();
                con.Dispose();
                CommonHelper.write_log("=== chcrequest_data END ===");
            }
            return Addlist;
        }
        public CHCRequest Getsingleempdetails(CHCRequest objModel)
        {

            NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString);
            CHCRequest RempDetails = new CHCRequest();
            DataTable dt = new DataTable();
            // int id;
            try
            {
               // CommonHelper.write_log("Getsingleempdetails() called for ID: " + objModel.id);

                // id = objModel.id;
                // objModel.emplid = id;
                using (con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {

                    con.Open();
                    using (NpgsqlCommand cmd = new NpgsqlCommand("select * from  mdcl_sp_get_single_chcrequest_data(:pid)", con))
                    {

                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("pid", objModel.id);
                        CommonHelper.write_log("Executing query for ID=" + objModel.id);

                        using (NpgsqlDataAdapter SqDA = new NpgsqlDataAdapter(cmd))
                        {
                            SqDA.Fill(dt);
                        }
                    }
                    CommonHelper.write_log("Rows returned from DB: " + dt.Rows.Count);

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
                                RempDetails.id = string.IsNullOrWhiteSpace(drow["id"].ToString()) ? 0 : Convert.ToInt32(drow["id"].ToString());
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
                                RempDetails.is_save_for_later = Convert.ToBoolean(drow["is_save_for_later"]);
                                RempDetails.created_by = string.IsNullOrWhiteSpace(drow["created_by"].ToString()) ? "" : drow["created_by"].ToString();
                               
                            }
                            if (RempDetails.relationship == "self")
                            {
                                //  HospitalListModel arnobj = new HospitalListModel();
                                RempDetails.id = string.IsNullOrWhiteSpace(drow["id"].ToString()) ? 0 : Convert.ToInt32(drow["id"].ToString());
                                RempDetails.emplid = string.IsNullOrWhiteSpace(drow["emplid"].ToString()) ? 0 : Convert.ToInt32(drow["emplid"].ToString());
                                RempDetails.employeename = string.IsNullOrWhiteSpace(drow["employeename"].ToString()) ? "" : drow["employeename"].ToString();
                                RempDetails.mob_no = string.IsNullOrWhiteSpace(drow["mob_no"].ToString()) ? "" : drow["mob_no"].ToString();
                                RempDetails.office_tel_no = string.IsNullOrWhiteSpace(drow["office_tel_no"].ToString()) ? "" : drow["office_tel_no"].ToString();
                                RempDetails.date_of_birth = Convert.ToDateTime(drow["date_of_birth"].ToString());
                                RempDetails.age = string.IsNullOrWhiteSpace(drow["age"].ToString()) ? "" : drow["age"].ToString();
                                RempDetails.date_of_checkup = Convert.ToDateTime(drow["date_of_checkup"].ToString());
                                RempDetails.chc_centerid = string.IsNullOrWhiteSpace(drow["chc_centerid"].ToString()) ? 0 : Convert.ToInt32(drow["chc_centerid"].ToString());
                                RempDetails.hosname = string.IsNullOrWhiteSpace(drow["hosname"].ToString()) ? "" : drow["hosname"].ToString();
                                RempDetails.is_save_for_later = Convert.ToBoolean(drow["is_save_for_later"]);
                                RempDetails.created_by = string.IsNullOrWhiteSpace(drow["created_by"].ToString()) ? "" : drow["created_by"].ToString();

                            }
                            if (RempDetails.relationship == "spouse")
                            {
                                //  HospitalListModel arnobj = new HospitalListModel();
                                RempDetails.id = string.IsNullOrWhiteSpace(drow["id"].ToString()) ? 0 : Convert.ToInt32(drow["id"].ToString());
                                RempDetails.emplid = string.IsNullOrWhiteSpace(drow["emplid"].ToString()) ? 0 : Convert.ToInt32(drow["emplid"].ToString());
                                RempDetails.employeename = string.IsNullOrWhiteSpace(drow["employeename"].ToString()) ? "" : drow["employeename"].ToString();
                                RempDetails.name_of_spouse = string.IsNullOrWhiteSpace(drow["name_of_spouse"].ToString()) ? "" : drow["name_of_spouse"].ToString();
                                RempDetails.dob_spouse = Convert.ToDateTime(drow["dob_spouse"].ToString());
                                RempDetails.spouse_age = string.IsNullOrWhiteSpace(drow["spouse_age"].ToString()) ? "" : drow["spouse_age"].ToString();
                                RempDetails.spouse_dt_of_checkup = Convert.ToDateTime(drow["spouse_dt_of_checkup"].ToString());
                                RempDetails.chc_centid = string.IsNullOrWhiteSpace(drow["chc_centid"].ToString()) ? 0 : Convert.ToInt32(drow["chc_centid"].ToString());
                                RempDetails.phosname = string.IsNullOrWhiteSpace(drow["name"].ToString()) ? "" : drow["name"].ToString();
                                RempDetails.is_save_for_later = Convert.ToBoolean(drow["is_save_for_later"]);
                                RempDetails.created_by = string.IsNullOrWhiteSpace(drow["created_by"].ToString()) ? "" : drow["created_by"].ToString();

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
        public ResponseModel ApproveCHCRequest(CHCRequest objModel)
        {
            NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString);
            ResponseModel Response = new ResponseModel();
            DataTable dt = new DataTable();
            string Status = "Applied";
            try
            {
                // NpgsqlConnection con = new NpgsqlConnection();
                objModel.status = Status;
                using (con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {

                    con.Open();

                    using (NpgsqlCommand cmd = new NpgsqlCommand("select * from mdcl_sp_update_CHCRequest(:pid,:pmob_no,:poffice_tel_no,:pdate_of_birth,:page,:pdate_of_checkup,:pchc_centerid,:pname_of_spouse,:pdob_spouse,:pspouse_age,:pspouse_dt_of_checkup,:pchc_centid,:pstatus,:prelationship)", con))

                    {
                        cmd.Connection = con;

                        cmd.Parameters.AddWithValue("pid", objModel.id);
                        cmd.Parameters.AddWithValue("pmob_no", string.IsNullOrEmpty(objModel.mob_no) ? "" : objModel.mob_no);
                        cmd.Parameters.AddWithValue("poffice_tel_no", string.IsNullOrEmpty(objModel.office_tel_no) ? "" : objModel.office_tel_no);
                        //cmd.Parameters.AddWithValue("date_of_birth", objModel.date_of_birth);
                        if (objModel.date_of_birth != null)
                        {
                            cmd.Parameters.AddWithValue("pdate_of_birth", objModel.date_of_birth);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("pdate_of_birth", new DateTime(1900, 01, 01));
                        }
                        cmd.Parameters.AddWithValue("page", string.IsNullOrEmpty(objModel.age) ? "" : objModel.age);

                        if (objModel.date_of_checkup != null)
                        {
                            cmd.Parameters.AddWithValue("pdate_of_checkup", objModel.date_of_checkup);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("pdate_of_checkup", new DateTime(1900, 01, 01));
                        }
                        cmd.Parameters.AddWithValue("pchc_centerid", objModel.chc_centerid);
                        cmd.Parameters.AddWithValue("pname_of_spouse", string.IsNullOrEmpty(objModel.name_of_spouse) ? "" : objModel.name_of_spouse);
                        cmd.Parameters.AddWithValue("pdob_spouse", objModel.dob_spouse);
                        if (objModel.dob_spouse != null)
                        {
                            cmd.Parameters.AddWithValue("pdob_spouse", objModel.dob_spouse);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("pdob_spouse", new DateTime(1900, 01, 01));
                        }
                        cmd.Parameters.AddWithValue("pspouse_age", string.IsNullOrEmpty(objModel.spouse_age) ? "" : objModel.spouse_age);
                        cmd.Parameters.AddWithValue("pspouse_dt_of_checkup", objModel.spouse_dt_of_checkup);
                        if (objModel.spouse_dt_of_checkup != null)
                        {
                            cmd.Parameters.AddWithValue("pspouse_dt_of_checkup", objModel.spouse_dt_of_checkup);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("pspouse_dt_of_checkup", new DateTime(1900, 01, 01));
                        }
                        cmd.Parameters.AddWithValue("pchc_centid", objModel.chc_centid);
                        cmd.Parameters.AddWithValue("pstatus", objModel.status);
                        cmd.Parameters.AddWithValue("prelationship", objModel.relationship);

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
                    using (NpgsqlCommand cmd = new NpgsqlCommand("select * from  mdcl_sp_get_chcrequest_for_pastdetail(:pid)", con))
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
                            robj.relationship = string.IsNullOrWhiteSpace(drow["relationship"].ToString()) ? "" : drow["relationship"].ToString();
                            if (robj.relationship == "Self and Spouse")
                            {
                                // robj.id = string.IsNullOrWhiteSpace(drow["id"].ToString()) ? 0 : Convert.ToInt32(drow["id"].ToString());
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
                            if (robj.relationship == "self")
                            {
                                // robj.id = string.IsNullOrWhiteSpace(drow["id"].ToString()) ? 0 : Convert.ToInt32(drow["id"].ToString());
                                robj.emplid = string.IsNullOrWhiteSpace(drow["emplid"].ToString()) ? 0 : Convert.ToInt32(drow["emplid"].ToString());
                                robj.employeename = string.IsNullOrWhiteSpace(drow["employeename"].ToString()) ? "" : drow["employeename"].ToString();
                                robj.date_of_checkup = Convert.ToDateTime(drow["date_of_checkup"].ToString());
                                robj.chc_centerid = string.IsNullOrWhiteSpace(drow["chc_centerid"].ToString()) ? 0 : Convert.ToInt32(drow["chc_centerid"].ToString());
                                robj.hosname = string.IsNullOrWhiteSpace(drow["hosname"].ToString()) ? "" : drow["hosname"].ToString();
                                robj.status = string.IsNullOrWhiteSpace(drow["status"].ToString()) ? "" : drow["status"].ToString();

                                Addlist.Add(robj);
                            }
                            if (robj.relationship == "spouse")
                            {
                                // robj.id = string.IsNullOrWhiteSpace(drow["id"].ToString()) ? 0 : Convert.ToInt32(drow["id"].ToString());
                                robj.emplid = string.IsNullOrWhiteSpace(drow["emplid"].ToString()) ? 0 : Convert.ToInt32(drow["emplid"].ToString());
                                robj.employeename = string.IsNullOrWhiteSpace(drow["employeename"].ToString()) ? "" : drow["employeename"].ToString();
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
        public ResponseModel CheckPriviousClaim(CHCRequest objModel)
        {
            NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString);
            CHCRequest RempDetails = new CHCRequest();
            ResponseModel Response = new ResponseModel();
            DataTable dt = new DataTable();
            // DataTable dtt = new DataTable();
            int id;
            int emp_age = 0;
            int spouse_age = 0;
            DateTime dateofchecheckup;
            string relship = "";
            if (objModel.relationship == "self")
            {
                relship = "Self and Spouse";
            }
            if (objModel.relationship == "spouse")
            {
                relship = "Self and Spouse";
            }
            if (objModel.relationship == "Self and Spouse")
            {
                relship = "self";
            }


            try
            {
                // NpgsqlConnection con = new NpgsqlConnection();
                // objModel.emplid = id;
                id = objModel.emplid;
                if (objModel.relationship == "spouse")
                {
                    dateofchecheckup = objModel.spouse_dt_of_checkup;
                }
                else
                {
                    dateofchecheckup = objModel.date_of_checkup;
                }

                using (con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {

                    con.Open();
                    using (NpgsqlCommand cmd = new NpgsqlCommand("select * from  mdcl_sp_Check_privious_claim_date(:pid,:prelationship)", con))
                    {

                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("pid", objModel.emplid);
                        cmd.Parameters.AddWithValue("prelationship", objModel.relationship);
                        using (NpgsqlDataAdapter SqDA = new NpgsqlDataAdapter(cmd))
                        {
                            SqDA.Fill(dt);
                        }
                    }
                    if (dt.Rows.Count == 0)
                    {
                        using (NpgsqlCommand cmd = new NpgsqlCommand("select * from  mdcl_sp_Check_privious_claim_date(:pid,:prelationship)", con))
                        {

                            cmd.Connection = con;
                            cmd.Parameters.AddWithValue("pid", objModel.emplid);
                            cmd.Parameters.AddWithValue("prelationship", relship);
                            using (NpgsqlDataAdapter SqDA = new NpgsqlDataAdapter(cmd))
                            {
                                SqDA.Fill(dt);
                            }
                        }
                    }

                    if (dt.Rows.Count == 0)
                    {


                        RempDetails = null;
                        Response.Status = true;


                    }
                    else
                    {

                        foreach (DataRow drow in dt.Rows)
                        {
                            //  HospitalListModel arnobj = new HospitalListModel();
                            // RempDetails.id = string.IsNullOrWhiteSpace(drow["id"].ToString()) ? 0 : Convert.ToInt32(drow["id"].ToString());
                            var relationship = RempDetails.relationship = string.IsNullOrWhiteSpace(drow["relationship"].ToString()) ? "" : drow["relationship"].ToString();
                            if (relationship == "Self and Spouse")
                            {
                                RempDetails.emplid = string.IsNullOrWhiteSpace(drow["emplid"].ToString()) ? 0 : Convert.ToInt32(drow["emplid"].ToString());
                                RempDetails.date_of_checkup = Convert.ToDateTime(drow["date_of_checkup"].ToString());
                                RempDetails.age = string.IsNullOrWhiteSpace(drow["age"].ToString()) ? "" : drow["age"].ToString();
                                emp_age = Convert.ToInt32(RempDetails.age);

                                RempDetails.spouse_dt_of_checkup = Convert.ToDateTime(drow["spouse_dt_of_checkup"].ToString());
                                RempDetails.spouse_age = string.IsNullOrWhiteSpace(drow["spouse_age"].ToString()) ? "" : drow["spouse_age"].ToString();
                                spouse_age = Convert.ToInt32(RempDetails.spouse_age);

                            }
                            if (relationship == "self")
                            {
                                RempDetails.emplid = string.IsNullOrWhiteSpace(drow["emplid"].ToString()) ? 0 : Convert.ToInt32(drow["emplid"].ToString());

                                RempDetails.age = string.IsNullOrWhiteSpace(drow["age"].ToString()) ? "" : drow["age"].ToString();
                                emp_age = Convert.ToInt32(RempDetails.age);
                                RempDetails.date_of_checkup = Convert.ToDateTime(drow["date_of_checkup"].ToString());

                            }
                            if (relationship == "spouse")
                            {
                                RempDetails.emplid = string.IsNullOrWhiteSpace(drow["emplid"].ToString()) ? 0 : Convert.ToInt32(drow["emplid"].ToString());
                                emp_age = string.IsNullOrWhiteSpace(drow["age"].ToString()) ? 0 : Convert.ToInt32(drow["age"]);

                                RempDetails.spouse_age = string.IsNullOrWhiteSpace(drow["spouse_age"].ToString()) ? "" : drow["spouse_age"].ToString();
                                spouse_age = Convert.ToInt32(RempDetails.spouse_age);

                                RempDetails.spouse_dt_of_checkup = Convert.ToDateTime(drow["spouse_dt_of_checkup"].ToString());


                            }
                        }
                        if (RempDetails.relationship == "spouse")
                        {
                            if ((emp_age >= 40 && emp_age <= 50))
                            {
                                //var Differenceindays = (dateofchecheckup.Date - RempDetails.date_of_checkup.Date).Days;
                                var Differenceindays = dateofchecheckup.AddYears(-2);
                                if (Differenceindays <= RempDetails.spouse_dt_of_checkup)
                                {
                                    Response.Status = false;
                                    Response.Message = MessageHelper.ClaimStatus;
                                }
                                else
                                {
                                    Response.Status = true;
                                }

                            }
                            else if (emp_age > 50)
                            {
                                // var Differenceindays = (dateofchecheckup.Date - RempDetails.date_of_checkup.Date).Days;
                                var Differenceindays = dateofchecheckup.AddYears(-1);
                                if (Differenceindays <= RempDetails.spouse_dt_of_checkup)
                                {
                                    Response.Status = false;
                                    Response.Message = MessageHelper.ClaimStatus;
                                }
                                else
                                {
                                    Response.Status = true;
                                }

                            }
                        }
                        else
                        {
                            if (emp_age <= 40)
                            {
                                //var Differenceindays = (dateofchecheckup.Date - RempDetails.date_of_checkup.Date).Days;
                                var Differenceindays = dateofchecheckup.AddYears(-2);
                                if (Differenceindays <= RempDetails.date_of_checkup)
                                {
                                    Response.Status = false;
                                    Response.Message = MessageHelper.ClaimStatus;
                                }
                                else
                                {
                                    Response.Status = true;
                                }

                            }
                            if (emp_age >= 40)
                            {
                                // var Differenceindays = (dateofchecheckup.Date - RempDetails.date_of_checkup.Date).Days;
                                var Differenceindays = dateofchecheckup.AddYears(-1);
                                if (Differenceindays <= RempDetails.date_of_checkup)
                                {
                                    Response.Status = false;
                                    Response.Message = MessageHelper.ClaimStatus;
                                }
                                else
                                {
                                    Response.Status = true;
                                }

                            }
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
        public ResponseModel SaveForLater(CHCRequest objModel)
        {
            NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString);
            ResponseModel Response = new ResponseModel();
            DataTable dt = new DataTable();
            string Status = "SaveLater";
            try
            {
                // NpgsqlConnection con = new NpgsqlConnection();
                objModel.status = Status;
                using (con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {

                    con.Open();

                    using (NpgsqlCommand cmd = new NpgsqlCommand("select * from mdcl_sp_insert_CHCRequest(:emplid,:mob_no,:office_tel_no,:date_of_birth,:age,:date_of_checkup,:chc_centerid,:name_of_spouse,:dob_spouse,:spouse_age,:spouse_dt_of_checkup,:chc_centid,:status,:relationship)", con))

                    {
                        cmd.Connection = con;

                        cmd.Parameters.AddWithValue("emplid", objModel.emplid);
                        cmd.Parameters.AddWithValue("mob_no", string.IsNullOrEmpty(objModel.mob_no) ? "" : objModel.mob_no);
                        cmd.Parameters.AddWithValue("office_tel_no", string.IsNullOrEmpty(objModel.office_tel_no) ? "" : objModel.office_tel_no);
                        //cmd.Parameters.AddWithValue("date_of_birth", objModel.date_of_birth);
                        if (objModel.date_of_birth != null)
                        {
                            cmd.Parameters.AddWithValue("date_of_birth", objModel.date_of_birth);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("date_of_birth", new DateTime(1900, 01, 01));
                        }
                        cmd.Parameters.AddWithValue("age", string.IsNullOrEmpty(objModel.age) ? "" : objModel.age);

                        if (objModel.date_of_checkup != null)
                        {
                            cmd.Parameters.AddWithValue("date_of_checkup", objModel.date_of_checkup);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("date_of_checkup", new DateTime(1900, 01, 01));
                        }
                        cmd.Parameters.AddWithValue("chc_centerid", objModel.chc_centerid);
                        cmd.Parameters.AddWithValue("name_of_spouse", string.IsNullOrEmpty(objModel.name_of_spouse) ? "" : objModel.name_of_spouse);

                        if (objModel.dob_spouse != null)
                        {
                            cmd.Parameters.AddWithValue("dob_spouse", objModel.dob_spouse);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("dob_spouse", new DateTime(1900, 01, 01));
                        }
                        cmd.Parameters.AddWithValue("spouse_age", string.IsNullOrEmpty(objModel.spouse_age) ? "" : objModel.spouse_age);
                        cmd.Parameters.AddWithValue("spouse_dt_of_checkup", objModel.spouse_dt_of_checkup);
                        if (objModel.spouse_dt_of_checkup != null)
                        {
                            cmd.Parameters.AddWithValue("spouse_dt_of_checkup", objModel.spouse_dt_of_checkup);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("spouse_dt_of_checkup", new DateTime(1900, 01, 01));
                        }
                        cmd.Parameters.AddWithValue("chc_centid", objModel.chc_centid);
                        cmd.Parameters.AddWithValue("status", objModel.status);
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
        public CHCRequest GetEmplAge(CHCRequest objModel)
        {

            CHCRequest EmpDetails = new CHCRequest();


            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {
                    con.Open();
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("pid", objModel.id);
                    EmpDetails = con.Query<CHCRequest>("select * from mdcl_sp_get_empl_age_for_chc(:pid)", parameters).FirstOrDefault();
                    if (EmpDetails.empl_age < 40)
                    {
                        EmpDetails.name_of_spouse = null;
                    }
                    int tot = con.Query<int>("select * from mdcl_sp_check_empl_is_mumbai_base(:pid)", parameters).FirstOrDefault();
                    EmpDetails.tot_row = tot;
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                EmpDetails = new CHCRequest();
            }
            return EmpDetails;

        }
    }
}