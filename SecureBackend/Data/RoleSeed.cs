using Microsoft.AspNetCore.Identity;
using SecureWebApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecureWebApp.Data
{
    public static class AdminSeed
    {
        public static async Task SeedUser(UserManager<User> userManager)
        {
            if (!userManager.Users.Any())
            {
                var users = new List<User>
                {
                    new() {
                        UserName = "test@gmail.com",
                        Email = "test@gmail.com",
                        // EmailConfirmed = true
                    }
                };

                foreach (var user in users)
                {
                    var result = await userManager.CreateAsync(user, "testPassword1@");
                    if (result.Succeeded)
                    {
                        // Lägg i roll "Admin" (om den finns seedad)
                        await userManager.AddToRoleAsync(user, "Admin");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            Console.WriteLine($"Error: {error.Description}");
                        }
                    }
                }
            }
        }
    }

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