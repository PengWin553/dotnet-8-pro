using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using api.Interfaces;
using api.Models;
using Microsoft.IdentityModel.Tokens;

namespace api.Service
{
    /// Service responsible for JWT token generation and management
    /// Implements ITokenService interface
    public class TokenService : ITokenService
    {
        // Configuration object to access appsettings.json values
        private readonly IConfiguration _config;

        // Symmetric security key used for signing the JWT tokens
        // Symmetric means the same key is used for both signing and validation
        private readonly SymmetricSecurityKey _key;

        /// Constructor for TokenService
        public TokenService(IConfiguration config)
        {
            _config = config;
            // Initialize the security key using the signing key from configuration
            // TODO: In production, use environment variables for the JWT key (e.g., Azure Key Vault).
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:SigningKey"])); // // The key is encoded as UTF8 bytes as required by SymmetricSecurityKey
        }

        /// Creates a JWT token for the specified user
        public string CreateToken(AppUser user) // AppUser object containing user details
        {
            // Create claims for the token. Claims represent attributes of the user
            // that will be embedded in the token payload
            var claims = new List<Claim>
            {
                // Add user's email as a claim using standard JWT claim name
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                // Add username as a claim using standard JWT claim name
                new Claim(JwtRegisteredClaimNames.GivenName, user.UserName)

                /// Consideration: Add a jti Claim (For Learning Purposes)
                //new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // Unique token ID for revocation  
            };

            // Create signing credentials using the security key
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature); // HmacSha512Signature is a strong hashing algorithm for token signing

            // Configure the token descriptor which defines token properties
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                // Set the claims identity containing our user claims
                Subject = new ClaimsIdentity(claims),

                // Set token expiration (7 days from now)
                // Shorten for production (e.g., 15 mins + refresh tokens) 
                Expires = DateTime.Now.AddDays(7), 

                // Set the signing credentials
                SigningCredentials = creds,

                // Set the issuer from configuration
                Issuer = _config["JWT:Issuer"],

                // Set the intended audience from configuration
                Audience = _config["JWT:Audience"]
            };

            // Create a token handler which will be used to generate the token
            var tokenHandler = new JwtSecurityTokenHandler();

            // Generate the JWT security token based on the descriptor
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token); // JWT token as a string with the WriteToken() method
        }
    }
}