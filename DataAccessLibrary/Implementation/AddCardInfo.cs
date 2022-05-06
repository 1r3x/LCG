using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLibrary.Interfaces;
using EntityModelLibrary.Models;

namespace DataAccessLibrary.Implementation
{
    public class AddCardInfo:IAddCardInfo
    {
        private readonly DbContextForTest _dbContext;
        private readonly DbContextForProdOld _dbContextProdOld;
        public AddCardInfo(DbContextForTest dbContext, DbContextForProdOld dbContextProdOld)
        {
            _dbContext = dbContext;
            _dbContextProdOld = dbContextProdOld;
        }

        public async Task<string> CreateCardInfo(LcgCardInfo cardObj, string environment)
        {
            try
            {
                if (environment == "T")
                {
                    await _dbContext.LcgCardInfos.AddAsync(cardObj);
                    await _dbContext.SaveChangesAsync();
                }
                else if (environment == "PO")
                {
                    await _dbContextProdOld.LcgCardInfos.AddAsync(cardObj);
                    await _dbContextProdOld.SaveChangesAsync();
                }
                else
                {
                    await _dbContext.LcgCardInfos.AddAsync(cardObj);
                    await _dbContext.SaveChangesAsync();
                }

            }
            catch (Exception e)
            {
                return e.Message;
            }

            return "CardInfo added successfully";
        }
    }
}
