using SchoolChat.Service.Models;

namespace SchoolChat.Service.Repository;

public interface IUserRepository
{
    User? GetUserById(string id);
  
    List<User> GetUsers(string searchString, string? excludeUserId);
  
    List<User> GetUsersByChatRoomId(string chatRoomId);
  
    User UpdateProfile(User user);
}