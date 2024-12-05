namespace SchoolChat.Service.Models;

public class Event
{
    public string Id { get; set; }
    public string Name { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string? Description { get; set; }
    public string? ChatRoomId { get; set; }
    public List<EventUser> Users { get; set; }
}