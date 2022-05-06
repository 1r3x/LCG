using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiAccessLibrary.ApiModels;

namespace ApiAccessLibrary.Interfaces
{
    public interface IPayment
    {
        Task<string> PaymentPlan(PaymentPlanRequestModel requestModel);
    }
}
