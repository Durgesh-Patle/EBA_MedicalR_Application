using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Antlr.Runtime.Tree;
using MedicalR.CustomHelper;
using MedicalR.DataAccessLayer.DAL.CommonSettings;
using MedicalR.DataAccessLayer.DAL.EmailConfiguration;
using MedicalR.DataAccessLayer.DAL.RoleManagement;
using MedicalR.DataAccessLayer.DAL.TemplateModule;
using MedicalR.DataAccessLayer.DAL.UserManagement;
using MedicalR.DataAccessLayer.IDAL.CommonSettings;
using MedicalR.DataAccessLayer.IDAL.EmailConfiguration;
using MedicalR.DataAccessLayer.IDAL.RoleManagement;
using MedicalR.DataAccessLayer.IDAL.TemplateModule;
using MedicalR.DataAccessLayer.IDAL.UserManagement;
using MedicalR.Models.CommonSettings;
using MedicalR.Models.RoleManagement;
using MedicalR.Models.TemplateModule;
using MedicalR.Models.UserManagement;

namespace MedicalR.Controllers
{
    public class SettingsController : Controller
    {
        IDALRoleManagement objDALRoleManagement = new DALRoleManagement();
        IDALUserManagement objDALUserManagement = new DALUserManagement();
        IDALCommonSetting objDALCommonSetting = new DALCommonSetting();
        IDALEmailConfiguration objDALEmailConfiguration = new DALEmailConfiguration();
        IDALTemplateModule objDALTemplateModule = new DALTemplateModule();

        [CustomAuthorize]
        public ActionResult Index()
        {
            return View();
        }

        #region Hospital list
        [CustomAuthorize]
        public ActionResult Hospital_list()
        {
            var result = objDALCommonSetting.GetHospitalList();

            return View(result);
        }

        [CustomAuthorize]
        [Route("settings/Hospital_list/{id}/add")]
        public ActionResult Hospital_AddNew()
        {
            ViewBag.HosCityList = objDALCommonSetting.GetHospitalCity();
            return View();
        }

        [HttpPost]
        public JsonResult HospitalNewAdd(HospitalListModel objModel)
        {
            var result = objDALCommonSetting.AddHospital(objModel);
            return Json(result);
        }

        [CustomAuthorize]
        [Route("settings/Hospital_list/{id}/edit")]
        public ActionResult Hospital_Edit(int id)
        {
            HospitalListModel model = new HospitalListModel();
            model.id = id;
            ViewBag.HosCityList = objDALCommonSetting.GetHospitalCity();
            var result = objDALCommonSetting.GetsingleHospitalDetails(model);
            return View(result);
        }

        [HttpPost]
        public JsonResult UpdateHospital(HospitalListModel objModel)
        {
            var result = objDALCommonSetting.UpdateHospital(objModel);
            return Json(result);
        }

        [HttpPost]
        public JsonResult HospitalStatus(HospitalListModel objModel)
        {
            var result = objDALCommonSetting.HospitalStatus(objModel);
            return Json(result);
        }
        #endregion

        #region Expense Type
        [CustomAuthorize]
        public ActionResult Expence_Type()
        {
            var result = objDALCommonSetting.GetExpenceList();
            return View(result);
        }

        [CustomAuthorize]
        [Route("settings/Expence_Type/{id}/add")]
        public ActionResult Expense_AddNew()
        {
            ViewBag.TreatmentType = objDALCommonSetting.GetTreatmentType();
            return View();
        }

        [HttpPost]   
        public JsonResult ExpenseNewAdd(ExpenceTypeModel objModel)
        {
            var result = objDALCommonSetting.AddExpense(objModel);
            return Json(result);
        }

        [CustomAuthorize]
        [Route("settings/Expence_Type/{id}/edit")]
        public ActionResult Expense_Edit(int id)
        {
            ExpenceTypeModel model = new ExpenceTypeModel();
            model.Id = id;
            ViewBag.TreatmentType = objDALCommonSetting.GetTreatmentType();
            var result = objDALCommonSetting.GetsingleExpenseDetails(model);
            return View(result);
        }

        [HttpPost]
        public JsonResult UpdateExpenses(ExpenceTypeModel objModel)
        {
            var result = objDALCommonSetting.UpdateExpenses(objModel);
            return Json(result);
        }

        [HttpPost]
        public JsonResult ExpenseStatus(ExpenceTypeModel objModel)
        {
            var result = objDALCommonSetting.ExpenseStatus(objModel);
            return Json(result);
        }
        #endregion

        #region Objection
        [CustomAuthorize]
        public ActionResult Objection()
        {
            var result = objDALCommonSetting.GetObjectionList();
            return View(result);
        }

        [CustomAuthorize]
        [Route("settings/Objection/{id}/add")]
        public ActionResult Objection_AddNew()
        {
            return View();
        }

        [HttpPost]
        public JsonResult ObjectionNewAdd(ObjectionModel objModel)
        {
            var result = objDALCommonSetting.AddObjection(objModel);
            return Json(result);
        }

        [CustomAuthorize]
        [Route("settings/Objection/{id}/edit")]
        public ActionResult Objection_Edit(int id)
        {
            ObjectionModel model = new ObjectionModel();
            model.Id = id;
            var result = objDALCommonSetting.GetsingleObjectionDetails(model);
            return View(result);
        }

        [HttpPost]
        public JsonResult UpdateObjection(ObjectionModel objModel)
        {
            var result = objDALCommonSetting.UpdateObjection(objModel);
            return Json(result);
        }

        [HttpPost]
        public JsonResult ObjectionStatus(ObjectionModel objModel)
        {
            var result = objDALCommonSetting.ObjectionStatus(objModel);
            return Json(result);
        }
        #endregion

        #region Doctor list
        [CustomAuthorize]
        public ActionResult Doctor_List()
        {
            var result = objDALCommonSetting.GetDoctorlist();
            return View(result);
        }

        [CustomAuthorize]
        [Route("settings/Doctor_List/{id}/add")]
        public ActionResult Doctor_AddNew()
        {
            return View();
        }


        [HttpPost]
        public JsonResult DoctorNewAdd(DoctorList objModel)
        {
            var result = objDALCommonSetting.AddDoctor(objModel);
            return Json(result);
        }

        [CustomAuthorize]
        [Route("settings/Doctor_List/{id}/edit")]
        public ActionResult Doctor_Edit(int id)
        {
            DoctorList model = new DoctorList();
            model.id = id;

            var result = objDALCommonSetting.GetSingleDoctordetail(model);
            return View(result);
        }

        [HttpPost]
        public JsonResult UpdateDoctor(DoctorList objModel)
        {
            var result = objDALCommonSetting.UpdateDr(objModel);
            return Json(result);
        }

        [HttpPost]
        public JsonResult DrStatus(DoctorList objModel)
        {
            var result = objDALCommonSetting.DrStatus(objModel);
            return Json(result);
        }
        #endregion

        //public ActionResult edit_user(int id)
        //{
        //    UserDetailModel objModel = new UserDetailModel();
        //    objModel.UserID = id;
        //    var result = objDALUserManagement.GetSingleUserDetails(objModel);
        //    ViewBag.RoleType = objDALRoleManagement.GetRoleList().Where(x => x.IsActive == true).ToList();
        //    return View(result);
        //}
        //#region company information
        //[CustomAuthorize]
        //public ActionResult company_information()
        //{
        //    var result = objDALCommonSetting.GetCompanyDetails();
        //    ViewBag.CountryList = objDALCommonSetting.GetCountryList();
        //    ViewBag.StateList = objDALCommonSetting.GetStateList(0);
        //    ViewBag.CityList = objDALCommonSetting.GetCityList(0);
        //    ViewBag.IndustryList = objDALCommonSetting.GetIndustryList();
        //    return View(result);
        //}

        //[HttpPost]
        //public JsonResult GetStateList(int CountryID)
        //{
        //    var result = objDALCommonSetting.GetStateList(CountryID);
        //    return Json(result);
        //}

        //[HttpPost]
        //public JsonResult GetCityList(int StateID)
        //{
        //    var result = objDALCommonSetting.GetCityList(StateID);
        //    return Json(result);
        //}


        //[HttpPost]
        //public JsonResult UpdateCompanyDetails(CompanyProfileModel objModel)
        //{
        //    if (Request.Files != null && Request.Files.Count > 0)
        //    {
        //        var FilePaths = FileUploadHelper.UploadFile(Request.Files, "CompanyProfile");
        //        if (FilePaths != null && FilePaths.Count() > 0)
        //        {
        //            objModel.LogoURL = FilePaths.FirstOrDefault().FilePath;
        //            objModel.LogoURL_GoogleID = FilePaths.FirstOrDefault().GoogleDrive_ID;
        //        }
        //    }
        //    var result = objDALCommonSetting.UpdateCompanyDetails(objModel);
        //    return Json(result);
        //}

        //#endregion

        //#region profile
        //[CustomAuthorize]
        //public ActionResult profile()
        //{
        //    UserDetailModel objModel = new UserDetailModel();
        //    objModel.UserID = UserManager.User.UserID;    
        //    var result = objDALUserManagement.GetSingleUserDetails(objModel);
        //    ViewBag.RoleType = objDALRoleManagement.GetRoleList().Where(x => x.IsActive == true).ToList();
        //    return View(result);
        //}

        //[HttpPost]
        //public JsonResult UpdateProfile(UserDetailModel objModel)
        //{
        //    if (Request.Files != null && Request.Files.Count > 0)
        //    {
        //        var FilePaths = FileUploadHelper.UploadFile(Request.Files, "UserProfile");
        //        if (FilePaths != null && FilePaths.Count() > 0)
        //        {
        //            objModel.PhotoURL = FilePaths.FirstOrDefault().FilePath;
        //            objModel.PhotoURL_GoogleID = FilePaths.FirstOrDefault().GoogleDrive_ID;
        //        }
        //    }
        //    objModel.UserID = UserManager.User.UserID;
        //    var result = objDALUserManagement.UpdateUser(objModel);
        //    return Json(result);
        //}
        //#endregion

        //#region data templates
        //[CustomAuthorize]
        //public ActionResult data_templates()
        //{
        //    return View();
        //}
        //#endregion

        #region change password
        [CustomAuthorize]
        public ActionResult change_password()
        {
            return View();
        }

        [HttpPost]
        //public JsonResult UpdateUserPassword(PasswordModel objModel)
        //{
        //    var result = objDALCommonSetting.UpdateUserPassword(objModel);
        //    return Json(result);
        //}
        #endregion

        #region user management
        [CustomAuthorize]
        public ActionResult users()
        {
            var result = objDALUserManagement.GetUserList();
            return View(result);
        }

        [CustomAuthorize]
        [Route("settings/users/add")]
        public ActionResult add_user()
        {
            ViewBag.RoleType = objDALRoleManagement.GetRoleList().Where(x => x.IsActive == true).ToList();
            return View();
        }

        [HttpPost]
        public JsonResult AddUser(UserDetailModel objModel)
        {
            var result = objDALUserManagement.AddUser(objModel);
            return Json(result);
        }

        [CustomAuthorize]
        [Route("settings/users/{id}/edit")]
        public ActionResult edit_user(int id)
        {
            UserDetailModel objModel = new UserDetailModel();
            objModel.UserID = id;
            var result = objDALUserManagement.GetSingleUserDetails(objModel);
            ViewBag.RoleType = objDALRoleManagement.GetRoleList().Where(x => x.IsActive == true).ToList();
            return View(result);
        }

        [HttpPost]
        public JsonResult UpdateUser(UserDetailModel objModel)
        {
            var result = objDALUserManagement.UpdateUser(objModel);
            return Json(result);
        }

        [HttpPost]
        public JsonResult UpdateUserStatus(UserDetailModel objModel)
        {
            var result = objDALUserManagement.UpdateUserStatus(objModel);
            return Json(result);
        }
        #endregion

        //#region email configuaration
        //[CustomAuthorize]
        //public ActionResult email_configuaration()
        //{
        //    var result = objDALEmailConfiguration.GetEmailConfigurationList();
        //    return View(result);
        //}

        //[CustomAuthorize]
        //[Route("settings/email-configuaration/add")]
        //public ActionResult add_email_configuaration()
        //{
        //    ViewBag.UserList = objDALUserManagement.GetUserList();
        //    return View();
        //}

        //[HttpPost]
        //public JsonResult AddEmailconfiguaration(EmailConfigurationModel objModel)
        //{
        //    var result = objDALEmailConfiguration.AddEmailConfiguration(objModel);
        //    return Json(result);
        //}

        //[CustomAuthorize]
        //[Route("settings/email-configuaration/{user_id}/edit")]
        //public ActionResult edit_email_configuaration(int user_id = 0)
        //{
        //    EmailConfigurationModel objModel = new EmailConfigurationModel();
        //    objModel.UserID = user_id;
        //    var result = objDALEmailConfiguration.GetSingleEmailConfigurationDetails(objModel);
        //    ViewBag.UserList = objDALUserManagement.GetUserList();
        //    return View(result);
        //}

        //[HttpPost]
        //public JsonResult UpdateEmailconfiguaration(EmailConfigurationModel objModel)
        //{
        //    var result = objDALEmailConfiguration.UpdateEmailConfiguration(objModel);
        //    return Json(result);
        //}

        //[HttpPost]
        //public JsonResult UpdateEmailConfigurationStatus(EmailConfigurationModel objModel)
        //{
        //    var result = objDALEmailConfiguration.UpdateEmailConfigurationStatus(objModel);
        //    return Json(result);
        //}

        //#endregion

        #region Role Section
        [CustomAuthorize]
        public ActionResult roles()
        {
            var result = objDALRoleManagement.GetRoleList();
            return View(result);
        }

        [CustomAuthorize]
        [Route("settings/roles/add")]
        public ActionResult add_role()
        {
            return View();
        }

        [HttpPost]
        public JsonResult AddRole(RoleViewModel objModel)
        {
            var result = objDALRoleManagement.AddRole(objModel);
            return Json(result);
        }

        [CustomAuthorize]
        [Route("settings/roles/{id}/edit")]
        public ActionResult edit_role(int id)
        {
            RoleViewModel objModel = new RoleViewModel();
            objModel.RoleID = id;
            var result = objDALRoleManagement.GetSingleRoleDetails(objModel);
            return View(result);
        }

        [HttpPost]
        public JsonResult UpdateRole(RoleViewModel objModel)
        {
            var result = objDALRoleManagement.UpdateRole(objModel);
            return Json(result);
        }

        [HttpPost]
        public JsonResult UpdateRoleStatus(RoleViewModel objModel)
        {
            var result = objDALRoleManagement.UpdateRoleStatus(objModel);
            return Json(result);
        }
        #endregion

        #region  templates module
        [CustomAuthorize]
        public ActionResult template_module()
        {
            var result = objDALTemplateModule.GetTemplateList();
            return View(result);
        }

        [CustomAuthorize]
        [Route("settings/template-module/add")]
        public ActionResult add_template_module()
        {
            ViewBag.TemplateEnum = Enum.GetValues(typeof(TemplateFieldEnum.TemplateField)).Cast<TemplateFieldEnum.TemplateField>().ToList();
            return View();
        }

        [HttpPost]
        public JsonResult AddTemplate(TemplateModel objModel)
        {
            var result = objDALTemplateModule.AddTemplate(objModel);
            return Json(result);
        }

        [CustomAuthorize]
        [Route("settings/template-module/{id}/edit")]
        public ActionResult edit_template_module(int id)
        {
            ViewBag.TemplateEnum = Enum.GetValues(typeof(TemplateFieldEnum.TemplateField)).Cast<TemplateFieldEnum.TemplateField>().ToList();
            TemplateModel objModel = new TemplateModel();
            objModel.templateID = id;
            var result = objDALTemplateModule.GetSingleTemplateDetails(objModel);
            return View(result);
        }

        [HttpPost]
        public JsonResult UpdateTemplate(TemplateModel objModel)
        {
            var result = objDALTemplateModule.UpdateTemplate(objModel);
            return Json(result);
        }
        #endregion

        #region job board

        [CustomAuthorize]
        public ActionResult job_boards()
        {
            return View();
        }
        #endregion
    }
}