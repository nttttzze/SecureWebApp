using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace SecureWebApp.Entities;

public class User : IdentityUser
{
    // Använder bara Username så jag lämnar denna tom och ärver från IdentityUser

    // public string FirstName { get; set; } = "";
    // public string LastName { get; set; } = "";
}
