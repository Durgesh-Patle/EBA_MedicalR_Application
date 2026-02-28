using MedicalR.Models.MedicalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalR.DataAccessLayer.IDAL.MedicalR
{
    public interface IDALBatchCreation
    {
        List<BatchCreationModel> Get_BatchCreation_Grid_Data();
        string Update_BatchDetails(UpdateBatchModel model);
        List<UpdateBatchModel> Get_UpdateBatchNo_Grid_data();
    }
}
