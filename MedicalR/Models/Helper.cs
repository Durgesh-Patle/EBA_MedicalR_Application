using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dapper;
using System.Data;
using Npgsql;
using DocumentFormat.OpenXml.Spreadsheet;
using MedicalR.CustomHelper;
using System.Security.Cryptography;

namespace MedicalR.Models
{
    public class Helper
    {
        //readonly static string constr = "Host=localhost;port=5432;Username=postgres;Password=uti@123;Database=MedicalR;";
        public static List<DDLMODEL> LOAD_DROPDOWN_DATA(string query, DynamicParameters parameters)
        {
            List<DDLMODEL> list_data = new List<DDLMODEL>();
            DataTable dt = new DataTable();
            using (NpgsqlConnection con = new NpgsqlConnection(CustomHelper.CommonHelper.GetConnectionString))
            {
                con.Open();
                list_data = con.Query<DDLMODEL>(query, parameters).ToList();
            }
            return list_data;
        }

        public static List<DDLMODEL> GetEmployeeWise_table_ids()
        {
            List<DDLMODEL> list_data = new List<DDLMODEL>();
            DataTable dt = new DataTable();
            using (NpgsqlConnection con = new NpgsqlConnection(CustomHelper.CommonHelper.GetConnectionString))
            {
                try
                {
                    con.Open();
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("p_employee_id", UserManager.User.UserID);
                    list_data = con.Query<DDLMODEL>("select * from fn_get_employeewise_tbl_ids(:p_employee_id)", parameters).ToList();
                }
                catch (Exception ex)
                {
                    CommonHelper.write_log("error in GetEmployeeWise_table_ids() :" + ex.Message);
                }
            }
            return list_data;
        }
        public static string GenerateRandomKey(int length)
        {
            const string validChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            char[] chars = new char[length];

            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                byte[] data = new byte[length];
                crypto.GetBytes(data);

                for (int i = 0; i < length; i++)
                {
                    chars[i] = validChars[data[i] % validChars.Length];
                }
            }

            return new string(chars);
        }

        public static string GetCurrentFY()
        {
            DateTime today = DateTime.Now;

            // Define the financial year start month (e.g., April for India)
            int financialYearStartMonth = 4;

            int currentYear = today.Year;
            int financialYearStartYear;
            int financialYearEndYear;

            if (today.Month >= financialYearStartMonth)
            {
                // If current date is in or after the start month, financial year starts this year
                financialYearStartYear = currentYear;
                financialYearEndYear = currentYear + 1;
            }
            else
            {
                // If current date is before the start month, financial year started last year
                financialYearStartYear = currentYear - 1;
                financialYearEndYear = currentYear;
            }

            string financialYear = $"{financialYearStartYear}-{financialYearEndYear}";
            Console.WriteLine($"Current Financial Year: {financialYear}");
            return financialYear;
        }

    }
    public class DDLMODEL
    {
        public int id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string gender { get; set; }
    }
}