using MedicalR.Models;
using MedicalR.Models.EmailConfiguration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedicalR.DataAccessLayer.IDAL.EmailConfiguration
{
    public interface IDALEmailConfiguration
    {
        List<EmailConfigurationModel> GetEmailConfigurationList();
        EmailConfigurationModel GetSingleEmailConfigurationDetails(EmailConfigurationModel objModel);
        ResponseModel AddEmailConfiguration(EmailConfigurationModel objModel);
        ResponseModel UpdateEmailConfiguration(EmailConfigurationModel objModel);
        ResponseModel UpdateEmailConfigurationStatus(EmailConfigurationModel objModel);

    }
}