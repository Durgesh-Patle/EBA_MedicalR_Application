using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedicalR.Models.RetiredEmployee
{
    public class ReimburseAmtModel
    {
        public int Id { get; set; }
        public string Finyear { get; set; }
        public decimal Amount { get; set; }
        public bool Status { get; set; }
    }
}