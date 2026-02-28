using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedicalR.Models.AttachmentTypeModel
{
    public class AttachmentTypeModel
    {
        public int DocumentID { get; set; }
        public int PageID { get; set; }
        public string DocumentType { get; set; }
    }
}