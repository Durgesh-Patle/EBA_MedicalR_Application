using MedicalR.Models.MedicalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalR.DataAccessLayer.IDAL.MedicalR
{
    public interface IDALQueryModule
    {
       List<TrackingMedicalBillsModel> GetQMleGridData(string batch_no,string appl_no,string empl_code,string query_type);
    }
}
