
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Threading.RateLimiting;
using SecureWebApp.Entities;
using Microsoft.AspNetCore.Identity;
using SecureWebApp.Data;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DevConnection"));
});



builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("UserOnly", policy => policy.RequireRole("User"));
});

builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.User.RequireUniqueEmail = true;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 4; // 4 For simplicity, should be at least 12 characters and a combination of uppercase, lowercase, number and symbold.
})
.AddEntityFrameworkStores<DataContext>();

// Rate limiting to prevent DDoS.
// Restricts number of request a user or bot can send to a server within a specific timeframe.
// Effective against single source.
builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.User.Identity?.Name ?? httpContext.Request.Headers.Host.ToString(),
            factory: partition => new FixedWindowRateLimiterOptions
            {
                AutoReplenishment = true,
                PermitLimit = 10,
                QueueLimit = 0,
                Window = TimeSpan.FromMinutes(1)
            }));
});





builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddControllers();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            RoleClaimType = "role",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("tokenSettings:tokenKey").Value!))
        };
    });


builder.Services.AddHsts(options =>
{
    options.MaxAge = TimeSpan.FromDays(365);
    options.IncludeSubDomains = true;
    options.Preload = true;
});

var app = builder.Build();


// Seeds roles.
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    await RoleSeed.SeedRolesAsync(roleManager);
}


if (app.Environment.IsDevelopment())
{
    app.UseHsts();
}

new CookieOptions
{
    HttpOnly = true,
    Secure = true,
    SameSite = SameSiteMode.Strict,
    Expires = DateTime.UtcNow.AddHours(1)
};

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.Use(async (context, next) =>
{
    context.Response.Headers.Append("Content-Security-Policy",
        "default-src 'self'; script-src 'self' https://trustedscripts.example.com; style-src 'self' 'unsafe-inline'; img-src 'self' data:;");
    await next();
});

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
