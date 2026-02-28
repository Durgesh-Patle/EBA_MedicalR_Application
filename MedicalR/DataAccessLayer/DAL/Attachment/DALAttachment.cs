using MedicalR.CustomHelper;
using MedicalR.DataAccessLayer.IDAL.Attachment;
using MedicalR.Models;
using MedicalR.Models.Attachment;
using MedicalR.Models.AttachmentTypeModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MedicalR.DataAccessLayer.DAL.Attachment
{
    public class DALAttachment: IDALAttachment
    {
        #region get attachment list
        public List<AttachmentTypeModel> GetAttachmentTypeList(int PageID = 0)
        {
            List<AttachmentTypeModel> AttachmentList = new List<AttachmentTypeModel>();
            SqlConnection con = new SqlConnection(CommonHelper.GetConnectionString);
            try
            {
                var CompanyID = UserManager.User.CompanyID;
                SqlCommand cmd = new SqlCommand("sproc_GettblMDocumentMaster", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PageID", PageID);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                DataSet ds = new DataSet();
                da.Fill(ds);

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    AttachmentTypeModel Attachment = new AttachmentTypeModel();
                    Attachment.DocumentID = Convert.ToInt32(ds.Tables[0].Rows[i]["DocumentID"].ToString());
                    Attachment.DocumentType = ds.Tables[0].Rows[i]["DocumentType"].ToString();
                    AttachmentList.Add(Attachment);
                }

            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                AttachmentList = new List<AttachmentTypeModel>();
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return AttachmentList;
        }
        #endregion

        #region get attachemnt list
        public List<AttachmentModel> GetAttachmentList(Int32 PageID, Int32 ReleventID, Int32? AttachmentID = null)
        {
            List<AttachmentModel> AttachmentList = new List<AttachmentModel>();
            SqlConnection con = new SqlConnection(CommonHelper.GetConnectionString);
            try
            {
                var CompanyID = UserManager.User.CompanyID;
                SqlCommand cmd = new SqlCommand("sproc_GettblAttachmentdetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PageID", PageID);
                cmd.Parameters.AddWithValue("@RelevenID", ReleventID);
                cmd.Parameters.AddWithValue("@CompanyID", CompanyID);
                cmd.Parameters.AddWithValue("@AttachementID", AttachmentID);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                DataSet ds = new DataSet();
                da.Fill(ds);

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    AttachmentModel Attachment = new AttachmentModel();
                    Attachment.AttachementID = Convert.ToInt32(ds.Tables[0].Rows[i]["AttachementID"].ToString());
                    Attachment.GoogleID = Convert.ToInt32(ds.Tables[0].Rows[i]["GoogleID"].ToString());
                    Attachment.DocumentID = Convert.ToInt32(ds.Tables[0].Rows[i]["DocumentID"].ToString());
                    Attachment.PageID = Convert.ToInt32(ds.Tables[0].Rows[i]["PageID"].ToString());
                    Attachment.RelevantID = Convert.ToInt32(ds.Tables[0].Rows[i]["RelevantID"].ToString());               
                    Attachment.CreatedDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["CreatedDate"].ToString()).ToString();
                    Attachment.AttachmentName = ds.Tables[0].Rows[i]["AttachmentName"].ToString();
                    Attachment.DocumnentType = ds.Tables[0].Rows[i]["DocumentType"].ToString();
                    Attachment.UserName = ds.Tables[0].Rows[i]["UserName"].ToString();
                    AttachmentList.Add(Attachment);
                }

            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                AttachmentList = new List<AttachmentModel>();
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return AttachmentList;
        }
        #endregion

        #region add attachemnt
        public ResponseModel AddAttachment(AttachmentModel objModel)
        {
            ResponseModel Response = new ResponseModel();
            SqlConnection con = new SqlConnection(CommonHelper.GetConnectionString);
            try
            {
                var CurrentUtcDate = CommonHelper.GetDate;
                var CompanyUserID = UserManager.User.UserID;
                var CompanyID = UserManager.User.CompanyID;
                SqlCommand cmd = new SqlCommand("sproc_InserttblAttachment", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@GoogleID", objModel.GoogleID);
                cmd.Parameters.AddWithValue("@DocumentID", objModel.DocumentID);
                cmd.Parameters.AddWithValue("@PageID", objModel.PageID);
                cmd.Parameters.AddWithValue("@RelevantID", objModel.RelevantID);
                cmd.Parameters.AddWithValue("@CompanyUserID", CompanyUserID);
                cmd.Parameters.AddWithValue("@CompanyID", CompanyID);
                cmd.Parameters.AddWithValue("@CreatedDate", CurrentUtcDate);
                cmd.Parameters.AddWithValue("@AttachmentName", objModel.AttachmentName);
                cmd.Parameters.AddWithValue("@AttachmentPath", objModel.AttachmentPath);
                con.Open();
                cmd.ExecuteNonQuery();
                Response.Status = true;
                Response.Message = MessageHelper.AttachmentAdded;
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

        #region delete attachment
        public ResponseModel DeleteAttachment(Int32 AttachmentID, Int32 PageID)
        {
            ResponseModel Response = new ResponseModel();
            SqlConnection con = new SqlConnection(CommonHelper.GetConnectionString);
            try
            {
                var CurrentUtcDate = CommonHelper.GetDate;
                var CompanyUserID = UserManager.User.UserID;
                var CompanyID = UserManager.User.CompanyID;
                SqlCommand cmd = new SqlCommand("sproc_DeletetblAttachmentdetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@AttachementID", AttachmentID);
                cmd.Parameters.AddWithValue("@PageID", PageID);
                cmd.Parameters.AddWithValue("@CompanyID", CompanyID);
                con.Open();
                cmd.ExecuteNonQuery();
                Response.Status = true;
                Response.Message = MessageHelper.AttachmentDeleted;
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