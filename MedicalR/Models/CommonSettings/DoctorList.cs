using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedicalR.Models.CommonSettings
{
    public class DoctorList
    {
        public int id { get; set; }
        public string dr_name { get; set; }
        public string mobile_no { get; set; }
        public string designation { get; set; }
        public Boolean isactive { get; set; }
    }
}