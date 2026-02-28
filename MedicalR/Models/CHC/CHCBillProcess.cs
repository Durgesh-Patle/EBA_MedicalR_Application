using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedicalR.Models.CHC
{
    public class ExportParaModel
    {
        public string lot { get; set; }
        public string lotno { get; set; }
        public decimal year { get; set; }
    }
    public class CHCBillProcess
    {
        public int srno { get; set; }
        public int id { get; set; }
        public int emplid { get; set; }
        public string employee_code { get; set; }
        public string employee_name { get; set; }
        public string date_of_checkup { get; set; }
        public int chc_centerid { get; set; }
        public string hosname { get; set; }
        public string amount { get; set; }
        public string name_of_spouse { get; set; }
        public string spouse_dt_of_checkup { get; set; }
        public int spouse_chc_centerid { get; set; }
        public string spouse_hosname { get; set; }
        public string spouse_amount { get; set; }
        public string total { get; set; }
        public string relationship { get; set; }

        public string employeecode { get; set; }

        public string phosname { get; set; }
        public string status { get; set; }
        public string bill_no { get; set; }
        public string tax_in_rs { get; set; }
        public string spouse_bill_no { get; set; }
        public string spouse_tax_in_rs { get; set; }
        public string lot_no { get; set; }
        public DateTime lot_date { get; set; }
        public string cheque_no { get; set; }
        public DateTime cheque_date { get; set; }
        public bool Status2 { get; set; } = false;
        public string lotdate { get; set; }
        public bool CrsalAcc_status { get; set; } = false;
        public DateTime CR_DATE { get; set; }
        public bool saveforlater_status { get; set; } = false;
        public string hosp_bank_name { get; set; }

        public string hosp_bank_accno { get; set; }

        public string bank_ifsc_code { get; set; }
        public string address { get; set; }
        public string emp_bank_name { get; set; }
        public string emp_account_no { get; set; }
        public string emp_ifsc_code { get; set; }

    }
}