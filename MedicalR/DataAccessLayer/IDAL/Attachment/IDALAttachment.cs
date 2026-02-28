using MedicalR.Models;
using MedicalR.Models.Attachment;
using MedicalR.Models.AttachmentTypeModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedicalR.DataAccessLayer.IDAL.Attachment
{
    public interface IDALAttachment
    {
        List<AttachmentTypeModel> GetAttachmentTypeList(int PageID = 0);
        List<AttachmentModel> GetAttachmentList(Int32 PageID, Int32 ReleventID, Int32? AttachmentID = null);
        ResponseModel AddAttachment(AttachmentModel objModel);
        ResponseModel DeleteAttachment(Int32 AttachmentID, Int32 PageID);
    }
}