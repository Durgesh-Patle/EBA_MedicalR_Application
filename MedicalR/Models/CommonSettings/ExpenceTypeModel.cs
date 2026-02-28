using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedicalR.Models.CommonSettings
{
    public class ExpenceTypeModel
    {
        public int Id { get; set; }
        public string Treat_Type { get; set; }
        public string Expence_Type { get; set; }
        public string Head_Expence { get; set; }
        public bool Status { get; set; }
    }
}