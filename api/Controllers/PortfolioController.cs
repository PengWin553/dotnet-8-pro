
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
            var appUser = await _userManager.FindByNameAsync(username);  // Third layer: Find and get the specific user in the database
            var userPortfolio = await _portfolioRepo.GetUserPortfolio(appUser); // Fourth layer: Only get portfolios belonging to THIS user
            return Ok(userPortfolio);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddPortfolio(string symbol)
        {
            // Get the current user's username from JWT token by calling the extension method from ClaimsExtensions.cs
            var username = User.GetUsername(); 

            // Find and get the specific authenticated user entity from the database using UserManager
            var appUser = await _userManager.FindByNameAsync(username);

            // Find and get the specific stock from the database by calling the GetBySymbolAsync(symbol) method of the StockRepository
            var stock = await _stockRepo.GetBySymbolAsync(symbol);

            // Display error message and return bad request response if stock is not found in the database
            if (stock == null)
                return BadRequest("Stock not found");

            // Get all portfolio entries that belong specifically to THIS authenticated user by calling the GetUserPortfolio method from the PortfolioRepository
            var userPortfolio = await _portfolioRepo.GetUserPortfolio(appUser);  // Retrieve all portfolio entries belonging to the current authenticated user

            // Display error message and prevent duplicate entries if the user tries to add an existing stock symbol to their portfolio
            // This performs a case-insensitive comparison to check for duplicates
            if (userPortfolio.Any(e => e.Symbol.ToLower() == symbol.ToLower())) return BadRequest("Cannot add same stock to portfolio");

            // Create a new Portfolio object that contains the required properties (fields) to be stored in the database when adding a stock to user's portfolio
            var portfolioModel = new Portfolio
            {
                StockId = stock.Id, // Store the stock's unique identifier as foreign key
                AppUserId = appUser.Id // Store the user's unique identifier as foreign key
            };

            // Save the new portfolio entry to the database by calling the CreateAsync() method and passing the portfolioModel to the PortfolioRepository
            await _portfolioRepo.CreateAsync(portfolioModel);

            // Display error message and return server error response if portfolio creation fails
            if (portfolioModel == null)
            {
                return StatusCode(500, "Could not create");
            }
            else
            {
                // Display success by returning Created status response indicating the stock was successfully added to user's portfolio
                return Created();
            }
        }
    }
}