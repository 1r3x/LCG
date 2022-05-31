using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityModelLibrary.Models;

namespace DataAccessLibrary.Interfaces
{
    public interface IPopulateDataForProcessSales
    {

        Task<PatientMaster> GetPatientMasterData(string debtorAcct, string environment);
        Task<DebtorAcctInfoT> GetDebtorAccountInfoT(string debtorAcct, string environment);
        Task<DebtorAcctInfoT> GetDebtorAccountNoByPatientAcct(string patientAcct, string environment);


    }
}
