using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityModelLibrary.ViewModels;

namespace DataAccessLibrary.Interfaces
{
    public interface IGetDetailsOfPreSchedulePayment
    {
        Task<LcgTablesViewModel> GetDetailsOfPreSchedulePaymentInfo(int paymentScheduleId, string environment);
    }
}
