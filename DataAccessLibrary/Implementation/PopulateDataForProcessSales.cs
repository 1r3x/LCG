﻿using System;
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
        public PopulateDataForProcessSales(DbContextForTest dbContext, DbContextForProdOld dbContextProdOld)
        {
            _dbContext = dbContext;
            _dbContextProdOld = dbContextProdOld;
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
                        Balance=i.Balance
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