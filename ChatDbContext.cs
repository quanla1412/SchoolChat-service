using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SchoolChat.Service.Models;

namespace SchoolChat.Service;

public class ChatDbContext : IdentityDbContext<User>
{
    public DbSet<ChatRoom> ChatRooms { get; set; }
    public DbSet<ChatRoomUser> ChatRoomUsers { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<DeleteMessageUser> DeleteMessageUsers { get; set; }
    public DbSet<ReadMessageStatus> ReadMessageStatuses { get; set; }
    public DbSet<UserTask> UserTasks { get; set; }
    public DbSet<TaskAssignee> TaskAssignees { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<EventUser> EventUsers { get; set; }

    public ChatDbContext(DbContextOptions<ChatDbContext> options) : base(options)
    {
        
    }
}