using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedicalR.Models.UserManagement
{
    public class UserDetailModel
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string EmailID { get; set; }
        public string MobileNo { get; set; }
        public string ApplnNo { get; set; }
        public string Band { get; set; }
        public string Department { get; set; }
        public string Location { get; set; }
        public string Designation { get; set; }
        public int RoleID { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public string RoleName { get; set; }
        public int PhotoURL_GoogleID { get; set; }
        public string PhotoURL { get; set; }
    }
    public class SideBarinfoModel
    {
        public int employeeid { get; set; }
        public string employeename { get; set; }
        public string email { get; set; }
        public string employeecode { get; set; }
        public string salutation { get; set; }
        public string mobile { get; set; }
        public string username { get; set; }
        public string Band { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public string locationcode { get; set; }
    }
    public class MenuHeaderModel
    {
        public string parentmenu_name { get; set; }
        public int id { get; set; }
        public List<MenuBindingModel> child_menu_list { get; set; }
    }
    public class MenuBindingModel
    {
        public string title { get; set; }
        public string link { get; set; }
        public int id { get; set; }
        public int parent_id { get; set; }
        public string parent_code { get; set; }
        public long menu_order { get; set; }
        public Boolean is_parent { get; set; }
        public DateTime dateofjoining { get; set; }
        public string role_code { get; set; }

    }
}