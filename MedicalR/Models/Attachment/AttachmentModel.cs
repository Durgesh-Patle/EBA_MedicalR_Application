using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedicalR.Models.Attachment
{
    public class AttachmentModel
    {
        public int AttachementID { get; set; }
        public int DocumentID { get; set; }
        public int PageID { get; set; }
        public int GoogleID { get; set; }
        public int RelevantID { get; set; }
        public string DocumnentType { get; set; }
        public string UserName { get; set; }
        public string CreatedDate { get; set; }
        public string AttachmentName { get; set; }
        public string AttachmentPath { get; set; }
    }
}