using SchoolChat.Service.Models;

namespace SchoolChat.Service.ViewModel;

public class ChatRoomDetailViewModel
{
    public string Id { get; set; }
    public string? Name { get; set; }
    public string? Avatar { get; set; }
    public bool IsAccepted { get; set; }
    public PinnedMessageViewModel? PinnedMessage { get; set; }
    public bool IsSingle { get; set; }
    public List<ShortUserViewModel> Users { get; set; }
    public List<UserTaskViewModel> Tasks { get; set; }
}