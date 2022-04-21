using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityModelLibrary.Models;

namespace DataAccessLibrary.Interfaces
{
    public interface IAddNotes
    {
        Task<string> Notes(string debtorAcct, int employee, string activityCode, string noteText,
            string important, string actionCode, string environment);
    }
}
