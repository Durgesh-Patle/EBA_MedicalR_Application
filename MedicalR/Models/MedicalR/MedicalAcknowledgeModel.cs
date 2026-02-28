using System;

namespace MedicalR.Models.MedicalR
{
    public class MedicalAcknowledgeModel
    {
        public int id { get; set; }
        public string employeecode { get; set; }
        public int employee_id { get; set; }
        public int claim_request_id { get; set; }
        public int approval_id { get; set; }
        public DateTime acknowledge_date { get; set; }
        public string appln_no { get; set; }
        public string employeename { get; set; }
        public decimal amt_claimed_rs { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string family_relation_code { get; set; }

        public int patient_id { get; set; }
        public string patient_name { get; set; }

        public bool is_acknowledged { get; set; } = false;
        public decimal total_claimed_rs { get; set; }
        public string enclosure { get; set; }

        //public int id { get; set; }
        //public string Nature_of_illness { get; set; }
        //public int treatment_type_id { get; set; }
        //public string expense_type { get; set; }
        //public DateTime from_date { get; set; }
        //public DateTime to_date { get; set; }
        ////public int id { get; set; }
        //public string hosp_awarded_i_t_rebate { get; set; }
        //public string heads_of_expense { get; set; }
        //public string bill_no { get; set; }

        //public string remark { get; set; }
        //public string total_claimed_rs { get; set; }
        //public Boolean is_treatment_taken_out_of_head_quaters { get; set; }
        //public Array expenses { get; set; }
    }

}