using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MedicalR.Models;
using Npgsql;
using MedicalR.Models.MedicalR;
using MedicalR.CustomHelper;
using MedicalR.DataAccessLayer.IDAL.MedicalR;
using System.Data;
using System.IO;
using Newtonsoft.Json;

namespace MedicalR.DataAccessLayer.DAL.MedicalR
{
    public class DALMedicalRequest : IDALMedicalRequest
    {
        //public List<TreatmentType> GetTreatmentTypelist(TreatmentType objModel)
        //{
        //    List<TreatmentType> TreatmentTypelist = new List<TreatmentType>();
        //    NpgsqlConnection con = new NpgsqlConnection(CustomHelper.CommonHelper.GetConnectionString);
        //    try
        //    {

        //        //NpgsqlCommand cmd = new NpgsqlCommand("getalltreatmentlist", con);
        //        //cmd.CommandType = CommandType.StoredProcedure;
        //        //cmd.Parameters.AddWithValue("@id", objModel.id);
        //        //cmd.Parameters.AddWithValue("@name", objModel.name);
        //        //con.Open();
        //        //NpgsqlDataAdapter da = new NpgsqlDataAdapter();
        //        //da.SelectCommand = cmd;
        //        //DataSet ds = new DataSet();
        //        //da.Fill(ds);
        //        NpgsqlCommand cmd = new NpgsqlCommand();
        //        cmd.Connection = con;
        //        cmd.CommandText = "Select id, name from mdcl_tbl_lov where categoryid=1";
        //        cmd.CommandType = CommandType.Text;
        //        NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
        //        DataSet ds = new DataSet();
        //        da.Fill(ds);
        //        if (ds.Tables[0].Rows.Count == 0)
        //        {
        //            TreatmentTypelist = null;
        //        }
        //        else
        //        {
        //            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        //            {
        //                TreatmentType Treatment = new TreatmentType();
        //                Treatment.id = Convert.ToInt32(ds.Tables[0].Rows[i]["id"].ToString());
        //                Treatment.name = ds.Tables[0].Rows[i]["name"].ToString();
        //                TreatmentTypelist.Add(Treatment);

        //            }

        //        }
        //    }
        //    catch (Exception ex)

        //    {
        //        ExceptionLogging.LogException(ex);
        //        TreatmentTypelist = new List<TreatmentType>();
        //    }
        //    finally
        //    {
        //        con.Close();
        //        con.Dispose();
        //    }
        //    return TreatmentTypelist;
        //}
        //public List<EmpFamilydetail> Getfamilydetail(EmpFamilydetail objModel)
        //{
        //    List<EmpFamilydetail> Familydetail = new List<EmpFamilydetail>();
        //    NpgsqlConnection con = new NpgsqlConnection(CustomHelper.CommonHelper.GetConnectionString);
        //    try
        //    {
        //        NpgsqlCommand cmd = new NpgsqlCommand();
        //        cmd.Connection = con;
        //        cmd.CommandText = "select distinct first_name,last_name,family_relation_id from uti_emp_family_details where employeeid=34033";
        //        cmd.CommandType = CommandType.Text;
        //        NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
        //        DataSet ds = new DataSet();
        //        da.Fill(ds);
        //        if (ds.Tables[0].Rows.Count == 0)
        //        {
        //            Familydetail = null;
        //        }
        //        else
        //        {
        //            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        //            {
        //                EmpFamilydetail Members = new EmpFamilydetail();
        //                //Members.first_name = ds.Tables[0].Rows[i]["first_name"].ToString(); 
        //                Members.family_relation_id = Convert.ToInt32(ds.Tables[0].Rows[i]["family_relation_id"].ToString());
        //                Members.first_name = ds.Tables[0].Rows[i]["first_name"].ToString() + " " + ds.Tables[0].Rows[i]["last_name"].ToString();
        //                Familydetail.Add(Members);

        //            }

        //        }
        //    }
        //    catch (Exception ex)

        //    {
        //        ExceptionLogging.LogException(ex);
        //        Familydetail = new List<EmpFamilydetail>();
        //    }
        //    finally
        //    {
        //        con.Close();
        //        con.Dispose();
        //    }
        //    return Familydetail;
        //}
        //public List<Hospitallist> GetHospitallist(Hospitallist objModel)
        //{
        //    List<Hospitallist> GetHospitallist = new List<Hospitallist>();
        //    NpgsqlConnection con = new NpgsqlConnection(CustomHelper.CommonHelper.GetConnectionString);
        //    try
        //    {


        //        NpgsqlCommand cmd = new NpgsqlCommand();
        //        cmd.Connection = con;
        //        cmd.CommandText = "Select hospital_id, name from mdcl_tbl_hospital_mast";
        //        cmd.CommandType = CommandType.Text;
        //        NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
        //        DataSet ds = new DataSet();
        //        da.Fill(ds);
        //        if (ds.Tables[0].Rows.Count == 0)
        //        {
        //            GetHospitallist = null;
        //        }
        //        else
        //        {
        //            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        //            {
        //                Hospitallist hospitallist = new Hospitallist();
        //                hospitallist.hospital_id = Convert.ToInt32(ds.Tables[0].Rows[i]["hospital_id"].ToString());
        //                hospitallist.name = ds.Tables[0].Rows[i]["name"].ToString();
        //                GetHospitallist.Add(hospitallist);

        //            }

        //        }
        //    }
        //    catch (Exception ex)

        //    {
        //        ExceptionLogging.LogException(ex);
        //        GetHospitallist = new List<Hospitallist>();
        //    }
        //    finally
        //    {
        //        con.Close();
        //        con.Dispose();
        //    }
        //    return GetHospitallist;
        //}
        //public List<string> getColumns(string columnType)
        //{
        //    List<string> SelectedColumns = new List<string>();
        //    NpgsqlConnection con = new NpgsqlConnection(CustomHelper.CommonHelper.GetConnectionString);
        //    try
        //    {

        //        //NpgsqlCommand cmd = new NpgsqlCommand("", con);



        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionLogging.LogException(ex);

        //    }
        //    finally
        //    {
        //        con.Close();
        //        con.Dispose();
        //    }
        //    return SelectedColumns;
        //}
        //public List<MedicalRequest> GetUserList(MedicalRequest objModel)
        //{
        //    List<MedicalRequest> getUserList = new List<MedicalRequest>();
        //    NpgsqlConnection con = new NpgsqlConnection(CustomHelper.CommonHelper.GetConnectionString);
        //    try
        //    {

        //        //NpgsqlCommand cmd = new NpgsqlCommand("", con);



        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionLogging.LogException(ex);

        //    }
        //    finally
        //    {
        //        con.Close();
        //        con.Dispose();
        //    }
        //    return getUserList;
        //}
        //public List<ExpenseType> GetExpenseType(string name)
        //{
        //    List<ExpenseType> getExpenseType = new List<ExpenseType>();
        //    NpgsqlConnection con = new NpgsqlConnection(CustomHelper.CommonHelper.GetConnectionString);
        //    DataTable dt = new DataTable();

        //    try
        //    {
        //        using (con = new NpgsqlConnection(CommonHelper.GetConnectionString))
        //        {

        //            con.Open();
        //            using (NpgsqlCommand cmd = new NpgsqlCommand("select * from  mdcl_sp_get_expensetype(:name)", con))
        //            {
        //                cmd.Connection = con;
        //                cmd.Parameters.AddWithValue("name", name);
        //                using (NpgsqlDataAdapter SqDA = new NpgsqlDataAdapter(cmd))
        //                {
        //                    SqDA.Fill(dt);
        //                }
        //            }

        //            if (dt.Rows.Count == 0)
        //            {
        //                ExpenseType expense = new ExpenseType();
        //            }
        //            else
        //            {
        //                foreach (DataRow drow in dt.Rows)
        //                {
        //                    ExpenseType expenses = new ExpenseType();
        //                    //expenses.id = string.IsNullOrWhiteSpace(drow["id"].ToString()) ? 0 : Convert.ToInt32(drow["id"].ToString());
        //                    expenses.expence_type = string.IsNullOrWhiteSpace(drow["expence_type"].ToString()) ? "" : drow["expence_type"].ToString();
        //                    getExpenseType.Add(expenses);

        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionLogging.LogException(ex);
        //        ExpenseType expense = new ExpenseType();
        //    }
        //    finally
        //    {
        //        con.Close();
        //        con.Dispose();
        //    }
        //    return getExpenseType;

        //}
        //public List<HeadsOfExpense> GetHeadsonExpense(string name)
        //{
        //    List<HeadsOfExpense> getHeadson = new List<HeadsOfExpense>();
        //    NpgsqlConnection con = new NpgsqlConnection(CustomHelper.CommonHelper.GetConnectionString);
        //    DataTable dt = new DataTable();
        //    try
        //    {
        //        using (con = new NpgsqlConnection(CommonHelper.GetConnectionString))
        //        {

        //            con.Open();
        //            using (NpgsqlCommand cmd = new NpgsqlCommand("select * from  mdcl_sp_get_headsonexpense(:name)", con))
        //            {
        //                cmd.Connection = con;
        //                cmd.Parameters.AddWithValue("name", name);
        //                using (NpgsqlDataAdapter SqDA = new NpgsqlDataAdapter(cmd))
        //                {
        //                    SqDA.Fill(dt);
        //                }
        //            }

        //            if (dt.Rows.Count == 0)
        //            {
        //                getHeadson = new List<HeadsOfExpense>();
        //            }
        //            else
        //            {
        //                foreach (DataRow drow in dt.Rows)
        //                {
        //                    HeadsOfExpense expense = new HeadsOfExpense();
        //                    //expense.id = string.IsNullOrWhiteSpace(drow["id"].ToString()) ? 0 : Convert.ToInt32(drow["id"].ToString());
        //                    expense.head_expence = string.IsNullOrWhiteSpace(drow["head_expence"].ToString()) ? "" : drow["head_expence"].ToString();
        //                    getHeadson.Add(expense);

        //                }
        //            }
        //        }
        //    }

        //    catch (Exception ex)

        //    {
        //        ExceptionLogging.LogException(ex);
        //        getHeadson = new List<HeadsOfExpense>();
        //    }
        //    finally
        //    {
        //        con.Close();
        //        con.Dispose();
        //    }
        //    return getHeadson;
        //}
        //public ResponseModel AddRequest(MedicalRequest objModel)
        //{
        //    NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString);
        //    ResponseModel Response = new ResponseModel();
        //    DataTable dt = new DataTable();
        //    int id;

        //    Boolean headerquater = false;
        //    try
        //    {
        //        // NpgsqlConnection con = new NpgsqlConnection();
        //        id = objModel.employee_id;
        //        headerquater = objModel.is_treatment_taken_out_of_head_quaters;
        //        using (con = new NpgsqlConnection(CommonHelper.GetConnectionString))
        //        {

        //            con.Open();

        //            using (NpgsqlCommand cmd = new NpgsqlCommand("select * from  mdcl_sp_insert_Request_claim(:id,:patient_id,:appln_no,:Nature_of_illness,:treatment_type_id,:expense_type,:from_date,:to_date,:hospital_id,:hosp_awarded_i_t_rebate,:heads_of_expense,:bill_no,:amt_claimed_rs,:remark,:total_claimed_rs,:is_treatment_taken_out_of_head_quaters)", con))

        //            {
        //                cmd.Connection = con;
        //                cmd.Parameters.AddWithValue("id", objModel.employee_id);
        //                cmd.Parameters.AddWithValue("patient_id", objModel.patient_id);
        //                cmd.Parameters.AddWithValue("appln_no", objModel.appln_no);
        //                cmd.Parameters.AddWithValue("Nature_of_illness", objModel.Nature_of_illness);
        //                cmd.Parameters.AddWithValue("treatment_type_id", objModel.treatment_type_id);
        //                cmd.Parameters.AddWithValue("expense_type", objModel.expense_type);
        //                cmd.Parameters.AddWithValue("from_date", objModel.from_date);
        //                cmd.Parameters.AddWithValue("to_date", objModel.to_date);
        //                cmd.Parameters.AddWithValue("hospital_id", objModel.hospital_id);
        //                cmd.Parameters.AddWithValue("hosp_awarded_i_t_rebate", objModel.hosp_awarded_i_t_rebate);
        //                // List<MedicalRequest> expenses = new List<MedicalRequest>();
        //                //List<string> temp = new List<string>();
        //                //temp[0] = expenses[0];
        //                // string[] tokens = objModel.expenses.Split(',');
        //                //var coount = tokens.Count();
        //                //  ExpenseModels modelObj = DALMedicalRequest.Deserialize<ExpenseModels>(objModel.expenses);
        //                //StringReader txtreader = new StringReader(value);
        //                //JsonTextReader reader = new JsonTextReader(txtreader);
        //                //return GetSerializer().Deserialize<T>(reader);
        //                // int i = 0;
        //                List<ExpenseModels> templist = new List<ExpenseModels>();
        //                ExpenseModels temobj = new ExpenseModels();
        //                //int counter = 0;
        //                //for (int i = 0; i <= tokens.Length - 1; i++)
        //                //{
        //                //    if(counter < 4)
        //                //    {
        //                //        for (int j = i; j < tokens.Length-1; j++)
        //                //        {
        //                //            //temobj.heads_of_expense = tokens[j].ToString();
        //                //            counter++;
        //                //        }
        //                //    }
        //                //    else
        //                //    {
        //                //        counter = 0;
        //                //    }
        //                //}


        //                //var heads = tokens[1];
        //                //var bill = tokens[1];
        //                //var amt = tokens[2];
        //                //var remark = tokens[3];
        //                //cmd.Parameters.AddWithValue("heads_of_expense", heads);
        //                //cmd.Parameters.AddWithValue("bill_no", bill);
        //                //cmd.Parameters.AddWithValue("amt_claimed_rs", amt);
        //                //cmd.Parameters.AddWithValue("remark", remark);


        //                //var heads = tokens[0];
        //                //var bill = tokens[1];
        //                //var amt = tokens[2];
        //                //var remark = tokens[3];
        //                //cmd.Parameters.AddWithValue("heads_of_expense", heads);
        //                //cmd.Parameters.AddWithValue("bill_no", bill);
        //                //cmd.Parameters.AddWithValue("amt_claimed_rs", amt);
        //                //cmd.Parameters.AddWithValue("remark", remark);





        //                //if (objModel.expenses.Count > 0)
        //                //{
        //                //    var expense = objModel.expenses;
        //                //    for (int i = 0; i < expense.Count; i++)
        //                //    {

        //                //        cmd.Parameters.AddWithValue("heads_of_expense", objModel.heads_of_expense);
        //                //        cmd.Parameters.AddWithValue("bill_no", objModel.bill_no);
        //                //        cmd.Parameters.AddWithValue("amt_claimed_rs", objModel.amt_claimed_rs);
        //                //        cmd.Parameters.AddWithValue("remark", objModel.remark);


        //                //    }

        //                //}
        //                //else
        //                //{
        //                //    cmd.Parameters.AddWithValue("heads_of_expense", objModel.heads_of_expense);
        //                //    cmd.Parameters.AddWithValue("bill_no", objModel.bill_no);
        //                //    cmd.Parameters.AddWithValue("amt_claimed_rs", objModel.amt_claimed_rs);
        //                //    cmd.Parameters.AddWithValue("remark", objModel.remark);

        //                //}
        //                //cmd.Parameters.AddWithValue("heads_of_expense", objModel.heads_of_expense);
        //                //cmd.Parameters.AddWithValue("bill_no", objModel.bill_no);
        //                //cmd.Parameters.AddWithValue("amt_claimed_rs", objModel.amt_claimed_rs);
        //                //cmd.Parameters.AddWithValue("remark", objModel.remark);
        //                cmd.Parameters.AddWithValue("total_claimed_rs", objModel.total_claimed_rs);
        //                cmd.Parameters.AddWithValue("is_treatment_taken_out_of_head_quaters", objModel.is_treatment_taken_out_of_head_quaters);

        //                int Res = (cmd.ExecuteNonQuery());

        //                if (Res == 0)
        //                {
        //                    Response.Status = false;
        //                    Response.Message = MessageHelper.ErroeMsg;
        //                }
        //                else
        //                {
        //                    Response.Status = true;
        //                    Response.Message = MessageHelper.RequestStatus;
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionLogging.LogException(ex);
        //        Response.Status = false;
        //        Response.Message = MessageHelper.ExceptionMessage;
        //    }
        //    finally
        //    {
        //        con.Close();
        //        con.Dispose();
        //    }
        //    return Response;
        //}
        //public List<MedicalRequest> GetAcknowledgelist()
        //{
        //    NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString);
        //    List<MedicalRequest> Addlist = new List<MedicalRequest>();
        //    DataTable dt = new DataTable();

        //    try
        //    {

        //        using (con = new NpgsqlConnection(CommonHelper.GetConnectionString))
        //        {

        //            con.Open();
        //            using (NpgsqlCommand cmd = new NpgsqlCommand("select * from  mdcl_sp_get_acknowledge_data()", con))
        //            {
        //                cmd.Connection = con;

        //                using (NpgsqlDataAdapter SqDA = new NpgsqlDataAdapter(cmd))
        //                {
        //                    SqDA.Fill(dt);
        //                }
        //            }

        //            if (dt.Rows.Count == 0)
        //            {
        //                Addlist = new List<MedicalRequest>();
        //            }
        //            else
        //            {
        //                foreach (DataRow drow in dt.Rows)
        //                {
        //                    MedicalRequest robj = new MedicalRequest();
        //                    //robj.id = string.IsNullOrWhiteSpace(drow["id"].ToString()) ? 0 : Convert.ToInt32(drow["id"].ToString());
        //                    robj.employee_id = string.IsNullOrWhiteSpace(drow["employee_id"].ToString()) ? 0 : Convert.ToInt32(drow["employee_id"].ToString());
        //                    robj.appln_no = string.IsNullOrWhiteSpace(drow["appln_no"].ToString()) ? "" : drow["appln_no"].ToString();                            
        //                    robj.employee_name = string.IsNullOrWhiteSpace(drow["employeename"].ToString()) ? "" : drow["employeename"].ToString();
        //                    robj.total_claimed_rs = string.IsNullOrWhiteSpace(drow["total_claimed_rs"].ToString()) ? "" : drow["total_claimed_rs"].ToString();
        //                    robj.patient_first_name = string.IsNullOrWhiteSpace(drow["first_name"].ToString()) ? "" : drow["first_name"].ToString();
        //                    robj.patient_last_name = string.IsNullOrWhiteSpace(drow["last_name"].ToString()) ? "" : drow["last_name"].ToString();
        //                    robj.relationship= string.IsNullOrWhiteSpace(drow["family_relation_code"].ToString()) ? "" : drow["family_relation_code"].ToString();
        //                    robj.patient_name = robj.patient_first_name +" "+ robj.patient_last_name;

        //                    Addlist.Add(robj);

        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionLogging.LogException(ex);
        //        Addlist = new List<MedicalRequest>();
        //    }
        //    finally
        //    {
        //        con.Close();
        //        con.Dispose();
        //    }
        //    return Addlist;
        //}
        //public static T Deserialize<T>(string value)
        //{
        //    StringReader txtreader = new StringReader(value);
        //    JsonTextReader reader = new JsonTextReader(txtreader);
        //    return GetSerializer().Deserialize<T>(reader);
        //}

        //static JsonSerializer GetSerializer()
        //{
        //    JsonSerializerSettings settings;
        //    settings = new JsonSerializerSettings();
        //    settings.Formatting = Newtonsoft.Json.Formatting.None;
        //    settings.NullValueHandling = NullValueHandling.Ignore;
        //    settings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
        //    settings.DateFormatString = "yyyy-MM-ddTHH:mm:00zzz";
        //    return JsonSerializer.Create(settings);
        //}

    }

}


