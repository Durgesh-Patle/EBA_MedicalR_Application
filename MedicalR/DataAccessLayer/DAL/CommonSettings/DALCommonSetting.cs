using MedicalR.CustomHelper;
using MedicalR.DataAccessLayer.IDAL.CommonSettings;
using MedicalR.Models;
using MedicalR.Models.CommonSettings;
using MedicalR.Models.MedicalR;
using Npgsql;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using static MedicalR.CustomHelper.EmailHelper;

namespace MedicalR.DataAccessLayer.DAL.CommonSettings
{
    public class DALCommonSetting : IDALCommonSetting
    {



        public List<HospitalListModel> GetHospitalList()
        {
            NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString);
            List<HospitalListModel> HospitalDetails = new List<HospitalListModel>();
            DataTable dt = new DataTable();
            bool status = true;
            try
            {
                // NpgsqlConnection con = new NpgsqlConnection();

                using (con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {

                    con.Open();
                    using (NpgsqlCommand cmd = new NpgsqlCommand("select * from  mdcl_sp_get_hospitallist(:status)", con))
                    {
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("status", status);
                        using (NpgsqlDataAdapter SqDA = new NpgsqlDataAdapter(cmd))
                        {
                            SqDA.Fill(dt);
                        }
                    }

                    if (dt.Rows.Count == 0)
                    {
                        HospitalDetails = new List<HospitalListModel>();
                    }
                    else
                    {
                        foreach (DataRow drow in dt.Rows)
                        {
                            HospitalListModel arnobj = new HospitalListModel();
                            arnobj.id = string.IsNullOrWhiteSpace(drow["id"].ToString()) ? 0 : Convert.ToInt32(drow["id"].ToString());
                            arnobj.Name = string.IsNullOrWhiteSpace(drow["name"].ToString()) ? "" : drow["name"].ToString();
                            arnobj.City = string.IsNullOrWhiteSpace(drow["city"].ToString()) ? "" : drow["city"].ToString();
                            arnobj.Pincode = string.IsNullOrWhiteSpace(drow["pincode"].ToString()) ? 0 : Convert.ToInt32(drow["pincode"].ToString());
                            arnobj.hosp_bank_name = string.IsNullOrWhiteSpace(drow["hosp_bank_name"].ToString()) ? "" : drow["hosp_bank_name"].ToString();
                            arnobj.hosp_bank_accno = string.IsNullOrWhiteSpace(drow["hosp_bank_accno"].ToString()) ? "" : drow["hosp_bank_accno"].ToString();
                            arnobj.bank_ifsc_code = string.IsNullOrWhiteSpace(drow["bank_ifsc_code"].ToString()) ? "" : drow["bank_ifsc_code"].ToString();
                            HospitalDetails.Add(arnobj);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                HospitalDetails = new List<HospitalListModel>();
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return HospitalDetails;
        }
        public HospitalListModel GetsingleHospitalDetails(HospitalListModel objModel)
        {
            NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString);
            HospitalListModel HospitalDetails = new HospitalListModel();
            DataTable dt = new DataTable();
            try
            {
                // NpgsqlConnection con = new NpgsqlConnection();

                using (con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {

                    con.Open();
                    using (NpgsqlCommand cmd = new NpgsqlCommand("select * from  mdcl_sp_get_singlehospitalDetails(:pid)", con))
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
                        HospitalDetails = null;
                    }
                    else
                    {
                        foreach (DataRow drow in dt.Rows)
                        {
                            //  HospitalListModel arnobj = new HospitalListModel();
                            HospitalDetails.id = string.IsNullOrWhiteSpace(drow["id"].ToString()) ? 0 : Convert.ToInt32(drow["id"].ToString());
                            HospitalDetails.Name = string.IsNullOrWhiteSpace(drow["name"].ToString()) ? "" : drow["name"].ToString();
                            HospitalDetails.City = string.IsNullOrWhiteSpace(drow["city"].ToString()) ? "" : drow["city"].ToString();
                            HospitalDetails.Pincode = string.IsNullOrWhiteSpace(drow["pincode"].ToString()) ? 0 : Convert.ToInt32(drow["pincode"].ToString());
                            HospitalDetails.Address = string.IsNullOrWhiteSpace(drow["address"].ToString()) ? "" : drow["address"].ToString();
                            HospitalDetails.hosp_bank_name = string.IsNullOrWhiteSpace(drow["hosp_bank_name"].ToString()) ? "" : drow["hosp_bank_name"].ToString();
                            HospitalDetails.hosp_bank_accno = string.IsNullOrWhiteSpace(drow["hosp_bank_accno"].ToString()) ? "" : drow["hosp_bank_accno"].ToString();
                            HospitalDetails.bank_ifsc_code = string.IsNullOrWhiteSpace(drow["bank_ifsc_code"].ToString()) ? "" : drow["bank_ifsc_code"].ToString();
                            HospitalDetails.is_cashless = string.IsNullOrWhiteSpace(drow["is_cashless"].ToString()) ? false : Convert.ToBoolean(drow["is_cashless"].ToString());
                            HospitalDetails.is_credit_facility = string.IsNullOrWhiteSpace(drow["is_credit_facility"].ToString()) ? false : Convert.ToBoolean(drow["is_credit_facility"].ToString());
                            HospitalDetails.email1 = string.IsNullOrWhiteSpace(drow["email1"].ToString()) ? "" : drow["email1"].ToString();
                            HospitalDetails.email2 = string.IsNullOrWhiteSpace(drow["email2"].ToString()) ? "" : drow["email2"].ToString();
                            HospitalDetails.mens_package = string.IsNullOrWhiteSpace(drow["mens_package"].ToString()) ? "" : drow["mens_package"].ToString();
                            HospitalDetails.womens_package = string.IsNullOrWhiteSpace(drow["womens_package"].ToString()) ? "" : drow["womens_package"].ToString();
                            // HospitalDetails.is_credit_facility = string.IsNullOrWhiteSpace(drow["is_credit_facility"].ToString()) ? "" : Convert.ToBoolean["is_credit_facility"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                HospitalDetails = null;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return HospitalDetails;

        }
        public ResponseModel AddHospital(HospitalListModel objModel)
        {
            NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString);
            ResponseModel Response = new ResponseModel();
            DataTable dt = new DataTable();
            int id;
            try
            {
                // NpgsqlConnection con = new NpgsqlConnection();
                id = objModel.id;
                using (con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {

                    con.Open();

                    using (NpgsqlCommand cmd = new NpgsqlCommand("select * from  mdcl_sp_updatehospital(:pid,:pname,:pcity,:paddress,:ppincode,:phosp_bank_name,:phosp_bank_accno,:pbank_ifsc_code,:pis_credit_facility,:pis_cashless,:pemail1,:pemail2,:p_mens_package,:p_womens_package)", con))
                    {
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("pid", id);
                        cmd.Parameters.AddWithValue("pname", objModel.Name);
                        cmd.Parameters.AddWithValue("pcity", string.IsNullOrWhiteSpace(objModel.City) ? "" : objModel.City);
                        cmd.Parameters.AddWithValue("paddress", string.IsNullOrWhiteSpace(objModel.Address) ? "" : objModel.Address);
                        cmd.Parameters.AddWithValue("ppincode", objModel.Pincode);
                        cmd.Parameters.AddWithValue("phosp_bank_name", string.IsNullOrWhiteSpace(objModel.hosp_bank_name) ? "" : objModel.hosp_bank_name);
                        cmd.Parameters.AddWithValue("phosp_bank_accno", string.IsNullOrWhiteSpace(objModel.hosp_bank_accno) ? "" : objModel.hosp_bank_accno);
                        cmd.Parameters.AddWithValue("pbank_ifsc_code", string.IsNullOrWhiteSpace(objModel.bank_ifsc_code) ? "" : objModel.bank_ifsc_code);
                        cmd.Parameters.AddWithValue("pis_credit_facility", objModel.is_credit_facility);
                        cmd.Parameters.AddWithValue("pis_cashless", objModel.is_cashless);
                        cmd.Parameters.AddWithValue("pemail1", string.IsNullOrWhiteSpace(objModel.email1) ? "" : objModel.email1);
                        cmd.Parameters.AddWithValue("pemail2", string.IsNullOrWhiteSpace(objModel.email2) ? "" : objModel.email2);
                        cmd.Parameters.AddWithValue("p_mens_package", string.IsNullOrWhiteSpace(objModel.mens_package) ? "" : objModel.mens_package);
                        cmd.Parameters.AddWithValue("p_womens_package", string.IsNullOrWhiteSpace(objModel.womens_package) ? "" : objModel.womens_package);

                        int Res = (cmd.ExecuteNonQuery());

                        if (Res == 0)
                        {
                            Response.Status = false;
                            Response.Message = MessageHelper.ErroeMsg;
                        }
                        else
                        {
                            Response.Status = true;
                            Response.Message = MessageHelper.HospitalStatus;
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
        public ResponseModel UpdateHospital(HospitalListModel objModel)
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
                    using (NpgsqlCommand cmd = new NpgsqlCommand("select * from  mdcl_sp_updatehospital(:pid,:pname,:pcity,:paddress,:ppincode,:phosp_bank_name,:phosp_bank_accno,:pbank_ifsc_code,:pis_credit_facility,:pis_cashless,:pemail1,:pemail2,:p_mens_package,:p_womens_package)", con))
                    {
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("pid", objModel.id);
                        cmd.Parameters.AddWithValue("pname", objModel.Name);
                        cmd.Parameters.AddWithValue("pcity", string.IsNullOrWhiteSpace(objModel.City) ? "" : objModel.City);
                        cmd.Parameters.AddWithValue("paddress", string.IsNullOrWhiteSpace(objModel.Address) ? "" : objModel.Address);
                        cmd.Parameters.AddWithValue("ppincode", objModel.Pincode);
                        cmd.Parameters.AddWithValue("phosp_bank_name", string.IsNullOrWhiteSpace(objModel.hosp_bank_name) ? "" : objModel.hosp_bank_name);
                        cmd.Parameters.AddWithValue("phosp_bank_accno", string.IsNullOrWhiteSpace(objModel.hosp_bank_accno) ? "" : objModel.hosp_bank_accno);
                        cmd.Parameters.AddWithValue("pbank_ifsc_code", string.IsNullOrWhiteSpace(objModel.bank_ifsc_code) ? "" : objModel.bank_ifsc_code);
                        cmd.Parameters.AddWithValue("pis_credit_facility", objModel.is_credit_facility);
                        cmd.Parameters.AddWithValue("pis_cashless", objModel.is_cashless);
                        cmd.Parameters.AddWithValue("pemail1", string.IsNullOrWhiteSpace(objModel.email1) ? "" : objModel.email1);
                        cmd.Parameters.AddWithValue("pemail2", string.IsNullOrWhiteSpace(objModel.email2) ? "" : objModel.email2);
                        cmd.Parameters.AddWithValue("p_mens_package", string.IsNullOrWhiteSpace(objModel.mens_package) ? "" : objModel.mens_package);
                        cmd.Parameters.AddWithValue("p_womens_package", string.IsNullOrWhiteSpace(objModel.womens_package) ? "" : objModel.womens_package);
                        //cmd.Parameters.AddWithValue("updateres", SqlDbType.Bit).Direction = ParameterDirection.Output;                      
                        int Res = (cmd.ExecuteNonQuery());
                        // var Res = Convert.ToInt32(cmd.Parameters["updateres"].Value.ToString());
                        if (Res == 0)
                        {
                            Response.Status = false;
                            Response.Message = MessageHelper.ErroeMsg;
                        }
                        else
                        {
                            Response.Status = true;
                            Response.Message = MessageHelper.HospitalUpdated;
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
        public ResponseModel HospitalStatus(HospitalListModel objModel)
        {
            NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString);
            ResponseModel Response = new ResponseModel();
            DataTable dt = new DataTable();
            int id;
            try
            {
                // NpgsqlConnection con = new NpgsqlConnection();
                id = objModel.id;
                using (con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {

                    con.Open();

                    using (NpgsqlCommand cmd = new NpgsqlCommand("select * from  mdcl_sp_updatehospitalstatus(:pid)", con))
                    {
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("pid", id);
                        int Res = (cmd.ExecuteNonQuery());

                        if (Res == 0)
                        {
                            Response.Status = false;
                            Response.Message = MessageHelper.ErroeMsg;
                        }
                        else
                        {
                            Response.Status = true;
                            Response.Message = MessageHelper.HospitalStatus;
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


        public List<DropdownModel> GetTreatmentType()
        {
            NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString);
            List<DropdownModel> Treatlist = new List<DropdownModel>();
            DataTable dt = new DataTable();
            bool status = true;
            try
            {
                using (con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {

                    con.Open();
                    using (NpgsqlCommand cmd = new NpgsqlCommand("select * from  mdcl_sp_get_treatmenttype(:pstatus)", con))
                    {
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("pstatus", status);
                        using (NpgsqlDataAdapter SqDA = new NpgsqlDataAdapter(cmd))
                        {
                            SqDA.Fill(dt);
                        }
                    }

                    if (dt.Rows.Count == 0)
                    {
                        Treatlist = new List<DropdownModel>();
                    }
                    else
                    {
                        foreach (DataRow drow in dt.Rows)
                        {
                            DropdownModel expobj = new DropdownModel();
                            expobj.Id = string.IsNullOrWhiteSpace(drow["id"].ToString()) ? 0 : Convert.ToInt32(drow["id"].ToString());
                            expobj.Name = string.IsNullOrWhiteSpace(drow["name"].ToString()) ? "" : drow["name"].ToString();
                            Treatlist.Add(expobj);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                Treatlist = new List<DropdownModel>();
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return Treatlist;
        }

        public List<DropdownModel> GetHospitalCity()
        {
            NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString);
            List<DropdownModel> Hoslist = new List<DropdownModel>();
            DataTable dt = new DataTable();
            bool status = true;
            try
            {
                using (con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {
                    con.Open();
                    using (NpgsqlCommand cmd = new NpgsqlCommand("select * from  mdcl_sp_get_hospitaldropdown(:pstatus)", con))
                    {
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("pstatus", status);
                        using (NpgsqlDataAdapter SqDA = new NpgsqlDataAdapter(cmd))
                        {
                            SqDA.Fill(dt);
                        }
                    }

                    if (dt.Rows.Count == 0)
                    {
                        Hoslist = new List<DropdownModel>();
                    }
                    else
                    {
                        foreach (DataRow drow in dt.Rows)
                        {
                            DropdownModel expobj = new DropdownModel();
                            expobj.Id = string.IsNullOrWhiteSpace(drow["id"].ToString()) ? 0 : Convert.ToInt32(drow["id"].ToString());
                            expobj.Name = string.IsNullOrWhiteSpace(drow["name"].ToString()) ? "" : drow["name"].ToString();
                            Hoslist.Add(expobj);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                Hoslist = new List<DropdownModel>();
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return Hoslist;
        }

        public List<ExpenceTypeModel> GetExpenceList()
        {
            NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString);
            List<ExpenceTypeModel> ExpenceDetails = new List<ExpenceTypeModel>();
            DataTable dt = new DataTable();
            bool status = true;
            try
            {


                using (con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {

                    con.Open();
                    using (NpgsqlCommand cmd = new NpgsqlCommand("select * from  mdcl_sp_get_expencelist(:pstatus)", con))
                    {
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("pstatus", status);
                        using (NpgsqlDataAdapter SqDA = new NpgsqlDataAdapter(cmd))
                        {
                            SqDA.Fill(dt);
                        }
                    }

                    if (dt.Rows.Count == 0)
                    {
                        ExpenceDetails = new List<ExpenceTypeModel>();
                    }
                    else
                    {
                        foreach (DataRow drow in dt.Rows)
                        {
                            ExpenceTypeModel expobj = new ExpenceTypeModel();
                            expobj.Id = string.IsNullOrWhiteSpace(drow["id"].ToString()) ? 0 : Convert.ToInt32(drow["id"].ToString());
                            expobj.Treat_Type = string.IsNullOrWhiteSpace(drow["treat_type"].ToString()) ? "" : drow["treat_type"].ToString();
                            expobj.Expence_Type = string.IsNullOrWhiteSpace(drow["expence_type"].ToString()) ? "" : drow["expence_type"].ToString();
                            expobj.Head_Expence = string.IsNullOrWhiteSpace(drow["head_expence"].ToString()) ? "" : drow["head_expence"].ToString();
                            ExpenceDetails.Add(expobj);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                ExpenceDetails = new List<ExpenceTypeModel>();
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return ExpenceDetails;
        }
        public ExpenceTypeModel GetsingleExpenseDetails(ExpenceTypeModel objModel)
        {
            NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString);
            ExpenceTypeModel ExpenseDetails = new ExpenceTypeModel();
            DataTable dt = new DataTable();
            try
            {
                // NpgsqlConnection con = new NpgsqlConnection();

                using (con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {

                    con.Open();
                    using (NpgsqlCommand cmd = new NpgsqlCommand("select * from  mdcl_sp_get_singleexpensedetails(:pid)", con))
                    {
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("pid", objModel.Id);
                        using (NpgsqlDataAdapter SqDA = new NpgsqlDataAdapter(cmd))
                        {
                            SqDA.Fill(dt);
                        }
                    }

                    if (dt.Rows.Count == 0)
                    {
                        ExpenseDetails = null;
                    }
                    else
                    {
                        foreach (DataRow drow in dt.Rows)
                        {
                            //  HospitalListModel arnobj = new HospitalListModel();
                            ExpenseDetails.Id = string.IsNullOrWhiteSpace(drow["id"].ToString()) ? 0 : Convert.ToInt32(drow["id"].ToString());
                            ExpenseDetails.Treat_Type = string.IsNullOrWhiteSpace(drow["treat_type"].ToString()) ? "" : drow["treat_type"].ToString();
                            ExpenseDetails.Expence_Type = string.IsNullOrWhiteSpace(drow["expence_type"].ToString()) ? "" : drow["expence_type"].ToString();
                            ExpenseDetails.Head_Expence = string.IsNullOrWhiteSpace(drow["head_expence"].ToString()) ? "" : drow["head_expence"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                ExpenseDetails = null;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return ExpenseDetails;

        }
        public ResponseModel AddExpense(ExpenceTypeModel objModel)
        {
            NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString);
            ResponseModel Response = new ResponseModel();
            DataTable dt = new DataTable();
            //int id;
            try
            {
                // NpgsqlConnection con = new NpgsqlConnection();
                //id = objModel.Id;
                using (con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {

                    con.Open();
                    using (NpgsqlCommand cmd = new NpgsqlCommand("select * from  mdcl_sp_updateexpense(:pid,:ptreat_type,:pexpence_type,:phead_expence)", con))
                    {
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("pid", objModel.Id);
                        cmd.Parameters.AddWithValue("ptreat_type", objModel.Treat_Type);
                        cmd.Parameters.AddWithValue("pexpence_type", objModel.Expence_Type);
                        cmd.Parameters.AddWithValue("phead_expence", objModel.Head_Expence);

                        int Res = (cmd.ExecuteNonQuery());

                        if (Res == 0)
                        {
                            Response.Status = false;
                            Response.Message = MessageHelper.ErroeMsg;
                        }
                        else
                        {
                            Response.Status = true;
                            Response.Message = MessageHelper.ExpenseUpdated;
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
        public ResponseModel UpdateExpenses(ExpenceTypeModel objModel)
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
                    using (NpgsqlCommand cmd = new NpgsqlCommand("select * from  mdcl_sp_updateexpense(:pid,:ptreat_type,:pexpence_type,:phead_expence)", con))
                    {
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("pid", objModel.Id);
                        cmd.Parameters.AddWithValue("ptreat_type", objModel.Treat_Type);
                        cmd.Parameters.AddWithValue("pexpence_type", objModel.Expence_Type);
                        cmd.Parameters.AddWithValue("phead_expence", objModel.Head_Expence);

                        int Res = (cmd.ExecuteNonQuery());

                        if (Res == 0)
                        {
                            Response.Status = false;
                            Response.Message = MessageHelper.ErroeMsg;
                        }
                        else
                        {
                            Response.Status = true;
                            Response.Message = MessageHelper.ExpenseUpdated;
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
        public ResponseModel ExpenseStatus(ExpenceTypeModel objModel)
        {
            NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString);
            ResponseModel Response = new ResponseModel();
            DataTable dt = new DataTable();
            int id;
            try
            {
                // NpgsqlConnection con = new NpgsqlConnection();
                id = objModel.Id;
                using (con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {

                    con.Open();

                    using (NpgsqlCommand cmd = new NpgsqlCommand("select * from  mdcl_sp_updateexpencestatus(:pid)", con))
                    {
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("pid", id);
                        int Res = (cmd.ExecuteNonQuery());

                        if (Res == 0)
                        {
                            Response.Status = false;
                            Response.Message = MessageHelper.ErroeMsg;
                        }
                        else
                        {
                            Response.Status = true;
                            Response.Message = MessageHelper.ExpenseStatus;
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

        public List<ObjectionModel> GetObjectionList()
        {
            NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString);
            List<ObjectionModel> ObjDetails = new List<ObjectionModel>();
            DataTable dt = new DataTable();
            bool status = true;
            try
            {


                using (con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {

                    con.Open();
                    using (NpgsqlCommand cmd = new NpgsqlCommand("select * from  mdcl_sp_get_objectionlist(:pstatus)", con))
                    {
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("pstatus", status);
                        using (NpgsqlDataAdapter SqDA = new NpgsqlDataAdapter(cmd))
                        {
                            SqDA.Fill(dt);
                        }
                    }

                    if (dt.Rows.Count == 0)
                    {
                        ObjDetails = new List<ObjectionModel>();
                    }
                    else
                    {
                        foreach (DataRow drow in dt.Rows)
                        {
                            ObjectionModel obobj = new ObjectionModel();
                            obobj.Id = string.IsNullOrWhiteSpace(drow["id"].ToString()) ? 0 : Convert.ToInt32(drow["id"].ToString());
                            obobj.Obj_Code = string.IsNullOrWhiteSpace(drow["obj_code"].ToString()) ? "" : drow["obj_code"].ToString();
                            obobj.Obj_Desc = string.IsNullOrWhiteSpace(drow["obj_desc"].ToString()) ? "" : drow["obj_desc"].ToString();

                            ObjDetails.Add(obobj);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                ObjDetails = new List<ObjectionModel>();
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return ObjDetails;
        }
        public ObjectionModel GetsingleObjectionDetails(ObjectionModel objModel)
        {
            NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString);
            ObjectionModel ObjDetails = new ObjectionModel();
            DataTable dt = new DataTable();
            try
            {
                // NpgsqlConnection con = new NpgsqlConnection();

                using (con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {

                    con.Open();
                    using (NpgsqlCommand cmd = new NpgsqlCommand("select * from  mdcl_sp_get_singleobjectiondetails(:pid)", con))
                    {
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("pid", objModel.Id);
                        using (NpgsqlDataAdapter SqDA = new NpgsqlDataAdapter(cmd))
                        {
                            SqDA.Fill(dt);
                        }
                    }

                    if (dt.Rows.Count == 0)
                    {
                        ObjDetails = null;
                    }
                    else
                    {
                        foreach (DataRow drow in dt.Rows)
                        {
                            //  HospitalListModel arnobj = new HospitalListModel();
                            ObjDetails.Id = string.IsNullOrWhiteSpace(drow["id"].ToString()) ? 0 : Convert.ToInt32(drow["id"].ToString());
                            ObjDetails.Obj_Code = string.IsNullOrWhiteSpace(drow["obj_code"].ToString()) ? "" : drow["obj_code"].ToString();
                            ObjDetails.Obj_Desc = string.IsNullOrWhiteSpace(drow["obj_desc"].ToString()) ? "" : drow["obj_desc"].ToString();

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                ObjDetails = null;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return ObjDetails;

        }
        public ResponseModel AddObjection(ObjectionModel objModel)
        {
            NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString);
            ResponseModel Response = new ResponseModel();
            DataTable dt = new DataTable();
            //int id;
            try
            {
                // NpgsqlConnection con = new NpgsqlConnection();
                //id = objModel.Id;
                using (con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {

                    con.Open();
                    using (NpgsqlCommand cmd = new NpgsqlCommand("select * from  mdcl_sp_updateobjection(:pid,:pobj_code,:pobj_desc)", con))
                    {
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("pid", objModel.Id);
                        cmd.Parameters.AddWithValue("pobj_code", objModel.Obj_Code);
                        cmd.Parameters.AddWithValue("pobj_desc", objModel.Obj_Desc);


                        int Res = (cmd.ExecuteNonQuery());

                        if (Res == 0)
                        {
                            Response.Status = false;
                            Response.Message = MessageHelper.ErroeMsg;
                        }
                        else
                        {
                            Response.Status = true;
                            Response.Message = MessageHelper.ObjectionUpdated;
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
        public ResponseModel UpdateObjection(ObjectionModel objModel)
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
                    using (NpgsqlCommand cmd = new NpgsqlCommand("select * from  mdcl_sp_updateobjection(:pid,:pobj_code,:pobj_desc)", con))
                    {
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("pid", objModel.Id);
                        cmd.Parameters.AddWithValue("pobj_code", objModel.Obj_Code);
                        cmd.Parameters.AddWithValue("pobj_desc", objModel.Obj_Desc);


                        int Res = (cmd.ExecuteNonQuery());

                        if (Res == 0)
                        {
                            Response.Status = false;
                            Response.Message = MessageHelper.ErroeMsg;
                        }
                        else
                        {
                            Response.Status = true;
                            Response.Message = MessageHelper.ObjectionUpdated;
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
        public ResponseModel ObjectionStatus(ObjectionModel objModel)
        {
            NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString);
            ResponseModel Response = new ResponseModel();
            DataTable dt = new DataTable();
            int id;
            try
            {
                id = objModel.Id;
                using (con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {
                    con.Open();
                    using (NpgsqlCommand cmd = new NpgsqlCommand("select * from  mdcl_sp_updateobjectionstatus(:pid)", con))
                    {
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("pid", id);
                        int Res = (cmd.ExecuteNonQuery());

                        if (Res == 0)
                        {
                            Response.Status = false;
                            Response.Message = MessageHelper.ErroeMsg;
                        }
                        else
                        {
                            Response.Status = true;
                            Response.Message = MessageHelper.ObjectionStatus;
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
        public List<DoctorList> GetDoctorlist()
        {
            NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString);
            List<DoctorList> DoctorDetails = new List<DoctorList>();
            DataTable dt = new DataTable();
            bool status = true;
            try
            {
                // NpgsqlConnection con = new NpgsqlConnection();

                using (con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {

                    con.Open();
                    using (NpgsqlCommand cmd = new NpgsqlCommand("select * from  mdcl_sp_get_doctorlist(:status)", con))
                    {
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("status", status);
                        using (NpgsqlDataAdapter SqDA = new NpgsqlDataAdapter(cmd))
                        {
                            SqDA.Fill(dt);
                        }
                    }

                    if (dt.Rows.Count == 0)
                    {
                        DoctorDetails = new List<DoctorList>();
                    }
                    else
                    {
                        foreach (DataRow drow in dt.Rows)
                        {
                            DoctorList arnobj = new DoctorList();
                            arnobj.id = string.IsNullOrWhiteSpace(drow["id"].ToString()) ? 0 : Convert.ToInt32(drow["id"].ToString());
                            arnobj.dr_name = string.IsNullOrWhiteSpace(drow["dr_name"].ToString()) ? "" : drow["dr_name"].ToString();
                            arnobj.mobile_no = string.IsNullOrWhiteSpace(drow["mobile_no"].ToString()) ? "" : drow["mobile_no"].ToString();
                            arnobj.designation = string.IsNullOrWhiteSpace(drow["designation"].ToString()) ? "" : drow["designation"].ToString();
                            DoctorDetails.Add(arnobj);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                DoctorDetails = new List<DoctorList>();
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return DoctorDetails;
        }
        public ResponseModel AddDoctor(DoctorList objModel)
        {
            NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString);
            ResponseModel Response = new ResponseModel();
            DataTable dt = new DataTable();
            int id;
            try
            {
                // NpgsqlConnection con = new NpgsqlConnection();
                id = objModel.id;

                using (con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {

                    con.Open();

                    using (NpgsqlCommand cmd = new NpgsqlCommand("select * from  mdcl_sp_updateDoctor(:pid,:pdr_name,:pmobile_no,:pdesignation)", con))
                    {
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("pid", id);
                        cmd.Parameters.AddWithValue("pdr_name", objModel.dr_name);
                        cmd.Parameters.AddWithValue("pmobile_no", objModel.mobile_no);
                        cmd.Parameters.AddWithValue("pdesignation", objModel.designation);
                        int Res = (cmd.ExecuteNonQuery());

                        if (Res == 0)
                        {
                            Response.Status = false;
                            Response.Message = MessageHelper.ErroeMsg;
                        }
                        else
                        {
                            Response.Status = true;
                            Response.Message = MessageHelper.Doctoradd;
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
        public DoctorList GetSingleDoctordetail(DoctorList objModel)
        {
            NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString);
            DoctorList Drdetail = new DoctorList();
            DataTable dt = new DataTable();
            try
            {
                // NpgsqlConnection con = new NpgsqlConnection();

                using (con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {

                    con.Open();
                    using (NpgsqlCommand cmd = new NpgsqlCommand("select * from  mdcl_sp_get_singleDrdetails(:pid)", con))
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
                        Drdetail = null;
                    }
                    else
                    {
                        foreach (DataRow drow in dt.Rows)
                        {
                            //  HospitalListModel arnobj = new HospitalListModel();
                            Drdetail.id = string.IsNullOrWhiteSpace(drow["id"].ToString()) ? 0 : Convert.ToInt32(drow["id"].ToString());
                            Drdetail.dr_name = string.IsNullOrWhiteSpace(drow["dr_name"].ToString()) ? "" : drow["dr_name"].ToString();
                            Drdetail.mobile_no = string.IsNullOrWhiteSpace(drow["mobile_no"].ToString()) ? "" : drow["mobile_no"].ToString();
                            Drdetail.designation = string.IsNullOrWhiteSpace(drow["designation"].ToString()) ? "" : drow["designation"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                Drdetail = null;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return Drdetail;

        }
        public ResponseModel UpdateDr(DoctorList objModel)
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
                    using (NpgsqlCommand cmd = new NpgsqlCommand("select * from  mdcl_sp_updateDoctor(:pid,:pdr_name,:pmobile_no,:pdesignation)", con))
                    {
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("pid", objModel.id);
                        cmd.Parameters.AddWithValue("pdr_name", objModel.dr_name);
                        cmd.Parameters.AddWithValue("pmobile_no", objModel.mobile_no);
                        cmd.Parameters.AddWithValue("pdesignation", objModel.designation);

                        int Res = (cmd.ExecuteNonQuery());

                        if (Res == 0)
                        {
                            Response.Status = false;
                            Response.Message = MessageHelper.ErroeMsg;
                        }
                        else
                        {
                            Response.Status = true;
                            Response.Message = MessageHelper.Doctorupdate;
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
        public ResponseModel DrStatus(DoctorList objModel)
        {
            NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString);
            ResponseModel Response = new ResponseModel();
            DataTable dt = new DataTable();
            int pid;
            try
            {
                // NpgsqlConnection con = new NpgsqlConnection();
                pid = objModel.id;
                using (con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {

                    con.Open();

                    using (NpgsqlCommand cmd = new NpgsqlCommand("select * from  mdcl_sp_updateDoctorstatus(:pid)", con))
                    {
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("pid", pid);
                        int Res = (cmd.ExecuteNonQuery());

                        if (Res == 0)
                        {
                            Response.Status = false;
                            Response.Message = MessageHelper.ErroeMsg;
                        }
                        else
                        {
                            Response.Status = true;
                            Response.Message = MessageHelper.DoctorStatus;
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

        #region get company detail
        public CompanyProfileModel GetCompanyDetails()
        {
            CompanyProfileModel CompanyDetails = new CompanyProfileModel();
            SqlConnection con = new SqlConnection(CommonHelper.GetConnectionString);
            try
            {
                var CompanyID = UserManager.User.CompanyID;
                SqlCommand cmd = new SqlCommand("sproc_GetCompanyDetail", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CompanyID", CompanyID);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                DataSet ds = new DataSet();
                da.Fill(ds);
                if (ds.Tables[0].Rows.Count == 0)
                {
                    CompanyDetails = null;
                }
                else
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        CompanyDetails.CompanyID = Convert.ToInt32(ds.Tables[0].Rows[i]["CompanyID"].ToString());
                        CompanyDetails.About = ds.Tables[0].Rows[i]["About"].ToString();
                        CompanyDetails.CityID = ds.Tables[0].Rows[i]["CityID"] != DBNull.Value ? Convert.ToInt32(ds.Tables[0].Rows[i]["CityID"].ToString()) : 0;
                        CompanyDetails.CompanyName = ds.Tables[0].Rows[i]["CompanyName"].ToString();
                        CompanyDetails.Contactno = ds.Tables[0].Rows[i]["Contactno"].ToString();
                        CompanyDetails.CountryID = ds.Tables[0].Rows[i]["CountryID"] != DBNull.Value ? Convert.ToInt32(ds.Tables[0].Rows[i]["CountryID"].ToString()) : 0;
                        CompanyDetails.EmailID = ds.Tables[0].Rows[i]["EmailID"].ToString();
                        CompanyDetails.FaxNumber = ds.Tables[0].Rows[i]["FaxNumber"].ToString();
                        CompanyDetails.IndustryID = Convert.ToInt32(ds.Tables[0].Rows[i]["IndustryID"].ToString());
                        CompanyDetails.Poc = ds.Tables[0].Rows[i]["Poc"].ToString();
                        CompanyDetails.PostalCode = ds.Tables[0].Rows[i]["PostalCode"].ToString();
                        CompanyDetails.StateID = ds.Tables[0].Rows[i]["StateID"] != DBNull.Value ? Convert.ToInt32(ds.Tables[0].Rows[i]["StateID"].ToString()) : 0;
                        CompanyDetails.website = ds.Tables[0].Rows[i]["website"].ToString();
                        CompanyDetails.LogoURL = ds.Tables[0].Rows[i]["LogoURL"].ToString();
                        CompanyDetails.LogoURL_GoogleID = ds.Tables[0].Rows[i]["LogoURL_GoogleID"] != DBNull.Value ? Convert.ToInt32(ds.Tables[0].Rows[i]["LogoURL_GoogleID"].ToString()) : 0;
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                CompanyDetails = null;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return CompanyDetails;
        }
        #endregion


        #region update company details
        public ResponseModel UpdateCompanyDetails(CompanyProfileModel objModel)
        {
            ResponseModel Response = new ResponseModel();
            SqlConnection con = new SqlConnection(CommonHelper.GetConnectionString);
            try
            {
                var CompanyID = UserManager.User.CompanyID;
                SqlCommand cmd = new SqlCommand("sproc_UpdateCompanyDetail", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CompanyName", objModel.CompanyName);
                cmd.Parameters.AddWithValue("@EmailID", objModel.EmailID);
                cmd.Parameters.AddWithValue("@Contactno", objModel.Contactno);
                cmd.Parameters.AddWithValue("@website", objModel.website);
                cmd.Parameters.AddWithValue("@StateID", objModel.StateID);
                cmd.Parameters.AddWithValue("@CityID", objModel.CityID);
                cmd.Parameters.AddWithValue("@CountryID", objModel.CountryID);
                cmd.Parameters.AddWithValue("@PostalCode", objModel.PostalCode);
                cmd.Parameters.AddWithValue("@LogoURL_GoogleID", objModel.LogoURL_GoogleID);
                cmd.Parameters.AddWithValue("@FaxNumber", objModel.FaxNumber);
                cmd.Parameters.AddWithValue("@Poc", objModel.Poc);
                cmd.Parameters.AddWithValue("@IndustryID", objModel.IndustryID);
                cmd.Parameters.AddWithValue("@About", objModel.About);
                cmd.Parameters.AddWithValue("@CompanyID", CompanyID);
                cmd.Parameters.AddWithValue("@LogoURL", objModel.LogoURL);
                con.Open();
                cmd.ExecuteNonQuery();

                Response.Status = true;
                Response.Message = MessageHelper.CompanyDetailsUpdate;

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

        #endregion



        #region get country list
        public List<CountryModel> GetCountryList()
        {
            List<CountryModel> CountryList = new List<CountryModel>();
            SqlConnection con = new SqlConnection(CommonHelper.GetConnectionString);
            try
            {
                SqlCommand cmd = new SqlCommand("sproc_GetCountryList", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                DataSet ds = new DataSet();
                da.Fill(ds);

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    CountryModel Country = new CountryModel();
                    Country.CountryID = Convert.ToInt32(ds.Tables[0].Rows[i]["CountryID"].ToString());
                    Country.CountryName = ds.Tables[0].Rows[i]["CountryName"].ToString();
                    CountryList.Add(Country);
                }

            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                CountryList = new List<CountryModel>();
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return CountryList;
        }
        #endregion

        #region get state list
        public List<StateModel> GetStateList(int CountryID = 0)
        {
            List<StateModel> StateList = new List<StateModel>();
            SqlConnection con = new SqlConnection(CommonHelper.GetConnectionString);
            try
            {
                var CompanyID = UserManager.User.CompanyID;
                SqlCommand cmd = new SqlCommand("sproc_GetStateList", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CountryId", CountryID);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                DataSet ds = new DataSet();
                da.Fill(ds);

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    StateModel State = new StateModel();
                    State.StateID = Convert.ToInt32(ds.Tables[0].Rows[i]["StateId"].ToString());
                    State.StateName = ds.Tables[0].Rows[i]["StateName"].ToString();
                    StateList.Add(State);
                }

            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                StateList = new List<StateModel>();
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return StateList;
        }
        #endregion

        #region get city list
        public List<CityModel> GetCityList(int StateID = 0)
        {
            List<CityModel> CityList = new List<CityModel>();
            SqlConnection con = new SqlConnection(CommonHelper.GetConnectionString);
            try
            {
                var CompanyID = UserManager.User.CompanyID;
                SqlCommand cmd = new SqlCommand("sproc_GetCityList", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StateID", StateID);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                DataSet ds = new DataSet();
                da.Fill(ds);

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    CityModel City = new CityModel();
                    City.CityID = Convert.ToInt32(ds.Tables[0].Rows[i]["CityId"].ToString());
                    City.CityName = ds.Tables[0].Rows[i]["CityName"].ToString();
                    CityList.Add(City);
                }

            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                CityList = new List<CityModel>();
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return CityList;
        }
        #endregion


        #region get industry list
        public List<IndustryTypeModel> GetIndustryList()
        {
            List<IndustryTypeModel> IndustryList = new List<IndustryTypeModel>();
            SqlConnection con = new SqlConnection(CommonHelper.GetConnectionString);
            try
            {
                SqlCommand cmd = new SqlCommand("sproc_GetIndustryList", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                DataSet ds = new DataSet();
                da.Fill(ds);

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    IndustryTypeModel Industry = new IndustryTypeModel();
                    Industry.IndustryID = Convert.ToInt32(ds.Tables[0].Rows[i]["IndustryID"].ToString());
                    Industry.IndustryName = ds.Tables[0].Rows[i]["Name"].ToString();
                    IndustryList.Add(Industry);
                }

            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                IndustryList = new List<IndustryTypeModel>();
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return IndustryList;
        }
        #endregion


        #region update company user password
        public ResponseModel UpdateUserPassword(PasswordModel objModel)
        {
            ResponseModel Response = new ResponseModel();
            SqlConnection con = new SqlConnection(CommonHelper.GetConnectionString);
            try
            {
                var CompanyID = UserManager.User.CompanyID;
                var CompanyUserID = UserManager.User.UserID;
                var NewPassword = AesGenerator.Encrypt(objModel.NewPassword);
                var CurrentPassword = AesGenerator.Encrypt(objModel.CurrentPassword);
                SqlCommand cmd = new SqlCommand("sproc_UpdateCompanyUserPassword", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Password", NewPassword);
                cmd.Parameters.AddWithValue("@CurentPassword", CurrentPassword);
                cmd.Parameters.AddWithValue("@CompanyID", CompanyID);
                cmd.Parameters.AddWithValue("@CompanyUserID", CompanyUserID);
                cmd.Parameters.AddWithValue("@UpdateRes", SqlDbType.Int).Direction = ParameterDirection.Output;
                con.Open();
                cmd.ExecuteNonQuery();
                var Res = Convert.ToInt32(cmd.Parameters["@UpdateRes"].Value.ToString());
                if (Res == 0)
                {
                    Response.Status = false;
                    Response.Message = MessageHelper.CurrentPasswordNotValid;
                }
                else
                {
                    Response.Status = true;
                    Response.Message = MessageHelper.PasswordUpdated;
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

        #endregion


        #region contact us

        //public void SendContactEmail(ContactUsModel objModel)
        //{
        //    try
        //    {
        //        var Body = "Email : '" + objModel.EmailID + "' <br/> Mobile Number : '" + objModel.MobileNo + "' <br/> Regarding : '" + objModel.Regarding + "' <br/> Date /Time : '" + objModel.DateTime + "' <br/> Message : '" + objModel.Message + "'";
        //        var AttchmentList = new List<string>();
        //        if (objModel.ListofAttchment != null)
        //        {
        //            foreach (var item in objModel.ListofAttchment)
        //            {
        //                AttchmentList.Add(CommonHelper.GetAppBaseUrl + item);
        //            }
        //        }

        //        EmailModel parameters = new EmailModel();
        //        parameters.Attachment = AttchmentList;
        //        parameters.Body = Body;
        //        parameters.toEmailID = CommonHelper.GetEmailToID;
        //        parameters.ccEmailID = CommonHelper.GetEmailCcID;
        //        parameters.Subject = "Enquiry From FIT";
        //        parameters.mailType = "contact";
        //        EmailHelper.SendEmail(parameters);
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionLogging.LogException(ex);
        //    }
        //}
        #endregion

        #region get source list
        public List<SourceModel> GetSourceList()
        {
            List<SourceModel> SourceList = new List<SourceModel>();
            SqlConnection con = new SqlConnection(CommonHelper.GetConnectionString);
            try
            {
                SqlCommand cmd = new SqlCommand("sproc_GetSourceList", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                DataSet ds = new DataSet();
                da.Fill(ds);

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    SourceModel Source = new SourceModel();
                    Source.SourceID = Convert.ToInt32(ds.Tables[0].Rows[i]["SocialMediaID"].ToString());
                    Source.SourceName = ds.Tables[0].Rows[i]["SocialMediaDetails"].ToString();
                    SourceList.Add(Source);
                }

            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                SourceList = new List<SourceModel>();
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return SourceList;
        }
        #endregion

        #region get social media list
        public List<SocialMediaModel> GetSocialMediaList()
        {
            List<SocialMediaModel> SocialMediaList = new List<SocialMediaModel>();
            SqlConnection con = new SqlConnection(CommonHelper.GetConnectionString);
            try
            {
                SqlCommand cmd = new SqlCommand("sproc_GetSocialMediaList", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                DataSet ds = new DataSet();
                da.Fill(ds);

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    SocialMediaModel SMedia = new SocialMediaModel();
                    SMedia.SocialMediaID = Convert.ToInt32(ds.Tables[0].Rows[i]["SocialMediaID"].ToString());
                    SMedia.SocialMediaName = ds.Tables[0].Rows[i]["SocialMediaDetails"].ToString();
                    SocialMediaList.Add(SMedia);
                }

            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                SocialMediaList = new List<SocialMediaModel>();
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return SocialMediaList;
        }
        #endregion

        #region get skill list
        public List<SkillModel> GetSkillList()
        {
            List<SkillModel> SkillList = new List<SkillModel>();
            SqlConnection con = new SqlConnection(CommonHelper.GetConnectionString);
            try
            {
                SqlCommand cmd = new SqlCommand("sproc_GetSkillList", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                DataSet ds = new DataSet();
                da.Fill(ds);

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    SkillModel skill = new SkillModel();
                    skill.SkillID = Convert.ToInt32(ds.Tables[0].Rows[i]["SkillID"].ToString());
                    skill.SkillName = ds.Tables[0].Rows[i]["SkillName"].ToString();
                    SkillList.Add(skill);
                }

            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                SkillList = new List<SkillModel>();
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return SkillList;
        }
        #endregion

        #region add skill
        public ResponseModel AddSkill(SkillModel objModel)
        {
            ResponseModel Response = new ResponseModel();
            SqlConnection con = new SqlConnection(CommonHelper.GetConnectionString);
            try
            {

                var CurrentUtcDate = CommonHelper.GetDate;
                var CompanyUserID = UserManager.User.UserID;
                var CompanyID = UserManager.User.CompanyID;
                SqlCommand cmd = new SqlCommand("sproc_InserttblSkill", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SkillType", objModel.SkillType);
                cmd.Parameters.AddWithValue("@SkillName", objModel.SkillName);
                cmd.Parameters.AddWithValue("@Description", objModel.SkillDescription);
                cmd.Parameters.AddWithValue("@RelatedKeywords", objModel.SkillKeyword);
                cmd.Parameters.AddWithValue("@CreatedDate", CurrentUtcDate);
                cmd.Parameters.AddWithValue("@SkillID", SqlDbType.Int).Direction = ParameterDirection.Output;
                con.Open();
                cmd.ExecuteNonQuery();
                var SkillIDID = Convert.ToInt32(cmd.Parameters["@SkillID"].Value.ToString());
                if (SkillIDID > 0)
                {
                    Response.Status = true;
                    Response.ID = SkillIDID;
                    Response.Message = MessageHelper.SkillAdded;
                }
                else
                {
                    Response.Status = false;
                    Response.ID = SkillIDID;
                    Response.Message = MessageHelper.SkillUpdated;
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
        #endregion

        #region update  columns
        public ResponseModel UpdateColumns(List<string> columns, string columnType)
        {
            ResponseModel Response = new ResponseModel();
            SqlConnection con = new SqlConnection(CommonHelper.GetConnectionString);
            try
            {
                var CurrentUtcDate = CommonHelper.GetDate;
                var CompanyUserID = UserManager.User.UserID;
                var CompanyID = UserManager.User.CompanyID;
                SqlCommand cmd = new SqlCommand("sproc_UpdateColumns", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CompanyUserID", CompanyUserID);
                cmd.Parameters.AddWithValue("@CompanyID", CompanyID);
                cmd.Parameters.AddWithValue("@ColumnsType", columnType);
                cmd.Parameters.AddWithValue("@Columns", columns != null ? string.Join(",", columns.ToArray()) : "");
                con.Open();
                cmd.ExecuteNonQuery();
                Response.Status = true;
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
        #endregion

        #region get  columns
        public List<string> getColumns(string columnType)
        {
            List<string> SelectedColumns = new List<string>();
            SqlConnection con = new SqlConnection(CommonHelper.GetConnectionString);
            try
            {
                var CompanyUserID = UserManager.User.UserID;
                var CompanyID = UserManager.User.CompanyID;
                SqlCommand cmd = new SqlCommand("sproc_GetColumns", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CompanyUserID", CompanyUserID);
                cmd.Parameters.AddWithValue("@CompanyID", CompanyID);
                cmd.Parameters.AddWithValue("@ColumnsType", columnType);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                DataSet ds = new DataSet();
                da.Fill(ds);
                if (ds.Tables[0].Rows.Count == 0)
                {
                    SelectedColumns = null;
                }
                else
                {
                    var Columns = "";
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Columns = ds.Tables[0].Rows[i]["SelectedColumns"] != DBNull.Value ? ds.Tables[0].Rows[i]["SelectedColumns"].ToString() : "";

                    }
                    if (!string.IsNullOrEmpty(Columns))
                    {
                        SelectedColumns = Columns.Split(',').ToList();
                    }


                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                SelectedColumns = null;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return SelectedColumns;
        }
        #endregion


    }
}