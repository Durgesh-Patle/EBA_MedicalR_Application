using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedicalR.DataAccessLayer.IDAL.MedicalR
{
    public interface IDALForgetPassword
    {
       
            string GetEmailByUserId(string userId);
        string GetMobileByUserId(string userId);
            bool ManageEmployeeOtp(string userId, string emailOrMobile, string otp);
            bool VerifyEmployeeOtp(string userId, string otp);
            bool UpdateEmployeePassword(string userId, string newPassword);
        
    }
}