using SchoolChat.Service.ViewModel;

namespace SchoolChat.Service.Service;

public interface IUserService
{
    UserViewModel? GetUserById(string id);
    
    List<UserViewModel> GetUsers(string searchString, string? excludeUserId);
    
    List<string> GetUserIdsByChatRoom(string chatRoomId);
  
    UpdateProfileViewModel UpdateProfile(UpdateProfileViewModel model);
}