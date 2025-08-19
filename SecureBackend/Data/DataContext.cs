using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SecureWebApp.Entities;

public class DataContext : IdentityDbContext<User>
{
    public DbSet<Todo> Todo { get; set; }



    public DataContext(DbContextOptions options) : base(options)
    {

    }
}
