using Dapper;
using MedicalR.CustomHelper;
using MedicalR.DataAccessLayer.IDAL.MedicalR;
using MedicalR.Models.MedicalR;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedicalR.DataAccessLayer.DAL.MedicalR
{
    public class DALMedicalPaymentDetails: IDALMedicalPaymentDetails
    {
        public List<MedicalPaymentDetailsModel> GetPaymentDetail_GridData()
        {

            List<MedicalPaymentDetailsModel> Addlist = new List<MedicalPaymentDetailsModel>();
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {
                    con.Open();
                    Addlist = con.Query<MedicalPaymentDetailsModel>("select * from mdcl_sp_get_medical_payment_detail_grid_data()").ToList();
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                Addlist = new List<MedicalPaymentDetailsModel>();
            }
            return Addlist;


        }
    }
}