using Dapper;
using MedicalR.CustomHelper;
using MedicalR.DataAccessLayer.DAL.EmailConfiguration;
using MedicalR.DataAccessLayer.IDAL.EmailConfiguration;
using MedicalR.DataAccessLayer.IDAL.UserManagement;
using MedicalR.Models;
using MedicalR.Models.RoleManagement;
using MedicalR.Models.UserManagement;
using Newtonsoft.Json;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.DirectoryServices;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Web;


namespace MedicalR.DataAccessLayer.DAL.UserManagement
{
    public class DALUserManagement : IDALUserManagement
    {
        IDALEmailConfiguration objDALEmailConfiguration = new DALEmailConfiguration();
        #region validate user for login
        bool is_adlogin_success(LoginViewModel objModel)
        {
            bool status = false;

            try
            {
                string adConnections = ConfigurationManager.AppSettings["ADConnectionString"].ToString();
                string[] adServers = adConnections.Split(';');  // multiple domains from config

                foreach (string adConn in adServers)
                {
                    if (string.IsNullOrWhiteSpace(adConn))
                        continue;

                    try
                    {
                        DirectoryEntry entry = new DirectoryEntry(adConn.Trim(), objModel.EmpCode.Trim(), objModel.Password.Trim());

                        object obj = entry.NativeObject; 

                        DirectorySearcher search = new DirectorySearcher(entry);
                        search.Filter = "(SAMAccountName=" + objModel.EmpCode.Trim() + ")";
                        search.PropertiesToLoad.Add("cn");

                        SearchResult result = search.FindOne();

                        if (result != null)
                        {
                            CommonHelper.write_log("Success ---> AD response for employee: " + objModel.EmpCode + " via " + adConn);
                            status = true;
                            break;   
                        }
                        else
                        {
                            CommonHelper.write_log("Failed ---> No result for employee: " + objModel.EmpCode + " via " + adConn);
                        }
                    }
                    catch (DirectoryServicesCOMException exCOM)
                    {
                        // ********** IMPORTANT CHANGE :
                        // Previously: This exception stopped on main domain. Now: We LOG and CONTINUE to alternative domain
                        CommonHelper.write_log("Failed ---> COM Exception for employee: " + objModel.EmpCode + " via " + adConn + " | " + exCOM.Message);
                        continue; 
                    }
                    catch (Exception exInner)
                    {
                        CommonHelper.write_log("Failed ---> General Exception for employee: " + objModel.EmpCode + " via " + adConn + " | " + exInner.Message);
                        continue; 
                    }
                }

                if (!status)
                {
                    CommonHelper.write_log("Failed ---> AD authentication failed for all domains for employee: " + objModel.EmpCode);
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException("is_adlogin_success", ex);
                status = false;
            }

            return status;
        }


        //bool is_adlogin_success(LoginViewModel objModel)
        //{
        //    bool status = false;

        //    try
        //    {
        //        string adConnections = ConfigurationManager.AppSettings["ADConnectionString"].ToString();
        //        string[] adServers = adConnections.Split(';');  // split multiple domains

        //        foreach (string adConn in adServers)
        //        {
        //            if (string.IsNullOrWhiteSpace(adConn))
        //                continue;

        //            try
        //            {
        //                DirectoryEntry entry = new DirectoryEntry(adConn.Trim(), objModel.EmpCode.Trim(), objModel.Password.Trim());
        //                object obj = entry.NativeObject; // throws if invalid

        //                DirectorySearcher search = new DirectorySearcher(entry);
        //                search.Filter = "(SAMAccountName=" + objModel.EmpCode.Trim() + ")";
        //                search.PropertiesToLoad.Add("cn");
        //                SearchResult result = search.FindOne();

        //                if (result != null)
        //                {
        //                    CommonHelper.write_log("Success ---> AD response for employee: " + objModel.EmpCode + " via " + adConn);
        //                    status = true;
        //                    break; // stop when one succeeds
        //                }
        //                else
        //                {
        //                    CommonHelper.write_log("Failed ---> No result from AD for employee: " + objModel.EmpCode + " via " + adConn);
        //                }
        //            }
        //            catch (Exception exInner)
        //            {
        //                CommonHelper.write_log("Failed ---> Exception for employee: " + objModel.EmpCode + " via " + adConn + " | " + exInner.Message);
        //                continue; // try next AD
        //            }
        //        }

        //        if (!status)
        //        {
        //            CommonHelper.write_log("Failed ---> AD authentication unsuccessful for all domains for employee: " + objModel.EmpCode);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionLogging.LogException("is_adlogin_success", ex);
        //        status = false;
        //    }

        //    return status;
        //}



        //bool is_adlogin_success(LoginViewModel objModel)
        //{
        //    bool status = false;

        //    try
        //    {
        //        string adConnections = ConfigurationManager.AppSettings["ADConnectionString"].ToString();
        //        string[] adServers = adConnections.Split(';');  //  split multiple domains

        //        foreach (string adConn in adServers)
        //        {
        //            if (string.IsNullOrWhiteSpace(adConn)) continue;

        //            try
        //            {
        //                DirectoryEntry entry = new DirectoryEntry(adConn.Trim(), objModel.EmpCode.Trim(), objModel.Password.Trim());
        //                object obj = entry.NativeObject; // throws if invalid
        //                DirectorySearcher search = new DirectorySearcher(entry);
        //                search.Filter = "(SAMAccountName=" + objModel.EmpCode.Trim() + ")";
        //                search.PropertiesToLoad.Add("cn");
        //                SearchResult result = search.FindOne();

        //                if (result != null)
        //                {
        //                    CommonHelper.write_log("Success ---> AD response for employee :" + objModel.EmpCode + " via " + adConn);
        //                    status = true;
        //                    break; // stop when one succeeds
        //                }
        //            }
        //            catch
        //            {
        //                // Try next AD if this one fails
        //                continue;
        //            }
        //        }

        //        if (!status)
        //        {
        //            CommonHelper.write_log("Failed ---> AD response for employee " + objModel.EmpCode + " via " + adConn);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionLogging.LogException("is_adlogin_success", ex);
        //        status = false;
        //    }

        //    return status;
        //}


        //bool is_adlogin_success(LoginViewModel objModel)
        //{

        //    bool status = false;

        //    try
        //    {
        //        DirectoryEntry entry = new DirectoryEntry(ConfigurationManager.AppSettings["ADConnectionString"].ToString(), objModel.EmpCode.Trim(), objModel.Password.Trim());
        //        object obj = entry.NativeObject;
        //        DirectorySearcher search = new DirectorySearcher(entry);
        //        search.Filter = "(SAMAccountName=" + objModel.EmpCode.Trim() + ")";
        //        search.PropertiesToLoad.Add("cn");
        //        SearchResult result = search.FindOne();
        //        if (null == result)
        //        {
        //            CommonHelper.write_log("Failed --->ad response for employee " + objModel.EmpCode);
        //            status = false;
        //        }
        //        else
        //        {
        //            CommonHelper.write_log("Success ---> ad response for employee :" + objModel.EmpCode);
        //            status = true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionLogging.LogException("is_adlogin_success", ex);
        //        status = false;
        //    }
        //    return status;
        //}

        public bool ValidateUserTravelDesk(LoginViewModel objModel)
        {
            LoginDetailModel UserDetail = new LoginDetailModel();
            bool ad_status = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("AdStatus"));
            if (ad_status)
            {
                if (!is_adlogin_success(objModel))
                {
                    return false;
                }
            }
            return true;
        }
        public LoginDetailModel ValidateUser(LoginViewModel objModel)
        {
            LoginDetailModel UserDetail = new LoginDetailModel();
            NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString);
            DataTable dt = new DataTable();
            // bool status = true;
            bool ad_status = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("AdStatus"));
            try
            {

                using (con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {

                    con.Open();
                    using (NpgsqlCommand cmd = new NpgsqlCommand("select * from  mdcl_sp_get_validateemp(:pemplid)", con))
                    {
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("pemplid", objModel.EmpCode);

                        using (NpgsqlDataAdapter SqDA = new NpgsqlDataAdapter(cmd))
                        {
                            SqDA.Fill(dt);
                        }
                    }

                    if (dt.Rows.Count == 0)
                    {
                        UserDetail = new LoginDetailModel();
                    }
                    else
                    {
                        foreach (DataRow drow in dt.Rows)
                        {

                            UserDetail.UserID = string.IsNullOrWhiteSpace(drow["employeeid"].ToString()) ? 0 : Convert.ToInt32(drow["employeeid"].ToString());
                            UserDetail.UserName = string.IsNullOrWhiteSpace(drow["employeename"].ToString()) ? "" : drow["employeename"].ToString();
                            UserDetail.EmailID = string.IsNullOrWhiteSpace(drow["email"].ToString()) ? "" : drow["email"].ToString();
                            //UserDetail.ApplnNo = string.IsNullOrWhiteSpace(drow["name"].ToString()) ? "" : drow["name"].ToString();
                            //UserDetail.Band = string.IsNullOrWhiteSpace(drow["name"].ToString()) ? "" : drow["name"].ToString();
                            //UserDetail.Department = string.IsNullOrWhiteSpace(drow["name"].ToString()) ? "" : drow["name"].ToString();
                            //UserDetail.Location = string.IsNullOrWhiteSpace(drow["name"].ToString()) ? "" : drow["name"].ToString();
                            //UserDetail.Designation = string.IsNullOrWhiteSpace(drow["name"].ToString()) ? "" : drow["name"].ToString();
                            UserDetail.Employeecode = string.IsNullOrWhiteSpace(drow["employeecode"].ToString()) ? "" : drow["employeecode"].ToString();
                            UserDetail.Salutation = string.IsNullOrWhiteSpace(drow["salutation"].ToString()) ? "" : drow["salutation"].ToString();
                            UserDetail.Mobile = string.IsNullOrWhiteSpace(drow["mobile"].ToString()) ? "" : drow["mobile"].ToString();
                            UserDetail.DateofBirth = string.IsNullOrWhiteSpace(drow["dateofbirth"].ToString()) ? "" : drow["dateofbirth"].ToString();
                            UserDetail.DateofJoin = string.IsNullOrWhiteSpace(drow["dateofjoining"].ToString()) ? "" : drow["dateofjoining"].ToString();
                            UserDetail.Employeestatus = string.IsNullOrWhiteSpace(drow["employeestatus"].ToString()) ? "" : drow["employeestatus"].ToString();
                            UserDetail.ReportingMname = string.IsNullOrWhiteSpace(drow["reportingmanagername"].ToString()) ? "" : drow["reportingmanagername"].ToString();
                            UserDetail.ReportingMcode = string.IsNullOrWhiteSpace(drow["reportingmanagercode"].ToString()) ? "" : drow["reportingmanagercode"].ToString();
                            UserDetail.DateofResignation = string.IsNullOrWhiteSpace(drow["dateofresignation"].ToString()) ? "" : drow["dateofresignation"].ToString();
                            UserDetail.Employementtype = string.IsNullOrWhiteSpace(drow["employmenttype"].ToString()) ? "" : drow["employmenttype"].ToString();
                            UserDetail.Nationality = string.IsNullOrWhiteSpace(drow["nationality"].ToString()) ? "" : drow["nationality"].ToString();
                            UserDetail.MaritalStatus = string.IsNullOrWhiteSpace(drow["maritalstatus"].ToString()) ? "" : drow["maritalstatus"].ToString();
                            UserDetail.UserName = UserDetail.Salutation + UserDetail.UserName;
                            UserDetail.is_full_access = string.IsNullOrWhiteSpace(drow["is_full_access"].ToString()) ? false : Convert.ToBoolean(drow["is_full_access"]);
                            UserDetail.gender = string.IsNullOrWhiteSpace(drow["gender"].ToString()) ? "" : drow["gender"].ToString();
                            UserDetail.is_operator = string.IsNullOrWhiteSpace(drow["is_operator"].ToString()) ? false : Convert.ToBoolean(drow["is_operator"]);
                            UserDetail.role_code = string.IsNullOrWhiteSpace(drow["role_code"].ToString()) ? "" : drow["role_code"].ToString();
                            UserDetail.Band = string.IsNullOrWhiteSpace(drow["band"].ToString()) ? "" : drow["band"].ToString();
                            UserDetail.IsLoggedIn = string.IsNullOrWhiteSpace(drow["is_logged_in"].ToString()) ? false : Convert.ToBoolean(drow["is_logged_in"]);
                            //UserDetail.UserID = 1001;
                            //UserDetail.CompanyID = 1001;
                            //UserDetail.UserName = "Setti Sir";

                            //if (UserDetail.gender == "Female")
                            //{
                            DateTime date_of_joining = DateTime.ParseExact(UserDetail.DateofJoin, "dd MMM yyyy", CultureInfo.InvariantCulture);
                            if (date_of_joining.Year > 2003)
                            {
                                UserDetail.is_maternity = true;
                            }
                            //}
                        }
                        if (UserDetail.Band.ToUpper() == "IV")
                        {
                            if (objModel.EmpCode == objModel.Password)
                            {
                                return UserDetail;
                            }
                            else
                            {
                                UserDetail = new LoginDetailModel();
                            }
                        }
                        else
                        {
                            if (ad_status)
                            {
                                if (!is_adlogin_success(objModel))
                                {
                                    UserDetail = new LoginDetailModel();
                                    return UserDetail;
                                }


                                var adEmpCode = objModel.EmpCode.ToLower();

                                // Map specific employee codes for AD only
                                switch (adEmpCode)
                                {
                                    case "2242":
                                        adEmpCode = "sohamp";
                                        break;
                                    case "1521":
                                        adEmpCode = "nikitap";
                                        break;
                                    default:
                                        adEmpCode = objModel.EmpCode; // no override
                                        break;
                                }

                                // Clone objModel to send modified empCode to AD check
                                var adModel = new LoginViewModel
                                {
                                    EmpCode = adEmpCode,
                                    Password = objModel.Password
                                };

                                if (!is_adlogin_success(adModel))
                                {
                                    UserDetail = new LoginDetailModel();
                                    return UserDetail;
                                }
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                CommonHelper.write_log("error :ValidateUser()" + ex.Message);
                ExceptionLogging.LogException(ex);
                UserDetail = null;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return UserDetail;
        }
        public void UpdateUserSession(string empl_code, bool IsLoggedIn, bool logout_InActiveUsers)
        {
            using (NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString))
            {
                try
                {
                    con.Open();
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("p_empl_code", empl_code);
                    parameters.Add("p_is_loggedin", IsLoggedIn);
                    parameters.Add("p_logout_InActiveUsers", logout_InActiveUsers);
                    con.Execute("select * from mdcl_sp_updateUser_session(:p_empl_code,:p_is_loggedin,:p_logout_InActiveUsers)", parameters);
              
                }
                catch (Exception ex)
                {
                    CommonHelper.write_log("error in UpdateUserSession() :" + ex.Message);
                }
            }
        }
        public bool IsAlreadyLoggedIn(string empl_code)
        {
            using (NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString))
            {
                try
                {
                    con.Open();
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("p_empl_code", empl_code);
                    bool is_session_found = con.Query<bool>("select * from mdcl_sp_check_anyactive_session(:p_empl_code,:p_is_loggedin)", parameters).FirstOrDefault();
                    return is_session_found;
                }
                catch (Exception ex)
                {
                    CommonHelper.write_log("error in UpdateUserSession() :" + ex.Message);
                }
                return false;
            }
        }

        public List<MenuHeaderModel> GetMenus()
        {

            List<MenuHeaderModel> mhm = new List<MenuHeaderModel>();
            List<MenuBindingModel> result = new List<MenuBindingModel>();
            DataTable dt = new DataTable();
            try
            {
                using (NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString))
                {
                    con.Open();
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("pid", UserManager.User.UserID);
                    result = con.Query<MenuBindingModel>("select * from mdcl_sp_get_rolewise_menu(:pid)", parameters).ToList();

                    int count_check = con.Query<int>("select * from mdcl_sp_check_empl_is_manager(:pid)", parameters).FirstOrDefault();

                    List<MenuBindingModel> result2 = result.Where(x => x.is_parent == true).ToList();

                    foreach (MenuBindingModel model in result2)
                    {
                        MenuHeaderModel mhmodel = new MenuHeaderModel();

                        mhmodel.parentmenu_name = model.title;
                        mhmodel.id = model.id;
                        mhmodel.child_menu_list = new List<MenuBindingModel>();
                        List<MenuBindingModel> result3 = result.Where(x => x.parent_id == model.id).ToList();

                        //CommonHelper.write_log("parent_code :" + model.parent_code + " | count_check :" + count_check.Count);
                        if (model.parent_code == "#CHCM#" && count_check == 0 && !UserManager.User.is_operator)// if manager then only show chc menus
                        {
                            if (model.role_code != "#CHC_PROCESS#")
                            {
                                continue;
                            }

                        }
                        else if (model.parent_code == "#REMM#" && model.dateofjoining.Year > 2003 && !UserManager.User.is_operator) // if emp date of joining before 2003 then only reimbursement module will show
                        {
                            if ((!UserManager.User.is_maternity) && (UserManager.User.gender != "Male"))
                            {
                                continue;
                            }
                        }
                        if (result3.Count > 0)
                        {
                            mhm.Add(mhmodel);
                            foreach (MenuBindingModel mm in result3)
                            {
                                MenuBindingModel binmodel = new MenuBindingModel();
                                binmodel.title = mm.title;
                                binmodel.link = mm.link;
                                binmodel.menu_order = mm.menu_order;
                                binmodel.parent_id = mm.parent_id;
                                mhmodel.child_menu_list.Add(binmodel);
                            }
                        }


                    }
                }
            }

            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
            }
            return mhm;
        }
        public SideBarinfoModel GetSidebarInfo(SideBarinfoModel objModel)
        {
            SideBarinfoModel Sidebarinfo = new SideBarinfoModel();
            NpgsqlConnection con = new NpgsqlConnection(CommonHelper.GetConnectionString);
            DataTable dt = new DataTable();
            // bool status = true;
            try
            {


                Sidebarinfo.employeeid = UserManager.SideBarInfor.employeeid;
                Sidebarinfo.username = UserManager.SideBarInfor.username;
                Sidebarinfo.email = UserManager.SideBarInfor.email;
                Sidebarinfo.employeecode = UserManager.SideBarInfor.employeecode;
                Sidebarinfo.mobile = UserManager.SideBarInfor.mobile;
                Sidebarinfo.Band = UserManager.SideBarInfor.Band;
                Sidebarinfo.Department = UserManager.SideBarInfor.Department;
                Sidebarinfo.Designation = UserManager.SideBarInfor.Designation;
                Sidebarinfo.locationcode = UserManager.SideBarInfor.locationcode;

            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                Sidebarinfo = null;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return Sidebarinfo;
        }
        #endregion


        #region get company user list
        public List<UserDetailModel> GetUserList()
        {
            List<UserDetailModel> UserList = new List<UserDetailModel>();
            SqlConnection con = new SqlConnection(CommonHelper.GetConnectionString);
            try
            {
                var CompanyID = UserManager.User.CompanyID;
                SqlCommand cmd = new SqlCommand("sproc_GettblCompanyUserList", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CompanyID", CompanyID);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                DataSet ds = new DataSet();
                da.Fill(ds);

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    UserDetailModel User = new UserDetailModel();
                    User.UserID = Convert.ToInt32(ds.Tables[0].Rows[i]["CompanyUserID"].ToString());
                    User.UserName = ds.Tables[0].Rows[i]["UserName"].ToString();
                    User.EmailID = ds.Tables[0].Rows[i]["EmailID"].ToString();
                    User.MobileNo = ds.Tables[0].Rows[i]["MobileNo"] != DBNull.Value ? ds.Tables[0].Rows[i]["MobileNo"].ToString() : "";
                    User.IsActive = Convert.ToBoolean(ds.Tables[0].Rows[i]["IsActive"].ToString());
                    User.RoleID = Convert.ToInt32(ds.Tables[0].Rows[i]["Role"].ToString());
                    User.RoleName = ds.Tables[0].Rows[i]["RoleName"].ToString();
                    UserList.Add(User);
                }

            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                UserList = new List<UserDetailModel>();
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return UserList;
        }

        #endregion region


        #region get company single user data
        public UserDetailModel GetSingleUserDetails(UserDetailModel objModel)
        {
            UserDetailModel UserDetails = new UserDetailModel();
            SqlConnection con = new SqlConnection(CommonHelper.GetConnectionString);
            try
            {
                var CompanyID = UserManager.User.CompanyID;
                SqlCommand cmd = new SqlCommand("sproc_GetSingleUser", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CompanyID", CompanyID);
                cmd.Parameters.AddWithValue("@CompanyUserID", objModel.UserID);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                DataSet ds = new DataSet();
                da.Fill(ds);
                if (ds.Tables[0].Rows.Count == 0)
                {
                    UserDetails = null;
                }
                else
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        UserDetails.UserID = Convert.ToInt32(ds.Tables[0].Rows[i]["CompanyUserID"].ToString());
                        UserDetails.UserName = ds.Tables[0].Rows[i]["UserName"].ToString();
                        UserDetails.EmailID = ds.Tables[0].Rows[i]["EmailID"].ToString();
                        UserDetails.MobileNo = ds.Tables[0].Rows[i]["MobileNo"] != DBNull.Value ? ds.Tables[0].Rows[i]["MobileNo"].ToString() : "";
                        UserDetails.IsActive = Convert.ToBoolean(ds.Tables[0].Rows[i]["IsActive"].ToString());
                        UserDetails.RoleID = Convert.ToInt32(ds.Tables[0].Rows[i]["Role"].ToString());
                        UserDetails.PhotoURL_GoogleID = ds.Tables[0].Rows[i]["PhotoURL_GoogleID"] != DBNull.Value ? Convert.ToInt32(ds.Tables[0].Rows[i]["PhotoURL_GoogleID"].ToString()) : 0;
                        UserDetails.PhotoURL = ds.Tables[0].Rows[i]["PhotoURL"].ToString();
                    }
                }

            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                UserDetails = null;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return UserDetails;
        }

        #endregion region



        #region add new user to company
        public ResponseModel AddUser(UserDetailModel objModel)
        {
            ResponseModel Response = new ResponseModel();
            SqlConnection con = new SqlConnection(CommonHelper.GetConnectionString);
            try
            {
                var EncryptedPassword = AesGenerator.Encrypt(objModel.Password);
                var CurrentUtcDate = CommonHelper.GetDate;
                var CompanyUserID = UserManager.User.UserID;
                var CompanyID = UserManager.User.CompanyID;
                SqlCommand cmd = new SqlCommand("sproc_InsertCompanyUser", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserName", objModel.UserName);
                cmd.Parameters.AddWithValue("@EmailID", objModel.EmailID);
                cmd.Parameters.AddWithValue("@Password", EncryptedPassword);
                cmd.Parameters.AddWithValue("@Role", objModel.RoleID);
                cmd.Parameters.AddWithValue("@CompanyID", CompanyID);
                cmd.Parameters.AddWithValue("@MobileNo", objModel.MobileNo);
                cmd.Parameters.AddWithValue("@CreatedDate", CurrentUtcDate);
                cmd.Parameters.AddWithValue("@Createdby", CompanyUserID);
                cmd.Parameters.AddWithValue("@IsActive", true);
                cmd.Parameters.AddWithValue("@CompanyUserID", SqlDbType.Int).Direction = ParameterDirection.Output;
                con.Open();
                cmd.ExecuteNonQuery();
                var UserID = Convert.ToInt32(cmd.Parameters["@CompanyUserID"].Value.ToString());
                if (UserID == 0)
                {
                    Response.Status = false;
                    Response.Message = MessageHelper.UserEmailExists;
                }
                else
                {
                    Response.Status = true;
                    Response.Message = MessageHelper.UserAdded;
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                Response.Status = false;
                Response.Message = MessageHelper.ExceptionMessage;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return Response;
        }

        #endregion



        #region update existing company user
        public ResponseModel UpdateUser(UserDetailModel objModel)
        {
            ResponseModel Response = new ResponseModel();
            SqlConnection con = new SqlConnection(CommonHelper.GetConnectionString);
            try
            {
                var CompanyUserID = UserManager.User.UserID;
                var CompanyID = UserManager.User.CompanyID;
                SqlCommand cmd = new SqlCommand("sproc_UpdateCompanyUser", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserName", objModel.UserName);
                cmd.Parameters.AddWithValue("@EmailID", objModel.EmailID);
                cmd.Parameters.AddWithValue("@Role", objModel.RoleID);
                cmd.Parameters.AddWithValue("@CompanyID", CompanyID);
                cmd.Parameters.AddWithValue("@MobileNo", objModel.MobileNo);
                cmd.Parameters.AddWithValue("@CompanyUserID", objModel.UserID);
                cmd.Parameters.AddWithValue("@PhotoURL_GoogleID", objModel.PhotoURL_GoogleID);
                cmd.Parameters.AddWithValue("@PhotoURL", objModel.PhotoURL);
                cmd.Parameters.AddWithValue("@UpdateRes", SqlDbType.Bit).Direction = ParameterDirection.Output;
                con.Open();
                cmd.ExecuteNonQuery();
                var Res = Convert.ToInt32(cmd.Parameters["@UpdateRes"].Value.ToString());
                if (Res == 0)
                {
                    Response.Status = false;
                    Response.Message = MessageHelper.UserEmailExists;
                }
                else
                {
                    Response.Status = true;
                    Response.Message = MessageHelper.UserUpdated;
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                Response.Status = false;
                Response.Message = MessageHelper.ExceptionMessage;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return Response;
        }

        #endregion


        #region update existing company user status

        public ResponseModel UpdateUserStatus(UserDetailModel objModel)
        {
            ResponseModel Response = new ResponseModel();
            SqlConnection con = new SqlConnection(CommonHelper.GetConnectionString);
            try
            {
                var CompanyUserID = UserManager.User.UserID;
                var CompanyID = UserManager.User.CompanyID;
                SqlCommand cmd = new SqlCommand("sproc_UpdateCompanyUserStatus", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IsActive", objModel.IsActive);
                cmd.Parameters.AddWithValue("@CompanyID", CompanyID);
                cmd.Parameters.AddWithValue("@CompanyUserID", objModel.UserID);
                con.Open();
                cmd.ExecuteNonQuery();
                Response.Status = true;
                Response.Message = MessageHelper.UserStatusUpdated;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                Response.Status = false;
                Response.Message = MessageHelper.ExceptionMessage;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return Response;
        }
        #endregion

        #region save reset password code
        public ResponseModel SaveResetPasswordCode(string emailID, string resetCode)
        {
            ResponseModel Response = new ResponseModel();
            SqlConnection con = new SqlConnection(CommonHelper.GetConnectionString);
            try
            {
                SqlCommand cmd = new SqlCommand("sproc_UpdateResetCode", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EmailID", emailID);
                cmd.Parameters.AddWithValue("@ResetPasswordCode", resetCode);
                cmd.Parameters.AddWithValue("@UpdateRes", SqlDbType.Bit).Direction = ParameterDirection.Output;
                con.Open();
                cmd.ExecuteNonQuery();
                var Res = Convert.ToInt32(cmd.Parameters["@UpdateRes"].Value.ToString());
                if (Res == 0)
                {
                    Response.Status = false;
                    Response.Message = MessageHelper.EmailNotRegistered;
                }
                else
                {
                    Response.Status = true;
                    Response.Message = MessageHelper.ResetLinkSent;
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                Response.Status = false;
                Response.Message = MessageHelper.ExceptionMessage;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return Response;
        }
        #endregion

        #region verify reset password link
        public ResponseModel VerifyResetPasswordLink(string resetCode)
        {
            ResponseModel Response = new ResponseModel();
            SqlConnection con = new SqlConnection(CommonHelper.GetConnectionString);
            try
            {
                SqlCommand cmd = new SqlCommand("sproc_VerifyResetCode", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ResetPasswordCode", resetCode);
                cmd.Parameters.AddWithValue("@UpdateRes", SqlDbType.Bit).Direction = ParameterDirection.Output;
                con.Open();
                cmd.ExecuteNonQuery();
                var Res = Convert.ToInt32(cmd.Parameters["@UpdateRes"].Value.ToString());
                if (Res == 0)
                {
                    Response.Status = false;
                }
                else
                {
                    Response.Status = true;
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                Response.Status = false;
                Response.Message = MessageHelper.ExceptionMessage;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return Response;
        }
        #endregion

        #region  reset password 
        public ResponseModel ResetPassword(string resetCode, string newPassword)
        {
            ResponseModel Response = new ResponseModel();
            SqlConnection con = new SqlConnection(CommonHelper.GetConnectionString);
            try
            {
                var EncryptedPassword = AesGenerator.Encrypt(newPassword);
                SqlCommand cmd = new SqlCommand("sproc_ResetPassword", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ResetPasswordCode", resetCode);
                cmd.Parameters.AddWithValue("@Password", EncryptedPassword);
                cmd.Parameters.AddWithValue("@UpdateRes", SqlDbType.Bit).Direction = ParameterDirection.Output;
                con.Open();
                cmd.ExecuteNonQuery();
                var Res = Convert.ToInt32(cmd.Parameters["@UpdateRes"].Value.ToString());
                if (Res == 0)
                {
                    Response.Status = false;
                    Response.Message = MessageHelper.ExceptionMessage;
                }
                else
                {
                    Response.Status = true;
                    Response.Message = MessageHelper.PasswordUpdated;
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                Response.Status = false;
                Response.Message = MessageHelper.ExceptionMessage;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return Response;
        }
        #endregion

        #region registration
        public ResponseModel RegisterUser(RegisterModel objModel)
        {
            ResponseModel Response = new ResponseModel();
            SqlConnection con = new SqlConnection(CommonHelper.GetConnectionString);
            try
            {
                var EncryptedPassword = AesGenerator.Encrypt(objModel.Password);
                var CurrentUtcDate = CommonHelper.GetDate;

                SqlCommand cmd = new SqlCommand("sproc_Register", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CompanyName", objModel.CompanyName);
                cmd.Parameters.AddWithValue("@UserName", objModel.UserName);
                cmd.Parameters.AddWithValue("@EmailID", objModel.EmailID);
                cmd.Parameters.AddWithValue("@Password", EncryptedPassword);
                cmd.Parameters.AddWithValue("@IndustryID", objModel.IndustryID);
                cmd.Parameters.AddWithValue("@CreatedDate", CurrentUtcDate);
                cmd.Parameters.AddWithValue("@Result", SqlDbType.Int).Direction = ParameterDirection.Output;
                con.Open();
                cmd.ExecuteNonQuery();
                var Result = Convert.ToInt32(cmd.Parameters["@Result"].Value.ToString());
                if (Result == 0)
                {
                    Response.Status = false;
                    Response.Message = MessageHelper.UserEmailExists;
                }
                else
                {
                    Response.Status = true;
                    Response.Message = MessageHelper.UserRegister;
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                Response.Status = false;
                Response.Message = MessageHelper.ExceptionMessage;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return Response;
        }

        #endregion

        #region SideBarInfor
        public SideBarinfoModel GetEmplSidebarInfo(int empl_id)
        {
            SideBarinfoModel details = new SideBarinfoModel();
            DataTable dt = new DataTable();
            using (NpgsqlConnection con = new NpgsqlConnection(CustomHelper.CommonHelper.GetConnectionString))
            {
                con.Open();
                int rowcount = 0;
                DynamicParameters parameters = new DynamicParameters();
                using (NpgsqlCommand cmd = new NpgsqlCommand("select * from mdcl_sp_get_employee_info(:pempid)", con))
                {
                    cmd.Connection = con;
                    cmd.Parameters.AddWithValue("pempid", empl_id);
                    using (NpgsqlDataAdapter SqDA = new NpgsqlDataAdapter(cmd))
                    {
                        SqDA.Fill(dt);
                    }
                }
                if (dt.Rows.Count == 0)
                {
                    details = new SideBarinfoModel();
                }
                else
                {

                    foreach (DataRow drow in dt.Rows)
                    {

                        if (rowcount == 0)
                        {
                            details.employeeid = string.IsNullOrWhiteSpace(drow["employeeid"].ToString()) ? 0 : Convert.ToInt32(drow["employeeid"].ToString());
                            details.employeename = string.IsNullOrWhiteSpace(drow["employeename"].ToString()) ? "" : drow["employeename"].ToString();
                            details.email = string.IsNullOrWhiteSpace(drow["email"].ToString()) ? "" : drow["email"].ToString();
                            details.employeecode = string.IsNullOrWhiteSpace(drow["employeecode"].ToString()) ? "" : drow["employeecode"].ToString();
                            details.mobile = string.IsNullOrWhiteSpace(drow["mobile"].ToString()) ? "" : drow["mobile"].ToString();
                            details.salutation = string.IsNullOrWhiteSpace(drow["salutation"].ToString()) ? "" : drow["salutation"].ToString();
                            details.username = details.salutation + "" + details.employeename;
                            rowcount++;
                        }

                        string var_attribute_type_code = string.IsNullOrWhiteSpace(drow["attribute_type_code"].ToString()) ? "" : drow["attribute_type_code"].ToString();


                        switch (var_attribute_type_code)
                        {
                            case "Department":
                                details.Department = string.IsNullOrWhiteSpace(drow["attribute_type_unit_desc"].ToString()) ? "" : drow["attribute_type_unit_desc"].ToString();
                                break;

                            case "Designation":
                                details.Designation = string.IsNullOrWhiteSpace(drow["attribute_type_unit_desc"].ToString()) ? "" : drow["attribute_type_unit_desc"].ToString();
                                break;
                            case "Band":
                                details.Band = string.IsNullOrWhiteSpace(drow["attribute_type_unit_desc"].ToString()) ? "" : drow["attribute_type_unit_desc"].ToString();
                                break;
                            case "Location Code":
                                details.locationcode = string.IsNullOrWhiteSpace(drow["attribute_type_unit_desc"].ToString()) ? "" : drow["attribute_type_unit_desc"].ToString();
                                break;
                        }

                    }
                }
            }
            return details;

        }



        #endregion
    }
}