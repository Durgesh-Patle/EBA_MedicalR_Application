using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MedicalR.Models.CHC
{
    public class CHCRequest
    {
        public int id { get; set; }
        public int emplid { get; set; }
        public string employeename { get; set; }
        public string employeecode { get; set; }
        public string mob_no { get; set; }
        public string salutation { get; set; }
        public string gender { get; set; }
        public string emp_email { get; set; }
        public string office_tel_no { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime date_of_birth { get; set; }
        public string age { get; set; }
        public DateTime date_of_checkup { get; set; }
        public int chc_centerid { get; set; }
        public string hosname { get; set; }
        public string name_of_spouse { get; set; }
        public DateTime dob_spouse { get; set; }
        public string spouse_age { get; set; }
        public DateTime spouse_dt_of_checkup { get; set; }
        public int chc_centid { get; set; }
        public string phosname { get; set; }
        public string status { get; set; }
        public string relationship { get; set; }
        public string appointmentno_empl { get; set; }
        public string appointmentno_spouse { get; set; }
        public string created_by { get; set; }
        public int tot_row { get; set; }
        public int empl_age { get; set; }
        public int spouse_cur_age { get; set; }
        public string otherchc_empl { get; set; }
        public string otherchc_sps { get; set; }
        public string attribute_type_unit_desc { get; set; }
        public string email1 { get; set; }
        public string email2 { get; set; }
        public string email_template { get; set; }
        public string cc_email_ids { get; set; }
        public string chc_approved_letter { get; set; }
        public string sf_address { get; set; }
        public string sf_city { get; set; }
        public int sf_pincode { get; set; }

        public string s_address { get; set; }
        public string s_city { get; set; }
        public int s_pincode { get; set; }
        public string remark { get; set; } = string.Empty;
        public bool is_save_for_later { get; set; }
        public string emp_location { get; set; }
        public string mens_package { get; set; }
        public string womens_package { get; set; }
        public string self_womens_package { get; set; }
    }
}