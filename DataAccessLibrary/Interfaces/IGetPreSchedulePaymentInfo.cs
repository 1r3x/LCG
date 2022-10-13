using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityModelLibrary.Models;
using EntityModelLibrary.ViewModels;

namespace DataAccessLibrary.Interfaces
{
    public interface IGetPreSchedulePaymentInfo
    {
        Task<IList<LcgPaymentSchedule>> GetAllPreSchedulePaymentInfo(string environment);
        //Task<List<LcgTablesViewModel>> GetAllPaymentDetailsForToday(string environment);
    }
}
