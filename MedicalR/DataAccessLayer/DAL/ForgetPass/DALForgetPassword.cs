using Dapper;
using MedicalR.CustomHelper;
using MedicalR.DataAccessLayer.IDAL.ForgetPass;
using MedicalR.Models.UserManagement;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Net.Http;
using System.Threading.Tasks;

namespace MedicalR.DataAccessLayer.DAL.ForgetPass
{
    public class DALForgetPassword : IDALForgetPassword
    {
        public OtpDetails GetOtpDetails(string userId)
        {
            OtpDetails otpDetails = null;

            try
            {
                using (var connection = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM public.check_lock_status(@p_UserId)";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@p_UserId", userId);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                otpDetails = new OtpDetails
                                {
                                    FailedAttempts = reader.IsDBNull(0) ? 0 : reader.GetInt32(0), // Default to 0 if NULL
                                    LastFailedAt = reader.IsDBNull(1) ? DateTime.MinValue : reader.GetDateTime(1), // Default to MinValue if NULL
                                    is_locked = reader.GetChar(2)
                                };
                            }
                        }
                    }

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                otpDetails = new OtpDetails
                {
                    FailedAttempts = 0,
                    LastFailedAt = DateTime.MinValue
                };
            }

            return otpDetails;
        }
        public bool IsOtpSentWithin2Minutes(string employeeId)
        {
            using (var connection = new NpgsqlConnection(CommonHelper.GetConnectionString))
            {
                connection.Open();
                var result = connection.ExecuteScalar<bool>(
                    "SELECT check_otp_sent_within_2_minutes(@EmployeeId);",
                    new { EmployeeId = employeeId });

                return result;
            }
        }
        public bool CheckRecentOtp(string employeeId)
        {
            using (var connection = new NpgsqlConnection(CommonHelper.GetConnectionString))
            {
                connection.Open();
                var parameters = new { employee_id_param = employeeId };
                string sql = "SELECT check_recent_otp(@employee_id_param);";

                var result = connection.ExecuteScalar<bool>(sql, parameters);
                return result;
            }
        }

        public void UpdateEmployeeOtp(string userId, string otp)
        {
            try
            {
                using (var connection = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("p_user_id", userId);
                    parameters.Add("p_otp", otp);
                    //parameters.Add("p_failed_attempts", failedAttempts, DbType.Int32);

                    connection.Open();
                    connection.Execute("CALL update_employee_otp(@p_user_id, @p_otp)", parameters, commandType: CommandType.Text);

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

        }
        public static void LogoutOtp(string userId)
        {
            try
            {
                using (var connection = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("p_empid", userId);

                    connection.Open();
                    // Call the function using a SELECT statement
                    connection.Execute("SELECT unlock_employee(@p_empid)", parameters, commandType: CommandType.Text);

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error unlocking employee: " + ex.Message);
                throw;
            }
        }

        public bool ManageEmployeeOtp(string userId, string emailOrMobile, string otp)
        {
            try
            {
                using (var connection = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {
                    connection.Open();
                    var query = "CALL public.manage_user_otp(@p_user_id, @p_otp)"; // Use CALL instead of SELECT
                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        Console.WriteLine("okey");
                        command.Parameters.AddWithValue("@p_user_id", userId);
                        command.Parameters.AddWithValue("@p_otp", otp);
                        command.ExecuteNonQuery();
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                ExceptionLogging.LogException(ex);
                return false; // Return false in case of exception
            }
            return true;
        }
        public void IncrementFailedAttempts(string userId)
        {
            try
            {
                using (var connection = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {
                    connection.Open();
                    string query = "SELECT public.increment_failed_attempts(@p_UserId)";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@p_UserId", userId);
                        command.ExecuteNonQuery();
                    }

                    connection.Close();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                ExceptionLogging.LogException(ex);
                throw;
            }

        }


        public EmployeeContactInfo GetEmployeeMobileAndEmail(string employeeCode)
        {
            using (IDbConnection db = new NpgsqlConnection(CommonHelper.GetConnectionString))
            {
                string query = "SELECT mobile, email FROM public.uti_employee_details WHERE employeecode = @EmployeeCode";
                return db.QueryFirstOrDefault<EmployeeContactInfo>(query, new { EmployeeCode = employeeCode });
            }
        }


        public int UnlockEmployeeOtps(string userId)
        {
            try
            {
                using (var connection = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {
                    connection.Open();

                    using (var command = new NpgsqlCommand("SELECT unlock_employee_otps(@p_user_id)", connection))
                    {
                        command.Parameters.AddWithValue("@p_user_id", userId);

                        var result = command.ExecuteScalar();
                        return result != null ? Convert.ToInt32(result) : 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                ExceptionLogging.LogException(ex);
                throw;
            }

        }

        public bool VerifyEmployeeOtp(string userId, string otp)
        {
            bool isOtpValid = false;
            try
            {
                using (var connection = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {
                    connection.Open();
                    string query = "SELECT public.verify_employee_otp(@UserId, @Otp)";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserId", userId);
                        command.Parameters.AddWithValue("@Otp", otp);
                        isOtpValid = Convert.ToBoolean(command.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                ExceptionLogging.LogException(ex);

                throw;
            }
            return isOtpValid;
        }
    }
}