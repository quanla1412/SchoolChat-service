namespace SchoolChat.Service.ViewModel;

public class MessageViewModel
{
    public string Id { get; set; }
    public string ChatRoomId { get; set; }
    public ShortUserViewModel FromUser { get; set; }
    public string Text { get; set; }
    public DateTime SentDate { get; set; }
    public bool IsPinned { get; set; }
    public bool IsUnsent { get; set; }
    public Boolean IsForwarded { get; set; }
    public string? Type { get; set; }
    public List<ReadMessageStatusViewModel> ReadStatuses { get; set; }
}