using MedicalR.Models;
using MedicalR.Models.CHC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedicalR.DataAccessLayer.IDAL.CHCAdmin
{
    public interface IDALCHCApproveRequestform
    {
        CHCRequest Getsingleempdetails(CHCRequest objModel);
        List<CHCRequest> chcrequest_data();
        ResponseModel ApproveCHCRequest(CHCRequest objModel);
        ResponseModel RejectCHCRequest(CHCRequest objModel);
        List<CHCBillProcess> CHCBillProcess();
        ResponseModel AddBillProcess(CHCBillProcess objModel);
        CHCBillProcess GetsingleEmplBill(CHCBillProcess objModel);
        ResponseModel HoldBillProcess(CHCBillProcess objModel);
        List<CHCBillProcess> CremplsalAcc();
        CHCBillProcess GetsingleEmplCr(CHCBillProcess objModel);
        ResponseModel CrToEmplAcc(CHCBillProcess objModel);
        ResponseModel AddCreaditTosalAcc(List<CHCBillProcess> objModel, DateTime CR_DATE);
        ResponseModel SaveforLaterAddCreaditTosalAcc(List<CHCBillProcess> objModel, DateTime CR_DATE);
        List<CHCBillProcess> forwardcheque();
        CHCBillProcess GetSingleCHC(CHCBillProcess objModel);
        ResponseModel forwardcheque(CHCBillProcess objModel);
        List<CHCRequest> Pastdetail(CHCRequest objModel);
        List<CHCBillProcess> OfficeNoteData(string lotno);
        List<CHCBillProcess> BankAdviseData(string lotno);
        List<CHCBillProcess> EnclosureEmplSummary();
        List<CHCBillProcess> LotGenration();
        ResponseModel CHCLotGenerationStart(List<CHCBillProcess> model);
        List<CHCBillProcess> GetLoTNo();
        List<CHCBillProcess> GetdatabyLotNo(CHCBillProcess objmodel);
        byte[] GetCHCOfficeNotepdf(string lotno);
        byte[] GetCHCBankadvicepdf(string lotno);
    }
}