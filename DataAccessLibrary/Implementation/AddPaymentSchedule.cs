﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLibrary.Interfaces;
using EntityModelLibrary.Models;

namespace DataAccessLibrary.Implementation
{
    public class AddPaymentSchedule:IAddPaymentSchedule
    {
        private readonly DbContextForTest _dbContext;
        private readonly DbContextForProdOld _dbContextProdOld;
        public AddPaymentSchedule(DbContextForTest dbContext, DbContextForProdOld dbContextProdOld)
        {
            _dbContext = dbContext;
            _dbContextProdOld = dbContextProdOld;
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
                        var noteMaster = new LcgPaymentSchedule()
                        {
                            CardInfoId = paymentScheduleObj.CardInfoId,
                            EffectiveDate = paymentDate,
                            IsActive = true,
                            NumerOfPayments = i,
                            PatientAccount = paymentScheduleObj.PatientAccount
                        };
                        await _dbContext.LcgPaymentSchedules.AddAsync(noteMaster);
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
                            NumerOfPayments = i,
                            PatientAccount = paymentScheduleObj.PatientAccount
                        };
                        await _dbContextProdOld.LcgPaymentSchedules.AddAsync(noteMaster);
                        paymentDate = paymentDate.AddMonths(1);
                    }
                    await _dbContextProdOld.SaveChangesAsync();
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
                            NumerOfPayments = i,
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
    }
}
