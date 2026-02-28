using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedicalR.Models.RetiredEmployee
{
    public class ExportParaModel
    {
        public DateTime lot { get; set; }
        public string lotno { get; set; }
        public decimal year { get; set; }
    }
    public class RetiredempTransModel
    {
        public int Id { get; set; }
        public string Empcd { get; set; }
        public string Name { get; set; } = string.Empty;
        //public string name { get; set; }
        public string Curryear { get; set; }
        public string Preyear { get; set; }
        public decimal Totalclaim { get; set;}
        public decimal Sancriamt { get; set; }
        public decimal reimburs_amt { get; set; }
        public decimal Balance { get; set; }      
        public string Remark { get; set; }
        public bool Status { get; set; } = false;
        public string yearfrom { get; set; }
        public string yearto { get; set; }
        public DateTime? req_date { get; set; }
        public decimal sanc_amt { get; set; }
        public bool is_paid { get; set; }
        public int bank_id { get; set; }
        public string bankname { get; set; } = string.Empty;
        public string accno { get; set; } = string.Empty;
        public string lot_no { get; set; } = string.Empty;
        public string ifsccode { get; set; } = string.Empty;
        public string option { get; set; }
        public DateTime lot_date { get; set; }
        public string disp_lot_date { get; set; } = string.Empty;
        public string paid_date { get; set; }
        public DateTime offndate { get; set; }
        public decimal year { get; set; }
        public List<emplSanc> EmplSanc_list { get; set; }
    }
    public class emplSanc
    {
        public string yearfrom { get; set; }
        public string yearto { get; set; }
        public decimal sanc_amt { get; set; }
        public string Empcd { get; set; }
        public string Name { get; set; } = string.Empty;
        public string bankname { get; set; } = string.Empty;
        public string accno { get; set; } = string.Empty;
        public string ifsccode { get; set; } = "";
        public string disp_lot_date { get; set; } = string.Empty;
        public string lot_no { get; set; } = string.Empty;
    }
}