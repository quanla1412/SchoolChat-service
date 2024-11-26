using SchoolChat.Service.ViewModel;

namespace SchoolChat.Service.Models;

public class PinnedMessageViewModel
{
    public string Id { get; set; }
    public string ChatRoomId { get; set; }
    public ShortUserViewModel FromUser { get; set; }
    public string Text { get; set; }
}