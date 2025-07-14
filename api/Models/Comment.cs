using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; } = DateTime.Now;
       
        // Foreign key property linking to the associated Stock
        public int? StockId { get; set; }

        // Navigation property to the associated Stock entity = this comment belongs to one stock
        public Stock? Stock { get; set; }
    }
}