using ApiAccessLibrary.ApiModels;
using LCG.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiAccessLibrary;
using ApiAccessLibrary.Interfaces;
using Microsoft.AspNetCore.Components;
using Outlet = LCG.Data.Outlet;



namespace LCG.Pages.SalesTrans
{
    public partial class ProcessSalesTrans
    {
        [Inject] private IProcessSaleTransactions Api { get; set; }
        private ViewSaleRequestModel _viewRequestModel = new();
        private ViewSaleResponseModel _responseModel;
        private string _errorModel;
        private int _loadingBar;
        private decimal _tempAmount;
        private bool _isSubmitting;


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
