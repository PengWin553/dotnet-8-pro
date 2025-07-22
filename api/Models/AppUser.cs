using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace api.Models
{
    // (Identity model - DO NOT add [Table] attribute). Identity automatically manages AspNetUsers table
    public class AppUser : IdentityUser // Inherits from ASP.NET Core Identity's base user clas
    {
        // You can extend with custom fields specific to your application needs

        // Collection Navigation Property
        public List<Portfolio> Portfolios { get; set; } = new List<Portfolio>();
    }
}