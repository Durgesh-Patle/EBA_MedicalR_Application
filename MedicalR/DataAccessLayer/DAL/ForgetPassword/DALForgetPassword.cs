using MedicalR.CustomHelper;
using MedicalR.DataAccessLayer.IDAL.MedicalR;
using Npgsql;
//using Retired_Emp.CustomHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace MedicalR.DataAccessLayer.DAL.MedicalR
{
    public class DALForgetPassword : IDALForgetPassword 
    {
        public string GetEmailByUserId(string userId)
        {
            string emailFromDb = null;

            using (var connection = new NpgsqlConnection(CommonHelper.GetConnectionString))
            {
                connection.Open();
                var query = "SELECT email FROM public.uti_employee_details WHERE employeecode = @UserId";
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        emailFromDb = reader["email"].ToString();
                    }
                }
            }

            return emailFromDb;
        }

        public string GetMobileByUserId(string userId)
        {
            string mobileFromDb = null;

            using (var connection = new NpgsqlConnection(CommonHelper.GetConnectionString))
            {
                connection.Open();
                var query = "SELECT mobile FROM public.uti_employee_details WHERE employeecode = @UserId";
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        mobileFromDb = reader["mobile"].ToString();
                    }
                }
            }

            return mobileFromDb;
        }

        public bool ManageEmployeeOtp(string userId, string emailOrMobile, string otp)
        {
            using (var connection = new NpgsqlConnection(CommonHelper.GetConnectionString))
            {
                connection.Open();
                var query = "SELECT public.manage_employee_otp(@UserId, @Email, @Otp)";
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);
                    command.Parameters.AddWithValue("@Email", emailOrMobile);
                    command.Parameters.AddWithValue("@Otp", otp);
                    command.ExecuteNonQuery();
                }
            }
            return true;
        }

        public bool VerifyEmployeeOtp(string userId, string otp)
        {
            bool isOtpValid = false;

            using (var connection = new NpgsqlConnection(CommonHelper.GetConnectionString))
            {
                connection.Open();
                var query = "SELECT public.verify_employee_otp(@UserId, @Otp)";
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);
                    command.Parameters.AddWithValue("@Otp", otp);
                    isOtpValid = Convert.ToBoolean(command.ExecuteScalar());
                }
            }

            return isOtpValid;
        }

        public bool UpdateEmployeePassword(string userId, string newPassword)
        {
            string encryptedPassword = AesGenerator.Encrypt(newPassword);
            using (var connection = new NpgsqlConnection(CommonHelper.GetConnectionString))
            {
                connection.Open();
                var query = "SELECT public.update_employee_password(@UserId, @NewPassword)";
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);
                    command.Parameters.AddWithValue("@NewPassword", encryptedPassword);

                    return Convert.ToBoolean(command.ExecuteScalar());
                }
            }
        }
    }
}