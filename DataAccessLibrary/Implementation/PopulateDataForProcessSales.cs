using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DataAccessLibrary.Interfaces;
using EntityModelLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLibrary.Implementation
{
    public class PopulateDataForProcessSales : IPopulateDataForProcessSales
    {
        private readonly DbContextForTest _dbContext;
        public PopulateDataForProcessSales(DbContextForTest dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<PatientMaster> GetPatientMasterData(string debtorAcct, string environment)
        {
            if (environment == "t")
            {
                return await _dbContext.PatientMasters.Where(x => x.DebtorAcct == debtorAcct).Select(i =>
                    new PatientMaster()
                    {
                        FirstName = i.FirstName,
                        LastName = i.LastName
                    }).SingleOrDefaultAsync();
            }

            else
            {
                //this is just a demo implements 
                return await _dbContext.PatientMasters.Where(x => x.DebtorAcct == debtorAcct).Select(i =>
                    new PatientMaster()
                    {
                        FirstName = i.FirstName,
                        LastName = i.LastName
                    }).SingleOrDefaultAsync();
            }

        }

        public async Task<DebtorAcctInfoT> GetDebtorAccountInfoT(string debtorAcct, string environment)
        {
            if (environment == "t")
            {
                return await _dbContext.DebtorAcctInfoTs.Where(x => x.DebtorAcct == debtorAcct).Select(i =>
                    new DebtorAcctInfoT()
                    {
                        SuppliedAcct = i.SuppliedAcct,
                        Balance=i.Balance
                    }).SingleOrDefaultAsync();
            }
            else
            {
                //this is just a demo implements 
                return await _dbContext.DebtorAcctInfoTs.Where(x => x.DebtorAcct == debtorAcct).Select(i =>
                    new DebtorAcctInfoT()
                    {
                        SuppliedAcct = i.SuppliedAcct,
                        Balance = i.Balance
                    }).SingleOrDefaultAsync();
            }
        }
    }
}
