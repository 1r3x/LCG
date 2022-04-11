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
        private readonly ViewSaleRequestModel _viewRequestModel = new();
        //private string _response;
        private ViewSaleResponseModel _responseModel;
        private string _errorModel;


        private async Task ProcessTrans()
        {
            var saleRequestModel = new SaleRequestModel()
            {
                Outlet = new ApiAccessLibrary.ApiModels.Outlet()
                {
                    //MerchantID = _viewRequestModel.Outlet.MerchantID,
                    MerchantID = "192837645 ",
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
                var resultVerify = await Api.PostProcessSalesTransactionAsync(saleRequestModel);
                if (resultVerify.Contains("FieldErrors"))
                {
                    _errorModel = resultVerify;
                }
                else
                {
                    _responseModel = new ViewSaleResponseModel(resultVerify);
                }
               

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }


        }




    }
}
