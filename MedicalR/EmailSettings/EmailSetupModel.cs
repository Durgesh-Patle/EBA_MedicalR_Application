using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedicalR.EmailSettings
{
    public class EmailSetupModel
    {
    }
    public class EmailConfigurationModel
    {
        public int id { get; set; }
        public string module_code { get; set; }
        public string email_category { get; set; }
        public int template_id { get; set; }
        public string template { get; set; }
        public string procedure_name { get; set; }
        public bool status { get; set; }
    }
    public class CHC_EmailModel
    {
        public int id { get; set; }
        public string employee_code { get; set; }
        public string employeename { get; set; }
        public string chc_email1 { get; set; }
        public string chc_email2 { get; set; }
        public DateTime date_of_checkup { get; set; }
        public string cc_email_ids { get; set; }
        public string emp_email { get; set; }
        public string yearfrom { get; set; }
        public string yearto { get; set; }

    }

    public class RetiredempTransModel
    {
        public int Id { get; set; }
        public string Empcd { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Curryear { get; set; }
        public string Preyear { get; set; }
        public decimal Totalclaim { get; set; }
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
        public string bankname { get; set; } = string.Empty;
        public string accno { get; set; } = string.Empty;
        public string lot_no { get; set; } = string.Empty;
        public string ifsccode { get; set; } = string.Empty;
        public string option { get; set; }
        public DateTime lot_date { get; set; }
        public string disp_lot_date { get; set; } = string.Empty;
        public DateTime offndate { get; set; }
        public string email { get; set; }
    }
    public class EmailModel
    {
        public int id { get; set; }
        public string attached_file_location { get; set; }
        public string attached_file { get; set; }
        public List<string> to_emailids { get; set; }
        public List<string> cc_emailids { get; set; }
        public List<string> attached_files { get; set; }
        public string file_name { get; set; }
        public string email_subject { get; set; }
        public string email_body { get; set; }
        public string module_code { get; set; }
        public byte[] eml_attachemet { get; set; }
        //public string yearfrom { get; set; }
        //public string yearto { get; set; }

    }
}