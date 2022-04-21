using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiAccessLibrary.ApiModels;

namespace ApiAccessLibrary.Interfaces
{
    public  interface IProcessCardAuthorization
    {
        Task<string> PostCardAuthorizationAsync(SaleRequestModel requestModel);
    }
}
