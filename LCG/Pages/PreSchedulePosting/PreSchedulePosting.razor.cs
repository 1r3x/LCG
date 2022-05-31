using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApiAccessLibrary.ApiModels;
using ApiAccessLibrary.Interfaces;
using DataAccessLibrary.Implementation;
using DataAccessLibrary.Interfaces;
using EntityModelLibrary.Models;
using LCG.Data;
using LCG.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace LCG.Pages.PreSchedulePosting
{
    public partial class PreSchedulePosting
    {
        [Inject] private IGetPreSchedulePaymentInfo GetPreSchedulePaymentInfo { get; set; }
        [Inject] private IGetDetailsOfPreSchedulePayment GetDetailsOfPreSchedulePayment { get; set; }
        [Inject] private IPopulateDataForProcessSales PopulateData { get; set; }
        [Inject] private IAddCcPayment AddCcPayment { get; set; }
        [Inject] private DbContextForTest DbContext { get; set; }
        [Inject] private DbContextForProdOld DbContextProdOld { get; set; }
        [Inject] private IProcessSaleTransactions SaleApi { get; set; }
        [Inject] private IAddNotes AddNotes { get; set; }

        private IList<LcgPaymentSchedule> _paymentSchedule;
        private LcgTablesViewModel _preScheduleLcgTablesViewModel;


        private ViewSaleResponseModel _responseModel;
        private readonly DateTime _scheduleDateTime = DateTime.Now;
        private string _errorModel;
        private decimal _tempAmount;
        private int _loadingBar;
        private bool _busyClick;

        protected override async Task OnInitializedAsync()
        {
            _paymentSchedule = await GetPreSchedulePaymentInfo.GetAllPreSchedulePaymentInfo("T");
            _loadingBar = 0;

        }

        async Task OpenOrder(int orderId)
        {
            _responseModel = null;
            _errorModel = null;
            _busyClick = true;
            _loadingBar = 1;
            _preScheduleLcgTablesViewModel =
                await GetDetailsOfPreSchedulePayment.GetDetailsOfPreSchedulePaymentInfo(orderId, "T");
            _tempAmount = _preScheduleLcgTablesViewModel.Amount;
            await ProcessSaleTrans();
            _loadingBar = 0;
            _paymentSchedule = await GetPreSchedulePaymentInfo.GetAllPreSchedulePaymentInfo("T");
            _busyClick = false;
        }
        private async Task ProcessSaleTrans()
        {
            _responseModel = null;
            _errorModel = null;
            var saleRequestModel = new SaleRequestModel()
            {
                Outlet = new ApiAccessLibrary.ApiModels.Outlet()
                {
                    MerchantID = "192837645",
                    StoreID = "0001",
                    TerminalID = "0001"
                },
                Amount = _preScheduleLcgTablesViewModel.Amount,
                PaymentMethod = "OnFile",
                PaymentMethodID = _preScheduleLcgTablesViewModel.PaymentMethodId,
                Patient = new ApiAccessLibrary.ApiModels.Patient()
                {
                    AccountNumber = _preScheduleLcgTablesViewModel.PatientAccount
                }
            };
            try
            {
                var resultVerify = await SaleApi.PostProcessSalesTransactionAsync(saleRequestModel);
                if (resultVerify.Contains("FieldErrors"))
                {
                    _errorModel = resultVerify;
                }
                else
                {
                    _responseModel = new ViewSaleResponseModel(resultVerify);

                }

                string noteText = null;
                if (@_responseModel != null && _responseModel.ResponseCode == "000")
                {
                    noteText = "INSTAMED CC APPROVED FOR $" + _tempAmount + " " + @_responseModel.ResponseMessage.ToUpper() +
                                  " AUTH #:" + @_responseModel.AuthorizationNumber;
                    await SaveCardInfoAndScheduleData();

                }
                else
                {
                    if (@_responseModel != null)
                        noteText = "INSTAMED CC DECLINED FOR $" + _tempAmount + " " +
                                   @_responseModel.ResponseMessage.ToUpper() +
                                   " AUTH #:" + @_responseModel.AuthorizationNumber;
                }
                //actual employee =31950
                //todo debtor account
                //var debtorAcct = (await PopulateData.GetDebtorAccountNoByPatientAcct(_preScheduleLcgTablesViewModel.PatientAccount, "T")).ToString();
                //todo remove demo acct
                await AddNotes.Notes(_preScheduleLcgTablesViewModel.AssociateDebtorAcct, 31950, "RA", noteText, "N", null, "T");//PO for prod_old & T is for test_db


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }


        }

        private async Task SaveCardInfoAndScheduleData()
        {
            try
            {

                var paymentScheduleExample = new LcgPaymentScheduleHistory()
                {
                    ResponseCode = _responseModel.ResponseCode,
                    AuthorizationNumber = _responseModel.AuthorizationNumber,
                    AuthorizationText = Environment.UserName,
                    ResponseMessage = _responseModel.ResponseMessage,
                    PaymentScheduleId = _preScheduleLcgTablesViewModel.IdForPaymentSchedule,
                    TransactionId = _responseModel.TransactionId
                };

                DbContext.LcgPaymentScheduleHistories.Add(paymentScheduleExample);
                await DbContext.SaveChangesAsync();

                var paymentScheduleUpdate =
                    DbContext.LcgPaymentSchedules.FirstAsync(x => x.Id == _preScheduleLcgTablesViewModel.IdForPaymentSchedule);
                paymentScheduleUpdate.Result.IsActive = false;
                await DbContext.SaveChangesAsync();

                var ccPaymentObj = new CcPayment()
                {
                    DebtorAcct = _preScheduleLcgTablesViewModel.PatientAccount,
                    Company = "TOTAL CREDIT RECOVERY",
                    //UserId = Environment.UserName,
                    //UserName = Environment.UserName + " -LCG",
                    UserId = "LCG",
                    UserName = "PD -LCG",
                    ChargeTotal = _preScheduleLcgTablesViewModel.Amount,
                    Subtotal = _preScheduleLcgTablesViewModel.Amount,
                    PaymentDate = _scheduleDateTime,
                    ApprovalStatus = "APPROVED",
                    //todo card holder it must be saved from multiple page
                    BillingName = _preScheduleLcgTablesViewModel.CardHolderName,
                    ApprovalCode = _responseModel.ResponseCode,
                    OrderNumber = _responseModel.TransactionId,
                    RefNumber = "INSTAMEDLH",
                    Sif = "N"
                };
                await AddCcPayment.CreateCcPayment(ccPaymentObj, "T");
            }
            catch (Exception)
            {

                throw;
            }


        }

    }


}
