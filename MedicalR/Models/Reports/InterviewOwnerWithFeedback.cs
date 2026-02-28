using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedicalR.Models.Reports
{
    public class InterviewOwnerWithFeedback
    {
        public int InterviewID { get; set; }
        public string InterviewName { get; set; }
        public int JobID { get; set; }

        public string Time { get; set; }
        public string Type { get; set; }
        public string CandidateName { get; set; }
        public int CandidateID { get; set; }
        public string ClientName { get; set; }
        public int ClientID { get; set; }
     
        public string Designation { get; set; }
        public string Status { get; set; }
        
    }
}