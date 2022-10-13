using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLibrary.Interfaces;
using EntityModelLibrary.Models;

namespace DataAccessLibrary.Implementation
{
    public class AddNotes:IAddNotes
    {
        private readonly DbContextForTest _dbContext;
        private readonly DbContextForProdOld _dbContextProdOld;
        private readonly DbContextForProd _dbContextForProd;
        public AddNotes(DbContextForTest dbContext, DbContextForProdOld dbContextProdOld,
            DbContextForProd dbContextForProd)
        {
            _dbContext = dbContext;
            _dbContextProdOld = dbContextProdOld;
            _dbContextForProd = dbContextForProd;
        }
        public async Task<string> Notes(string debtorAcct, int employee, string activityCode, string noteText, string important,
            string actionCode, string environment)
        {
            try
            {
                if (environment == "T")
                {
                    var datetimeNow = DateTime.Now;
                    var noteMaster = new NoteMaster()
                    {
                       ActionCode = actionCode,
                       ActivityCode = activityCode,
                       DebtorAcct = debtorAcct,
                       Employee = employee,
                       Important = important,
                       NoteDate = datetimeNow.AddSeconds(-datetimeNow.Second).AddMilliseconds(-datetimeNow.Millisecond),
                       NoteText = noteText
                    };
                    await _dbContext.NoteMasters.AddAsync(noteMaster);
                    await _dbContext.SaveChangesAsync();
                }
                else if (environment == "PO")
                {
                    var datetimeNow = DateTime.Now;
                    var noteMaster = new NoteMaster()
                    {
                        ActionCode = actionCode,
                        ActivityCode = activityCode,
                        DebtorAcct = debtorAcct,
                        Employee = employee,
                        Important = important,
                        NoteDate = datetimeNow.AddSeconds(-datetimeNow.Second).AddMilliseconds(-datetimeNow.Millisecond),
                        NoteText = noteText
                    };
                    await _dbContextProdOld.NoteMasters.AddAsync(noteMaster);
                    await _dbContextProdOld.SaveChangesAsync();
                }
                else if (environment == "P")
                {
                    var datetimeNow = DateTime.Now;
                    var noteMaster = new NoteMaster()
                    {
                        ActionCode = actionCode,
                        ActivityCode = activityCode,
                        DebtorAcct = debtorAcct,
                        Employee = employee,
                        Important = important,
                        NoteDate = datetimeNow.AddSeconds(-datetimeNow.Second).AddMilliseconds(-datetimeNow.Millisecond),
                        NoteText = noteText
                    };
                    await _dbContextForProd.NoteMasters.AddAsync(noteMaster);
                    await _dbContextForProd.SaveChangesAsync();
                }
                else
                {
                    var datetimeNow = DateTime.Now;
                    var noteMaster = new NoteMaster()
                    {
                        ActionCode = actionCode,
                        ActivityCode = activityCode,
                        DebtorAcct = debtorAcct,
                        Employee = employee,
                        Important = important,
                        NoteDate = datetimeNow.AddSeconds(-datetimeNow.Second).AddMilliseconds(-datetimeNow.Millisecond),
                        NoteText = noteText
                    };
                    await _dbContext.NoteMasters.AddAsync(noteMaster);
                    await _dbContext.SaveChangesAsync();
                }

            }
            catch (Exception e)
            {
                return e.Message;
            }

            return "Note added successfully";
        }
    }
    
}
