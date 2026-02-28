using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedicalR.Models.MedicalBillsException
{
    public class HospitilizationDHRD
    {
        public int id { get; set; }
        public string appln_no { get; set; }
        public DateTime date_of_request { get; set; }
        public string band_group { get; set; }
        public int patient_id { get; set; }
        public string relationship { get; set; }
        public string nature_of_illness { get; set; }
        public string treatment_type { get; set; }
        public DateTime from_date { get; set; }
        public DateTime to_date { get; set; }
        public string name_of_hospital { get; set; }
        public string bill_no { get; set; }
        public DateTime bill_date { get; set; }
        public string wheather_hospital_awarded_i_t_rebate { get; set; }
        public decimal? medical_claim_forwd_dhrd_inrs { get; set; }
        public decimal? amount_sanction_by_tmo_inrs { get; set; }
        public decimal? balance_amount { get; set; }
        public DateTime send_date_to_tmo { get; set; }
        public DateTime received_date_from_tmo { get; set; }
        public string status { get; set; }
        public int emplid { get; set; }
        public int employee_id { get; set; }
        public string employee_code { get; set; }
        public string employeename { get; set; }
        public string band_level { get; set; }
        public int hospital_id { get; set; }
    }
}