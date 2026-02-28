using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedicalR.Models.UserManagement
{
    public class ResetPassword
    {
        public string resetCode { get; set; }
        public string password { get; set; }
    }
    public class OtpDetails
    {
        public char is_locked { get; set; }
        public int FailedAttempts { get; set; }
        public DateTime LastFailedAt { get; set; }

    }
    public class EmployeeContactInfo
    {
        public string Mobile { get; set; }
        public string Email { get; set; }
    }
}