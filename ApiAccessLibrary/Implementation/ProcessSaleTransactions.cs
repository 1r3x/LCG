using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ApiAccessLibrary.ApiModels;
using ApiAccessLibrary.Interfaces;
using EntityModelLibrary.ViewModels;
using Microsoft.Extensions.Options;

namespace ApiAccessLibrary.Implementation
{

    public class ProcessSaleTransactions : IProcessSaleTransactions
    {
        private static HttpClient _client = new();

        private readonly IOptions<CentralizeVariablesModel> _centralizeVariablesModel;
        public ProcessSaleTransactions(HttpClient client, IOptions<CentralizeVariablesModel> centralizeVariablesModel)
        {
            _centralizeVariablesModel = centralizeVariablesModel;
            _client = client;
            _client.BaseAddress = new Uri(_centralizeVariablesModel.Value.InstaMedCredentials.BaseAddress);
            _client.DefaultRequestHeaders.Add("api-key", _centralizeVariablesModel.Value.InstaMedCredentials.APIkey);
            _client.DefaultRequestHeaders.Add("api-secret", _centralizeVariablesModel.Value.InstaMedCredentials.APIsecret);
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
