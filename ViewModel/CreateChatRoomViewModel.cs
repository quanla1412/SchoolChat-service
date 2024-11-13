namespace SchoolChat.Service.ViewModel;

public class CreateChatRoomViewModel
{
    public string? Name { get; set; }
    public string? FromUserId { get; set; }
    public List<string> ToUserIds { get; set; }
}