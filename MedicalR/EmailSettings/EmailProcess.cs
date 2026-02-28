using Dapper;
using MedicalR.CustomHelper;
using MedicalR.Models;
using MedicalR.Models.CHC;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using MedicalR.EmailSettings;

namespace MedicalR.EmailSettings
{
    public class EmailProcess
    {
        //  email for retirment module

        public static bool SendMail_CHC_Applied(int chc_request_id, string relationship, NpgsqlConnection con)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("p_chc_request_id", chc_request_id);
                CHCRequest email_config = con.Query<CHCRequest>("select * from mdcl_sp_get_email_chc_applied_cases(:p_chc_request_id)", parameters).FirstOrDefault();

                CommonHelper.write_log("chc_request_id :" + chc_request_id);
                EmailModel eml_model = new EmailModel();
                // string val_emailbody = EmailHelper.GetEmailBody("RetVHSPayment_Confirmation.html");


                string val_emailbody = email_config.email_template;
                val_emailbody = val_emailbody.Replace("#{employee_name}", string.IsNullOrWhiteSpace(email_config.employeename) ? "" : email_config.employeename);
                val_emailbody = val_emailbody.Replace("#{employee_code}", string.IsNullOrWhiteSpace(email_config.employeecode) ? "" : email_config.employeecode);

                val_emailbody = val_emailbody.Replace("#{dob}", string.IsNullOrWhiteSpace(email_config.date_of_birth.ToString()) ? "" : email_config.date_of_birth.ToString("dd/MM/yyyy"));
                val_emailbody = val_emailbody.Replace("#{age}", string.IsNullOrWhiteSpace(email_config.age) ? "" : email_config.age);
                val_emailbody = val_emailbody.Replace("#{location}", string.IsNullOrWhiteSpace(email_config.attribute_type_unit_desc) ? "" : email_config.attribute_type_unit_desc);
                val_emailbody = val_emailbody.Replace("#{mob_no}", string.IsNullOrWhiteSpace(email_config.mob_no) ? "" : email_config.mob_no);
                val_emailbody = val_emailbody.Replace("#{office_no}", string.IsNullOrWhiteSpace(email_config.office_tel_no) ? "" : email_config.office_tel_no);
                val_emailbody = val_emailbody.Replace("#{date_checkup}", string.IsNullOrWhiteSpace(email_config.date_of_checkup.ToString()) ? "" : email_config.date_of_checkup.ToString("dd/MM/yyyy"));
                val_emailbody = val_emailbody.Replace("#{chc_name}", string.IsNullOrWhiteSpace(email_config.hosname) ? "" : email_config.hosname);

                val_emailbody = val_emailbody.Replace("#{name_of_sps}", string.IsNullOrWhiteSpace(email_config.name_of_spouse) ? "" : email_config.name_of_spouse);
                val_emailbody = val_emailbody.Replace("#{dob_sps}", string.IsNullOrWhiteSpace(email_config.dob_spouse.ToString()) ? "" : email_config.dob_spouse.ToString("dd/MM/yyyy"));

                val_emailbody = val_emailbody.Replace("#{sps_age}", string.IsNullOrWhiteSpace(email_config.spouse_age) ? "" : email_config.spouse_age);
                val_emailbody = val_emailbody.Replace("#{spsdtofcheck}", string.IsNullOrWhiteSpace(email_config.spouse_dt_of_checkup.ToString()) ? "" : email_config.spouse_dt_of_checkup.ToString("dd/MM/yyyy"));

                val_emailbody = val_emailbody.Replace("#{sps_chc}", string.IsNullOrWhiteSpace(email_config.phosname) ? "" : email_config.phosname);

                // val_emailbody = val_emailbody.Replace("#{yearto}", bll.yearto);
                eml_model.module_code = "CHC_APPL";
                eml_model.id = chc_request_id;

                eml_model.email_subject = GetEmailBodyWithPlan(email_config.mens_package, email_config.womens_package, email_config.self_womens_package, relationship);
                eml_model.email_body = val_emailbody;
                eml_model.to_emailids = new List<string>();

                if (!string.IsNullOrEmpty(email_config.email1))
                {
                    eml_model.to_emailids.Add(email_config.email1);
                }
                if (!string.IsNullOrEmpty(email_config.email2))
                {
                    eml_model.to_emailids.Add(email_config.email2);
                }
                eml_model.cc_emailids = new List<string>();

                if (!string.IsNullOrEmpty(email_config.cc_email_ids))
                {
                    foreach (string cc_ids in email_config.cc_email_ids.Split(',').AsList())
                    {
                        eml_model.cc_emailids.Add(cc_ids);
                    }
                }
                CommonHelper.write_log("above send mail");
                if (EmailHelper.SendEmail(eml_model, "CONFIRM"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                return false;
            }
        }

        public static string GetEmailBodyWithPlan(string mens_package, string womens_package, string self_womens_package, string relationship)
        {
            string val_eml_body = "Request for Comprehensive Health Checkup";

            string val_gender = UserManager.User.gender.ToLower();

            mens_package = string.IsNullOrWhiteSpace(mens_package) ? "" : mens_package;
            womens_package = string.IsNullOrWhiteSpace(womens_package) ? "" : womens_package;
            self_womens_package = string.IsNullOrWhiteSpace(self_womens_package) ? "" : self_womens_package;

            if (!string.IsNullOrWhiteSpace(mens_package) && !string.IsNullOrWhiteSpace(womens_package) && relationship.ToLower() == "self and spouse")
            {
                val_eml_body = "Request for Comprehensive Health Checkup (" + mens_package + "/" + womens_package + ")";
            }
            if (!string.IsNullOrWhiteSpace(mens_package) && UserManager.User.gender.ToLower() == "male" && relationship.ToLower() == "self")
            {
                val_eml_body = "Request for Comprehensive Health Checkup (" + mens_package + ")";
            }
            if (!string.IsNullOrWhiteSpace(womens_package) && val_gender == "male" && relationship.ToLower() == "spouse")
            {
                val_eml_body = "Request for Comprehensive Health Checkup (" + womens_package + ")";
            }
            if (!string.IsNullOrWhiteSpace(self_womens_package) && UserManager.User.gender.ToLower() == "female" && relationship.ToLower() == "self")
            {
                val_eml_body = "Request for Comprehensive Health Checkup (" + self_womens_package + ")";
            }
            if (!string.IsNullOrWhiteSpace(mens_package) && val_gender == "female" && relationship.ToLower() == "spouse")
            {
                val_eml_body = "Request for Comprehensive Health Checkup (" + mens_package + ")";
            }
            return val_eml_body;
        }
        public static bool SendMail_CHC_Approved(int chc_request_id, NpgsqlConnection con)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("p_chc_request_id", chc_request_id);
                parameters.Add("p_status", "Approved");
                CHCRequest email_config = con.Query<CHCRequest>("select * from mdcl_sp_get_email_chc_approved_cases(:p_chc_request_id,:p_status)", parameters).FirstOrDefault();

                CommonHelper.write_log("chc_request_id :" + chc_request_id);
                EmailModel eml_model = new EmailModel();
                // string val_emailbody = EmailHelper.GetEmailBody("RetVHSPayment_Confirmation.html");
                CommonHelper.write_log("email_config.chc_centerid :" + email_config.chc_centerid + " | email_config.chc_centid :" + email_config.chc_centid);
                if ((email_config.chc_centerid != 4 && email_config.chc_centid == 0) || (email_config.chc_centerid == 0 && email_config.chc_centid != 4))
                {


                    string val_chc_letter = email_config.chc_approved_letter;
                    val_chc_letter = val_chc_letter.Replace("#{self_solutation}", string.IsNullOrWhiteSpace(email_config.salutation) ? "" : email_config.salutation);
                    val_chc_letter = val_chc_letter.Replace("#{emp_name}", string.IsNullOrWhiteSpace(email_config.employeename) ? "" : email_config.employeename);
                    val_chc_letter = val_chc_letter.Replace("#{emp_code}", string.IsNullOrWhiteSpace(email_config.employeecode) ? "" : email_config.employeecode);
                    val_chc_letter = val_chc_letter.Replace("#{age}", string.IsNullOrWhiteSpace(email_config.age) ? "" : email_config.age);
                    val_chc_letter = val_chc_letter.Replace("#{date_of_checkup}", string.IsNullOrWhiteSpace(email_config.date_of_checkup.ToString()) ? "" : email_config.date_of_checkup.ToString("dd/MM/yyyy"));
                    if (email_config.gender == "Male" && email_config.relationship == "Self and Spouse")
                    {
                        val_chc_letter = val_chc_letter.Replace("#{relation}", "His wife Mrs.");
                    }
                    if (email_config.gender == "Female" && email_config.relationship == "Self and Spouse")
                    {
                        val_chc_letter = val_chc_letter.Replace("#{relation}", "Her Husband Mr.");
                    }
                    val_chc_letter = val_chc_letter.Replace("#{spouse_name}", string.IsNullOrWhiteSpace(email_config.name_of_spouse) ? "" : email_config.name_of_spouse);
                    val_chc_letter = val_chc_letter.Replace("#{spouse_date_of_checkup}", string.IsNullOrWhiteSpace(email_config.spouse_dt_of_checkup.ToString()) ? "" : email_config.spouse_dt_of_checkup.ToString("dd/MM/yyyy"));
                    val_chc_letter = val_chc_letter.Replace("#{spouse_age}", string.IsNullOrWhiteSpace(email_config.spouse_age) ? "" : email_config.spouse_age);

                    val_chc_letter = val_chc_letter.Replace("#{finacial_year}", DateTime.Now.Year + "-" + DateTime.Now.AddYears(1).ToString().Substring(2, 2));
                    val_chc_letter = val_chc_letter.Replace("#{letter_date}", DateTime.Now.ToString("dd/MM/yyyy"));
                    val_chc_letter = val_chc_letter.Replace("#{chc_center}", string.IsNullOrWhiteSpace(email_config.hosname) ? "" : email_config.hosname);
                    val_chc_letter = val_chc_letter.Replace("#{address}", string.IsNullOrWhiteSpace(email_config.sf_address) ? "" : email_config.sf_address);
                    val_chc_letter = val_chc_letter.Replace("#{city}", string.IsNullOrWhiteSpace(email_config.sf_city) ? "" : email_config.sf_city);
                    val_chc_letter = val_chc_letter.Replace("#{pincode}", string.IsNullOrWhiteSpace(email_config.sf_pincode.ToString()) ? "" : email_config.sf_pincode.ToString());

                    eml_model.eml_attachemet = CommonHelper.Convert2(val_chc_letter);
                }
                else
                {
                    eml_model.eml_attachemet = null;
                }

                eml_model.module_code = "CHC_APPR";
                eml_model.id = chc_request_id;
                eml_model.email_subject = "Comprehensive Health Checkup - Approved by DOAA";
                eml_model.email_body = email_config.email_template;
                eml_model.to_emailids = new List<string>();

                if (!string.IsNullOrEmpty(email_config.email1))
                {
                    eml_model.to_emailids.Add(email_config.email1);
                }
                if (!string.IsNullOrEmpty(email_config.email2))
                {
                    eml_model.to_emailids.Add(email_config.email2);
                }
                if (!string.IsNullOrEmpty(email_config.emp_email))
                {
                    eml_model.to_emailids.Add(email_config.emp_email);
                }
                eml_model.cc_emailids = new List<string>();

                if (!string.IsNullOrEmpty(email_config.cc_email_ids))
                {
                    foreach (string cc_ids in email_config.cc_email_ids.Split(',').AsList())
                    {
                        eml_model.cc_emailids.Add(cc_ids);
                    }
                }
                CommonHelper.write_log("above send mail");
                if (EmailHelper.SendEmail(eml_model, "CONFIRM"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                return false;
            }
        }

        public static bool SendMail_CHC_Rejected(int chc_request_id, NpgsqlConnection con)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("p_chc_request_id", chc_request_id);
                parameters.Add("p_status", "Rejected");
                CHCRequest email_config = con.Query<CHCRequest>("select * from mdcl_sp_get_email_chc_rejected_cases(:p_chc_request_id,:p_status)", parameters).FirstOrDefault();

                CommonHelper.write_log("chc_request_id :" + chc_request_id);
                EmailModel eml_model = new EmailModel();
                // string val_emailbody = EmailHelper.GetEmailBody("RetVHSPayment_Confirmation.html");
                CommonHelper.write_log("email template :" + email_config.email_template);
                string val_chc_rejected = email_config.email_template;
                val_chc_rejected = val_chc_rejected.Replace("#{employee_name}", string.IsNullOrWhiteSpace(email_config.employeename) ? "" : email_config.employeename);
                val_chc_rejected = val_chc_rejected.Replace("#{employee_code}", string.IsNullOrWhiteSpace(email_config.employeecode) ? "" : email_config.employeecode);
                val_chc_rejected = val_chc_rejected.Replace("#{dob}", string.IsNullOrWhiteSpace(email_config.date_of_birth.ToString()) ? "" : email_config.date_of_birth.ToString("dd/MM/yyyy"));
                val_chc_rejected = val_chc_rejected.Replace("#{age}", string.IsNullOrWhiteSpace(email_config.age) ? "" : email_config.age);
                val_chc_rejected = val_chc_rejected.Replace("#{location}", string.IsNullOrWhiteSpace(email_config.attribute_type_unit_desc) ? "" : email_config.attribute_type_unit_desc);
                val_chc_rejected = val_chc_rejected.Replace("#{mob_no}", string.IsNullOrWhiteSpace(email_config.mob_no) ? "" : email_config.mob_no);
                val_chc_rejected = val_chc_rejected.Replace("#{office_no}", string.IsNullOrWhiteSpace(email_config.office_tel_no) ? "" : email_config.office_tel_no);
                val_chc_rejected = val_chc_rejected.Replace("#{date_checkup}", string.IsNullOrWhiteSpace(email_config.date_of_checkup.ToString()) ? "" : email_config.date_of_checkup.ToString("dd/MM/yyyy"));
                val_chc_rejected = val_chc_rejected.Replace("#{chc_name}", string.IsNullOrWhiteSpace(email_config.hosname) ? "" : email_config.hosname);
                val_chc_rejected = val_chc_rejected.Replace("#{name_of_sps}", string.IsNullOrWhiteSpace(email_config.name_of_spouse) ? "" : email_config.name_of_spouse);
                val_chc_rejected = val_chc_rejected.Replace("#{dob_sps}", string.IsNullOrWhiteSpace(email_config.dob_spouse.ToString()) ? "" : email_config.dob_spouse.ToString("dd/MM/yyyy"));
                val_chc_rejected = val_chc_rejected.Replace("#{sps_age}", string.IsNullOrWhiteSpace(email_config.spouse_age) ? "" : email_config.spouse_age);
                val_chc_rejected = val_chc_rejected.Replace("#{spsdtofcheck}", string.IsNullOrWhiteSpace(email_config.spouse_dt_of_checkup.ToString()) ? "" : email_config.spouse_dt_of_checkup.ToString("dd/MM/yyyy"));
                val_chc_rejected = val_chc_rejected.Replace("#{sps_chc}", string.IsNullOrWhiteSpace(email_config.phosname) ? "" : email_config.phosname);
                val_chc_rejected = val_chc_rejected.Replace("#{rejection_remark}", string.IsNullOrWhiteSpace(email_config.remark) ? "" : email_config.remark);

                eml_model.module_code = "CHC_REJECT";
                eml_model.id = chc_request_id;
                eml_model.email_subject = "Comprehensive Health Checkup - Rejected by DOAA";
                eml_model.email_body = val_chc_rejected;
                eml_model.to_emailids = new List<string>();

                if (!string.IsNullOrEmpty(email_config.emp_email))
                {
                    eml_model.to_emailids.Add(email_config.emp_email);
                }
                if (!string.IsNullOrEmpty(email_config.email1))
                {
                    eml_model.to_emailids.Add(email_config.email1);
                }
                if (!string.IsNullOrEmpty(email_config.email2))
                {
                    eml_model.to_emailids.Add(email_config.email2);
                }
                if (!string.IsNullOrEmpty(email_config.emp_email))
                {
                    eml_model.to_emailids.Add(email_config.emp_email);
                }
                eml_model.cc_emailids = new List<string>();

                if (!string.IsNullOrEmpty(email_config.cc_email_ids))
                {
                    foreach (string cc_ids in email_config.cc_email_ids.Split(',').AsList())
                    {
                        eml_model.cc_emailids.Add(cc_ids);
                    }
                }
                if (EmailHelper.SendEmail(eml_model, "CONFIRM"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                return false;
            }
        }
        public static bool SendMail_MedicalR_Sanctions(int request_id, NpgsqlConnection con)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("p_chc_request_id", request_id);
                parameters.Add("p_status", "Rejected");
                CHCRequest email_config = con.Query<CHCRequest>("select * from mdcl_sp_get_email_chc_approved_cases(:p_chc_request_id,:p_status)", parameters).FirstOrDefault();

                EmailModel eml_model = new EmailModel();
                // string val_emailbody = EmailHelper.GetEmailBody("RetVHSPayment_Confirmation.html");
                //string val_chc_rejected = email_config.email_template;

                eml_model.module_code = "CHC_REJECT";
                eml_model.id = request_id;
                eml_model.email_subject = "Comprehensive Health Checkup - Rejected by DOAA";
                eml_model.email_body = email_config.email_template;
                eml_model.to_emailids = new List<string>();

                if (!string.IsNullOrEmpty(email_config.email1))
                {
                    eml_model.to_emailids.Add(email_config.email1);
                }
                if (!string.IsNullOrEmpty(email_config.email2))
                {
                    eml_model.to_emailids.Add(email_config.email2);
                }
                eml_model.cc_emailids = new List<string>();

                if (!string.IsNullOrEmpty(email_config.cc_email_ids))
                {
                    foreach (string cc_ids in email_config.cc_email_ids.Split(',').AsList())
                    {
                        eml_model.cc_emailids.Add(cc_ids);
                    }
                }
                if (EmailHelper.SendEmail(eml_model, "CONFIRM"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                return false;
            }
        }
    }
}