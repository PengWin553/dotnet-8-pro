using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models; // add this manually

namespace api.Interfaces
{
    public interface IStockRepository
    {
        Task<List<Stock>> GetAllAsync();
    }
}