namespace SchoolChat.Service.ViewModel;

public class CreateTaskViewModel
{
    public string ChatRoomId { get; set; }
    public string CreatorId { get; set; }
    public string Message { get; set; }
    public DateTime Deadline { get; set; }
    public string Status { get; set; }
    public List<string> TaskAssignees { get; set; }
}