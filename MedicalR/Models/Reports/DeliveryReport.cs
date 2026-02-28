using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedicalR.Models.Reports
{
    public class DeliveryReport
    {
        public string recruiter_Name { get; set; }
        public string client_Name { get; set; }
        public int client_ID { get; set; }
        public int job_ID { get; set; }
        public int rejected_Candidate { get; set; }
        public int selected_Candidate { get; set; }
        public int total_Candidate { get; set; }
    }
}