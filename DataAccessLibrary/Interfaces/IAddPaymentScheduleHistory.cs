using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityModelLibrary.Models;

namespace DataAccessLibrary.Interfaces
{
    public  interface IAddPaymentScheduleHistory
    {
        Task<string> SavePaymentScheduleHistory(LcgPaymentScheduleHistory paymentScheduleHistoryObj, string environment);
    }
}
