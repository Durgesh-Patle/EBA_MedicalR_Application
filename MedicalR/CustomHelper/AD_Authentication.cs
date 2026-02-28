using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.DirectoryServices;
using System.IO;
using System.Linq;
using System.Text;

namespace MedicalR.CustomHelper
{
    internal class AD_Authentication
    {
        public static bool CheckAccountLock_status(string employee_code)
        {
            bool AccLocked = false;
            try
            {
                DirectoryEntry directory_entry = GetDirectoryEntryByUserName(employee_code);
                if (directory_entry != null)
                {
                    AccLocked = Convert.ToBoolean(directory_entry.InvokeGet("IsAccountLocked"));
                }
                CommonHelper.write_log("AccLocked --> :" + AccLocked + " | " + employee_code);
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException("CheckAccountLock_status", ex);
            }
            return AccLocked;
        }
        public static DirectoryEntry GetDirectoryEntryByUserName(string employee_code)
        {
            string decpass = Decryptdata("UEBzJEFkbTFu");
            DirectoryEntry entry = new DirectoryEntry();

            try
            {
                DirectoryEntry de = new DirectoryEntry(ConfigurationManager.AppSettings["ADConnectionString"].ToString(), "ADPassAdmin", decpass);
                DirectorySearcher deSearch = new DirectorySearcher();
                deSearch.SearchRoot = de;
                deSearch.Filter = "(&(objectClass=user)(SAMAccountName=" + employee_code + "))";
                deSearch.SearchScope = SearchScope.Subtree;
                var result = deSearch.FindOne();
                return result != null ? result.GetDirectoryEntry() : null;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException("GetDirectoryEntryByUserName", ex);
                return null;
            }
        }
        private static string Decryptdata(string encryptpwd)
        {
            string decryptpwd = string.Empty;
            UTF8Encoding encodepwd = new UTF8Encoding();
            Decoder Decode = encodepwd.GetDecoder();
            byte[] todecode_byte = Convert.FromBase64String(encryptpwd);
            int charCount = Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
            char[] decoded_char = new char[charCount];
            Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
            decryptpwd = new String(decoded_char);
            return decryptpwd;
        }

    }
}