using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedicalR.Models.UserManagement
{
    public class RegisterModel
    {
        public string CompanyName { get; set; }
        public string UserName { get; set; }
        public string EmailID { get; set; }
        public string Password { get; set; }
        public int IndustryID { get; set; }
    }
}