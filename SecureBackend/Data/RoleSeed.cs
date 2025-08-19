using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace SecureWebApp.Data
{
    public static class RoleSeed
    {
        public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            if (!await roleManager.RoleExistsAsync("User"))
            {
                await roleManager.CreateAsync(new IdentityRole("User"));
            }
        }
    }
}