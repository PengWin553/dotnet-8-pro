using Microsoft.EntityFrameworkCore;  // reference to Entity Framework
using api.Data; // reference to ApplicationDBContext
using api.Interfaces; // reference to the Interfaces
using api.Repository;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens; // reference to the Repositories

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Makes the database context available throughout the application
builder.Services.AddDbContext<ApplicationDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Add Authentication Service
builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    // Password Security Requirements
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 12;
})
.AddEntityFrameworkStores<ApplicationDBContext>();

// Add Authentication Schemes
builder.Services.AddAuthentication(options =>
{
    // Set JWT Bearer as the default authentication scheme for all operations
    options.DefaultAuthenticateScheme = // Used when authenticating requests
    options.DefaultChallengeScheme = // Used when challenging unauthorized requests
    options.DefaultForbidScheme = // Used when access is forbidden
    options.DefaultScheme = // Default scheme for all operations
    options.DefaultSignInScheme = // Used when signing users in
    options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme; // Used when signing users out
}).AddJwtBearer(options =>
{
    // JWT Token Validation Parameters
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,  // Validate the token issuer
        ValidIssuer = builder.Configuration["JWT:Issuer"], // Expected issuer from config
        ValidateAudience = true, // Validate the token audience
        ValidAudience = builder.Configuration["JWT:Audience"], // Expected audience from config
        ValidateIssuerSigningKey = true, // Validate the signing key
        IssuerSigningKey = new SymmetricSecurityKey(    // Signing key for token verification
            System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JWT:SigningKey"]) 
        )
    };
});

// Registers the dependency injection mapping
builder.Services.AddScoped<IStockRepository, StockRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Enable Authentication Middleware (must come before Authorization)
app.UseAuthentication(); // Identifies the user based on the token
app.UseAuthorization();  // Determines if the user has permission to access resources

app.MapControllers();

app.Run();