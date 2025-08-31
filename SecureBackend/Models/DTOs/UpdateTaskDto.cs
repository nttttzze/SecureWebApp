
using System.ComponentModel.DataAnnotations;

namespace SecureWebApp.Models
{
    public class UpdateTaskDto
    {
        [Required]
        [MaxLength(50)]
        [RegularExpression(@"^[a-zA-Z0-9åÅäÄöÖ\s]+$", ErrorMessage = "Only letters allowed and numbers allowed.")]
        public string Task { get; set; } = "";
    }
}
