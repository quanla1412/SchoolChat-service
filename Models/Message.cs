namespace SchoolChat.Service.Models;

public class Message
{
    public string Id { get; set; }
    public string ChatRoomId { get; set; }
    public string FromUserId { get; set; }
    public string Text { get; set; }
    public DateTime SentDate { get; set; }
    public List<ReadMessageStatus> ReadStatuses { get; set; }
    public Boolean IsForwarded { get; set; }
    public Boolean IsPinned { get; set; }
    public Boolean IsUnsent { get; set; }
}