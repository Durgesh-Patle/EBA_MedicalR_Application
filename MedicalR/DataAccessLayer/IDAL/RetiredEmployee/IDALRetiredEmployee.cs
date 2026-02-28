using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MedicalR.Models;
using MedicalR.Models.CommonSettings;
using MedicalR.Models.RetiredEmployee;

namespace MedicalR.DataAccessLayer.IDAL.RetiredEmployee
{
    public interface IDALRetiredEmployee
    {
        List<DropdownModel> Getyears(string empcd);
        byte [] GetEditCheckingHtmlString();

        byte[] GetApprovalHtmlString();

        byte[] GetTransOfficeNoteHtmlstring(string lotno);

        byte[] GetTransBankAdviseHtmlstring(string lotno);

        List<RetiredEmployeeModel> GetRetiredEmp();
        ResponseModel AddRemployee(RetiredEmployeeModel model);
        ResponseModel EditCheckingStart(List<RetiredempTransModel> model);
        RetiredEmployeeModel GetsingleRempdetails(RetiredEmployeeModel objModel);
        ResponseModel UpdateRemp(RetiredEmployeeModel model);
        ResponseModel RempStatus(RetiredEmployeeModel model);

        List<RetiredempTransModel> GetTransRetiredEmp();
        ResponseModel RemptransNewAdd(RetiredempTransModel model);
        RetiredempTransModel GetsingleRemptransdetails(RetiredempTransModel objModel);
        ResponseModel UpdateRemptrans(RetiredempTransModel model);
        ResponseModel RemptransStatus(RetiredempTransModel model);


        List<ReimburseAmtModel> GetRetiredReimurseAmt();
        ResponseModel AddReimbusamt(ReimburseAmtModel model);
        ReimburseAmtModel GetsingleReimburseamt(ReimburseAmtModel objModel);
        ResponseModel UpdateReimburse(ReimburseAmtModel model);
        ResponseModel ReimburseamtStatus(ReimburseAmtModel model);


        List<RempDemiseModel> GetRetiredEmpDemise();
        ResponseModel AddRempDemise(RempDemiseModel model);
        RempDemiseModel GetsingleRempdemise(RempDemiseModel objModel);
        ResponseModel UpdateRempdemise(RempDemiseModel model);
        ResponseModel RempDemiseStatus(RempDemiseModel model);
        //  ResponseModel UpdateRemp(RempDemiseModel model);
        List<DropdownModel> GetYears(string empcd);
        List<RetiredempTransModel> GetdataForLot();
        ResponseModel LotGenerationStart(List<RetiredempTransModel> model);
        RetiredempTransModel GetName(string empcd);
        List<RetiredempTransModel> CheckingVerify();
        RetiredempTransModel GetsingleEmpForChecking(RetiredempTransModel objModel);
       
        ResponseModel CheckVerified(RetiredempTransModel objModel);
        List<RetiredempTransModel> Trans_Sanction_Grid();
        RetiredempTransModel GetsingleEmpForSanction(RetiredempTransModel objModel);
        ResponseModel AddSanction(RetiredempTransModel objModel);
        ResponseModel Reject(RetiredempTransModel objModel);
        RetiredempTransModel GetTransOfficeNote(string lotno);
        List<RetiredempTransModel> GetLoTNo();
        List<RetiredempTransModel> GetEmpByLoT(string lotno);
        // List<RetiredempTransModel> GetEmpByEmpcd(string Empcd);
        List<RetiredempTransModel> GetEmpByEmpcd(RetiredempTransModel objmodel);
        RetiredempTransModel GetEmpByLoTAnnexure(string lotno);
        RetiredempTransModel GetAmount(string yearfrom);
        ResponseModel CheckInTable(RetiredEmployeeModel objModel);
        ResponseModel ApproveTrans(RetiredempTransModel model);
        List<DropdownModel> GetYearsforSpecialcase(string empcd);
        List<DropdownModel> GetYearsforSpecialcaseEdit(string empcd);
        List<DropdownModel> GetBank();
        RetiredempTransModel GetTransBankAdvise(string lotno);
        List<RetiredempTransModel> GetEmplPending(RetiredempTransModel objmodel);
        ResponseModel ApproveLotno(RetiredempTransModel model);
        RetiredempTransModel EditCheckingPrint();
        RetiredempTransModel EmplApprovalList();
        List<DropdownModel> GetYear();
        List<RetiredempTransModel> GetyearwiseReport(RetiredempTransModel objmodel);

    }
}