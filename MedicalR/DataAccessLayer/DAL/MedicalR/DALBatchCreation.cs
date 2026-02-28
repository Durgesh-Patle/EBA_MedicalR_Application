using Dapper;
using MedicalR.CustomHelper;
using MedicalR.DataAccessLayer.IDAL.MedicalR;
using MedicalR.Models.MedicalR;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Text;
using System.IO;

namespace MedicalR.DataAccessLayer.DAL.MedicalR
{
    public class DALBatchCreation : IDALBatchCreation
    {
        public string BatchCreationStart(List<BatchCreationModel> model_list)
        {
            StringBuilder sb = new StringBuilder();
            Npgsql.NpgsqlTransaction trans = null; ;
            List<string> created_batches = new List<string>();
            using (NpgsqlConnection con = new NpgsqlConnection(CustomHelper.CommonHelper.GetConnectionString))
            {
                try
                {
                    con.Open();
                    trans = con.BeginTransaction();

                    //foreach (string band in model_list.Select(o => o.band).Distinct())
                    //{
                    //    List<BatchCreationModel> same_bands_records = model_list.Where(x => x.band == band).ToList();
                    int k = 0;
                    foreach (string treatment_type in model_list.Select(o => o.treatment_type).Distinct())
                    {
                        bool is_repeat_M = false;
                        bool is_repeat_SM = false;
                        bool is_repeat_NM = false;
                        string batch_no_M = string.Empty;
                        string batch_no_SM = string.Empty;
                        string batch_no_NM = string.Empty;

                        string batch_number = string.Empty;
                        foreach (BatchCreationModel model in model_list.Where(x => x.treatment_type == treatment_type))
                        {
                            string des_type = string.Empty;
                            k++;
                            if (model.is_batch_created)
                            {
                                int numericValue;
                                bool isNumber = int.TryParse(model.band, out numericValue);
                                if (isNumber)
                                {
                                    if ((Convert.ToInt32(model.band) < 6) && isNumber)
                                    {
                                        des_type = "M";

                                        if (!is_repeat_M)
                                        {
                                            is_repeat_M = true;
                                            batch_number = GetbatchNumber(des_type, treatment_type, trans, con);
                                            created_batches.Add(batch_number);
                                            batch_no_M = batch_number;
                                        }
                                        else
                                        {
                                            batch_number = batch_no_M;

                                        }
                                    }
                                    else if ((Convert.ToInt32(model.band) >= 6) && isNumber)
                                    {

                                        des_type = "SM";
                                        if (!is_repeat_SM)
                                        {
                                            is_repeat_SM = true;
                                            batch_number = GetbatchNumber(des_type, treatment_type, trans, con);
                                            created_batches.Add(batch_number);
                                            batch_no_SM = batch_number;
                                        }
                                        else
                                        {
                                            batch_number = batch_no_SM;
                                        }
                                    }
                                }
                                else
                                {
                                    des_type = "NM";
                                    if (!is_repeat_NM)
                                    {
                                        is_repeat_NM = true;
                                        batch_number = GetbatchNumber(des_type, treatment_type, trans, con);
                                        created_batches.Add(batch_number);
                                        batch_no_NM = batch_number;
                                    }
                                    else
                                    {
                                        batch_number = batch_no_NM;
                                    }
                                }

                                DynamicParameters parameters = new DynamicParameters();

                                parameters.Add("p_batch_no", batch_number);
                                parameters.Add("p_employee_id", model.employee_id);
                                parameters.Add("p_claim_request_id", model.id);
                                parameters.Add("p_created_by", UserManager.User.UserID);
                                parameters.Add("p_band_level", des_type);
                                parameters.Add("p_treatment_type", model.treatment_type);

                                parameters.Add("p_last_number", Convert.ToInt32(batch_number.Split('-')[1]));
                                con.Query<ResponseData>("select * from mdcl_sp_insert_batch(:p_batch_no,:p_employee_id,:p_claim_request_id,:p_created_by,:p_band_level,:p_treatment_type,:p_last_number)", parameters, trans).FirstOrDefault();

                                DynamicParameters para = new DynamicParameters();
                                para.Add("p_employeeid", UserManager.User.UserID);
                                para.Add("p_claim_request_id", model.id);
                                para.Add("p_status_code", "REDFORDISP");
                                con.Query("select * from mdcl_sp_insert_empl_activities(:p_employeeid,:p_claim_request_id,:p_status_code)", para, trans);
                            }
                        }
                        k = 0;
                    }
                    // }
                    trans.Commit();
                    for (int k2 = 0; k2 < created_batches.Count; k2++)
                    {
                        if (k2 == created_batches.Count - 1 || created_batches.Count == k2)
                        {
                            sb.Append(created_batches[k2] + "  ");
                        }
                        else
                        {
                            sb.Append(created_batches[k2] + " | ");
                        }
                    }
                    return "Batch ( " + sb.ToString() + ")  created successfully !";
                }
                catch (Exception ex)
                {
                    ExceptionLogging.LogException(ex);
                    trans.Rollback();
                    return MessageHelper.ErroeMsg;

                }
            }
        }
        public string GetbatchNumber(string band, string treatment_type, NpgsqlTransaction trans, NpgsqlConnection con)
        {
            string batch_number = string.Empty;
            string treatment_typ = string.Empty;

            //DynamicParameters parameters = new DynamicParameters();

            //parameters.Add("p_band_level", band);
            //parameters.Add("p_treatment_type", treatment_type);
            //con.Query<ResponseData>("select * from mdcl_sp_insert_batchmaster(:p_band_level,:p_treatment_type)", parameters, trans).FirstOrDefault();

            //List<BatchMasterModel> bm_list = con.Query<BatchMasterModel>("select * from mdcl_sp_get_batchmaster_data()").ToList();
            DynamicParameters para = new DynamicParameters();
            para.Add("p_treatment_type", treatment_type);
            para.Add("p_band_level", band);
            BatchMasterModel bmm = con.Query<BatchMasterModel>("select * from mdcl_sp_get_batchmaster_data(:p_treatment_type,:p_band_level)", para).FirstOrDefault();

            switch (treatment_type)
            {
                case "#DOMCIL#":
                    treatment_typ = "DO";
                    break;
                case "#HOSPIL#":
                    treatment_typ = "HO";
                    break;
                case "#MATERNITY#":
                    treatment_typ = "MA";
                    break;
                case "#DENTAL#":
                    treatment_typ = "DA";
                    break;
                    //default:
                    //    treatment_typ = "RA";
                    //    break;
            }
            if (!string.IsNullOrWhiteSpace(treatment_typ))
            {
                if (bmm == null)
                {
                    batch_number = treatment_typ + band + "-" + 1;
                }
                else
                {
                    batch_number = treatment_typ + band + "-" + (bmm.last_number + 1);
                }
            }
            return batch_number;
        }
        public int GenerateRandomNo()
        {
            int _min = 1000;
            int _max = 9999;
            Random _rdm = new Random();
            return _rdm.Next(_min, _max);
        }

        public List<BatchCreationModel> Get_BatchCreation_Grid_Data()
        {
            List<BatchCreationModel> ack_list = new List<BatchCreationModel>();
            DataTable dt = new DataTable();
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(CustomHelper.CommonHelper.GetConnectionString))
                {
                    con.Open();
                    ack_list = con.Query<BatchCreationModel>("select * from mdcl_sp_get_batchcreation_grid_data()").ToList();
                }
            }
            catch (Exception ex)
            {

            }
            return ack_list;
        }

        public List<BatchCreationModel> Get_BatchItems_Grid_Data(string batch_no)
        {
            List<BatchCreationModel> ack_list = new List<BatchCreationModel>();
            DataTable dt = new DataTable();
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(CustomHelper.CommonHelper.GetConnectionString))
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("p_batch_no", batch_no);
                    con.Open();
                    ack_list = con.Query<BatchCreationModel>("select * from mdcl_sp_get_batchitems_grid_data(:p_batch_no)", parameters).ToList();
                }
            }
            catch (Exception ex)
            {

            }
            return ack_list;
        }
        public List<UpdateBatchModel> Get_UpdateBatchNo_Grid_data()
        {
            List<UpdateBatchModel> batch_list = new List<UpdateBatchModel>();
            DataTable dt = new DataTable();
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(CustomHelper.CommonHelper.GetConnectionString))
                {
                    con.Open();
                    batch_list = con.Query<UpdateBatchModel>("select * from mdcl_sp_get_updatebatch_grid_data()").ToList();
                }
            }
            catch (Exception ex)
            {

            }
            return batch_list;
        }

        public string Update_BatchDetails(UpdateBatchModel model)
        {
            NpgsqlTransaction trans = null;
            using (NpgsqlConnection con = new NpgsqlConnection(CustomHelper.CommonHelper.GetConnectionString))
            {
                try
                {
                    con.Open();
                    trans = con.BeginTransaction();
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("p_batch_no", model.batch_no);
                    parameters.Add("p_doctor_id", model.doctor_id);
                    parameters.Add("p_doctor_sent_date", model.doctor_sent_date);
                    parameters.Add("p_doctor_received_date", model.doctor_received_date);
                    parameters.Add("p_created_by", UserManager.User.UserID);
                    con.Query<ResponseData>("select * from mdcl_sp_update_batchcreation(:p_batch_no,:p_doctor_id,:p_doctor_sent_date,:p_doctor_received_date,:p_created_by)", parameters, trans).FirstOrDefault();

                    foreach (string val_claim_request_id in model.claim_request_ids.Split(','))
                    {
                        DynamicParameters para = new DynamicParameters();
                        para.Add("p_employeeid", UserManager.User.UserID);
                        para.Add("p_claim_request_id", Convert.ToInt32(val_claim_request_id));
                        if (model.doctor_sent_date != null && model.doctor_received_date == null)
                        {
                            para.Add("p_status_code", "SENTTOTMO");
                        }
                        if (model.doctor_received_date != null && model.doctor_sent_date != null)
                        {
                            para.Add("p_status_code", "RECFRMTMO");
                        }
                        con.Query("select * from mdcl_sp_insert_empl_activities(:p_employeeid,:p_claim_request_id,:p_status_code)", para, trans);
                    }
                    trans.Commit();

                    return MessageHelper.CommonUpdateStatus;
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    return MessageHelper.ErroeMsg;

                }
            }


        }
        public string PreparePrintDoc(string batch_no)
        {
            string res = string.Empty;
            StringBuilder sb = new StringBuilder();
            List<BatchCreationModel> ack_list = new List<BatchCreationModel>();
            DataTable dt = new DataTable();
            decimal Total = 0;
            string tratement_type = string.Empty;
            string Batch_date = string.Empty;
            int Trcount = 0;
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(CustomHelper.CommonHelper.GetConnectionString))
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("p_batch_no", batch_no);
                    con.Open();
                    ack_list = con.Query<BatchCreationModel>("select * from mdcl_sp_get_batchitems_grid_data_for_print(:p_batch_no)", parameters).ToList();
                }
            }
            catch (Exception ex)
            {

            }
            Trcount = ack_list.Count;
            foreach (BatchCreationModel batch in ack_list)
            {
                Total = Total + batch.total_claimed_rs;
                tratement_type = batch.treatment_type;
                Batch_date = batch.batch_created_date.ToString("dd/MM/yyyy");
            }
            using (StreamReader sr = new StreamReader(CommonHelper.Html_Template_Dir + "\\" + "BatchDetail.html"))
            {

                String line;
                decimal total_amt = 0;
                while ((line = sr.ReadLine()) != null && ack_list.Count > 0)
                {
                    line = line.Replace("#BatchNo", batch_no.ToString());
                    line = line.Replace("#Total", Total.ToString());
                    line = line.Replace("#Count", Trcount.ToString());
                    line = line.Replace("#TreatMentType", ack_list[0].treatment_type.ToString());
                    line = line.Replace("#BatchDate", Batch_date);

                    int n;
                    bool isNumeric = int.TryParse(ack_list[0].band, out n);


                    if (isNumeric)
                    {
                        if (Convert.ToInt32(ack_list[0].band) > 0)
                        {
                            line = line.Replace("#BAND_CLASS", "Officer");
                        }
                        else
                        {
                            line = line.Replace("#BAND_CLASS", "Non Officer");
                        }
                    }
                    else
                    {
                        line = line.Replace("#BAND_CLASS", "Non Officer");
                    }
                    // line = line.Replace("#total", Total + Convert.ToDecimal(bll.total_claimed_rs).ToString("##,##,###.00"));
                    if (line.Trim() == "<tbody>")
                    {
                        int srno = 0;
                        foreach (BatchCreationModel batch in ack_list)
                        {
                            string tr_line = string.Empty;
                            tr_line = "<tr><td style='border: 1px solid black; border-collapse: collapse;' align='center'>" + (srno + 1).ToString() + "</td><td style='border: 1px solid black; border-collapse: collapse;' align='center'>" + batch.employeecode + "</td><td style='border: 1px solid black; border-collapse: collapse;' align='center'>" + batch.employeename + "</td><td style='border: 1px solid black; border-collapse: collapse;' align='center'>" + batch.appln_no + "</td><td style='border: 1px solid black; border-collapse: collapse;' align='center'>" + batch.patient_name + "</td><td style='border: 1px solid black; border-collapse: collapse;' align='right'>" + batch.total_claimed_rs.ToString("##,##,###.00") + "</td></tr>";
                            sb.AppendLine(tr_line);
                            srno++;
                            total_amt += batch.total_claimed_rs;
                        }
                    }
                    line = line.Replace("#{total_amt}", total_amt.ToString("##,##,###.00"));
                    sb.AppendLine(line);
                }
            }
            return sb.ToString();
        }
    }
}