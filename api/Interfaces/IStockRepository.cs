using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models; // add this manually
using api.Dto.Stock; // add this manully

namespace api.Interfaces
{
    public interface IStockRepository
    {
        Task<List<Stock>> GetAllAsync();
        Task<Stock?> GetByIdAsync(int id); //FirstOrDefault can be a null
        Task<Stock> CreateAsync(Stock stockModel);
        Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto stockDto);
        Task<Stock?> DeleteAsync(int id);

        // Stock Finder
        Task<bool> StockExists(int id);
    }
}