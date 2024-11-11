using Microsoft.AspNetCore.Identity;

namespace SchoolChat.Service.Models;

public class User : IdentityUser
{
    public string? Name { get; set; }
    public string? Gender { get; set; }
    public DateTime? Birthday { get; set; }
}