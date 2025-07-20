using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Helpers
{
    public class QueryObject
    {
        // For Filtering
        public string? Symbol { get; set; } = null;
        public string? CompanyName { get; set; } = null;

        // For Soring
        public string? SortBy { get; set; } = null;
        public bool IsDescending { get; set; } = false;

    }
}