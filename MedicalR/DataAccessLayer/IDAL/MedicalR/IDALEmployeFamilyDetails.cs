using MedicalR.Models;
using MedicalR.Models.MedicalR;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace MedicalR.DataAccessLayer.IDAL.MedicalR
{
    public interface IDALEmployeFamilyDetails
    {
        ResponseModel ReadExcel(string filePath);

        ResponseModel AddingDataInDatabase(DataTable FamilyDetailsList);
        List<UploadFamilyDetailsModel> GetEmployeeFamilyDetails();


    }
}