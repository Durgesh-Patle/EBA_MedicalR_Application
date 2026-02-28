using MedicalR.CustomHelper;
using MedicalR.DataAccessLayer.IDAL.RoleManagement;
using MedicalR.Models;
using MedicalR.Models.RoleManagement;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MedicalR.DataAccessLayer.DAL.RoleManagement
{
    public class DALRoleManagement: IDALRoleManagement
    {

        #region get company role list
        public List<RoleViewModel> GetRoleList()
        {
            List<RoleViewModel> RoleList = new List<RoleViewModel>();
            SqlConnection con = new SqlConnection(CommonHelper.GetConnectionString);
            try
            {
                var CompanyID = UserManager.User.CompanyID;  
                SqlCommand cmd = new SqlCommand("sproc_GetRoleList", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CompanyID", CompanyID);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                DataSet ds = new DataSet();
                da.Fill(ds);

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    RoleViewModel Role = new RoleViewModel();
                    Role.RoleID = Convert.ToInt32(ds.Tables[0].Rows[i]["RoleID"].ToString());
                    Role.CompanyID = Convert.ToInt32(ds.Tables[0].Rows[i]["CompanyID"].ToString());
                    Role.RoleName = ds.Tables[0].Rows[i]["RoleName"].ToString();            
                    Role.RoleDescription = ds.Tables[0].Rows[i]["RoleDescription"].ToString();
                    Role.IsActive = Convert.ToBoolean(ds.Tables[0].Rows[i]["IsActive"].ToString());
                    RoleList.Add(Role);
                }
                
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                RoleList = new List<RoleViewModel>();
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return RoleList;
        }
        #endregion



        #region get single role detail
        public RoleViewModel GetSingleRoleDetails(RoleViewModel objModel)
        {
            RoleViewModel RoleDetails = new RoleViewModel();
            SqlConnection con = new SqlConnection(CommonHelper.GetConnectionString);
            try
            {
                var CompanyID = UserManager.User.CompanyID;
                SqlCommand cmd = new SqlCommand("sproc_GetSingleRole", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CompanyID", CompanyID);
                cmd.Parameters.AddWithValue("@RoleID", objModel.RoleID);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                DataSet ds = new DataSet();
                da.Fill(ds);
                if (ds.Tables[0].Rows.Count == 0)
                {
                    RoleDetails = null;
                }
                else
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        RoleDetails.RoleID = Convert.ToInt32(ds.Tables[0].Rows[i]["RoleID"].ToString());
                        RoleDetails.CompanyID = Convert.ToInt32(ds.Tables[0].Rows[i]["CompanyID"].ToString());
                        RoleDetails.RoleName = ds.Tables[0].Rows[i]["RoleName"].ToString();
                        RoleDetails.RoleDescription = ds.Tables[0].Rows[i]["RoleDescription"].ToString();
                        RoleDetails.IsActive = Convert.ToBoolean(ds.Tables[0].Rows[i]["IsActive"].ToString());
                        RoleDetails.RoleRights = ds.Tables[0].Rows[i]["RoleRights"] != DBNull.Value ? ds.Tables[0].Rows[i]["RoleRights"].ToString() : "";
                    }

                    if(!string.IsNullOrEmpty(RoleDetails.RoleRights))
                    {
                        RoleDetails.RoleRightDetails = JsonConvert.DeserializeObject<RoleRightDetailModel>(RoleDetails.RoleRights);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                RoleDetails = null;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return RoleDetails;
        }
        #endregion

        #region add company role
        public ResponseModel AddRole(RoleViewModel objModel)
        {
            ResponseModel Response = new ResponseModel();
            SqlConnection con = new SqlConnection(CommonHelper.GetConnectionString);
            try
            {    
                var CurrentUtcDate = CommonHelper.GetDate;
                var CompanyUserID = UserManager.User.UserID;
                var CompanyID = UserManager.User.CompanyID;
                SqlCommand cmd = new SqlCommand("sproc_InserttblRole", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@RoleName", objModel.RoleName);
                cmd.Parameters.AddWithValue("@RoleRights", objModel.RoleRights);
                cmd.Parameters.AddWithValue("@RoleDescription", objModel.RoleDescription);              
                cmd.Parameters.AddWithValue("@CompanyID", CompanyID);
                cmd.Parameters.AddWithValue("@CreatedDate", CurrentUtcDate);
                cmd.Parameters.AddWithValue("@CompanyUserID", CompanyUserID);
                cmd.Parameters.AddWithValue("@IsActive", true);
                cmd.Parameters.AddWithValue("@RoleID", SqlDbType.Int).Direction = ParameterDirection.Output;
                con.Open();
                cmd.ExecuteNonQuery();
                var RoleID = Convert.ToInt32(cmd.Parameters["@RoleID"].Value.ToString());
                Response.Status = true;
                Response.Message = MessageHelper.RoleAdded;
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

        #region update company role
        public ResponseModel UpdateRole(RoleViewModel objModel)
        {
            ResponseModel Response = new ResponseModel();
            SqlConnection con = new SqlConnection(CommonHelper.GetConnectionString);
            try
            {
                var CompanyID = UserManager.User.CompanyID;
                SqlCommand cmd = new SqlCommand("sproc_UpdatetblRole", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@RoleName", objModel.RoleName);
                cmd.Parameters.AddWithValue("@RoleRights", objModel.RoleRights);
                cmd.Parameters.AddWithValue("@RoleDescription", objModel.RoleDescription);
                cmd.Parameters.AddWithValue("@CompanyID", CompanyID);
                cmd.Parameters.AddWithValue("@RoleID", objModel.RoleID);
                con.Open();
                cmd.ExecuteNonQuery();
                Response.Status = true;
                Response.Message = MessageHelper.RoleUpdated;
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

        #region update company role status
        public ResponseModel UpdateRoleStatus(RoleViewModel objModel)
        {
            ResponseModel Response = new ResponseModel();
            SqlConnection con = new SqlConnection(CommonHelper.GetConnectionString);
            try
            {
                var CompanyID = UserManager.User.CompanyID;
                SqlCommand cmd = new SqlCommand("sproc_UpdatetblRoleStatus", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IsActive", objModel.IsActive);
                cmd.Parameters.AddWithValue("@CompanyID", CompanyID);
                cmd.Parameters.AddWithValue("@RoleID", objModel.RoleID);
                con.Open();
                cmd.ExecuteNonQuery();
                Response.Status = true;
                Response.Message = MessageHelper.RoleStatusUpdated;
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
    }
}