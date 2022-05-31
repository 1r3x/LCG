using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLibrary.Interfaces;
using EntityModelLibrary.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace DataAccessLibrary.Implementation
{
    public class GetPreSchedulePaymentInfo : IGetPreSchedulePaymentInfo
    {
        private readonly DbContextForTest _dbContext;
        private readonly DbContextForProdOld _dbContextProdOld;
        public GetPreSchedulePaymentInfo(DbContextForTest dbContext, DbContextForProdOld dbContextProdOld)
        {
            _dbContext = dbContext;
            _dbContextProdOld = dbContextProdOld;
        }
        public async Task<IList<LcgPaymentSchedule>> GetAllPreSchedulePaymentInfo(string environment)
        {
            //todo after quality check should  active the code block
            //DateTime startDate;
            //DateTime endDate;
            //if (DateAndTime.Now.DayOfWeek.ToString() == "Monday")
            //{
            //    startDate = DateTime.Now.AddDays(-1);
            //    endDate = DateTime.Now;
            //}
            //else
            //{
            //    startDate = DateTime.Now;
            //    endDate = DateTime.Now;
            //}
            //if (environment == "T")
            //{
            //    return await _dbContext.LcgPaymentSchedules.
            //        Where(x => x.EffectiveDate >= startDate && x.EffectiveDate <= endDate).ToListAsync();
            //}
            //else if (environment=="PO")
            //{
            //    return await _dbContextProdOld.LcgPaymentSchedules.
            //        Where(x => x.EffectiveDate >= startDate && x.EffectiveDate <= endDate).ToListAsync();
            //}
            //else
            //{
            //    return await _dbContext.LcgPaymentSchedules.
            //        Where(x => x.EffectiveDate >= startDate && x.EffectiveDate <= endDate).ToListAsync(); ;
            //}

            return await _dbContext.LcgPaymentSchedules.ToListAsync();
        }
    }
}
