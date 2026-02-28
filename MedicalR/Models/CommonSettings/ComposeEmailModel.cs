using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedicalR.Models.CommonSettings
{
    public class ComposeEmailModel
    {
        public String Subject { get; set; }
        public string toEmailID { get; set; }
        public string ccEmailID { get; set; }
        public string Body { get; set; }
        public List<string> Attachment { get; set; }
    }
}