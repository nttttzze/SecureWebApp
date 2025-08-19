using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using SecureWebApp.Entities;
using SecureWebApp.ViewModels;

namespace SecureWebApp.Controllers;

[Authorize(Roles = "Admin")]
[ApiController]
[Route("api/[controller]")]
public class AdminController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager) : ControllerBase
{
    private readonly UserManager<User> _userManager = userManager;
    private readonly RoleManager<IdentityRole> _roleManager = roleManager;

    [HttpPost("makeAdmin/{userName}")]
    public async Task<IActionResult> MakeUserAdmin(string userName)
    {
        var user = await _userManager.FindByNameAsync(userName);
        if (user == null) return NotFound("User not found");

        if (!await _roleManager.RoleExistsAsync("Admin"))
            await _roleManager.CreateAsync(new IdentityRole("Admin"));

        var result = await _userManager.AddToRoleAsync(user, "Admin");

        if (result.Succeeded) return Ok("User is now Admin");

        return BadRequest(result.Errors);
    }
}

