﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiAccessLibrary.ApiModels;
using Microsoft.AspNetCore.Components;
using ApiAccessLibrary.Interfaces;
using DataAccessLibrary.Interfaces;
using LCG.Data;

namespace LCG.Pages.SalesTrans
{

    public partial class ProcessSalesTransV2
    {
        [Inject] private IAddNotes AddNotes { get; set; }
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
            var patientInfo = await PopulateData.GetPatientMasterData(DebtorAcct, "t");
            var debtorAccountInfoT = await PopulateData.GetDebtorAccountInfoT(DebtorAcct, "t");
            if (patientInfo != null && debtorAccountInfoT != null)
            {
                _viewRequestModel.Patient.FirstName = patientInfo.FirstName;
                _viewRequestModel.Patient.LastName = patientInfo.LastName;
                _viewRequestModel.Patient.AccountNumber = debtorAccountInfoT.SuppliedAcct;
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
                    _viewRequestModel = new ViewSaleRequestModel();
                }

                string noteText = null;
                if (@_responseModel != null)
                {
                    noteText = "InstaMed CC Processed for $" + _tempAmount + " " + @_responseModel.ResponseMessage +
                                  " Auth #:" + @_responseModel.AuthorizationNumber;
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

    }
}
