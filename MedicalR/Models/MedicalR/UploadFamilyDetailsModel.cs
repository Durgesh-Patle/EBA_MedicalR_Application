using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedicalR.Models.MedicalR
{
    public class UploadFamilyDetailsModel
    {
        public string EMPID { get; set; }
        public string Name { get; set; }
        public DateTime? DOB { get; set; }
        public string Relation { get; set; }
        public string RelationID { get; set; }
    }
}