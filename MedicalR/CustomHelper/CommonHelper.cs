using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Data;
using OfficeOpenXml;
using System.Net.Http;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;

namespace MedicalR.CustomHelper
{
    public class CommonHelper
    {
        public static DateTime GetDate
        {
            get
            {
                return DateTime.UtcNow;
            }
        }
        public static DataTable GetDataTableFromExcel(string path, bool hasHeader = true)
        {
            DataTable tbl = new DataTable();
            try
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (var pck = new OfficeOpenXml.ExcelPackage())
                {


                    using (var stream = File.OpenRead(path))
                    {
                        pck.Load(stream);
                    }
                    var ws = pck.Workbook.Worksheets.FirstOrDefault();

                    foreach (var firstRowCell in ws.Cells[1, 1, 1, ws.Dimension.End.Column])
                    {
                        tbl.Columns.Add(hasHeader ? firstRowCell.Text : string.Format("Column {0}", firstRowCell.Start.Column));
                    }
                    var startRow = hasHeader ? 2 : 1;
                    for (int rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
                    {
                        var wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
                        DataRow row = tbl.Rows.Add();
                        foreach (var cell in wsRow)
                        {
                            row[cell.Start.Column - 1] = cell.Text;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CommonHelper.write_log("error in GetDataTableFromExcel () :" + ex.Message);
            }
            return tbl;

        }

        public static async Task<bool> SendOtpMailAsync(string userId, string otp, string email, string mobileNumber)
        {
            try
            {
                bool otpSentToMobile = false;
                bool otpSentToEmail = false;

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;


                if (!string.IsNullOrEmpty(email))
                {
                    otpSentToEmail = SendOtpEmail(otp, email);
                }
                else
                {
                    CommonHelper.write_log("Email is null or empty. Cannot send OTP via email. " + userId + " ");
                }



                if (!string.IsNullOrEmpty(mobileNumber))
                {
                    //otpSentToMobile = await SendOtpSmsAsync(otp, mobileNumber);
                }
                else
                {
                    CommonHelper.write_log("Mobile number is null or empty. Cannot send OTP via SMS." + userId + " ");
                }


                return otpSentToEmail;

            }
            catch (Exception ex)
            {
                CommonHelper.write_log($"Error in SendOtpMailAsync: {ex.Message}");
                return false;
            }
        }

        private static bool SendOtpEmail(string otp, string email)
        {
            try
            {
                string smtpPrimaryMailServer = ConfigurationManager.AppSettings["SmtpPrimaryMailServer"];
                string senderEmail = ConfigurationManager.AppSettings["SenderEmail"];
                string senderPass = ConfigurationManager.AppSettings["SenderPass"];
                int port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]);
                bool isSSL = Convert.ToBoolean(ConfigurationManager.AppSettings["IsSsl"]);

                using (SmtpClient smtpClient = new SmtpClient(smtpPrimaryMailServer, port))
                {
                    smtpClient.Credentials = new NetworkCredential(senderEmail, senderPass);
                    smtpClient.EnableSsl = isSSL;
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtpClient.Timeout = 20000;

                    using (MailMessage message = new MailMessage())
                    {
                        message.IsBodyHtml = true;
                        message.From = new MailAddress(senderEmail);
                        message.To.Add(email);
                        message.Subject = "EBA Application OTP Code";
                        message.Body = $"<b>{otp}</b>  is the OTP for the EBA Application and is valid for the next two minutes.<br><br>Regards,<br>UTIAMC";

                        smtpClient.Send(message);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                CommonHelper.write_log($"Error while sending OTP email: {ex.Message}");
                return false;
            }
        }


        //private static async Task<bool> SendOtpSmsAsync(string otp, string mobileNumber)
        //{
        //    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

        //    try
        //    {
        //        string baseUrl = "https://enterprise.smsgupshup.com/apps/apis/global/rest.php";
        //        string otpMessage = $"{otp} is the OTP for the Retired UTI AMC Employees Medical Application and is valid for the next two minutes.UTIAMC";
        //        //string otpMessage = $"{otp} is the OTP for the Digicard Application and is valid for the next two minutes. UTIAMC";
        //        //   string otpMessage = $"{otp} is the OTP";
        //        string encodedOtpMessage = Uri.EscapeDataString(otpMessage);
        //        string userId = "UTIMF";
        //        string password = "ujgh55hs";
        //        string url = $"{baseUrl}?userid={userId}&password={password}&send_to={mobileNumber}&msg={encodedOtpMessage}";

        //        HttpClientHandler handler = new HttpClientHandler
        //        {
        //            // Optional: Uncomment for debugging purposes
        //            // ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
        //        };

        //        HttpClient client = new HttpClient(handler);

        //        await client.GetAsync(url);

        //        //if (response.IsSuccessStatusCode)
        //        //{
        //        //    string responseText = await response.Content.ReadAsStringAsync();
        //        //    //          _commonHelperService.LogError("Mobile Number: " + mobileNumber + " | Response: " + responseText);
        //        //    CommonHelper.write_log("Mobile Number: " + mobileNumber + " | Response: " + responseText);

        //        //    return responseText.Contains("SUCCESS");
        //        //}
        //        //else
        //        //{
        //        //    //          _commonHelperService.LogError($"Failed to send OTP SMS. Status code: {response.StatusCode}");
        //        //    CommonHelper.write_log($"Failed to send OTP SMS. Status code: {response.StatusCode}");
        //        return false;
        //        //}

        //    }
        //    catch (Exception ex)
        //    {
        //        //      _commonHelperService.LogError("Error in SendOtpMobileAsync() : " + ex.Message);
        //        CommonHelper.write_log("Error in async SendOtpMobile for Login() : " + ex.Message);
        //        return false;
        //    }
        //}
        public static string GetConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["mycon"].ToString();
            }
        }
        public static string GetConnectionStrings
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["myconn"].ToString();
            }
        }
        public static string GetEmailConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["myemailcon"].ToString();
            }
        }
        public static string Html_Template_Dir
        {
            get
            {
                return ConfigurationManager.AppSettings["html_template_dir"].ToString(); ;
            }
        }

        public static string GetProjectID
        {
            get
            {
                return ConfigurationManager.AppSettings["ProjectID"].ToString();
            }
        }
        public static string GetGDServiceUploadURL
        {
            get
            {
                return ConfigurationManager.AppSettings["GDServiceUploadURL"].ToString();
            }
        }
        public static string GetGDServiceDownloadURL
        {
            get
            {
                return ConfigurationManager.AppSettings["GDServiceDownloadURL"].ToString();
            }
        }
        public static string GetGDServiceBaseURL
        {
            get
            {
                return ConfigurationManager.AppSettings["GDServiceBaseURL"].ToString();
            }
        }
        public static string GetGDServiceAbsolutePath
        {
            get
            {
                return ConfigurationManager.AppSettings["GDServiceAbsolutePath"].ToString();
            }
        }

        public static string GetResumeParsingAPI
        {
            get
            {
                return ConfigurationManager.AppSettings["ResumeParsingAPI"].ToString();
            }
        }
        public static string GetEmailServiceSendURL
        {
            get
            {
                return ConfigurationManager.AppSettings["EmailServiceSendURL"].ToString();
            }
        }

        public static string GetSendMeetingRequestURL
        {
            get
            {
                return ConfigurationManager.AppSettings["EmailServiceSendMeetingRequest"].ToString();
            }
        }
        public static string GetEmailServiceUser
        {
            get
            {
                return ConfigurationManager.AppSettings["EmailServiceUser"].ToString();
            }
        }
        public static string GetEmailServicePassword
        {
            get
            {
                return ConfigurationManager.AppSettings["EmailServicePassword"].ToString();
            }
        }
        public static string GetAppBaseUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["AppBaseUrl"].ToString();
            }
        }

        public static List<string> GetEmailToID
        {
            get
            {
                var emailIDs = ConfigurationManager.AppSettings["EmailToID"].ToString();
                return emailIDs.Split(',').ToList();
            }
        }
        public static List<string> GetEmailCcID
        {
            get
            {
                var emailIDs = ConfigurationManager.AppSettings["EmailCcID"].ToString();
                return emailIDs.Split(',').ToList();
            }
        }
        public static void write_log(string message)
        {
            try
            {
                string ErrorLogDir = ConfigurationManager.AppSettings["Error_log"].ToString();
                if (!Directory.Exists(ErrorLogDir))
                    Directory.CreateDirectory(ErrorLogDir);

                ErrorLogDir += "\\medicalr_error_log" + DateTime.Now.ToString("dd-MMM-yyyy") + ".txt";

                using (StreamWriter sw = new StreamWriter(ErrorLogDir, true))
                {
                    sw.WriteLine(DateTime.Now.ToString("dd-MMM-yy HH:mm:ss") + "\t" + message);
                }
            }
            catch (Exception exx)
            {

            }
        }
        public static byte[] Convert2(string source)
        {
            Process p;
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = ConfigurationManager.AppSettings["wkhtml"];
            // psi.WorkingDirectory = @"D:\";// Path.GetDirectoryName(psi.FileName);

            // run the conversion utility
            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;
            psi.RedirectStandardInput = true;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;

            // note: that we tell wkhtmltopdf to be quiet and not run scripts
            string args = "-q -n ";
            args += "--disable-smart-shrinking ";
            args += "";
            args += "--outline-depth 0 ";
            args += "--page-size A4 ";
            args += " - -";

            psi.Arguments = args;

            p = Process.Start(psi);

            try
            {
                using (StreamWriter stdin = p.StandardInput)
                {
                    stdin.AutoFlush = true;
                    stdin.Write(source);
                }

                //read output
                byte[] buffer = new byte[32768];
                byte[] file;
                using (var ms = new MemoryStream())
                {
                    while (true)
                    {
                        int read = p.StandardOutput.BaseStream.Read(buffer, 0, buffer.Length);
                        if (read <= 0)
                            break;
                        ms.Write(buffer, 0, read);
                    }
                    file = ms.ToArray();
                }

                p.StandardOutput.Close();
                // wait or exit
                p.WaitForExit(60000);

                // read the exit code, close process
                int returnCode = p.ExitCode;
                p.Close();

                return file;
                //if (returnCode == 1)
                //    return file;
                //else
                //  CommonHelper.write_log("Could not create PDF, returnCode:" + returnCode);
            }
            catch (Exception ex)
            {
                CommonHelper.write_log("Could not create PDF :-" + ex.Message);
            }
            finally
            {
                p.Close();
                p.Dispose();
            }
            return null;
        }
        public static string encrypt(string encryptString)
        {
            string EncryptionKey = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            byte[] clearBytes = Encoding.Unicode.GetBytes(encryptString);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] {
            0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
        });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    encryptString = Convert.ToBase64String(ms.ToArray());
                }
            }
            return encryptString;
        }

        public static string Decrypt(string cipherText)
        {
            string EncryptionKey = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] {
            0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
        });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }
    }
}