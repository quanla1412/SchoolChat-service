namespace SchoolChat.Service.Models;

public class ChatRoom
{
    public string Id { get; set; }
    public string? Name { get; set; }
    public List<ChatRoomUser> Users { get; set; }
}