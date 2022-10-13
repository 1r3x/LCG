using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityModelLibrary.Models;

namespace DataAccessLibrary.Interfaces
{
    public interface IAddPaymentSchedule
    {
        Task<string> SavePaymentSchedule(LcgPaymentSchedule paymentScheduleObj,int numberOfPayments, string environment);
        Task<string> InactivePaymentSchedule(int paymentScheduleId, string environment);
    }
}
