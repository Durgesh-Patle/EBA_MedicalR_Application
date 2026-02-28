using Dapper;
using MedicalR.CustomHelper;
using MedicalR.DataAccessLayer.IDAL.RetiredEmployee;
using MedicalR.Models;
using MedicalR.Models.CommonSettings;
using MedicalR.Models.RetiredEmployee;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Text;
using System.IO;

namespace MedicalR.DataAccessLayer.DAL.RetiredEmployee
{
    public class DALRetiredEmployee : IDALRetiredEmployee
    {
        public List<DropdownModel> Getyears(string empcd)
        {
            NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString);
            List<DropdownModel> yearlist = new List<DropdownModel>();
            DataTable dt = new DataTable();
            var Empcd = empcd;
            try
            {
                using (con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {
                    con.Open();
                    using (NpgsqlCommand cmd = new NpgsqlCommand("select * from  mdcl_sp_get_tofill_yeardropdown_for_edit(:pid)", con))
                    {
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("pid", Empcd);
                        using (NpgsqlDataAdapter SqDA = new NpgsqlDataAdapter(cmd))
                        {
                            SqDA.Fill(dt);
                        }
                    }

                    if (dt.Rows.Count == 0)
                    {
                        yearlist = new List<DropdownModel>();
                    }
                    else
                    {
                        foreach (DataRow drow in dt.Rows)
                        {
                            DropdownModel expobj = new DropdownModel();
                            expobj.Id = string.IsNullOrWhiteSpace(drow["id"].ToString()) ? 0 : Convert.ToInt32(drow["id"].ToString());
                            expobj.Name = string.IsNullOrWhiteSpace(drow["name"].ToString()) ? "" : drow["name"].ToString();
                            yearlist.Add(expobj);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                yearlist = new List<DropdownModel>();
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return yearlist;
        }

        public List<RetiredEmployeeModel> GetRetiredEmp()
        {
            NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString);
            List<RetiredEmployeeModel> Remplist = new List<RetiredEmployeeModel>();
            DataTable dt = new DataTable();
            bool status = true;
            try
            {

                using (con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {

                    con.Open();
                    using (NpgsqlCommand cmd = new NpgsqlCommand("select * from  mdcl_sp_get_retiredemplist(:pstatus)", con))
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
                        Remplist = new List<RetiredEmployeeModel>();
                    }
                    else
                    {
                        foreach (DataRow drow in dt.Rows)
                        {
                            RetiredEmployeeModel robj = new RetiredEmployeeModel();
                            robj.Id = string.IsNullOrWhiteSpace(drow["id"].ToString()) ? 0 : Convert.ToInt32(drow["id"].ToString());
                            robj.EmpId = string.IsNullOrWhiteSpace(drow["empid"].ToString()) ? "" : drow["empid"].ToString();
                            robj.Name = string.IsNullOrWhiteSpace(drow["name"].ToString()) ? "" : drow["name"].ToString();
                            robj.Bankname = string.IsNullOrWhiteSpace(drow["bankname"].ToString()) ? "" : drow["bankname"].ToString();
                            robj.Ifccode = string.IsNullOrWhiteSpace(drow["ifsccode"].ToString()) ? "" : drow["ifsccode"].ToString();
                            robj.Accno = string.IsNullOrWhiteSpace(drow["accno"].ToString()) ? "" : drow["accno"].ToString();
                            robj.Othinfo = string.IsNullOrWhiteSpace(drow["othinfo"].ToString()) ? "" : drow["othinfo"].ToString();
                            if (!string.IsNullOrEmpty(drow["effdate"].ToString()))
                            {
                                robj.Effdate = Convert.ToDateTime(drow["effdate"].ToString());
                            }
                            robj.Mobile = string.IsNullOrWhiteSpace(drow["mobile"].ToString()) ? "" : drow["mobile"].ToString();
                            robj.Email = string.IsNullOrWhiteSpace(drow["email"].ToString()) ? "" : drow["email"].ToString();
                            robj.Status = string.IsNullOrWhiteSpace(drow["status"].ToString()) ? false : Convert.ToBoolean(drow["status"].ToString());
                            //robj.Bankname = string.IsNullOrWhiteSpace(drow["pincode"].ToString()) ? 0 : Convert.ToInt32(drow["pincode"].ToString());
                            Remplist.Add(robj);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                Remplist = new List<RetiredEmployeeModel>();
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return Remplist;
        }

        public ResponseModel EditCheckingStart(List<RetiredempTransModel> model)
        {
            ResponseModel Response = new ResponseModel();
            NpgsqlTransaction tran = null;
            using (NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString))
            {
                con.Open();
                tran = con.BeginTransaction();
                foreach (RetiredempTransModel objmodel in model)
                {
                    if (objmodel.Status)
                    {
                        DynamicParameters para = new DynamicParameters();
                        para.Add("p_id", objmodel.Id);
                        con.Query("select * from mdcl_sp_ret_edit_checking_start(:p_id)", para, tran);
                    }
                }
                tran.Commit();
                Response.Message = MessageHelper.RequestStatus;
            }
            return Response;

        }
        public ResponseModel LotGenerationStart(List<RetiredempTransModel> model)
        {
            ResponseModel Response = new ResponseModel();
            NpgsqlTransaction tran = null;
            using (NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString))
            {
                con.Open();
                tran = con.BeginTransaction();
                string lot_no = con.Query<string>("select * from mdcl_sp_get_ret_last_lotno_details()").FirstOrDefault();
                if (string.IsNullOrWhiteSpace(lot_no))
                {
                    lot_no = "Lot-" + DateTime.Now.Year + "-1";
                }
                else
                {
                    string var_lot = Convert.ToString(Convert.ToInt32(lot_no.Split('-')[2]) + 1);
                    lot_no = "Lot-" + DateTime.Now.Year + "-" + var_lot;
                }
                foreach (RetiredempTransModel objmodel in model)
                {
                    if (objmodel.Status)
                    {
                        DynamicParameters para = new DynamicParameters();
                        para.Add("p_id", objmodel.Id);
                        para.Add("p_lot_no", lot_no);
                        para.Add("p_bank_id", objmodel.bank_id);
                        para.Add("p_accno", objmodel.accno);
                        para.Add("p_ifsccode", objmodel.ifsccode);
                        con.Query("select * from mdcl_sp_ret_lot_no(:p_id,:p_lot_no,:p_bank_id,:p_accno,:p_ifsccode)", para, tran);
                    }
                }
                tran.Commit();
                Response.Message = MessageHelper.RequestStatus;

            }
            return Response;
        }
        public ResponseModel AddRemployee(RetiredEmployeeModel objModel)
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
                    DynamicParameters para2 = new DynamicParameters();
                    para2.Add("pid", objModel.EmpId);

                    string varVal = con.Query<string>("select * from mdcl_sp_check_empl_alreadyexit(:pid)", para2).FirstOrDefault();
                    if (varVal == "0")
                    {
                        using (NpgsqlCommand cmd = new NpgsqlCommand("select * from  mdcl_sp_addorupdateremp(:pid,:pempid,:pname,:pbank_id,:pifsccode,:paccno,:pothinfo,:pmobile,:pemail,:peffdate)", con))
                        {
                            cmd.Connection = con;
                            if (objModel.Othinfo == null || objModel.Othinfo == "")
                            {
                                string pothinfo = "";
                                objModel.Othinfo = pothinfo;
                            }
                            else
                            {
                                string pothinfo = objModel.Othinfo;

                            }
                            cmd.Parameters.AddWithValue("pid", objModel.Id);
                            cmd.Parameters.AddWithValue("pempid", objModel.EmpId);
                            cmd.Parameters.AddWithValue("pname", string.IsNullOrEmpty(objModel.Name) ? "" : objModel.Name);
                            cmd.Parameters.AddWithValue("pbank_id", objModel.bank_id);
                            cmd.Parameters.AddWithValue("pifsccode", string.IsNullOrEmpty(objModel.Ifccode) ? "" : objModel.Ifccode);
                            cmd.Parameters.AddWithValue("paccno", string.IsNullOrEmpty(objModel.Accno) ? "" : objModel.Accno);
                            cmd.Parameters.AddWithValue("pothinfo", string.IsNullOrEmpty(objModel.Othinfo) ? "" : objModel.Othinfo);

                            cmd.Parameters.AddWithValue("pmobile", string.IsNullOrEmpty(objModel.Mobile) ? "" : objModel.Mobile);
                            cmd.Parameters.AddWithValue("pemail", string.IsNullOrEmpty(objModel.Email) ? "" : objModel.Email);
                            if (objModel.Effdate != null)
                            {
                                cmd.Parameters.AddWithValue("peffdate", objModel.Effdate);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("peffdate", new DateTime(1900, 01, 01));
                            }

                            int Res = (cmd.ExecuteNonQuery());

                            if (Res == 0)
                            {
                                Response.Status = false;
                                Response.Message = MessageHelper.ErroeMsg;
                            }
                            else
                            {
                                Response.Status = true;
                                Response.Message = MessageHelper.REMPADD;
                            }
                        }
                    }
                    else
                    {
                        Response.Status = false;
                        Response.Message = "Employee already exists";
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

        public RetiredEmployeeModel GetsingleRempdetails(RetiredEmployeeModel objModel)

        {
            NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString);
            RetiredEmployeeModel RempDetails = new RetiredEmployeeModel();
            DataTable dt = new DataTable();
            try
            {
                // NpgsqlConnection con = new NpgsqlConnection();

                using (con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {

                    con.Open();
                    using (NpgsqlCommand cmd = new NpgsqlCommand("select * from  mdcl_sp_get_singlerempdetails(:pid)", con))
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
                        RempDetails = null;
                    }
                    else
                    {
                        foreach (DataRow drow in dt.Rows)
                        {
                            //  HospitalListModel arnobj = new HospitalListModel();
                            RempDetails.Id = string.IsNullOrWhiteSpace(drow["id"].ToString()) ? 0 : Convert.ToInt32(drow["id"].ToString());
                            RempDetails.EmpId = string.IsNullOrWhiteSpace(drow["empid"].ToString()) ? "" : drow["empid"].ToString();
                            RempDetails.Name = string.IsNullOrWhiteSpace(drow["name"].ToString()) ? "" : drow["name"].ToString();
                            RempDetails.bank_id = string.IsNullOrWhiteSpace(drow["bank_id"].ToString()) ? 0 : Convert.ToInt32(drow["bank_id"].ToString());
                            RempDetails.Bankname = string.IsNullOrWhiteSpace(drow["bankname"].ToString()) ? "" : drow["bankname"].ToString();
                            RempDetails.Ifccode = string.IsNullOrWhiteSpace(drow["ifsccode"].ToString()) ? "" : drow["ifsccode"].ToString();
                            RempDetails.Accno = string.IsNullOrWhiteSpace(drow["accno"].ToString()) ? "" : drow["accno"].ToString();
                            RempDetails.Othinfo = string.IsNullOrWhiteSpace(drow["othinfo"].ToString()) ? "" : drow["othinfo"].ToString();
                            if (!string.IsNullOrWhiteSpace(drow["effdate"].ToString()) && !drow["effdate"].ToString().Contains("1900"))
                            {
                                RempDetails.Effdate = Convert.ToDateTime(drow["effdate"].ToString());
                            }
                            RempDetails.Mobile = string.IsNullOrWhiteSpace(drow["mobile"].ToString()) ? "" : drow["mobile"].ToString();
                            RempDetails.Email = string.IsNullOrWhiteSpace(drow["email"].ToString()) ? "" : drow["email"].ToString();
                            RempDetails.Status = string.IsNullOrWhiteSpace(drow["status"].ToString()) ? false : Convert.ToBoolean(drow["status"].ToString());
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

        public ResponseModel UpdateRemp(RetiredEmployeeModel objModel)
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
                    using (NpgsqlCommand cmd = new NpgsqlCommand("select * from  mdcl_sp_addorupdateremp(:pid,:pempid,:pname,:pbank_id,:pifsccode,:paccno,:pothinfo,:pmobile,:pemail,:peffdate)", con))
                    {
                        cmd.Connection = con;
                        if (objModel.Othinfo == null || objModel.Othinfo == "")
                        {
                            string pothinfo = "";
                            objModel.Othinfo = pothinfo;
                        }
                        else
                        {
                            string pothinfo = objModel.Othinfo;

                        }
                        cmd.Parameters.AddWithValue("pid", objModel.Id);
                        cmd.Parameters.AddWithValue("pempid", objModel.EmpId);
                        cmd.Parameters.AddWithValue("pname", string.IsNullOrEmpty(objModel.Name) ? "" : objModel.Name);
                        cmd.Parameters.AddWithValue("pbank_id", objModel.bank_id);
                        cmd.Parameters.AddWithValue("pifsccode", string.IsNullOrEmpty(objModel.Ifccode) ? "" : objModel.Ifccode);
                        cmd.Parameters.AddWithValue("paccno", string.IsNullOrEmpty(objModel.Accno) ? "" : objModel.Accno);
                        cmd.Parameters.AddWithValue("pothinfo", string.IsNullOrEmpty(objModel.Othinfo) ? "" : objModel.Othinfo);

                        cmd.Parameters.AddWithValue("pmobile", string.IsNullOrEmpty(objModel.Mobile) ? "" : objModel.Mobile);
                        cmd.Parameters.AddWithValue("pemail", string.IsNullOrEmpty(objModel.Email) ? "" : objModel.Email);
                        if (objModel.Effdate != null)
                        {
                            cmd.Parameters.AddWithValue("peffdate", objModel.Effdate);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("peffdate", new DateTime(1900, 01, 01));
                        }

                        int Res = (cmd.ExecuteNonQuery());


                        if (Res == 0)
                        {
                            Response.Status = false;
                            Response.Message = MessageHelper.ErroeMsg;
                        }
                        else
                        {
                            Response.Status = true;
                            Response.Message = MessageHelper.REMPADD;
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

        public ResponseModel RempStatus(RetiredEmployeeModel objModel)
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
                    using (NpgsqlCommand cmd = new NpgsqlCommand("select * from  mdcl_sp_updaterempstatus(:pid)", con))
                    {
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("pid", id);
                        object Res2 = cmd.ExecuteScalar();
                        int Res = Convert.ToInt32(Res2);
                        if (Res == 0)
                        {
                            Response.Status = false;
                            Response.Message = MessageHelper.ErroeMsg;
                        }
                        else
                        {
                            Response.Status = true;
                            Response.Message = MessageHelper.REMPDEL;
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



        public List<RetiredempTransModel> GetTransRetiredEmp()
        {
            List<RetiredempTransModel> Remplist = new List<RetiredempTransModel>();
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {
                    con.Open();
                    Remplist = con.Query<RetiredempTransModel>("select * from mdcl_sp_get_remptranslist()").ToList();

                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                Remplist = new List<RetiredempTransModel>();
                CommonHelper.write_log("error in function  :GetTransRetiredEmp() :" + ex.Message);
            }
            return Remplist;

        }
        public ResponseModel RemptransNewAdd(RetiredempTransModel objModel)
        {
            NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString);
            ResponseModel Response = new ResponseModel();
            DataTable dt = new DataTable();
            bool status = false;
            try
            {
                // NpgsqlConnection con = new NpgsqlConnection();

                using (con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {

                    con.Open();

                    using (NpgsqlCommand cmd = new NpgsqlCommand("select * from  mdcl_sp_addorupdateremptrans(:pid,:pempid,:pname,:pcurryear,:ppreyear,:ptotalclaim,:psancriamt,:pis_paid)", con))
                    {
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("pid", objModel.Id);
                        cmd.Parameters.AddWithValue("pempid", objModel.Empcd);
                        cmd.Parameters.AddWithValue("pname", objModel.Name);
                        cmd.Parameters.AddWithValue("pcurryear", objModel.yearfrom);
                        cmd.Parameters.AddWithValue("ppreyear", objModel.yearto);
                        cmd.Parameters.AddWithValue("ptotalclaim", objModel.Totalclaim);
                        cmd.Parameters.AddWithValue("psancriamt", objModel.sanc_amt);
                        cmd.Parameters.AddWithValue("pis_paid", status);

                        int Res = (cmd.ExecuteNonQuery());

                        if (Res == 0)
                        {
                            Response.Status = false;
                            Response.Message = MessageHelper.ErroeMsg;
                        }
                        else
                        {
                            Response.Status = true;
                            Response.Message = MessageHelper.REMPTRANS;
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

        public RetiredempTransModel GetsingleRemptransdetails(RetiredempTransModel model)
        {
            NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString);
            RetiredempTransModel Remplist = new RetiredempTransModel();
            DataTable dt = new DataTable();

            try
            {

                using (con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {

                    con.Open();
                    using (NpgsqlCommand cmd = new NpgsqlCommand("select * from  mdcl_sp_get_singleremptransdetails(:pid)", con))
                    {
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("pid", model.Id);
                        using (NpgsqlDataAdapter SqDA = new NpgsqlDataAdapter(cmd))
                        {
                            SqDA.Fill(dt);
                        }
                    }

                    if (dt.Rows.Count == 0)
                    {
                        Remplist = new RetiredempTransModel();
                    }
                    else
                    {
                        foreach (DataRow drow in dt.Rows)
                        {
                            // RetiredempTransModel robj = new RetiredempTransModel();
                            Remplist.Id = string.IsNullOrWhiteSpace(drow["id"].ToString()) ? 0 : Convert.ToInt32(drow["id"].ToString());
                            Remplist.Empcd = string.IsNullOrWhiteSpace(drow["empcd"].ToString()) ? "" : drow["empcd"].ToString();
                            Remplist.Name = string.IsNullOrWhiteSpace(drow["name"].ToString()) ? "" : drow["name"].ToString();
                            Remplist.yearfrom = string.IsNullOrWhiteSpace(drow["yearfrom"].ToString()) ? "" : drow["yearfrom"].ToString();
                            Remplist.yearto = string.IsNullOrWhiteSpace(drow["yearto"].ToString()) ? "" : drow["yearto"].ToString();
                            Remplist.Totalclaim = string.IsNullOrWhiteSpace(drow["req_amt"].ToString()) ? 0 : Convert.ToDecimal(drow["req_amt"].ToString());
                            Remplist.sanc_amt = string.IsNullOrWhiteSpace(drow["sanc_amt"].ToString()) ? 0 : Convert.ToDecimal(drow["sanc_amt"].ToString());
                            // Remplist.Balance = string.IsNullOrWhiteSpace(drow["balance"].ToString()) ? 0 : Convert.ToDecimal(drow["Balance"].ToString());
                            //Remplist.Remark = string.IsNullOrWhiteSpace(drow["remark"].ToString()) ? "" : drow["remark"].ToString();
                            // robj.Status = string.IsNullOrWhiteSpace(drow["status"].ToString()) ? false : Convert.ToBoolean(drow["status"].ToString());                           

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                Remplist = new RetiredempTransModel();
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return Remplist;
        }

        public ResponseModel UpdateRemptrans(RetiredempTransModel objModel)
        {
            NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString);
            ResponseModel Response = new ResponseModel();
            DataTable dt = new DataTable();
            bool status = false;
            try
            {
                // NpgsqlConnection con = new NpgsqlConnection();

                using (con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {

                    con.Open();

                    using (NpgsqlCommand cmd = new NpgsqlCommand("select * from  mdcl_sp_addorupdateremptrans(:pid,:pempid,:pname,:pcurryear,:ppreyear,:ptotalclaim,:psancriamt,:pis_paid)", con))
                    {
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("pid", objModel.Id);
                        cmd.Parameters.AddWithValue("pempid", objModel.Empcd);
                        cmd.Parameters.AddWithValue("pname", objModel.Name);
                        cmd.Parameters.AddWithValue("pcurryear", objModel.yearfrom);
                        cmd.Parameters.AddWithValue("ppreyear", objModel.yearto);
                        cmd.Parameters.AddWithValue("ptotalclaim", objModel.Totalclaim);
                        cmd.Parameters.AddWithValue("psancriamt", objModel.sanc_amt);
                        cmd.Parameters.AddWithValue("pis_paid", status);
                        //cmd.Parameters.AddWithValue("pbalance", objModel.Balance);
                        //cmd.Parameters.AddWithValue("premark", objModel.Remark);

                        int Res = (cmd.ExecuteNonQuery());

                        if (Res == 0)
                        {
                            Response.Status = false;
                            Response.Message = MessageHelper.ErroeMsg;
                        }
                        else
                        {
                            Response.Status = true;
                            Response.Message = MessageHelper.REMPTRANS;
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

        public ResponseModel RemptransStatus(RetiredempTransModel objModel)
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
                    using (NpgsqlCommand cmd = new NpgsqlCommand("select * from  mdcl_sp_updateremptransstatus(:pid)", con))
                    {
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("pid", id);
                        object Res2 = cmd.ExecuteScalar();
                        int Res = Convert.ToInt32(Res2);
                        if (Res == 0)
                        {
                            Response.Status = false;
                            Response.Message = MessageHelper.ErroeMsg;
                        }
                        else
                        {
                            Response.Status = true;
                            Response.Message = MessageHelper.REMPTRANSDEL;
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

        public List<ReimburseAmtModel> GetRetiredReimurseAmt()
        {
            NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString);
            List<ReimburseAmtModel> Remplist = new List<ReimburseAmtModel>();
            DataTable dt = new DataTable();
            bool status = true;
            try
            {

                using (con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {

                    con.Open();
                    using (NpgsqlCommand cmd = new NpgsqlCommand("select * from  mdcl_sp_get_reimbuseramtlist(:pstatus)", con))
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
                        Remplist = new List<ReimburseAmtModel>();
                    }
                    else
                    {
                        foreach (DataRow drow in dt.Rows)
                        {
                            ReimburseAmtModel robj = new ReimburseAmtModel();
                            robj.Id = string.IsNullOrWhiteSpace(drow["id"].ToString()) ? 0 : Convert.ToInt32(drow["id"].ToString());
                            robj.Finyear = string.IsNullOrWhiteSpace(drow["finyear"].ToString()) ? "" : drow["finyear"].ToString();
                            robj.Amount = string.IsNullOrWhiteSpace(drow["amount"].ToString()) ? 0 : Convert.ToDecimal(drow["amount"].ToString());
                            robj.Status = string.IsNullOrWhiteSpace(drow["status"].ToString()) ? false : Convert.ToBoolean(drow["status"].ToString());
                            Remplist.Add(robj);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                Remplist = new List<ReimburseAmtModel>();
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return Remplist;
        }

        public ResponseModel AddReimbusamt(ReimburseAmtModel objModel)
        {
            NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString);
            ResponseModel Response = new ResponseModel();
            DataTable dt = new DataTable();
            bool status = true;
            //objModel.Amount = Convert.ToDecimal(objModel.Amount);
            try
            {
                // NpgsqlConnection con = new NpgsqlConnection();

                using (con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {

                    con.Open();

                    using (NpgsqlCommand cmd = new NpgsqlCommand("select * from  mdcl_sp_updatereimburseamt(:pid,:pfinyear,:pamount,:pstatus)", con))
                    {
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("pid", objModel.Id);
                        cmd.Parameters.AddWithValue("pfinyear", objModel.Finyear);
                        cmd.Parameters.AddWithValue("pamount", objModel.Amount);
                        cmd.Parameters.AddWithValue("pstatus", status);

                        object Res2 = (cmd.ExecuteScalar());
                        int Res = Convert.ToInt32(Res2);
                        if (Res == 0)
                        {
                            Response.Status = false;
                            Response.Message = MessageHelper.ErroeMsg;
                        }
                        else
                        {
                            Response.Status = true;
                            Response.Message = MessageHelper.REMPAMTADD;
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

        public ReimburseAmtModel GetsingleReimburseamt(ReimburseAmtModel objModel)
        {
            NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString);
            ReimburseAmtModel RempDetails = new ReimburseAmtModel();
            DataTable dt = new DataTable();
            try
            {
                // NpgsqlConnection con = new NpgsqlConnection();

                using (con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {

                    con.Open();
                    using (NpgsqlCommand cmd = new NpgsqlCommand("select * from  mdcl_sp_get_singlereimburseamt(:pid)", con))
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
                        RempDetails = null;
                    }
                    else
                    {
                        foreach (DataRow drow in dt.Rows)
                        {
                            //  HospitalListModel arnobj = new HospitalListModel();
                            RempDetails.Id = string.IsNullOrWhiteSpace(drow["id"].ToString()) ? 0 : Convert.ToInt32(drow["id"].ToString());
                            RempDetails.Finyear = string.IsNullOrWhiteSpace(drow["finyear"].ToString()) ? "" : drow["finyear"].ToString();
                            RempDetails.Amount = string.IsNullOrWhiteSpace(drow["amount"].ToString()) ? 0 : Convert.ToDecimal(drow["amount"].ToString());
                            RempDetails.Status = string.IsNullOrWhiteSpace(drow["status"].ToString()) ? false : Convert.ToBoolean(drow["status"].ToString());
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

        public ResponseModel UpdateReimburse(ReimburseAmtModel objModel)
        {
            NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString);
            ResponseModel Response = new ResponseModel();
            DataTable dt = new DataTable();
            bool status = true;
            //objModel.Amount = Convert.ToDecimal(objModel.Amount);
            try
            {
                // NpgsqlConnection con = new NpgsqlConnection();

                using (con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {

                    con.Open();

                    using (NpgsqlCommand cmd = new NpgsqlCommand("select * from  mdcl_sp_updatereimburseamt(:pid,:pfinyear,:pamount,:pstatus)", con))
                    {
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("pid", objModel.Id);
                        cmd.Parameters.AddWithValue("pfinyear", objModel.Finyear);
                        cmd.Parameters.AddWithValue("pamount", objModel.Amount);
                        cmd.Parameters.AddWithValue("pstatus", status);

                        object Res2 = (cmd.ExecuteScalar());
                        int Res = Convert.ToInt32(Res2);
                        if (Res == 0)
                        {
                            Response.Status = false;
                            Response.Message = MessageHelper.ErroeMsg;
                        }
                        else
                        {
                            Response.Status = true;
                            Response.Message = MessageHelper.REMPAMTADD;
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

        public ResponseModel ReimburseamtStatus(ReimburseAmtModel objModel)
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
                    using (NpgsqlCommand cmd = new NpgsqlCommand("select * from  mdcl_sp_updateramtstatus(:pid)", con))
                    {
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("pid", id);
                        object Res2 = cmd.ExecuteScalar();
                        int Res = Convert.ToInt32(Res2);
                        if (Res == 0)
                        {
                            Response.Status = false;
                            Response.Message = MessageHelper.ErroeMsg;
                        }
                        else
                        {
                            Response.Status = true;
                            Response.Message = MessageHelper.RAMTDEL;
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

        public List<RempDemiseModel> GetRetiredEmpDemise()
        {
            NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString);
            List<RempDemiseModel> Remplist = new List<RempDemiseModel>();
            DataTable dt = new DataTable();
            bool status = true;
            try
            {

                using (con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {

                    con.Open();
                    using (NpgsqlCommand cmd = new NpgsqlCommand("select * from  mdcl_sp_get_rempdemiselist(:pstatus)", con))
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
                        Remplist = new List<RempDemiseModel>();
                    }
                    else
                    {
                        foreach (DataRow drow in dt.Rows)
                        {
                            RempDemiseModel robj = new RempDemiseModel();
                            robj.Id = string.IsNullOrWhiteSpace(drow["id"].ToString()) ? 0 : Convert.ToInt32(drow["id"].ToString());
                            robj.EmpId = string.IsNullOrWhiteSpace(drow["empid"].ToString()) ? "" : drow["empid"].ToString();
                            robj.name = string.IsNullOrWhiteSpace(drow["name"].ToString()) ? "" : drow["name"].ToString();
                            robj.Ddate = Convert.ToDateTime(drow["ddate"].ToString());
                            robj.Dcertificate = string.IsNullOrWhiteSpace(drow["dcertificate"].ToString()) ? "" : drow["dcertificate"].ToString();
                            robj.Lreceived = string.IsNullOrWhiteSpace(drow["lreceived"].ToString()) ? "" : drow["lreceived"].ToString();
                            robj.Sbankname = string.IsNullOrWhiteSpace(drow["sbankname"].ToString()) ? "" : drow["sbankname"].ToString();
                            robj.Sifsccode = string.IsNullOrWhiteSpace(drow["sifsccode"].ToString()) ? "" : drow["sifsccode"].ToString();
                            robj.Saccno = string.IsNullOrWhiteSpace(drow["saccno"].ToString()) ? "" : drow["saccno"].ToString();
                            robj.Smobile = string.IsNullOrWhiteSpace(drow["smobile"].ToString()) ? "" : drow["smobile"].ToString();
                            robj.Semail = string.IsNullOrWhiteSpace(drow["semail"].ToString()) ? "" : drow["semail"].ToString();
                            // robj.Status = string.IsNullOrWhiteSpace(drow["status"].ToString()) ? false : Convert.ToBoolean(drow["status"].ToString());
                            //robj.Bankname = string.IsNullOrWhiteSpace(drow["pincode"].ToString()) ? 0 : Convert.ToInt32(drow["pincode"].ToString());
                            Remplist.Add(robj);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                Remplist = new List<RempDemiseModel>();
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return Remplist;
        }

        public ResponseModel AddRempDemise(RempDemiseModel objModel)
        {
            NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString);
            ResponseModel Response = new ResponseModel();
            DataTable dt = new DataTable();
            bool status = true;
            bool dstatus = true;
            try
            {
                // NpgsqlConnection con = new NpgsqlConnection();

                using (con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {

                    con.Open();

                    using (NpgsqlCommand cmd = new NpgsqlCommand("select * from  mdcl_sp_addorupdaterempdemise(:pid,:pempid,:pddate,:pdcertificate,:plreceived,:psname,:psbankname,:psifsccode,:psaccno,:psmobile,:psemail,:pstatus,:pdstatus)", con))
                    {
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("pid", objModel.Id);
                        cmd.Parameters.AddWithValue("pempid", objModel.EmpId);
                        // cmd.Parameters.AddWithValue("pdempname", objModel.Dempname);
                        cmd.Parameters.AddWithValue("pddate", objModel.Ddate);
                        cmd.Parameters.AddWithValue("pdcertificate", objModel.Dcertificate);
                        cmd.Parameters.AddWithValue("plreceived", objModel.Lreceived);
                        cmd.Parameters.AddWithValue("psname", objModel.Sname);
                        cmd.Parameters.AddWithValue("psbankname", objModel.Sbankname);
                        cmd.Parameters.AddWithValue("psifsccode", string.IsNullOrEmpty(objModel.Sifsccode) ? "" : objModel.Sifsccode);
                        cmd.Parameters.AddWithValue("psaccno", objModel.Saccno);
                        cmd.Parameters.AddWithValue("psmobile", string.IsNullOrEmpty(objModel.Smobile) ? "" : objModel.Smobile);
                        cmd.Parameters.AddWithValue("psemail", string.IsNullOrEmpty(objModel.Semail) ? "" : objModel.Semail);
                        cmd.Parameters.AddWithValue("pstatus", status);
                        cmd.Parameters.AddWithValue("pdstatus", dstatus);

                        int Res = (cmd.ExecuteNonQuery());

                        if (Res == 0)
                        {
                            Response.Status = false;
                            Response.Message = MessageHelper.ErroeMsg;
                        }
                        else
                        {
                            Response.Status = true;
                            Response.Message = MessageHelper.REMPADD;
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


        public RempDemiseModel GetsingleRempdemise(RempDemiseModel model)
        {
            NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString);
            RempDemiseModel robj = new RempDemiseModel();
            DataTable dt = new DataTable();

            try
            {

                using (con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {

                    con.Open();
                    using (NpgsqlCommand cmd = new NpgsqlCommand("select * from  mdcl_sp_get_getsinglerempdemise(:pid)", con))
                    {
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("pid", model.Id);
                        using (NpgsqlDataAdapter SqDA = new NpgsqlDataAdapter(cmd))
                        {
                            SqDA.Fill(dt);
                        }
                    }

                    if (dt.Rows.Count == 0)
                    {
                        robj = new RempDemiseModel();
                    }
                    else
                    {
                        foreach (DataRow drow in dt.Rows)
                        {

                            robj.Id = string.IsNullOrWhiteSpace(drow["id"].ToString()) ? 0 : Convert.ToInt32(drow["id"].ToString());
                            robj.EmpId = string.IsNullOrWhiteSpace(drow["empid"].ToString()) ? "" : drow["empid"].ToString();
                            robj.name = string.IsNullOrWhiteSpace(drow["name"].ToString()) ? "" : drow["name"].ToString();
                            robj.Ddate = Convert.ToDateTime(drow["ddate"].ToString());
                            robj.Dcertificate = string.IsNullOrWhiteSpace(drow["dcertificate"].ToString()) ? "" : drow["dcertificate"].ToString();
                            robj.Lreceived = string.IsNullOrWhiteSpace(drow["lreceived"].ToString()) ? "" : drow["lreceived"].ToString();
                            // robj.Sname = string.IsNullOrWhiteSpace(drow["sname"].ToString()) ? "" : drow["sname"].ToString();
                            robj.Sbankname = string.IsNullOrWhiteSpace(drow["sbankname"].ToString()) ? "" : drow["sbankname"].ToString();
                            robj.Sifsccode = string.IsNullOrWhiteSpace(drow["sifsccode"].ToString()) ? "" : drow["sifsccode"].ToString();
                            robj.Saccno = string.IsNullOrWhiteSpace(drow["saccno"].ToString()) ? "" : drow["saccno"].ToString();
                            robj.Smobile = string.IsNullOrWhiteSpace(drow["smobile"].ToString()) ? "" : drow["smobile"].ToString();
                            robj.Semail = string.IsNullOrWhiteSpace(drow["semail"].ToString()) ? "" : drow["semail"].ToString();

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                robj = new RempDemiseModel();
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return robj;
        }

        public ResponseModel UpdateRempdemise(RempDemiseModel objModel)
        {
            NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString);
            ResponseModel Response = new ResponseModel();
            DataTable dt = new DataTable();
            bool status = true;
            bool dstatus = true;
            try
            {
                // NpgsqlConnection con = new NpgsqlConnection();

                using (con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {

                    con.Open();

                    using (NpgsqlCommand cmd = new NpgsqlCommand("select * from  mdcl_sp_addorupdaterempdemise(:pid,:pempid,:pddate,:pdcertificate,:plreceived,:psname,:psbankname,:psifsccode,:psaccno,:psmobile,:psemail,:pstatus,:pdstatus)", con))
                    {
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("pid", objModel.Id);
                        cmd.Parameters.AddWithValue("pempid", objModel.EmpId);
                        // cmd.Parameters.AddWithValue("pdempname", objModel.Dempname);
                        cmd.Parameters.AddWithValue("pddate", objModel.Ddate);
                        cmd.Parameters.AddWithValue("pdcertificate", objModel.Dcertificate);
                        cmd.Parameters.AddWithValue("plreceived", objModel.Lreceived);
                        cmd.Parameters.AddWithValue("psname", objModel.Sname);
                        cmd.Parameters.AddWithValue("psbankname", objModel.Sbankname);
                        cmd.Parameters.AddWithValue("psifsccode", string.IsNullOrEmpty(objModel.Sifsccode) ? "" : objModel.Sifsccode);
                        cmd.Parameters.AddWithValue("psaccno", objModel.Saccno);
                        cmd.Parameters.AddWithValue("psmobile", string.IsNullOrEmpty(objModel.Smobile) ? "" : objModel.Smobile);
                        cmd.Parameters.AddWithValue("psemail", string.IsNullOrEmpty(objModel.Semail) ? "" : objModel.Semail);
                        cmd.Parameters.AddWithValue("pstatus", status);
                        cmd.Parameters.AddWithValue("pdstatus", dstatus);
                        int Res = (cmd.ExecuteNonQuery());

                        if (Res == 0)
                        {
                            Response.Status = false;
                            Response.Message = MessageHelper.ErroeMsg;
                        }
                        else
                        {
                            Response.Status = true;
                            Response.Message = MessageHelper.REMPADD;
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


        public ResponseModel RempDemiseStatus(RempDemiseModel objModel)
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
                    using (NpgsqlCommand cmd = new NpgsqlCommand("select * from  mdcl_sp_updaterempdemisestatus(:pid)", con))
                    {
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("pid", id);
                        object Res2 = cmd.ExecuteScalar();
                        int Res = Convert.ToInt32(Res2);
                        if (Res == 0)
                        {
                            Response.Status = false;
                            Response.Message = MessageHelper.ErroeMsg;
                        }
                        else
                        {
                            Response.Status = true;
                            Response.Message = MessageHelper.RAMTDEL;
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
        public List<DropdownModel> GetYears(string empcd)
        {
            NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString);
            List<DropdownModel> yearlist = new List<DropdownModel>();
            DataTable dt = new DataTable();
            var Empcd = empcd;
            try
            {
                using (con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {
                    con.Open();
                    using (NpgsqlCommand cmd = new NpgsqlCommand("select * from  mdcl_sp_get_tofill_yeardropdown(:pid)", con))
                    {
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("pid", Empcd);
                        using (NpgsqlDataAdapter SqDA = new NpgsqlDataAdapter(cmd))
                        {
                            SqDA.Fill(dt);
                        }
                    }

                    if (dt.Rows.Count == 0)
                    {
                        yearlist = new List<DropdownModel>();
                    }
                    else
                    {
                        foreach (DataRow drow in dt.Rows)
                        {
                            DropdownModel expobj = new DropdownModel();
                            expobj.Id = string.IsNullOrWhiteSpace(drow["id"].ToString()) ? 0 : Convert.ToInt32(drow["id"].ToString());
                            expobj.Name = string.IsNullOrWhiteSpace(drow["name"].ToString()) ? "" : drow["name"].ToString();
                            yearlist.Add(expobj);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                yearlist = new List<DropdownModel>();
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return yearlist;
        }
        public List<RetiredempTransModel> GetdataForLot()
        {
            List<RetiredempTransModel> Remplist = new List<RetiredempTransModel>();
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {
                    con.Open();
                    Remplist = con.Query<RetiredempTransModel>("select * from mdcl_sp_get_data_for_lot_genration()").ToList();
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                Remplist = new List<RetiredempTransModel>();
            }
            return Remplist;

        }
        public RetiredempTransModel GetName(string empcd)
        {
            RetiredempTransModel Remplist = new RetiredempTransModel();
            var Empcd = empcd;
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {
                    con.Open();
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("empcd", Empcd);
                    Remplist = con.Query<RetiredempTransModel>("select * from mdcl_sp_get_name_by_emcdcode(:empcd)", parameters).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                Remplist = new RetiredempTransModel();
            }
            return Remplist;

        }
        public List<RetiredempTransModel> CheckingVerify()
        {
            List<RetiredempTransModel> Remplist = new List<RetiredempTransModel>();
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {
                    con.Open();
                    Remplist = con.Query<RetiredempTransModel>("select * from mdcl_sp_get_data_of_retiredtrans_for_editchecking()").ToList();
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                Remplist = new List<RetiredempTransModel>();
            }
            return Remplist;

        }
        public RetiredempTransModel GetsingleEmpForChecking(RetiredempTransModel model)
        {
            RetiredempTransModel Remplist = new RetiredempTransModel();
            string Empcd;
            Empcd = model.Empcd;
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {
                    con.Open();
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("pempcd", Empcd);
                    Remplist = con.Query<RetiredempTransModel>("select * from mdcl_sp_get_singleremptransdetails_verify(:pempcd)", parameters).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                Remplist = new RetiredempTransModel();
            }
            return Remplist;

        }

        public ResponseModel CheckVerified(RetiredempTransModel objModel)
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

                    using (NpgsqlCommand cmd = new NpgsqlCommand("select * from mdcl_sp_update_retiredtrans_checking_verifried(:pid,:pyearfrom,:pyearto,:psanc_amt)", con))

                    {
                        cmd.Connection = con;

                        cmd.Parameters.AddWithValue("pid", objModel.Id);
                        cmd.Parameters.AddWithValue("pyearfrom", objModel.yearfrom);
                        cmd.Parameters.AddWithValue("pyearto", objModel.yearto);
                        cmd.Parameters.AddWithValue("psanc_amt", objModel.sanc_amt);


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
        //public List<RetiredempTransModel> Trans_Sanction_Grid()
        //{
        //    List<RetiredempTransModel> Remplist = new List<RetiredempTransModel>();
        //    try
        //    {
        //        using (NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString))
        //        {
        //            con.Open();
        //            Remplist = con.Query<RetiredempTransModel>("select * from mdcl_sp_get_data_of_retiredtrans_for_sanctiongrid()").ToList();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionLogging.LogException(ex);
        //        Remplist = new List<RetiredempTransModel>();
        //    }
        //    return Remplist;

        //}
        public List<RetiredempTransModel> Trans_Sanction_Grid()
        {
            List<RetiredempTransModel> Remplist = new List<RetiredempTransModel>();
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {
                    con.Open();
                    Remplist = con.Query<RetiredempTransModel>("select * from mdcl_sp_get_empl_trans_for_sanction()").ToList();
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                Remplist = new List<RetiredempTransModel>();
            }
            return Remplist;

        }
        public RetiredempTransModel GetsingleEmpForSanction(RetiredempTransModel model)
        {
            RetiredempTransModel Remplist = new RetiredempTransModel();
            string Empcd;
            Empcd = model.Empcd;
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {
                    con.Open();
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("pempcd", Empcd);
                    Remplist = con.Query<RetiredempTransModel>("select * from mdcl_sp_get_singleremptransdetails_sanction(:pempcd)", parameters).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                Remplist = new RetiredempTransModel();
            }
            return Remplist;

        }
        public ResponseModel AddSanction(RetiredempTransModel objModel)
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

                    using (NpgsqlCommand cmd = new NpgsqlCommand("select * from mdcl_sp_update_retiredtrans_sanctioned(:pid)", con))

                    {
                        cmd.Connection = con;

                        cmd.Parameters.AddWithValue("pid", objModel.Id);



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
        public ResponseModel Reject(RetiredempTransModel objModel)
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

                    using (NpgsqlCommand cmd = new NpgsqlCommand("select * from mdcl_sp_update_retiredtrans_reject(:pid)", con))

                    {
                        cmd.Connection = con;

                        cmd.Parameters.AddWithValue("pid", objModel.Id);



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
        public RetiredempTransModel GetTransOfficeNote(string lotno)
        {
            List<emplSanc> emplist = new List<emplSanc>();
            RetiredempTransModel RempDetails = new RetiredempTransModel();
            var lotn = lotno;
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {
                    con.Open();
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("lotno", lotn);
                    RempDetails = con.Query<RetiredempTransModel>("select * from mdcl_sp_get_empl_lotno_for_officenote(:lotno)", parameters).FirstOrDefault();
                    emplist = con.Query<emplSanc>("select * from mdcl_sp_get_empl_trans_for_officenote(:lotno)", parameters).ToList();
                    foreach (emplSanc ex_ty_details in emplist)
                    {
                        if (RempDetails.EmplSanc_list == null)
                        {
                            RempDetails.EmplSanc_list = new List<emplSanc>();
                        }
                        RempDetails.EmplSanc_list.Add(ex_ty_details);

                        // k++;
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                RempDetails = new RetiredempTransModel();
            }
            return RempDetails;

        }
        public List<RetiredempTransModel> GetLoTNo()
        {
            List<RetiredempTransModel> Remplist = new List<RetiredempTransModel>();
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {
                    con.Open();
                    Remplist = con.Query<RetiredempTransModel>("select * from mdcl_sp_get_empl_lotno()").ToList();
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                Remplist = new List<RetiredempTransModel>();
            }
            return Remplist;

        }
        public List<RetiredempTransModel> GetEmpByLoT(string lotno)
        {
            List<RetiredempTransModel> Remplist = new List<RetiredempTransModel>();
            var lotn = lotno;
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {
                    con.Open();
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("lotno", lotn);
                    Remplist = con.Query<RetiredempTransModel>("select * from mdcl_sp_get_empl_by_lotno(:lotno)", parameters).ToList();
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                Remplist = new List<RetiredempTransModel>();
            }
            return Remplist;

        }
        public List<RetiredempTransModel> GetEmpByEmpcd(RetiredempTransModel objmodel)
        {
            List<RetiredempTransModel> Remplist = new List<RetiredempTransModel>();
            string empcd = objmodel.Empcd;
            string lotno = objmodel.lot_no;
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {
                    con.Open();
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("pempcd", empcd);
                    parameters.Add("lotno", lotno);
                    Remplist = con.Query<RetiredempTransModel>("select * from mdcl_sp_get_emp_list(:pempcd,:lotno)", parameters).ToList();
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                Remplist = new List<RetiredempTransModel>();
            }
            return Remplist;

        }
        //public List<RetiredempTransModel> GetEmpByEmpcd(string pempcd)
        //{
        //    List<RetiredempTransModel> Remplist = new List<RetiredempTransModel>();
        //    var empcd = pempcd;
        //    try
        //    {
        //        using (NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString))
        //        {
        //            con.Open();
        //            DynamicParameters parameters = new DynamicParameters();
        //            parameters.Add("pempcd", empcd);
        //            Remplist = con.Query<RetiredempTransModel>("select * from mdcl_sp_get_empl_by_empcd(:pempcd)", parameters).ToList();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionLogging.LogException(ex);
        //        Remplist = new List<RetiredempTransModel>();
        //    }
        //    return Remplist;

        //}
        public RetiredempTransModel GetEmpByLoTAnnexure(string lotno)
        {
            List<emplSanc> emplist = new List<emplSanc>();
            RetiredempTransModel RempDetails = new RetiredempTransModel();
            var lotn = lotno;
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {
                    con.Open();
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("lotno", lotn);
                    RempDetails = con.Query<RetiredempTransModel>("select * from mdcl_sp_get_empl_lotno_for_Annexure(:lotno)", parameters).FirstOrDefault();
                    emplist = con.Query<emplSanc>("select * from mdcl_sp_get_empl_by_lotno_for_annexure(:lotno)", parameters).ToList();
                    foreach (emplSanc ex_ty_details in emplist)
                    {
                        if (RempDetails.EmplSanc_list == null)
                        {
                            RempDetails.EmplSanc_list = new List<emplSanc>();
                        }
                        RempDetails.EmplSanc_list.Add(ex_ty_details);

                        // k++;
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                RempDetails = new RetiredempTransModel();
            }
            return RempDetails;

        }
        public RetiredempTransModel GetAmount(string pyearfrom)
        {
            RetiredempTransModel Remplist = new RetiredempTransModel();
            var Yearfrom = pyearfrom;
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {
                    con.Open();
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("pyearfrom", Yearfrom);
                    Remplist = con.Query<RetiredempTransModel>("select * from mdcl_sp_get_amount(:pyearfrom)", parameters).FirstOrDefault();
                    // CommonHelper.write_log("amout :" + Remplist.Name);
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                Remplist = new RetiredempTransModel();
                CommonHelper.write_log("error in function  :GetAmount() :" + ex.Message);
            }
            return Remplist;

        }
        public ResponseModel CheckInTable(RetiredEmployeeModel objModel)
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

                    using (NpgsqlCommand cmd = new NpgsqlCommand("select * from mdcl_sp_check_empl_alreadyexit(:pid)", con))

                    {
                        cmd.Connection = con;

                        cmd.Parameters.AddWithValue("pid", objModel.EmpId);



                        int Res = (cmd.ExecuteNonQuery());

                        if (Res == 0)
                        {
                            Response.Status = false;

                        }
                        else
                        {
                            Response.Status = true;
                            Response.Message = MessageHelper.AlrExitsStatus;

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
        public ResponseModel ApproveLotno(RetiredempTransModel objModel)
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

                    using (NpgsqlCommand cmd = new NpgsqlCommand("select * from  mdcl_sp_update_retiredtrans_approve_by_lotno(:pid)", con))
                    {
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("pid", objModel.lot_no);


                        int Res = (cmd.ExecuteNonQuery());

                        if (Res == 0)
                        {
                            Response.Status = false;
                            Response.Message = MessageHelper.ErroeMsg;
                        }
                        else
                        {
                            Response.Status = true;
                            Response.Message = MessageHelper.Approved;
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
        public ResponseModel ApproveTrans(RetiredempTransModel objModel)
        {
            NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString);
            ResponseModel Response = new ResponseModel();
            RetiredempTransModel RempDetails = new RetiredempTransModel();
            DataTable dt = new DataTable();

            try
            {
                // NpgsqlConnection con = new NpgsqlConnection();

                using (con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {

                    con.Open();
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("lotno", objModel.lot_no);
                    RempDetails = con.Query<RetiredempTransModel>("select * from mdcl_sp_check_lotno_is_approved(:lotno)", parameters).FirstOrDefault();

                    if (RempDetails.Status == true)
                    {
                        Response.Status = true;
                        Response.Message = MessageHelper.REMPTRANS;

                    }
                    else
                    {
                        Response.Status = false;
                        Response.Message = MessageHelper.PendingLot;
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
        public List<DropdownModel> GetYearsforSpecialcase(string empcd)
        {
            NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString);
            List<DropdownModel> yearlist = new List<DropdownModel>();
            DataTable dt = new DataTable();
            var Empcd = empcd;
            try
            {
                using (con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {
                    con.Open();
                    using (NpgsqlCommand cmd = new NpgsqlCommand("select * from  mdcl_sp_get_tofill_yeardropdown_for_specase(:pid)", con))
                    {
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("pid", Empcd);
                        using (NpgsqlDataAdapter SqDA = new NpgsqlDataAdapter(cmd))
                        {
                            SqDA.Fill(dt);
                        }
                    }

                    if (dt.Rows.Count == 0)
                    {
                        yearlist = new List<DropdownModel>();
                    }
                    else
                    {
                        foreach (DataRow drow in dt.Rows)
                        {
                            DropdownModel expobj = new DropdownModel();
                            expobj.Id = string.IsNullOrWhiteSpace(drow["id"].ToString()) ? 0 : Convert.ToInt32(drow["id"].ToString());
                            expobj.Name = string.IsNullOrWhiteSpace(drow["name"].ToString()) ? "" : drow["name"].ToString();
                            yearlist.Add(expobj);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                yearlist = new List<DropdownModel>();
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return yearlist;
        }
        public List<DropdownModel> GetYearsforSpecialcaseEdit(string empcd)
        {
            NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString);
            List<DropdownModel> yearlist = new List<DropdownModel>();
            DataTable dt = new DataTable();
            var Empcd = empcd;
            try
            {
                using (con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {
                    con.Open();
                    using (NpgsqlCommand cmd = new NpgsqlCommand("select * from  mdcl_sp_get_tofill_yeardropdown_for_specaseedit(:pid)", con))
                    {
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("pid", Empcd);
                        using (NpgsqlDataAdapter SqDA = new NpgsqlDataAdapter(cmd))
                        {
                            SqDA.Fill(dt);
                        }
                    }

                    if (dt.Rows.Count == 0)
                    {
                        yearlist = new List<DropdownModel>();
                    }
                    else
                    {
                        foreach (DataRow drow in dt.Rows)
                        {
                            DropdownModel expobj = new DropdownModel();
                            expobj.Id = string.IsNullOrWhiteSpace(drow["id"].ToString()) ? 0 : Convert.ToInt32(drow["id"].ToString());
                            expobj.Name = string.IsNullOrWhiteSpace(drow["name"].ToString()) ? "" : drow["name"].ToString();
                            yearlist.Add(expobj);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                yearlist = new List<DropdownModel>();
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return yearlist;
        }
        public List<DropdownModel> GetBank()
        {
            NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString);
            List<DropdownModel> banklist = new List<DropdownModel>();
            DataTable dt = new DataTable();

            try
            {
                using (con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {
                    con.Open();
                    using (NpgsqlCommand cmd = new NpgsqlCommand("select * from  mdcl_sp_get_bankname()", con))
                    {
                        cmd.Connection = con;

                        using (NpgsqlDataAdapter SqDA = new NpgsqlDataAdapter(cmd))
                        {
                            SqDA.Fill(dt);
                        }
                    }

                    if (dt.Rows.Count == 0)
                    {
                        banklist = new List<DropdownModel>();
                    }
                    else
                    {
                        foreach (DataRow drow in dt.Rows)
                        {
                            DropdownModel expobj = new DropdownModel();
                            expobj.Id = string.IsNullOrWhiteSpace(drow["id"].ToString()) ? 0 : Convert.ToInt32(drow["id"].ToString());
                            expobj.Name = string.IsNullOrWhiteSpace(drow["name"].ToString()) ? "" : drow["name"].ToString();
                            banklist.Add(expobj);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                banklist = new List<DropdownModel>();
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return banklist;
        }
        public RetiredempTransModel GetTransBankAdvise(string lotno)
        {

            RetiredempTransModel RempDetails = new RetiredempTransModel();
            var lotn = lotno;
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {
                    con.Open();
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("lotno", lotn);
                    RempDetails = con.Query<RetiredempTransModel>("select * from mdcl_sp_get_empl_trans_for_bankadvise(:lotno)", parameters).FirstOrDefault();


                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                RempDetails = new RetiredempTransModel();
            }
            return RempDetails;

        }
        public List<RetiredempTransModel> GetEmplPending(RetiredempTransModel objmodel)
        {
            List<RetiredempTransModel> Remplist = new List<RetiredempTransModel>();
            string empcd = objmodel.Empcd;

            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {
                    con.Open();
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("pid", empcd);
                    string varVal = con.Query<string>("select * from mdcl_sp_check_empl_alreadyexit(:pid)", parameters).FirstOrDefault();
                    if (varVal == "1")
                    {

                        Remplist = con.Query<RetiredempTransModel>("select * from mdcl_sp_get_pendinglist(:pid)", parameters).ToList();
                    }
                    else
                    {
                        List<RetiredempTransModel> emplist = new List<RetiredempTransModel>();
                        Remplist = emplist;
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                Remplist = new List<RetiredempTransModel>();
            }
            return Remplist;

        }
        //public List<RetiredempTransModel> GetEmplPending(RetiredempTransModel objmodel)
        //{
        //    List<RetiredempTransModel> Remplist = new List<RetiredempTransModel>();
        //    string empcd = objmodel.Empcd;

        //    try
        //    {
        //        using (NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString))
        //        {
        //            con.Open();
        //            DynamicParameters parameters = new DynamicParameters();
        //            parameters.Add("pid", empcd);

        //            Remplist = con.Query<RetiredempTransModel>("select * from mdcl_sp_get_pendinglist(:pid)", parameters).ToList();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionLogging.LogException(ex);
        //        Remplist = new List<RetiredempTransModel>();
        //    }
        //    return Remplist;

        //}
        public RetiredempTransModel EditCheckingPrint()
        {
            RetiredempTransModel RempDetails = new RetiredempTransModel();
            List<emplSanc> emplist = new List<emplSanc>();

            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {
                    con.Open();
                    RempDetails = con.Query<RetiredempTransModel>("select * from mdcl_sp_get_date()").FirstOrDefault();
                    emplist = con.Query<emplSanc>("select * from mdcl_sp_get_data_of_retiredtrans_for_editchecking()").ToList();
                    foreach (emplSanc ex_ty_details in emplist)
                    {
                        if (RempDetails.EmplSanc_list == null)
                        {
                            RempDetails.EmplSanc_list = new List<emplSanc>();
                        }
                        RempDetails.EmplSanc_list.Add(ex_ty_details);

                        // k++;
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                RempDetails = new RetiredempTransModel();
            }
            return RempDetails;

        }
        public RetiredempTransModel EmplApprovalList()
        {
            RetiredempTransModel RempDetails = new RetiredempTransModel();
            List<emplSanc> emplist = new List<emplSanc>();

            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {
                    con.Open();
                    RempDetails = con.Query<RetiredempTransModel>("select * from mdcl_sp_get_date()").FirstOrDefault();
                    emplist = con.Query<emplSanc>("select * from mdcl_sp_get_empl_trans_for_sanction()").ToList();
                    foreach (emplSanc ex_ty_details in emplist)
                    {
                        if (RempDetails.EmplSanc_list == null)
                        {
                            RempDetails.EmplSanc_list = new List<emplSanc>();
                        }
                        RempDetails.EmplSanc_list.Add(ex_ty_details);

                        // k++;
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                RempDetails = new RetiredempTransModel();
            }
            return RempDetails;

        }

        public byte[] GetEditCheckingHtmlString()
        {
            DataTable dt = new DataTable();
            StringBuilder sb = new StringBuilder();
            using (NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString))
            {
                con.Open();
                List<RetiredempTransModel> Remplist = con.Query<RetiredempTransModel>("select * from mdcl_sp_get_data_of_retiredtrans_for_editchecking()").ToList();

                using (StreamReader sr = new StreamReader(System.Configuration.ConfigurationManager.AppSettings["Gridhtml"].ToString()))
                {

                    int rownumber = 0;
                    decimal total_amount = 0;
                    String line;

                    while ((line = sr.ReadLine()) != null)
                    {
                        line = line.Replace("#listDate", DateTime.Now.ToString("dd-MMM-yyyy"));

                        if ((line.Trim() == ("<tbody>") && rownumber <= dt.Rows.Count))
                        {


                            foreach (RetiredempTransModel model in Remplist)
                            {
                                rownumber++;

                                string tr_line = "<tbody>" + "<tr><td>" + rownumber + "</td><td>" + model.Empcd + "</td><td>" + model.Name + "</td><td>" + model.yearfrom + "-" + model.yearto + "</td><td>" + model.sanc_amt + "</td><td>" + model.bankname + "</td><td>" + model.accno + "</td><td>" + model.ifsccode + "</td></tr>";
                                sb.AppendLine(tr_line);
                                total_amount += model.sanc_amt;
                            }

                        }
                        else if (rownumber == Remplist.Count)
                        {
                            string tr_line = "<tr><td></td><td></td><td></td><td>Total</td><td>" + total_amount + "</td><td></td><td></td><td></td></tr>";
                            sb.AppendLine(tr_line);
                            sb.AppendLine(line);
                            break;
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

        public byte[] GetApprovalHtmlString()
        {
            DataTable dt = new DataTable();
            StringBuilder sb = new StringBuilder();
            using (NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString))
            {
                con.Open();
                List<RetiredempTransModel> Remplist = con.Query<RetiredempTransModel>("select * from mdcl_sp_get_empl_trans_for_sanction()").ToList();

                using (StreamReader sr = new StreamReader(System.Configuration.ConfigurationManager.AppSettings["Approvalhtml"].ToString()))
                {

                    int rownumber = 0;
                    decimal total_amount = 0;
                    String line;

                    while ((line = sr.ReadLine()) != null)
                    {
                        line = line.Replace("#listDate", DateTime.Now.ToString("dd-MMM-yyyy"));

                        if ((line.Trim() == ("<tbody>") && rownumber <= dt.Rows.Count))
                        {


                            foreach (RetiredempTransModel model in Remplist)
                            {
                                rownumber++;

                                string tr_line = "<tbody>" + "<tr><td>" + rownumber + "</td><td>" + model.lot_no + "</td><td>" + model.disp_lot_date + "</td><td>" + model.sanc_amt + "</td></tr>";
                                sb.AppendLine(tr_line);
                                total_amount += model.sanc_amt;
                            }

                        }
                        else if (rownumber == Remplist.Count)
                        {
                            string tr_line = "<tr><td></td><td></td><td>Total</td><td>" + total_amount + "</td></tr>";
                            sb.AppendLine(tr_line);
                            sb.AppendLine(line);
                            break;
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

        public byte[] GetTransOfficeNoteHtmlstring(string lotno)
        {
            DataTable dt = new DataTable();
            var lotn = lotno;
            StringBuilder sb = new StringBuilder();
            using (NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString))
            {
                con.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("lotno", lotn);
                List<RetiredempTransModel> Remplist = con.Query<RetiredempTransModel>("select * from mdcl_sp_get_empl_trans_for_officenote(:lotno)", parameters).ToList();

                using (StreamReader sr = new StreamReader(System.Configuration.ConfigurationManager.AppSettings["OfficeNotehtml"].ToString()))
                {

                    int rownumber = 0;
                    decimal total_amount = 0;
                    // string str = total_amount.ToString();
                    String line;

                    total_amount = Remplist.Select(x => x.sanc_amt).Sum();
                    //Added by Amit
                    string lotDate = string.Empty;
                    if (Remplist.Count > 0)
                    {
                        if (Remplist[0].lot_date != null)
                        {
                            lotDate = Remplist[0].lot_date.ToString("dd-MMM-yyyy");
                        }
                    }
                    //end

                    while ((line = sr.ReadLine()) != null)
                    {
                        line = line.Replace("#listDate", lotDate);
                        line = line.Replace("#lot_no", lotn);
                        line = line.Replace("#Amount", total_amount.ToString("##,##,###.00"));

                        if ((line.Trim() == ("<tbody>") && rownumber <= dt.Rows.Count))
                        {


                            foreach (RetiredempTransModel model in Remplist)
                            {
                                rownumber++;

                                string tr_line = "<tbody>" + "<tr><td>" + rownumber + "</td><td>" + model.yearfrom + "</td><td>" + model.yearto + "</td><td>" + model.sanc_amt.ToString("##,##,###.00") + "</td></tr>";
                                sb.AppendLine(tr_line);
                                // total_amount += model.sanc_amt;
                            }

                        }
                        else if (rownumber == Remplist.Count)
                        {
                            string tr_line = "<tr><td></td><td></td><td>Total</td><td>" + total_amount.ToString("##,##,###.00") + "</td></tr>";
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

        public byte[] GetTransBankAdviseHtmlstring(string lotno)
        {
            DataTable dt = new DataTable();
            var lotn = lotno;
            StringBuilder sb = new StringBuilder();
            using (NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString))
            {
                con.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("lotno", lotn);
                RetiredempTransModel Remplist = con.Query<RetiredempTransModel>("select * from mdcl_sp_get_empl_trans_for_bankadvise(:lotno)", parameters).FirstOrDefault();

                using (StreamReader sr = new StreamReader(System.Configuration.ConfigurationManager.AppSettings["BankAdvisehtml"].ToString()))
                {

                    //Added by Amit
                    string lotDate = string.Empty;

                    if (Remplist.lot_date != null)
                    {
                        lotDate = Remplist.lot_date.ToString("dd-MMM-yyyy");
                    }
                    //end


                    String line;
                    decimal total_amount = Remplist.sanc_amt;
                    string str = total_amount.ToString();
                    while ((line = sr.ReadLine()) != null)
                    {
                        line = line.Replace("#listDate", lotDate);
                        line = line.Replace("#lot_no", lotn);

                        line = line.Replace("#Amount", str);

                        sb.AppendLine(line);
                    }
                }


            }
            return CommonHelper.Convert2(sb.ToString());
        }
        public List<DropdownModel> GetYear()
        {
            NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString);
            List<DropdownModel> yearlist = new List<DropdownModel>();
            DataTable dt = new DataTable();

            try
            {
                using (con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {
                    con.Open();
                    using (NpgsqlCommand cmd = new NpgsqlCommand("select * from  mdcl_sp_get_tofill_years()", con))
                    {
                        cmd.Connection = con;

                        using (NpgsqlDataAdapter SqDA = new NpgsqlDataAdapter(cmd))
                        {
                            SqDA.Fill(dt);
                        }
                    }

                    if (dt.Rows.Count == 0)
                    {
                        yearlist = new List<DropdownModel>();
                    }
                    else
                    {
                        foreach (DataRow drow in dt.Rows)
                        {
                            DropdownModel expobj = new DropdownModel();
                            expobj.Id = string.IsNullOrWhiteSpace(drow["id"].ToString()) ? 0 : Convert.ToInt32(drow["id"].ToString());
                            expobj.Name = string.IsNullOrWhiteSpace(drow["name"].ToString()) ? "" : drow["name"].ToString();
                            yearlist.Add(expobj);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                yearlist = new List<DropdownModel>();
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return yearlist;
        }
        public List<RetiredempTransModel> GetyearwiseReport(RetiredempTransModel objmodel)
        {
            List<RetiredempTransModel> Remplist = new List<RetiredempTransModel>();
            decimal yearfrom = objmodel.year;

            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {
                    con.Open();
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("pyear", yearfrom);

                    Remplist = con.Query<RetiredempTransModel>("select * from mdcl_sp_get_yearwise_report(:pyear)", parameters).ToList();

                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                Remplist = new List<RetiredempTransModel>();
            }
            return Remplist;

        }
    }
}