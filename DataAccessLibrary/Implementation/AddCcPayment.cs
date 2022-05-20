using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLibrary.Interfaces;
using EntityModelLibrary.Models;

namespace DataAccessLibrary.Implementation
{
    public class AddCcPayment : IAddCcPayment
    {
        private readonly DbContextForTest _dbContext;
        private readonly DbContextForProdOld _dbContextProdOld;

        public AddCcPayment(DbContextForTest dbContext, DbContextForProdOld dbContextProdOld)
        {
            _dbContext = dbContext;
            _dbContextProdOld = dbContextProdOld;
        }

        public async Task<string> CreateCcPayment(CcPayment ccPaymentObj, string environment)
        {
            try
            {
                if (environment == "T")
                {
                    await _dbContext.CcPayments.AddAsync(ccPaymentObj);
                    await _dbContext.SaveChangesAsync();
                }
                else if (environment == "PO")
                {
                    await _dbContextProdOld.CcPayments.AddAsync(ccPaymentObj);
                    await _dbContextProdOld.SaveChangesAsync();
                }
                else
                {
                    await _dbContext.CcPayments.AddAsync(ccPaymentObj);
                    await _dbContext.SaveChangesAsync();
                }

            }
            catch (Exception e)
            {
                return e.Message;
            }
            return "Cc Payment added successfully";
        }
    }
}
