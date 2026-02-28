
using MedicalR.Models;
using MedicalR.Models.CHC;
using MedicalR.Models.MedicalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedicalR.DataAccessLayer.IDAL.CHC
{
    public interface IDALChcRequest
    {
        List<Hospitallist> GetHospitallist(Hospitallist objModel);
        ResponseModel AddCHCRequest(CHCRequest objModel);
        List<CHCRequest> chcrequest_data(string empcode);
        CHCRequest Getsingleempdetails(CHCRequest objModel);
        ResponseModel ApproveCHCRequest(CHCRequest objModel);
        List<CHCRequest> Pastdetail(CHCRequest objModel);
        ResponseModel CheckPriviousClaim(CHCRequest objModel);
        ResponseModel SaveForLater(CHCRequest objModel);
        CHCRequest GetEmplAge(CHCRequest objModel);
    }
}