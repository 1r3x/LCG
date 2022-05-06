using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityModelLibrary.Models;

namespace DataAccessLibrary.Interfaces
{
    public interface IAddCardInfo
    {
        Task<string> CreateCardInfo(LcgCardInfo cardObj, string environment);
    }
}
