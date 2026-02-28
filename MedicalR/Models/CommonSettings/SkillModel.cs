using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedicalR.Models.CommonSettings
{
    public class SkillModel
    {
        public int SkillID { get; set; }
        public string SkillName { get; set; }
        public string SkillType { get; set; }
        public string SkillKeyword { get; set; }
        public string SkillDescription { get; set; }
    }
}