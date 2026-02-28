using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedicalR.Models.MedicalR
{
    public class MedicalPaymentDetailsModel
    {
        public string employeecode { get; set; }
        public int employee_id { get; set; }
        public string employeename { get; set; }
        public int patient_id { get; set; }
        public string patient_name { get; set; }
        public string appln_no { get; set; }
        public int id { get; set; }
        public string amt_claimed_rs { get; set; }
        public string remark { get; set; }
        public decimal total_claimed_rs { get; set; }
        public string family_relation_code { get; set; }
        public string treatment_type { get; set; }
    }
}