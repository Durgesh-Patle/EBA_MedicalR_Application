using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedicalR.Models.RetiredEmployee
{
    public class RetiredEmployeeModel
    {
        public int Id { get; set; }
        public string EmpId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Bankname { get; set; } = string.Empty;
        public int bank_id { get; set; }
        public string Ifccode { get; set; } = string.Empty;
        public string Accno { get; set; } = string.Empty;
        public string Othinfo { get; set; } = string.Empty;
        public DateTime? Effdate { get; set; }
        public string Mobile { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool Status { get; set; }
    }
}