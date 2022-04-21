using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ApiAccessLibrary.ApiModels;
using ApiAccessLibrary.Interfaces;

namespace ApiAccessLibrary.Implementation
{
    public class ProcessSaleTransactions : IProcessSaleTransactions
    {
        private static HttpClient _client = new();
        public ProcessSaleTransactions(HttpClient client)
        {
            _client = client;
            _client.BaseAddress = new Uri("https://connect.instamed.com/");
            _client.DefaultRequestHeaders.Add("api-key", "fceabac30f1e40758cc8e9bf02567f14");
            _client.DefaultRequestHeaders.Add("api-secret", "MU9dIPodksnusBM7");
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }


        public async Task<string> PostProcessSalesTransactionAsync(SaleRequestModel requestModel)
        {
            var response = await _client.PostAsJsonAsync(
                "rest/payment/sale", requestModel);
            var resultString = response.Content.ReadAsStringAsync();
            //var ensureSuccessStatusCode = response.EnsureSuccessStatusCode();
            return resultString.Result;

        }



    }
}
