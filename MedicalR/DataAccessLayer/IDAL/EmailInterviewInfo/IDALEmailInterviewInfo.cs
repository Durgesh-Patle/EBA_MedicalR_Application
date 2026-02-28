using MedicalR.Models;
using MedicalR.Models.Attachment;
//using MedicalR.Models.EmailInterviewInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedicalR.DataAccessLayer.IDAL.EmailInterviewInfo
{
    public interface IDALEmailInterviewInfo
    {
        List<AttachmentModel> GetJobDescriptionDoc(Int32 JobID);
        List<AttachmentModel> GetCandidateResume(Int32 JobID);
        List<AttachmentModel> GetCandidateResumeMaskDetails(Int32 JobID);
        //ResponseModel SendEmail(EmailInterviewInfoModel objModel);
    }
}