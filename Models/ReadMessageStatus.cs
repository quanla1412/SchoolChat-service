namespace SchoolChat.Service.Models;

public class ReadMessageStatus
{
    public string Id { get; set; }
    public string MessageId { get; set; }
    public string UserId { get; set; }
    public DateTime ReadDate { get; set; }
}