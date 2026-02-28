using MedicalR.Models;
using MedicalR.Models.MedicalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalR.DataAccessLayer.IDAL.MedicalR
{
    public interface IDALSanctionMedicalBills
    {
        List<SanctionMedicalBillsModel> SanctionBills_grid_data(string batch_no, string appl_no);
        List<SanctionMedicalBillsModel> SanctionBills_grid_data();
        List<MedicalRequestModel> EmployeePastHistory_grid_data();
        ResponseModel Save(MedicalRequestModel model, bool is_sanctioned);
        List<SanctionMedicalBillsModel> OfficeNoteData();
        List<SanctionMedicalBillsModel> AnnexureData();
        MedicalRequestModel GetItem(int id);
    }
}
