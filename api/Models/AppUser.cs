using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace api.Models
{
    public class AppUser : IdentityUser // Inherits all default Identity properties
    {
        // You can extend with custom fields specific to your application needs
    }
}