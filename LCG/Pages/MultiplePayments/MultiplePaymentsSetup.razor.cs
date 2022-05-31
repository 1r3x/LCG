using System;
using System.Linq.Dynamic.Core;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using ApiAccessLibrary.ApiModels;
using ApiAccessLibrary.Interfaces;
using DataAccessLibrary.Interfaces;
using EntityModelLibrary.Models;
using LCG.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Radzen;

namespace LCG.Pages.MultiplePayments
{
    public partial class MultiplePaymentsSetup
    {

        [Inject] private IAddNotes AddNotes { get; set; }
        [Inject] private IPopulateDataForProcessSales PopulateData { get; set; }
        [Parameter] public string DebtorAcct { get; set; }
        [Inject] private IProcessSaleTransactions SaleApi { get; set; }
        [Inject] private IProcessCardAuthorization CardApi { get; set; }
        [Inject] private IPayment Tokenization { get; set; }
        [Inject] private IAddCardInfo AddCardInfo { get; set; }
        [Inject] private IAddPaymentSchedule AddPaymentSchedule { get; set; }
        [Inject] private DbContextForTest DbContext { get; set; }
        [Inject] private DbContextForProdOld DbContextProdOld { get; set; }
        [Inject] private IAddCcPayment AddCcPayment { get; set; }
        
        private  ViewMultiplePaymentsRequestModel _viewRequestModel = new();
        private ViewSaleResponseModel _responseModel;
        private int _numberOfPayment = 1;
        private DateTime _scheduleDateTime = DateTime.Now;
        private string _errorModel;
        private int _loadingBar;
        private decimal _tempAmount;
        private bool _isSubmitting;
        private decimal _debtorAcctTBalance;

        protected override async Task OnInitializedAsync()
        {
            var patientInfo = await PopulateData.GetPatientMasterData(DebtorAcct, "T");
            var debtorAccountInfoT = await PopulateData.GetDebtorAccountInfoT(DebtorAcct, "T");
            var userNameMac = Environment.UserName;

            if (patientInfo != null && debtorAccountInfoT != null)
            {
                _viewRequestModel.Patient.FirstName = patientInfo.FirstName;
                _viewRequestModel.Patient.LastName = patientInfo.LastName;
                _viewRequestModel.Patient.AccountNumber = debtorAccountInfoT.SuppliedAcct.TrimStart(new[] { '0' });//for leading zero;
                _viewRequestModel.Balance = debtorAccountInfoT.Balance;
                _debtorAcctTBalance = debtorAccountInfoT.Balance;

            }
        }
        private async Task ProcessSaleTrans()
        {
            _responseModel = null;
            _errorModel = null;
            _loadingBar = 0;
            _tempAmount = 0;
            _isSubmitting = true;
            var saleRequestModel = new SaleRequestModel()
            {
                Outlet = new ApiAccessLibrary.ApiModels.Outlet()
                {
                    MerchantID = "192837645",
                    StoreID = "0001",
                    TerminalID = "0001"
                },
                Amount = _viewRequestModel.Amount,
                PaymentMethod = "Card",
                Card = new ApiAccessLibrary.ApiModels.Card()
                {
                    CVN = _viewRequestModel.Card.CVN,
                    CardHolderEmail = _viewRequestModel.Card.CardHolderEmail,
                    CardHolderName = _viewRequestModel.Card.CardHolderName,
                    CardNumber = _viewRequestModel.Card.CardNumber,
                    EntryMode = "key",
                    Expiration = _viewRequestModel.Card.Expiration,
                    IsCardDataEncrypted = false,
                    IsEMVCapableDevice = false,
                },
                Patient = new ApiAccessLibrary.ApiModels.Patient()
                {
                    AccountNumber = _viewRequestModel.Patient.AccountNumber,
                    FirstName = _viewRequestModel.Patient.FirstName,
                    LastName = _viewRequestModel.Patient.LastName
                }
            };
            try
            {
                _loadingBar = 1;
                var resultVerify = await SaleApi.PostProcessSalesTransactionAsync(saleRequestModel);
                if (resultVerify.Contains("FieldErrors"))
                {
                    _errorModel = resultVerify;
                }
                else
                {
                    _tempAmount = _viewRequestModel.Amount;
                    _responseModel = new ViewSaleResponseModel(resultVerify);

                }

                string noteText = null;
                if (@_responseModel != null && _responseModel.ResponseCode == "000")
                {
                    noteText = "INSTAMED CC APPROVED FOR $" + _tempAmount + " " + @_responseModel.ResponseMessage.ToUpper() +
                                  " AUTH #:" + @_responseModel.AuthorizationNumber;
                    await SaveCardInfoAndScheduleData();

                    _viewRequestModel = new ViewMultiplePaymentsRequestModel();
                }
                else
                {
                    if (@_responseModel != null)
                        noteText = "INSTAMED CC DECLINED FOR $" + _tempAmount + " " +
                                   @_responseModel.ResponseMessage.ToUpper() +
                                   " AUTH #:" + @_responseModel.AuthorizationNumber;
                }
                //actual employee =31950
                await AddNotes.Notes(DebtorAcct, 31950, "RA", noteText, "N", null, "T");//PO for prod_old & T is for test_db
                _loadingBar = 0;
                _isSubmitting = false;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }


        }

        private async Task ProcessCardAuthorization()
        {
            _responseModel = null;
            _errorModel = null;
            _loadingBar = 0;
            _tempAmount = 0;
            _isSubmitting = true;
            var saleRequestModel = new SaleRequestModel()
            {
                Outlet = new ApiAccessLibrary.ApiModels.Outlet()
                {
                    MerchantID = "192837645",
                    StoreID = "0001",
                    TerminalID = "0001"
                },
                Amount = _viewRequestModel.Amount,
                PaymentMethod = "Card",
                Card = new ApiAccessLibrary.ApiModels.Card()
                {
                    CVN = _viewRequestModel.Card.CVN,
                    CardHolderEmail = _viewRequestModel.Card.CardHolderEmail,
                    CardHolderName = _viewRequestModel.Card.CardHolderName,
                    CardNumber = _viewRequestModel.Card.CardNumber,
                    EntryMode = "key",
                    Expiration = _viewRequestModel.Card.Expiration,
                    IsCardDataEncrypted = false,
                    IsEMVCapableDevice = false,
                },
                Patient = new ApiAccessLibrary.ApiModels.Patient()
                {
                    AccountNumber = _viewRequestModel.Patient.AccountNumber,
                    FirstName = _viewRequestModel.Patient.FirstName,
                    LastName = _viewRequestModel.Patient.LastName
                }
            };
            try
            {
                _loadingBar = 1;
                var resultVerify = await CardApi.PostCardAuthorizationAsync(saleRequestModel);
                if (resultVerify.Contains("FieldErrors"))
                {
                    _errorModel = resultVerify;
                }
                else
                {
                    _tempAmount = _viewRequestModel.Amount;
                    _responseModel = new ViewSaleResponseModel(resultVerify);
                   
                }

                string noteText = null;
                if (@_responseModel != null && _responseModel.ResponseCode == "000")
                {
                    noteText = "INSTAMED CC APPROVED FOR $" + _tempAmount + " " + @_responseModel.ResponseMessage.ToUpper() +
                               " AUTH #:" + @_responseModel.AuthorizationNumber;
                    await SaveCardInfoAndScheduleData();

                    _viewRequestModel = new ViewMultiplePaymentsRequestModel();
                }
                else
                {
                    if (@_responseModel != null)
                        noteText = "INSTAMED CC DECLINED FOR $" + _tempAmount + " " +
                                   @_responseModel.ResponseMessage.ToUpper() +
                                   " AUTH #:" + @_responseModel.AuthorizationNumber;
                }

                await AddNotes.Notes(DebtorAcct, 31950, "RA", noteText, "N", null, "T");
                _loadingBar = 0;
                _isSubmitting = false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }


        }

        private async Task SaveCardInfoAndScheduleData()
        {
            var saleRequestModel = new PaymentPlanRequestModel()
            {
                Outlet = new ApiAccessLibrary.ApiModels.Outlet()
                {
                    MerchantID = "192837645",
                    StoreID = "0001",
                    TerminalID = "0001"
                },
                PaymentPlanType = "SaveOnFile",
                PaymentMethod = "Card",
                Card = new ApiAccessLibrary.ApiModels.Card()
                {
                    CVN = _viewRequestModel.Card.CVN,
                    CardHolderEmail = _viewRequestModel.Card.CardHolderEmail,
                    CardHolderName = _viewRequestModel.Card.CardHolderName,
                    CardNumber = _viewRequestModel.Card.CardNumber,
                    EntryMode = "key",
                    Expiration = _viewRequestModel.Card.Expiration,
                    IsCardDataEncrypted = false,
                    IsEMVCapableDevice = false,
                },
            };
            try
            {
                var resultVerify = await Tokenization.PaymentPlan(saleRequestModel);

                var cardInfoData = new ViewPaymentPlanResponseModel(resultVerify);
                //await AddCardInfo.CreateCardInfo(cardInfoData.CardInfo,"T");
                var cardInfoObj = new LcgCardInfo()
                {
                    IsActive = true,
                    EntryMode = cardInfoData.CardInfo.EntryMode,
                    BinNumber = cardInfoData.CardInfo.BinNumber,
                    ExpirationMonth = cardInfoData.CardInfo.ExpirationMonth,
                    ExpirationYear = cardInfoData.CardInfo.ExpirationYear,
                    LastFour = cardInfoData.CardInfo.LastFour,
                    PaymentMethodId = cardInfoData.CardInfo.PaymentMethodId,
                    Type = cardInfoData.CardInfo.Type,
                    AssociateDebtorAcct = DebtorAcct,
                    CardHolderName = _viewRequestModel.Card.CardHolderName
                };
                DbContext.LcgCardInfos.Add(cardInfoObj);
                await DbContext.SaveChangesAsync();

                var paymentScheduleObj = new LcgPaymentSchedule()
                {
                    CardInfoId = cardInfoObj.Id,
                    IsActive = true,
                    EffectiveDate = _scheduleDateTime,
                    NumberOfPayments = _numberOfPayment,
                    PatientAccount = _viewRequestModel.Patient.AccountNumber,
                    Amount = _viewRequestModel.Amount
                };
                //await AddPaymentSchedule.SavePaymentSchedule(paymentScheduleObj, _numberOfPayment, "T");
                //experimental
                var paymentDate = paymentScheduleObj.EffectiveDate;
                for (var i = 1; i <= _numberOfPayment; i++)
                {
                    var noteMaster = new LcgPaymentSchedule()
                    {
                        CardInfoId = paymentScheduleObj.CardInfoId,
                        EffectiveDate = paymentDate,
                        IsActive = true,
                        NumberOfPayments = i,
                        PatientAccount = paymentScheduleObj.PatientAccount,
                        Amount = paymentScheduleObj.Amount
                    };
                    await DbContext.LcgPaymentSchedules.AddAsync(noteMaster);
                    
                    await DbContext.SaveChangesAsync();
                    if (i == 1)
                    {
                        GlobalVariable.LcgPaymentScheduleId = noteMaster.Id;
                    }
                    paymentDate = paymentDate.AddMonths(1);

                }
                
                

                var paymentScheduleExample = new LcgPaymentScheduleHistory()
                {
                    ResponseCode = _responseModel.ResponseCode,
                    AuthorizationNumber = _responseModel.AuthorizationNumber,
                    AuthorizationText = Environment.UserName,
                    ResponseMessage = _responseModel.ResponseMessage,
                    PaymentScheduleId = GlobalVariable.LcgPaymentScheduleId,
                    TransactionId = _responseModel.TransactionId
                };

                DbContext.LcgPaymentScheduleHistories.Add(paymentScheduleExample);
                await DbContext.SaveChangesAsync();

                var paymentScheduleUpdate =
                    DbContext.LcgPaymentSchedules.FirstAsync(x => x.Id == GlobalVariable.LcgPaymentScheduleId);
                paymentScheduleUpdate.Result.IsActive = false;
                await DbContext.SaveChangesAsync();

                var ccPaymentObj = new CcPayment()
                {
                    DebtorAcct = _viewRequestModel.Patient.AccountNumber,
                    Company = "TOTAL CREDIT RECOVERY",
                    UserId = Environment.UserName,
                    UserName = Environment.UserName+" -LCG",
                    ChargeTotal = _viewRequestModel.Amount,
                    Subtotal = _viewRequestModel.Amount,
                    PaymentDate = _scheduleDateTime,
                    ApprovalStatus = "APPROVED",
                    BillingName = _viewRequestModel.Card.CardHolderName,
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

        private void OperationPutSlash()
        {
            if (_viewRequestModel.Card.Expiration is not { Length: 4 }) return;
            var firstHalf = _viewRequestModel.Card.Expiration[..2];
            var secondHalf = _viewRequestModel.Card.Expiration.Substring(2, 2);
            var newString = firstHalf + "/" + secondHalf;
            _viewRequestModel.Card.Expiration = newString;

        }

        private void CalculatePaymentAmount()
        {
            if (_debtorAcctTBalance > 0)
            {
                _viewRequestModel.Amount = _debtorAcctTBalance / _numberOfPayment;
            }
        }

        private async Task SubmittingDecision()
        {

            if (_scheduleDateTime.Date == DateTime.Now.Date)
            {
                await ProcessSaleTrans();
            }
            else
            {
                await ProcessCardAuthorization();
            }

        }

        private void DateTimeVerify()
        {
            if (_scheduleDateTime.Date < DateTime.Now.Date)
            {
                _scheduleDateTime = DateTime.Now;
            }
        }
    }
}
