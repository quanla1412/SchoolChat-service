using SchoolChat.Service.ViewModel;

namespace SchoolChat.Service.Models;

public class MessageViewModel
{
    public string Id { get; set; }
    public string ChatRoomId { get; set; }
    public string FromUserId { get; set; }
    public string Text { get; set; }
    public DateTime SentDate { get; set; }
    public bool IsPinned { get; set; }
    public bool IsUnsent { get; set; }
    public List<ReadMessageStatusViewModel> ReadStatuses { get; set; }
}