using MedicalR.Models;
using MedicalR.Models.MedicalBillsException;
using MedicalR.Models.MedicalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedicalR.DataAccessLayer.IDAL.MedicalBillsException
{
    public interface IDALMedicalBillsException
    {
        ResponseModel Save(MedicalRequestModel model);
        MedicalRequestModel GetItem(int id);
        List<AdditionalSanction> GetAdditionalSanc(string empl_code, string appln_no);
        List<HospitilizationCr> GetHospitalizationCr();
        List<HospitilizationDHRD> GetHospitalizationDHRD();
        AdditionalSanction Getsingleempdetails(AdditionalSanction objModel);
        HospitilizationCr GetsingleHospitalCr(HospitilizationCr objModel);
        HospitilizationDHRD GetsingleHospitalDHRD(HospitilizationDHRD objModel);
        ResponseModel Approve(AdditionalSanction objModel);
        ResponseModel HospitalCrApprove(HospitilizationCr objModel);
        ResponseModel HospitalDHRDSave(HospitilizationDHRD objModel);


    }
}