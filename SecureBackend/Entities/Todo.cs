using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace SecureWebApp.Entities;

public class Todo
{
    public int Id { get; set; }
    public string Task { get; set; } = "";
}
