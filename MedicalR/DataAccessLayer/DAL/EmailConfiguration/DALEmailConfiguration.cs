using MedicalR.CustomHelper;
using MedicalR.DataAccessLayer.IDAL.EmailConfiguration;
using MedicalR.Models;
using MedicalR.Models.EmailConfiguration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using static MedicalR.CustomHelper.EmailHelper;

namespace MedicalR.DataAccessLayer.DAL.EmailConfiguration
{
    public class DALEmailConfiguration : IDALEmailConfiguration
    {
        #region get company email configuration list
        public List<EmailConfigurationModel> GetEmailConfigurationList()
        {
            List<EmailConfigurationModel> EmailConfigurationList = new List<EmailConfigurationModel>();
            SqlConnection con = new SqlConnection(CommonHelper.GetEmailConnectionString);
            try
            {
                var CompanyID = UserManager.User.CompanyID;
                var ProjectCode = CommonHelper.GetProjectID;  
                SqlCommand cmd = new SqlCommand("sproc_GettblUserEmailSettings", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CompanyID", CompanyID);
                cmd.Parameters.AddWithValue("@ProjectCode", ProjectCode);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                DataSet ds = new DataSet();
                da.Fill(ds);

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    EmailConfigurationModel EmailConfiguration = new EmailConfigurationModel();
                    EmailConfiguration.UserEmailSettingsID = Convert.ToInt32(ds.Tables[0].Rows[i]["UserEmailSettingsID"].ToString());
                    EmailConfiguration.UserID = Convert.ToInt32(ds.Tables[0].Rows[i]["CompanyUserID"].ToString());
                    EmailConfiguration.AccountType = ds.Tables[0].Rows[i]["AccountType"].ToString();
                    EmailConfiguration.CompanyName = ds.Tables[0].Rows[i]["CompanyName"].ToString();
                    EmailConfiguration.Email = ds.Tables[0].Rows[i]["Email"].ToString();
                    EmailConfiguration.IsActive = Convert.ToBoolean(ds.Tables[0].Rows[i]["IsActive"].ToString());
                    EmailConfiguration.EmailPassword = ds.Tables[0].Rows[i]["EmailPassword"].ToString();
                    EmailConfiguration.Name = ds.Tables[0].Rows[i]["Name"].ToString();
                    EmailConfiguration.Port = Convert.ToInt32(ds.Tables[0].Rows[i]["Port"].ToString());
                    EmailConfiguration.ProjectCode = ds.Tables[0].Rows[i]["ProjectCode"].ToString();
                    EmailConfiguration.Server = ds.Tables[0].Rows[i]["Server"].ToString();
                    EmailConfigurationList.Add(EmailConfiguration);
                }

            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                EmailConfigurationList = new List<EmailConfigurationModel>();
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return EmailConfigurationList;
        }
        #endregion



        #region get single company email configuration detail
        public EmailConfigurationModel GetSingleEmailConfigurationDetails(EmailConfigurationModel objModel)
        {
            EmailConfigurationModel EmailConfigurationDetails = new EmailConfigurationModel();
            SqlConnection con = new SqlConnection(CommonHelper.GetEmailConnectionString);
            try
            {
                var CompanyID = UserManager.User.CompanyID;
                var ProjectCode = CommonHelper.GetProjectID; 
                SqlCommand cmd = new SqlCommand("sproc_GetSingletblUserEmailSettings", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CompanyID", CompanyID);
                cmd.Parameters.AddWithValue("@ProjectCode", ProjectCode);
                cmd.Parameters.AddWithValue("@CompanyUserID", objModel.UserID);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                DataSet ds = new DataSet();
                da.Fill(ds);
                if (ds.Tables[0].Rows.Count == 0)
                {
                    EmailConfigurationDetails = null;
                    if(UserManager.User.UserID == objModel.UserID)
                    {
                        EmailConfigurationDetails = new EmailConfigurationModel();
                        EmailConfigurationDetails.UserID = UserManager.User.UserID;
                    }
                }
                else
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        EmailConfigurationDetails.UserEmailSettingsID = Convert.ToInt32(ds.Tables[0].Rows[i]["UserEmailSettingsID"].ToString());
                        EmailConfigurationDetails.UserID = Convert.ToInt32(ds.Tables[0].Rows[i]["CompanyUserID"].ToString());
                        EmailConfigurationDetails.AccountType = ds.Tables[0].Rows[i]["AccountType"].ToString();
                        EmailConfigurationDetails.CompanyName = ds.Tables[0].Rows[i]["CompanyName"].ToString();
                        EmailConfigurationDetails.Email = ds.Tables[0].Rows[i]["Email"].ToString();
                        EmailConfigurationDetails.IsActive = Convert.ToBoolean(ds.Tables[0].Rows[i]["IsActive"].ToString());
                        EmailConfigurationDetails.EmailPassword = ds.Tables[0].Rows[i]["EmailPassword"].ToString();
                        EmailConfigurationDetails.Name = ds.Tables[0].Rows[i]["Name"].ToString();
                        EmailConfigurationDetails.Port = Convert.ToInt32(ds.Tables[0].Rows[i]["Port"].ToString());
                        EmailConfigurationDetails.ProjectCode = ds.Tables[0].Rows[i]["ProjectCode"].ToString();
                        EmailConfigurationDetails.Server = ds.Tables[0].Rows[i]["Server"].ToString();
                    }

                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                EmailConfigurationDetails = null;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return EmailConfigurationDetails;
        }
        #endregion

        #region add email configuration
        public ResponseModel AddEmailConfiguration(EmailConfigurationModel objModel)
        {
            ResponseModel Response = new ResponseModel();
            SqlConnection con = new SqlConnection(CommonHelper.GetEmailConnectionString);
            try
            {
                var CurrentUtcDate = CommonHelper.GetDate;
                var CompanyUserID = UserManager.User.UserID;
                var CompanyID = UserManager.User.CompanyID;
                var CompanyName = UserManager.User.CompanyName;
                var ProjectCode = CommonHelper.GetProjectID;
                var ServiceUser = CommonHelper.GetEmailServiceUser;
                var ServicePassword = CommonHelper.GetEmailServicePassword;
                SqlCommand cmd = new SqlCommand("sproc_InserttblUserEmailSetting", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProjectCode", ProjectCode);
                cmd.Parameters.AddWithValue("@CompanyName", CompanyName);
                cmd.Parameters.AddWithValue("@AccountType", objModel.AccountType);
                cmd.Parameters.AddWithValue("@Name", objModel.Name);
                cmd.Parameters.AddWithValue("@Email", objModel.Email);
                cmd.Parameters.AddWithValue("@EmailPassword", objModel.EmailPassword);
                cmd.Parameters.AddWithValue("@Server", objModel.Server);
                cmd.Parameters.AddWithValue("@Port", objModel.Port);
                cmd.Parameters.AddWithValue("@CreatedbyID", CompanyUserID);
                cmd.Parameters.AddWithValue("@CreatedDate", CurrentUtcDate);
                cmd.Parameters.AddWithValue("@CompanyID", CompanyID);
                cmd.Parameters.AddWithValue("@CompanyUserID", objModel.UserID);
                cmd.Parameters.AddWithValue("@IsActive", true);
                cmd.Parameters.AddWithValue("@ServiceUser", ServiceUser);
                cmd.Parameters.AddWithValue("@ServicePassword", ServicePassword);
                cmd.Parameters.AddWithValue("@UserEmailSettingsID", SqlDbType.Int).Direction = ParameterDirection.Output;
                con.Open();
                cmd.ExecuteNonQuery();
                var EmailSettingsID = Convert.ToInt32(cmd.Parameters["@UserEmailSettingsID"].Value.ToString());
                if (EmailSettingsID == 0)
                {
                    Response.Status = false;
                    Response.Message = MessageHelper.EmailConfigurationExists;
                }
                else
                {
                    Response.Status = true;
                    Response.Message = MessageHelper.EmailConfigurationAdded;

                    var EmailResult = SendTestEmail();
                    if (EmailResult == false)
                    {
                        Response.Status = false;
                        Response.Message = MessageHelper.EmailConfigurationTestFail;
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
        #endregion

        #region update email configuration
        public ResponseModel UpdateEmailConfiguration(EmailConfigurationModel objModel)
        {
            ResponseModel Response = new ResponseModel();
            SqlConnection con = new SqlConnection(CommonHelper.GetEmailConnectionString);
            try
            {

                var CompanyID = UserManager.User.CompanyID;
                var ProjectCode = CommonHelper.GetProjectID;
                SqlCommand cmd = new SqlCommand("sproc_UpdatetblUserEmailSetting", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProjectCode", ProjectCode);
                cmd.Parameters.AddWithValue("@AccountType", objModel.AccountType);
                cmd.Parameters.AddWithValue("@Name", objModel.Name);
                cmd.Parameters.AddWithValue("@Email", objModel.Email);
                cmd.Parameters.AddWithValue("@EmailPassword", objModel.EmailPassword);
                cmd.Parameters.AddWithValue("@Server", objModel.Server);
                cmd.Parameters.AddWithValue("@Port", objModel.Port);
                cmd.Parameters.AddWithValue("@CompanyID", CompanyID);
                cmd.Parameters.AddWithValue("@CompanyUserID", objModel.UserID);
                cmd.Parameters.AddWithValue("@UserEmailSettingsID", objModel.UserEmailSettingsID);
                con.Open();
                cmd.ExecuteNonQuery();
                Response.Status = true;
                Response.Message = MessageHelper.EmailConfigurationUpdated;
                var EmailResult = SendTestEmail();
                if(EmailResult==false)
                {
                    Response.Status = false;
                    Response.Message = MessageHelper.EmailConfigurationTestFail;
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

        #region update email configuration status
        public ResponseModel UpdateEmailConfigurationStatus(EmailConfigurationModel objModel)
        {
            ResponseModel Response = new ResponseModel();
            SqlConnection con = new SqlConnection(CommonHelper.GetEmailConnectionString);
            try
            {

                var CompanyID = UserManager.User.CompanyID;
                var ProjectCode = CommonHelper.GetProjectID;
                SqlCommand cmd = new SqlCommand("sproc_UpdatetblUserEmailSettingStatus", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProjectCode", ProjectCode);
                cmd.Parameters.AddWithValue("@CompanyID", CompanyID);
                cmd.Parameters.AddWithValue("@IsActive", objModel.IsActive);
                cmd.Parameters.AddWithValue("@UserEmailSettingsID", objModel.UserEmailSettingsID);
                con.Open();
                cmd.ExecuteNonQuery();
                Response.Status = true;
                Response.Message = MessageHelper.EmailConfigurationStatusUpdated;
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


        #region send test email
        public bool SendTestEmail()
        {
            var result = true;
            try
            {
              
                EmailConfigurationModel objEmailConfigModel = new EmailConfigurationModel();
                objEmailConfigModel.UserID = UserManager.User.UserID;
                var emailConfiguration = GetSingleEmailConfigurationDetails(objEmailConfigModel);
                if (emailConfiguration != null)
                {
                    EmailModel parameters = new EmailModel();
                    parameters.emailConfigName = emailConfiguration.Name;
                    parameters.emailConfigAccountType = emailConfiguration.AccountType;
                    parameters.emailConfigEmailID = emailConfiguration.Email;
                    parameters.emailConfigEmailPassword = emailConfiguration.EmailPassword;
                    parameters.emailConfigPort = emailConfiguration.Port;
                    parameters.emailConfigServer = emailConfiguration.Server;
                    parameters.Body = "Test Email";
                    parameters.toEmailID = new List<string>() { emailConfiguration.Email };                   
                    parameters.Subject = "Test Email";
                    result = EmailHelper.SendEmail(parameters);           
                }
                
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                result = false;
            }
            return result;
        }
        #endregion
    }
}