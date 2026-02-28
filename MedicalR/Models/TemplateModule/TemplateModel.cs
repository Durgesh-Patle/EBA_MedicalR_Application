using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedicalR.Models.TemplateModule
{
    public class TemplateModel
    {
        public int templateID { get; set; }
        public string templateTitle { get; set; }
        public string templateDescription { get; set; }
        public bool templatePublish { get; set; }
        public DateTime templatePublishDate { get; set; }
        public DateTime templateLastModified { get; set; }
    }
}