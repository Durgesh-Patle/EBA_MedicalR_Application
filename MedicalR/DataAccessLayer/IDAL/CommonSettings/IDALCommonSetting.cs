using MedicalR.Models;
using MedicalR.Models.CommonSettings;
//using MedicalR.Models.ContactUs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedicalR.DataAccessLayer.IDAL.CommonSettings
{
    public interface IDALCommonSetting
    {
        // common settings for dropdown
        List<DropdownModel> GetTreatmentType();
        List<DropdownModel> GetHospitalCity();

        List<HospitalListModel> GetHospitalList();
        HospitalListModel GetsingleHospitalDetails(HospitalListModel model);
        ResponseModel AddHospital(HospitalListModel model);
        ResponseModel UpdateHospital(HospitalListModel model);
        ResponseModel HospitalStatus(HospitalListModel model);

        List<ExpenceTypeModel> GetExpenceList();
        ExpenceTypeModel GetsingleExpenseDetails(ExpenceTypeModel model);
        ResponseModel AddExpense(ExpenceTypeModel model);
        ResponseModel UpdateExpenses(ExpenceTypeModel model);
        ResponseModel ExpenseStatus(ExpenceTypeModel model);

        List<ObjectionModel> GetObjectionList();
        ObjectionModel GetsingleObjectionDetails(ObjectionModel model);
        ResponseModel AddObjection(ObjectionModel model);
        ResponseModel UpdateObjection(ObjectionModel model);
        ResponseModel ObjectionStatus(ObjectionModel model);

        CompanyProfileModel GetCompanyDetails();
        ResponseModel UpdateCompanyDetails(CompanyProfileModel objModel);
        List<CountryModel> GetCountryList();
        List<StateModel> GetStateList(int CountryID);
        List<CityModel> GetCityList(int StateID);
        List<IndustryTypeModel> GetIndustryList();
        ResponseModel UpdateUserPassword(PasswordModel objModel);
        //void SendContactEmail(ContactUsModel objModel);
        List<SourceModel> GetSourceList();
        List<SocialMediaModel> GetSocialMediaList();
        List<SkillModel> GetSkillList();
        ResponseModel AddSkill(SkillModel objModel);
        ResponseModel UpdateColumns(List<string> columns, string columnType);
        List<string> getColumns(string columnType);
        List<DoctorList> GetDoctorlist();
        ResponseModel AddDoctor(DoctorList model);
        DoctorList GetSingleDoctordetail(DoctorList model);
        ResponseModel UpdateDr(DoctorList model);
        ResponseModel DrStatus(DoctorList model);
    }
}