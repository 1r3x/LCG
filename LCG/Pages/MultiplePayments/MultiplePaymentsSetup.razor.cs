using System;
using System.Threading.Tasks;
using ApiAccessLibrary.ApiModels;
using ApiAccessLibrary.Interfaces;
using DataAccessLibrary.Interfaces;
using EntityModelLibrary.Models;
using LCG.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;

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
        [Inject] private DbContextForProd DbContextForProd { get; set; }
        [Inject] private IAddCcPayment AddCcPayment { get; set; }
        [Inject] private IAddPaymentScheduleHistory AddPaymentScheduleHistory { get; set; }

        private ViewMultiplePaymentsRequestModel _viewRequestModel = new();
        private ViewSaleResponseModel _responseModel;
        private int _numberOfPayment = 1;
        private DateTime _scheduleDateTime = DateTime.Now;
        private string _errorModel;
        private int _loadingBar;
        private decimal _tempAmount;
        private bool _isSubmitting;
        private decimal _debtorAcctTBalance;


        //todo username 
        private string _username;
        private async Task ProcessUserNameData()
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            if (user.Identity != null)
            {

                if (user.Identity is { IsAuthenticated: true })
                {
                    if (user.Identity.Name != null)
                    {
                        var username = user.Identity.Name.Split("\\");
                        _username = username[1].Length > 5
                            ? username[1][..5] == "admin" ? "31950" : username[1]
                            : username[1];
                    }
                }
                else
                {
                    _username = "LCG";
                }
            }
        }
        //todo end
        protected override async Task OnInitializedAsync()
        {
            if (DebtorAcct == null && StateContainer.Property.Length > 0)
            {
                DebtorAcct = StateContainer.Property;
            }
            await ProcessUserNameData();

            if (DebtorAcct != null)
            {
                StateContainer.Property = DebtorAcct;
                var acctLimitForHBTemp = DebtorAcct.Split('-');
                var acctLimitForHB = Convert.ToInt64(acctLimitForHBTemp[0] + acctLimitForHBTemp[1]);
                if (acctLimitForHB >= 4950000001 && acctLimitForHB < 4950999999)
                {
                    var patientInfo = await PopulateData.GetPatientMasterData(DebtorAcct, _centralizeVariablesModel.Value.DbEnvironment);
                    var debtorAccountInfoT = await PopulateData.GetDebtorAccountInfoT(DebtorAcct, _centralizeVariablesModel.Value.DbEnvironment);

                    if (patientInfo != null && debtorAccountInfoT != null)
                    {
                        _viewRequestModel.Patient.FirstName = patientInfo.FirstName;
                        _viewRequestModel.Patient.LastName = patientInfo.LastName;
                        _viewRequestModel.Patient.AccountNumber = debtorAccountInfoT.SuppliedAcct.TrimStart(new[] { '0' });//for leading zero;
                        _viewRequestModel.Balance = debtorAccountInfoT.Balance;
                        _debtorAcctTBalance = debtorAccountInfoT.Balance;

                    }
                }
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
                    MerchantID = _centralizeVariablesModel.Value.Outlet.MerchantID,
                    StoreID = _centralizeVariablesModel.Value.Outlet.StoreID,
                    TerminalID = _centralizeVariablesModel.Value.Outlet.TerminalID
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
                await AddNotes.Notes(DebtorAcct, 31950, "RA", noteText, "N", null, _centralizeVariablesModel.Value.DbEnvironment);//PO for prod_old & T is for test_db
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
                    MerchantID = _centralizeVariablesModel.Value.Outlet.MerchantID,
                    StoreID = _centralizeVariablesModel.Value.Outlet.StoreID,
                    TerminalID = _centralizeVariablesModel.Value.Outlet.TerminalID
                },
                //Amount = _viewRequestModel.Amount,
                Amount = 1,
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
                    //_tempAmount = _viewRequestModel.Amount;
                    _tempAmount = 1;
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

                await AddNotes.Notes(DebtorAcct, 31950, "RA", noteText, "N", null, _centralizeVariablesModel.Value.DbEnvironment);
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
                    MerchantID = _centralizeVariablesModel.Value.Outlet.MerchantID,
                    StoreID = _centralizeVariablesModel.Value.Outlet.StoreID,
                    TerminalID = _centralizeVariablesModel.Value.Outlet.TerminalID
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

                //DbContext.LcgCardInfos.Add(cardInfoObj);
                //await DbContext.SaveChangesAsync();


                await AddCardInfo.CreateCardInfo(cardInfoObj, _centralizeVariablesModel.Value.DbEnvironment);


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

                if (_centralizeVariablesModel.Value.DbEnvironment == "T")
                {
                    var paymentDate = paymentScheduleObj.EffectiveDate;
                    for (var i = 1; i <= _numberOfPayment; i++)
                    {
                        var lcgPaymentScheduleObj = new LcgPaymentSchedule()
                        {
                            CardInfoId = paymentScheduleObj.CardInfoId,
                            EffectiveDate = paymentDate,
                            IsActive = true,
                            NumberOfPayments = i,
                            PatientAccount = paymentScheduleObj.PatientAccount,
                            Amount = paymentScheduleObj.Amount
                        };
                        await DbContext.LcgPaymentSchedules.AddAsync(lcgPaymentScheduleObj);

                        await DbContext.SaveChangesAsync();
                        if (i == 1)
                        {
                            GlobalVariable.LcgPaymentScheduleId = lcgPaymentScheduleObj.Id;
                        }
                        paymentDate = paymentDate.AddMonths(1);

                    }
                }
                else if (_centralizeVariablesModel.Value.DbEnvironment == "PO")
                {
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
                        await DbContextProdOld.LcgPaymentSchedules.AddAsync(noteMaster);

                        await DbContextProdOld.SaveChangesAsync();
                        if (i == 1)
                        {
                            GlobalVariable.LcgPaymentScheduleId = noteMaster.Id;
                        }
                        paymentDate = paymentDate.AddMonths(1);

                    }
                }
                else if (_centralizeVariablesModel.Value.DbEnvironment == "P")
                {
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
                        await DbContextForProd.LcgPaymentSchedules.AddAsync(noteMaster);

                        await DbContextForProd.SaveChangesAsync();
                        if (i == 1)
                        {
                            GlobalVariable.LcgPaymentScheduleId = noteMaster.Id;
                        }
                        paymentDate = paymentDate.AddMonths(1);

                    }
                }
                else
                {
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
                }




                var paymentScheduleHistoryObj = new LcgPaymentScheduleHistory()
                {
                    ResponseCode = _responseModel.ResponseCode,
                    AuthorizationNumber = _responseModel.AuthorizationNumber,
                    AuthorizationText = _username,
                    ResponseMessage = _responseModel.ResponseMessage,
                    PaymentScheduleId = GlobalVariable.LcgPaymentScheduleId,
                    TransactionId = _responseModel.TransactionId
                };

                //DbContext.LcgPaymentScheduleHistories.Add(paymentScheduleExample);
                //await DbContext.SaveChangesAsync();

                //var paymentScheduleUpdate =
                //    DbContext.LcgPaymentSchedules.FirstAsync(x => x.Id == GlobalVariable.LcgPaymentScheduleId);
                //paymentScheduleUpdate.Result.IsActive = false;
                //await DbContext.SaveChangesAsync();

                await AddPaymentScheduleHistory.SavePaymentScheduleHistory(paymentScheduleHistoryObj, _centralizeVariablesModel.Value.DbEnvironment);
                if (_scheduleDateTime.Date == DateTime.Now.Date)
                {
                    await AddPaymentSchedule.InactivePaymentSchedule(GlobalVariable.LcgPaymentScheduleId,
                   _centralizeVariablesModel.Value.DbEnvironment);

                    var ccPaymentObj = new CcPayment()
                    {
                        DebtorAcct = _viewRequestModel.Patient.AccountNumber,
                        Company = "TOTAL CREDIT RECOVERY",
                        UserId = _username,
                        UserName = _username + " -LCG",
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
                    await AddCcPayment.CreateCcPayment(ccPaymentObj, _centralizeVariablesModel.Value.DbEnvironment);
                }

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

        public void Dispose() => StateContainer.OnChange -= StateHasChanged;
    }
}
