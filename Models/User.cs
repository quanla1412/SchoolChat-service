using Microsoft.AspNetCore.Identity;

namespace SchoolChat.Service.Models;

public class User : IdentityUser
{
    public string? Name { get; set; }
}