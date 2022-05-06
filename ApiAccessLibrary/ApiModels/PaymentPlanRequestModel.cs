using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiAccessLibrary.ApiModels
{
    public class PaymentPlanRequestModel
    {
        public Outlet Outlet { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentPlanType { get; set; }
        public Card Card { get; set; }
    }
    

   
}
