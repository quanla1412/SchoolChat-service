namespace SchoolChat.Service.Models;

public class UserTask
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string? ChatRoomId { get; set; }
    public string CreatorId { get; set; }
    public string Description { get; set; }
    public DateTime Deadline { get; set; }
    public DateTime CreatedDate { get; set; }
    public bool IsCompleted { get; set; }
}