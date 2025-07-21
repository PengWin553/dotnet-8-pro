using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dto.Account;
using api.Interfaces;
using api.Models;
using api.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<AppUser> _signInManager; // Handles user sign-in and authenticati

        public AccountController(UserManager<AppUser> userManager, ITokenService tokenService, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
        }

        [HttpPost("login")] 
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            // Validate the request model (e.g., check for missing username/password)
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Fetch the user from the database (case-insensitive search)
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == loginDto.UserName.ToLower());

            // Reject if user doesn't exist
            if (user == null)
                return Unauthorized("Invalid username");

            // Verify the password
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            // Reject if password is invalid
            if (!result.Succeeded)
                return Unauthorized("Username not found and/or password incorrect.");

            // Return user details + JWT token on success
            return Ok(
                new NewUserDto
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    Token = _tokenService.CreateToken(user)
                }
            );
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
                        return Ok(
                            new NewUserDto
                            {
                                UserName = appUser.UserName,
                                Email = appUser.Email,
                                Token = _tokenService.CreateToken(appUser)
                            }
                        );
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