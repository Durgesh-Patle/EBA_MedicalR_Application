using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedicalR.Models.EmailConfiguration
{
    public class EmailConfigurationModel
    {
        public int UserEmailSettingsID { get; set; }
        public int UserID { get; set; }
        public string ProjectCode { get; set; }
        public string CompanyName { get; set; }
        public string AccountType { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string EmailPassword { get; set; }
        public string Server { get; set; }
        public int Port { get; set; }
        public bool IsActive { get; set; }
    }
}