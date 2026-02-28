using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedicalR.Models.MedicalR
{
    public class BatchCreationModel
    {
        public int id { get; set; }
        public int employee_id { get; set; }
        public string employeecode { get; set; }
        public int claim_request_id { get; set; }
        public string batch_number { get; set; }
        public DateTime batch_created_date { get; set; }
        public string appln_no { get; set; }
        public string employeename { get; set; }
        public decimal amt_claimed_rs { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string family_relation_code { get; set; }
        public DateTime doctor_sent_date { get; set; }
        public DateTime doctor_recieve_date { get; set; }
        public int patient_id { get; set; }
        public string patient_name { get; set; }
        public bool is_batch_created { get; set; } = false;
        public int created_by { get; set; }
        public decimal total_claimed_rs { get; set; }
        public string band_level { get; set; }
        public string band { get; set; }
        public string treatment_type { get; set; }
    }
    public class UpdateBatchModel
    {
        public int id { get; set; }
        public int claim_request_id { get; set; }
        public string claim_request_ids { get; set; }
        public string batch_no { get; set; }
        public DateTime batch_created_date { get; set; }
        public string batch_status { get; set; } = string.Empty;
        public int doctor_id { get; set; }
        public DateTime? doctor_sent_date { get; set; }
        public DateTime? doctor_received_date { get; set; }
        public int application_count { get; set; }
    }
    public class BatchMasterModel
    {
        public int id { get; set; }
        public string band_level { get; set; }
        public string band { get; set; }
        public string treatment_type { get; set; }
        public int last_number { get; set; }
        public DateTime created_date { get; set; }
    }

}