
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MedicalR.CustomHelper
{
    public class EmailHelper
    {
       
        public static bool SendEmail(EmailModel objModel)
        {
            try
            {
                var ServiceURL = CommonHelper.GetEmailServiceSendURL;
                EmailJsonModel parameters = new EmailJsonModel();
                var ToemailList = objModel.toEmailID;
                var CcemailList = objModel.ccEmailID;
                var BcccemailList = objModel.bccEmailID;
                if (objModel.mailType == "contact")
                {
                    parameters.UserID = "reachus@cylsys.com";
                    parameters.AccountType = "Exchange";
                    parameters.Name = "";
                    parameters.Email = "";
                    parameters.Server = null;
                    parameters.EmailPassword = "";
                    parameters.Port = 0;
                    parameters.ProjectCode = "CYLSYS";
                    parameters.CompanyName = "CYLSYS";
                    parameters.ServiceUser = "gfpemailserviceUser";
                    parameters.ServicePassword = "gfpemailserviceUser@@##$$";
                }
                else
                {
                    parameters.UserID = objModel.emailConfigEmailID;
                    parameters.AccountType = objModel.emailConfigAccountType;
                    parameters.Name = objModel.emailConfigName;
                    parameters.Email = objModel.emailConfigEmailID;
                    parameters.Server = objModel.emailConfigServer;
                    parameters.EmailPassword = objModel.emailConfigEmailPassword;
                    parameters.Port = objModel.emailConfigPort;
                    parameters.ProjectCode = CommonHelper.GetProjectID;
                    parameters.CompanyName = UserManager.User.CompanyName;
                    parameters.ServiceUser = CommonHelper.GetEmailServiceUser;
                    parameters.ServicePassword = CommonHelper.GetEmailServicePassword;
                }
               
               
                parameters.ToEmail = ToemailList;
                parameters.CCEmail = CcemailList;
                parameters.BCCEmail = BcccemailList;
                parameters.body = objModel.Body;
                parameters.subject = objModel.Subject;
                parameters.templatename = null;
                parameters.ListofAttchment = objModel.Attachment;
               
                string json = JsonConvert.SerializeObject(parameters, Formatting.Indented);
                var Content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json")); 
                var result = client.PostAsync(ServiceURL, Content).Result;
                if (result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var ResultString = result.Content.ReadAsStringAsync().Result;
                    var ResultObj = JsonConvert.DeserializeObject<EmailResponseModel>(ResultString);
                    if(ResultObj.ResultType=="Success" || ResultObj.DBEmailID > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    var ResultString = result.Content.ReadAsStringAsync().Result;
                    return false;
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                return false;
            }
        }

        public static bool SendMeetingRequest(EmailModel objModel)
        {
            try
            {
                var ServiceURL = CommonHelper.GetSendMeetingRequestURL;
                EmailJsonModel parameters = new EmailJsonModel();
                var ToemailList = objModel.toEmailID;
                parameters.UserID = objModel.emailConfigEmailID;
                parameters.AccountType = objModel.emailConfigAccountType;
                parameters.Name = objModel.emailConfigName;
                parameters.Email = objModel.emailConfigEmailID;
                parameters.Server = objModel.emailConfigServer;
                parameters.EmailPassword = objModel.emailConfigEmailPassword;
                parameters.Port = objModel.emailConfigPort;
                parameters.ProjectCode = CommonHelper.GetProjectID;
                parameters.CompanyName = UserManager.User.CompanyName;
                parameters.ServiceUser = CommonHelper.GetEmailServiceUser;
                parameters.ServicePassword = CommonHelper.GetEmailServicePassword;
                parameters.ToEmail = ToemailList;
                parameters.CCEmail = null;
                parameters.body = objModel.Body;
                parameters.subject = objModel.Subject;
                parameters.templatename = null;
                parameters.ListofAttchment = objModel.Attachment;
                parameters.meetingstartdate = objModel.meetingstartdate;
                parameters.meetingdurationinMins = objModel.meetingdurationinMins;
                parameters.Location = objModel.meetingLocation;
                string json = JsonConvert.SerializeObject(parameters, Formatting.Indented);
                var Content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var result = client.PostAsync(ServiceURL, Content).Result;
                if (result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var ResultString = result.Content.ReadAsStringAsync().Result;
                    var ResultObj = JsonConvert.DeserializeObject<EmailResponseModel>(ResultString);
                    if (ResultObj.ResultType == "Success" || ResultObj.DBEmailID > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    var ResultString = result.Content.ReadAsStringAsync().Result;
                    return false;
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                return false;
            }
        }

        public class EmailJsonModel
        {
            public String UserID { get; set; }
            public String AccountType { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
            public string EmailPassword { get; set; }
            public string Server { get; set; }
            public int Port { get; set; }
            public String ProjectCode { get; set; }
            public String CompanyName { get; set; }

            public List<string> ToEmail { get; set; }

            public List<string> CCEmail { get; set; }

            public List<string> BCCEmail { get; set; }
            public string body { get; set; }
            public string subject { get; set; }
            public string templatename { get; set; }
            public List<string> ListofAttchment { get; set; }
            public string ServiceUser { get; set; }
            public string ServicePassword { get; set; }

            public string Location { get; set; }
            public DateTime meetingstartdate { get; set; }
            public int meetingdurationinMins { get; set; }

        }

        public class EmailModel
        {
            public String Subject { get; set; }
            public List<string> toEmailID { get; set; }
            public List<string> ccEmailID { get; set; }
            public List<string> bccEmailID { get; set; }
            public String Body { get; set; }
            public List<string> Attachment { get; set; }
            public string meetingLocation { get; set; }
            public DateTime meetingstartdate { get; set; }
            public int meetingdurationinMins { get; set; }
            public string mailType { get; set; }
            public string emailConfigName { get; set; }
            public string emailConfigAccountType { get; set; }
            public string emailConfigEmailID { get; set; }
            public string emailConfigEmailPassword { get; set; }
            public int emailConfigPort { get; set; }
            public string emailConfigServer { get; set; }
        }

        public class EmailResponseModel
        {
            public int DBEmailID { get; set; }
            public string ResultType { get; set; }
            public string Message { get; set; }
        }
    }
}