using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedicalR.Models.MedicalBillsException
{
    public class HospitilizationCr
    {
        public int id { get; set; }
        public string appln_no { get; set; }
        public DateTime date_of_request { get; set; }
        public string band_group { get; set; }
        public string relationship { get; set; }
        public string nature_of_illness { get; set; }
        public string treatment_type { get; set; }
        public DateTime from_date { get; set; }
        public DateTime to_date { get; set; }
        public string name_of_hospital { get; set; }
        public string bill_no { get; set; }
        public DateTime bill_date { get; set; }
        public string wheather_hospital_awarded_i_t_rebate { get; set; }
        public string gross_bill_amount_in_rs { get; set; }
        public string amount_paid_to_hospital_in_rs { get; set; }
        public string amount_approved_sanction_tms_rs { get; set; }
        public string amount_approved_sanction_maf_rs { get; set; }
        public string addl_sanction_in_rs { get; set; }
        public string amount_to_recover_from_empl { get; set; }
        public string recovery_installment_period { get; set; }
        public string emi { get; set; }
        public DateTime send_date_to_tmo { get; set; }
        public DateTime received_date_from_tmo { get; set; }
        public int emplid { get; set; }

    }
}