using MedicalR.Models.MedicalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedicalR.DataAccessLayer.IDAL.MedicalR
{
    public interface IDALAcknowledge
    {
         List<MedicalAcknowledgeModel> GetAcknowledgeGridData();
    }
}