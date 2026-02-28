using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedicalR.Models.MedicalR
{

    public class ClaimRequestModel
    {

    }
    public class ResponseDataHospitalization_cf_data
    {
        public int mdcl_sp_insert_hospitalization_credit_fac_data_on_send_letter { get; set; }
    }
    public class ResponseData
    {
        public int mdcl_sp_insert_claim_request_data { get; set; }
        public int mdcl_sp_insert_hospitalization_credit_fac_data_on_send_letter { get; set; }
    }
    public class ResponseData_expense_type_details
    {
        public int mdcl_sp_insert_claim_request_expensetype_data { get; set; }
    }
    public class ResponseData_exp_type_items
    {
        public int mdcl_sp_insert_claim_request_expensetype_detail_data { get; set; }
    }
    public class ApplicationNoModel
    {
        public string appl_no { get; set; }
    }
    public class MR_OBJECTION_LETTER_MODEL
    {
        public int claim_request_id { get; set; }
        public DateTime rejection_date { get; set; }
        public string appln_no { get; set; }
        public string objection_remark { get; set; }
        public string email_template { get; set; }

        public string employee_name { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string address3 { get; set; }
        public string address4 { get; set; }
        public string city { get; set; }
        public string postal { get; set; }
        public string floor { get; set; }
    }
    public class MedicalRequestModel
    {
        public int id { get; set; }
        public string employeename { get; set; }
        public string employeecode { get; set; }
        public int employee_id { get; set; }
        public int patient_id { get; set; }
        public string department_name { get; set; }
        public string national_location { get; set; }
        public string email { get; set; }
        public string appln_no { get; set; }
        public string patient_name { get; set; }
        public string nature_of_illness { get; set; }
        public int treatment_type_id { get; set; }
        public string treatment_type_code { get; set; }
        public bool is_treatment_taken_out_of_head_quaters { get; set; }
        public string family_relation_code { get; set; }
        public decimal total_claimed_rs { get; set; }
        public decimal total_sanctioned_rs { get; set; }
        public DateTime created_date { get; set; }
        public bool is_payment_released { get; set; }
        public string payment_status { get; set; }
        public string treatment_type { get; set; }
        public bool is_saveforletter { get; set; }
        public string mobile { get; set; }
        public string band_level { get; set; }
        public string edit_status { get; set; } = "Received from TMO";
        public string enclosure { get; set; } = string.Empty;
        public string band { get; set; }
        public string designation { get; set; }
        public int objection_code_id { get; set; }
        public string objection_remark { get; set; }

        public decimal? add_pay_amt { get; set; }
        public string add_pay_remark { get; set; }
        public string add_pay_enclosure { get; set; }
        public List<RCExpenseTypeDetailsModel> expense_type_detalis { get; set; }
        public bool is_delete_appln { get; set; } = false;
        public bool is_objection { get; set; }
        public bool is_operator { get; set; }
        public DateTime payment_date { get; set; }
    }
    public class RCExpenseTypeDetailsModel
    {
        public int id { get; set; }

        public int expense_type_id { get; set; }
        public string expense_type_name { get; set; }
        public string expense_type_code { get; set; }
        public DateTime from_date { get; set; }
        public DateTime to_date { get; set; }
        public int hospital_id { get; set; }
        public string hospital_name { get; set; }
        public decimal total_claimed_rs { get; set; }
        public decimal total_sanctioned_rs { get; set; }
        public bool isHospitalAwardedCertificate { get; set; }
        public List<RCExpenseTypeDetailsItemsModel> request_claim_expense_items { get; set; }
    }
    public class RCExpenseTypeDetailsItemsModel
    {
        public int id { get; set; }
        public DateTime? from_date { get; set; }
        public DateTime? to_date { get; set; }
        public DateTime? only_date { get; set; }
        public int objection_code_id { get; set; }
        public int head_of_expense_id { get; set; }
        public string head_of_expense_name { get; set; }
        public string head_of_expense_code { get; set; }
        public string bill_no { get; set; }
        public decimal? amt_claimed_rs { get; set; }
        public decimal? amt_sanctioned_rs { get; set; }
        public string remark { get; set; }

        public string description { get; set; }
        public string ma_remark { get; set; } = string.Empty;
        public int? no_of_teeth { get; set; }
        public int days { get; set; }

    }

}