using MedicalR.Models;
using MedicalR.Models.MedicalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalR.DataAccessLayer.IDAL.MedicalR
{
    public interface IDALEditCheckingBills
    {
        List<EditCheckingBillsModel> EditChecking_grid_data();
        List<EditCheckingBillsModel> EditChecking_grid_data(string batch_no, string appl_no);
        ResponseModel Save(MedicalRequestModel model);
        MedicalRequestModel GetDataforEditChecking(MedicalRequestModel objModel);
        MedicalRequestModel GetItem(int id);
        string DownloadEditCheckingBills();
    }
}
