using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.VisualBasic;

namespace LCG.Data
{
    public class ViewMultiplePaymentsRequestModel
    {
        public ApiAccessLibrary.ApiModels.Outlet Outlet { get; set; } = new ApiAccessLibrary.ApiModels.Outlet();
        [Required]
        [Range(double.Epsilon, double.MaxValue, ErrorMessage = "Please enter a value bigger than zero.")]
        public decimal Amount { get; set; }
        //[Required]
        public string PaymentMethod { get; set; }
        [ValidateComplexType]
        public ApiAccessLibrary.ApiModels.Card Card { get; set; } = new ApiAccessLibrary.ApiModels.Card();
        public ApiAccessLibrary.ApiModels.BillingAddress BillingAddress { get; set; } = new ApiAccessLibrary.ApiModels.BillingAddress();
        [ValidateComplexType]
        public ApiAccessLibrary.ApiModels.Patient Patient { get; set; } = new ApiAccessLibrary.ApiModels.Patient();
        public DateTime StartingDate { get; set; }
        public int NumberOfPayments { get; set; }
        public decimal Balance { get; set; }

    }


}
