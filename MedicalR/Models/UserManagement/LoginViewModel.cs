using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedicalR.Models.UserManagement
{
    public class LoginViewModel
    {  
        public string EmpCode { get; set; }
        public string otp { get; set; }
        public string Password { get; set; }
        public string ReturnUrl { get; set; }
        public string CaptchaInput { get; set; }
    }
}