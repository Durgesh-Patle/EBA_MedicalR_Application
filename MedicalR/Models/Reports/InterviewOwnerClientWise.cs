using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedicalR.Models.Reports
{
    public class InterviewOwnerClientWise
    {
        public string InterviewName { get; set; }
        public string Panel { get; set; }
        public string ClientName { get; set; }
        public int ClientID { get; set; }
        public int JobID { get; set; }
        public int Total { get; set; }
        public int Selected { get; set; }
        public int Rejected { get; set; }
    }
}