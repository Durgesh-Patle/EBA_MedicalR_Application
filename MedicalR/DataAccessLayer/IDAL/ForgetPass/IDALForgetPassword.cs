using MedicalR.Models.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedicalR.DataAccessLayer.IDAL.ForgetPass
{
    public interface IDALForgetPassword
    {
        //void LogoutOtp(string userId);
        bool IsOtpSentWithin2Minutes(string employeeId);
        bool CheckRecentOtp(string employeeId);
        void UpdateEmployeeOtp(string userId, string otp);
        bool VerifyEmployeeOtp(string userId, string otp);
        void IncrementFailedAttempts(string userId);
        int UnlockEmployeeOtps(string userId);
        bool ManageEmployeeOtp(string userId, string emailOrMobile, string otp);
        OtpDetails GetOtpDetails(string userId);
        EmployeeContactInfo GetEmployeeMobileAndEmail(string employeeCode);

    }
}