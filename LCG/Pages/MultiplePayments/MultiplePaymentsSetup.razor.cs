using System;
using System.Threading.Tasks;
using ApiAccessLibrary.ApiModels;
using ApiAccessLibrary.Interfaces;
using DataAccessLibrary.Interfaces;
using LCG.Data;
using Microsoft.AspNetCore.Components;

namespace LCG.Pages.MultiplePayments
{
    public partial class MultiplePaymentsSetup
    {

        [Inject] private IAddNotes AddNotes { get; set; }
        [Inject] private IPopulateDataForProcessSales PopulateData { get; set; }
        [Parameter] public string DebtorAcct { get; set; }
        [Inject] private IProcessSaleTransactions SaleApi { get; set; }
        [Inject] private IProcessCardAuthorization CardApi { get; set; }
        [Inject] private IPayment AutoPaymentApi { get; set; }

        private ViewMultiplePaymentsRequestModel _viewRequestModel = new();
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
            if (patientInfo != null && debtorAccountInfoT != null)
            {
                _viewRequestModel.Patient.FirstName = patientInfo.FirstName;
                _viewRequestModel.Patient.LastName = patientInfo.LastName;
                _viewRequestModel.Patient.AccountNumber = debtorAccountInfoT.SuppliedAcct.TrimStart(new[] { '0' });//for leading zero;
                _viewRequestModel.Balance = debtorAccountInfoT.Balance;
                _debtorAcctTBalance = debtorAccountInfoT.Balance;
            }
        }
        private async Task Enroll()
        {
            _loadingBar = 0;
            _isSubmitting = true;
            var saleRequestModel = new SaleRequestModel()
            {
                Outlet = new ApiAccessLibrary.ApiModels.Outlet()
                {
                    //MerchantID = _viewRequestModel.Outlet.MerchantID,
                    MerchantID = "192837645",
                    //StoreID = _viewRequestModel.Outlet.StoreID,
                    StoreID = "0001",
                    //TerminalID = _viewRequestModel.Outlet.TerminalID
                    TerminalID = "0001"
                },
                Amount = _viewRequestModel.Amount,
                //PaymentMethod = _viewRequestModel.PaymentMethod,
                PaymentMethod = "Card",
                Card = new ApiAccessLibrary.ApiModels.Card()
                {
                    CVN = _viewRequestModel.Card.CVN,
                    CardHolderEmail = _viewRequestModel.Card.CardHolderEmail,
                    CardHolderName = _viewRequestModel.Card.CardHolderName,
                    CardNumber = _viewRequestModel.Card.CardNumber,
                    //EntryMode = _viewRequestModel.Card.EntryMode,
                    EntryMode = "key",
                    Expiration = _viewRequestModel.Card.Expiration,
                    //IsCardDataEncrypted = _viewRequestModel.Card.IsCardDataEncrypted,
                    IsCardDataEncrypted = false,
                    //IsEMVCapableDevice = _viewRequestModel.Card.IsEMVCapableDevice,
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
                var resultVerify = await AutoPaymentApi.PaymentPlan(saleRequestModel);
                if (resultVerify.Contains("FieldErrors"))
                {
                    _errorModel = resultVerify;
                }
                else
                {
                    _tempAmount = _viewRequestModel.Amount;
                    _responseModel = new ViewSaleResponseModel(resultVerify);
                    _viewRequestModel = new ViewMultiplePaymentsRequestModel();
                }
                

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
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
                    //MerchantID = _viewRequestModel.Outlet.MerchantID,
                    MerchantID = "192837645",
                    //StoreID = _viewRequestModel.Outlet.StoreID,
                    StoreID = "0001",
                    //TerminalID = _viewRequestModel.Outlet.TerminalID
                    TerminalID = "0001"
                },
                Amount = _viewRequestModel.Amount,
                //PaymentMethod = _viewRequestModel.PaymentMethod,
                PaymentMethod = "Card",
                Card = new ApiAccessLibrary.ApiModels.Card()
                {
                    CVN = _viewRequestModel.Card.CVN,
                    CardHolderEmail = _viewRequestModel.Card.CardHolderEmail,
                    CardHolderName = _viewRequestModel.Card.CardHolderName,
                    CardNumber = _viewRequestModel.Card.CardNumber,
                    //EntryMode = _viewRequestModel.Card.EntryMode,
                    EntryMode = "key",
                    Expiration = _viewRequestModel.Card.Expiration,
                    //IsCardDataEncrypted = _viewRequestModel.Card.IsCardDataEncrypted,
                    IsCardDataEncrypted = false,
                    //IsEMVCapableDevice = _viewRequestModel.Card.IsEMVCapableDevice,
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
                    _viewRequestModel = new ViewMultiplePaymentsRequestModel();
                }

                string noteText = null;
                if (@_responseModel != null && _responseModel.ResponseCode == "000")
                {
                    noteText = "INSTAMED CC APPROVED FOR $" + _tempAmount + " " + @_responseModel.ResponseMessage.ToUpper() +
                                  " AUTH #:" + @_responseModel.AuthorizationNumber;
                }
                else
                {
                    if (@_responseModel != null)
                        noteText = "INSTAMED CC DECLINED FOR $" + _tempAmount + " " +
                                   @_responseModel.ResponseMessage.ToUpper() +
                                   " AUTH #:" + @_responseModel.AuthorizationNumber;
                }

                await AddNotes.Notes(DebtorAcct, 31950, "RA", noteText, "N", null, "PO");//PO for prod_old & T is for test_db
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
                    //MerchantID = _viewRequestModel.Outlet.MerchantID,
                    MerchantID = "192837645",
                    //StoreID = _viewRequestModel.Outlet.StoreID,
                    StoreID = "0001",
                    //TerminalID = _viewRequestModel.Outlet.TerminalID
                    TerminalID = "0001"
                },
                Amount = _viewRequestModel.Amount,
                //PaymentMethod = _viewRequestModel.PaymentMethod,
                PaymentMethod = "Card",
                Card = new ApiAccessLibrary.ApiModels.Card()
                {
                    CVN = _viewRequestModel.Card.CVN,
                    CardHolderEmail = _viewRequestModel.Card.CardHolderEmail,
                    CardHolderName = _viewRequestModel.Card.CardHolderName,
                    CardNumber = _viewRequestModel.Card.CardNumber,
                    //EntryMode = _viewRequestModel.Card.EntryMode,
                    EntryMode = "key",
                    Expiration = _viewRequestModel.Card.Expiration,
                    //IsCardDataEncrypted = _viewRequestModel.Card.IsCardDataEncrypted,
                    IsCardDataEncrypted = false,
                    //IsEMVCapableDevice = _viewRequestModel.Card.IsEMVCapableDevice,
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
                    _viewRequestModel = new ViewMultiplePaymentsRequestModel();
                }

                string noteText = null;
                if (@_responseModel != null && _responseModel.ResponseCode == "000")
                {
                    noteText = "INSTAMED CC APPROVED FOR $" + _tempAmount + " " + @_responseModel.ResponseMessage.ToUpper() +
                               " AUTH #:" + @_responseModel.AuthorizationNumber;
                }
                else
                {
                    if (@_responseModel != null)
                        noteText = "INSTAMED CC DECLINED FOR $" + _tempAmount + " " +
                                   @_responseModel.ResponseMessage.ToUpper() +
                                   " AUTH #:" + @_responseModel.AuthorizationNumber;
                }

                await AddNotes.Notes(DebtorAcct, 31950, "RA", noteText, "N", null, "t");
                _loadingBar = 0;
                _isSubmitting = false;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
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
