using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;  // Required for Column attribute

namespace api.Models
{
    [Table("Stocks")] // Explicitly names the table "Stocks"
    public class Stock
    {
        public int Id { get; set; }
        public string Symbol { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Purchase { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal LastDiv { get; set; }

        public string Industry { get; set; } = string.Empty;
        public long MarketCap { get; set; }

        // Collection navigation - one stock can have many comments
        public List<Comment> Comments { get; set; } = new List<Comment>();

        // Navigation: All users who own this stock (via Portfolios join table)
        public List<Portfolio> Portfolios { get; set; } = new List<Portfolio>();
    }
}