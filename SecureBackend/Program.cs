
using Microsoft.EntityFrameworkCore;
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
    options.AddPolicy("UserAndAdmin", policy => policy.RequireRole("User", "Admin"));
});

// builder.Services.AddIdentity<User, IdentityRole>(options =>
// {
// options.User.RequireUniqueEmail = true;
// options.Password.RequireDigit = false;
// options.Password.RequireLowercase = false;
// options.Password.RequireUppercase = false;
// options.Password.RequireNonAlphanumeric = false;
// options.Password.RequiredLength = 4; // 4 For simplicity, should be at least 12 characters and a combination of uppercase, lowercase, number and symbold.
// })
// .AddEntityFrameworkStores<DataContext>();



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






builder.Services.AddCors();

builder.Services.AddIdentityApiEndpoints<User>(options =>
{
    options.User.RequireUniqueEmail = true;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<DataContext>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.None;  // för cross-origin
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // HTTPS
});

builder.Services.AddControllers();

// builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//     .AddJwtBearer(options =>
//     {
//         options.TokenValidationParameters = new TokenValidationParameters
//         {
//             ValidateIssuer = false,
//             ValidateAudience = false,
//             ValidateLifetime = true,
//             ValidateIssuerSigningKey = true,
//             RoleClaimType = "role",
//             IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("tokenSettings:tokenKey").Value!))
//         };
//     }); Används för JWT


builder.Services.AddHsts(options =>
{
    options.MaxAge = TimeSpan.FromDays(365);
    options.IncludeSubDomains = true;
    options.Preload = true;
});

var app = builder.Build();

app.UseCors(c => c
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials()
    .WithOrigins("http://localhost:5501", "http://127.0.0.1:5501")
);


// Seeds roles.
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = services.GetRequiredService<UserManager<User>>();

        await RoleSeed.SeedRolesAsync(roleManager);
        await AdminSeed.SeedUser(userManager);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Something went wrong on seeding");
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseHsts();
}

// new CookieOptions
// {
//     HttpOnly = true,
//     Secure = true,
//     SameSite = SameSiteMode.Strict,
//     Expires = DateTime.UtcNow.AddHours(1)

// };

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

app.MapGroup("api").MapIdentityApi<User>();




app.Run();
