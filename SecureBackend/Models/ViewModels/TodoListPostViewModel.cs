using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SecureWebApp.ViewModels;

public class TodoListPostViewModel
{
    [Required]
    [MaxLength(50)]

    public string Task { get; set; } = "";
}
