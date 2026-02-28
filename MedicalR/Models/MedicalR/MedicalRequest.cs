using Dapper;
using MedicalR.CustomHelper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace MedicalR.Models.MedicalR
{
    public class MedicalRequest
    {


        readonly static string constr = "Host=localhost;port=5432;Username=postgres;Password=uti@123;Database=MedicalR;";
        List<RCExpenseTypeDetailsModel> rcexpense_type_list = new List<RCExpenseTypeDetailsModel>();
        public MedicalRequestModel GetItem(int id)
        {
            MedicalRequestModel bll = new MedicalRequestModel();
            if (id == 0)
            {
                List<RCExpenseTypeDetailsModel> expense_type_details = new List<RCExpenseTypeDetailsModel>();
                bll = new MedicalRequestModel();


                RCExpenseTypeDetailsModel rdm = new RCExpenseTypeDetailsModel();
                rcexpense_type_list.Add(rdm);

                bll.expense_type_detalis = rcexpense_type_list;

                List<RCExpenseTypeDetailsItemsModel> rc_expensetypedetail_items = new List<RCExpenseTypeDetailsItemsModel>();
                RCExpenseTypeDetailsItemsModel rc_expensetypedetailsItem_model = new RCExpenseTypeDetailsItemsModel();
                rc_expensetypedetail_items.Add(rc_expensetypedetailsItem_model);
                bll.expense_type_detalis[0].request_claim_expense_items = rc_expensetypedetail_items;
            }
            else
            {
                using (NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {
                    con.Open();
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("p_claim_request_id", id);
                    bll = con.Query<MedicalRequestModel>("select * from mdcl_sp_getclaim_requestdetails_byid(:p_claim_request_id)", parameters).FirstOrDefault();

                    rcexpense_type_list = con.Query<RCExpenseTypeDetailsModel>("select * from mdcl_sp_getclaim_request_expensetypebyid(:p_claim_request_id)", parameters).ToList();

                    int k = 0;
                    foreach (RCExpenseTypeDetailsModel ex_ty_details in rcexpense_type_list)
                    {
                        if (bll.expense_type_detalis == null)
                        {
                            bll.expense_type_detalis = new List<RCExpenseTypeDetailsModel>();
                        }
                        bll.expense_type_detalis.Add(ex_ty_details);
                        DynamicParameters para2 = new DynamicParameters();
                        para2.Add("p_exptype_detail_id", ex_ty_details.id);
                        List<RCExpenseTypeDetailsItemsModel> item_list = con.Query<RCExpenseTypeDetailsItemsModel>("select * from mdcl_sp_getclaim_request_expensetype_items_byid(:p_exptype_detail_id)", para2).ToList();

                        foreach (RCExpenseTypeDetailsItemsModel item_model in item_list)
                        {
                            if (bll.expense_type_detalis[k].request_claim_expense_items == null)
                            {
                                bll.expense_type_detalis[k].request_claim_expense_items = new List<RCExpenseTypeDetailsItemsModel>();
                            }
                            bll.expense_type_detalis[k].request_claim_expense_items.Add(item_model);
                        }
                        k++;
                    }
                }
            }
            return bll;
        }
        public List<MedicalRequestModel> GetClaimRequestGridData()
        {

            List<MedicalRequestModel> Addlist = new List<MedicalRequestModel>();
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {
                    con.Open();
                    DynamicParameters para = new DynamicParameters();
                    para.Add("p_employee_id", UserManager.User.UserID);
                    Addlist = con.Query<MedicalRequestModel>("select * from mdcl_sp_get_claim_request_grid_data(:p_employee_id)", para).ToList();
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                Addlist = new List<MedicalRequestModel>();
            }
            return Addlist;

        }
        public string PreparePrintDoc(int request_id)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                MedicalRequestModel bll = new MedicalRequestModel();
                using (NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {
                    con.Open();
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("p_claim_request_id", request_id);
                    bll = con.Query<MedicalRequestModel>("select * from mdcl_sp_getclaim_requestdetails_byid_for_print(:p_claim_request_id)", parameters).FirstOrDefault();
                    rcexpense_type_list = con.Query<RCExpenseTypeDetailsModel>("select * from mdcl_sp_getclaim_request_expensetypebyid(:p_claim_request_id)", parameters).ToList();

                    int k = 0;
                    decimal Total = 0;

                    using (StreamReader sr = new StreamReader(System.Configuration.ConfigurationManager.AppSettings["MedicalRequestPrint"].ToString()))
                    {

                        String line;
                        while ((line = sr.ReadLine()) != null && rcexpense_type_list.Count > 0)
                        {
                            line = line.Replace("#AppNo", bll.appln_no);
                            line = line.Replace("#Date", bll.created_date.ToString("dd/MM/yyyy"));
                            line = line.Replace("#Empcode", bll.employeecode);
                            line = line.Replace("#Name", bll.employeename);
                            line = line.Replace("#Band", bll.band);
                            line = line.Replace("#Designation", bll.designation);
                            line = line.Replace("#Dept", bll.department_name);
                            line = line.Replace("#LOC", bll.national_location);
                            line = line.Replace("#PatientName", bll.patient_name);
                            line = line.Replace("#REL", bll.family_relation_code);
                            line = line.Replace("#NOILLNESS", bll.nature_of_illness);
                            line = line.Replace("#TRtype", bll.treatment_type);
                            line = line.Replace("#Enclosure", bll.enclosure);

                            line = line.Replace("#Mob", bll.mobile);
                            // line = line.Replace("#total",Total.ToString("##,##,###.00"));
                            line = line.Replace("#total", Convert.ToDecimal(bll.total_claimed_rs).ToString("##,##,###.00"));
                            if (line.Trim() == "</br></br>")
                            {
                                int count = 1;
                                foreach (RCExpenseTypeDetailsModel ex_ty_details in rcexpense_type_list)
                                {
                                    if (bll.treatment_type_code == "#HOSPIL#")
                                    {
                                        string hospital_cert = string.Empty;

                                        if (ex_ty_details.isHospitalAwardedCertificate)
                                        {
                                            hospital_cert = "Yes";
                                        }
                                        else
                                        {
                                            hospital_cert = "No";
                                        }

                                        string hosp_tbldats = "<div><label>From Date       :     " + ex_ty_details.from_date.ToString("dd/MM/yyyy") + "           |         To Date     :   " + ex_ty_details.to_date.ToString("dd/MM/yyyy") + "</br></label>";
                                        sb.AppendLine(hosp_tbldats);

                                        string hosp_tblnams = "<label>Hospital Name  : " + ex_ty_details.hospital_name + "</label><label>     |     Hospital Awarded I.T R Cert.?  : " + hospital_cert + "</label></br><table><div></label></div></div>";
                                        sb.AppendLine(hosp_tblnams);
                                    }
                                    if (bll.expense_type_detalis == null)
                                    {
                                        bll.expense_type_detalis = new List<RCExpenseTypeDetailsModel>();
                                    }
                                    bll.expense_type_detalis.Add(ex_ty_details);
                                    DynamicParameters para2 = new DynamicParameters();
                                    para2.Add("p_exptype_detail_id", ex_ty_details.id);
                                    List<RCExpenseTypeDetailsItemsModel> item_list = con.Query<RCExpenseTypeDetailsItemsModel>("select * from mdcl_sp_getclaim_request_expensetype_items_byid(:p_exptype_detail_id)", para2).ToList();

                                    string tblheaderEx = "<div><label>Expence Type  : " + ex_ty_details.expense_type_name + "</br></label></div>";
                                    sb.AppendLine(tblheaderEx);
                                    //if (count == 1)
                                    //{
                                    //    string tblheaderEx = "<div><label>Expence Type  : " + ex_ty_details.expense_type_name + " </br></label></div>";
                                    //    sb.AppendLine(tblheaderEx);
                                    //}
                                    //else
                                    //{
                                    //    string tblheaderEx = "<div><label>Expence Type  : " + ex_ty_details.expense_type_name + "</br></label></div>";
                                    //    sb.AppendLine(tblheaderEx);
                                    //}
                                    if (ex_ty_details.expense_type_code == "#PHYC#")
                                    {
                                        string tblline = "<table style='border: 1px solid black; border-collapse: collapse;' width='100%'><thead><tr><th style='border: 1px solid black; border-collapse: collapse;'>From Date</th><th style='border: 1px solid black; border-collapse: collapse;'>To Date</th><th style='border: 1px solid black; border-collapse: collapse;'>" + "Bill.No" + "</th><th style='border: 1px solid black; border-collapse: collapse;'>" + "Amt.Claimed Rs." + "</th><th style='border: 1px solid black; border-collapse: collapse;'>" + "Remark" + "</th></tr></thead>";
                                        sb.AppendLine(tblline);
                                    }
                                    else if (ex_ty_details.expense_type_code == "#CFDT#")
                                    {
                                        string tblline = "<table style='border: 1px solid black; border-collapse: collapse;' width='100%'><thead><tr><th style='border: 1px solid black; border-collapse: collapse;'>" + "Date" + "</th><th style='border: 1px solid black; border-collapse: collapse;'>" + "Head Of Expense" + "</th><th style='border: 1px solid black; border-collapse: collapse;'>" + "Bill.No" + "</th><th style='border: 1px solid black; border-collapse: collapse;'>" + "Amt.Claimed Rs." + "</th><th style='border: 1px solid black; border-collapse: collapse;'>" + "Remark" + "</th></tr></thead>";
                                        sb.AppendLine(tblline);
                                    }
                                    else if (ex_ty_details.expense_type_code == "#HAT#")
                                    {
                                        string tblline = "<table style='border: 1px solid black; border-collapse: collapse;' width='100%'><thead><tr><th style='border: 1px solid black; border-collapse: collapse;'>Date</th><th style='border: 1px solid black; border-collapse: collapse;'>" + "Bill.No" + "</th><th style='border: 1px solid black; border-collapse: collapse;'>From Date</th><th style='border: 1px solid black; border-collapse: collapse;'>To Date</th><th style='border: 1px solid black; border-collapse: collapse;'>" + "Days" + "</th><th style='border: 1px solid black; border-collapse: collapse;'>" + "Amt.Claimed Rs." + "</th><th style='border: 1px solid black; border-collapse: collapse;'>" + "Remark" + "</th></tr></thead>";
                                        sb.AppendLine(tblline);
                                    }
                                    else
                                    {
                                        string tblline = "<table style='border: 1px solid black; border-collapse: collapse;' width='100%'><thead><tr><th style='border: 1px solid black; border-collapse: collapse;'>" + "Date" + "</th><th style='border: 1px solid black; border-collapse: collapse;'>" + "Bill.No" + "</th><th style='border: 1px solid black; border-collapse: collapse;'>" + "Amt.Claimed Rs." + "</th><th style='border: 1px solid black; border-collapse: collapse;'>" + "Remark" + "</th></tr></thead>";
                                        sb.AppendLine(tblline);
                                    }

                                    foreach (RCExpenseTypeDetailsItemsModel item_model in item_list)
                                    {

                                        if (bll.expense_type_detalis[k].request_claim_expense_items == null)
                                        {
                                            bll.expense_type_detalis[k].request_claim_expense_items = new List<RCExpenseTypeDetailsItemsModel>();
                                        }
                                        bll.expense_type_detalis[k].request_claim_expense_items.Add(item_model);

                                        string tr_line = string.Empty;
                                        if (ex_ty_details.expense_type_code == "#PHYC#")
                                        {
                                            tr_line = "<tbody style='border: 1px solid black; border-collapse: collapse;'>" + "<tr><td style='border: 1px solid black; border-collapse: collapse;' align='center'>" + Convert.ToDateTime(item_model.from_date).ToString("dd/MM/yyyy") + "</td><td align='center'style='border: 1px solid black; border-collapse: collapse;'>" + Convert.ToDateTime(item_model.to_date).ToString("dd/MM/yyyy") + "</td><td align='center' style='border: 1px solid black; border-collapse: collapse;'>" + item_model.bill_no + "</td><td align='right' style='border: 1px solid black; border-collapse: collapse;'>" + Convert.ToDecimal(item_model.amt_claimed_rs).ToString("##,##,###.00") + "</td><td align='center' style='border: 1px solid black; border-collapse: collapse;'>" + item_model.remark + "</td></tr>";
                                        }
                                        else if (ex_ty_details.expense_type_code == "#HAT#")
                                        {
                                            tr_line = "<tbody style='border: 1px solid black; border-collapse: collapse;'>" + "<tr><td style='border: 1px solid black; border-collapse: collapse;' align='center'>" + Convert.ToDateTime(item_model.only_date).ToString("dd/MM/yyyy") + "</td><td align='center' style='border: 1px solid black; border-collapse: collapse;'>" + item_model.days + "</td><td style='border: 1px solid black; border-collapse: collapse;' align='center'>" + Convert.ToDateTime(item_model.from_date).ToString("dd/MM/yyyy") + "</td><td align='center'style='border: 1px solid black; border-collapse: collapse;'>" + Convert.ToDateTime(item_model.to_date).ToString("dd/MM/yyyy") + "</td><td align='center' style='border: 1px solid black; border-collapse: collapse;'>" + item_model.days + "</td><td align='right' style='border: 1px solid black; border-collapse: collapse;'>" + Convert.ToDecimal(item_model.amt_claimed_rs).ToString("##,##,###.00") + "</td><td align='center' style='border: 1px solid black; border-collapse: collapse;'>" + item_model.remark + "</td></tr>";
                                        }
                                        else if (ex_ty_details.expense_type_code == "#CFDT#")
                                        {
                                            tr_line = "<tbody style='border: 1px solid black; border-collapse: collapse;'>" + "<tr><td align='center' style='border: 1px solid black; border-collapse: collapse;'>" + Convert.ToDateTime(item_model.only_date).ToString("dd/MM/yyyy") + "</td><td align='center' style='border: 1px solid black; border-collapse: collapse;'>" + item_model.head_of_expense_name + "</td><td align='center' style='border: 1px solid black; border-collapse: collapse;'>" + item_model.bill_no + "</td><td style='border: 1px solid black; border-collapse: collapse;' align='right'>" + Convert.ToDecimal(item_model.amt_claimed_rs).ToString("##,##,###.00") + "</td><td align='center' style='border: 1px solid black; border-collapse: collapse;'>" + item_model.remark + "</td></tr>";
                                        }
                                        else
                                        {
                                            tr_line = "<tbody style='border: 1px solid black; border-collapse: collapse;'>" + "<tr><td align='center' style='border: 1px solid black; border-collapse: collapse;'>" + Convert.ToDateTime(item_model.only_date).ToString("dd/MM/yyyy") + "</td><td align='center' style='border: 1px solid black; border-collapse: collapse;'>" + item_model.bill_no + "</td><td style='border: 1px solid black; border-collapse: collapse;' align='right'>" + Convert.ToDecimal(item_model.amt_claimed_rs).ToString("##,##,###.00") + "</td><td align='center' style='border: 1px solid black; border-collapse: collapse;'>" + item_model.remark + "</td></tr>";
                                        }
                                        sb.AppendLine(tr_line);
                                        Total = Total + Convert.ToDecimal(item_model.amt_claimed_rs);
                                    }
                                    string tblehead = "</thead>";
                                    sb.AppendLine(tblehead);
                                    string tbleclose = "</table>";
                                    sb.AppendLine(tbleclose);
                                    string tblebr = "</br>";
                                    sb.AppendLine(tblebr);
                                    count++;
                                }
                                string tbltotalhe = "<center><label><b>" + "Total :" + "" + Total.ToString("##,##,###.00") + "</b></label><center>";
                                sb.AppendLine(tbltotalhe);
                            }
                            sb.AppendLine(line);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
            
                ExceptionLogging.LogException($"Error in PreparePrintDoc | ReqId: {request_id}", ex);

                //return "Unexpected error occurred while preparing print document.";
            }


            return sb.ToString();
        }
        public List<MedicalRequestModel> GetClaimRequestPastDetailsGridData()
        {

            List<MedicalRequestModel> Addlist = new List<MedicalRequestModel>();
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {
                    con.Open();
                    DynamicParameters para = new DynamicParameters();
                    para.Add("p_employee_id", UserManager.User.UserID);
                    Addlist = con.Query<MedicalRequestModel>("select * from mdcl_sp_get_claim_request_pastdetails_grid_data(:p_employee_id)", para).ToList();
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                Addlist = new List<MedicalRequestModel>();
            }
            return Addlist;

        }

        public List<SanctionMedicalBillsModel> ApplicationHistory_grid_data()
        {

            List<SanctionMedicalBillsModel> Addlist = new List<SanctionMedicalBillsModel>();
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {
                    con.Open();
                    DynamicParameters dpara = new DynamicParameters();
                    dpara.Add("login_id", UserManager.User.UserID);
                    Addlist = con.Query<SanctionMedicalBillsModel>("select * from mdcl_sp_get_sanction_medical_bills_grid_data(:login_id)", dpara).ToList();
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                Addlist = new List<SanctionMedicalBillsModel>();
            }
            return Addlist;


        }
        public List<MedicalRequestModel> GetClaimRequestGrid()
        {
            List<MedicalRequestModel> bll_list = new List<MedicalRequestModel>();
            using (NpgsqlConnection con = new NpgsqlConnection(constr))
            {
                con.Open();
                bll_list = con.Query<MedicalRequestModel>("select * from ").ToList();
            }
            return bll_list;
        }

        public string GetApplicationNo()
        {
            string appl_no = string.Empty;
            using (NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString))
            {
                con.Open();
                appl_no = con.Query<string>("select * from mdcl_sp_getlast_applnos()").FirstOrDefault();
                if (string.IsNullOrWhiteSpace(appl_no))
                {
                    appl_no = "A-1";
                }
                else
                {
                    string[] last_appl_no = appl_no.Split('-');
                    if (last_appl_no.Length > 0)
                    {
                        appl_no = "A-" + (Convert.ToInt32(last_appl_no[1]) + 1);
                    }
                }
            }
            return appl_no;
        }

        public ResponseModel Claim_validation_Check(string Check_Point, MedicalRequestModel model, int employee_id, NpgsqlConnection con)
        {
            ResponseModel resp = new ResponseModel();
            try
            {
                resp.Status = true;
                switch (Check_Point)
                {
                    case "TEETH_QTY":
                        {


                            foreach (RCExpenseTypeDetailsModel mdl_exp_types in model.expense_type_detalis)
                            {

                                foreach (RCExpenseTypeDetailsItemsModel mdl_items in mdl_exp_types.request_claim_expense_items)
                                {
                                    if ((mdl_items.head_of_expense_id == 2 || mdl_items.head_of_expense_id == 10) || mdl_exp_types.expense_type_id == 18)
                                    {
                                        DynamicParameters parameters = new DynamicParameters();
                                        parameters.Add("p_employee_id", employee_id);
                                        parameters.Add("p_patient_id", model.patient_id);
                                        parameters.Add("p_bill_date", mdl_items.only_date);
                                        parameters.Add("p_head_of_expense_id", mdl_items.head_of_expense_id);
                                        parameters.Add("p_expense_type_id", mdl_exp_types.expense_type_id);

                                        int? db_teeth_qty = con.Query<int>("select * from mdcl_sp_get_already_applied_teeth_qty(:p_employee_id,:p_patient_id,:p_bill_date,:p_head_of_expense_id,:p_expense_type_id)", parameters).FirstOrDefault();

                                        if (db_teeth_qty != null && mdl_items.no_of_teeth > 0 && mdl_exp_types.expense_type_id != 7)
                                        {
                                            if (db_teeth_qty >= 3)
                                            {
                                                resp.Message = "You have exceeded the teeth limits !";
                                                resp.Status = false;
                                            }
                                        }

                                    }
                                    if (mdl_exp_types.expense_type_id == 7)
                                    {
                                        DynamicParameters para2 = new DynamicParameters();
                                        para2.Add("p_patient_id", model.patient_id);
                                        DateTime last_spectacles_date = con.Query<DateTime>("select * from mdcl_sp_get_already_applied_spectacles(:p_patient_id)", para2).FirstOrDefault();
                                        if (last_spectacles_date != null)
                                        {
                                            last_spectacles_date = last_spectacles_date.AddYears(3);
                                            if (last_spectacles_date > mdl_items.only_date)
                                            {
                                                resp.Message = "You have exceeded the spectacles limits !";
                                                resp.Status = false;
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                        }

                }
            }
            catch (Exception ex)
            {
                resp.Status = false;
                resp.Message = "Something went wrong " + ex.Message;
                ExceptionLogging.LogException(ex);
            }
            return resp;

        }
        public ResponseModel Save(MedicalRequestModel model)
        {
            ResponseModel resp = new ResponseModel();
            string respnse = string.Empty;
            Npgsql.NpgsqlTransaction trans = null; ;
            string appl_no = string.Empty;
            using (NpgsqlConnection con = new NpgsqlConnection(CustomHelper.CommonHelper.GetConnectionString))
            {
                try
                {
                    con.Open();
                    trans = con.BeginTransaction();
                    if (model.id > 0)
                    {

                        appl_no = model.appln_no;
                    }
                    if (model.id > 0 || model.is_delete_appln)
                    {
                        DynamicParameters delete_parameters = new DynamicParameters();
                        delete_parameters.Add("p_id", model.id);
                        con.Query<ResponseData>("select * from mdcl_sp_delete_existing_claims(:p_id)", delete_parameters, trans).FirstOrDefault();
                        if (model.is_delete_appln)
                        {
                            resp.Status = true;
                            resp.Message = "Application deleted successfully!";
                            trans.Commit();
                            return resp;
                        }
                    }
                    resp = Claim_validation_Check("TEETH_QTY", model, UserManager.User.UserID, con);
                    if (resp.Status)
                    {

                        DynamicParameters parameters = new DynamicParameters();
                        parameters.Add("p_id", model.id);
                        parameters.Add("p_employee_id", UserManager.User.UserID);
                        parameters.Add("p_patient_id", model.patient_id);
                        if (string.IsNullOrWhiteSpace(appl_no))
                        {
                            appl_no = GetApplicationNo();
                            parameters.Add("p_appln_no", appl_no);
                        }
                        else
                        {
                            parameters.Add("p_appln_no", appl_no);
                        }
                        parameters.Add("p_nature_of_illness", model.nature_of_illness);
                        parameters.Add("p_treatment_type_id", model.treatment_type_id);
                        parameters.Add("p_is_treatment_taken_out_of_head_quaters", model.is_treatment_taken_out_of_head_quaters);
                        parameters.Add("p_total_claimed_rs", model.total_claimed_rs);
                        parameters.Add("p_created_by", UserManager.User.UserID);
                        parameters.Add("p_is_saveforletter", model.is_saveforletter);
                        parameters.Add("p_enclosure", model.enclosure);
                        ResponseData response = con.Query<ResponseData>("select * from mdcl_sp_insert_claim_request_data(:p_id,:p_employee_id,:p_patient_id,:p_appln_no,:p_nature_of_illness,:p_treatment_type_id,:p_is_treatment_taken_out_of_head_quaters,:p_total_claimed_rs,:p_created_by,:p_is_saveforletter,:p_enclosure)", parameters, trans).FirstOrDefault();

                        if (model.id == 0)
                        {
                            DynamicParameters audit_para = new DynamicParameters();
                            audit_para.Add("p_employee_id", UserManager.User.UserID);

                            audit_para.Add("p_appln_no", appl_no);
                            CommonHelper.write_log("mdcl_sp_insert_appln_audit : appl_no :" + appl_no);
                            int last_rec = con.Query<int>("select * from mdcl_sp_insert_appln_audit(:p_employee_id,:p_appln_no)", audit_para, trans).FirstOrDefault();
                        }

                        if (model.expense_type_detalis.Count > 0 && response.mdcl_sp_insert_claim_request_data > 0)
                        {
                            foreach (RCExpenseTypeDetailsModel exp_type_detail in model.expense_type_detalis)
                            {
                                DynamicParameters ex_type_parameters = new DynamicParameters();
                                ex_type_parameters.Add("p_claim_request_id", response.mdcl_sp_insert_claim_request_data);
                                ex_type_parameters.Add("p_expense_type_id", exp_type_detail.expense_type_id);
                                ex_type_parameters.Add("p_from_date", exp_type_detail.from_date);
                                ex_type_parameters.Add("p_to_date", exp_type_detail.to_date);
                                ex_type_parameters.Add("p_hospital_name", exp_type_detail.hospital_name);
                                ex_type_parameters.Add("p_ishospitalawardedcertificate", exp_type_detail.isHospitalAwardedCertificate);
                                ex_type_parameters.Add("p_appln_et_no", appl_no);

                                ResponseData_expense_type_details res_exp_type = con.Query<ResponseData_expense_type_details>("select * from mdcl_sp_insert_claim_request_expensetype_data(:p_claim_request_id,:p_expense_type_id,:p_from_date,:p_to_date,:p_hospital_name,:p_isHospitalAwardedCertificate,:p_appln_et_no)", ex_type_parameters, trans).FirstOrDefault();

                                try
                                {

                                    foreach (RCExpenseTypeDetailsItemsModel item in exp_type_detail.request_claim_expense_items)
                                    {
                                        DynamicParameters items_parameters = new DynamicParameters();
                                        items_parameters.Add("p_claim_request_exp_type_id", res_exp_type.mdcl_sp_insert_claim_request_expensetype_data);
                                        items_parameters.Add("p_from_date", item.from_date);
                                        items_parameters.Add("p_to_date", item.to_date);
                                        items_parameters.Add("p_only_date", item.only_date);
                                        items_parameters.Add("p_head_of_expense_id", item.head_of_expense_id);
                                        items_parameters.Add("p_no_of_teeth", item.no_of_teeth);
                                        items_parameters.Add("p_bill_no", item.bill_no);
                                        items_parameters.Add("p_claim_amt", item.amt_claimed_rs);
                                        items_parameters.Add("p_remark", item.remark);
                                        items_parameters.Add("p_days", item.days);
                                        items_parameters.Add("p_appln_ed_no", appl_no);
                                        var exe = con.Query("select * from mdcl_sp_insert_claim_request_expensetype_detail_data" +
                                            "(:p_claim_request_exp_type_id,:p_from_date,:p_to_date,:p_only_date,:p_head_of_expense_id,:p_no_of_teeth,:p_bill_no,:p_claim_amt,:p_remark,:p_days,:p_appln_ed_no)", items_parameters, trans);

                                    }
                                }
                                catch (Exception ex)
                                {
                                    ExceptionLogging.LogException(ex);
                                }
                            }
                            if (!model.is_saveforletter)
                            {
                                DynamicParameters para = new DynamicParameters();
                                para.Add("p_employeeid", UserManager.User.UserID);
                                para.Add("p_claim_request_id", response.mdcl_sp_insert_claim_request_data);
                                para.Add("p_status_code", "SUBBYEMP");
                                con.Query("select * from mdcl_sp_insert_empl_activities(:p_employeeid,:p_claim_request_id,:p_status_code)", para, trans);
                            }
                        }



                        trans.Commit();
                        resp.Status = true;
                        resp.Message = MessageHelper.RequestStatus;

                    }

                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    resp.Status = false;
                    resp.Message = MessageHelper.ErroeMsg + " | " + ex.Message;
                    ExceptionLogging.LogException(ex);
                }
            }
            return resp;
        }

    }
}
//public class MedicalRequest
//{
//    public int employee_id { get; set; }
//    public string employee_name { get; set; }
//    public int patient_id { get; set; }
//    public string patient_name { get; set; }
//    public string patient_first_name { get; set; }
//    public string patient_last_name { get; set; }
//    public string appln_no { get; set; }
//    public int id { get; set; }
//    public string Nature_of_illness { get; set; }
//    public int treatment_type_id { get; set; }
//    public string expense_type { get; set; }
//    public DateTime from_date { get; set; }
//    public DateTime to_date { get; set; }
//    public int hospital_id { get; set; }
//    public string hosp_awarded_i_t_rebate { get; set; }
//    public string heads_of_expense { get; set; }
//    public string bill_no { get; set; }
//    public string amt_claimed_rs { get; set; }
//    public string remark { get; set; }
//    public string total_claimed_rs { get; set; }
//    public Boolean is_treatment_taken_out_of_head_quaters { get; set; }
//    public string relationship { get; set; }
//  //  public Array expenses { get; set; }
//  //  public List<ExpenseModels> ExpenseModels { get; set; }

//}
//public class ExpenseModels
//{
//    public string heads_of_expense { get; set; }
//    public string bill_no { get; set; }
//    public int amt_claimed_rs { get; set; }
//    public string remark { get; set; }


//}

//public class CommonModels
//{
//    public MedicalRequest mmodel { get; set; }
//    public List<ExpenseModels> emodel { get; set; }

//}




