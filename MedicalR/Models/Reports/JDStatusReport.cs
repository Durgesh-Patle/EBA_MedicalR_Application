using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MedicalR.Models.Reports
{
    public class JDStatusReport
    {
        public string client_name { get; set; }
        public int client_ID { get; set; }
        public int job_ID { get; set; }
        public string job_Title { get; set; }
        public List<JobInterviewDetail> job_Interview { get; set; }
    }
    public class JobInterviewDetail
    {
        public string stage_Name { get; set; }
        public string owner { get; set; }
        public int selected_Candidate { get; set; }
        public int rejected_Candidate { get; set; }
        public int onhold_Candidate { get; set; }
        public int total_Candidate { get; set; }
    }

    public class MedicalRDetails
    {
        public int id { get; set; }
        public string appln_no { get; set; }
        public string employeename { get; set; }
        public string employeecode { get; set; }
        public DateTime? lot_date { get; set; }
        public string band { get; set; }
        public string lot_no { get; set; }
        public DateTime? sanction_date { get; set; }
        public string batch_no { get; set; }
        public DateTime? batch_date { get; set; }
        public DateTime? sent_to_tmo { get; set; }
        public DateTime? received_from_tmo { get; set; }
        public string patient_name { get; set; }
        public int treatment_id { get; set; }
        public string treatment_type { get; set; }
        public DateTime from_date { get; set; }
        public DateTime to_date { get; set; }

        public int doctor_id { get; set; }
        public string doctor_name { get; set; }
        public int batch_id { get; set; }
        public string payment_status { get; set; }

        public string sanction_amount { get; set; }
        public string payment_amount { get; set; }
        public string total_sanction { get; set; }
        public string total_claim { get; set; }

        public DateTime? date_of_request { get; set; }
        public DateTime? bill_date { get; set; }
        public int claim_amount { get; set; }
        [DisplayName("sanction_amount")]
        public int amount_sanction_by_tmo_inrs { get; set; }
        public int balance_amount { get; set; }
        public string status { get; set; }
        public string report_status { get; set; }
    }
    public class ReportDownloadParaModel
    {
        public DateTime? from_date { get; set; }
        public DateTime? to_date { get; set; }
        public string band { get; set; }
        public int treatment_type { get; set; }
        public int doctor_id { get; set; }
        public string batch_no { get; set; }
        public string employeecode { get; set; }
        public string lot_no { get; set; }
        public string payment_status { get; set; }
    }
}