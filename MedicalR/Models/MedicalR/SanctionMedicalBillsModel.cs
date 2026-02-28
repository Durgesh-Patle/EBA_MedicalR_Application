using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MedicalR.Models.MedicalR
{
    public class SanctionMedicalBillsModel
    {
        public int batch_id { get; set; }
        public string employeecode { get; set; }
        public string employeename { get; set; }
        public string batch_no { get; set; }
        public int claim_request_id { get; set; }
        public string appln_no { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime date_of_request { get; set; }
        public string total_claimed_rs { get; set; }

        public DateTime batch_date { get; set; }
        public DateTime docter_received_date { get; set; }
        public decimal approved_amount { get; set; }
        public long application_count { get; set; }
        public string status { get; set; }
        public int employee_id { get; set; }
        public decimal total_sanctioned_rs { get; set; }
        public string empl_class { get; set; }
        public string bank_name { get; set; }
        public string account_no { get; set; }
        public string trans_status { get; set; }
        public string band { get; set; }
        public int total_objection_count { get; set; }
        public decimal total_objection_amt { get; set; }
        public int total_sanctioned_count { get; set; }
        public decimal total_sanctioned_rs_lotwise { get; set; }
        public bool is_sanctioned { get; set; }
        public bool is_objection { get; set; }
        public DateTime hewitt_last_download_date { get; set; }
        public DateTime hewitt_to_download_date { get; set; } = DateTime.Now.AddDays(-1);
        public string lot_no { get; set; }
        public DateTime payment_date { get; set; }
        public DateTime? lot_generate_date { get; set; }
    }
}