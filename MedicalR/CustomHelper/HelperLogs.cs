using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;

namespace MedicalR.CustomHelper
{
    public class HelperLogs
    {
        //public static void LogUserActivity(string userId, string action, string pageUrl)
        //{
        //    using (NpgsqlConnection conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["mycon"].ToString()))
        //    {
        //        conn.Open();
        //        using (NpgsqlCommand command = new NpgsqlCommand("insert_Activity_Log", conn))
        //        {
        //            command.CommandType = CommandType.StoredProcedure;
        //            command.Parameters.AddWithValue("p_userid", userId);
        //            command.Parameters.AddWithValue("p_action", action);
        //            command.Parameters.AddWithValue("p_datetime", DateTime.Now);
        //            command.Parameters.AddWithValue("p_pageurl", pageUrl);
        //            command.Parameters.AddWithValue("p_systemname", Environment.MachineName);
        //            command.Parameters.AddWithValue("p_ipaddress", GetIpAddress());
        //            command.ExecuteNonQuery();
        //        }
        //    }
        //}

        public static void LogUserActivity(string userId, string action, string pageUrl)
        {
            using (var conn = new NpgsqlConnection(
                ConfigurationManager.ConnectionStrings["mycon"].ToString()))
            {
                conn.Open();

                string sql = @"SELECT public.insert_activity_log(
                            @p_userid,
                            @p_action,
                            @p_datetime,
                            @p_pageurl,
                            @p_systemname,
                            @p_ipaddress
                       );";

                using (var command = new NpgsqlCommand(sql, conn))
                {
                    command.Parameters.AddWithValue("@p_userid", userId);
                    command.Parameters.AddWithValue("@p_action", action);
                    command.Parameters.AddWithValue("@p_datetime", DateTime.Now);
                    command.Parameters.AddWithValue("@p_pageurl", pageUrl);
                    command.Parameters.AddWithValue("@p_systemname", Environment.MachineName);
                    command.Parameters.AddWithValue("@p_ipaddress", GetIpAddress());

                    command.ExecuteNonQuery();
                }
            }
        }

        public static string GetIpAddress()
        {
            string ipAddress;
            if (HttpContext.Current != null)
            {
                ipAddress = HttpContext.Current.Request.UserHostAddress;
            }
            else
            {
                ipAddress = Dns.GetHostEntry(Dns.GetHostName())
                               .AddressList
                               .FirstOrDefault(addr => addr.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)?
                               .ToString();
            }
            return ipAddress;
        }
    }
}