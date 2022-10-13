using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLibrary.Interfaces;
using EntityModelLibrary.Models;
using EntityModelLibrary.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace DataAccessLibrary.Implementation
{
    public class GetPreSchedulePaymentInfo : IGetPreSchedulePaymentInfo
    {
        private readonly DbContextForTest _dbContext;
        private readonly DbContextForProdOld _dbContextProdOld;
        private readonly DbContextForProd _dbContextForProd;
        public GetPreSchedulePaymentInfo(DbContextForTest dbContext, DbContextForProdOld dbContextProdOld,
            DbContextForProd dbContextForProd)
        {
            _dbContext = dbContext;
            _dbContextProdOld = dbContextProdOld;
            _dbContextForProd = dbContextForProd;
        }

        public async Task<IList<LcgPaymentSchedule>> GetAllPreSchedulePaymentInfo(string environment)
        {
            //todo after quality check should active the code block
            DateTime startDate;
            DateTime endDate;
            if (DateAndTime.Now.DayOfWeek.ToString() == "Monday")
            {
                startDate = DateTime.Now.AddDays(-1);
                endDate = DateTime.Now;
            }
            else
            {
                startDate = DateTime.Now;
                endDate = DateTime.Now;
            }
            if (environment == "T")
            {
                return await (from schedule in _dbContext.LcgPaymentSchedules
                              join card in _dbContext.LcgCardInfos on schedule.CardInfoId equals card.Id
                              where schedule.EffectiveDate >= startDate && schedule.EffectiveDate <= endDate && card.AssociateDebtorAcct.Substring(0, 4) == "4950"
                              select schedule).ToListAsync();


                //return await _dbContext.LcgPaymentSchedules.
                //    Where(x => x.EffectiveDate >= startDate && x.EffectiveDate <= endDate).ToListAsync();
            }
            else if (environment == "PO")
            {
                return await (from schedule in _dbContextProdOld.LcgPaymentSchedules
                              join card in _dbContextProdOld.LcgCardInfos on schedule.CardInfoId equals card.Id
                              where schedule.EffectiveDate >= startDate && schedule.EffectiveDate <= endDate && card.AssociateDebtorAcct.Substring(0, 4) == "4950"
                              select schedule).ToListAsync();

                //return await _dbContextProdOld.LcgPaymentSchedules.
                //    Where(x => x.EffectiveDate >= startDate && x.EffectiveDate <= endDate).ToListAsync();
            }
            else if (environment == "P")
            {

                return await (from schedule in _dbContextForProd.LcgPaymentSchedules
                              join card in _dbContextForProd.LcgCardInfos on schedule.CardInfoId equals card.Id
                              where schedule.EffectiveDate >= startDate && schedule.EffectiveDate <= endDate && card.AssociateDebtorAcct.Substring(0, 4) == "4950"
                              select schedule).ToListAsync();

                //return await _dbContextForProd.LcgPaymentSchedules.
                //    Where(x => x.EffectiveDate >= startDate && x.EffectiveDate <= endDate).ToListAsync();
            }
            else
            {
                return await (from schedule in _dbContext.LcgPaymentSchedules
                              join card in _dbContext.LcgCardInfos on schedule.CardInfoId equals card.Id
                              where schedule.EffectiveDate >= startDate && schedule.EffectiveDate <= endDate && card.AssociateDebtorAcct.Substring(0, 4) == "4950"
                              select schedule).ToListAsync();

                //return await _dbContext.LcgPaymentSchedules.
                //    Where(x => x.EffectiveDate >= startDate && x.EffectiveDate <= endDate).ToListAsync();
            }

            //return await _dbContext.LcgPaymentSchedules.ToListAsync();
        }
    }
}
