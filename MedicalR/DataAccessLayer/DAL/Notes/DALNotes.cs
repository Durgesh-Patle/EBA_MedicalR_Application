using MedicalR.CustomHelper;
using MedicalR.DataAccessLayer.IDAL.Notes;
using MedicalR.Models;
using MedicalR.Models.Notes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MedicalR.DataAccessLayer.DAL.Notes
{
    public class DALNotes: IDALNotes
    {
        #region get notes list
        public List<NotesModel> GetNotesList(Int32 PageID, Int32 ReleventID)
        {
            List<NotesModel> NotesList = new List<NotesModel>();
            SqlConnection con = new SqlConnection(CommonHelper.GetConnectionString);
            try
            {
                var CompanyID = UserManager.User.CompanyID;
                SqlCommand cmd = new SqlCommand("sproc_GettblNotesdetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PageID", PageID);
                cmd.Parameters.AddWithValue("@RelevenID", ReleventID);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                DataSet ds = new DataSet();
                da.Fill(ds);

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    NotesModel Note = new NotesModel();
                    Note.NoteID = Convert.ToInt32(ds.Tables[0].Rows[i]["NoteID"].ToString());
                    Note.PageID = Convert.ToInt32(ds.Tables[0].Rows[i]["PageID"].ToString());
                    Note.RelevantID = Convert.ToInt32(ds.Tables[0].Rows[i]["RelevantID"].ToString());
                    Note.CreatedDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["CreatedDate"].ToString()).ToString();
                    Note.Comment = ds.Tables[0].Rows[i]["Comment"].ToString();
                    Note.UserName = ds.Tables[0].Rows[i]["UserName"].ToString();
                    NotesList.Add(Note);
                }

            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                NotesList = new List<NotesModel>();
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return NotesList;
        }
        #endregion

        #region add notes
        public ResponseModel AddNotes(NotesModel objModel)
        {
            ResponseModel Response = new ResponseModel();
            SqlConnection con = new SqlConnection(CommonHelper.GetConnectionString);
            try
            {
                var CurrentUtcDate = CommonHelper.GetDate;
                var CompanyUserID = UserManager.User.UserID;
                var CompanyID = UserManager.User.CompanyID;
                SqlCommand cmd = new SqlCommand("sproc_InserttblNotes", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PageID", objModel.PageID);
                cmd.Parameters.AddWithValue("@RelevantID", objModel.RelevantID);
                cmd.Parameters.AddWithValue("@CompanyUserID", CompanyUserID);
                cmd.Parameters.AddWithValue("@CompanyID", CompanyID);
                cmd.Parameters.AddWithValue("@CreatedDate", CurrentUtcDate);
                cmd.Parameters.AddWithValue("@Comment", objModel.Comment);
                con.Open();
                cmd.ExecuteNonQuery();
                Response.Status = true;
                Response.Message = MessageHelper.NoteAdded;
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


        #region update notes
        public ResponseModel UpdateNotes(NotesModel objModel)
        {
            ResponseModel Response = new ResponseModel();
            SqlConnection con = new SqlConnection(CommonHelper.GetConnectionString);
            try
            {
                var CurrentUtcDate = CommonHelper.GetDate;
                var CompanyUserID = UserManager.User.UserID;
                var CompanyID = UserManager.User.CompanyID;
                SqlCommand cmd = new SqlCommand("sproc_UpdatetblNotesDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PageID", objModel.PageID);
                cmd.Parameters.AddWithValue("@RelevantID", objModel.RelevantID);
                cmd.Parameters.AddWithValue("@CompanyID", CompanyID);
                cmd.Parameters.AddWithValue("@Comment", objModel.Comment);
                cmd.Parameters.AddWithValue("@NoteID", objModel.NoteID);
                con.Open();
                cmd.ExecuteNonQuery();
                Response.Status = true;
                Response.Message = MessageHelper.NoteUpdated;
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

        #region delete notes
        public ResponseModel DeleteNote(Int32 NoteID, Int32 PageID)
        {
            ResponseModel Response = new ResponseModel();
            SqlConnection con = new SqlConnection(CommonHelper.GetConnectionString);
            try
            {
                var CurrentUtcDate = CommonHelper.GetDate;
                var CompanyUserID = UserManager.User.UserID;
                var CompanyID = UserManager.User.CompanyID;
                SqlCommand cmd = new SqlCommand("sproc_DeletetblNotesdetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@NoteID", NoteID);
                cmd.Parameters.AddWithValue("@PageID", PageID);
                cmd.Parameters.AddWithValue("@CompanyID", CompanyID);
                con.Open();
                cmd.ExecuteNonQuery();
                Response.Status = true;
                Response.Message = MessageHelper.NoteDeleted;
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