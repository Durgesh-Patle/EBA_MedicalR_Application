using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MedicalR.Models.MedicalR
{
    public class EditCheckingBillsModel
    {
        public int batch_id { get; set; }
        public string batch_no { get; set; }
        public string employeecode { get; set; }
        public string employeename { get; set; }
        public int claim_request_id { get; set; }
        public string appln_no { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime date_of_request { get; set; }
        public decimal total_claimed_rs { get; set; }
        public decimal total_sanctioned_rs { get; set; }
        public DateTime batch_date { get; set; }
        public DateTime dr_receive_date { get; set; }
        public int head_of_expense_id { get; set; }
        public string head_of_expenses { get; set; }
        public string amt_claimed_rs { get; set; }
        public string bill_no { get; set; }
        public string remark { get; set; }
        public string patient_name { get; set; }
        public string family_relation_code { get; set; }
        public string treatment_type { get; set; }
        public string expense_type { get; set; }
        public int employee_id { get; set; }
    }
}