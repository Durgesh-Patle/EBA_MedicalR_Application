using MedicalR.Models;
using MedicalR.Models.RoleManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedicalR.DataAccessLayer.IDAL.RoleManagement
{
    public interface IDALRoleManagement
    {
        List<RoleViewModel> GetRoleList();
        RoleViewModel GetSingleRoleDetails(RoleViewModel objModel);
        ResponseModel AddRole(RoleViewModel objModel);
        ResponseModel UpdateRole(RoleViewModel objModel);
        ResponseModel UpdateRoleStatus(RoleViewModel objModel);
    }
}