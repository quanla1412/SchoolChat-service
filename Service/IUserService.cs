using SchoolChat.Service.ViewModel;

namespace SchoolChat.Service.Service;

public interface IUserService
{
    UserViewModel? GetUserById(string id);
    
    List<UserViewModel> GetUsers(string searchString);
}