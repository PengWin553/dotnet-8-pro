using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    // Represents the many-to-many join table between AppUser and Stock
    [Table("Portfolios")]  // Explicitly names the table "Portfolios"
    public class Portfolio
    {
        // Composite key components (foreign keys)
        public string AppUserId { get; set; } // Links to AspNetUsers.Id
        public int StockId { get; set; } // Links to Stocks.Id
        
        // Navigation properties (optional but recommended for EF Core query convenience)
        public AppUser AppUser { get; set; } // Reference to the associated user
        public Stock Stock { get; set; } // Reference to the associated user
    }
}