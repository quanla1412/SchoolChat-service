namespace SchoolChat.Service.Models;

public class ChatRoomUser
{
    public string Id { get; set; }
    public string ChatRoomId { get; set; }
    public User User { get; set; }
    public bool IsAccepted { get; set; }
}