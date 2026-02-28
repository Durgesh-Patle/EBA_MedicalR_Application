using MedicalR.Models.RoleManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedicalR.Models.UserManagement
{
    public class LoginDetailModel
    {
        public int UserID { get; set; }
        public string Salutation { get; set; }
        public string UserName { get; set; }
        public string EmailID { get; set; }
        public string ApplnNo { get; set; }
        public string Band { get; set; }
        public string Department { get; set; }
        public string Location { get; set; }
        public string Designation { get; set; }

        public string Employeecode { get; set; }
        public string Mobile { get; set; }
        public string DateofBirth { get; set; }
        public string DateofJoin { get; set; }
        public string Employeestatus { get; set; }
        public string ReportingMname { get; set; }
        public string ReportingMcode { get; set; }
        public string DateofResignation { get; set; }
        public string Employementtype { get; set; }
        public string Nationality { get; set; }
        public string MaritalStatus { get; set; }

        public string PhotoURL { get; set; }
        public int CompanyID { get; set; }
        public string CompanyName { get; set; }
        public int RoleID { get; set; }
        public string RoleRights { get; set; }
        public bool is_full_access { get; set; }
        public string gender { get; set; }
        public bool is_maternity { get; set; } = false;
        public bool is_operator { get; set; } = false;
        public string role_code { get; set; }
        public bool IsLoggedIn { get; set; }
        public RoleRightDetailModel RoleRightDetails { get; set; }
    }
}