using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLibrary.Interfaces;
using EntityModelLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLibrary.Implementation
{
    public class AddPaymentSchedule : IAddPaymentSchedule
    {
        private readonly DbContextForTest _dbContext;
        private readonly DbContextForProdOld _dbContextProdOld;
        private readonly DbContextForProd _dbContextForProd;
        public AddPaymentSchedule(DbContextForTest dbContext, DbContextForProdOld dbContextProdOld,
            DbContextForProd dbContextForProd)
        {
            _dbContext = dbContext;
            _dbContextProdOld = dbContextProdOld;
            _dbContextForProd = dbContextForProd;
        }

        public async Task<string> SavePaymentSchedule(LcgPaymentSchedule paymentScheduleObj, int numberOfPayments, string environment)
        {
            try
            {
                if (environment == "T")
                {
                    var paymentDate = paymentScheduleObj.EffectiveDate;
                    for (var i = 1; i <= numberOfPayments; i++)
                    {
                        var LcgPaymentScheduleObj = new LcgPaymentSchedule()
                        {
                            CardInfoId = paymentScheduleObj.CardInfoId,
                            EffectiveDate = paymentDate,
                            IsActive = true,
                            NumberOfPayments = i,
                            PatientAccount = paymentScheduleObj.PatientAccount
                        };
                        await _dbContext.LcgPaymentSchedules.AddAsync(LcgPaymentScheduleObj);

                        paymentDate = paymentDate.AddMonths(1);
                    }
                    await _dbContext.SaveChangesAsync();
                }
                else if (environment == "PO")
                {
                    var paymentDate = paymentScheduleObj.EffectiveDate;
                    for (var i = 0; i < numberOfPayments; i++)
                    {
                        var noteMaster = new LcgPaymentSchedule()
                        {
                            CardInfoId = paymentScheduleObj.CardInfoId,
                            EffectiveDate = paymentDate,
                            IsActive = true,
                            NumberOfPayments = i,
                            PatientAccount = paymentScheduleObj.PatientAccount
                        };
                        await _dbContextProdOld.LcgPaymentSchedules.AddAsync(noteMaster);
                        paymentDate = paymentDate.AddMonths(1);
                    }
                    await _dbContextProdOld.SaveChangesAsync();
                }
                else if (environment == "P")
                {
                    var paymentDate = paymentScheduleObj.EffectiveDate;
                    for (var i = 0; i < numberOfPayments; i++)
                    {
                        var noteMaster = new LcgPaymentSchedule()
                        {
                            CardInfoId = paymentScheduleObj.CardInfoId,
                            EffectiveDate = paymentDate,
                            IsActive = true,
                            NumberOfPayments = i,
                            PatientAccount = paymentScheduleObj.PatientAccount
                        };
                        await _dbContextForProd.LcgPaymentSchedules.AddAsync(noteMaster);
                        paymentDate = paymentDate.AddMonths(1);
                    }
                    await _dbContextForProd.SaveChangesAsync();
                }
                else
                {
                    var paymentDate = paymentScheduleObj.EffectiveDate;
                    for (var i = 0; i < numberOfPayments; i++)
                    {
                        var noteMaster = new LcgPaymentSchedule()
                        {
                            CardInfoId = paymentScheduleObj.CardInfoId,
                            EffectiveDate = paymentDate,
                            IsActive = true,
                            NumberOfPayments = i,
                            PatientAccount = paymentScheduleObj.PatientAccount
                        };
                        await _dbContext.LcgPaymentSchedules.AddAsync(noteMaster);
                        paymentDate = paymentDate.AddMonths(1);
                    }
                    await _dbContext.SaveChangesAsync();
                }

            }
            catch (Exception e)
            {
                return e.Message;
            }

            return "Payment Schedules added successfully";
        }

        public async Task<string> InactivePaymentSchedule(int paymentScheduleId, string environment)
        {
            try
            {
                if (environment == "T")
                {
                    var paymentScheduleUpdate =
                        _dbContext.LcgPaymentSchedules.FirstAsync(x => x.Id == paymentScheduleId);
                    paymentScheduleUpdate.Result.IsActive = false;
                    await _dbContext.SaveChangesAsync();
                }
                else if (environment == "PO")
                {
                    var paymentScheduleUpdate =
                        _dbContextProdOld.LcgPaymentSchedules.FirstAsync(x => x.Id == paymentScheduleId);
                    paymentScheduleUpdate.Result.IsActive = false;
                    await _dbContextProdOld.SaveChangesAsync();
                }
                else
                {
                    var paymentScheduleUpdate =
                        _dbContext.LcgPaymentSchedules.FirstAsync(x => x.Id == paymentScheduleId);
                    paymentScheduleUpdate.Result.IsActive = false;
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            return "Payment Schedules inactive successfully";

        }
    }
}
