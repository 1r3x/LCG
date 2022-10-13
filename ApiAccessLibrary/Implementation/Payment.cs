using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using ApiAccessLibrary.ApiModels;
using ApiAccessLibrary.Interfaces;
using EntityModelLibrary.ViewModels;
using Microsoft.Extensions.Options;

namespace ApiAccessLibrary.Implementation
{
    public class Payment : IPayment
    {
        private static HttpClient _client = new();
        private readonly IOptions<CentralizeVariablesModel> _centralizeVariablesModel;
        public Payment(HttpClient client, IOptions<CentralizeVariablesModel> centralizeVariablesModel)
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
        public async Task<string> PaymentPlan(PaymentPlanRequestModel requestModel)
        {
            var response = await _client.PostAsJsonAsync(
                "rest/payment/paymentplan", requestModel);
            var resultString = response.Content.ReadAsStringAsync();
            return resultString.Result;
        }
    }
}
