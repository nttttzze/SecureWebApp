
using System.ComponentModel.DataAnnotations;

namespace SecureWebApp.ViewModels;

public class LoginViewModel
{
    [StringLength(50, ErrorMessage = "Max 50 char")]
    [Required(ErrorMessage = "Username is required")]
    public string UserName { get; set; } = "";

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; } = "";
}
