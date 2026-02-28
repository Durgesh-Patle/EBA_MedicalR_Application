using MedicalR.Models;
using MedicalR.Models.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedicalR.DataAccessLayer.IDAL.UserManagement
{
    public interface IDALUserManagement
    {
        LoginDetailModel ValidateUser(LoginViewModel objModel);
        bool ValidateUserTravelDesk(LoginViewModel objModel);

        SideBarinfoModel GetSidebarInfo(SideBarinfoModel objModel);
        SideBarinfoModel GetEmplSidebarInfo(int empl_id);
        List<MenuHeaderModel> GetMenus();
        void UpdateUserSession(string empl_code, bool IsLoggedIn, bool logout_InActiveUsers);
        bool IsAlreadyLoggedIn(string empl_code);
        List<UserDetailModel> GetUserList();
        UserDetailModel GetSingleUserDetails(UserDetailModel objModel);
        ResponseModel AddUser(UserDetailModel objModel);
        ResponseModel UpdateUser(UserDetailModel objModel);
        ResponseModel UpdateUserStatus(UserDetailModel objModel);
        ResponseModel SaveResetPasswordCode(string emailID, string resetCode);
        ResponseModel VerifyResetPasswordLink(string resetCode);
        ResponseModel ResetPassword(string resetCode, string newPassword);
        ResponseModel RegisterUser(RegisterModel objModel);
    }
}