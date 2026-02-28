using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedicalR.Models.RetiredEmployee
{
    public class RempDemiseModel
    {
        public int Id { get; set; }
        public string EmpId { get; set; }
        public string Dempname { get; set; }
        public string name { get; set; }

        public DateTime Ddate { get; set; }
        public string Dcertificate { get; set; }
        public string Lreceived { get; set; }
        public string Sname { get; set; }
        public string Sbankname { get; set; }      
        public string Sifsccode { get; set; }
        public string Saccno { get; set; }
        public string Smobile { get; set; }
        public string Semail { get; set; }
        public bool Status { get; set; }
    }
}