using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MedicalR.Models.MedicalR
{
    public class TrackingMedicalBillsModel
    {
        public int employee_id { get; set; }
        public string employeecode { get; set; }
        public string employeename { get; set; }
        public int patient_id { get; set; }
        public string patient_name { get; set; }
        public string appln_no { get; set; }
        public int id { get; set; }
        public string amt_claimed_rs { get; set; }
        public string remark { get; set; }
        public string total_claimed_rs { get; set; }
        public string family_relation_code { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime date_of_request { get; set; }
        public List<EmpStatusmodel> EmplStatus_list { get; set; }

        public string batch_no { get; set; }
    }
    public class EmpStatusmodel
    {
        public DateTime date_of_request { get; set; }
        public string ack_officername { get; set; }
        public int approval_id { get; set; }
        public int activityid { get; set; }
        public string status_text { get; set; }
        public string employeecode { get; set; }
    }
}