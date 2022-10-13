using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLibrary.Interfaces;
using EntityModelLibrary.Models;

namespace DataAccessLibrary.Implementation
{
    public class AddPaymentScheduleHistory:IAddPaymentScheduleHistory
    {
        private readonly DbContextForTest _dbContext;
        private readonly DbContextForProdOld _dbContextProdOld;
        private readonly DbContextForProd _dbContextForProd;
        public AddPaymentScheduleHistory(DbContextForTest dbContext, DbContextForProdOld dbContextProdOld,
            DbContextForProd dbContextForProd)
        {
            _dbContext = dbContext;
            _dbContextProdOld = dbContextProdOld;
            _dbContextForProd = dbContextForProd;
        }

        public async Task<string> SavePaymentScheduleHistory(LcgPaymentScheduleHistory paymentScheduleHistoryObj, string environment)
        {
            try
            {
                if (environment == "T")
                {
                    var lcgPaymentScheduleHistoryObj = new LcgPaymentScheduleHistory()
                        {
                            ResponseCode = paymentScheduleHistoryObj.ResponseCode,
                            AuthorizationNumber = paymentScheduleHistoryObj.AuthorizationNumber,
                            AuthorizationText = paymentScheduleHistoryObj.AuthorizationText,
                            PaymentScheduleId = paymentScheduleHistoryObj.PaymentScheduleId,
                            ResponseMessage = paymentScheduleHistoryObj.ResponseMessage,
                            TimeLog = DateTime.Now,
                            TransactionId = paymentScheduleHistoryObj.TransactionId
                        };
                        await _dbContext.LcgPaymentScheduleHistories.AddAsync(lcgPaymentScheduleHistoryObj);
                        await _dbContext.SaveChangesAsync();
                }
                else if (environment == "PO")
                {
                    var lcgPaymentScheduleHistoryObj = new LcgPaymentScheduleHistory()
                    {
                        ResponseCode = paymentScheduleHistoryObj.ResponseCode,
                        AuthorizationNumber = paymentScheduleHistoryObj.AuthorizationNumber,
                        AuthorizationText = paymentScheduleHistoryObj.AuthorizationText,
                        PaymentScheduleId = paymentScheduleHistoryObj.PaymentScheduleId,
                        ResponseMessage = paymentScheduleHistoryObj.ResponseMessage,
                        TimeLog = DateTime.Now,
                        TransactionId = paymentScheduleHistoryObj.TransactionId
                    };
                    await _dbContextProdOld.LcgPaymentScheduleHistories.AddAsync(lcgPaymentScheduleHistoryObj);
                    await _dbContextProdOld.SaveChangesAsync();
                }
                else if (environment == "P")
                {
                    var lcgPaymentScheduleHistoryObj = new LcgPaymentScheduleHistory()
                    {
                        ResponseCode = paymentScheduleHistoryObj.ResponseCode,
                        AuthorizationNumber = paymentScheduleHistoryObj.AuthorizationNumber,
                        AuthorizationText = paymentScheduleHistoryObj.AuthorizationText,
                        PaymentScheduleId = paymentScheduleHistoryObj.PaymentScheduleId,
                        ResponseMessage = paymentScheduleHistoryObj.ResponseMessage,
                        TimeLog = DateTime.Now,
                        TransactionId = paymentScheduleHistoryObj.TransactionId
                    };
                    await _dbContextForProd.LcgPaymentScheduleHistories.AddAsync(lcgPaymentScheduleHistoryObj);
                    await _dbContextForProd.SaveChangesAsync();
                }
                else
                {
                    var lcgPaymentScheduleHistoryObj = new LcgPaymentScheduleHistory()
                    {
                        ResponseCode = paymentScheduleHistoryObj.ResponseCode,
                        AuthorizationNumber = paymentScheduleHistoryObj.AuthorizationNumber,
                        AuthorizationText = paymentScheduleHistoryObj.AuthorizationText,
                        PaymentScheduleId = paymentScheduleHistoryObj.PaymentScheduleId,
                        ResponseMessage = paymentScheduleHistoryObj.ResponseMessage,
                        TimeLog = DateTime.Now,
                        TransactionId = paymentScheduleHistoryObj.TransactionId
                    };
                    await _dbContext.LcgPaymentScheduleHistories.AddAsync(lcgPaymentScheduleHistoryObj);
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
