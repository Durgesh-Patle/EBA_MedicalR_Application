using MedicalR.Models;
using MedicalR.Models.TemplateModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedicalR.DataAccessLayer.IDAL.TemplateModule
{
    public interface IDALTemplateModule
    {
        List<TemplateModel> GetTemplateList();
        TemplateModel GetSingleTemplateDetails(TemplateModel objModel);
        ResponseModel AddTemplate(TemplateModel objModel);
        ResponseModel UpdateTemplate(TemplateModel objModel);
    }
}