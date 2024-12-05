namespace SchoolChat.Service.Models;

public class EventUser
{
    public string Id { get; set; }
    public string EventId { get; set; }
    public User User { get; set; }
}