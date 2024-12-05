namespace SchoolChat.Service.ViewModel;

public class CreateEventViewModel
{
    public string Name { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string? Description { get; set; }
    public string? ChatRoomId { get; set; }
}