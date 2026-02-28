using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MedicalR.Models.MedicalR
{
    public class ProcessPaymentModel
    {
        public int id { get; set; }
        public string batch_no { get; set; }
        public int employeeid { get; set; }
        public string employeecode { get; set; }
        public string employeename { get; set; }
        public int claim_request_id { get; set; }
        public string appln_no { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime date_of_request { get; set; }
        public decimal total_claimed_rs { get; set; }
        public decimal approved_amount { get; set; }
        public DateTime batch_date { get; set; }
        public DateTime doctor_received_date { get; set; }
        public string account_number { get; set; }
        public decimal total_sanctioned_rs { get; set; }
        public string lot_no { get; set; }
        public DateTime date_of_payment { get; set; }
        public bool is_process { get; set; } = false;
        public string email { get; set; }
        public string cc_email_ids { get; set; }
        public string email_template { get; set; }
        public string objection_code { get; set; }
        public string objection_remark { get; set; }
    }

    public class SavePaymentRequest
    {
        public List<ProcessPaymentModel> modelPayment { get; set; }
        public string  date_of_payment { get; set; }  
    }
}