using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLibrary.Interfaces;
using EntityModelLibrary.Models;
using EntityModelLibrary.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace DataAccessLibrary.Implementation
{
    public class GetDetailsOfPreSchedulePayment : IGetDetailsOfPreSchedulePayment
    {
        private readonly DbContextForTest _dbContext;
        private readonly DbContextForProdOld _dbContextProdOld;
        private readonly DbContextForProd _dbContextForProd;
        public GetDetailsOfPreSchedulePayment(DbContextForTest dbContext, DbContextForProdOld dbContextProdOld,
            DbContextForProd dbContextForProd)
        {
            _dbContext = dbContext;
            _dbContextProdOld = dbContextProdOld;
            _dbContextForProd = dbContextForProd;
        }
        public async Task<LcgTablesViewModel> GetDetailsOfPreSchedulePaymentInfo(int paymentScheduleId, string environment)
        {

            if (environment == "T")
            {
                var viewModel = await (from paymentSchedule in _dbContext.LcgPaymentSchedules
                                       join cardInfo in _dbContext.LcgCardInfos on paymentSchedule.CardInfoId equals cardInfo.Id
                                       //join paymentScheduleHistory in _dbContext.LcgPaymentScheduleHistories 
                                       //    on paymentSchedule.Id equals paymentScheduleHistory.PaymentScheduleId
                                       where paymentSchedule.Id == paymentScheduleId
                                       select new
                                       {
                                           CardInfo = cardInfo,
                                           PaymentSchedule = paymentSchedule,
                                           //PaymentScheduleHistory=paymentScheduleHistory,
                                       }).SingleOrDefaultAsync();
                var response = new LcgTablesViewModel()
                {
                    //cardInfo
                    IdForCardInfo = viewModel.CardInfo.Id,
                    PaymentMethodId = viewModel.CardInfo.PaymentMethodId,
                    EntryMode = viewModel.CardInfo.EntryMode,
                    Type = viewModel.CardInfo.Type,
                    BinNumber = viewModel.CardInfo.BinNumber,
                    LastFour = viewModel.CardInfo.LastFour,
                    ExpirationMonth = viewModel.CardInfo.ExpirationMonth,
                    ExpirationYear = viewModel.CardInfo.ExpirationYear,
                    AssociateDebtorAcct = viewModel.CardInfo.AssociateDebtorAcct,
                    CardHolderName = viewModel.CardInfo.CardHolderName,
                    IsActiveForCardInfo = viewModel.CardInfo.IsActive,
                    //paymentSchedule
                    IdForPaymentSchedule = viewModel.PaymentSchedule.Id,
                    PatientAccount = viewModel.PaymentSchedule.PatientAccount,
                    CardInfoId = viewModel.PaymentSchedule.CardInfoId,
                    Amount = viewModel.PaymentSchedule.Amount,
                    NumberOfPayments = viewModel.PaymentSchedule.NumberOfPayments,
                    IsActiveForPaymentSchedule = viewModel.PaymentSchedule.IsActive,
                    //history
                    //IdForPaymentScheduleHistory = viewModel.PaymentScheduleHistory.Id,
                    //PaymentScheduleId = viewModel.PaymentScheduleHistory.PaymnetScheduleId,
                    //TransactionId = viewModel.PaymentScheduleHistory.TransactionId,
                    //ResponseCode = viewModel.PaymentScheduleHistory.ResponseCode,
                    //ResponseMessage = viewModel.PaymentScheduleHistory.ResponseMessage,
                    //AuthorizationNumber = viewModel.PaymentScheduleHistory.AuthorizationNumber,
                    //AuthorizationText = viewModel.PaymentScheduleHistory.AuthorizationText
                };
                return response;
            }
            else if (environment == "PO")
            {
                var viewModel = await (from paymentSchedule in _dbContextProdOld.LcgPaymentSchedules
                                       join cardInfo in _dbContextProdOld.LcgCardInfos on paymentSchedule.CardInfoId equals cardInfo.Id
                                       //join paymentScheduleHistory in _dbContext.LcgPaymentScheduleHistories 
                                       //    on paymentSchedule.Id equals paymentScheduleHistory.PaymentScheduleId
                                       where paymentSchedule.Id == paymentScheduleId
                                       select new
                                       {
                                           CardInfo = cardInfo,
                                           PaymentSchedule = paymentSchedule,
                                           //PaymentScheduleHistory=paymentScheduleHistory,
                                       }).SingleOrDefaultAsync();
                var response = new LcgTablesViewModel()
                {
                    //cardInfo
                    IdForCardInfo = viewModel.CardInfo.Id,
                    PaymentMethodId = viewModel.CardInfo.PaymentMethodId,
                    EntryMode = viewModel.CardInfo.EntryMode,
                    Type = viewModel.CardInfo.Type,
                    BinNumber = viewModel.CardInfo.BinNumber,
                    LastFour = viewModel.CardInfo.LastFour,
                    ExpirationMonth = viewModel.CardInfo.ExpirationMonth,
                    ExpirationYear = viewModel.CardInfo.ExpirationYear,
                    AssociateDebtorAcct = viewModel.CardInfo.AssociateDebtorAcct,
                    CardHolderName = viewModel.CardInfo.CardHolderName,
                    IsActiveForCardInfo = viewModel.CardInfo.IsActive,
                    //paymentSchedule
                    IdForPaymentSchedule = viewModel.PaymentSchedule.Id,
                    PatientAccount = viewModel.PaymentSchedule.PatientAccount,
                    CardInfoId = viewModel.PaymentSchedule.CardInfoId,
                    Amount = viewModel.PaymentSchedule.Amount,
                    NumberOfPayments = viewModel.PaymentSchedule.NumberOfPayments,
                    IsActiveForPaymentSchedule = viewModel.PaymentSchedule.IsActive,
                    //history
                    //IdForPaymentScheduleHistory = viewModel.PaymentScheduleHistory.Id,
                    //PaymentScheduleId = viewModel.PaymentScheduleHistory.PaymnetScheduleId,
                    //TransactionId = viewModel.PaymentScheduleHistory.TransactionId,
                    //ResponseCode = viewModel.PaymentScheduleHistory.ResponseCode,
                    //ResponseMessage = viewModel.PaymentScheduleHistory.ResponseMessage,
                    //AuthorizationNumber = viewModel.PaymentScheduleHistory.AuthorizationNumber,
                    //AuthorizationText = viewModel.PaymentScheduleHistory.AuthorizationText
                };
                return response;
            }
            else if (environment == "P")
            {
                var viewModel = await (from paymentSchedule in _dbContextForProd.LcgPaymentSchedules
                                       join cardInfo in _dbContextForProd.LcgCardInfos on paymentSchedule.CardInfoId equals cardInfo.Id
                                       //join paymentScheduleHistory in _dbContext.LcgPaymentScheduleHistories 
                                       //    on paymentSchedule.Id equals paymentScheduleHistory.PaymentScheduleId
                                       where paymentSchedule.Id == paymentScheduleId
                                       select new
                                       {
                                           CardInfo = cardInfo,
                                           PaymentSchedule = paymentSchedule,
                                           //PaymentScheduleHistory=paymentScheduleHistory,
                                       }).SingleOrDefaultAsync();
                var response = new LcgTablesViewModel()
                {
                    //cardInfo
                    IdForCardInfo = viewModel.CardInfo.Id,
                    PaymentMethodId = viewModel.CardInfo.PaymentMethodId,
                    EntryMode = viewModel.CardInfo.EntryMode,
                    Type = viewModel.CardInfo.Type,
                    BinNumber = viewModel.CardInfo.BinNumber,
                    LastFour = viewModel.CardInfo.LastFour,
                    ExpirationMonth = viewModel.CardInfo.ExpirationMonth,
                    ExpirationYear = viewModel.CardInfo.ExpirationYear,
                    AssociateDebtorAcct = viewModel.CardInfo.AssociateDebtorAcct,
                    CardHolderName = viewModel.CardInfo.CardHolderName,
                    IsActiveForCardInfo = viewModel.CardInfo.IsActive,
                    //paymentSchedule
                    IdForPaymentSchedule = viewModel.PaymentSchedule.Id,
                    PatientAccount = viewModel.PaymentSchedule.PatientAccount,
                    CardInfoId = viewModel.PaymentSchedule.CardInfoId,
                    Amount = viewModel.PaymentSchedule.Amount,
                    NumberOfPayments = viewModel.PaymentSchedule.NumberOfPayments,
                    IsActiveForPaymentSchedule = viewModel.PaymentSchedule.IsActive,
                    //history
                    //IdForPaymentScheduleHistory = viewModel.PaymentScheduleHistory.Id,
                    //PaymentScheduleId = viewModel.PaymentScheduleHistory.PaymnetScheduleId,
                    //TransactionId = viewModel.PaymentScheduleHistory.TransactionId,
                    //ResponseCode = viewModel.PaymentScheduleHistory.ResponseCode,
                    //ResponseMessage = viewModel.PaymentScheduleHistory.ResponseMessage,
                    //AuthorizationNumber = viewModel.PaymentScheduleHistory.AuthorizationNumber,
                    //AuthorizationText = viewModel.PaymentScheduleHistory.AuthorizationText
                };
                return response;
            }
            else
            {
                var viewModel = await (from paymentSchedule in _dbContext.LcgPaymentSchedules
                                       join cardInfo in _dbContext.LcgCardInfos on paymentSchedule.CardInfoId equals cardInfo.Id
                                       //join paymentScheduleHistory in _dbContext.LcgPaymentScheduleHistories 
                                       //    on paymentSchedule.Id equals paymentScheduleHistory.PaymentScheduleId
                                       where paymentSchedule.Id == paymentScheduleId
                                       select new
                                       {
                                           CardInfo = cardInfo,
                                           PaymentSchedule = paymentSchedule,
                                           //PaymentScheduleHistory=paymentScheduleHistory,
                                       }).SingleOrDefaultAsync();
                var response = new LcgTablesViewModel()
                {
                    //cardInfo
                    IdForCardInfo = viewModel.CardInfo.Id,
                    PaymentMethodId = viewModel.CardInfo.PaymentMethodId,
                    EntryMode = viewModel.CardInfo.EntryMode,
                    Type = viewModel.CardInfo.Type,
                    BinNumber = viewModel.CardInfo.BinNumber,
                    LastFour = viewModel.CardInfo.LastFour,
                    ExpirationMonth = viewModel.CardInfo.ExpirationMonth,
                    ExpirationYear = viewModel.CardInfo.ExpirationYear,
                    AssociateDebtorAcct = viewModel.CardInfo.AssociateDebtorAcct,
                    CardHolderName = viewModel.CardInfo.CardHolderName,
                    IsActiveForCardInfo = viewModel.CardInfo.IsActive,
                    //paymentSchedule
                    IdForPaymentSchedule = viewModel.PaymentSchedule.Id,
                    PatientAccount = viewModel.PaymentSchedule.PatientAccount,
                    CardInfoId = viewModel.PaymentSchedule.CardInfoId,
                    Amount = viewModel.PaymentSchedule.Amount,
                    NumberOfPayments = viewModel.PaymentSchedule.NumberOfPayments,
                    IsActiveForPaymentSchedule = viewModel.PaymentSchedule.IsActive,
                    //history
                    //IdForPaymentScheduleHistory = viewModel.PaymentScheduleHistory.Id,
                    //PaymentScheduleId = viewModel.PaymentScheduleHistory.PaymnetScheduleId,
                    //TransactionId = viewModel.PaymentScheduleHistory.TransactionId,
                    //ResponseCode = viewModel.PaymentScheduleHistory.ResponseCode,
                    //ResponseMessage = viewModel.PaymentScheduleHistory.ResponseMessage,
                    //AuthorizationNumber = viewModel.PaymentScheduleHistory.AuthorizationNumber,
                    //AuthorizationText = viewModel.PaymentScheduleHistory.AuthorizationText
                };
                return response;
            }

        }
    }
}
