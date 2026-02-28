using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedicalR.Models.MedicalR
{
    public class EmpFamilydetail
    {
        public int employeeid { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string family_relation_code { get; set; }
        public int family_relation_id { get; set; }
    }
}