using MedicalR.Models;
using MedicalR.Models.MedicalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalR.DataAccessLayer.IDAL.MedicalR
{
    public interface IDALProcessPayment
    {
        List<ProcessPaymentModel> ProcessPayment_grid_data();
        List<ProcessPaymentModel> EmployeeWiseSummary();
        ResponseModel Processpayment(List<ProcessPaymentModel> objModel, DateTime date_of_payment);
        ResponseModel Processpayment(ProcessPaymentModel objModel);
    }
}
