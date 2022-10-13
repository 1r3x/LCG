using ApiAccessLibrary.ApiModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiAccessLibrary.Interfaces
{
    public interface IiProGatewayServices
    {
        Task<string> PostIProGateway();
    }
}
