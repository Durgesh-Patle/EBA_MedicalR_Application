using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedicalR.Models.RoleManagement
{
    public class RoleViewModel
    {
        public int RoleID { get; set; }
        public string RoleName { get; set; }
        public string RoleDescription { get; set; }
        public string RoleRights { get; set; }
        public bool IsActive { get; set; }
        public int CompanyID { get; set; }
        public RoleRightDetailModel RoleRightDetails { get; set; }
    }
}