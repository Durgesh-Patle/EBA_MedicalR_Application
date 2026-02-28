using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedicalR.Models.Reports
{
    public class CompanyUsageReport
    {
        public string company_Name { get; set; }
        public string user_count { get; set; }
        public string client_count { get; set; }
        public string client_poc_count { get; set; }
        public string job_running_count { get; set; }
        public string job_onhold_count { get; set; }
        public string job_close_count { get; set; }
        public string job_nbtc_count { get; set; }
        public string candidate_count { get; set; }
        public string interview_count { get; set; }
    }
}