﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiAccessLibrary.ApiModels;
using Microsoft.AspNetCore.Components;
using ApiAccessLibrary.Interfaces;
using DataAccessLibrary.Interfaces;
using EntityModelLibrary.Models;
using LCG.Data;

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
        protected override async Task OnInitializedAsync()
        {
            var patientInfo = await PopulateData.GetPatientMasterData(DebtorAcct, "T");//PO for prod_old & T is for test_db
            var debtorAccountInfoT = await PopulateData.GetDebtorAccountInfoT(DebtorAcct, "T");//PO for prod_old & T is for test_db
            if (patientInfo != null && debtorAccountInfoT != null)
            {
                _viewRequestModel.Patient.FirstName = patientInfo.FirstName;
                _viewRequestModel.Patient.LastName = patientInfo.LastName;
                _viewRequestModel.Patient.AccountNumber = debtorAccountInfoT.SuppliedAcct.TrimStart(new[] { '0' });//for leading zero
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
                if (@_responseModel != null && _responseModel.ResponseCode=="000")
                {
                    noteText = "INSTAMED CC APPROVED FOR $" + _tempAmount + " " + @_responseModel.ResponseMessage.ToUpper() +
                                  " AUTH #:" + @_responseModel.AuthorizationNumber;
                    //todo requirements auth
                    var ccPaymentObj = new CcPayment()
                    {
                        DebtorAcct = _viewRequestModel.Patient.AccountNumber,
                        Company = "TOTAL CREDIT RECOVERY",
                        UserId = Environment.UserName,
                        UserName = Environment.UserName + " -LCG",
                        ChargeTotal = _viewRequestModel.Amount,
                        Subtotal = _viewRequestModel.Amount,
                        PaymentDate = DateTime.Now,
                        ApprovalStatus = "APPROVED",
                        ApprovalCode = "",
                        OrderNumber = "",
                        RefNumber = "INSTAMEDLH",
                        Sif = "Y"
                    };
                    await AddCcPayment.CreateCcPayment(ccPaymentObj, "T");
                    _viewRequestModel = new ViewSaleRequestModel();
                }
                else
                {
                    if (@_responseModel != null)
                        noteText = "INSTAMED CC DECLINED FOR $" + _tempAmount + " " +
                                   @_responseModel.ResponseMessage.ToUpper() +
                                   " AUTH #:" + @_responseModel.AuthorizationNumber;
                }

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

        private void OperationPutSlash()
        {
            if (_viewRequestModel.Card.Expiration is not { Length: 4 }) return;
            var firstHalf = _viewRequestModel.Card.Expiration[..2];
            var secondHalf = _viewRequestModel.Card.Expiration.Substring(2, 2);
            var newString = firstHalf + "/" + secondHalf;
            _viewRequestModel.Card.Expiration = newString;

        }

    }
}

