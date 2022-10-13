using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiAccessLibrary.ApiModels;
using Microsoft.AspNetCore.Components;
using ApiAccessLibrary.Interfaces;
using DataAccessLibrary.Interfaces;
using EntityModelLibrary.Models;
using LCG.Data;
using Microsoft.AspNetCore.Http;

namespace LCG.Pages.SalesTrans
{

    public partial class ProcessSalesTransV2
    {
        [Inject] private IAddNotes AddNotes { get; set; }
        [Inject] private IAddCcPayment AddCcPayment { get; set; }
        [Inject] private IPopulateDataForProcessSales PopulateData { get; set; }
        [Parameter]
        public string DebtorAcct { get; set; }
        [Inject] private IProcessSaleTransactions Api { get; set; }
        private ViewSaleRequestModel _viewRequestModel = new();
        private ViewSaleResponseModel _responseModel;
        private string _errorModel;
        private int _loadingBar;
        private decimal _tempAmount;
        private bool _isSubmitting;

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
            StateContainer.OnChange += StateHasChanged;
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
                        _viewRequestModel.Patient.AccountNumber = debtorAccountInfoT.SuppliedAcct.TrimStart(new[] { '0' });//for leading zero
                    }
                }
            }




        }



        private async Task ProcessTrans()
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
                var resultVerify = await Api.PostProcessSalesTransactionAsync(saleRequestModel);
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
                    // for success
                    var ccPaymentObj = new CcPayment()
                    {
                        DebtorAcct = DebtorAcct,
                        Company = "TOTAL CREDIT RECOVERY",
                        //UserId = username,
                        UserId = _username,
                        //UserName = username + " -LCG",
                        UserName = _username,
                        ChargeTotal = _viewRequestModel.Amount,
                        Subtotal = _viewRequestModel.Amount,
                        PaymentDate = DateTime.Now,
                        ApprovalStatus = "APPROVED",
                        BillingName = _viewRequestModel.Card.CardHolderName,
                        ApprovalCode = _responseModel.ResponseCode,
                        OrderNumber = _responseModel.TransactionId,
                        RefNumber = "INSTAMEDLH",
                        Sif = "N",
                        VoidSale = "N"
                    };
                    await AddCcPayment.CreateCcPayment(ccPaymentObj, _centralizeVariablesModel.Value.DbEnvironment);//PO for prod_old & T is for test_db
                    _viewRequestModel = new ViewSaleRequestModel();
                }
                else
                {
                    if (@_responseModel != null)
                        noteText = "INSTAMED CC DECLINED FOR $" + _tempAmount + " " +
                                   @_responseModel.ResponseMessage.ToUpper() +
                                   " AUTH #:" + @_responseModel.AuthorizationNumber;
                    // for DECLINED
                    if (_responseModel != null)
                    {
                        var ccPaymentObj = new CcPayment()
                        {
                            DebtorAcct = DebtorAcct,
                            Company = "TOTAL CREDIT RECOVERY",
                            //UserId = username,
                            UserId = _username,
                            //UserName = username + " -LCG",
                            UserName = _username,
                            ChargeTotal = _viewRequestModel.Amount,
                            Subtotal = _viewRequestModel.Amount,
                            PaymentDate = DateTime.Now,
                            ApprovalStatus = "DECLINED",
                            BillingName = _viewRequestModel.Card.CardHolderName,
                            ErrorCode = _responseModel.ResponseCode,
                            OrderNumber = _responseModel.TransactionId,
                            RefNumber = "INSTAMEDLH",
                            Sif = "N",
                            VoidSale = "N"
                        };
                        await AddCcPayment.CreateCcPayment(ccPaymentObj, _centralizeVariablesModel.Value.DbEnvironment);//PO for prod_old & T is for test_db
                    }
                }

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

        private void OperationPutSlash()
        {
            if (_viewRequestModel.Card.Expiration is not { Length: 4 }) return;
            var firstHalf = _viewRequestModel.Card.Expiration[..2];
            var secondHalf = _viewRequestModel.Card.Expiration.Substring(2, 2);
            var newString = firstHalf + "/" + secondHalf;
            _viewRequestModel.Card.Expiration = newString;
        }
        public void Dispose() => StateContainer.OnChange -= StateHasChanged;

    }
}

