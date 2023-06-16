using Microsoft.AspNetCore.Identity;

namespace Goba_Store.Models;

public class AppUser : IdentityUser
{
    public string FullName { get; set; }
}