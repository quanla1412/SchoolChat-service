using Microsoft.IdentityModel.Tokens;
using SchoolChat.Service.Models;

namespace SchoolChat.Service.Repository.RepositoryImpl;

public class UserRepositoryImpl(ChatDbContext context) : IUserRepository
{
    public User? GetUserById(string id)
    {
        return context.Users.SingleOrDefault(u => u.Id == id);
    }

    public List<User> GetUsers(string searchString, string? excludeUserId, string exceptChatRoomId)
    {
        var query = context.Users.Where(user => user.Email.Contains(searchString) || user.Name.Contains(searchString));
        if (excludeUserId != null)
        {
            query = query.Where(user => user.Id != excludeUserId);
        }

        if (!exceptChatRoomId.IsNullOrEmpty())
        {
            List<string> userIdInChatroom = context.ChatRoomUsers.Where(chatRoomUser => chatRoomUser.ChatRoomId == exceptChatRoomId).Select(user => user.User.Id).ToList();
            query = query.Where(user => !userIdInChatroom.Contains(user.Id));
        }
        return query.ToList();
    }

    public List<User> GetUsersByChatRoomId(string chatRoomId)
    {   var userIds = context.ChatRoomUsers
            .Where(chatRoomUser => chatRoomUser.ChatRoomId == chatRoomId)
            .Select(chatRoomUser => chatRoomUser.User.Id)
            .ToList();

        var result = context.Users
            .Where(user => userIds.Contains(user.Id))
            .ToList();

        return result;
    }

    public User UpdateProfile(User user)
    {
        var existingUser = context.Users.Find(user.Id);
        if (existingUser == null)
            throw new KeyNotFoundException("User not found.");

        existingUser.Name = user.Name;
        existingUser.Birthday = user.Birthday;
        existingUser.Gender = user.Gender;
        existingUser.PhoneNumber = user.PhoneNumber;
        
        context.Users.Update(existingUser);
        context.SaveChanges();

        return existingUser;
    }
}