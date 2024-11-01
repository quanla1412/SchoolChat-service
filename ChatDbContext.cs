using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SchoolChat.Service.Models;

namespace SchoolChat.Service;

public class ChatDbContext : IdentityDbContext<User>
{
    public DbSet<ChatRoom> ChatRooms { get; set; }
    public DbSet<ChatRoomUser> ChatRoomUsers { get; set; }

    public ChatDbContext(DbContextOptions<ChatDbContext> options) : base(options)
    {
        
    }
}