using MedicalR.Models;
using MedicalR.Models.UserManagement;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedicalR.CustomHelper
{
    public class UserManager
    {
        public static LoginDetailModel User
        {
            get
            {
                return JsonConvert.DeserializeObject<LoginDetailModel>(Convert.ToString(HttpContext.Current.Session["UserDetails"]));
            }
        }
        public static SideBarinfoModel SideBarInfor
        {
            get
            {
                return (SideBarinfoModel)(HttpContext.Current.Session["EmployeeSideBarInfor"]);

            }
        }
        public static List<MenuHeaderModel> UserMenus
        {
            get
            {
                //return JsonConvert.DeserializeObject<MenuHeaderModel>(Convert.ToString(HttpContext.Current.Session["user_menus"]));
                return (HttpContext.Current.Session["user_menus"]) as List<MenuHeaderModel>;
            }
        }
        public static List<DDLMODEL> lst_table_ids
        {
            get
            {
                return (HttpContext.Current.Session["lst_ids"]) as List<DDLMODEL>;
            }
        }
    }
}