using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Ganss.Xss;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using SecureWebApp.Entities;
using SecureWebApp.ViewModels;

namespace SecureWebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(UserManager<User> userManager, IConfiguration config) : ControllerBase
{
    private readonly HtmlSanitizer _htmlSanitizer = new();
    private readonly UserManager<User> _userManager = userManager;
    private readonly IConfiguration _config = config;


    // Remove comment later
    [Authorize(Roles = "Admin")]
    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterUserViewModel model)
    {
        try
        {
            if (!ModelState.IsValid) return ValidationProblem();

            model.UserName = _htmlSanitizer.Sanitize(model.UserName);
            model.Password = _htmlSanitizer.Sanitize(model.Password);
            ModelState.Clear();
            TryValidateModel(model);

            if (!ModelState.IsValid) return ValidationProblem();

            var user = new User
            {
                //userName has to be an Email
                UserName = model.UserName,
                Email = model.UserName,

            };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                // Gives new user the "User" role on registration.
                await _userManager.AddToRoleAsync(user, "User");
                return StatusCode(201, "User registered succesfully");
            }
            return BadRequest(new { success = false, message = result.Errors });
        }
        catch (Exception ex)
        {
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    [HttpPost("login")]
    // ------------------------------------------
    //    "username": "test@gmail.com", (Admin)
    //   "password": "testPassword",
    // ------------------------------------------
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);

            }
            var user = await _userManager.FindByNameAsync(model.UserName);

            if (user is null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                return Unauthorized(new { success = false, message = "Unauthorized" });
            }
            return Ok(new { success = true, user.UserName, token = CreateToken(user) });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = ex.Message });
        }



    }

    /* TOKEN SERVICE... */
    private string CreateToken(User user)
    {


        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.UserName!),
        };



        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["tokenSettings:tokenKey"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

        var options = new JwtSecurityToken(
            issuer: null,
            audience: null,
            claims: claims,
            expires: DateTime.Now.AddDays(10),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(options);
    }

}

