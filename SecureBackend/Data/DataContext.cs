using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SecureWebApp.Entities;

public class DataContext(DbContextOptions options) : IdentityDbContext<User>(options)
{
    public DbSet<Todo> Todo { get; set; }
}
