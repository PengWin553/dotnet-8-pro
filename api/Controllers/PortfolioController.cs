
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Extensions;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/portfolio")]
    [ApiController]
    public class PortfolioController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IStockRepository _stockRepo;
        private readonly IPortfolioRepository _portfolioRepo;
        public PortfolioController(UserManager<AppUser> userManager,
        IStockRepository stockRepo, IPortfolioRepository portfolioRepo)
        {
            _userManager = userManager;
            _stockRepo = stockRepo;
            _portfolioRepo = portfolioRepo;
        }

        [HttpGet]
        [Authorize] // First layer: Only authenticated users can access
        public async Task<IActionResult> GetUserPortfolio()
        { 
            var username = User.GetUsername(); // Second layer: Get the current user's username from JWT token; This line is calling the extension method from ClaimsExtensions.cs
            var appUser = await _userManager.FindByNameAsync(username);  // Third layer: Look up the specific user in the database
            var userPortfolio = await _portfolioRepo.GetUserPortfolio(appUser); // Fourth layer: Only get portfolios belonging to THIS user
            return Ok(userPortfolio);
        }
    }
}