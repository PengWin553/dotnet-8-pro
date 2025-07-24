using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models; // add this manually
using api.Dto.Stock;
using api.Helpers; // add this manully

namespace api.Interfaces
{
    public interface IStockRepository
    {
        Task<List<Stock>> GetAllAsync(QueryObject query);
        Task<Stock?> GetByIdAsync(int id); //FirstOrDefault can be a null
        Task<Stock> CreateAsync(Stock stockModel);
        Task<Stock?> UpdateAsync(int id, Stock stockModel);
        Task<Stock?> DeleteAsync(int id);

        // Get stock by symbol
        Task<Stock?> GetBySymbolAsync(string symbol);

        // Stock Finder
        Task<bool> StockExists(int id);
    }
}