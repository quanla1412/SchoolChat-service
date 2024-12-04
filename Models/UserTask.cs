namespace SchoolChat.Service.Models;

public class UserTask
{
    public string Id { get; set; }
    public string ChatRoomId { get; set; }
    public string CreatorId { get; set; }
    public string Message { get; set; }
    public DateTime Deadline { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Status { get; set; }
}