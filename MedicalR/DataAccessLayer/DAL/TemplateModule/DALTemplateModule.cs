using MedicalR.CustomHelper;
using MedicalR.DataAccessLayer.IDAL.TemplateModule;
using MedicalR.Models;
using MedicalR.Models.TemplateModule;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MedicalR.DataAccessLayer.DAL.TemplateModule
{
    public class DALTemplateModule : IDALTemplateModule
    {

        #region get template list
        public List<TemplateModel> GetTemplateList()
        {
            List<TemplateModel> TemplateList = new List<TemplateModel>();          
            NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString);
            bool status = true;
            DataTable dt = new DataTable();
            try
            {
                using (con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {
                    con.Open();
                    using (NpgsqlCommand cmd = new NpgsqlCommand("select * from  mdcl_sp_get_templatelist(:pstatus)", con))
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
                        TemplateList = new List<TemplateModel>();
                    }
                    else
                    {
                        foreach (DataRow drow in dt.Rows)
                        {
                            TemplateModel Template = new TemplateModel();

                            Template.templateID = string.IsNullOrWhiteSpace(drow["templateid"].ToString()) ? 0 : Convert.ToInt32(drow["templateid"].ToString());
                            Template.templateTitle = string.IsNullOrWhiteSpace(drow["templatetitle"].ToString()) ? "" : drow["templatetitle"].ToString();
                            Template.templateDescription = string.IsNullOrWhiteSpace(drow["templatedesc"].ToString()) ? "" : drow["templatedesc"].ToString();
                            Template.templatePublish = string.IsNullOrWhiteSpace(drow["templatepublish"].ToString()) ? false : Convert.ToBoolean(drow["templatepublish"].ToString());                           
                            Template.templatePublishDate = Convert.ToDateTime(drow["templatepublishdate"].ToString());
                            Template.templateLastModified = Convert.ToDateTime(drow["templatelastmodify"].ToString());
                            TemplateList.Add(Template);
                          
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                TemplateList = new List<TemplateModel>();
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return TemplateList;
        }

        #endregion region


        #region get single template date
        public TemplateModel GetSingleTemplateDetails(TemplateModel objModel)
        {
            TemplateModel TemplateList = new TemplateModel();
            TemplateModel Template = new TemplateModel();
            NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString);
            
            DataTable dt = new DataTable();
            try
            {
                using (con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {
                    con.Open();
                    using (NpgsqlCommand cmd = new NpgsqlCommand("select * from  mdcl_sp_get_singletemplatedetails(:ptemplateid)", con))
                    {
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("ptemplateid", objModel.templateID);
                        using (NpgsqlDataAdapter SqDA = new NpgsqlDataAdapter(cmd))
                        {
                            SqDA.Fill(dt);
                        }
                    }

                    if (dt.Rows.Count == 0)
                    {
                        Template =new TemplateModel();
                    }
                    else
                    {
                        foreach (DataRow drow in dt.Rows)
                        {
                            
                            Template.templateID = string.IsNullOrWhiteSpace(drow["templateid"].ToString()) ? 0 : Convert.ToInt32(drow["templateid"].ToString());
                            Template.templateTitle = string.IsNullOrWhiteSpace(drow["templatetitle"].ToString()) ? "" : drow["templatetitle"].ToString();
                            Template.templateDescription = string.IsNullOrWhiteSpace(drow["templatedesc"].ToString()) ? "" : drow["templatedesc"].ToString();
                            Template.templatePublish = string.IsNullOrWhiteSpace(drow["templatepublish"].ToString()) ? false : Convert.ToBoolean(drow["templatepublish"].ToString());
                            Template.templatePublishDate = Convert.ToDateTime(drow["templatepublishdate"].ToString());
                            Template.templateLastModified = Convert.ToDateTime(drow["templatelastmodify"].ToString());
                          
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                Template = new TemplateModel();
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return Template;
           
        }

        #endregion region


        #region add Template
        public ResponseModel AddTemplate(TemplateModel objModel)
        {
            ResponseModel Response = new ResponseModel();
            NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString);
            var CurrentUtcDate = CommonHelper.GetDate;
            var EmployeeId = UserManager.User.UserID;
            bool status = true;
            try
            {
                using (con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {

                    con.Open();
                    using (NpgsqlCommand cmd = new NpgsqlCommand("select * from  mdcl_sp_addtemplates(:ptemplatetitle,:ptemplatedesc,:ptemplatepublish,:ptemplatepublishdate,:ptemplatelastmodify,:pstatus,:pcreatedby)", con))
                    {
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("ptemplatetitle", objModel.templateTitle);
                        cmd.Parameters.AddWithValue("ptemplatedesc", objModel.templateDescription);
                        cmd.Parameters.AddWithValue("ptemplatepublish", objModel.templatePublish);
                        cmd.Parameters.AddWithValue("ptemplatepublishdate", CurrentUtcDate);
                        cmd.Parameters.AddWithValue("ptemplatelastmodify", CurrentUtcDate);
                        cmd.Parameters.AddWithValue("pstatus", status);
                        cmd.Parameters.AddWithValue("pcreatedby", Convert.ToString(EmployeeId));

                        object Res = (cmd.ExecuteScalar());
                        int Res2 = Convert.ToInt32(Res);
                        if (Res2 == 0)
                        {
                            Response.Status = false;
                            Response.Message = MessageHelper.ErroeMsg;
                        }
                        else
                        {
                            Response.Status = true;
                            Response.ID = Res2;
                            Response.Message = MessageHelper.TemplateAdded;
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
        #endregion

        #region Upadte Template
        public ResponseModel UpdateTemplate(TemplateModel objModel)
        {
            NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString);
            ResponseModel Response = new ResponseModel();
                   
            var EmployeeId = UserManager.User.UserID;
            var CurrentUtcDate = CommonHelper.GetDate;
            try
            {
                using (con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {

                    con.Open();
                    using (NpgsqlCommand cmd = new NpgsqlCommand("select * from  mdcl_sp_updatetemplates(:ptemplateid,:ptemplatetitle,:ptemplatedesc,:ptemplatepublish,:ptemplatepublishdate,:ptemplatelastmodify)", con))
                    {
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("ptemplateid", objModel.templateID);
                        cmd.Parameters.AddWithValue("ptemplatetitle", objModel.templateTitle);
                        cmd.Parameters.AddWithValue("ptemplatedesc", objModel.templateDescription);
                        cmd.Parameters.AddWithValue("ptemplatepublish", objModel.templatePublish);
                        cmd.Parameters.AddWithValue("ptemplatepublishdate", CurrentUtcDate);
                        cmd.Parameters.AddWithValue("ptemplatelastmodify", CurrentUtcDate);
                        //cmd.Parameters.AddWithValue("pstatus", status);
                        // cmd.Parameters.AddWithValue("pcreatedby", Convert.ToString(EmployeeId));

                        object Res = (cmd.ExecuteScalar());
                        int Res2 = Convert.ToInt32(Res);
                        if (Res2 == 0)
                        {
                            Response.Status = false;
                            Response.Message = MessageHelper.ErroeMsg;
                        }
                        else
                        {
                            Response.Status = true;
                            Response.ID = Res2;
                            Response.Message = MessageHelper.TemplateUdated;
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
        #endregion
    }
}