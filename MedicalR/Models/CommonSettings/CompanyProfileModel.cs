using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedicalR.Models.CommonSettings
{
    public class CompanyProfileModel
    {
        public int CompanyID { get; set; }
        public string CompanyName { get; set; }
        public string EmailID { get; set; }
        public string Contactno { get; set; }
        public string website { get; set; }
        public int StateID { get; set; }
        public int CityID { get; set; }
        public int CountryID { get; set; }
        public string PostalCode { get; set; }
        public int LogoURL_GoogleID { get; set; }
        public string FaxNumber { get; set; }
        public string Poc { get; set; }
        public int IndustryID { get; set; }
        public string About { get; set; }
        public string LogoURL { get; set; }
    }
}