using SchoolChat.Service.ViewModel;

namespace SchoolChat.Service.Service;

public interface IUserService
{
    UserViewModel? GetUserById(string id);
    ShortUserViewModel? GetShortUserById(string id);
    
    List<UserViewModel> GetUsers(string searchString, string? excludeUserId, string exceptChatRoomId);
    
    List<string> GetUserIdsByChatRoom(string chatRoomId);
  
    UpdateProfileViewModel UpdateProfile(UpdateProfileViewModel model);
}