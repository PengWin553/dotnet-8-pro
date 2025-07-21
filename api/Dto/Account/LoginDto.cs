using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dto.Account
{
    public class LoginDto
    {
        [Required] // Username is a required field
        public string UserName { get; set; }

        [Required] // Password is a required field
        public string Password { get; set; }
    }
}