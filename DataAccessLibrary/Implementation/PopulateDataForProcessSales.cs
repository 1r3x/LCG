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
        private readonly DbContextForProdOld _dbContextProdOld;
        private readonly DbContextForProd _dbContextForProd;
        public PopulateDataForProcessSales(DbContextForTest dbContext, DbContextForProdOld dbContextProdOld,
            DbContextForProd dbContextForProd)
        {
            _dbContext = dbContext;
            _dbContextProdOld = dbContextProdOld;
            _dbContextForProd = dbContextForProd;
        }
        public async Task<PatientMaster> GetPatientMasterData(string debtorAcct, string environment)
        {
            if (environment == "T")
            {
                return await _dbContext.PatientMasters.Where(x => x.DebtorAcct == debtorAcct).Select(i =>
                    new PatientMaster()
                    {
                        FirstName = i.FirstName,
                        LastName = i.LastName
                    }).SingleOrDefaultAsync();
            }
            else if (environment == "PO")
            {
                return await _dbContextProdOld.PatientMasters.Where(x => x.DebtorAcct == debtorAcct).Select(i =>
                    new PatientMaster()
                    {
                        FirstName = i.FirstName,
                        LastName = i.LastName
                    }).SingleOrDefaultAsync();
            }
            else if (environment == "P")
            {
                return await _dbContextForProd.PatientMasters.Where(x => x.DebtorAcct == debtorAcct).Select(i =>
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
            if (environment == "T")
            {
                return await _dbContext.DebtorAcctInfoTs.Where(x => x.DebtorAcct == debtorAcct).Select(i =>
                    new DebtorAcctInfoT()
                    {
                        SuppliedAcct = i.SuppliedAcct,
                        Balance = i.Balance
                    }).SingleOrDefaultAsync();
            }
            else if (environment == "PO")
            {
                return await _dbContextProdOld.DebtorAcctInfoTs.Where(x => x.DebtorAcct == debtorAcct).Select(i =>
                    new DebtorAcctInfoT()
                    {
                        SuppliedAcct = i.SuppliedAcct,
                        Balance = i.Balance
                    }).SingleOrDefaultAsync();
            }
            else if (environment == "P")
            {
                return await _dbContextForProd.DebtorAcctInfoTs.Where(x => x.DebtorAcct == debtorAcct).Select(i =>
                    new DebtorAcctInfoT()
                    {
                        SuppliedAcct = i.SuppliedAcct,
                        Balance = i.Balance
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

        public async Task<DebtorAcctInfoT> GetDebtorAccountNoByPatientAcct(string patientAcct, string environment)
        {
            if (environment == "T")
            {
                return await _dbContext.DebtorAcctInfoTs.Where(x => x.SuppliedAcct.TrimStart(new[] { '0' }) == patientAcct).Select(i =>
                    new DebtorAcctInfoT()
                    {
                        DebtorAcct = i.DebtorAcct
                    }).SingleOrDefaultAsync();
            }
            else if (environment == "PO")
            {
                return await _dbContextProdOld.DebtorAcctInfoTs.Where(x => x.SuppliedAcct.TrimStart(new[] { '0' }) == patientAcct).Select(i =>
                    new DebtorAcctInfoT()
                    {
                        DebtorAcct = i.DebtorAcct
                    }).SingleOrDefaultAsync();
            }
            else if (environment == "P")
            {
                return await _dbContextForProd.DebtorAcctInfoTs.Where(x => x.SuppliedAcct.TrimStart(new[] { '0' }) == patientAcct).Select(i =>
                    new DebtorAcctInfoT()
                    {
                        DebtorAcct = i.DebtorAcct
                    }).SingleOrDefaultAsync();
            }
            else
            {
                //this is just a demo implements 
                return await _dbContext.DebtorAcctInfoTs.Where(x => x.SuppliedAcct.TrimStart(new[] { '0' }) == patientAcct).Select(i =>
                    new DebtorAcctInfoT()
                    {
                        DebtorAcct = i.DebtorAcct
                    }).SingleOrDefaultAsync();
            }
        }
    }
}
