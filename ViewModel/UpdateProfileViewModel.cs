namespace SchoolChat.Service.ViewModel;

public class UpdateProfileViewModel
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public DateTime? Birthday { get; set; }
    public string? Gender { get; set; }
    public string? Phone { get; set; }
    public List<IFormFile>? AvatarFiles { get; set; }
}