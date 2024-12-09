namespace SchoolChat.Service.ViewModel;

public class UserTaskViewModel
{
    public string Id { get; set; }
    public string? ChatRoomId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime Deadline { get; set; }
    public string CreatorId { get; set; }
    public bool IsCompleted { get; set; }
    public string? Type { get; set; }
    public ShortUserViewModel? Creator { get; set;}
    public List<UserViewModel>? TaskAssignees { get; set; }
}