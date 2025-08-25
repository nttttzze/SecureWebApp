using System.ComponentModel.DataAnnotations;

namespace SecureWebApp.ViewModels;

public class RegisterUserViewModel : LoginViewModel
{
    // [StringLength(50, ErrorMessage = "Max 50 char")]
    // [RegularExpression(@"^[a-öA-Ö\s]+$", ErrorMessage = "Only letters allowed.")]
    // [Required(ErrorMessage = "Username is required")]
    // public string Username { get; set; } = "";




    [Required]
    [Compare("Password", ErrorMessage = "Paswords do not match.")]
    public string ConfirmPassword { get; set; } = "";
}
