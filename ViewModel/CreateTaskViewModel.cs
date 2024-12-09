namespace SchoolChat.Service.ViewModel;

public class CreateTaskViewModel
{
    public string? ChatRoomId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime Deadline { get; set; }
    public List<string> TaskAssigneeIds { get; set; }
}