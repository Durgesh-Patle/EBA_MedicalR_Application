using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Dapper;
using MedicalR.CustomHelper;
using MedicalR.DataAccessLayer.IDAL.MedicalR;
using MedicalR.Models.MedicalR;
using Npgsql;

namespace MedicalR.DataAccessLayer.DAL.MedicalR
{
    public class DALAcknowledge : IDALAcknowledge
    {
        public List<MedicalAcknowledgeModel> GetAcknowledgeGridData()
        {
            List<MedicalAcknowledgeModel> ack_list = new List<MedicalAcknowledgeModel>();
            DataTable dt = new DataTable();
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(CustomHelper.CommonHelper.GetConnectionString))
                {
                    con.Open();
                    ack_list = con.Query<MedicalAcknowledgeModel>("select * from mdcl_sp_get_acknowledge_grid_data()").ToList();
                }
            }
            catch (Exception ex)
            {

            }
            return ack_list;
        }
        public string AcknowledgeStart(List<MedicalAcknowledgeModel> model_list)
        {
            Npgsql.NpgsqlTransaction trans = null; ;

            using (NpgsqlConnection con = new NpgsqlConnection(CustomHelper.CommonHelper.GetConnectionString))
            {
                try
                {
                    con.Open();
                    trans = con.BeginTransaction();
                    foreach (MedicalAcknowledgeModel model in model_list)
                    {
                        if (model.is_acknowledged)
                        {
                            DynamicParameters parameters = new DynamicParameters();
                            parameters.Add("p_claim_request_id", model.id);
                            parameters.Add("p_acknowledged_by_id", UserManager.User.UserID);
                            con.Query<ResponseData>("select * from mdcl_sp_acknowledge_done(:p_claim_request_id,:p_acknowledged_by_id)", parameters, trans).FirstOrDefault();

                            DynamicParameters para = new DynamicParameters();
                            para.Add("p_employeeid", UserManager.User.UserID);
                            para.Add("p_claim_request_id", model.id);
                            para.Add("p_status_code", "RECBYDOAA");
                            con.Query("select * from mdcl_sp_insert_empl_activities(:p_employeeid,:p_claim_request_id,:p_status_code)", para, trans);
                        }
                    }
                    trans.Commit();
                    return MessageHelper.AcknowledgeStatus;
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    return MessageHelper.ErroeMsg;
                }
            }

        }
    }
}