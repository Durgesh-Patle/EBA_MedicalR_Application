using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedicalR.Models.MedicalBillsException
{
    public class AdditionalSanction
    {
        public int id { get; set; }
        public string appln_no { get; set; }
        public DateTime date_of_request { get; set; }
        public string department { get; set; }
        public string band_group { get; set; }
        public string name_of_patient { get; set; }
        public string relationship { get; set; }
        public string nature_of_illness { get; set; }
        public string treatment_type { get; set; }
        public string total_claimed_rs { get; set; }
        public string sanction_and_paid_rs { get; set; }
        public string addl_sanction_rs { get; set; }
        public string payment_remark { get; set; }
        public string no_of_enclosure { get; set; }
        public int emplid { get; set; }
        public string EmpCode { get; set; }
    }
    public class HospitalizationFCModel
    {
        public int id { get; set; }
        public int employee_id { get; set; }
        public string employee_code { get; set; } = string.Empty;
        public string employeename { get; set; } = string.Empty;
        public string band_level { get; set; } = string.Empty;
        public int patient_id { get; set; }
        public string relationship { get; set; } = string.Empty;
        public string nature_of_illness { get; set; } = string.Empty;
        public string treatment_type { get; set; } = "Hospitality";
        public DateTime? from_date { get; set; }
        public DateTime? to_date { get; set; }
        public int hospital_id { get; set; }
        public string bill_no { get; set; } = string.Empty;
        public DateTime? bill_date { get; set; }
        public string is_rebate_certificate { get; set; } = string.Empty;
        public decimal? gross_bill_amt_rs { get; set; }
        public decimal? amt_paid_to_hospital_rs { get; set; }
        public decimal? amt_approved_sanc_tms_rs { get; set; }
        public decimal? amt_approved_sanc_maf_rs { get; set; }
        public decimal? addl_sanction_rs { get; set; }
        public decimal? amt_tobe_recover_frm_empl { get; set; }
        public int? recovery_installments_period { get; set; }
        public DateTime? send_date_to_tmo { get; set; }
        public DateTime? received_date_from_tmo { get; set; }
        public bool is_save_for_later { get; set; } = false;
        public string appln_no { get; set; } = string.Empty;
        public string hospi_email1 { get; set; }
        public string hospi_email2 { get; set; }
        public string empl_email { get; set; }
        public string cc_email_ids { get; set; }
        public string cf_letter_template { get; set; }
        public string email_subject { get; set; }
        public string email_body { get; set; }
        public string is_send_letter { get; set; }
        public string btn_check_points { get; set; }
        public string ufc_name { get; set; }
        public DateTime letter_date { get; set; }
        public string send_letter_status { get; set; }
    }

    public class CF_SEND_LETTERMODEL
    {
        public string employee_code { get; set; }
        public string employeename { get; set; }
        public string designation { get; set; }
        public string patient_name { get; set; }
        public string family_relation_code { get; set; }
        public string nature_of_illness { get; set; }
        public string hospital_name { get; set; }
        public string hosp_address { get; set; }
        public string hosp_city { get; set; }
        public int hosp_pincode { get; set; }
        public string hospi_email1 { get; set; }
        public string hospi_email2 { get; set; }
        public string empl_email { get; set; }
        public string cc_email_ids { get; set; }
        public string cf_letter_template { get; set; }
        public string email_subject { get; set; }
        public string email_body { get; set; }

        public DateTime letter_date { get; set; }

    }
    public class DownloadBankAdviseOfficeNoteModel
    {
        public int id { get; set; }
        public string employeecode { get; set; }
        public string employeename { get; set; }
        public string appln_no { get; set; }
        public string hosp_name { get; set; }
        public string hosp_bank_name { get; set; }
        public string hosp_bank_accno { get; set; }
        public string bank_ifsc_code { get; set; }
        public decimal total_claimed_amt { get; set; }
        public decimal amt_sanc_tms { get; set; }
        public decimal amt_sanc_maf { get; set; }
        public decimal total_sanc { get; set; }
        public string name_of_hospital { get; set; }
        public string bank_name { get; set; }
        public string account_no { get; set; }
        public string ifsc_code { get; set; }
        public decimal amount_sanction_by_tmo_inrs { get; set; }

    }  

   











}