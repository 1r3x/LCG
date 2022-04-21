using System.Threading.Tasks;
using ApiAccessLibrary.ApiModels;

namespace ApiAccessLibrary.Interfaces
{
    public interface IProcessSaleTransactions
    {
        Task<string> PostProcessSalesTransactionAsync(SaleRequestModel requestModel);
    }
}