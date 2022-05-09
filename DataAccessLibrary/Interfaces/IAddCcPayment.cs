using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityModelLibrary.Models;

namespace DataAccessLibrary.Interfaces
{
    public interface IAddCcPayment
    {
        Task<string> CreateCcPayment(CcPayment ccPaymentObj, string environment);
    }
}
