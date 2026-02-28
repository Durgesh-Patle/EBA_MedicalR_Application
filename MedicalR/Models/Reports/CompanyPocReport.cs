using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedicalR.Models.Reports
{
    public class CompanyPocReport
    {
        public string company_Name { get; set; }
        public string created_date { get; set; }
        public string status { get; set; }
        public string location { get; set; }
        public string company_poc { get; set; }
        public string company_poc_email { get; set; }
        public string company_poc_mobile { get; set; }
    }
}