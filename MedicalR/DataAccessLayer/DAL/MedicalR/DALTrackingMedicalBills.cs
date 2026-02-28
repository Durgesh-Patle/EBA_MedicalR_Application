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
    public class DALTrackingMedicalBills : IDALTrackingMedicalBills
    {
        public List<TrackingMedicalBillsModel> GetTrackingBills_GridData()
        {

            List<TrackingMedicalBillsModel> Addlist = new List<TrackingMedicalBillsModel>();
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {
                    con.Open();
                    Addlist = con.Query<TrackingMedicalBillsModel>("select * from mdcl_sp_get_tracking_medical_bills()").ToList();
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                Addlist = new List<TrackingMedicalBillsModel>();
            }
            return Addlist;


        }
        public TrackingMedicalBillsModel GetSingleMedicalBill(TrackingMedicalBillsModel objModel)
        {
            NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString);
            List<EmpStatusmodel> Status_list = new List<EmpStatusmodel>();
            TrackingMedicalBillsModel RempDetails = new TrackingMedicalBillsModel();
            DataTable dt = new DataTable();
            int id;
            try
            {
                // NpgsqlConnection con = new NpgsqlConnection();
                id = objModel.id;

                using (con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {

                    con.Open();

                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("pid", objModel.id);
                    RempDetails = con.Query<TrackingMedicalBillsModel>("select * from mdcl_sp_get_single_tracking_medical_bills(:pid)", parameters).FirstOrDefault();
                    Status_list = con.Query<EmpStatusmodel>("select * from mdcl_sp_get_single_tracking_medical_bills_status(:pid)", parameters).ToList();
                    // int k = 0;
                    foreach (EmpStatusmodel ex_ty_details in Status_list)
                    {
                        if (RempDetails.EmplStatus_list == null)
                        {
                            RempDetails.EmplStatus_list = new List<EmpStatusmodel>();
                        }
                        RempDetails.EmplStatus_list.Add(ex_ty_details);

                        // k++;
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
    }
}