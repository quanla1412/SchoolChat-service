namespace SchoolChat.Service.ViewModel;

public class ChatRoomViewModel
{
    public string Id { get; set; }
    public string? Name { get; set; }
    public string? Avatar { get; set; }
    public bool IsSingle {get; set;}
    public List<UserViewModel> Users { get; set; }
    public MessageViewModel? NewestMessage { get; set; }
}