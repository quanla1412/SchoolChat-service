namespace SchoolChat.Service.ViewModel;

public class UpdateChatRoomVIewModel
{
    public string Id { get; set; }
    public string? Name { get; set; }
    public List<IFormFile>? Avatar { get; set; }
}