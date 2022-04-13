using ApiAccessLibrary.ApiModels;
using LCG.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiAccessLibrary;
using Microsoft.AspNetCore.Components;
using Outlet = LCG.Data.Outlet;



namespace LCG.Pages.SalesTrans
{
    public partial class ProcessSalesTrans
    {
        [Inject] private IProcessSaleTransactions Api { get; set; }
        private ViewSaleRequestModel _viewRequestModel = new();
        //private string _response;
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

    }
}
