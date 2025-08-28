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
public class AuthController(UserManager<User> userManager, SignInManager<User> signInManager) : ControllerBase
{
    private readonly HtmlSanitizer _htmlSanitizer = new();
    private readonly UserManager<User> _userManager = userManager;


    // Remove test comments later
    [Authorize(Policy = "AdminOnly")]
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
                return StatusCode(201, new { success = true, message = "User registered succesfully" });
            }
            return BadRequest(new { success = false, message = result.Errors });
        }
        catch (Exception ex)
        {
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    [HttpPost("badlogin")]
    public ActionResult BadLogin()
    {
        return Ok();
    }
    [HttpPost("logout")]
    public async Task<ActionResult> Logout()
    {
        await signInManager.SignOutAsync();
        return NoContent();
    }

}

