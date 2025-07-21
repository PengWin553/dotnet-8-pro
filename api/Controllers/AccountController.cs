using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dto.Account;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        public AccountController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                // Validate the request model using DataAnnotations (e.g., [Required], [EmailAddress])
                if (!ModelState.IsValid)
                    return BadRequest(ModelState); // Return validation errors if any

                // Create a new AppUser object with the provided username and email
                var appUser = new AppUser
                {
                    UserName = registerDto.UserName,
                    Email = registerDto.Email
                };

                // Create the user with the provided password (automatically hashed by Identity)
                var createdUser = await _userManager.CreateAsync(appUser, registerDto.Password);

                if (createdUser.Succeeded)
                {
                    // If user creation succeeds, assign the default "User" role
                    var roleResult = await _userManager.AddToRoleAsync(appUser, "User");

                    if (roleResult.Succeeded)
                    {
                        return Ok("User created");
                    }
                    else
                    {
                        // Return role assignment errors (e.g., role doesn't exist)
                        return StatusCode(500, roleResult.Errors);
                    }
                }
                else
                {
                    // Return user creation errors (e.g., duplicate email, weak password)
                    return StatusCode(500, createdUser.Errors);
                }
            }
            catch (Exception e)
            {
                // Catch unexpected exceptions (e.g., database connection issues)
                return StatusCode(500, e);
            }
        }
    }
}