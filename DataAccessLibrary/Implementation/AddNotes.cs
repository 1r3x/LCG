﻿using System;
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
        public AddNotes(DbContextForTest dbContext)
        {
            _dbContext = dbContext;
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
