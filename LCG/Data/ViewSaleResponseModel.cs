using System.Linq;
using Newtonsoft.Json.Linq;

namespace LCG.Data
{
    public class ViewSaleResponseModel
    {
        public ViewSaleResponseModel(string jsonResponse)
        {
            var jObject = JObject.Parse(jsonResponse);
            ResponseCode = (string)jObject["ResponseCode"];
            ResponseMessage = (string)jObject["ResponseMessage"];
            AuthorizationNumber = (string)jObject["AuthorizationNumber"];
        }
        public string ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
        public string AuthorizationNumber { get; set; }
    }

}
