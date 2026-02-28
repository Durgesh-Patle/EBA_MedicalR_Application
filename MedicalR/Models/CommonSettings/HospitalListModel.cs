using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedicalR.Models.CommonSettings
{
    public class HospitalListModel
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public int Pincode { get; set; }
        public string Address { get; set; }
        public bool Status { get; set; }
        public string hosp_bank_name { get; set; }
        public string hosp_bank_accno { get; set; }
        public string bank_ifsc_code { get; set; }
        public Boolean is_credit_facility { get; set; }
        public Boolean is_cashless { get; set; }
        public string email1 { get; set; }
        public string email2 { get; set; }
        public string mens_package { get; set; }
        public string womens_package { get; set; }

    }
}