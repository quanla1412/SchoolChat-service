using SchoolChat.Service.Models;

namespace SchoolChat.Service.ViewModel;

public class ChatRoomDetailViewModel
{
    public string Id { get; set; }
    public string? Name { get; set; }
    public PinnedMessageViewModel? PinnedMessage { get; set; }
}