using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LCG.Data
{
    public class ViewSaleRequestModel
    {
        //[ValidateComplexType]
        public Outlet Outlet { get; set; } = new Outlet();
        [Required]
        [Range(double.Epsilon, double.MaxValue, ErrorMessage = "Please enter a value bigger than zero.")]
        public decimal Amount { get; set; }
        //[Required]
        public string PaymentMethod { get; set; }
        [ValidateComplexType]
        public Card Card { get; set; } = new Card();
        public BillingAddress BillingAddress { get; set; } = new BillingAddress();
        [ValidateComplexType]
        public Patient Patient { get; set; } = new Patient();

    }


    public class Outlet
    {
        [Required]
        public string MerchantID { get; set; }
        [Required]
        public string StoreID { get; set; }
        [Required]
        public string TerminalID { get; set; }
    }

    public class Card
    {
        //[Required]
        public string EntryMode { get; set; }
        [Required]
        public string CardNumber { get; set; }
        [Required]
        public string Expiration { get; set; }
        [Required]
        public string CVN { get; set; }
        public string CardHolderName { get; set; }
        public string CardHolderEmail { get; set; }
        public bool IsCardDataEncrypted { get; set; }
        public bool IsEMVCapableDevice { get; set; }
    }

    public class BillingAddress
    {
        public string Street1 { get; set; }
        public string Street2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
    }

    public class Patient
    {
        [Required]
        public string AccountNumber { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
    }
}
