using System;
using System.Net.Http;
using System.Threading.Tasks;
using ApiAccessLibrary.ApiModels;

namespace ApiAccessLibrary
{
    public interface IProcessSaleTransactions
    {
        Task<string> PostProcessSalesTransactionAsync(SaleRequestModel requestModel);
    }
}